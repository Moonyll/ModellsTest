namespace Modells.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;
    using System.Web;

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
        [RegularExpression(pictureControls.PatternForPictureTitles, ErrorMessage = pictureControls.ErrorForPictureTitles)]
        public string pictureTitle { get; set; }

        // Picture alternative title :
        [Required]
        [StringLength(50)]
        [Display(Name = "Titre alternatif")]
        [RegularExpression(pictureControls.PatternForPictureTitles, ErrorMessage = pictureControls.ErrorForPictureTitles)]
        public string pictureAlternateTitle { get; set; }

        // Picture description :
        [Column(TypeName = "text")]
        [Required]
        [Display(Name = "Description")]
        [RegularExpression(pictureControls.PatternForPictureDescription, ErrorMessage = pictureControls.ErrorForPictureDescription)]
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
        public const string PatternForPictureTitles = @"^[\-/A-Za-z\u00C0-\u017F]+$";
        public const string ErrorForPictureTitles = "Veuillez renseigner un titre valide.";

        // Regex & error message for the description :
        public const string PatternForPictureDescription = @"^[\-/A-Za-z\u00C0-\u017F]+$";
        public const string ErrorForPictureDescription = "Veuillez renseigner une description valide.";

        // Regex & error message for the url :
        public const string PatternForpictureStandardUrl = @"^[\-/A-Za-z\u00C0-\u017F]+$";
        public const string ErrorForpictureStandardUrl = "erreur";

        // Upload file picture attributes limitations :
        
        // Size
        public const int pictureFileToUploadMaxSize = 7000000;
        public const string errorMessageForPictureOutOfSize = "La taille maximale de l'image doit être de 7 Mo.";
        
        // Extension :
        public static string[] pictureFileToUploadExtension = { "image/jpg", "image/jpeg" };
        public const string errorMessageForPictureOutOfExt = "L'image doit être au format jpg / jpeg";

    }

}
