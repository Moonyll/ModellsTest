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

    // Input value :
    var inputValue = input.value;

    // Input label entity :
    var inputEntity = input.name;

    // Input pattern to match (regex) :
    var inputPattern = (inputEntity == 'pictureDescription') ?
                       new RegExp('^[ 0-9\-._/A-Za-z\u00C0-\u017F]+$') :
                       new RegExp('^[0-9\-._/A-Za-z\u00C0-\u017F]+$');

    // Result from the test of input value with pattern :
    var inputControl = inputPattern.test(inputValue);

    if (inputControl) {
        $(input).addClass('is-valid');
        $(input).prev('label').css('color', 'green');
    }
    else {
        $(input).addClass('is-invalid');
        $(input).prev('label').css('color', 'red');
        $(input).prev('label').attr({ 'data-toggle': 'tooltip', 'data-placement': 'left', 'title': 'Caractères spéciaux autorisés . - _ ' });
        //$('label.tooltip').css(color, 'red');
        $(input).prev('label').tooltip('show');

        // Tests in console :
        console.log(test);
    }
}

// Clear form from validations tips :
function clearCss(input) {

    $(input).removeClass('is-valid');
    $(input).removeClass('is-invalid');
    $(input).prev('label').tooltip('hide');
    $(input).next('[data-toggle="popover"]').popover('hide');

}

// Enable the popover elements in the view :
$(function () {
    $('[data-toggle="popover"]').popover();
})

$(function ValidateForm() {

    $('[data-toggle="popover"]').popover('toggle');

})