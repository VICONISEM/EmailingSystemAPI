namespace EmailingSystemAPI.Errors
{
    public class APIValidationErrorResponse : APIErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public APIValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }
    }
}
