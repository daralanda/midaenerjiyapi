var modalTab = document.getElementById("exampleModal");
var modalMain = document.getElementById("mainModal");
var Domain = document.getElementById("Domain").value;
var Keys = [];
var maindata = {
    LanguageId:0,
    ShortCode: "",
    LanguageName: "",
    IsActive: false,
    MetaKeywords: "",
    Description: "",
    Title: "",
    UrlText: "",
    Queno: 0,
    ContactUrl: "",
    UrlString: "",
    ContactString:""
}
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllLanguage',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                Keys = data.keys;
                var columns = [
                    {"data": "LanguageName"},
                    { "data": "ShortCode" },
                    {
                        "data": "IsActive",
                        render: function (x) {
                            return Chekbox(x, true);
                        }
                    },
                    { "data": "Queno" },
                    {
                        "data": null,
                        render: function (data, type, row) {
                            return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light text-white edit'  id='" + data.LanguageId + "'  data-toggle='modal' data-target='#mainModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> " +
                                "<a onclick='Delete(this)' class='btn btn-danger btn-sm waves-effect waves-light text-white delete'  id='" + data.LanguageId + "' ><i class='fas fa-pencil-alt'></i> Sil</a> " +
                                "<a onclick='btnSubClick(" + data.LanguageId + ")' class='btn btn-success btn-sm waves-effect waves-light text-white'   data-toggle='modal' data-target='#exampleModal'><i class='fas fa-pencil-alt'></i> Cevirler</a> ";
                        }
                    }
                ];
                DatatablesLoad("datatables", data.data, columns);
             
            }
        }
    });

}
function btnClick(obj) {
    if (obj.className.includes("insert")) {
        document.getElementById("modalTitle").innerHTML = "Dil Ekle";
        document.getElementById("btnSumbit").innerHTML = "Yeni Dil Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Dil Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Dil Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    document.getElementById("LanguageName").value = "";
    document.getElementById("ShortCode").value = "";
    document.getElementById("Title").value = "";
    document.getElementById("MetaKeywords").value = "";
    document.getElementById("Description").value = "";
    document.getElementById("UrlText").value = "";
    document.getElementById("Queno").value = 0;
    document.getElementById("xIsActive").value = false;
    document.getElementById("ContactUrl").value = "";
    document.getElementById("UrlString").value = "";
    document.getElementById("ContactString").value = "";
}
function Find(id) {
    maindata.LanguageId = id;
    $.ajax({
        url: '/api/FindLanguage',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(maindata),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                maindata = data.data;
                document.getElementById("LanguageName").value = maindata.LanguageName;
                document.getElementById("ShortCode").value = maindata.ShortCode;
                document.getElementById("Title").value = maindata.Title;
                document.getElementById("MetaKeywords").value = maindata.MetaKeywords;
                document.getElementById("Description").value = maindata.Description;
                document.getElementById("UrlText").value = maindata.UrlText;
                document.getElementById("Queno").value = maindata.Queno;
                document.getElementById("xIsActive").value = maindata.IsActive;
                document.getElementById("ContactUrl").value = maindata.ContactUrl;
                document.getElementById("UrlString").value = maindata.UrlString;
                document.getElementById("ContactString").value = maindata.ContactString;
            }
        }
    });

}
function PostData(obj) {
    maindata.Title = document.getElementById("Title").value;
    maindata.LanguageName = document.getElementById("LanguageName").value;
    maindata.MetaKeywords = document.getElementById("MetaKeywords").value;
    maindata.Description = document.getElementById("Description").value;
    maindata.IsActive = document.getElementById("xIsActive").value;
    maindata.Queno = document.getElementById("Queno").value;
    maindata.ShortCode = document.getElementById("ShortCode").value;
    maindata.UrlText = document.getElementById("UrlText").value;
    maindata.ContactUrl = document.getElementById("ContactUrl").value;
    maindata.UrlString = document.getElementById("UrlString").value;
    maindata.ContactString = document.getElementById("ContactString").value;
    if (obj.innerHTML == "Dil Güncelle") {
        $.ajax({
            url: '/api/UpdateLanguage',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Dil Güncelleme!",
                        text: "Dil başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Dil Güncelleme!",
                        text: "Dil bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        maindata.LanguageId = 0;
        $.ajax({
            url: '/api/AddLanguage',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Dil Kaydetme!",
                        text: "Dil başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Dil Kaydetme!",
                        text: "Dil bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}
function Delete(obj) {
    maindata.LanguageId = obj.id;
    Swal.fire({
        title: "Silmek istediğinize emin misiniz?",
        text: "Silme işlemi gerçekleştikten sonra geri alınamaz!",
        type: "warning",
        showCancelButton: !0,
        confirmButtonColor: "#34c38f",
        cancelButtonColor: "#f46a6a",
        confirmButtonText: "Sil"
    }).then(function (t) {
        if (t.value) {
            $.ajax({
                url: '/api/DeleteLanguage',
                type: 'Delete',
                dataType: 'Json',
                contentType: 'application/json',
                data: JSON.stringify(maindata),
                success: function (data) {
                    if (data) {
                        Swal.fire("Silme İşlemi!", "Silme işlemi başarılı bir şekilde gerçekleşmiştir.", "success");
                        PageLoad();
                    }
                    else {
                        Swal.fire("Silme İşlemi!", "Silme işlemi başarısız olmuştur.", "warning");
                    }
                },
                error: function () {
                    Swal.fire("Silme İşlemi!", "Beklenmedik bir hata ile karşılaşıldı.", "error");
                }

            })

        }

    })
}

var subdata = [];

function btnSubClick(Id) {
    maindata.LanguageId = Id;
    $.ajax({
        url: '/api/FindSubLanguage',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(maindata),
        success: function (data) {
            if (data.state) {
                subdata = [];
                if (data.data.length > 0) {
                    data.data.forEach(function (x) {

                        Keys.forEach(function (y) {
                            if (y.ConversionKeyId == x.ConversionKeyId) {
                                subdata.push({
                                    LanguageId: 0,
                                    ConversionKeyId: x.ConversionKeyId,
                                    ConversionKeyName:y.ConversionKeyName,
                                    ConversionKeyDescription:y.ConversionKeyDescription,
                                    Value: x.Value
                                });
                            }
                        });
                    });
                }
                else {
                    Keys.forEach(function (x) {
                        subdata.push({
                            LanguageId: 0,
                            ConversionKeyId: x.ConversionKeyId,
                            ConversionKeyName: x.ConversionKeyName,
                            ConversionKeyDescription: x.ConversionKeyDescription,
                            Value: ""
                        });
                    });
                }
                var accordion = document.getElementById("accordion");
             accordion.innerHTML = "";
                subdata.forEach(function (x) {
                    accordion.innerHTML += '<div class="card mb-1 shadow-none"><div class="card-header" id="headingOne">' +
                        '<h6 class="m-0"><a href="#' + x.ConversionKeyName + '" class="text-dark collapsed" data-toggle="collapse" aria-expanded="false" aria-controls="collapseOne">' +
                        '' + x.ConversionKeyDescription + " || Key : " + x.ConversionKeyName + '</a></h6></div>' +
                        '<div id="' + x.ConversionKeyName + '" class="collapse pt-3 pb-3" aria-labelledby="headingOne" data-parent="#accordion">' +
                        '<textarea class="form-control " maxlength="225" rows="3"  id="input-' + x.ConversionKeyName + '">' + x.Value + '</textarea> ' +
                        '</div></div>';
                });
            }
        },
        error: function () {
            Swal.fire("Hata !", "Beklenmedik bir hata ile karşılaşıldı.", "error");
        }

    })
}



function SubPostData() {

    var subPostData = [];

    subdata.forEach(function (x) {
        var subrow = {
            LanguageConversionId: 0,
            LanguageId: maindata.LanguageId,
            ConversionKeyId: x.ConversionKeyId,
            Value: document.getElementById("input-" + x.ConversionKeyName).value
        };
        subPostData.push(subrow);
    })
    $.ajax({
        url: '/api/AddSubLanguage',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(subPostData),
        success: function (data) {
            if (data.state) {
                modalTab.style = "display:none;";
                document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                Swal.fire({
                    title: "Dil İçerik Güncelleme!",
                    text: "Dil İçerik başarıyla güncellenmiştir.",
                    type: "success"
                })
                PageLoad();
            }
            else {
                Swal.fire("Dil İçerik Günceleme!", x.message, "warning");
            }
        },
        error: function (x) {
            Swal.fire("Dil İçerik Günceleme!", x.message, "error");
        }

    });
  
}
