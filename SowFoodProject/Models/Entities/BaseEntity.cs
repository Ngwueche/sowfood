﻿namespace SowFoodProject.Models.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(1);
        public bool IsActive { get; set; } = true;
    }
}
