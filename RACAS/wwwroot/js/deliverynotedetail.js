function DeleteUploadedFile(selectedFile, dnId) {
    //existing file - call ajax to delete
    if ($(selectedFile).parents(".uploaded_file").find('.is-existing').length > 0) {
        Swal.fire({
            title: 'Are you sure?',
            text: "This will remove previously uploaded file.",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.value) {
                $('#loading').show();

                var data;
                var isGRN = $(selectedFile).parents(".uploaded_file").find('.is-grn').length > 0;
                if (isGRN) {
                    var fileUrl = $(selectedFile).parents(".uploaded_file").find('.is-grn').attr('href');
                    data = {
                        id: dnId,
                        files: {
                            grn: [fileUrl.substr(fileUrl.lastIndexOf('/') + 1)],
                            poison: []
                        }
                    };
                }

                var isPoison = $(selectedFile).parents(".uploaded_file").find('.is-poison').length > 0;
                if (isPoison) {
                    var fileUrl = $(selectedFile).parents(".uploaded_file").find('.is-poison').attr('href');

                    data = {
                        id: dnId,
                        files: {
                            grn: [],
                            poison: [fileUrl.substr(fileUrl.lastIndexOf('/') + 1)],
                        }
                    };
                }

                var webAPI = decodeURIComponent(readCookie('web_api_host'));

                $.ajax({
                    url: webAPI + "/api/DeliveryNotes/DeleteDocs",
                    headers: {
                        'Authorization': 'Bearer ' + readCookie('auth_cookie'),
                    },
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(data),
                    success: function (result) {
                        $(selectedFile).parents(".uploaded_file").remove();
                        Swal.fire(
                            'Success!',
                            'File deleted!',
                            'success'
                        );
                        $('#loading').hide();

                    },
                    error: function (result) {
                        Swal.fire(
                            'Oops!',
                            'Something went wrong!',
                            'error'
                        );
                        $('#loading').hide();
                        console.log(result);
                    }
                });
            }
        });
    }
    else {// new file
        $(selectedFile).parents(".uploaded_file").remove();
        $('#UploadFileBtn').val('');
    }

    return false;
}

async function OnFileUploaded() {
    //$("#uploaded_files").empty();
    var fileInput = document.getElementById("UploadFileBtn");
    var files = fileInput.files;
    var file;

    const toBase64 = file => new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });

    var uploadedFileList = [];
    var uploadedFileJson = '{}';

    var uploadedFileEle = '';
    var fileExt = '';
    for (var i = 0; i < files.length; i++) {
        var uploadedFileJsonObj = JSON.parse(uploadedFileJson);
        file = files[i];

        const blobUrl = URL.createObjectURL(file);
        var base64 = await toBase64(file);
        console.log(base64);

        fileExt = file.name.substr(file.name.length - 4);
        console.log(fileExt);

        switch (fileExt) {
            case ".pdf":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><img src = "images/icon_pdf.png" /><a class="uploaded-file-class" data=' + base64 + ' href=' + blobUrl + ' download="' + file.name + '">' + file.name + '</a></div><div class="col-2 btn-delete"><img src="images/icon_trash.png" onclick="DeleteUploadedFile(this)" /></div></div></div>';
                break;
            case ".xls":
            case ".xlsx":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><i class="fas fa-file-excel"></i><a class="uploaded-file-class" data=' + base64 + '  href=' + blobUrl + ' download="' + file.name + '">' + file.name + '</a></div><div class="col-2 btn-delete"><img src="images/icon_trash.png" onclick="DeleteUploadedFile(this)" /></div></div></div>';
                break;
            case ".ppt":
            case ".pptx":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><i class="fas fa-file-powerpoint"></i><a class="uploaded-file-class" data=' + base64 + '  href=' + blobUrl + ' download="' + file.name + '">' + file.name + '</a></div><div class="col-2 btn-delete"><img src="images/icon_trash.png" onclick="DeleteUploadedFile(this)" /></div></div></div>';
                break;
            case ".doc":
            case ".docx":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><i class="fas fa-file-word"></i><a class="uploaded-file-class" data=' + base64 + '  href=' + blobUrl + ' download="' + file.name + '">' + file.name + '</a></div><div class="col-2 btn-delete"><img src="images/icon_trash.png" onclick="DeleteUploadedFile(this)" /></div></div></div>';
                break;
            default:
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><img src = "images/icon_photo.png" /><a class="uploaded-file-class" data=' + base64 + '  href=' + blobUrl + ' download="' + file.name + '">' + file.name + '</a></div><div class="col-2 btn-delete"><img src="images/icon_trash.png" onclick="DeleteUploadedFile(this)" /></div></div></div>';
                break
        }

        $("#uploaded_files").append(uploadedFileEle);
    }

}

