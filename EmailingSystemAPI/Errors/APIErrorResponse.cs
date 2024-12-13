namespace EmailingSystemAPI.Errors
{
    public class APIErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public APIErrorResponse(int StatusCode, string? Message = null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message ?? GetDefaultErrorMessage(StatusCode);
        }

        private string? GetDefaultErrorMessage(int StatusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not found",
                500 => "An unexpected error occurred on the server. Please try again later or contact support if the problem persists.",
                _ => null
            };
        }
    }
}
