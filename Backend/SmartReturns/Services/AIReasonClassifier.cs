namespace SmartReturns.Services
{
    public class AIReasonClassifier : IAIReasonClassifier
    {
        public string Classify(string input)
        {
            if (input.Contains("broken", StringComparison.OrdinalIgnoreCase)) return "Defective";
            if (input.Contains("wrong", StringComparison.OrdinalIgnoreCase)) return "Incorrect Item";
            return "Other";
        }
    }
}
