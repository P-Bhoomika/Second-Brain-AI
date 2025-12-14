public class FakeEmbeddingService : IEmbeddingService
{
    public Task<float[]> CreateEmbeddingAsync(
        string input,
        CancellationToken ct = default)
    {
        var random = new Random(input.GetHashCode());

        var vector = Enumerable.Range(0, 384)
            .Select(_ => (float)random.NextDouble())
            .ToArray();

        return Task.FromResult(vector);
    }
}
