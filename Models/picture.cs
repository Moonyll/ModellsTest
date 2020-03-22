namespace Modells.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
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
        [Display(Name = "Text alt.")]
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
        public const string PatternForPictureTitles = @"^[0-9\-._/A-Za-z\u00C0-\u017F]+$";
        public const string ErrorForPictureTitles = "Veuillez renseigner un titre valide.";

        // Regex & error message for the description (blank space authorized) :
        public const string PatternForPictureDescription = @"^[ 0-9\-._/A-Za-z\u00C0-\u017F]+$";
        public const string ErrorForPictureDescription = "Veuillez renseigner une description valide.";

        // Regex & error message for the url :
        public const string PatternForpictureStandardUrl = @"^[0-9\-._/A-Za-z\u00C0-\u017F]+$";
        public const string ErrorForpictureStandardUrl = "Veuillez saisir un nom valide";

        // Upload file picture attributes limitations :
        
        // Size
        public const int pictureFileToUploadMaxSize = 7000000;
        public const string ErrorMessageForPictureOutOfSize = "La taille maximale de l'image doit être de 7 Mo.";
        
        // Extension :
        public static string[] pictureFileToUploadExtension = { "image/jpg", "image/jpeg", ".jpg",".jpeg" };
        public const string errorMessageForPictureOutOfExt = "L'image doit être au format jpg / jpeg";

        // Diretory :
        public const string pictureFileDirectory = "/Content/Images/Pictures/";
        public const string tempFileDirectory = "/Content/Images/Temp/";
        public const string ErrorMessageForPictureFileUnicity = "Une image ayant le même nom existe déjà.";
        public const string ErrorMessageForPictureEmptyUpload = "Veuillez ajouter une image.";

        // Patterns for original date & time regex :
        public const string OriginalDateFormatA = @"\d{2}-\d{2}-\d{4}";
        public const string OriginalDateFormatB = @"\d{2}:\d{2}:\d{4}";
        public const string OriginalDateFormatC = @"\d{4}-\d{2}-\d{2}";
        public const string OriginalDateFormatD = @"\d{4}:\d{2}:\d{2}";
        public const string OriginalTimeFormatA = @" \d{2}:\d{2}:\d{2}";

        // Regex for original date & time :
        public static Regex PatternOrigDtFA = new Regex(OriginalDateFormatA);
        public static Regex PatternOrigDtFB = new Regex(OriginalDateFormatB);
        public static Regex PatternOrigDtFC = new Regex(OriginalDateFormatC);
        public static Regex PatternOrigDtFD = new Regex(OriginalDateFormatD);
        public static Regex PatternOrigTmFA = new Regex(OriginalTimeFormatA);
    }

    public class pictureExifMetaData
    {

        // Constant string values to display :

        public const string EmptyValue = "---";

        public const string SpaceTabulation = "\u2003";

        public const string TabEmpty = "\u2003---";

        public const string KiloOctets = " Ko";

        public const string MegaOctets = " Mo";

        public const string Pixels = " px";

        public const string ISO = " iso";

        public const string DateLabel = "Date : ";

        public const string TimeLabel = "Heure : ";

        public const string Times = "   x   ";

        // Camera make :
        public string pictureCameraMake { get; set; }

        // Camera model :
        public string pictureCameraModel { get; set; }

        // Original date time :
        public string pictureOriginalDateTime { get; set; }
        
        // F-Number value :
        public string pictureFnumberValue { get; set; }

        // Aperture value :
        public string pictureApertureValue { get; set; }

        // Exposure time :
        public string pictureExposureTime { get; set; }

        // ISO speed ratings :
        public string pictureIsoSpeedRatings { get; set; }

        // Picture flash :
        public string pictureFlash { get; set; }

        // Focal length :
        public string pictureFocalLength { get; set; }

        // Picture width :
        public string pictureWidth { get; set; }

        // Picture height :
        public string pictureHeight { get; set; }

        // Picture dimensions:
        public string pictureDimensions { get; set; }

        // Picture file size :
        public string pictureFileSize { get; set; }

    }

    public class pictureGlobalLabels
    {
        public const string PictureMainTitle = "titre";

        public const string PictureAlTitle = "titre alternatif";

        public const string PictureDecription = "description";

        public const string AddPicture = "Ajouter image";

        public const string EditPicture = "Editer image";

        public const string DelPicture = "Supprimer image";

        public const string UpPicture = "Valider";
        
        public const string collHome = "Accueil";
        
        public const string collPicture = "Collection";
        
        public const string UpLoad = "Upload";

        public const string BackToColl = "Retour collection";

        public const string ResetForm = "Réinitialiser";

        public const string PreviewImgMTitle = "Aperçu image";

        public const string PreviewImgATitle = "Ici votre image";

        public const string DefaultPictureUrl = "/Content/Images/Pictures/upload.png";

        public const string NormLogoPictureUrl = "/Content/Images/Logos/modells.jpg";

        public const string MiniLogoPictureUrl = "/Content/Images/Logos/logo.jpg";

    }
}
