using System.Text.Json;
using ApiContracts;
using BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync(
        int? postId = null,
        int? userId = null,
        string? username = null)
    {
        var queryParameters = new List<string>();

        if (postId.HasValue)
        {
            queryParameters.Add($"postId={postId.Value}");
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

        var url = "Comments";

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

        return JsonSerializer.Deserialize<IEnumerable<CommentDto>>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<CommentDto>();
    }

    public async Task<CommentDto?> GetCommentByIdAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.GetAsync($"Comments/{id}");

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task<CommentDto> AddCommentAsync(
        CreateCommentDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync("Comments", request);

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task UpdateCommentAsync(
        int id,
        UpdateCommentDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PutAsJsonAsync($"Comments/{id}", request);

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage httpResponse =
            await client.DeleteAsync($"Comments/{id}");

        string response =
            await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}
