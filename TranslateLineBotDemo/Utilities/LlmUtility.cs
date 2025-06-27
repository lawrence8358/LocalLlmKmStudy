#pragma warning disable SKEXP0070
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using System.Text.Json;
using TranslateLineBotDemo.Models;

namespace TranslateLineBotDemo.Utilities
{
    public class LlmUtility
    {
        private readonly string _geminiApiKey;
        private readonly string _pluginDirectory = "Plugins/KaohsiungPlugin";
        private readonly string _pluginName = "KaohsiungTranslate";
        private readonly string _functionName = "TranslatePrompt";
        private readonly string _geminiModelId = "gemini-2.0-flash";

        public LlmUtility(string geminiApiKey)
        {
            _geminiApiKey = geminiApiKey;
        }

        private Kernel CreateKernel()
        {
            return Kernel.CreateBuilder()
                .AddGoogleAIGeminiChatCompletion(
                    modelId: _geminiModelId,
                    apiKey: _geminiApiKey,
                    apiVersion: Microsoft.SemanticKernel.Connectors.Google.GoogleAIVersion.V1_Beta
                )
                .Build();
        }

        public async Task<LanguageTranslation?> TranslateAsync(string userInput)
        {
            var kernel = CreateKernel();
            kernel.ImportPluginFromPromptDirectory(_pluginDirectory, _pluginName);

            var promptExecutionSettings = new GeminiPromptExecutionSettings
            {
                MaxTokens = 1000,
                ResponseSchema = typeof(LanguageTranslation),
                ResponseMimeType = "application/json"
            };
            var kernelArgs = new KernelArguments(promptExecutionSettings)
            {
                ["user_input"] = userInput,
                ["class_type"] = nameof(LanguageTranslation)
            };

            var result = await kernel.InvokeAsync(
                pluginName: _pluginName,
                functionName: _functionName,
                arguments: kernelArgs
            );

            var response = result.GetValue<object>()?.ToString();
            if (string.IsNullOrWhiteSpace(response)) return null;
            try
            {
                return JsonSerializer.Deserialize<LanguageTranslation>(response!);
            }
            catch
            {
                return null;
            }
        }
    }
}
