namespace CodeReviewAssistant.Models
{
    public class UserAISettings
    {
        public string ApiKey { get; set; } = "";

        public string Model { get; set; } = "llama-3.1-8b-instant";

        public double Temperature { get; set; } = 1;
        public int MaxTokens { get; set; } = 1024;

        public int ImprovementCount { get; set; } = 3;

        public int PositiveCount { get; set; } = 1;

        public bool IncludeRefactoredCode { get; set; } = true;
    }
}
