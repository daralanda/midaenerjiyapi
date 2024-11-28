var modalMain = document.getElementById("mainModal");
var Domain = document.getElementById("Domain").value;
var category = document.getElementById("MainFooterPluginId");
var dil = document.getElementById("LanguageId");

var mainCat = [];
var Lang = [];
var maindata = {
    FooterPluginId:0,
    Title: "",
    PostUrl: "",
    MainFooterPluginId: 0,
    LanguageId: 0,
    Direction: false,
}
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllFooterPlugin',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                mainCat = data.main;
                Lang = data.lang;
                var columns = [
                    { "data":"CatName" },
                    { "data": "Title" },
                    { "data": "LanguageName" },
                    {
                        "data": "Direction",
                        render: function (x) {
                            if (x) {
                                return "Sol";
                            }
                            else {
                                return "Sağ";
                            }
                        }
                    },
                    {
                        "data": null,
                        render: function (data, type, row) {
                            return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light text-white edit'  id='" + data.FooterPluginId + "'  data-toggle='modal' data-target='#mainModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> " +
                                "<a onclick='Delete(this)' class='btn btn-danger btn-sm waves-effect waves-light text-white delete'  id='" + data.FooterPluginId + "' ><i class='fas fa-pencil-alt'></i> Sil</a> ";
                        }
                    }
                ];
                data.data.forEach(function (x) {
                    mainCat.forEach(function (y) {
                        if (x.MainFooterPluginId == y.FooterPluginId) {
                            x.CatName = y.Title;
                        }
                        else {
                            x.CatName = "";
                        }
                    })
                })
                console.clear();
                console.log(data.data);
                DatatablesLoad("datatables", data.data, columns);
                category.innerHTML = "";
                category.innerHTML = "<option value='0'> Lütfen Kategori Seçiniz..</option>";
                mainCat.forEach(function (element) {
                    category.innerHTML += "<option value='" + element.FooterPluginId + "'>" + element.Title + "</option>";
                });
                dil.innerHTML = "";
                dil.innerHTML = "<option value='0'> Lütfen Dil Seçiniz.. (Zorunlu Alan)</option>";
                Lang.forEach(function (element) {
                    dil.innerHTML += "<option value='" + element.LanguageId + "'>" + element.LanguageName + "</option>";
                });
            }
        },
        error: function (data) {
            console.clear();
            console.log(data);
        }
    });

}
function btnClick(obj) {
    if (obj.className.includes("insert")) {
        document.getElementById("modalTitle").innerHTML = "Alt Bilgi Ekle";
        document.getElementById("btnSumbit").innerHTML = "Yeni Alt Bilgi Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Alt Bilgi Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Alt Bilgi Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    document.getElementById("Title").value = "";
    document.getElementById("PostUrl").value = "";
    document.getElementById("MainFooterPluginId").value = 0;
    document.getElementById("LanguageId").value = 0;
    document.getElementById("Direction").value = null;
}
function Find(id) {
    maindata.FooterPluginId = id;
    $.ajax({
        url: '/api/FindFooterPlugin',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(maindata),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                maindata = data.data;
                document.getElementById("Title").value = maindata.Title;
                document.getElementById("PostUrl").value = maindata.PostUrl;
                document.getElementById("MainFooterPluginId").value = maindata.MainFooterPluginId;
                document.getElementById("LanguageId").value = maindata.LanguageId;
                document.getElementById("Direction").value = maindata.Direction;
            }
        }
    });

}
function PostData(obj) {
    maindata.Title = document.getElementById("Title").value ;
    maindata.PostUrl = document.getElementById("PostUrl").value ;
    maindata.MainFooterPluginId = document.getElementById("MainFooterPluginId").value ;
    maindata.LanguageId=document.getElementById("LanguageId").value ;
    maindata.Direction = document.getElementById("Direction").value ;
    if (obj.innerHTML == "Alt Bilgi Güncelle") {
        $.ajax({
            url: '/api/UpdateFooterPlugin',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Alt Bilgi Güncelleme!",
                        text: "Alt Bilgi başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Alt Bilgi Güncelleme!",
                        text: "Alt Bilgi bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        maindata.FooterPluginId = 0;
        $.ajax({
            url: '/api/AddFooterPlugin',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Alt Bilgi Kaydetme!",
                        text: "Alt Bilgi başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Alt Bilgi Kaydetme!",
                        text: "Alt Bilgi bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}
function Delete(obj) {
    maindata.VerificationId = obj.id;
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
                url: '/api/DeleteFooterPlugin',
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
