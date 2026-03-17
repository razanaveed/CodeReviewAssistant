namespace CodeReviewAssistant.Models
{
    public class ReviewResult
    {
        public List<string> Improvements { get; set; } = new();
        public List<string> Positives { get; set; } = new();
        public string RefactoredCode { get; set; } = "";
    }
}
