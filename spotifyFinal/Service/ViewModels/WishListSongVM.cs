namespace Service.ViewModels
{
    public class WishListSongVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string FormattedTime { get; set; }
        public int AlbumId { get; set; }
        public int SongId { get; set; }
        public string Image { get; set; }
        public string Path { get; set; }
        public string ArtistName { get; set; }
        public int ArtistId { get; set; }
        public string AlbumName { get; set; }
    }
}
