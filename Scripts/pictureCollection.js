  $(document).ready(function () {
      $(".details").click(function () {

            // Get index of selected picture :
            var indexItem = $(".details").index(this);

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

            // Manage the previous button :
            $(".prev").click(function () {

                // Previous index :
                var prevIndex = ((indexItem - 1) <= 0) ? 0 : (indexItem - 1);

                // Display button :
                displayButton(prevIndex);

                // Previous button :
                var prevElementButton = $(".details").eq(prevIndex);

                // Previous picture elements :
                var prevPictureElements = getPictureElements(prevElementButton);

                // Previous src :
                selectedPictureFile = prevPictureElements[0];

                // Previous id :
                selectedPictureId = prevPictureElements[1];

                // Display previous picture elements :
                displayPictureElements(prevPictureElements);
                displayPictureExifs();

                // Decrement index :
                indexItem--;
            });

            // Manage the next button :
            $(".next").click(function () {

                // Next index :
                var nextIndex = ((indexItem + 1) >= 7) ? 7 : (indexItem + 1);

                // Display button :
                displayButton(nextIndex);

                // Next button :
                var nextElementButton = $(".details").eq(nextIndex);

                // Next picture elements :
                var nextPictureElements = getPictureElements(nextElementButton);

                // Next src :
                selectedPictureFile = nextPictureElements[0];

                // Next id :
                selectedPictureId = nextPictureElements[1];

                // Display next picture elements :
                displayPictureElements(nextPictureElements);
                displayPictureExifs();

                // Increment index :
                indexItem++;
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

            // Ajax call to edit the picture :
            $(".editPicture").click(function () {

                $.ajax({
                    url: urlPictureEdit,
                    type: 'GET',
                    dataType: 'html',
                    contentType: 'application/json; charset=utf-8',
                    data: { id: selectedPictureId },

                    // Id is found :
                    success: function () {

                        // Redirect to edition view page to update picture :
                        var urlEditPicture = urlPictureEdit +'/'+ selectedPictureId;
                        console.log(urlEditPicture);
                        window.location.href = urlEditPicture;
                    },

                    // Id is not found :
                    error: function () {

                        // Redirect to error view page :
                        window.location.href = urlPictureError;
                    }
                });
            });

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
          $('[data-toggle="popover"]').popover({
              html: true,
              content: function () {
                  return $('#popover-content').html();
              }
          });
      });
      //
            // New Code here:
      //
    });