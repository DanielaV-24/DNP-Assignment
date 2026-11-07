using System.Text.Json;
using ApiContracts;
using BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IEnumerable<PostDto>> GetAllPostsAsync(
        string? title = null,
        int? userId = null,
        string? username = null)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrWhiteSpace(title))
        {
            queryParameters.Add(
                $"title={Uri.EscapeDataString(title)}");
        }

        if (userId.HasValue)
        {
            queryParameters.Add($"userId={userId.Value}");
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            queryParameters.Add(
                $"username={Uri.EscapeDataString(username)}");
        }

        var url = "Posts";

        if (queryParameters.Count > 0)
        {
            url += "?" + string.Join("&", queryParameters);
        }

        HttpResponseMessage httpResponse =
            await client.GetAsync(url);

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<IEnumerable<PostDto>>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PostDto>();
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"Posts/{id}");

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync("Posts", request);

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task UpdatePostAsync(
        int id,
        UpdatePostDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PutAsJsonAsync($"Posts/{id}", request);

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.DeleteAsync($"Posts/{id}");

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}
