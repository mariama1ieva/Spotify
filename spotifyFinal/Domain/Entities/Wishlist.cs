﻿using Domain.Common;

namespace Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
