//Custom validation script for the RequiredIf validator
/*
 * Note that, jQuery validation registers its rules before the DOM is loaded.
 * If you try to register your adapter after the DOM is loaded, your rules will
 * not be processed. So wrap it in a self-executing function.
 * */
(function ($) {

    var $jQval = $.validator;
    
    $jQval.setDefaults({
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("is-invalid form-control:invalid").removeClass("is-valid form-control:valid");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).addClass("is-valid form-control:valid").removeClass("is-invalid form-control:invalid");
        },
        onfocusout: function (element) {// enable eager evaluation
            $(element).valid();
        },
        submitHandler: function (form) { //triggered only when form is valid.
            console.log("submit from validator");
            return true;
        }        
    });

    /*
     * A function that register the validator for field dependency checking.
     *      
     */
    function IsValueMatched(vals, actual) {
        if (vals == null) throw "vals cannot be null";

        vals = vals.split(';');

        for (var i = 0; i < vals.length; i++) {
            if (vals[i] == actual)
                return true;
        }    

        return false;
    }

    $jQval.addMethod("requiredif",
        function (value, element, parameters) {
            var id = $(element).attr('data-val-requiredif-id');
            var val = $(element).attr('data-val-requiredif-value');//valid values to mandate the dependency (can be more than 1 value seperated by ;)
            var el = $('#' + id); //drop down id
            var type = el.prop("tagName").toLowerCase();

            value = $.trim(value); //dependent value


                if (type == 'select') {
                    if (el.val() != '') { //drop down not empty
                        if (value == '' && IsValueMatched(val,el.val())) { //empty even if something is selected.
                            //remove the is-valid class
                            $(element).removeClass('is-valid form-control:valid');
                            $(element).addClass('is-invalid form-control:invalid');
                            return false;
                        }
                        else {
                            $(element).removeClass('is-invalid form-control:invalid');
                            $(element).addClass('is-valid form-control:valid');

                            return true;
                        }
                    }
                    else {
                        $(element).removeClass('is-invalid form-control:invalid');
                        $(element).addClass('is-valid form-control:valid');

                        return true;
                    }
                }
                else {
                    if (value == '' && IsValueMatched(val,el.val())) {
                        $(element).addClass('is-invalid form-control:invalid');
                        $(element).removeClass('is-valid form-control:valid');

                        return true;
                    }
                    else {
                        $(element).removeClass('is-invalid form-control:invalid');
                        $(element).addClass('is-valid form-control:valid');

                        return false;
                    }
                }
        }
    );

    var adapters = $jQval.unobtrusive.adapters;
    adapters.addBool('requiredif');

    /*
     * A function that register the validator for upload file input element.
     * this function can be extended further to include file type checking and file size checking.
     * 
     */
    $jQval.addMethod("uploadrequired",
        function (value, element, parameters) {
            var previous = $(element).attr('data-val-previous').trim();
            
            if (value == '' || value == null) {
                if (previous == '') {
                    $(element).removeClass('is-valid form-control:valid');
                    $(element).addClass('is-invalid form-control:invalid');
                    return false;
                }
                else return true;
            }
            else {
                $(element).addClass('is-valid form-control:valid');
                $(element).removeClass('is-invalid form-control:invalid');
                return true;
            }
        });

    adapters.addBool("uploadrequired");

})(jQuery);