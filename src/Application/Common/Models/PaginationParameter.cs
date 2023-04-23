namespace BookStore.Application.Common.Models
{
    public abstract class PaginationParameter
    {
        private const int maxPageSize = 50;

        private int _pageSize = maxPageSize;

        public int PageSize
        {
            get { return _pageSize; }

            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public int Page { get; set; } = 1;

        public bool DisablePageLimit { get; set; }

    }
}