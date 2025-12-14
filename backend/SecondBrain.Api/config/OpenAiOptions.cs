namespace SecondBrain.Api.Config;

public class OpenAiOptions
{
	public string ApiKey { get; set; } = string.Empty;
	public string EmbeddingModel { get; set; } = "text-embedding-3-small";
}