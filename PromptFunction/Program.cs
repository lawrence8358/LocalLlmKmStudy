using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;

namespace PromptFunction
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

            // 使用 Import Plugin 的方式載入 Prompt Function
            await RunTranslationPromptUseImportAsync(geminiApiKey);

            // 使用 Create Plugin 的方式載入 Prompt Function
            await RunTranslationPromptUseCreateAsync(geminiApiKey);
        }

        private static async Task RunTranslationPromptUseImportAsync(string geminiApiKey)
        {
            Kernel kernel = CreateKernel(geminiApiKey);

            // pluginName 參數非必要，它用來指定 Plugin 的別名
            // 假設有載入多個 Plugin 時，後續如果要指定 Plugin 的話，則必須使用這裡定義的 pluginName
            kernel.ImportPluginFromPromptDirectory(
                pluginDirectory: "Plugins\\TaipeiPlugin",
                pluginName: "TaipeiTranslate"
            );
            kernel.ImportPluginFromPromptDirectory(
                pluginDirectory: "Plugins\\KaohsiungPlugin",
                pluginName: "KaohsiungTranslate"
            );


            var promptExecutionSettings = new GeminiPromptExecutionSettings
            {
                Temperature = 1,
                MaxTokens = 1000,
                ResponseSchema = typeof(LanguageTranslation),
                ResponseMimeType = "application/json"
            };
            var kernelArgs = new KernelArguments(promptExecutionSettings)
            {
                ["user_input"] = "早安，今天依舊是令人振奮的一天。",
                ["class_type"] = nameof(LanguageTranslation)
            };

            // 使用 Prompt Function
            // https://www.cnblogs.com/xbotter/p/semantic_kernel_introduction_semantic_function.html
            // 若只有載入一個 Plugin，則 pluginName 可以省略不寫，但如果有多個 Plugin 建議寫上 pluginName
            // 實務上發現如果不寫 pluginName 它會自動選擇第一個 Plugin
            FunctionResult result = await kernel.InvokeAsync(
                pluginName: "KaohsiungTranslate",
                functionName: "TranslatePrompt",
                arguments: kernelArgs
            );

            var response = result.GetValue<string>();
            Console.WriteLine("response:\n" + response);
        }

        private static async Task RunTranslationPromptUseCreateAsync(string geminiApiKey)
        {
            Kernel kernel = CreateKernel(geminiApiKey);

            var promptExecutionSettings = new GeminiPromptExecutionSettings
            {
                MaxTokens = 1000,
                ResponseSchema = typeof(LanguageTranslation),
                ResponseMimeType = "application/json"
            };
            var kernelArgs = new KernelArguments(promptExecutionSettings)
            {
                ["user_input"] = "早安，今天依舊是令人振奮的一天。",
                ["class_type"] = nameof(LanguageTranslation)
            };

            KernelPlugin function = kernel.CreatePluginFromPromptDirectory(
                pluginDirectory: "Plugins\\TaipeiPlugin"
            );

            FunctionResult response = await kernel.InvokeAsync(
                function: function["TranslatePrompt"],
                arguments: kernelArgs
            );

            // 理論上應該可以使用下面這個寫法，但目前可能是有 BUG，所以會拋回 Microsoft.SemanticKernel.KernelException 
            // No service was found for any of the supported types:
            // Microsoft.SemanticKernel.ChatCompletion.IChatCompletionService,
            // Microsoft.SemanticKernel.TextGeneration.ITextGenerationService,
            // Microsoft.Extensions.AI.IChatClient.'
            // var response2 = await function["TranslatePrompt"].InvokeAsync(arguments: kernelArgs);

            Console.WriteLine("response:\n" + response);
        }


        private static Kernel CreateKernel(string geminiApiKey)
        {
            // 設定 Gemini API 金鑰與模型 ID（這裡使用 gemini-2.0-flash，適合對話應用） 
            var geminiModelId = "gemini-2.0-flash"; // 可以依需求改成其他版本，例如 gemini-pro

            // 建立 Semantic Kernel 的核心物件，並註冊 Gemini 的 Chat Completion 服務
            Kernel kernel = Kernel.CreateBuilder()
                .AddGoogleAIGeminiChatCompletion(
                    modelId: geminiModelId,
                    apiKey: geminiApiKey,
                    apiVersion: Microsoft.SemanticKernel.Connectors.Google.GoogleAIVersion.V1_Beta
                )
                .Build();

            return kernel;
        }
    }
}
