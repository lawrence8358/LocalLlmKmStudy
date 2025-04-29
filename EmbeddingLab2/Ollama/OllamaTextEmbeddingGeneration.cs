using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel; 
using System.Net.Http.Json; 

namespace EmbeddingLab2.Ollama
{
#pragma warning disable SKEXP0001  
    public class OllamaTextEmbeddingGeneration(string modelId, string baseUrl, HttpClient http, ILoggerFactory? loggerFactory) :
        OllamaBase<OllamaTextEmbeddingGeneration>(modelId, baseUrl, http, loggerFactory), ITextEmbeddingGenerationService
    {
        public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = new())
        {
            var result = new List<ReadOnlyMemory<float>>(data.Count);

            string? model = Attributes["model_id"] as string;
            string? url = Attributes["base_url"] as string;



            foreach (var text in data)
            {
                var request = new
                {
                    model,
                    input = text
                };

                var response = await Http.PostAsJsonAsync($"{url}/api/embed", request, cancellationToken).ConfigureAwait(false);

                var json = await GetOllamaResponseAsync<OllamaEmbeddingResult>(response);
                if (json == null) break;

                var embedding = new ReadOnlyMemory<float>(json.Embeddings[0].ToArray());

                result.Add(embedding);
            }

            return result;
        }
    }
}
