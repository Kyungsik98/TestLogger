using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public static class MRPumpService
{
    private static readonly HttpClient Client = new HttpClient();

    public static async Task GetConnectionInformationAsync(int pumpDriver)
    {
        var response = await Client.GetAsync($"http://localhost:5000/pump/config/port?pumpDriver={pumpDriver}");
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    public static async Task ChangeSerialPortAsync(int pumpDriver)
    {
        var requestBody = new
        {
            IPAddress = "192.168.0.100",
            PortNumber = 12345
        };
        var response = await Client.PutAsJsonAsync(
            $"http://localhost:5000/pump/config/port?pumpDriver={pumpDriver}",
            requestBody
        );
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    public static async Task GetPumpStatusAsync()
    {
        var response = await Client.GetAsync("http://localhost:5000/pump/status");
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    public static async Task ConnectAsync(int pumpDriver)
    {
        var response = await Client.PutAsync(
            $"http://localhost:5000/pump/connect?pumpDriver={pumpDriver}",
            null
        );
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    public static async Task DisconnectAsync(int pumpDriver)
    {
        var response = await Client.PutAsync(
            $"http://localhost:5000/pump/disconnect?pumpDriver={pumpDriver}",
            null
        );
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    public static async Task StartPumpAsync(int pumpDriver)
    {
        var response = await Client.PutAsync(
            $"http://localhost:5000/pump/start?pumpDriver={pumpDriver}",
            null
        );
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    public static async Task StopPumpAsync(int pumpDriver)
    {
        var response = await Client.PutAsync(
            $"http://localhost:5000/pump/stop?pumpDriver={pumpDriver}",
            null
        );
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}
