﻿function DatatablesLoad(tableName,data,columns) {
    var table = $('#' + tableName).DataTable({
     "data": data,
     "destroy": true,
    "columns": columns,
    "processing": false,
    "serverSide": false,
    "responsive": true,
    "dom": 'Bfrtip',
    "lengthChange": !1,
    "buttons": [ "excel","colvis"],
    "language": {
        "url": '/Content/Dashboard/assets/language/tr.json',
        "paginate": {
            "previous": "<i class='mdi mdi-chevron-left'>",
            "next": "<i class='mdi mdi-chevron-right'>"
          }
      }
    });
    table.buttons().container()
        .appendTo($('.col-sm-12 .col-md-6:eq(0)', table.table().container()));
}

function Chekbox(checked, disable) {
    var IsCheced = "";
    var IsDisable = "";
    var drm = "Pasif";
    if (checked) {
        IsCheced = "checked";
        drm = "Aktif";
    }
    if (disable) {
        IsDisable = "disabled";
    }

    return "<div class='custom-control custom-checkbox mb-2'>" +
        "<input type='checkbox' id='autoSizingCheck' class='custom-control-input' " + IsDisable + " " + IsCheced + ">" +
        "<label class='custom-control-label' for='autoSizingCheck'>" + drm + "</label></div>";
}

function TarihFormat(date) {
    if (date != null) {
        return moment(date).format('L');
    }
    else {
        return null;
    }

}



