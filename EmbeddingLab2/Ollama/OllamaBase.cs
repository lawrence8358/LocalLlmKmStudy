using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EmbeddingLab2.Ollama
{
    public interface IOllamaBase
    {
    }

    public abstract class OllamaBase<T> : IOllamaBase where T : OllamaBase<T>
    {
        #region Members

        protected readonly HttpClient Http;
        protected readonly ILogger<T> Logger;
        private readonly Dictionary<string, object?> _attributes = [];

        #endregion

        #region  Properties

        public IReadOnlyDictionary<string, object?> Attributes => _attributes;

        #endregion

        #region Constructor

        protected OllamaBase(string modelId, string baseUrl, HttpClient http, ILoggerFactory? loggerFactory)
        {
            _attributes.Add("model_id", modelId);
            _attributes.Add("base_url", baseUrl);

            Http = http;
            Logger = loggerFactory is not null ? loggerFactory.CreateLogger<T>() : NullLogger<T>.Instance;
        }

        #endregion

        #region Protected Methods

        protected async Task<TModel?> GetOllamaResponseAsync<TModel>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TModel>(responseContent);
        }

        #endregion
    }
}
