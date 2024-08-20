namespace Service.ViewModels
{
    public class WishListVM
    {
        public WishListVM()
        {
            WishlistSongs = new List<WishListSongVM>();
        }
        public List<WishListSongVM> WishlistSongs { get; set; }
    }
}