function url_slug(s, opt) {
    s = String(s);
    opt = Object(opt);
    var defaults = {
        'delimiter': '-',
        'limit': undefined,
        'lowercase': true,
        'replacements': {},
        'transliterate': (typeof (XRegExp) === 'undefined') ? true : false
    };
    for (var k in defaults) {
        if (!opt.hasOwnProperty(k)) {
            opt[k] = defaults[k];
        }
    }
    var char_map = {
        // Latin
        'À': 'A', 'Á': 'A', 'Â': 'A', 'Ã': 'A', 'Ä': 'A', 'Å': 'A', 'Æ': 'AE', 'Ç': 'C',
        'È': 'E', 'É': 'E', 'Ê': 'E', 'Ë': 'E', 'Ì': 'I', 'Í': 'I', 'Î': 'I', 'Ï': 'I',
        'Ð': 'D', 'Ñ': 'N', 'Ò': 'O', 'Ó': 'O', 'Ô': 'O', 'Õ': 'O', 'Ö': 'O', 'Ő': 'O',
        'Ø': 'O', 'Ù': 'U', 'Ú': 'U', 'Û': 'U', 'Ü': 'U', 'Ű': 'U', 'Ý': 'Y', 'Þ': 'TH',
        'ß': 'ss',
        'à': 'a', 'á': 'a', 'â': 'a', 'ã': 'a', 'ä': 'a', 'å': 'a', 'æ': 'ae', 'ç': 'c',
        'è': 'e', 'é': 'e', 'ê': 'e', 'ë': 'e', 'ì': 'i', 'í': 'i', 'î': 'i', 'ï': 'i',
        'ð': 'd', 'ñ': 'n', 'ò': 'o', 'ó': 'o', 'ô': 'o', 'õ': 'o', 'ö': 'o', 'ő': 'o',
        'ø': 'o', 'ù': 'u', 'ú': 'u', 'û': 'u', 'ü': 'u', 'ű': 'u', 'ý': 'y', 'þ': 'th',
        'ÿ': 'y',
        // Latin symbols
        '©': '(c)',
        // Greek
        'Α': 'A', 'Β': 'B', 'Γ': 'G', 'Δ': 'D', 'Ε': 'E', 'Ζ': 'Z', 'Η': 'H', 'Θ': '8',
        'Ι': 'I', 'Κ': 'K', 'Λ': 'L', 'Μ': 'M', 'Ν': 'N', 'Ξ': '3', 'Ο': 'O', 'Π': 'P',
        'Ρ': 'R', 'Σ': 'S', 'Τ': 'T', 'Υ': 'Y', 'Φ': 'F', 'Χ': 'X', 'Ψ': 'PS', 'Ω': 'W',
        'Ά': 'A', 'Έ': 'E', 'Ί': 'I', 'Ό': 'O', 'Ύ': 'Y', 'Ή': 'H', 'Ώ': 'W', 'Ϊ': 'I',
        'Ϋ': 'Y',
        'α': 'a', 'β': 'b', 'γ': 'g', 'δ': 'd', 'ε': 'e', 'ζ': 'z', 'η': 'h', 'θ': '8',
        'ι': 'i', 'κ': 'k', 'λ': 'l', 'μ': 'm', 'ν': 'n', 'ξ': '3', 'ο': 'o', 'π': 'p',
        'ρ': 'r', 'σ': 's', 'τ': 't', 'υ': 'y', 'φ': 'f', 'χ': 'x', 'ψ': 'ps', 'ω': 'w',
        'ά': 'a', 'έ': 'e', 'ί': 'i', 'ό': 'o', 'ύ': 'y', 'ή': 'h', 'ώ': 'w', 'ς': 's',
        'ϊ': 'i', 'ΰ': 'y', 'ϋ': 'y', 'ΐ': 'i',
        // Turkish
        'Ş': 'S', 'İ': 'I', 'Ç': 'C', 'Ü': 'U', 'Ö': 'O', 'Ğ': 'G',
        'ş': 's', 'ı': 'i', 'ç': 'c', 'ü': 'u', 'ö': 'o', 'ğ': 'g',
        // Russian
        'А': 'A', 'Б': 'B', 'В': 'V', 'Г': 'G', 'Д': 'D', 'Е': 'E', 'Ё': 'Yo', 'Ж': 'Zh',
        'З': 'Z', 'И': 'I', 'Й': 'J', 'К': 'K', 'Л': 'L', 'М': 'M', 'Н': 'N', 'О': 'O',
        'П': 'P', 'Р': 'R', 'С': 'S', 'Т': 'T', 'У': 'U', 'Ф': 'F', 'Х': 'H', 'Ц': 'C',
        'Ч': 'Ch', 'Ш': 'Sh', 'Щ': 'Sh', 'Ъ': '', 'Ы': 'Y', 'Ь': '', 'Э': 'E', 'Ю': 'Yu',
        'Я': 'Ya',
        'а': 'a', 'б': 'b', 'в': 'v', 'г': 'g', 'д': 'd', 'е': 'e', 'ё': 'yo', 'ж': 'zh',
        'з': 'z', 'и': 'i', 'й': 'j', 'к': 'k', 'л': 'l', 'м': 'm', 'н': 'n', 'о': 'o',
        'п': 'p', 'р': 'r', 'с': 's', 'т': 't', 'у': 'u', 'ф': 'f', 'х': 'h', 'ц': 'c',
        'ч': 'ch', 'ш': 'sh', 'щ': 'sh', 'ъ': '', 'ы': 'y', 'ь': '', 'э': 'e', 'ю': 'yu',
        'я': 'ya',
        // Ukrainian
        'Є': 'Ye', 'І': 'I', 'Ї': 'Yi', 'Ґ': 'G',
        'є': 'ye', 'і': 'i', 'ї': 'yi', 'ґ': 'g',
        // Czech
        'Č': 'C', 'Ď': 'D', 'Ě': 'E', 'Ň': 'N', 'Ř': 'R', 'Š': 'S', 'Ť': 'T', 'Ů': 'U',
        'Ž': 'Z',
        'č': 'c', 'ď': 'd', 'ě': 'e', 'ň': 'n', 'ř': 'r', 'š': 's', 'ť': 't', 'ů': 'u',
        'ž': 'z',
        // Polish
        'Ą': 'A', 'Ć': 'C', 'Ę': 'e', 'Ł': 'L', 'Ń': 'N', 'Ó': 'o', 'Ś': 'S', 'Ź': 'Z',
        'Ż': 'Z',
        'ą': 'a', 'ć': 'c', 'ę': 'e', 'ł': 'l', 'ń': 'n', 'ó': 'o', 'ś': 's', 'ź': 'z',
        'ż': 'z',
        // Latvian
        'Ā': 'A', 'Č': 'C', 'Ē': 'E', 'Ģ': 'G', 'Ī': 'i', 'Ķ': 'k', 'Ļ': 'L', 'Ņ': 'N',
        'Š': 'S', 'Ū': 'u', 'Ž': 'Z',
        'ā': 'a', 'č': 'c', 'ē': 'e', 'ģ': 'g', 'ī': 'i', 'ķ': 'k', 'ļ': 'l', 'ņ': 'n',
        'š': 's', 'ū': 'u', 'ž': 'z'
    };
    for (var k in opt.replacements) {
        s = s.replace(RegExp(k, 'g'), opt.replacements[k]);
    }
    if (opt.transliterate) {
        for (var k in char_map) {
            s = s.replace(RegExp(k, 'g'), char_map[k]);
        }
    }
    var alnum = (typeof (XRegExp) === 'undefined') ? RegExp('[^a-z0-9]+', 'ig') : XRegExp('[^\\p{L}\\p{N}]+', 'ig');
    s = s.replace(alnum, opt.delimiter);
    s = s.replace(RegExp('[' + opt.delimiter + ']{2,}', 'g'), opt.delimiter);
    s = s.substring(0, opt.limit);
    s = s.replace(RegExp('(^' + opt.delimiter + '|' + opt.delimiter + '$)', 'g'), '');
    return opt.lowercase ? s.toLowerCase() : s;
}

