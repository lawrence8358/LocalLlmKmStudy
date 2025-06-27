# 部落格文章範例 
[AI Agent 時代來臨：用 GitHub Copilot 實戰 LINE 翻譯機器人](https://lawrencetech.blogspot.com/2025/06/ai-agent-github-copilot-line.html)

<SPAN style="color: red;">
本專案由 GitHub Copilot Coding Agent 產生，包含底下文件內容，及 99% 的程式碼，若要使用，請自行斟酌是否包含非預期的錯誤。
</SPAN>


## 專案說明
本專案為製造業工廠設計的多國語言 LINE Bot，支援中文、英文、日文、越南、菲律賓五國語言翻譯，並結合 Microsoft.SemanticKernel 與 LLM 進行多語系翻譯。

### 主要功能
1. 支援五國語言翻譯，並依格式回傳。
2. 僅處理文字訊息，非文字訊息自動回覆「我現在只會翻譯文字喔」。
3. 翻譯失敗時自動回覆「翻譯失敗，請稍後再試」。
4. 採用 SOLID 原則，翻譯與 LINE Bot 處理皆有獨立 Utility 類別。

### 重要設定
- 於系統環境變數設置 `LINE_CHANNEL_ACCESS_TOKEN`、`GEMINI_API_KEY`。
- 或 `appsettings.json` 設定 `LineBot:ChannelAccessToken`、`Google:GeminiApiKey`。
- Plugins 目錄下已複製 KaohsiungPlugin 的 Prompt 樣板。


### 重要參考
- LLM 多語系翻譯功能封裝於 `Utilities/LlmUtility.cs`，參考 PromptFunction 專案設計。
- LINE Bot 處理封裝於 `Utilities/LineBotUtility.cs`，請參考 `LineBotSDK_Readme.txt`。
- Model 統一於 `Models/` 目錄下。
- Controller 實作於 `Controllers/LineBotWebHookController.cs`。


### 回覆格式範例
```
###
中文: 早安，今天依舊是令人振奮的一天。

英文: Good morning, another inspiring day ahead.

日文: おはようございます。今日もまた、刺激的な一日になりそうですね。

越南: Chào buổi sáng, một ngày đầy hứng khởi nữa lại đến.

菲律賓: Magandang umaga, isa na namang araw na puno ng inspirasyon ang naghihintay.
###
```
