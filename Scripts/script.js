$(document).ready(function () {

    // Reset form & refresh page :
    $('.resetForm').click(function (e) {

        e.preventDefault();
        // Clear form :
        $("form")[0].reset();
        // Refresh page (relocate) :
        window.location.href = window.location.href;
    });

    // Enable the popover elements in the view :
    $(function() {
        $('[data-toggle="popover"]').popover();
    });

    $('#submitPicture').click(function () {

        $('[data-toggle="popover"]').popover('toggle');
    });

    $('#addPic').click(function () {

        $('#newPictureToUpload').click();
    });

});

var integratePicture = function (event) {

    // Capture event of input :
    var input = event.target;

    // Declare new file reader object :
    var pictureReader = new FileReader();

    // Get complete path name of the uploaded picture :
    var pictureFileEntireName = $('input[type=file]').val();

    // Get the short name of the uploaded picture by removing prefixes "C:\fakepath" or "C:\fake_path" :
    var pictureFileShortName = pictureFileEntireName.split('\\').pop();

    // Set Image label with picture short name value :
    $(".pictureStandardUrl").val(pictureFileShortName);

    // On load function :
    pictureReader.onload = function () {

        // Read url of choosen picture :
        var pictureChoiceURL = pictureReader.result;

        // Set pictureChoice source with previous url :
        $("#pictureChoice").attr('src', pictureChoiceURL);
        $("#pictureChoice").addClass('img-fluid img-thumbnail d-block');
    };

    // Display choosen picture :
    pictureReader.readAsDataURL(input.files[0]);
};

// Client-side validations :
function isValid(input) {

    // Input value :
    var inputValue = input.value;
    var inputLength = inputValue.length;

    // Remove spaces :
    var removeBlank = inputValue.trim();
    var removeBlankLength = removeBlank.length;

    // Input label entity :
    var inputEntity = input.name;

    // Input pattern to match (regex) :
    var inputPattern = (inputEntity == 'pictureDescription') ?
                       new RegExp('^[ 0-9\-._/A-Za-z\u00C0-\u017F]+$') :
                       new RegExp('^[0-9\-._/A-Za-z\u00C0-\u017F]+$');

    // Result from the test of input value with pattern :
    var inputControl = inputPattern.test(inputValue);

    // Check extenions value for picture file name :
    var inputExtension = inputValue.endsWith('.jpg') || inputValue.endsWith('.jpeg');

    inputControl = (inputEntity == 'pictureStandardUrl') ? (inputControl && inputExtension) : inputControl;

    // Display validations :
    if (inputControl && inputLength > 2 && removeBlankLength != 0) {
        $(input).addClass('is-valid');
        $(input).removeClass('currentInput');
        $(input).addClass('validInput');
        $(input).parent().next('.col-11').removeClass('visibleErrorMessage');
        $(input).parent().next('.col-11').addClass('hiddenErrorMessage');

        console.log($(input).parent().next('.col-11'));
    }
    else if (!inputControl && inputLength > 2) {
        $(input).addClass('is-invalid');
        $(input).removeClass('currentInput');
        $(input).addClass('invalidInput');
        $(input).prev('label').attr({ 'data-toggle': 'tooltip', 'data-placement': 'left', 'title': 'Autorisés [ . - _ ] seulement' });
        $(input).prev('label').tooltip('show');
    }
    else if (inputEntity == 'categoryId')
    {
        $(input).addClass('is-valid');
        $(input).removeClass('currentInput');
        $(input).addClass('validInput');
    }
}

// Clear form from validations tips :
function clearCss(input) {

    if ($(input).hasClass('is-invalid'))
    {
        $(input).val("");
    }
    $(input).removeClass('is-valid');
    $(input).removeClass('is-invalid');
    $(input).removeClass('validInput');
    $(input).removeClass('invalidInput');
    $(input).addClass('currentInput');
    $(input).prev('label').tooltip('hide');
    $(input).next('[data-toggle="popover"]').popover('hide');

}

// Clean The Temp Directory :
function cleanTempDir() {

    $.ajax({
        url: urlCleanTempDir,
        // Temp Dir is cleaned :
        success: function () {

            // Redirect to edition view page to update picture :
            console.log("Temp Directory is now cleaned");
        },

        // Error :
        error: function () {

            // Redirect to error view page :
            console.log("Temp Directory is not cleaned");
        }
    });
}

// Show the button when the user scrolls down 50px :
window.onscroll = function () { scrollFunction() };

function scrollFunction() {

    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        $("#topButton").css("display", "block");
    }
    else {
        $("#topButton").css("display", "none");
    }
}

// Scrolls to the top of the document :
function topFunction() {

    document.body.scrollTop = 0;

    document.documentElement.scrollTop = 0;
}