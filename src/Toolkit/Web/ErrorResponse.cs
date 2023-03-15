namespace Mttechne.Toolkit.Web;

public class ErrorResponse
{
    public ErrorResponse(string id)
    {
        ID = id;
        Moment = DateTime.Now;
        Message = "An unexpected error occurred on the server. Please contact support.";
    }

    public string ID { get; set; }
    public DateTime Moment { get; set; }
    public string Message { get; set; }
}