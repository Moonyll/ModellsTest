$(document).ready(function () {

    // Hide uploaded picture box :
    $("#pictureChoice").removeClass();

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