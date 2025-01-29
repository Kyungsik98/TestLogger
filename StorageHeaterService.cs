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

    static async Task Status()
    {
        var response = await client.GetAsync($"{BaseUrl}/heater/status");
        // response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
    }

    // Heater On API 호출
    static async Task StartHeater(int heater)
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
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{BaseUrl}/heater/status");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Assuming the response is in JSON format and needs to be parsed
            var statusData = JsonConvert.DeserializeObject<StatusData>(responseBody);

            // Prepare CSV content
            string csvContent = "Timestamp,TemperatureTop,TemperatureBottom,TemperatureLeft,TemperatureRight,TemperatureUserDoor,TemperatureRobotDoorUpper,TemperatureRobotDoorLower,TargetTemperatureTop,TargetTemperatureBottom,TargetTemperatureLeft,TargetTemperatureRight,TargetTemperatureUserDoor,TargetTemperatureRobotDoorUpper,TargetTemperatureRobotDoorLower\n";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            csvContent += $"{timestamp},{statusData.TemperatureTop},{statusData.TemperatureBottom},{statusData.TemperatureLeft},{statusData.TemperatureRight},{statusData.TemperatureUserDoor},{statusData.TemperatureRobotDoorUpper},{statusData.TemperatureRobotDoorLower},{statusData.TargetTemperatureTop},{statusData.TargetTemperatureBottom},{statusData.TargetTemperatureLeft},{statusData.TargetTemperatureRight},{statusData.TargetTemperatureUserDoor},{statusData.TargetTemperatureRobotDoorUpper},{statusData.TargetTemperatureRobotDoorLower}\n";


            // Save to CSV file
            string filePath = "/path/to/your/csvfile.csv";
            await File.WriteAllTextAsync(filePath, csvContent);

            Console.WriteLine("Status saved to CSV file successfully.");
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