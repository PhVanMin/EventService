﻿using System.ComponentModel.DataAnnotations;

namespace EventService.Infrastructure.Idempotency {
    public class ClientRequest {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public DateTime Time { get; set; }
    }
}
