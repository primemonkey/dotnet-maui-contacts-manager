using System.Net.Http.Json;

namespace TestApp.Services;

public class InfoService
{
    HttpClient httpClient;
    public InfoService()
    {
        this.httpClient = new HttpClient();
    }

    List<Info> infoList;
    public async Task<List<Info>> GetInfos()
    {
        if (infoList?.Count > 0)
            return infoList;

        // Online
        var response = await httpClient.GetAsync("https://raw.githubusercontent.com/primemonkey/dotnet-maui-contacts-manager-data/main/infodata.json");
        if (response.IsSuccessStatusCode)
        {
            infoList = await response.Content.ReadFromJsonAsync(InfoContext.Default.ListInfo);
        }

        // Offline
        //using var stream = await FileSystem.OpenAppPackageFileAsync("infodata.json");
        //using var reader = new StreamReader(stream);
        //var contents = await reader.ReadToEndAsync();
        //infoList = JsonSerializer.Deserialize(contents, InfoContext.Default.ListInfo); 

        return infoList;
    }
}
