/*
	Dropdown with Multiple checkbox select with jQuery - May 27, 2013
	(c) 2013 @ElmahdiMahmoud
	license: https://www.opensource.org/licenses/mit-license.php
*/

$("#Menucheckmutiselect .dropdown dt input").on('click', function () {
    $("#Menucheckmutiselect .dropdown dd ul").slideToggle('fast');
});

$("#Menucheckmutiselect .dropdown dd ul li a").on('click', function () {
    $("#Menucheckmutiselect .dropdown dd ul").hide();
});

function getSelectedValue(id) {
    return $("#" + id).find("#Menucheckmutiselect dt a span.value").html();
}

$(document).bind('click', function (e) {
    var $clicked = $(e.target);
    if (!$clicked.parents().hasClass("dropdown")) $(".dropdown dd ul").hide();
});

$('#Menucheckmutiselect .mutliSelect input[type="checkbox"]').on('click', function () {

    var title = $(this).closest('#Menucheckmutiselect .mutliSelect').find('input[type="checkbox"]').val(),
        title = $(this).val() + ",";

    if ($(this).is(':checked')) {
        var html = '<span title="' + title + '">' + title + '</span>';
        $('.multiSel').append(html);
        $(".hida").hide();
    } else {
        $('span[title="' + title + '"]').remove();
        var ret = $(".hida");
        $('#Menucheckmutiselect .dropdown dt input').val(ret);

    }
});