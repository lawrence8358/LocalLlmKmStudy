using Microsoft.SemanticKernel.Memory; 

namespace EmbeddingLab2.Ollama
{
#pragma warning disable SKEXP0001 
    internal static class OllamaMemoryBuilderExtensions
    {
        /// <summary>
        /// 使用 Ollama 詞嵌入
        /// </summary> 
        public static MemoryBuilder WithOllamaTextEmbeddingGeneration(this MemoryBuilder builder, string modelId, string baseUrl)
        {
            builder.WithTextEmbeddingGeneration((logger, http) => new OllamaTextEmbeddingGeneration(
                modelId,
                baseUrl,
                http,
                logger
            ));

            return builder;
        }
    }
}
