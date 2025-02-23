using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

class StorageHeaterService
{
    private static readonly HttpClient client = new HttpClient();
    private const string BaseUrl = "http://localhost:5000";

    public async Task Status()
    {
        var response = await client.GetAsync($"{BaseUrl}/heater/status");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    public async Task MainStatus()
    {
        var response = await client.GetAsync($"{BaseUrl}/main/status");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }
    
    // Heater On API 호출 (히터 모두 켜는 걸로 수정필요요)
    public async Task StartHeater(int heater)
    {
        var response = await client.PutAsync(
            $"{BaseUrl}/heater/on?heater={heater}",
            null
        );
        response.EnsureSuccessStatusCode();
    }

    // Heater Status Logging
    public async Task SaveStatusToCsvAsync()
    {
        await CreateCsvFileAsync();
        await LogStatusToCsvAsync();
    }

    public async Task CreateCsvFileAsync()
    {
        string fileName = $"{DateTime.Now:yyyyMMdd}HeaterTemperatureLog.csv";
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        if (!File.Exists(filePath))
        {
            string header = "Timestamp,TemperatureTop,TemperatureBottom,TemperatureLeft,TemperatureRight,TemperatureUserDoor,TemperatureRobotDoorUpper,TemperatureRobotDoorLower,TargetTemperatureTop,TargetTemperatureBottom,TargetTemperatureLeft,TargetTemperatureRight,TargetTemperatureUserDoor,TargetTemperatureRobotDoorUpper,TargetTemperatureRobotDoorLower,CurentTemperater\n";
            await File.WriteAllTextAsync(filePath, header);
        }
    }

    public async Task LogStatusToCsvAsync()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{BaseUrl}/heater/status");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Assuming the response is in JSON format and needs to be parsed
            var statusData = JsonConvert.DeserializeObject<StatusData>(responseBody);

            HttpResponseMessage response02 = await client.GetAsync("http://localhost:5000/main/status");
            response02.EnsureSuccessStatusCode();
            string responseBody02 = await response02.Content.ReadAsStringAsync();

            // Assuming the response is in JSON format and needs to be parsed
            var mainStatusData = JsonConvert.DeserializeObject<MainStatusData>(responseBody02);

            // Prepare CSV content
            string csvContent = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{statusData.TemperatureTop},{statusData.TemperatureBottom},{statusData.TemperatureLeft},{statusData.TemperatureRight},{statusData.TemperatureUserDoor},{statusData.TemperatureRobotDoorUpper},{statusData.TemperatureRobotDoorLower},{statusData.TargetTemperatureTop},{statusData.TargetTemperatureBottom},{statusData.TargetTemperatureLeft},{statusData.TargetTemperatureRight},{statusData.TargetTemperatureUserDoor},{statusData.TargetTemperatureRobotDoorUpper},{statusData.TargetTemperatureRobotDoorLower},{mainStatusData.Temperature}\n";

            // Append to CSV file
            string fileName = $"{DateTime.Now:yyyyMMdd}HeaterTemperatureLog.csv";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            await File.AppendAllTextAsync(filePath, csvContent);

            Console.WriteLine("Status appended to CSV file successfully.");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }
}

public class StatusData
{
     #region Current Chamber Temperature
        public double TemperatureChamberUpper { get; set; }
        public double TemperatureChamberLower { get; set; }
        #endregion

        #region Current Wall Temperature
        public double TemperatureTop { get; set; }
        public double TemperatureBottom { get; set; }
        public double TemperatureLeft { get; set; }
        public double TemperatureRight { get; set; }
        public double TemperatureUserDoor { get; set; }
        public double TemperatureRobotDoorUpper { get; set; }
        public double TemperatureRobotDoorLower { get; set; }
        #endregion

        #region Current Wall Target Temperature
        public double TargetTemperatureTop { get; set; }
        public double TargetTemperatureBottom { get; set; }
        public double TargetTemperatureLeft { get; set; }
        public double TargetTemperatureRight { get; set; }
        public double TargetTemperatureUserDoor { get; set; }
        public double TargetTemperatureRobotDoorUpper { get; set; }
        public double TargetTemperatureRobotDoorLower { get; set; }
        #endregion
}

public class MainStatusData
{
     public double Temperature { get; set; } = 0.0;
        public double Humidity { get; set; } = 0.0;
        public bool IsRunHumidifier { get; set; } = false;
        public double Co2Concentration { get; set; } = 0.0;
        public bool IsOnCo2ValveA { get; set; } = false;
        public bool IsOnCo2ValveB { get; set; } = false;
        public bool IsRemainCo2GasA { get; set; } = false;
        public bool IsRemainCo2GasB { get; set; } = false;
        public double CirculationFanSpeedBackLeft { get; set; } = 0;
        public double CirculationFanSpeedBackRight { get; set; } = 0;
        public double CirculationFanSpeedFrontRight { get; set; } = 0;
        public double CirculationFanSpeedFrontLeft { get; set; } = 0;
        public bool IsExistWaterLeft { get; set; } = false;
        public bool IsExistWaterRight { get; set; } = false;
        public bool IsOnUvLamp { get; set; } = false;
        public bool IsOnBuzzer { get; set; } = false;
        public bool IsOnDetectionLightSensor { get; set; } = false;
        public bool IsOnUpperRobotDoorOpenSensor { get; set; } = false;
        public bool IsOnUpperRobotDoorCloseSensor { get; set; } = true;
        public bool IsOnLowerRobotDoorOpenSensor { get; set; } = false;
        public bool IsOnLowerRobotDoorCloseSensor { get; set; } = true;
        public bool IsMovingUpperRobotDoor { get; set; } = false;
        public bool IsMovingLowerRobotDoor { get; set; } = false;
        public bool IsUserDoorSafety { get; set; } = false;
        public bool IsOpenUserDoor { get; set; } = false;
        public bool IsOpenGlassDoor { get; set; } = false;
        public bool IsOpenStockerDoor { get; set; } = false;
        public bool IsOnTrashRobotDoorOpenSensor { get; set; } = false;
        public bool IsOnTrashRobotDoorCloseSensor { get; set; } = true;
        public bool IsMovingTrashRobotDoor { get; set; } = false;
        public bool IsPushCarouselStopButton { get; set; } = false;
        public bool IsTurnHumidityControlOffWhenTargetReached { get; set; } = false;
        public bool IsTurnCo2ControlOffWhenTargetReached { get; set; } = false;
        public bool IsError { get; set; } = false;
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public double FADTemperature { get; set; } = 0.0;
        public bool IsDemo { get; set; } = false;
        public bool IsUseOpenRobotDoorPopup { get; set; } = false;
        public string FADVersion { get; set; }

}