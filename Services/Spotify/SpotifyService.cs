namespace TappApi.Services.Spotify;
using TappApi.ViewModels;
using System.Web;
using System.Net.Http.Headers;

public class SpotifyService
{
    private readonly HttpClient _httpClient;
    public SpotifyService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");
    }

    public async Task<object> SearchForItem(SearchItemViewModel searchItem,string Token)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["q"] = searchItem.Name;
        query["type"] = string.Join(",", searchItem.ItemType.Select(t => t.ToString()));
        query["limit"] = "10";
        var url = $"search?{query}";
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer",Token);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}