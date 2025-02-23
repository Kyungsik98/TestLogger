using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
// Ensure PumpService is in scope
using static MRPumpService; 
using static StorageHeaterService;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {   
        bool isValidInput = false;
        var service = new StorageHeaterService();

        // isValidInput값이 이상하면 계속 반복
        do
        {
            var SelectAction = "";
            Console.WriteLine("Select Action: Robo-I/IS HeaterLoggig = 1, Robo-MR Action = 2");
            SelectAction = Console.ReadLine();
            
            switch(SelectAction)
            {
                case "1":
                    isValidInput = true;
                    await service.CreateCsvFileAsync();
                    while (true)
                    {
                        await service.LogStatusToCsvAsync();
                        await Task.Delay(1000); // Wait for 10 seconds
                    }
                case "2":
                    isValidInput = true;
                    //await GetStatusAsync();
                    await GetMainStatus();
                    break;
                default:
                    Console.WriteLine("잘못된 입력력");
                    break;
            }
        }while(!isValidInput);
        
        // await GetSerialPortStatusAsync();
        // await ChangeSerialPortAsync();
        // await GetStatusAsync();
        // await GetHeaterInfosAsync();
        // await AddAllHeaterInfosAsync();
        // await DeleteAllHeaterInfosAsync();
        // await GetIsHeatingAsync();
        // await GetCurrentTemperatureAsync();
        // await SetTargetTemperatureAsync();
        // await StartHeaterAsync();
        // await StopHeaterAsync();
        // await StopHeaterAllAsync();
        // await GetConnectionInformationAsync(0);  // Example usage
       
    }

    #region Serial Port Methods

    private static async Task GetSerialPortStatusAsync()
    {
        var response = await client.GetAsync("http://localhost:5000/heater/config/port");
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task ChangeSerialPortAsync()
    {
        var serialPortRequest = new
        {
            PortName = "COM3",
            BaudRate = 9600,
            StopBits = 1
        };
        var content = new StringContent(JsonConvert.SerializeObject(serialPortRequest), Encoding.UTF8, "application/json");
        var response = await client.PutAsync("http://localhost:5000/heater/config/port?serialId=1", content);
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    #endregion

    #region Heater Status & Configuration Methods

    private static async Task GetStatusAsync()
    {
        var response = await client.GetAsync("http://localhost:5000/heater/status");
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task GetMainStatus()
    {
        var response = await client.GetAsync("http://localhost:5000/main/status");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task GetHeaterInfosAsync()
    {
        var response = await client.GetAsync("http://localhost:5000/heater/config/heater-infos");
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task AddAllHeaterInfosAsync()
    {
        var heaterInfos = new[]
        {
            new { Id = 1, Name = "Heater1" },
            new { Id = 2, Name = "Heater2" }
        };
        var content = new StringContent(JsonConvert.SerializeObject(heaterInfos), Encoding.UTF8, "application/json");
        var response = await client.PutAsync("http://localhost:5000/heater/config/heater-Infos", content);
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task DeleteAllHeaterInfosAsync()
    {
        var response = await client.DeleteAsync("http://localhost:5000/heater/config/heater-Infos");
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    #endregion

    #region Heater Command Methods

    private static async Task GetIsHeatingAsync()
    {
        var response = await client.GetAsync("http://localhost:5000/heater/command/is-heating?heater=1");
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task GetCurrentTemperatureAsync()
    {
        var response = await client.GetAsync("http://localhost:5000/heater/command/current-temperature?heater=1");
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task SetTargetTemperatureAsync()
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var response = await client.PutAsync("http://localhost:5000/heater/command/target-temperature?heater=1&targetTemperatureInC=100", content);
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task StartHeaterAsync()
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var response = await client.PutAsync("http://localhost:5000/heater/command/start-heater?heater=1", content);
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task StopHeaterAsync()
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var response = await client.PutAsync("http://localhost:5000/heater/command/stop-heater?heater=1", content);
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    private static async Task StopHeaterAllAsync()
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var response = await client.PutAsync("http://localhost:5000/heater/command/stop-heater-all", content);
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    #endregion
}