﻿namespace ManagementSystem.Models
{
    public class CreateUserDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
