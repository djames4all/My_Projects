// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// site.js: any global client JS (kept lightweight)
console.log("MunicipalServicesApp client loaded.");

$('#MediaFile').on('change', function () {
    var file = this.files && this.files[0];
    var $error = $('#fileError');

    $error.hide().text('');
    $('#imagePreview').hide();

    if (!file) return;

    var allowed = ['jpg', 'jpeg', 'png', 'gif', 'pdf'];
    var ext = file.name.split('.').pop().toLowerCase();

    if (!allowed.includes(ext)) {
        $error.text("Invalid file type. Only images and PDFs are allowed.").show();
        $(this).val('');
        return;
    }

    if (file.size > 5 * 1024 * 1024) {
        $error.text("File is too large. Maximum allowed size is 5 MB.").show();
        $(this).val('');
        return;
    }

    // Show preview if valid
    if (['jpg', 'jpeg', 'png', 'gif'].includes(ext)) {
        var reader = new FileReader();
        reader.onload = function (evt) {
            $('#previewImg').attr('src', evt.target.result);
            $('#imagePreview').show();
        };
        reader.readAsDataURL(file);
    }
});
