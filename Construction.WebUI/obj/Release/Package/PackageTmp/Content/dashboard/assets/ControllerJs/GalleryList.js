var modalTab = document.getElementById("exampleModal");
var modalMain = document.getElementById("mainModal");
var category = document.getElementById("MainCategoryId");
var Domain = document.getElementById("Domain").value;
var newFile = null;
var MainCategory = [];
var Language = [];
$(document).ready(function () {
    PageLoad();
});
var GalleryMain = {
    GalleryId:0,
    Url:"",
    Title:"",
    CreateDate:""
}
function PageLoad() {
    $.ajax({
        url: '/api/GetAllGallery',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                MainCategory = data.main;
                Language = data.lang;
                var columns = [
                    {
                        "data": "Url",
                        render: function (x) {
                            return "<img class='img-thumbnail' src='" + Domain + x + "' width='100'/>";
                        }
                    },
                    { "data": "Title" },
                    { "data": "CreateDate" },
                    {
                        "data": "GalleryId",
                        render: function (data) {
                            // return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light edit'  id='" + data + "'  data-toggle='modal' data-target='#exampleModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> ";
                            var resultSnc = '<div class="btn-group" role="group">' +
                                '<button id="btnGroupVerticalDrop1" type="button" class="btn btn-primary btn-sm waves-effect waves-light edit dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fas fa-pencil - alt"></i> Düzenle <i class="mdi mdi-chevron-down"></i></button>' +
                                '<div class="dropdown-menu" aria-labelledby="btnGroupVerticalDrop1">';
                            resultSnc += '<a class="dropdown-item edit" id="' + data + '" data-toggle="modal" data-target="#mainModal" onclick="btnClick(this)">Düzenle </a>';
                            Language.forEach(function (x) {
                                resultSnc += '<a class="dropdown-item ' + x.LanguageId + '-' + data + '" onclick="btnSubClick(' + data + "," + x.LanguageId + ')" data-toggle="modal" data-target="#exampleModal">' + x.LanguageName + ' </a>';
                            });
                            resultSnc += '</div></div>';
                            resultSnc += " | <a onclick='Delete(this)' class='btn btn-danger btn-sm waves-effect waves-light text-white delete'  id='" + data + "' ><i class='fas fa-trash'></i> Sil</a>";
                            return resultSnc;


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
        document.getElementById("modalTitle").innerHTML = "Galeri Ekle";
        document.getElementById("btnSumbit").innerHTML = "Galeri Yazı Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Galeri Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Galeri Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    document.getElementById("Title").value = "";
    newFile = null;
    document.getElementById("SilinecekResimler").innerHTML = "";
}
function Find(id) {
    GalleryMain.GalleryId = id;
    $.ajax({
        url: '/api/FindGallery',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(categories),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                document.getElementById("Title").value = data.data.Title;
                newFile = data.data.Url;
                document.getElementById("SilinecekResimler").innerHTML = '<img src="' + data.data.Url + '" class="' + data.data.Url + '" width="80" height="auto"/> <a onclick="DeleteSingleFile(this)" id="' + data.data.ImageUrl + '" > Sil </a>';
            }
        }
    });

}


function PostData(obj) {
    GalleryMain.Title = document.getElementById("Title").value;
    if (obj.innerHTML == "Galeri Güncelle") {
        if (Imgdata.length > 0) {
            DeleteSingleFile('<img src="' + newFile + '" class="' + newFile + '" width="80" height="auto"/> <a onclick="DeleteSingleFile(this)" id="' + newFile + '" > Sil </a>');
            newFile = Imgdata[0].FileUrl;
        }
        GalleryMain.Url = newFile.replace("~", "");;
        $.ajax({
            url: '/api/UpdateGallery',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(GalleryMain),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Galeri Güncelleme!",
                        text: "Galeri başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Galeri Güncelleme!",
                        text: "Galeri bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        GalleryMain.GalleryId = 0;
        GalleryMain.Url = (Imgdata[0].FileUrl).replace("~", "");
        $.ajax({
            url: '/api/AddGallery',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(GalleryMain),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Galeri Kaydetme!",
                        text: "Galeri başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Galeri Kaydetme!",
                        text: "Galeri bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}

function Delete(obj) {
    GalleryMain.GalleryId = obj.id;
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
                url: '/api/DeleteGallery',
                type: 'Delete',
                dataType: 'Json',
                contentType: 'application/json',
                data: JSON.stringify(GalleryMain),
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
var SubData = {
    GalleryConversionId:0,
    GalleryId: 0,
    Title: "",
    LanguageId: 0
}
function btnSubClick(Id, LangId) {
    SubData.GalleryId = Id;
    SubData.LanguageId = LangId;
    $.ajax({
        url: '/api/FindSubGallery',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(SubData),
        success: function (data) {
            if (data.state && data.data != null) {
                SubData = data.data;
                document.getElementById("SubTitle").value = SubData.Title;
            }
            else {
                document.getElementById("SubTitle").value = "";
                SubData.GalleryConversionId = 0;
            }
        },
        error: function () {
            Swal.fire("Hata !", "Beklenmedik bir hata ile karşılaşıldı.", "error");
        }

    })
}



function SubPostData() {

    SubData.Title = document.getElementById("SubTitle").value;
    if (SubData.GalleryConversionId != 0) {
        $.ajax({
            url: '/api/UpdateSubGallery',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(SubData),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Galeri İçerik Güncelleme!",
                        text: "Galeri İçerik başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire("Galeri İçerik Günceleme!", x.message, "warning");
                }
            },
            error: function (x) {
                Swal.fire("Galeri İçerik Günceleme!", x.message, "error");
            }

        });
    }
    else {
        $.ajax({
            url: '/api/AddSubGallery',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(SubData),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Galeri İçerik Ekleme!",
                        text: "Galeri İçerik başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire("Galeri İçerik Ekleme!", x.message, "warning");
                }
            },
            error: function (x) {
                Swal.fire("Galeri İçerik Ekleme!", x.message, "error");
            }
        });
    }
}
