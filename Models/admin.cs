namespace Modells.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("admin")]
    public partial class admin
    {
        public int adminId { get; set; }

        [Required]
        [StringLength(50)]
        public string adminLogin { get; set; }

        [Required]
        [StringLength(50)]
        public string adminPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string adminEmail { get; set; }

        [Required]
        [StringLength(255)]
        public string adminPictureUrl { get; set; }
    }
}
