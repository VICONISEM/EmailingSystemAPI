﻿namespace EmailingSystem.Core.Entities
{
    public class Signature : BaseEntity
    {
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;


    }
}