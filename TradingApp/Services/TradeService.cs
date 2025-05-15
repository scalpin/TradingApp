using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TradingApp.Constants;
using TradingApp.Services;
using TradingApp.Models;
using System.Net.Http.Json;
using Grpc.Net.Client;
using System.Collections.Generic;
using System.Linq;

public class TradeService
{
    private readonly HttpClient _httpClient;
    private readonly SettingsService _settings;

    public TradeService(SettingsService settingsService)
    {
        _httpClient = new HttpClient();
        _settings = settingsService;
    }

    // Метод для размещения тейк-профит ордера
    public async Task<bool> PlaceTakeProfitOrderAsync(string board, string code, double activationPrice)
    {
        var endpoint = ApiEndpoints.PlaceStopOrder; // Убедись, что этот константный путь определен

        var requestBody = new
        {
            clientId = _settings.ClientId,
            securityBoard = board,
            securityCode = code,
            buySell = "Buy",
            takeProfit = new
            {
                activationPrice = activationPrice,
                marketPrice = true,
                quantity = new { value = 1, units = "Percent" },
                time = 0,
                useCredit = false
            },
            stopLoss = (object)null,
            validBefore = new
            {
                type = "TillEndSession",
                time = DateTime.UtcNow.ToString("o")
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

        var response = await _httpClient.PostAsync(endpoint, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return true;

        Console.WriteLine($"Ошибка: {response.StatusCode} - {responseBody}");
        return false;
    }

    /*
    public async Task<OrderBookResponse> GetOrderBookRestAsync(string symbol)
    {
        var url = $"https://trade-api.finam.ru/v1/instruments/{symbol}/orderbook";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Token}");

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Ошибка получения стакана: {response.StatusCode}");
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderBookResponse>(json);
    }
    */
}