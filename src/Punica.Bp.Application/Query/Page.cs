namespace Punica.Bp.Application.Query
{
    public class Page<TViewModel>
    {
        public IEnumerable<TViewModel> Results { get; private set; }
        public long TotalResult { get; private set; }
        public int CurrentPage { get; private set; }
        public Pagination? NextPage { get; private set; }
        public Pagination? PreviousPage { get; private set; }
        public int TotalPages { get; private set; }

        public Page(IEnumerable<TViewModel> results, long totalResult, int page, int limit)
        {
            Results = results;
            TotalResult = totalResult;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(((double)totalResult / (double)limit));

            var startIndex = (page - 1) * limit;
            var endIndex = page * limit;

            if (page > 1)
            {
                PreviousPage = new Pagination()
                {
                    Page = page - 1,
                    Limit = limit
                };
            }
            if (page * limit <= totalResult)
            {
                NextPage = PreviousPage = new Pagination()
                {
                    Page = page + 1,
                    Limit = limit
                };
            }
        }
    }
}
