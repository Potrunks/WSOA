﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSOA.Shared.Entity
{
    public class MainNavSubSection
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Label { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        [ForeignKey(nameof(MainNavSection))]
        public int MainNavSectionId { get; set; }
        public MainNavSection MainNavSection { get; set; }
    }
}
