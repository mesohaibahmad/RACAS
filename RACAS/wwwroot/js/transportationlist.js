
$(document).ready(function () {

    var table = $('#transportationlisttbl').DataTable({
        searching: true,
        ordering: true,
        pageLength: 10,
        lengthChange: false,
        language: {
            paginate: {
                next: '<i class="fas fa-angle-right">', // or '→'
                previous: '<i class="fas fa-angle-left">' // or '←'
            },
            search: "_INPUT_",
            searchPlaceholder: "Search..."
        },

        buttons: [
            //{
            //    extend: 'excelHtml5',
            //    text: '<i class="fas fa-download" style="padding-right:10px"></i><a>Excel</a>',
            //    titleAttr: 'Excel',
            //},
            {
                text: '<i class="fas fa-download" style="padding-right:10px"></i><a>Excel</a>',
                attr: {
                    'data-toggle': 'modal',
                    'data-target': '#searchCriteriaModal',
                    'style': 'background:inherit;'
                }
            }
        ],
        pagingType: 'simple',
        dom: "<'row justify-content-start'<'jquery-filter col-8'f><'jquery-export-btn col-1'B><'col-2'i><p>>"
    });
});

function exportExcel() {
    var excelBtn = $('.buttons-excel');

    excelBtn.click();
}

function ModelUpdateEvt(model) {
    model.EvtMessage = "test"
}
function ClickEvt(msg) {
    $('#ClickEvt').val(msg);
    console.log(msg);
    $("form").submit();
}
$('#headingOne').click(function () {
    $(this).find('i').toggleClass('fa-chevron-up fa-chevron-down')
});

function onClickBtnExcel(e) {
    var modal = $("#searchCriteriaModal");
    modal.modal('hide');
    $('#loading').show();

    var isEmail = $('#GenerateExcelModel_IsEmail').is(':checked');

    var generateExcelModel =
    {
        PodDateFrom: modal.find('#GenerateExcelModel_PodDateFrom').val(),
        PodDateTo: modal.find('#GenerateExcelModel_PodDateTo').val(),
        ReportType: parseInt(modal.find('#GenerateExcelModel_ReportType').val()),
        IsEmail: isEmail
    };
    
    $.ajax({
        url: "/TransportationList/GenerateExcel",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(generateExcelModel),
        success: function (result) {
            setTimeout(function () {
                $('#loading').hide();
                Swal.fire(
                    'Success!',
                    'Excel generated!',
                    'success'
                )
                var link = document.createElement("a");
                document.body.appendChild(link);
                link.setAttribute("type", "hidden");
                link.href = "data:text/plain;base64," + result.content;
                link.download = result.filename;
                link.click();
                document.body.removeChild(link);
            }, 2000);
        },
        error: function (result) {
            $('#loading').hide();
            alert('error!');
            console.log(result);
        }
    });
}

function onClickBtnClose(e) {
    //reset search criteria
}