async function CreateUploadedFile(uploadedFileListJson) {

    var uploadedFileList = [];
    uploadedFileList = JSON.parse(uploadedFileListJson);;
    console.log(uploadedFileList);

    for (var i = 0; i < uploadedFileList.length; i++) {


        fileName = uploadedFileList[i].FileName;
        fileContent = uploadedFileList[i].FileContent;
        //const base64Response = await fetch(fileContent);
        //const blobUrl = URL.createObjectURL(await base64Response.blob());
        var blobUrl = uploadedFileList[i].FileUrl;

        var uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><i class="fas fa-file-pdf"></i><a class="uploaded-file-class" href=' + blobUrl + ' download="' + fileName + '">' + fileName + '</a></div><div class="col-2 btn-delete"></div></div></div>';
        fileExt = fileName.substr(fileName.length - 4);
        console.log(fileExt);

        switch (fileExt) {
            case ".pdf":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><img src = "images/icon_pdf.png" /><a class="uploaded-file-class" href=' + blobUrl + ' download="' + fileName + '">' + fileName + '</a></div><div class="col-2 btn-delete"></div></div></div>';
                break;
            case ".xls":
            case ".xlsx":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><i class="fas fa-file-excel"></i><a class="uploaded-file-class" href=' + blobUrl + ' download="' + fileName + '">' + fileName + '</a></div><div class="col-2 btn-delete"></div></div></div>';
                break;
            case ".ppt":
            case ".pptx":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><i class="fas fa-file-powerpoint"></i><a class="uploaded-file-class" href=' + blobUrl + ' download="' + fileName + '">' + fileName + '</a></div><div class="col-2 btn-delete"></div></div></div>';
                break;
            case ".doc":
            case ".docx":
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><i class="fas fa-file-word"></i><a class="uploaded-file-class" href=' + blobUrl + ' download="' + fileName + '">' + fileName + '</a></div><div class="col-2 btn-delete"></div></div></div>';
                break;
            default:
                uploadedFileEle = '<div class="col-5 uploaded_file"><div class="row"><div class="col-10 uploaded_file_text"><img src = "images/icon_photo.png" /><a class="uploaded-file-class" href=' + blobUrl + ' download="' + fileName + '">' + fileName + '</a></div><div class="col-2 btn-delete"></div></div></div>';
                break
        }

        $("#uploaded_files").append(uploadedFileEle);
    }

}

function UploadFile() {
    $('#UploadFileBtn').click();
};

function ClickEvt(evtType, evtMsg) {
    $('#EvtTypeHid').val(evtType);
    $('#EvtMsgHid').val(evtMsg);
    $("form").submit();
}

function DownloadFile() {
    window.location = '/DeliveryOrderDetail/DownloadFile';
};

function LoadUploadedFiles() {
    $(document).ready(function () {
        var uploadedFileJsonStr = $('#UploadedFileJsonStrHid').val();
        if (uploadedFileJsonStr != "")
            CreateUploadedFile(uploadedFileJsonStr);

    });
};


LoadUploadedFiles();

