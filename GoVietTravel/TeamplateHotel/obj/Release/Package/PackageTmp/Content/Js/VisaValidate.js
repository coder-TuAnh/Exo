function pushData() {
    $("#fullname").val($("#FullName_0").val());
    $("#gender").val($("#Gender_0").val());
    $("#Country").val($("#nationality").val());
}

function isValid() {
    jQuery.fn.extend({
        scrollToMe: function () {
            var x = jQuery(this).offset().top - 100;
            jQuery('html,body').animate({ scrollTop: x }, 500);
        }
    });
    if ($("#select_number_visa").val() == "") {
        $('#select_number_visa').scrollToMe();
        toastr.error('Please Select number visa!', 'Error');
        return false;
    }
    if ($("#datearrival").val() == "") {
        toastr.error('Please Select Date Of Arrival!', 'Error');
        $('#datearrival').scrollToMe();
        return false;
    } else {
        var val = $("#datearrival").val()
        if (!checkDate(val)) {
            $('#datearrival').scrollToMe();
            toastr.error('Invalid Date Of Arrival!', 'Error', { timeOut: 5000 });
            return false;
        } else {
            toastr.remove();
        }
    }
    x = $("#select_number_visa").val();
    for (var i = 0; i < x; i++) {
        if ($("#FullName_" + i).val() == "") {
            toastr.error('Please enter FullName at No ' + (i + 1) + '!', 'Error');
            $("#FullName_" + i).scrollToMe();
            return false;
        }
        if ($("#Gender_" + i).val() == "") {
            $("#Gender_" + i).scrollToMe();
            toastr.error('Please Select Gender at No ' + (i + 1) + '!', 'Error');
            return false;
        }
        if ($("#DateOfBirth_" + i).val() == "") {
            $("#DateOfBirth_" + i).scrollToMe();
            toastr.error('Please Select Date Of Birth at No ' + (i + 1) + '!', 'Error');
            return false;
        } else {
            if ((checkDate($("#DateOfBirth_" + i).val()))) {
                toastr.error('Invalid Date! Date Of Birth at No ' + (i + 1) + '!', 'Error');
                $("#DateOfBirth_" + i).scrollToMe();
                return false;
            }
        }
        if (($("#PassportNumber_" + i).val().length) > 10 || ($("#PassportNumber_" + i).val().length) == 0) {
            toastr.error('Please enter Passport Number ( < 10 digits) at No ' + (i + 1) + '!', 'Error');
            return false;
        }if ($("#PassportExpirationDate_" + i).val() == "") {
            $("#PassportExpirationDate_" + i).scrollToMe();
            toastr.error('Please Select Passport Expiration Date at No ' + (i + 1) + '!', 'Error');
            return false;
        } else 
        {
            if ((!checkDate($("#PassportExpirationDate_" + i).val())))
            {
                toastr.error('Invalid Date! Passport Expiration Date at No ' + (i + 1) + '!', 'Error');
                $("#PassportExpirationDate_" + i).scrollToMe();
                return false;
            }
        }
    }
    if ($("#arrival_airport").val() == "") {
        $("#arrival_airport").scrollToMe();
        toastr.error('Please Select arrival airport!', 'Error');
        return false;
    }
    return true;
}
function validForm() {
    $("#formOrder").validate({
        rules: {
            DateOfArrival: {
                required: true,
                date: true
            },
            FullName: "required",
            Email: {
                required: true,
                email: true
            },
            ReMail: {
                required: true,
                email: true,
                equalTo: "#email"
            },
            Country: "required",
            City: "required",
            PhoneNumber: {
                required: true,
                number: true,
                minlength: 9
            },
            City: "required"
        },
        messages: {
            FullName: "Please specify your name",
            Email: {
                required: "We need your email address to contact you",
                email: "Your email address must be in the format of name@domain.com"
            },
            PhoneNumber: {
                required: "We need your Phone Number to contact you",
                number: "Your Phone Number must be in the format of 6641234567"
            }
        }
    });
    if ($("#formOrder").valid()) {
        return true;
    } else {
        return false;
    }
}

$('#confirm').on('click', function (e) {
    e.preventDefault()
    if (isValid() && validForm()) {
        $("#formOrder").submit();
    }
})


$("#datearrival").change(function () {
    var val = $("#datearrival").val()
    if(!checkDate(val))
    {
        toastr.error('Invalid Date Of Arrival!', 'Error', { timeOut: 5000 });
    }else
    {
        toastr.remove();
    }
});

function checkDate(value)
{
    var curDate = new Date();
    value = new Date(value);
    var inputDate = new Date((value.getTime() - 7 * 60 * 60 * 1000) + curDate.getHours() * 60 * 60 * 1000 + curDate.getMinutes() * 60 * 1000 + curDate.getSeconds() * 1000 + 2000);
    if (inputDate >= curDate) {
        return true;
    } else {
        return false;
    }
}