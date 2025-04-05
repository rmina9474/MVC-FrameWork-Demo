namespace Reina.MacCredy.Models
{
    public class ErrorViewModel
    {
        public required string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public required string ErrorMessage { get; set; }
    }
}
