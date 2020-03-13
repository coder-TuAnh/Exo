
console.log(1);
var countimg = 0;
$('html').click(function (e) {
    $('#context-menucabin').hide();
});
$('#context-menucabin').click(function (e) {
    e.stopPropagation();
    var imagname = $(curentImg).children("input").val();
    deletegallery(imagname);
    $(curentImg).remove();
    sapxep();
    $('#context-menucabin').hide(100);
});
$(window).resize(function () {
    $('#context-menucabin').hide();
});
$("#createcabin").click(function (e) {
    $(this).attr('disabled', 'disabled');
    $("#Content").val(CKEDITOR.instances['Content'].getData());
    $("#Descrip").val(CKEDITOR.instances['Descrip'].getData());
    var form = $("#cabinfrom").serialize();
    $.ajax({
        type: 'POST',
        url: "/admin/Cabin/Create",
        data: form,
        dataType: 'json',
        success: function (data) {
            if (data.success == true) {
                bootbox.alert("Thêm Cabin thành công", function () {
                    window.location.reload();

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
function openPopup(check) {
    var ckfinder = new CKFinder();
    ckfinder.selectActionFunction = function (url) {
        if (check == 1){
            $("#img-thumbcabin").val(url);
        } else {
            $("#txtimgcabin").val(url);
        }
       
    };
    ckfinder.popup({
        chooseFiles: true,
        width: 1000,
        height: 600,
        onInit: function (finder) {
            finder.on('files:choose', function (evt) {
                var file = evt.data.files.first();
                var output = document.getElementById('txtimgcabin');
                $("#txtimgcabin").val(file.getUrl());

            });

            finder.on('file:choose:resizedImage', function (evt) {
                var output = document.getElementById('txtimgcabin');

                $("#txtimgcabin").val(evt.data.resizedUrl);
            });
            finder.on('createResources:after', function (event) {

            });
        }
    });
}

$("#uploadimagescabin input[type='file']").off('change').change(function (e) {
    var tmp = URL.createObjectURL(e.target.files[0]);
    var test_value = e.target.files[0].name;
    if (checkfileimg(test_value) == false) {
        bootbox.alert("Bạn chọn ảnh không hợp lệ");
        $("#uploadimagescabin input[type='file']").val('');
        $("#nameimages").html("");
        var linknone = "~/Areas/Administrator/Content/img/noneimage.jpg";
        $(this).closest('.img-link').find('.img').attr("src", linknone);
    } else {
        $("#nameimages").html("Bạn vừa chọn ảnh:" +
            e.target.files[0].name);
        $(this).closest('.img-link').find('.img').attr("src", tmp);
    }

});// khong dùng
$("#uploadgallerycabin input[type='file']").off('change').change(function (e) {
    var test_value = e.target.files[0].name;
    var tmp = URL.createObjectURL(e.target.files[0]);
    var idcruise = $("#IDCabin").val();
    if (checkfileimg(test_value) == false) {
        bootbox.alert("Bạn chọn ảnh không hợp lệ");
    } else {
        updategallerycabin(idcruise);
    }

}); // không dùng
function checkfileimg(test_value) {

    var extension = test_value.split('.').pop().toLowerCase();

    if ($.inArray(extension, ['png', 'gif', 'jpeg', 'jpg']) == -1) {
        return false;
    } else {
        return true;
    }
}// khong dùng
function deletegallery(nameimg) {
    var idcabin = $("#IDCabin").val();
    var model = {
        id: idcabin,
        name: nameimg,
    }
    $.ajax({
        type: 'POST',
        url: "/admin/Cabin/DeletePhoto",
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
    var idcabin = $("#IDCabin").val();
    loadlistgallery(idcabin)
}// khong dùng
function loadlistgallery(idcabin) {
    if (idcabin != "0") {
        var model = {
            idcabin: idcabin,
        }
        $.ajax({
            type: 'POST',
            url: "/admin/Cabin/Listgalleryfcabin",
            data: JSON.stringify(model),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.success == true) {
                    var html = "";
                    var myList = [];
                    if (data.data != null && data.data != "") {
                        myList = JSON.parse(data.data);
                        for (let i = 0; i < myList.length; i++) {
                            let element = myList[i];
                            var image = '<input id="ECabinGalleryITems_' + i + '__Image" name="ECabinGalleryITems[' + i + '].Image" type="hidden" value="' + element.NameImages + '"/>';
                            var img = '<img src="' + element.NameImages + '" width="100" height="80" />';
                            html += '<li>' + image + img + '</li>';
                        }
                        $("#listCabingallery").html(html);
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
}
function updateimagecabin(id) {
    //Lay File
    var fileUpload = $("#uploadimagescabin input[type='file']").get(0);
    var files = fileUpload.files;
    if (files.length != 0) {


        // Create FormData object
        var fileData = new FormData();

        // Looping over all files and add it to FormData object

        fileData.append(files[0].name, files[0]);

        // Adding id cruise
        fileData.append(id, id);

        $.ajax({
            url: '/admin/Cabin/UploadFiles',
            type: 'POST',
            data: fileData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (data) {

            }

        });
    }

} //không dùng
function updategallerycabin(id) {
    //Lay File
    var fileUpload = $("#uploadgallerycabin input[type='file']").get(0);
    var files = fileUpload.files;

    // Create FormData object
    var fileData = new FormData();

    // Looping over all files and add it to FormData object

    fileData.append(files[0].name, files[0]);

    // Adding id cruise
    fileData.append(id, id);
    $.ajax({
        url: '/admin/Cabin/UpfileGallery',
        type: 'POST',
        data: fileData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        success: function (data) {
            loadlistgallery(id);
        }

    });
}//khong dùng
function bindRightClick() {
    $('#listCabingallery li').unbind();
    $('#listCabingallery li').bind('contextmenu', function (e) {
        $('#context-menucabin').css('left', (e.pageX - 200) + 'px');
        $('#context-menucabin').css('top', '-45px');
        $('#context-menucabin').show();
        e.preventDefault();
        curentImg = $(this);
        return false;
    });
}
function sapxep() {
    var temp = 0;
    $("#listCabingallery li").each(function () {
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
function addgallerycruise(nameimg) {
    var idcruise = $("#IDCabin").val();
    var model = {
        id: idcruise,
        name: nameimg,
    }
    $.ajax({
        type: 'POST',
        url: "/admin/Cabin/AddgalleryPhoto",
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function () {
            bootbox.alert("Lỗi");
        }
    });
}
function loadHtmlForImage(val) {
    if ($('#ECabinGalleryITems_' + val + '__Image').length > 0) {
        return loadHtmlForImage(++val);
    } else {
        //console.log(val);
        var image = '<input id="ECabinGalleryITems_' + val + '__Image" name="ECabinGalleryITems[' + val + '].Image" type="hidden" value="' + $('#img-thumbcabin').val() + '"/>';
        var img = '<img src="' + $('#img-thumbcabin').val() + '" width="100" height="80" />';
        return ('<li>' + image + img + '</li>');
    }
}
//Thêm ảnh
$('#addimgcabin').click(function () {
    if ($('#img-thumbcabin').val() == "") {
        alert("Please select image");
    } else {
        var data = '';
        while (true) {
            if ($('#ECabinGalleryITems_0__Image').length == 0) {
                data = loadHtmlForImage(0);
                countimg = 1;
            } else {
                data = loadHtmlForImage(countimg++);
            }
            break;
        }
        $('#listCabingallery').append(data);
        addgallerycruise($('#img-thumbcabin').val());
        sapxep();
        bindRightClick();
        $('#img-titlecabin').val('');
        $('#img-thumbcabin').val('');
        $('#imgcabin').val('');
    }

});