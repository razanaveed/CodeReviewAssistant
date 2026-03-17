namespace CodeReviewAssistant.Models
{
    public class OpenAIErrorResponse
    {
        public OpenAIError Error { get; set; }
    }

    public class OpenAIError
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public string? Param { get; set; }
        public string Code { get; set; }
    }
    public class GroqChatResponse
    {
        public List<GroqChoice> Choices { get; set; } = new();
    }

    public class GroqChoice
    {
        public GroqMessage Message { get; set; }
    }

    public class GroqMessage
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
    }
}
