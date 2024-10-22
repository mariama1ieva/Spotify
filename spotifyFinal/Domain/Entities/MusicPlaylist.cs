﻿using Domain.Common;

namespace Domain.Entities
{
    public class MusicPlaylist : BaseEntity
    {
        public int SongId { get; set; }
        public Song Song { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
