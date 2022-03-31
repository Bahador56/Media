function onFolderClicked(e, folderId) {
    let title = $(`#span-title_${folderId}`).text();
    $(`#span-title_${folderId}`).addClass('d-none');
    $(`#input-title_${folderId}`).removeClass('d-none');
    $(`#input-title_${folderId}`).val(title);
}
function renameFolder(e) {
    let folderTitle = $(e).val();
    let folderId = parseInt($(e).attr('data-folderid'));

    $(e).prop('disabled', true);
    let data = {
        'folderTitle': folderTitle,
        'folderId': folderId
    };

    $.ajax('/Home/RenameFolder', {
        type: 'POST',
        data: data,
        success: function (data, status, xhr) {
            if (data.isSuccess === true) {
                $(`#span-title_${folderId}`).html(folderTitle);
                $(`#span-title_${folderId}`).removeClass('d-none');
                $(`#span-title_${folderId}`).addClass('d-block');

                $(`#input-title_${folderId}`).removeClass('d-block');
                $(`#input-title_${folderId}`).addClass('d-none');
            } else {
                alert(data.message);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.error({ jqXhr, textStatus, errorMessage })
        },
        complete: function () {
            $(e).prop('disabled', false);
        }
    });


}
function getUploadPartial(folderId) {
    $('.upload-file-span').html(`<button type="button" onclick="uploadfile(${folderId})" class="btn btn-warning">Upload!</button>`);
    $('#uploadNewFileModal').modal('toggle')
}


$(document).ready(function () {
    $('.rename-folder').on('change', function () { renameFolder(this) });

})

function uploadfile(folderId) {
    debugger;
    var input = document.getElementById('fileInput');
    var files = input.files;
    var formData = new FormData();
    if (folderId !== null) {
        formData.append('folderId', parseInt(folderId));
    } else {
        formData.append('folderId', null);
    }
    for (var i = 0; i !== files.length; i++) {
        formData.append("files", files[i]);
    }

    $.ajax({
        url: "/home/UploadFile",
        type: "POST",
        dataType: "json",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data === true) {
                location.reload();
            }
        },
        error: function (data) {
            console.log(data);
        }
    })
}

