
$(document).ready(function () {

    var app = new Vue({
        el: '#appPay',
        data: {
            mainLedger: {
                Id: '0',
                ContractNumber: '',
                PartnerId: '',
                UserId: '',
                BranchId: '',
                CausedById: '',
                DescriptionId: '',
                Comment: '',
                CostIncuredDate: '',
                InvoiceNumber: '',
                Amount: '',
                OrderedById: 0,
                PaymentType: '',
                IsSaved: false,
                IsSubmitted: false,
                IsControlCheck: false,
                IsApproved: false,
                IsRejected: false
            },
            exactLength: 7,
            inputError: null,
            RequestList: [],
            CheckedData: [],
            TotalIncome: 0,
            TotalExpense: 0,
            AllCheck: false,
            BranchList: BranchList,
            PartnerList: PartnerList,
            UserList: UserList,
            UserBranchList: UserBranchList,
            currDate: new Date().toISOString().split('T')[0],
            UserName: UserName,
            IdsSelected: [],
            common: {
                EventId: 0,
                EventType: "",
                EventBy: 0,
                EventDescription: "",
                EventDateTime: new Date().toISOString().split('T')[0],
                InvoiceNumber: ''
            }
        },
        mounted() {
            this.LoadAllRequest(null);
         
        },
        computed: {
            filteredPartners() {
                const selectedBranchId = this.mainLedger.BranchId;
                var dataReturn = this.PartnerList.filter(partner =>
                    this.UserBranchList.some(userBranch =>
                        userBranch.branchId === selectedBranchId && userBranch.recordType === 'Partner' && userBranch.userId === partner.id
                    )
                );
    
                return dataReturn;
            },
            filteredUsers() {
                const selectedBranchId = this.mainLedger.BranchId;

                return this.UserList.filter(user =>
                    this.UserBranchList.some(userBranch =>
                        userBranch.branchId === selectedBranchId && userBranch.recordType === 'User' && userBranch.userId === user.userTypeId
                    )
                );
            },
        },
        methods: {
            LoadAllRequest($event) {
                let dthis = this;

                var param = {
                    SearchText: null, SortColumn: "", SortOrder: "", PageIndex: 1, PageSize: Number($("#txtPageSize").val()),
                    RecordStatus: $("#ddlStatus").val(), SubmittedById: Number($("#ddlSubmittedBy").val())
                    , ControlCheckById: Number($("#ddlControlCheckBy").val()), ApprovedById: Number($("#ddlAprovedBy").val()),
                    PaymentType: $("#ddlPaymentType").val(), PartnerId: Number($("#ddlPartner").val()),
                    InvoiceNumber: $("#txtInvoiceNumber").val()
                };

                objCommon.AjaxCall("/payments/AllPaymentRequests", $.param(param), "GET", true, function (d) {
                    console.log(d);
                    dthis.RequestList = d;

                    if (d != null) {
                        if (d.length > 0) {
                            dthis.TotalIncome = d[0].TotalIncome;
                            dthis.TotalExpense = d[0].TotalExpense;
                        }
                    }


                }, $event == null ? "" : $event.currentTarget)

            },
            add() {
                this.mainLedger = {
                    Id: '0',
                    ContractNumber: '',
                    PartnerId: '',
                    UserId: '',
                    BranchId: '',
                    CausedById: '',
                    DescriptionId: '',
                    Comment: '',
                    CostIncuredDate: '',
                    InvoiceNumber: '',
                    Amount: '',
                    OrderedById: 0,
                    RecordStatus: '',
                    PaymentType: '',
                    IsSaved: false,
                    IsSubmitted: false,
                    IsControlCheck: false,
                    IsApproved: false,
                    IsRejected:false

                }
                $("#paymentModal").modal("show");
            },

            formatCurrency(value) {
                return new Intl.NumberFormat('de-DE', {
                    style: 'currency',
                    currency: 'EUR',
                }).format(value);
            },
            insert($event, Type) {

                if (!objCommon.Validate("#paymentForm"))
                    return;
                var dthis = this;
                var type = Type;

                this.mainLedger.Id = Number(this.mainLedger.Id);
                this.mainLedger.InvoiceNumber = this.mainLedger.InvoiceNumber;
                this.mainLedger.Amount = Number(this.mainLedger.Amount);
                this.mainLedger.PartnerId = Number(this.mainLedger.PartnerId);
                this.mainLedger.UserId = Number(this.mainLedger.UserId);
                this.mainLedger.BranchId = Number(this.mainLedger.BranchId);
                this.mainLedger.CausedById = Number(this.mainLedger.CausedById);
                this.mainLedger.DescriptionId = Number(this.mainLedger.DescriptionId);
                // this.mainLedger.OrderedById = Number(this.mainLedger.OrderedById);
                this.mainLedger.BranchId = Number(this.mainLedger.BranchId);
                this.mainLedger.PaymentType = this.mainLedger.PaymentType;
                this.mainLedger.Comment = this.mainLedger.Comment;
                if (type == 'Save') {
                    this.mainLedger.IsSaved = true;
                    this.mainLedger.RecordStatus = "New";
                    this.mainLedger.IsSubmitted = false;
                }
                else if (type == 'Submmit') {
                    this.mainLedger.IsSaved = false;
                    this.mainLedger.IsSubmitted = true;
                    this.mainLedger.RecordStatus = "Pending";
                }

                console.log(this.mainLedger);

                let param = {
                    ...this.mainLedger
                };

                objCommon.AjaxCall("/payments/insertPayment", JSON.stringify(param), "POST", true, function (Id) {


                    Swal.fire({
                        title: "Success!",
                        text: "Data has been saved successfully!",
                        icon: "success"
                    });
                    // dthis.CommonModel(Id, type, dthis.UserName, dthis.currDate)
                    window.location.reload();
                    $("#paymentModal").modal("hide");
                    // $("#commonModel").modal("show");



                }, $event.currentTarget)


            },
            InsertEvent($event) {



                dthis = this;
                this.IdsSelected = [];

                const ActionType = $("#ddlAction").val();
                if (ActionType === "") {
                    objCommon.ShowMessage("please select action type.", "error");
                    return;
                }
                this.RequestList.forEach((v) => {
                    if (v.IsChecked) {
                        this.IdsSelected.push(Number(v.Id)); // Correct assignment
                    }
                });

                if (this.IdsSelected.length === 0) {
                    objCommon.ShowMessage("please select row first.", "error");
                    return;
                }
                Swal.fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes, " + ActionType + " it!"
                }).then((result) => {
                    if (result.isConfirmed) {


                        const param = {
                            Ids: this.IdsSelected,
                            ...this.common
                        };
                        console.log()
                        objCommon.AjaxCall("/payments/MultiRowsAction", JSON.stringify(param), "POST", true, function (response) {

                            if (response == "success") {

                                dthis.IdsSelected = [];
                                Swal.fire({
                                    title: "Success!",
                                    text: "Data has been saved successfully!",
                                    icon: "success"
                                });
                                $("#commonModel").modal("hide");

                                window.location.reload();
                            }
                            else {
                                dthis.IdsSelected = [];
                                Swal.fire({
                                    title: "Error!",
                                    text: response,
                                    icon: "error"
                                });
                            }
                        }, $event.currentTarget);
                    }
                })


            },
            validateCommonModal() {
                return this.common.EventDateTime && this.common.EventBy && this.common.EventDescription;
            },

            edit(mainLedger) {

                const formattedDate = dayjs(mainLedger.EntryDate).format('YYYY-MM-DD');
                mainLedger.EntryDate = formattedDate;
                const CostIncuredDate = dayjs(mainLedger.CostIncuredDate).format('YYYY-MM-DD');
                mainLedger.CostIncuredDate = CostIncuredDate;
                console.log(mainLedger);
                let param = {
                    Id: mainLedger.Id,
                    ContractNumber: mainLedger.ContractNumber,
                    InvoiceNumber: mainLedger.InvoiceNumber,
                    Amount: mainLedger.Amount,
                    PartnerId: mainLedger.PartnerId,
                    BranchId: mainLedger.BranchId,
                    CausedById: mainLedger.CausedById,
                    DescriptionId: mainLedger.DescriptionId,
                    OrderedById: mainLedger.OrderedById,
                    CostIncuredDate: mainLedger.CostIncuredDate,
                    PaymentType: mainLedger.PaymentType,
                    Comment: mainLedger.Comment,
                    UserId: mainLedger.UserId,
                    IsApproved: mainLedger.IsApproved,
                    IsSubmitted: mainLedger.IsSubmitted,
                    IsRejected: mainLedger.IsRejected,
                    IsControlCheck: mainLedger.IsControlCheck
                };

                this.mainLedger = { ...param }; // Clone the partner data
                $("#paymentModal").modal("show");
            },

            Alert(id) {

            },
            Del(id) {
                Swal.fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes, delete it!"
                }).then((result) => {
                    if (result.isConfirmed) {


                        var param = {
                            Id: Number(id)
                        }
                        objCommon.AjaxCall("/payments/delete", JSON.stringify(param), "POST", true, function (response) {

                            Swal.fire({
                                title: "Deleted!",
                                text: "Record has been deleted.",
                                icon: "success"
                            });
                            window.location.reload();
                        })


                    }
                });



            },
            ActionClick($event) {
                dthis = this;
                this.IdsSelected = [];

                const ActionType = $("#ddlAction").val();
                if (ActionType === "") {
                    objCommon.ShowMessage("please select action type.", "error");
                    return;
                }

                this.RequestList.forEach((v) => {
                    if (v.IsChecked) {
                        this.IdsSelected.push(Number(v.Id)); // Correct assignment
                    }
                });

                if (this.IdsSelected.length === 0) {
                    objCommon.ShowMessage("please select row first.", "error");
                    return;
                }


                this.resetCommonModal();
                this.CommonModel(0, ActionType, this.UserName, this.currDate);


            },
            RowAction() {
                dthis = this;
                this.IdsSelected = [];

                const ActionType = $("#ddlAction").val();
                if (ActionType === "") {
                    objCommon.ShowMessage("please select action type.", "error");
                    return;
                }
                this.RequestList.forEach((v) => {
                    if (v.IsChecked) {
                        this.IdsSelected.push(Number(v.Id)); // Correct assignment
                    }
                });

                if (this.IdsSelected.length === 0) {
                    objCommon.ShowMessage("please select row first.", "error");
                    return;
                }
                Swal.fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes, " + ActionType + " it!"
                }).then((result) => {
                    if (result.isConfirmed) {


                        const param = {
                            Ids: this.IdsSelected,
                            ActionType: ActionType
                        };

                        objCommon.AjaxCall("/payments/MultiRowsAction", JSON.stringify(param), "POST", true, function (response) {
                            dthis.IdsSelected = [];
                            Swal.fire({
                                title: "Success!",
                                text: "Data has been saved successfully!",
                                icon: "success"
                            });
                            window.location.reload();
                        });
                    }
                })

            },

            validateLength() {
                if (this.ContractNumber.length !== this.exactLength) {
                    this.inputError = `Input must be exactly ${this.exactLength} characters long.`;
                } else {
                    this.inputError = null;
                }
            },
            CommonModel(id, type, person, date) {
                console.log(id, date);
                this.common.EventId = id;
                this.common.EventType = type;
                this.common.EventBy = person;
                this.common.EventDate = date;
                $("#commonModel").modal("show");
            },
            resetCommonModal() {
                this.common = {
                    EventId: 0,
                    EventType: "",
                    EventBy: 0,
                    EventDescription: "",
                    EventDateTime: '',
                };
            },
            OnChangeAllCheck(AllCheck) {
                this.RequestList.forEach(function (v) {
                    if (AllCheck)
                        v.IsChecked = true;
                    else
                        v.IsChecked = false;
                })
            }
        }

    });


    setTimeout(function () {
        $("#leftmenu li").removeClass("active");
        $("#lipayments").addClass("active");

    }, 200)

})