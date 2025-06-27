using System.ComponentModel;

namespace TranslateLineBotDemo.Models
{
    public class LanguageTranslation
    {
        [Description("繁體中文")]
        public string Chinese { get; set; } = string.Empty;

        [Description("英語")]
        public string English { get; set; } = string.Empty;

        [Description("日文")]
        public string Japanese { get; set; } = string.Empty;

        [Description("越南")]
        public string Vietnam { get; set; } = string.Empty;

        [Description("菲律賓")]
        public string Pilipinas { get; set; } = string.Empty;

        [Description("使用者輸入的語言代碼，例如:tw、en、jp、vn、ph")]
        public string InputCode { get; set; } = string.Empty;
    }
}
