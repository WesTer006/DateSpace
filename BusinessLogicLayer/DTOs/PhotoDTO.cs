﻿namespace BusinessLogicLayer.DTOs
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
