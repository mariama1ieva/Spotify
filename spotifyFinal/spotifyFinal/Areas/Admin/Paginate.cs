namespace spotifyFinal.Areas.Admin
{
    public class Paginate<T> : List<T>
    {

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public Paginate(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static Paginate<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new Paginate<T>(items, count, pageIndex, pageSize);

        }



    }
}