function OnReceivedQuantityInputChange(selElement) {

    //var deliveredQty = $(selElement).parents().prev().children('.delivered-qty').text();
    var deliveredQty = $(selElement).parent().prev().val();
    var receivedQty = $(selElement).val();
    if (receivedQty == '') {
        receivedQty = $(selElement).text();
    }
    var itemRemarkSection = $(selElement).parents().next('.item-remark-section');
    var selectedCard = $(selElement).parents().prev('.card-header').next();
    // var selectedCard = $(selElement).parents().prev('.card-body');

    console.log(deliveredQty);
    console.log(receivedQty);


    if (parseInt(deliveredQty) === parseInt(receivedQty)) {
        $(selectedCard).css('background-color', 'white');
        $(itemRemarkSection).hide("slow");
    } else {
        var isReadOnly = getParameterByName("isReadOnly");
        if (isReadOnly != 'True') {
            $(selectedCard).css('background-color', '#fcdf03');
        }
        $(itemRemarkSection).show("slow");
    }


}

function LoadItemRemark() {
    var itemCards = $('#do_item_detail').children().find('.received-qty');
    var receivedQtyEle;
    for (let i = 0; i < itemCards.length; i++) {
        receivedQtyEle = $(itemCards[i]);
        console.log($(receivedQtyEle).text());
        OnReceivedQuantityInputChange(receivedQtyEle)
    }
}

LoadItemRemark();

function BackToList() {
    window.location.href = "TransportationList";
}

//$(".main-header").find("#page_title").append('<i class="fas fa-arrow-left" onclick="BackToList();"></i>');
//$(".main-header").find("#page_title").children().text('   @(ViewBag.Title)');


function TransporterModalUpdate(transporterOpt, oriValue) {
    console.log($(transporterOpt).val());

    var selectedValue = $(transporterOpt).find(":selected").text();
    if (selectedValue != '') {
        $('#transporterId').first().text(selectedValue);
    }
    else {
        $('#transporterId').first().text(oriValue);
    }

};

async function Save() {
    //var saveConfirm = confirm("Are you sure to submit the delivery note?")
    //if (!saveConfirm)
    //    return false;
    Swal.fire({
        title: 'Are you sure?',
        text: "This will submit the delivery note.",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {


            var selectedFilesEle = $('#uploaded_files').children('.uploaded_file');
            var uploadedFileList = [];
            var uploadedFileJson = '{}';

            if (selectedFilesEle.length == 0) {

                $("form").submit();
            }

            for (let i = 0; i < selectedFilesEle.length; i++) {
                var uploadedFileJsonObj = JSON.parse(uploadedFileJson);
                var uploadedFile = $(selectedFilesEle[i]).find('.uploaded-file-class');
                var blobUrl = uploadedFile.attr('href');

                //let blob = await fetch(blobUrl).then(r => r.blob());
                var uploadedFileName = uploadedFile.text();
                //var uploadedFileBase64Str = await toBase64(blob);

                uploadedFileJsonObj.FileUrl = blobUrl;
                uploadedFileJsonObj.FileName = uploadedFileName;

                //hardcode to grn first
                uploadedFileJsonObj.IsGRN = true;
                //if ($(selectedFilesEle[i]).find('.is-grn').length > 0) {
                //    uploadedFileJsonObj.IsGRN = true;
                //}

                //if ($(selectedFilesEle[i]).find('.is-poison').length > 0) {
                //    uploadedFileJsonObj.IsPoison = true;
                //}

                if ($(selectedFilesEle[i]).find('.is-existing').length > 0) {
                    uploadedFileJsonObj.IsExisting = true;
                }

                if (uploadedFile.attr('data'))
                    uploadedFileJsonObj.FileContent = uploadedFile.attr('data').substr(uploadedFile.attr('data').lastIndexOf(',') + 1);

                uploadedFileList.push(uploadedFileJsonObj);
                console.log(uploadedFileName);
            }
            var uploadedFileJsonStr = JSON.stringify(uploadedFileList);
            $('#UploadedFileJsonStrHid').val(uploadedFileJsonStr);

            console.log(JSON.stringify(uploadedFileList));
            $("form").submit();
            $('#loading').show(); // view will redirect after successfully save.
        }
    });
}

const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});

function getParameterByName(name) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(window.location.href);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
