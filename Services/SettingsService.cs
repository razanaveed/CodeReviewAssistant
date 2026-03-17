using CodeReviewAssistant.Models;

namespace CodeReviewAssistant.Services
{
    public class SettingsService
    {
        public UserAISettings Settings { get; set; } = new UserAISettings();
    }
}
