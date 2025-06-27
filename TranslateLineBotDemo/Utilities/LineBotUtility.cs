using isRock.LineBot;
using System.Threading.Tasks;

namespace TranslateLineBotDemo.Utilities
{
    public class LineBotUtility
    {
        private readonly string _channelAccessToken;
        public LineBotUtility(string channelAccessToken)
        {
            _channelAccessToken = channelAccessToken;
        }

        public static bool IsTextMessage(dynamic receivedEvent)
        {
            // LINE BotSDK: event.message.type == "text"
            return receivedEvent?.message?.type == "text";
        }

        public static string GetUserText(dynamic receivedEvent)
        {
            return receivedEvent?.message?.text ?? string.Empty;
        }

        public static string GetReplyToken(dynamic receivedEvent)
        {
            return receivedEvent?.replyToken ?? string.Empty;
        }

        public async Task ReplyMessageAsync(string replyToken, string message)
        {
            await Task.Run(() => Utility.ReplyMessage(replyToken, message, _channelAccessToken));
        }
    }
}
