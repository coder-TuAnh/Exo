    $('#NgayKetThuc').datepicker({
        orientation: "bottom auto",
        todayHighlight: true,
        dateFormat: "dd/mm/yy"
    });
    $('#NgayBatDau').datepicker({
        orientation: "bottom auto",
        todayHighlight: true,
        dateFormat: "dd/mm/yy"
    });
    var choiseCabin = function () {
        var listCabin = [];
        var _idcabin = $("#IDCruises").val();
        $.get("/admin/PromotionCodeCabin/GetListCabin", { id: _idcabin }, function (data) {
            $("#IDCabin").empty();
            $.each(data.listmenu, function (index, row) {
                $("#IDCabin").append("<option value='" + row.Value + "'>" + addzero(row.Value, 5) + "-" + row.Text + "</option>")
            });
        });
    }
    // auto sinh so 0 truoc ma cabin
    function addzero(number, size) {
        var s = number + "";
        while (s.length < size) s = "0" + s;
        return s;
    }
    // sinh ma giam gia tu dong
   
    function randomPassword(length) {
    var chars = "abcdefghijklmnopqrstuvwxyz!@#ABCDEFGHIJKLMNOP1234567890";
    var pass = "";
    for (var x = 0; x < length; x++) {
        var i = Math.floor(Math.random() * chars.length);
        pass += chars.charAt(i);
    }
    return pass;
    }
    function generate() {
        $("#Code").val(randomPassword(10));
    }
    