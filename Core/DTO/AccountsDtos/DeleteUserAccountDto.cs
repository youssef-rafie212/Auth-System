﻿using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class DeleteUserAccountDto
    {
        [Required]
        public string? Username { get; set; }
    }
}
