using System.Threading;
using System.Threading.Tasks;

public interface IEmbeddingService
{
    Task<float[]> CreateEmbeddingAsync(string input, CancellationToken ct = default);
}
