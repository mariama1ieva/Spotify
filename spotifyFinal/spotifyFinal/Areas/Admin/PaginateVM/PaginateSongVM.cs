namespace spotifyFinal.Areas.Admin.PaginateVM
{
    public class PaginateSongVM<T>
    {
        public List<T> Songs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }

        public PaginateSongVM(List<T> songs, int currentPage, int totalPage)
        {
            Songs = songs;
            CurrentPage = currentPage;
            TotalPage = totalPage;
        }
        public bool Previous
        {
            get
            {
                return CurrentPage > 1;
            }
        }
        public bool Next
        {
            get
            {
                return CurrentPage < TotalPage;
            }
        }
    }
}
