var modalTab = document.getElementById("exampleModal");
var modalMain = document.getElementById("mainModal");
var category = document.getElementById("MainCategoryId");
var Domain = document.getElementById("Domain").value;
var newFile = null;
var MainCategory = [];
var Language = [];
var categories = {
    BannerId:0,
    ImageUrl:"",
    Queno:0,
    Title:""
}
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllBanner',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                MainCategory = data.main;
                Language = data.lang;
                var columns = [
                    {
                        "data": "ImageUrl",
                        render: function (x) {
                            return "<img class='img-thumbnail' src='" + Domain + x + "' width='100'/>";
                        }
                    },
                    { "data": "Queno" },
                    {
                        "data": "BannerId",
                        render: function (data) {
                            // return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light edit'  id='" + data + "'  data-toggle='modal' data-target='#exampleModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> ";
                            var resultSnc = '<div class="btn-group" role="group">' +
                                '<button id="btnGroupVerticalDrop1" type="button" class="btn btn-primary btn-sm waves-effect waves-light edit dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fas fa-pencil - alt"></i> Düzenle <i class="mdi mdi-chevron-down"></i></button>' +
                                '<div class="dropdown-menu" aria-labelledby="btnGroupVerticalDrop1">';
                            resultSnc += '<a class="dropdown-item edit" id="' + data + '" data-toggle="modal" data-target="#mainModal" onclick="btnClick(this)">Yazı Düzenle </a>';
                            //Language.forEach(function (x) {
                            //    resultSnc += '<a class="dropdown-item ' + x.LanguageId + '-' + data + '" onclick="btnSubClick(' + data + "," + x.LanguageId + ')" data-toggle="modal" data-target="#exampleModal">' + x.LanguageName + ' </a>';
                            //});
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
        document.getElementById("modalTitle").innerHTML = "Banner Ekle";
        document.getElementById("btnSumbit").innerHTML = "Banner Yazı Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Banner Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Banner Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    document.getElementById("Title").value = "";
    document.getElementById("Queno").value = 1;
    newFile = null;
    document.getElementById("SilinecekResimler").innerHTML = "";
}
function Find(id) {
    categories.BannerId = id;
    $.ajax({
        url: '/api/FindBanner',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(categories),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                document.getElementById("Title").value = data.data.Title;
                document.getElementById("Queno").value = data.data.Queno;
                categories.ImageUrl = data.data.ImageUrl;
                newFile = data.data.ImageUrl;
                document.getElementById("SilinecekResimler").innerHTML = '<img src="' + data.data.ImageUrl + '" class="' + data.data.ImageUrl + '" width="80" height="auto"/> <a onclick="DeleteSingleFile(this)" id="' + data.data.ImageUrl + '" > Sil </a>';
            }
        }
    });

}


function PostData(obj) {
    categories.Title = document.getElementById("Title").value;
    categories.Queno = document.getElementById("Queno").value;

    if (obj.innerHTML == "Banner Güncelle") {
        if (Imgdata.length > 0) {
            DeleteSingleFile('<img src="' + newFile + '" class="' + newFile + '" width="80" height="auto"/> <a onclick="DeleteSingleFile(this)" id="' + newFile + '" > Sil </a>');
            newFile = Imgdata[0].FileUrl;
        }
        if (newFile != undefined) {
            categories.ImageUrl = newFile.replace("~", "");;
        }
        $.ajax({
            url: '/api/UpdateBanner',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(categories),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Banner Güncelleme!",
                        text: "Banner başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Banner Güncelleme!",
                        text: "Banner bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        categories.BannerId = 0;
        categories.ImageUrl = (Imgdata[0].FileUrl).replace("~", "");
        $.ajax({
            url: '/api/AddBanner',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(categories),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Banner Kaydetme!",
                        text: "Banner başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Banner Kaydetme!",
                        text: "Banner bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}

function Delete(obj) {
    categories.BannerId = obj.id;
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
                url: '/api/DeleteBanner',
                type: 'Delete',
                dataType: 'Json',
                contentType: 'application/json',
                data: JSON.stringify(categories),
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
    SliderConversionId:0,
    BannerId: 0,
    PostUrl: "",
    LanguageId: 0
}
function btnSubClick(Id, LangId) {
    SubData.BannerId = Id;
    SubData.LanguageId = LangId;
    $.ajax({
        url: '/api/FindSubBanner',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(SubData),
        success: function (data) {
            if (data.state && data.data != null) {
                SubData = data.data;
                document.getElementById("PostUrl").value = SubData.PostUrl;
            }
            else {
                document.getElementById("PostUrl").value = "";
                SubData.SliderConversionId = 0;
            }
        },
        error: function () {
            Swal.fire("Hata !", "Beklenmedik bir hata ile karşılaşıldı.", "error");
        }

    })
}



function SubPostData() {

    SubData.PostUrl = document.getElementById("PostUrl").value;
   

    if (SubData.SliderConversionId != 0) {
        $.ajax({
            url: '/api/UpdateSubBanner',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(SubData),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Banner İçerik Güncelleme!",
                        text: "Banner İçerik başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire("Banner İçerik Günceleme!", x.message, "warning");
                }
            },
            error: function (x) {
                Swal.fire("Banner İçerik Günceleme!", x.message, "error");
            }

        });
    }
    else {
        $.ajax({
            url: '/api/AddSubBanner',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(SubData),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Banner İçerik Ekleme!",
                        text: "Banner İçerik başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire("Banner İçerik Ekleme!", x.message, "warning");
                }
            },
            error: function (x) {
                Swal.fire("Banner İçerik Ekleme!", x.message, "error");
            }
        });
    }
}
