namespace Surging.Core.Domain
{
    public abstract class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        public string Sorting { get; set; }
    }
}
