using System.Collections.Generic;

namespace Surging.Core.Domain
{
    public interface IListResult<T>
    {
        IReadOnlyList<T> Items { get; set; }
    }
}
