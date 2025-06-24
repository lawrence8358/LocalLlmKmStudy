using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;

namespace PromptInline
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
                .AddGoogleAIGeminiChatCompletion(
                    modelId: geminiModelId,
                    apiKey: geminiApiKey,
                    apiVersion: Microsoft.SemanticKernel.Connectors.Google.GoogleAIVersion.V1_Beta
                )
                .Build();

            // 台電於早上八點發生壓降，請負責各設備負責人，務必確實檢查對應的機台狀況是否正常，如有問題請聯絡廠務值班 886886。
            string userInput = "Um 8:00 Uhr morgens kam es bei Taipower zu einem Spannungsabfall. Bitte stellen Sie sicher, dass die für die jeweiligen Geräte verantwortlichen Personen den Zustand der entsprechenden Maschinen überprüfen. Bei Problemen wenden Sie sich bitte an den diensthabenden Mitarbeiter im Werk unter der Telefonnummer 886886.";

            // 使用自然語言回應，並且不指定輸出格式，結果可參考 OutputExample\NaturalLanguageOutputNonSpecMiniType.txt
            await RunNaturalLanguageOutputWithoutSpecMinitTypeAsync(kernel, userInput);

            // 使用自然語言回應，並且指定輸出格式為 JSON，結果可參考 OutputExample\NaturalLanguageOutputSpecMiniType.json
            await RunNaturalLanguageOutputWithSpecMiniTypeAsync(kernel, userInput);

            // 使用結構化輸出回應，並且指定輸出格式為 JSON，結果可參考 OutputExample\StructureOutput.json
            await RunStructureOutputAsync(kernel, userInput);

            // 另外一種將提示工程放到 System Prompt 的方式，這樣可以讓提示工程與使用者輸入分離。
            await RunSystemPromptAsync(kernel, userInput);
        }

        /// <summary>
        /// 自然語言回應(不指定輸出格式)
        /// </summary>
        public static async Task RunNaturalLanguageOutputWithoutSpecMinitTypeAsync(Kernel kernel, string userInput)
        {
            var promptExecutionSettings = new GeminiPromptExecutionSettings
            {
                MaxTokens = 1000
            };

            await RunNaturalLanguageOutputAsync(kernel, userInput, promptExecutionSettings);
        }

        /// <summary>
        /// 自然語言回應(指定輸出格式)
        /// </summary>
        public static async Task RunNaturalLanguageOutputWithSpecMiniTypeAsync(Kernel kernel, string userInput)
        {
            var promptExecutionSettings = new GeminiPromptExecutionSettings
            {
                MaxTokens = 1000,
                ResponseMimeType = "application/json"
            };

            await RunNaturalLanguageOutputAsync(kernel, userInput, promptExecutionSettings);
        }

        /// <summary>
        /// 自然語言回應
        /// </summary>
        private static async Task RunNaturalLanguageOutputAsync(Kernel kernel, string userInput, GeminiPromptExecutionSettings promptExecutionSettings)
        {
            var kernelArgs = new KernelArguments(promptExecutionSettings)
            {
                ["user_input"] = userInput
            };

            string prompt = @"您是一位專業的翻譯專家，本次的任務是要對象是製造業所屬的工廠場域進行多語言翻譯，
使用者的輸入可能是任何語言，我需要你的協助翻譯成中文、英文、日文、越南和菲律賓五個國家的語言，以下是使用者輸入的內容。
### 
{{$user_input}}

### json format
{
    ""tw"": """",
    ""en"": """",
    ""jp"": """",
    ""vn"": """",
    ""ph"": """"，
    ""input-code"": """"
}

備註:
1. 各國語言請依當地語言習慣翻譯，並確保翻譯的內容符合當地文化。 
2. input-code 欄位請輸入使用者的語言代碼，例如：tw、en、jp、vn、ph 等。
3. 在您的回復中，不包括任何解釋性文字或評論，僅需回傳原始的JSON。
";

            Console.WriteLine("Prompt:\n" + prompt);
            var assistant = await kernel.InvokePromptAsync(prompt, kernelArgs);
            Console.WriteLine("Assistant:\n" + assistant);
        }

        /// <summary>
        /// 結構化輸出回應
        /// </summary>
        private static async Task RunStructureOutputAsync(Kernel kernel, string userInput)
        {
            var promptExecutionSettings = new GeminiPromptExecutionSettings
            {
                Temperature = 1,
                MaxTokens = 1000,
                ResponseSchema = typeof(LanguageTranslation),
                ResponseMimeType = "application/json"
            };

            var kernelArgs = new KernelArguments(promptExecutionSettings);
            kernelArgs.Add("user_input", userInput);

            string prompt = @"您是一位專業的翻譯專家，本次的任務是要對象是製造業所屬的工廠場域進行多語言翻譯，
使用者的輸入可能是任何語言，我需要你的協助翻譯成中文、英文、日文、越南和菲律賓五個國家的語言，以下是使用者輸入的內容。
### 
{{$user_input}}

備註:
1. 各國語言請依當地語言習慣翻譯，並確保翻譯的內容符合當地文化。
2. 請使用 LanguageTranslation 轉換成對應的 JSON 欄位。";

            Console.WriteLine("Prompt:\n" + prompt);
            var assistant = await kernel.InvokePromptAsync(prompt, kernelArgs);
            Console.WriteLine("Assistant:\n" + assistant);
        }

        /// <summary>
        /// 使用 System Prompt 的方式，分離使用者輸入與提示工程。
        /// </summary> 
        private static async Task RunSystemPromptAsync(Kernel kernel, string userInput)
        {
            var promptExecutionSettings = new GeminiPromptExecutionSettings
            {
                MaxTokens = 1000,
                ResponseSchema = typeof(LanguageTranslation),
                ResponseMimeType = "application/json"
            };

            string prompt = @"您是一位專業的翻譯專家，本次的任務是要對象是製造業所屬的工廠場域進行多語言翻譯，
使用者的輸入可能是任何語言，我需要你的協助翻譯成中文、英文、日文、越南和菲律賓五個國家的語言，以下是使用者輸入的內容。
### 
{{$user_input}}

備註:
1. 各國語言請依當地語言習慣翻譯，並確保翻譯的內容符合當地文化。
2. 請使用 LanguageTranslation 轉換成對應的 JSON 欄位。";

            var chatService = kernel.GetRequiredService<IChatCompletionService>();
            var chatHistory = new ChatHistory(prompt);
            // 也可以使用此底下這個方式來設定系統提示
            // chatHistory.AddSystemMessage(prompt); 

            chatHistory.AddUserMessage(userInput);

            Console.WriteLine("User: " + userInput);
            var response = await chatService.GetChatMessageContentAsync(chatHistory, promptExecutionSettings);
            Console.WriteLine("Assistant: " + response.Content ?? "");
        }
    }
}
