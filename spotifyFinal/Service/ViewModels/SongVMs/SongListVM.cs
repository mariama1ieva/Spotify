namespace Service.ViewModels.SongVMs
{
    public class SongListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Song { get; set; }

        public string Color { get; set; }

        public string Path { get; set; }
        public double? PointRayting { get; set; }

        public string ArtistName { get; set; }
        public string CategoryName { get; set; }
        public string AlbumName { get; set; }
    }
}
