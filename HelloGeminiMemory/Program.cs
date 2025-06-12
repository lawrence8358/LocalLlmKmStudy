using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace HelloGeminiMemory
{
#pragma warning disable SKEXP0070 // 類型僅供評估之用，可能會在未來更新中變更或移除。抑制此診斷以繼續。
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var geminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "";
            if (string.IsNullOrEmpty(geminiApiKey))
            {
                Console.WriteLine("請設定環境變數 Gemini API Key");
                return;
            }

            // 設定 Gemini API 金鑰與模型 ID（這裡使用 gemini-2.0-flash，適合對話應用） 
            var geminiModelId = "gemini-2.0-flash"; // 可以依需求改成其他版本，例如 gemini-pro

            // 建立 Semantic Kernel 的核心物件，並註冊 Gemini 的 Chat Completion 服務
            Kernel kernel = Kernel.CreateBuilder()
                .AddGoogleAIGeminiChatCompletion(modelId: geminiModelId, apiKey: geminiApiKey)
                .Build();


            Console.WriteLine("====== 單次對話(無記憶功能)的呼叫範例 ======");
            await InvokePromptAsync(kernel, "請問台灣總統是誰?");
            await InvokePromptAsync(kernel, "那他的背景是什麼?");


            Console.WriteLine("====== 多次對話(有記憶功能)的呼叫範例 ======");
            var chatService = kernel.GetRequiredService<IChatCompletionService>();
            var chatHistory = new ChatHistory();

            await GetChatMessageContentAsync(chatService, chatHistory, "請問台灣總統是誰?");
            await GetChatMessageContentAsync(chatService, chatHistory, "那他的背景是什麼?");
        }

        /// <summary>
        /// 單次對話(無記憶功能)的呼叫範例
        /// </summary>
        private static async Task InvokePromptAsync(Kernel kernel, string userInput)
        {
            // 輸出使用者輸入內容 
            Console.WriteLine("User: " + userInput);

            // 呼叫 Gemini 模型並取得回應
            var assistant = await kernel.InvokePromptAsync(userInput);

            // 輸出 Gemini 回應的內容 
            Console.WriteLine("Assistant: " + assistant);
        }

        /// <summary>
        /// 多次對話(有記憶功能)的呼叫範例
        /// </summary>

        private static async Task GetChatMessageContentAsync(IChatCompletionService chatService, ChatHistory chatHistory, string userInput)
        {
            // 輸出使用者輸入內容 
            Console.WriteLine("User: " + userInput);
            chatHistory.AddUserMessage(userInput);

            // 呼叫 Gemini 模型並取得回應
            var response = await chatService.GetChatMessageContentAsync(chatHistory);

            // 輸出 Gemini 回應的內容
            var content = response.Content ?? "";
            Console.WriteLine("Assistant: " + content);
            chatHistory.AddAssistantMessage(content);
        }
    }
}