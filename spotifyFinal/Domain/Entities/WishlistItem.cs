﻿using Domain.Common;

namespace Domain.Entities
{
    public class WishlistItem : BaseEntitiy
    {
        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
    }
}