////Phuc ND

var TypeOfVisa;
var ProcessingTime;

function initBaseData() {
    ///////////Get TypeOfVisa
    $.ajax({
        type: 'GET',
        url: '../visa-vietnam/GetTypeOfVisa',
        dataType: 'json',
        success: function (data) {
            TypeOfVisa = data;
            $.each(data, function (index, element) {
                $('#visa_type').append($('<option>', {
                    value: element.ID,
                    text: element.Name
                }));
            });
        }
    });
    ///////////Get Processing time
    $.ajax({
        type: 'GET',
        url: '../visa-vietnam/GetProcessingTime',
        dataType: 'json',
        success: function (data) {
            ProcessingTime = data;
            $.each(data, function (index, element) {
                $('#processing_time').append($('<option>', {
                    value: element.ID,
                    text: element.Name
                }));
            });
        }
    });
}

$(document).ready(function () {
    initBaseData();
    initInfVisa(1);
    window.onload = function () {
        changedata();
    };
});

$(document).on('change', '#select_number_visa', function () {
    var Sl = $("#select_number_visa").val();
    initInfVisa(Sl);
});
function initInfVisa(Sl) {
    var datenow = new Date();
    var mMonth;
    if((datenow.getMonth() + 1)>9)
    {
        mMonth = (datenow.getMonth() + 1);
    } else
    {
        mMonth = "0" + (datenow.getMonth() + 1);
    }
    var datenowFormat = (datenow.getFullYear()+2) + "-" + mMonth + "-" + datenow.getDate();
    var s = "";
    for (var i = 1; i <= Sl; i++) {
        s += '<h4 class="text-highlight"><label class="group-label">Applicant ' + i + '(*)</label></h4><div class="row"><div class="col-sm-1"><div class="form-group"><label class="control-label">No</label><label class="form-control">' + i + '</label></div></div><div class="col-md-5"><div class="form-group"><label class="control-label">Full name: (*)</label><input type="text" class="form-control" name="lst[' + (i - 1) + '].FullName" id="FullName_' + (i - 1) + '"/></div></div><div class="col-sm-2"><div class="form-group"><label class="control-label">Gender: *</label><select name="lst[' + (i - 1) + '].Gender" id="Gender_' + (i - 1) + '" class="form-control"><option value="" selected="selected">---</option><option value="True">Male</option><option value="False">Female</option></select></div></div><div class="col-sm-4"><div class="form-group"><label class="control-label">Date of birth: *</label><input type="date" class="form-control" name="lst[' + (i - 1) + '].DateOfBirth" id="DateOfBirth_' + (i - 1) + '"/></div></div></div><div class="row"><div class="col-sm-4"><div class="form-group"><label class="control-label">Nationality: *</label><select id="nationality" name="lst[' + (i - 1) + '].Nationality" class="form-control"><option value=""> -- Please select your nationality -- </option><option value="Argentina">Argentina</option><option value="Australia" selected="selected">Australia</option><option value="Austria">Austria</option><option value="Belarus">Belarus</option><option value="Belgium">Belgium</option><option value="Brazil">Brazil</option><option value="Brunei">Brunei</option><option value="Bulgaria">Bulgaria</option><option value="Cambodia">Cambodia</option><option value="Canada">Canada</option><option value="Chile">Chile</option><option value="China">China</option><option value="Colombia">Colombia</option><option value="Croatia">Croatia</option><option value="Cuba">Cuba</option><option value="Czech">Czech Republic</option><option value="Ecuador">Ecuador</option><option value="Fiji">Fiji</option><option value="France">France</option><option value="Germany">Germany</option><option value="Greece">Greece</option><option value="Greenland">Greenland</option><option value="Hong Kong">Hong Kong</option><option value="Hungary">Hungary</option><option value="Iceland">Iceland</option><option value="India">India</option><option value="Ireland">Ireland</option><option value="Italy">Italy</option><option value="Latvia">Latvia</option><option value="Liechtenstein">Liechtenstein</option><option value="Lithuania">Lithuania</option><option value="Luxembourg">Luxembourg</option><option value="Macau">Macau</option><option value="Macedonia">Macedonia</option><option value="Malta">Malta</option><option value="Mexico">Mexico</option><option value="Moldova">Moldova</option><option value="Monaco">Monaco</option><option value="Mongolia">Mongolia</option><option value="Montenegro">Montenegro</option><option value="Myanmar">Myanmar</option><option value="Netherlands">Netherlands</option><option value="New Zealand">New Zealand</option><option value="Paraguay">Paraguay</option><option value="Peru">Peru</option><option value="Poland">Poland</option><option value="Portugal">Portugal</option><option value="Romania">Romania</option><option value="Russia">Russia</option><option value="Serbia">Serbia</option><option value="Slovakia">Slovakia</option><option value="Slovenia">Slovenia</option><option value="South Africa">South Africa</option><option value="South Kore">South Korea</option><option value="Spain">Spain</option><option value="Switzerland">Switzerland</option><option value="Taiwan">Taiwan</option><option value="Ukraine">Ukraine</option><option value="United Kingdom">United Kingdom</option><option value="United States">United States</option><option value="Uruguay">Uruguay</option><option value="Vanuatu">Vanuatu</option><option value="Vatican City">Vatican City</option><option value="Venezuela">Venezuela</option></select></div></div><div class="col-sm-4"><div class="form-group"><label class="control-label">Passport Number: *</label><input type="text" class="form-control" name="lst[' + (i - 1) + '].PassportNumber" id="PassportNumber_' + (i - 1) + '"  /></div></div><div class="col-sm-4"><div class="form-group"><label class="control-label">Passport Expiration Date: *</label><input type="date" class="form-control" name="lst[' + (i - 1) + '].PassportExpirationDate" value="' + datenowFormat + '" id="PassportExpirationDate_' + (i - 1) + '"  /></div></div></div></div>'
    }
    $("#visa-detail").html(s);
}

