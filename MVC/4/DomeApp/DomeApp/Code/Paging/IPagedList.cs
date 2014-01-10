using System.Collections;

namespace DomeApp.Code.Paging
{
    public interface IPagedList: IEnumerable
    {
        int TotalPages { get; }

        int CurrentPage { get; set; }

        int PageSize { get; set; }

        bool HasNextPage { get; }

        bool HasPreviousPage { get; }
    }
}