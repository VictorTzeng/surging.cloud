namespace Surging.Core.Domain
{
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {

    }
}