$(".text-editor").summernote({
    height: 300,
    minHeight: null,
    maxHeight: null,
    focus: !0,
    lang: 'tr-TR'
})

var Imgdata = [];
function FileUpload(obj) {
    var snc = "";

    var postFile = Array.from(document.getElementById(obj.id).files);
    var data = new FormData();
    if (postFile != null && postFile.length > 0) {
        postFile.forEach(function (x) {
            data.append('file', x);
        })
        $.ajax({
            url: '/upload/upload-file',
            processData: false,
            contentType: false,
            data: data,
            type: 'POST',
            success: function (data) {
                PostFiles = data.Img;
                Imgdata = data.Img;
                Imgdata.forEach(function (x) {
                    // document.getElementById("seciliresim").innerHTML = x.FileName;
                    snc += '<a id="' + x.FileId + '" class="' + x.FileId + '" onclick="DeleteFile(this)">' + x.FileName + ' X Sil </a>';
                });
                document.getElementById("seciliresim").innerHTML = snc;
                document.getElementById("SilinecekResimler").innerHTML += snc;
            },
            async: false
        });
        return Imgdata;
    }
    else {
        return null;
    }

}
function DeleteFile(obj) {

    Imgdata.forEach(function (x) {
        if (x.FileId == obj.id) {
            $.ajax({
                url: '/upload/delete-file',
                data: JSON.stringify(x),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    if (data.state) {
                        var Xdata = $("." + obj.id);
                        for (var i = 0; i < Xdata.length; i++) {
                            Xdata[i].remove();
                        }
                        Imgdata.splice(x, 1);
                    }
                },
                async: false
            });
        }
    })

}
function DeleteSingleFile(obj) {
    var singleImg = {
        "FileUrl": obj.id,
        "FileName": "",
        "FileId": null
    }
    $.ajax({
        url: '/upload/delete-file',
        data: JSON.stringify(singleImg),
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        success: function (data) {
            if (data.state) {

                if (obj.parentElement != undefined) {
                    obj.parentElement.innerHTML = "";
                }
                //var Xdata = $("." + obj.id);
                //for (var i = 0; i < Xdata.length; i++) {
                //    Xdata[i].remove();
                //}
                //Imgdata.splice(x, 1);
            }
        },
        async: false
    });
}