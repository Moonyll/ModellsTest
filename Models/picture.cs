namespace Modells.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("picture")]
    public partial class picture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public picture()
        {
            comments = new HashSet<comments>();
        }

        public int pictureId { get; set; }

        // Picture title :
        [Required]
        [StringLength(50)]
        [Display(Name = "Titre")]
        //[RegularExpression(pictureControls.PatternForPictureTitles, ErrorMessage = pictureControls.ErrorForPictureTitles)]
        public string pictureTitle { get; set; }

        // Picture alternative title :
        [Required]
        [StringLength(50)]
        [Display(Name = "Titre alternatif")]
        //[RegularExpression(pictureControls.PatternForPictureTitles, ErrorMessage = pictureControls.ErrorForPictureTitles)]
        public string pictureAlternateTitle { get; set; }

        // Picture description :
        [Column(TypeName = "text")]
        [Required]
        [Display(Name = "Description")]
        //[RegularExpression(pictureControls.PatternForPictureDescription, ErrorMessage = pictureControls.ErrorForPictureDescription)]
        public string pictureDescription { get; set; }

        // Picture url :
        [Column(TypeName = "text")]
        [Required]
        [Display(Name = "Image")]
        //[RegularExpression(pictureControls.PatternForpictureStandardUrl, ErrorMessage = pictureControls.ErrorForpictureStandardUrl)]
        public string pictureStandardUrl { get; set; }

        // Picture rating :
        [Display(Name = "Rating")]
        public short? pictureRatingValue { get; set; }

        // Picture views :
        [Display(Name = "Vues")]
        public short? pictureViewsNumber { get; set; }

        public int categoryId { get; set; }

        public virtual category category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comments> comments { get; set; }

    }

    // Static class for security checks when a picture is created :
    public static class pictureControls
    {
        // Regex & error message for the titles :
        public const string PatternForPictureTitles = @"^\d*,?\d+$";
        public const string ErrorForPictureTitles = "erreur";

        // Regex & error message for the description :
        public const string PatternForPictureDescription = @"^\d*,?\d+$";
        public const string ErrorForPictureDescription = "erreur";

        // Regex & error message for the description :
        public const string PatternForpictureStandardUrl = @"^\d*,?\d+$";
        public const string ErrorForpictureStandardUrl = "erreur";
    }
}
