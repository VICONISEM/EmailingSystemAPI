namespace EmailingSystemAPI.Errors
{
    public class APIExceptionResponse : APIErrorResponse
    {
        public string? Description { get; set; }

        public APIExceptionResponse(int StatusCode, string? Message = null, string? Description = null) : base(StatusCode, Message)
        {
            this.Description = Description;
        }
    }
}
