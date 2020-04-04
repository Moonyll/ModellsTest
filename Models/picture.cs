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
        public const string ErrorMessageForPictureOutOfSize = "La taille maximale de l'image doit �tre de 7 Mo.";
        
        // Extension :
        public static string[] pictureFileToUploadExtension = { "image/jpg", "image/jpeg", ".jpg",".jpeg" };
        public const string errorMessageForPictureOutOfExt = "L'image doit �tre au format jpg / jpeg";

        // Diretory :
        public const string pictureFileDirectory = "/Content/Images/Pictures/";
        public const string tempFileDirectory = "/Content/Images/Temp/";
        public const string ErrorMessageForPictureFileUnicity = "Une image ayant le m�me nom existe d�j�.";
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

        public const string Success = "Success";

        public const string Successful = "Succ�s";

        public const string Update = "Update";

        public const string Updated = "Mise � jour";

        public const string Remove = "Remove";

        public const string Removed = "Suppression";

        public const string Error = "Error";

        public const string ErrorOccurred = "Erreur";

        public const string WWayError = "404Error";

        public const string WWayErrorOccurred = "Erreur 404";

        public const string SuccessMessage = "\u2003Image ajout�e avec succ�s !";

        public const string UpdateMessage = "\u2003Image mise � jour avec succ�s !";

        public const string RemoveMessage = "\u2003Image supprim�e...";

        public const string ErrorMessage = "\u2003Une erreur s'est produite lors du traitement de votre demande.";

        public const string WwayErrorMessage = "\u2003Erreur 404...";

        public const string FirstSlide = "First Slide";

        public const string SecondSlide = "Second Slide";

        public const string ThirdSlide = "Third Slide";

        public const string FirstSlideUpH1 = "\u0020Bienvenue sur Modells\u0020";
        
        public const string FirstSlideUpH2 = "\u0020Con�u pour les passionn�s de photographie\u0020";

        public const string SecondSlideUpH1 = "\u0020Mise en ligne des photos\u0020";

        public const string SecondSlideUpH2 = "\u0020Adapt�e � tous les supports\u0020";

        public const string ThirdSlideUpH1 = "\u0020Galerie photos sous forme de collection\u0020";

        public const string ThirdSlideUpH2 = "\u0020Affichage des m�tadonn�es Exifs\u0020";

        public const string FirstSlideDownH = "\u0020Le Havre\u0020";

        public const string FirstSlideDownP = "\u0020Cat�ne des 500 ans\u0020";

        public const string SecondSlideDownH = "\u0020Lanzarote\u0020";

        public const string SecondSlideDownP = "\u0020Plage de Papagyo\u0020";

        public const string ThirdSlideDownH = "\u0020Fuenteventura\u0020";

        public const string ThirdSlideDownP = "\u0020Mer et surfeur\u0020";

        public const string BackToColl = "Retour collection";

        public const string ResetForm = "R�initialiser";

        public const string PreviewImgMTitle = "Aper�u image";

        public const string PreviewImgATitle = "Ici votre image";

        public const string Blank = "_blank";

        public const string Facebook = "Facebook";
        
        public const string Instragam = "Instragam";

        public const string Twitter = "Twitter";

        public const string Pixabay = "Pixabay";

        public const string DefaultPictureUrl = "/Content/Images/Pictures/upload.png";

        public const string NormLogoPictureUrl = "/Content/Images/Logos/modells.jpg";

        public const string MiniLogoPictureUrl = "/Content/Images/Logos/logo.jpg";

        public const string MiniLogoPixabay = "/Content/Images/Logos/pixabay.png";

        public const string MiniLogoPixabayUrl = "https://pixabay.com/";

        public const string MiniLogoFacebookUrl = "https://www.facebook.com/";

        public const string MiniLogoInstagramUrl = "https://www.instagram.com/";

        public const string MiniLogoTwitterUrl = "https://twitter.com/";

        public const string AddPictureSuccessUrl = "/Content/Images/AppPics/Success.jpg";

        public const string UpdatePictureUrl = "/Content/Images/AppPics/Update.jpg";

        public const string ErrorUrl = "/Content/Images/AppPics/Error.jpg";

        public const string WWayErrorUrl = "/Content/Images/AppPics/404.jpg";

        public const string RemovePictureUrl = "/Content/Images/AppPics/Removed.jpg";

        public const string CarFirstSlide = "/Content/Images/AppPics/Arche_LH.jpg";

        public const string CarSecondSlide = "/Content/Images/AppPics/Papagyo.jpg";

        public const string CarThirdSlide = "/Content/Images/AppPics/Surfing.jpg";


    }
}
