namespace Surging.Core.Domain
{
    public interface IPagedResultRequest
    {
        int PageCount { get; set; }

        int PageIndex { get; set; }
    }
}
