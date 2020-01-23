namespace Modells.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("picture")]
    public partial class picture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public picture()
        {
            comments = new HashSet<comments>();
        }

        public int pictureId { get; set; }

        [Required]
        [StringLength(50)]
        public string pictureTitle { get; set; }

        [Required]
        [StringLength(50)]
        public string pictureAlternateTitle { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string pictureDescription { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string pictureStandardUrl { get; set; }

        public short? pictureRatingValue { get; set; }

        public short? pictureViewsNumber { get; set; }

        public int categoryId { get; set; }

        public virtual category category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comments> comments { get; set; }
    }
}
