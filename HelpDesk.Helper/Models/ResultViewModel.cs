namespace HelpDesk.Helper.Models
{
    public class ResultViewModel
    {
        public ResultViewModel()
        {
            IsError = false;
            StatusCode = 200;
            Message = "Completed";
        }

        public bool IsError { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