$('#select_number_visa').change(function () {
    changedata();
});
$('#visa_type').change(function () {
    changedata();
});

$('#processing_time').change(function () {
    changedata();
});

function getTimePrice() {
    var PrTime = $('#processing_time').val();
    for (var i = 0; i < ProcessingTime.length; i++) {
        if (ProcessingTime[i].ID == PrTime) {
            return ProcessingTime[i].Price;
        }
    }
}

function changedata() {
    var type = $('#visa_type').val();
    var NumberOfVisa = $('#select_number_visa').val();
    var PrTime = $('#processing_time').val();
    for (var i = 0; i < TypeOfVisa.length; i++) {
        if (TypeOfVisa[i].ID == type) {
            var s = '$' + TypeOfVisa[i].Price + ' x' + NumberOfVisa + '= ' + '<span class="text-price">$' + (TypeOfVisa[i].Price * NumberOfVisa + getTimePrice()) + '</span>'
            $('#visa_from_price').html(s);
            $('#selection_nationality').text($("#select_number_visa option:selected").text());
            $('#visa_total_fee').text('$' + (TypeOfVisa[i].Price * NumberOfVisa + getTimePrice()));
            $('#selection_visa_type').text($("#visa_type option:selected").text());
            $('#selection_rush_service').text($("#processing_time option:selected").text());
            $('#totalprice').val(TypeOfVisa[i].Price * NumberOfVisa + getTimePrice());
            // step review
            $('#visa_from_price-2').html(s);
            $('#selection_nationality-2').text($("#select_number_visa option:selected").text());
            $('#visa_total_fee-2').text('$' + (TypeOfVisa[i].Price * NumberOfVisa + getTimePrice()));
            $('#selection_visa_type-2').text($("#visa_type option:selected").text());
            $('#selection_rush_service-2').text($("#processing_time option:selected").text());
        }
    }
}