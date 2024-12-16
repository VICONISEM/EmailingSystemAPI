namespace EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams
{
    public class ConversationSpecParams
    {
        public string Sort { get; set; } = "dsec";
        public string Type { get; set; } = "inbox";

        private int pageNumber = 1;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value <= 0 ? 1 : value; }
        }

        private int pageSize = 50;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 50 || value <= 0 ? 50 : value; }
        }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.Trim().ToLower(); }
        }
    }
}
