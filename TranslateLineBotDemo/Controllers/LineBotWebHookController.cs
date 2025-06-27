using Microsoft.AspNetCore.Mvc;
using TranslateLineBotDemo.Utilities;

namespace TranslateLineBotDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LineBotWebHookController : ControllerBase
    {
        private readonly string _channelAccessToken;
        private readonly LlmUtility _llmUtility;

        public LineBotWebHookController(IConfiguration config)
        {
            _channelAccessToken = Environment.GetEnvironmentVariable("LINE_CHANNEL_ACCESS_TOKEN") ?? config["LineBot:ChannelAccessToken"] ?? string.Empty;
            var geminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? config["Google:GeminiApiKey"] ?? string.Empty;
            _llmUtility = new LlmUtility(geminiApiKey);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var reader = new StreamReader(Request.Body);
            var postData = await reader.ReadToEndAsync();
            var received = isRock.LineBot.Utility.Parsing(postData);
            if (received?.events == null) return Ok();

            foreach (var evt in received.events)
            {
                var replyToken = LineBotUtility.GetReplyToken(evt);
                var lineUtil = new LineBotUtility(_channelAccessToken);
                if (!LineBotUtility.IsTextMessage(evt))
                {
                    await lineUtil.ReplyMessageAsync(replyToken, "我現在只會翻譯文字喔");
                    continue;
                }
                string userText = LineBotUtility.GetUserText(evt);
                var translation = await _llmUtility.TranslateAsync(userText);
                if (translation == null)
                {
                    await lineUtil.ReplyMessageAsync(replyToken, "翻譯失敗，請稍後再試");
                    continue;
                }
                string reply = $"###\n中文: {translation.Chinese}\n\n英文: {translation.English}\n\n日文: {translation.Japanese}\n\n越南: {translation.Vietnam}\n\n菲律賓: {translation.Pilipinas}\n###";
                await lineUtil.ReplyMessageAsync(replyToken, reply);
            }

            return Ok();
        }
    }
}
