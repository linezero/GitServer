namespace GitServer.Models
{
	public class ErrorModel
    {
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string Description { get; set; }
    }
}
