﻿using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.Entities;

public class City
{
    [Key] public Guid Id { get; set; }
    [Required] public string? Name { get; set; }
}
