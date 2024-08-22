using Domain.Entities;

namespace spotifyFinal.Areas.Admin.PaginateVM
{
    public class PaginateCategoryListVM
    {
        public Paginate<Category> Categories { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }



}
