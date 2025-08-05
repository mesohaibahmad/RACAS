
var objCommon = new Common();

function GetReconciliationReport($event) {

    var data = $('#formReport').serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});

    if (data.Country == "" || data.Country == null) {
        objCommon.ShowMessage("Please select country first.", "error");
        return;
    }
    if (data.DateFrom == "" || data.DateFrom == null || data.DateTo == "" || data.DateTo == null) {
        objCommon.ShowMessage("Please select date range first.", "error");
        return;
    }


    $("#dashboardlisttbl tbody").html('');
    var html = "";
    objCommon.AjaxCall("../api/admin/GetReconciliationReport", JSON.stringify(data), "POST", true, function (d) {
        if (d != null) {
            for (var i = 0; i < d.length; i++) {
                var v = d[i];
                html += "<tr><td>" + v.reportDateTime+"</td>";
                html += "<td>" + v.companyCode +"</td>";
                html += "<td>" + v.collectionCode +"</td>";
                html += "<td>" + v.payerCodeName +"</td>";
                html += "<td>" + v.amount +"</td>";
                html += "<td>" + v.paymentMethod + "</td>";
                html += "<td>" + v.billingDocument + "</td>";
                html += "<td style='width:100px'><a class='btn-sm btn-danger btnDetail' data-refid='" + v.collectionCode +"'><i class='fa fa-file-excel'></i></a> / ";
                html += "<a class='btn-sm btn-danger btnSummary ml-2' data-refid='" + v.collectionCode +"'><i class='fa fa-file-excel'></i></a></td></tr> ";
            };
            $("#dashboardlisttbl tbody").html(html);
        }
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


        $('#dashboardlisttbl').on('page.dt', function () {
            console.log("a");
            RenderEvents();
        });
        RenderEvents();
     
        $("#filterModel").modal("hide");

    }, $event.currentTarget);
}

function RenderEvents() {
   // $(document).live("a.btnDetail", "click", function (e) {
    $(document).on("click", "a.btnDetail", function () {
        var RefId = $(this).data("refid");

        var data = {
            RefId: RefId
        }

        objCommon.AjaxCall("../api/admin/GenerateDetailReport", $.param(data), "GET", true, function (d) {
            window.location.href = objCommon.baseUrl + d.FileUrl;
        });
    })

    //$(document).live("a.btnSummary", "click", function (e) {
    $(document).on("click", "a.btnSummary", function () {
        var RefId = $(this).data("refid");

        var data = {
            RefId: RefId
        }

        objCommon.AjaxCall("../api/admin/GenerateSummaryReport", $.param(data), "GET", true, function (d) {
            window.location.href = objCommon.baseUrl + d.FileUrl;
        });
    })
}

function FilterModel() {
    $("#filterModel").modal("show");
}

var dataObject = [];

$(function () {
    

    var data = {

    }

    $("#ddlCountry").html('');
    $("#ddlBusinessUnit").html('');
    $("#ddlCompanyCode").html('');
    $("#ddlPaymentMethod").html('');
    $("#ddlLocation").html('');

    objCommon.AjaxCall("../Admin/GetFilterData", JSON.stringify(data), "POST", true, function (d) {
        dataObject = d;
        if (d != null) {
            $("#ddlBusinessUnit").append('<option value="">All</option>');
            $("#ddlCountry").append('<option value="">All</option>');
            $("#ddlCompanyCode").append('<option value="">All</option>');
            $("#ddlLocation").append('<option value="">All</option>');
            //$("#ddlPaymentMethod").append('<option value="0">All</option>');

            $.each(d.businessList, function (index, v) {
                $("#ddlBusinessUnit").append('<option value="' + v.buCode + '">' + v.buName + '</option>');
            });
            $.each(d.countryList, function (index, v) {
                $("#ddlCountry").append('<option value="' + v.countryCode + '">' + v.name + '</option>');
            });
            $.each(d.comanyList, function (index, v) {
                $("#ddlCompanyCode").append('<option value="' + v.companyCode + '">' + v.companyName + '</option>');
            });
            $.each(d.locationList, function (index, v) {
                $("#ddlLocation").append('<option value="' + v.id + '">' + v.locationName + '</option>');
            });
            $.each(d.paymentModeList, function (index, v) {
                $("#ddlPaymentMethod").append('<option value="' + v.paymentTypeCode + '">' + v.paymentType + '</option>');
            });
        }
        //$scope.BusinessUnitList = d.BusinessUnits;
        //$scope.CountryList = d.CountryList;
        //$scope.CompanyList = d.CompanyList;
        //$scope.PaymentModeList = d.PaymentModes;
        //$scope.LocationList = d.LocationList;
    });


    $("#ddlCountry").change(function () {
        var _CountryCode = $(this).val();
        $("#ddlLocation").html('');
        $("#ddlLocation").append('<option value="0">All</option>');
        $.each(dataObject.locationList, function (index, v) {
            if (v.countryCode == _CountryCode || _CountryCode =='0') {
                $("#ddlLocation").append('<option value="' + v.id + '">' + v.locationName + '</option>');
            }
        });
    });

    $("#ddlBusinessUnit").change(function () {
        $("#ddlPaymentMethod").html('');
        var _BUCode = $(this).val();
        $.each(dataObject.paymentModeList, function (index, v) {
            if (v.buCode == _BUCode || _BUCode == '0') {
                $("#ddlPaymentMethod").append('<option value="' + v.paymentTypeCode + '">' + v.paymentType + '</option>');
            }
        });
    })
})

