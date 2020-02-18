$(document).ready(function () {

    // Reset form & refresh page :
    $('.resetForm').click(function (e) {

        e.preventDefault();
        // Clear form :
        $("form")[0].reset();
        // Refresh page (relocate) :
        window.location.href = window.location.href;
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
    var inputValue = input.value;
    console.log(input.class);

    if (inputValue == 'toto') {
        $(input).addClass('is-valid');
    }
    else {
        $(input).addClass('is-invalid');
    }
}

// Clear form from validations tips :
function clearCss(input) {

    $(input).removeClass('is-valid');
    $(input).removeClass('is-invalid');
    $(input).next('[data-toggle="popover"]').popover('hide');

}

// Enable the popover elements in the view :
$(function () {
    $('[data-toggle="popover"]').popover();
})

$(function ValidateForm() {

    $('[data-toggle="popover"]').popover('toggle');

})