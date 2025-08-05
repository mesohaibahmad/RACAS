
$(document).ready(function () {

    var table = $('#dashboardlisttbl').DataTable({
        searching: true,
        ordering: true,
        pageLength: 10,
        lengthChange: false,
        language: {
            paginate: {
                next: '<i class="fas fa-angle-right">', // or '→'
                previous: '<i class="fas fa-angle-left">' // or '←'
            }, search: "_INPUT_",
            searchPlaceholder: "Search..."
        },
        buttons: [
            {
                extend: 'excelHtml5',
                text: '<i class="fas fa-download" style="padding-right:10px"></i><a>Excel</a>',
                titleAttr: 'Excel'
            }],
        pagingType: 'simple',
        dom: "<'row justify-content-start'<'jquery-filter col-8'f><'jquery-export-btn col-1'B><'col-2'i><p>>"
    });

    $("#ddlMonths").on("change", function () {
        $("form").submit();
    });
});

function DateChanged() {

    $("form").submit();
}

function ClickEvt(msg) {
    $('#ClickEvt').val(msg);
    console.log(msg);
    $("form").submit();
}

