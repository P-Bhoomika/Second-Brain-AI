using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Options;
using SecondBrain.Api.Config;

namespace SecondBrain.Api.Services;


public class OpenAiEmbeddingService : IEmbeddingService
{
	private readonly HttpClient _httpClient;
	private readonly OpenAiOptions _options;

	public OpenAiEmbeddingService(HttpClient httpClient, IOptions<OpenAiOptions> options)
	{
		_httpClient = httpClient;
		_options = options.Value;
	}

	public async Task<float[]> CreateEmbeddingAsync(string input, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(_options.ApiKey))
			throw new InvalidOperationException("OpenAI API key is missing.");

		var requestBody = new
		{
			model = _options.EmbeddingModel,
			input
		};

		using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/embeddings");
		req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
		req.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

		using var resp = await _httpClient.SendAsync(req, ct);
		var json = await resp.Content.ReadAsStringAsync(ct);

		if (!resp.IsSuccessStatusCode)
			throw new InvalidOperationException($"OpenAI embeddings failed: {resp.StatusCode} - {json}");

		using var doc = JsonDocument.Parse(json);
		var vector = doc.RootElement
			.GetProperty("data")[0]
			.GetProperty("embedding")
			.EnumerateArray()
			.Select(x => x.GetSingle())
			.ToArray();

		return vector;
	}
}
