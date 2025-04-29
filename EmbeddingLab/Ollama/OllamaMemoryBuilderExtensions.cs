using Microsoft.SemanticKernel.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddingLab.Ollama
{
    internal class OllamaMemoryBuilderExtensions
    {
        ///// <summary>
        ///// 使用 Ollama 詞嵌入
        ///// </summary> 
        //public static MemoryBuilder WithOllamaTextEmbeddingGeneration(this MemoryBuilder builder, string modelId, string baseUrl)
        //{
        //    // https://www.cnblogs.com/mingupupu/p/18339290
        //    // SemanticKernel/C#：使用Ollama中的对话模型与嵌入模型用于本地离线场景

        //    // builder.WithOllamaTextEmbeddingGeneration("all-minilm:latest", embeddingEndpoint);

        //    //builder.WithTextEmbeddingGeneration((logger, http) => new OllamaTextEmbeddingGeneration(
        //    //    modelId,
        //    //    baseUrl,
        //    //    http,
        //    //    logger
        //    //));

        //    return builder;
        //}
    }
}
