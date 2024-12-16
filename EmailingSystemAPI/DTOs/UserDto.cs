﻿namespace EmailingSystemAPI.DTOs
{
    public class UserDto
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Role { get; set; }
        public string? PicturePath { get; set; } = null!;
        public string? CollegeName { get; set; } = null!;
        public string? DepartmentName { get; set; } = null!;

    }
}