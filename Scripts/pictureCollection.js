$(document).ready(function () {

      $(".details").click(function () {

            // Get index of selected picture :
            indexItem = $(".details").index(this);

            // Get selected picture elements :
            var mainPictureElements = getPictureElements($(this));

            // Catch the picture src :
            var selectedPictureFile = mainPictureElements[0];

            // Catch the picture id :
            var selectedPictureId = mainPictureElements[1];

            // Display selected picture elements :
            displayPictureElements(mainPictureElements);
            displayPictureExifs();

            // Display button :
            displayButton(indexItem);

            // Display index to check :
            console.log(
            "index : ", indexItem,
            " id : ", selectedPictureId,
            " main : ", mainPictureElements,
            );

            // Manage the previous button :
            $(".prev").click(function () {

                // Previous index :
                indexItem = ((indexItem - 1) < 0) ? 0 : (indexItem - 1);

                // Display button :
                displayButton(indexItem);

                // Previous button :
                var prevElementButton = $(".details").eq(indexItem);

                // Previous picture elements :
                mainPictureElements = getPictureElements(prevElementButton);

                // Previous src :
                selectedPictureFile = mainPictureElements[0];

                // Previous id :
                selectedPictureId = mainPictureElements[1];

                // Display previous picture elements :
                displayPictureElements(mainPictureElements);
                displayPictureExifs();

                // Display index to check :
                console.log(
                    "index : ", indexItem,
                    " id : ", selectedPictureId,
                    " main : ", mainPictureElements,
                );
            });

            // Manage the next button :
            $(".next").click(function () {

                // Next index :
                indexItem = ((indexItem + 1) > 7) ? 7 : (indexItem + 1);

                // Display button :
                displayButton(indexItem);

                // Next button :
                var nextElementButton = $(".details").eq(indexItem);

                // Next picture elements :
                mainPictureElements = getPictureElements(nextElementButton);

                // Next src :
                selectedPictureFile = mainPictureElements[0];

                // Next id :
                selectedPictureId = mainPictureElements[1];

                // Display next picture elements :
                displayPictureElements(mainPictureElements);
                displayPictureExifs();

                // Display index to check :
                console.log(
                    "index : ", indexItem,
                    " id : ", selectedPictureId,
                    " main : ", mainPictureElements,
                );
            });

            // Get picture selected elements :
            function getPictureElements(button) {

                // Get picture source to display in the modal :
                var source = button.parent().prev().attr("src");

                // Get the selected id in the modal :
                var id = parseInt(button.prev().prev().children(".pictureId").text());

                // Get the selected title image in the modal :
                var title = button.prev().prev().children(".pictureTitle").text();

                // Get the selected alt. title image in the modal :
                var alternate = button.parent().prev().attr("title");

                 // Get the selected description image in the modal :
                var description = button.prev().prev().children(".pictureDescription").text();

                // Get the selected category image in the modal :
                var category = button.prev().prev().children(".pictureCategory").text();

                    return pictureElements = new Array(source, id, title, alternate, description, category);
            }

            // Display selected elements :
            function displayPictureElements(pictureElements) {

                // Display the selected image in the modal :
                $(".displayPicture").attr("src", pictureElements[0]);

                // Display the selected id in the modal :
                $("#pictureId").text(pictureElements[1]);

                // Display the selected image title in the modal :
                $("#pictureTitle").text(pictureElements[2]);

                // Display the alt. title in the modal :
                $(".displayPicture").attr("title", pictureElements[3]);

                // Display the selected image description in the modal :
                $("#pictureDescription").text(pictureElements[4]);

                // Display the selected image category in the modal :
                $("#pictureCategory").text(pictureElements[5]);
            }

             // Display prev / next buttons :
             function displayButton(buttonIndex) {

                // Hide prev button on first picture :
                (buttonIndex == 0) ? $(".prev").hide() : $(".prev").show();

                // Hide next button on last picture :
                (buttonIndex == 7) ? $(".next").hide() : $(".next").show();
            }

             // Display picture exifs with an ajax call :
            function displayPictureExifs() {

                $.ajax({
                    url: urlPictureExifs,
                    type: 'GET',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: { pictureFile: selectedPictureFile },

                    // src is found :
                    success: function (result) {

                        // Redirect to edition view page to update picture :
                        $("#pictureCameraMake").text(result.pictureCameraMake);
                        $("#pictureCameraModel").text(result.pictureCameraModel);
                        $("#pictureOriginalDateTime").text(result.pictureOriginalDateTime);
                        $("#pictureApertureValue").text(result.pictureApertureValue);
                        $("#pictureExposureTime").text(result.pictureExposureTime);
                        $("#pictureIsoSpeedRatings").text(result.pictureIsoSpeedRatings);
                        $("#pictureFocalLength").text(result.pictureFocalLength);
                        $("#pictureFlash").text(result.pictureFlash);
                        $("#pictureDimensions").text(result.pictureDimensions);
                        $("#pictureFileSize").text(result.pictureFileSize);

                    },

                    // src is not found :
                    error: function () {

                        // Redirect to error view page :
                        window.location.href = urlPictureError;
                    }
                });

            }
        });

      // Display exifs data for the selected picture :
      $(function () {
          $(".exifsBlue").popover({
              html: true,
              content: function () {
                  return $('#popover-content').html();
              }
          });
      });

      // Display exifs help legend :
      $(function () {
          $(".exifsGreen").popover({
              html: true,
              content: function () {
                  return $('#popover-content-legend').html();
              }
          });
      });
       //
            // If needed - new Code here:
      //
  });

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

// Tip : sanitize: false, => sanitize property for security reasons !

// Picture Edition Function :
function pictureToEdit() {

    // Get the selected picture Id to edit :
    var selectedPictureId = $("#pictureId").text();

    // Ajax call to edit the picture :
    $.ajax({
        url: urlPictureEdit,
        type: 'GET',
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        data: { id: selectedPictureId },

        // Id is found :
        success: function () {

            // Redirect to edition view page to update picture :
            var urlEditPicture = urlPictureEdit + '/' + selectedPictureId;
            console.log(urlEditPicture);
            window.location.href = urlEditPicture;
        },

        // Id is not found :
        error: function () {

            // Redirect to error view page :
            window.location.href = urlPictureError;
        }
    });
}

// Picture Removing Function :
function pictureToDelete() {

    // Get the selected picture Id to edit :
    var selectedPictureId = $("#pictureId").text();

    // Ajax call to delete the picture :
    $.ajax({
        url: urlPictureDelete,
        type: 'GET',
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        data: { id: selectedPictureId },

        // Id is found :
        success: function () {

            // Redirect to edition view page to update picture :
            console.log(urlPictureDelete);
            window.location.href = urlPictureDeleteOk;
        },

        // Id is not found :
        error: function () {

            // Redirect to error view page :
            window.location.href = urlPictureError;
        }
    });
}