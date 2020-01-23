namespace Modells.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            comments = new HashSet<comments>();
        }

        [Key]
        public int userId { get; set; }

        [Required]
        [StringLength(50)]
        public string userLogin { get; set; }

        [Required]
        [StringLength(50)]
        public string userPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string userEmail { get; set; }

        [Required]
        [StringLength(255)]
        public string userPictureUrl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comments> comments { get; set; }
    }
}
