$("#create").click(function (e) {
    if(true) {
        $(this).attr('disabled', 'disabled');
        var editors = $("textarea");
        $("#Description").val(CKEDITOR.instances['Description'].getData());
        if (editors.length) {
            editors.each(function () {
                var instance = CKEDITOR.instances[$(this).attr("id")];
                if (instance && $(this).attr("id") !== 'Description') {
                    instance.destroy(true);
                    $(this).val(instance.getData());
                    $(this).hide();
                }
            });
        }
        var form = $("#cruisefrom").serialize();
        $.ajax({
            type: 'POST',
            url: "/admin/Cruise/Create",
            data: form,
            dataType: 'json',
            success: function (data) {
                if (data.success === true) {
                    bootbox.alert("Thêm chuyến đi thành công", function () {
                        window.location = "/admin/Cruise/Index";
                    });
                } else {
                    bootbox.alert("Lỗi");
                    $(this).removeAttr('disabled');
                }
            },
            error: function () {
                bootbox.alert("Lỗi");
                $(this).removeAttr('disabled');
            }
        });
    } else {
        bootbox.alert("Có Lỗi trong quá trình nhập đánh giá");
    }
    
})
$("#update").click(function (e) {
    var editors = $("textarea");
    $(this).attr('disabled', 'disabled');
    $("#Description").val(CKEDITOR.instances['Description'].getData());
    if (editors.length) {
        editors.each(function () {
            var instance = CKEDITOR.instances[$(this).attr("id")];
            if (instance && $(this).attr("id") !== 'Description') {
                instance.destroy(true);
                $(this).val(instance.getData());
                $(this).hide();
            }
        });
    }
    var form = $("#cruisefrom").serialize();
    $.ajax({
        type: 'POST',
        url: "/admin/Cruise/Update",
        data: form,
        dataType: 'json',
        success: function (data) {
            if (data.success === true) {
                bootbox.alert("Sửa chuyến đi thành công", function () {
                    window.location = "/admin/Cruise/Index";
                });
            } else {
                bootbox.alert("Lỗi");
                $(this).removeAttr('disabled');
            }
        },
        error: function () {
            bootbox.alert("Lỗi");
            $(this).removeAttr('disabled');
        }
    });
})
function checkratefrom() {
    if (isNumeric($("#Cruisequality").val()) && isNumeric($("#FoodDrink").val()) && isNumeric($("#Cabinquality").val()) && isNumeric($("#Staffquality").val()) && isNumeric($("#Entertainment").val())) {
        if (checkmaxnumber($("#Cruisequality").val()) && checkmaxnumber($("#FoodDrink").val()) && checkmaxnumber($("#Cabinquality").val()) && checkmaxnumber($("#Staffquality").val()) && checkmaxnumber($("#Entertainment").val())) {
            return true;
        } else {
            return false;
        }
    }
    else {
        return false;
    }
}
function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function checkmaxnumber(n) {
    if (parseFloat(n) >= 0 && parseFloat(n) <= 10) {
        return true;
    } else {
        return false;
    }
}
$("#uploadimages input[type='file']").off('change').change(function (e) {
    var tmp = URL.createObjectURL(e.target.files[0]);
    var test_value = $("#uploadimages input[type='file']").val();
    if (checkfileimg(test_value) === false) {
        bootbox.alert("Bạn chọn ảnh không hợp lệ");
        $("#uploadimages input[type='file']").val('');
        $("#nameimages").html("");
        var linknone = "~/Areas/Administrator/Content/img/noneimage.jpg";
        $(this).closest('.img-link').find('.img').attr("src", linknone);
    } else {
        $("#nameimages").html("Bạn vừa chọn ảnh:" +
            e.target.files[0].name);
        $(this).closest('.img-link').find('.img').attr("src", tmp);
    }

}); // không dùng
$("#uploadgallery input[type='file']").off('change').change(function (e) {
    var test_value = $("#uploadgallery input[type='file']").val();
    var tmp = URL.createObjectURL(e.target.files[0]);
    var idcruise = $("#ID").val();
    if (checkfileimg(test_value) === false) {
        bootbox.alert("Bạn chọn ảnh không hợp lệ");
    } else {
        updategallerycruise(idcruise);
    }

});// không dùng
function checkfileimg(test_value) {

    var extension = test_value.split('.').pop().toLowerCase();

    if ($.inArray(extension, ['png', 'gif', 'jpeg', 'jpg']) === -1) {
        return false;
    } else {
        return true;
    }
}
function loadlistcabin(idcruise) {
    var model = {
        idcruise: idcruise,
    }
    $.ajax({
        type: 'POST',
        url: "/admin/Cruise/Listcabinofcruise",
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.success === true) {
                var html = "";
                for (let i = 0; i < data.data.length; i++) {
                    let element = data.data[i];
                    html += "<div class='cabinstyle col-md-3' id ='cabincruise-" + element.ID  +"'>" +
                         
                        "<div><h4>" +element.Name +"</h4></div>"+
                        "<div><img src=" + element.Image + " style='width: 100%;'></div>" +
                        "<div style='padding:5px'><a style='margin-right:5px' class='btn btn-warning' onclick='onclicktest(event," + element.ID + ")' >Xem chi tiết</a>" +
                        "<a class='btn btn-danger' onclick='deleteItem(" + element.ID + ")' >Xóa</a></div>" +
                        "</div>";
                }
                $("#listcabin").html(html);
            } else {
                bootbox.alert("Lỗi");
            }
        },
        error: function () {
            bootbox.alert("Lỗi");
        }
    });
}
function deletegallery(nameimg) {
    var idcruise = $("#ID").val();
    var model = {
        id: idcruise,
        name: nameimg,
    }
    $.ajax({
        type: 'POST',
        url: "/admin/Cruise/DeletePhoto",
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function () {
            bootbox.alert("Lỗi");
        }
    });
}
function Addgallery(nameimg) {
    var list = $("#ImgGallery").val();
    var idcruise = $("#ID").val();
    loadlistgallery(idcruise)
} // không dùng
function loadlistgallery(idcruise) {
    var model = {
        idcruise: idcruise,
    }
    $.ajax({
        type: 'POST',
        url: "/admin/Cruise/Listgalleryfcruise",
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.success === true) {
                var html = "";
                var myList = [];
                if (data.data !== null && data.data !== ""){
                    myList = JSON.parse(data.data);
                    for (let i = 0; i < myList.length; i++) {
                        let element = myList[i];
                        var image = '<input id="EGalleryITems_' + i + '__Image" name="EGalleryITems[' + i + '].Image" type="hidden" value="' + element.NameImages + '"/>';
                        var img = '<img src="' + element.NameImages + '" width="100" height="80" />';
                        html += '<li>' + image + img + '</li>';
                    }
                    $("#listCruisegallery").html(html);
                    bindRightClick();
                }    
            } else {
                bootbox.alert("Lỗi");
            }
        },
        error: function () {
            bootbox.alert("Lỗi");
        }
    });
}
function onclicktest(event, id) {
    /*
       * hủy liên kết
       *
       * việc này quan trọng
       * nếu không hủy liên kết, modal sẽ không được load đúng cách
       * vì action sẽ trả về 1 trang html trống với partialview
       *
       * cách tương tự return false; cách này thực hiện ở cuối method
       */
    event.preventDefault();
    var idcruise = $("#ID").val();
    var $modal = $('#myModal_cabinedit');
    var $modalDialog = $modal.find('.modal-dialog');
    var href = "/admin/Cabin/AddCabin/" + id + "?idcruise=" + idcruise;

    // không cho phép tắt modal khi click bên ngoài modal
    var option = { backdrop: 'static' };

    // kiểm tra (logic, điều kiện...)

    // load modal
    $modalDialog.load(href, function () {
        $modal.modal(option, 'show');
        bindRightClick();
        var idcabincheck = $("#IDCabin").val() !== null ? $("#IDCabin").val() : "0";
        loadlistgallery(idcabincheck);
        CreateCkEditor();
    });
};
function sapxep() {
    var temp = 0;
    $(".allImage li").each(function () {
        $(this).find('input').each(function () {
            var id = $(this).attr("id");
            var name = $(this).attr("name");
            var abc = name.substring(name.indexOf('[') + 1, name.indexOf(']'));
            //console.log('name: ' + name + ' and : ' + abc);
            var idreplace = id.replace(abc, temp);
            var namereplace = name.replace(abc, temp);
            $(this).attr("id", idreplace);
            $(this).attr("name", namereplace);
        });
        temp++;
    });
    bindRightClick();
}
function bindRightClick() {
    $('.allImage li').unbind();
    $('.allImage li').bind('contextmenu', function (e) {
        $('#context-menu').css('left', e.pageX + 'px');
        $('#context-menu').css('top', e.pageY + 'px');
        $('#context-menu').show();
        e.preventDefault();
        curentImg = $(this);
        return false;
    });
}
function loadHtmlForImage(val) {
    if ($('#EGalleryITems_' + val + '__Image').length > 0) {
        return loadHtmlForImage(++val);
    } else {
        //console.log(val);
        var image = '<input id="EGalleryITems_' + val + '__Image" name="EGalleryITems[' + val + '].Image" type="hidden" value="' + $('#img-thumb').val() + '"/>';
        var img = '<img src="' + $('#img-thumb').val() + '" width="100" height="80" />';
        return ('<li>' + image + img + '</li>');
    }
}
function addgallerycruise(nameimg) {
    var idcruise = $("#ID").val();
    var model = {
        id: idcruise,
        name: nameimg,
    }
    $.ajax({
        type: 'POST',
        url: "/admin/Cruise/AddgalleryPhoto",
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
        },
        error: function () {
            bootbox.alert("Lỗi");
        }
    });
}
function updateimagecruise(id) {
    //Lay File
    var fileUpload = $("#uploadimages input[type='file']").get(0);
    var files = fileUpload.files;
    if (files.length !== 0) {


        // Create FormData object
        var fileData = new FormData();

        // Looping over all files and add it to FormData object

        fileData.append(files[0].name, files[0]);

        // Adding id cruise
        fileData.append(id, id);

        $.ajax({
            url: '/admin/Cruise/UploadFiles',
            type: 'POST',
            data: fileData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (data) {

            }

        });
    }

}// ko dùng
function updategallerycruise(id) {
    //Lay File
    var fileUpload = $("#uploadgallery input[type='file']").get(0);
    var files = fileUpload.files;

    // Create FormData object
    var fileData = new FormData();

    // Looping over all files and add it to FormData object

    fileData.append(files[0].name, files[0]);

    // Adding id cruise
    fileData.append(id, id);
    $.ajax({
        url: '/admin/Cruise/UpfileGallery',
        type: 'POST',
        data: fileData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        success: function (data) {
            loadlistgallery(id);
        }

    });
}// ko dùng