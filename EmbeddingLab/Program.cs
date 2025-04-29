using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Connectors.Sqlite;
using Codeblaze.SemanticKernel.Connectors.Ollama;

namespace EmbeddingLab
{
#pragma warning disable SKEXP0001, SKEXP0020 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var ollamaEndpoint = "http://localhost:11434";
            var ollamaEmbeddingModel = "snowflake-arctic-embed2";
            var category = "EmbeddingLab";

            Console.Write("請輸入要編碼的內容：");
            var text = Console.ReadLine() ?? "";

            var memory = await CreateSemanticTextMemoryAsync(modelId: ollamaEmbeddingModel, baseUrl: ollamaEndpoint);

            // 將輸入的文本儲存到 Sqlite 
            await memory.SaveInformationAsync(collection: category, text: text, id: "Demo");
        }

        /// <summary>
        /// 建立語意文本詞嵌入記憶體 (使用 Semantic Kernel)
        /// </summary>
        /// <param name="modelId">模型名稱</param>
        /// <param name="baseUrl">本地端 Ollama 站台 URL</param>
        /// <returns></returns>
        private static async Task<ISemanticTextMemory> CreateSemanticTextMemoryAsync(string modelId, string baseUrl)
        {
            var memory = new MemoryBuilder()
                .WithHttpClient(new HttpClient())
                .WithOllamaTextEmbeddingGeneration(modelId, baseUrl)
                .WithMemoryStore(await SqliteMemoryStore.ConnectAsync("vectors.db")) // 使用 Sqlite 作為記憶體儲存
                .Build();

            return memory;
        }
    }
}
