var modalTab = document.getElementById("exampleModal");
var modalMain = document.getElementById("mainModal");
var category = document.getElementById("MainCategoryId");
var Domain = document.getElementById("Domain").value;
var newFile = null;
var MainCategory = [];
var Language = [];
var categories = {
    CategoryId: 0,
    Title: "",
    ImageUrl: "",
    MainCategoryId: 0,
    Queno: 0,
    CategoryType: 5,
    IsBlog: false
}
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllProducts',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                MainCategory = data.main;
                Language = data.lang;
                var columns = [
                    //{
                    //    "data": "ImageUrl",
                    //    render: function (x) {
                    //        return "<img class='img-thumbnail' src='" + Domain + x+"' width='100'/>";
                    //    }
                    //},
                    { "data": "Title" },
                    {
                        "data": "MainCategoryId",
                        render: function (x) {
                            var result = "";
                            MainCategory.forEach(function (y) {
                                if (y.CategoryId == x) {
                                    result = y.Title;
                                }
                            });
                            return result;
                        }
                    },
                    { "data": "Queno" },
                
                    {
                        "data": "CategoryId",
                        render: function (data) {
                            // return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light edit'  id='" + data + "'  data-toggle='modal' data-target='#exampleModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> ";
                            var resultSnc = '<div class="btn-group" role="group">' +
                                '<button id="btnGroupVerticalDrop1" type="button" class="btn btn-primary btn-sm waves-effect waves-light edit dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fas fa-pencil - alt"></i> Düzenle <i class="mdi mdi-chevron-down"></i></button>' +
                                '<div class="dropdown-menu" aria-labelledby="btnGroupVerticalDrop1">';
                            resultSnc += '<a class="dropdown-item edit" id="' + data + '" data-toggle="modal" data-target="#mainModal" onclick="btnClick(this)">Ürün Düzenle </a>';
                            Language.forEach(function (x) {
                                resultSnc += '<a class="dropdown-item ' + x.LanguageId + '-' + data + '" onclick="btnSubClick(' + data + "," + x.LanguageId + ')" data-toggle="modal" data-target="#exampleModal">' + x.LanguageName + ' </a>';
                            });  
                            resultSnc += '</div></div>';
                            resultSnc += " | <a onclick='Delete(this)' class='btn btn-danger btn-sm waves-effect waves-light text-white delete'  id='" + data + "' ><i class='fas fa-trash'></i> Sil</a>";
                            return resultSnc;


                        }
                    }
                ];
                DatatablesLoad("datatables",data.data, columns);
                category.innerHTML = "";
                category.innerHTML = "<option value='0'> Lütfen Kategori Seçiniz..</option>";
                data.main.forEach(function (element) {
                    category.innerHTML += "<option value='" + element.CategoryId + "'>" + element.Title + "</option>";
                });
            }
        }
    });
   
}

function btnClick(obj) {
    if (obj.className.includes("insert")) {
        document.getElementById("modalTitle").innerHTML = "Ürün Ekle";
        document.getElementById("btnSumbit").innerHTML = "Yeni Ürün Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Ürün Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Ürün Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    document.getElementById("Title").value = "";
    document.getElementById("MainCategoryId").value = 0;
    document.getElementById("Queno").value = 1;
    document.getElementById("Title").value = "";
    newFile = null;
    document.getElementById("SilinecekResimler").innerHTML = "";
}
function Find(id) {
    categories.CategoryId = id;
    $.ajax({
        url: '/api/FindProduct',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(categories),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                document.getElementById("Title").value = data.data.Title;
                document.getElementById("MainCategoryId").value = data.data.MainCategoryId;
                document.getElementById("Queno").value = data.data.Queno;
                //newFile = data.data.ImgUrl;
            //    document.getElementById("SilinecekResimler").innerHTML = '<img src="' + data.data.ImageUrl + '" class="' + data.data.ImageUrl + '" width="80" height="auto"/> <a onclick="DeleteSingleFile(this)" id="' + data.data.ImageUrl + '" > Sil </a>';
            }
        }
    });

}


function PostData(obj) {
    categories.Title = document.getElementById("Title").value;
    categories.MainCategoryId = document.getElementById("MainCategoryId").value;
    categories.Queno = document.getElementById("Queno").value;

    if (obj.innerHTML == "Kategori Güncelle") {
        if (Imgdata.length > 0) {
          DeleteSingleFile('<img src="' + newFile + '" class="' + newFile + '" width="80" height="auto"/> <a onclick="DeleteSingleFile(this)" id="' + newFile + '" > Sil </a>');
            newFile = Imgdata[0].FileUrl;
        }
        //categories.ImageUrl = newFile.replace("~", "");;
        $.ajax({
            url: '/api/UpdateProduct',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(categories),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Ürün Güncelleme!",
                        text: "Ürün başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Ürün Güncelleme!",
                        text: "Ürün bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        categories.CategoryId = 0;
        //categories.ImageUrl = (Imgdata[0].FileUrl).replace("~", "");
        $.ajax({
            url: '/api/AddProduct',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(categories),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Ürün Kaydetme!",
                        text: "Ürün başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Ürün Kaydetme!",
                        text: "Ürün bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}

function Delete(obj) {
    categories.CategoryId = obj.id;
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
                url: '/api/DeleteProduct',
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
var urlRecord = {
    UrlRecordId: 0,
    Keywords: "",
    Description: "",
    SeoUrl: "",
    Title: "",
    Content: "",
    LanguageId: 0,
    CategoryId: 0,
    SecurityObjectId: 0,
}
function btnSubClick(Id, LangId) {
    urlRecord.CategoryId = Id;
    urlRecord.LanguageId = LangId;
    $.ajax({
        url: '/api/FindSubProduct',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(urlRecord),
        success: function (data) {
            if (data.state && data.data != null) {
                document.getElementById("LanguageName").value = data.data.LanguageName;
                document.getElementById("SubTitle").value = data.data.Title;
                $("#Content").summernote("code", data.data.Content);
                document.getElementById("Keywords").value = data.data.Keywords;
                document.getElementById("Description").value = data.data.Description; 
              
                urlRecord = data.data;
                document.getElementById("SeoUrl").value = data.data.SeoUrl;
            }
            else {
                document.getElementById("LanguageName").value ="";
                document.getElementById("SubTitle").value = "";
                $("#Content").summernote("code","");
                document.getElementById("Keywords").value = "";
                document.getElementById("Description").value = "";
                document.getElementById("SubCategoryName").value = "";
                document.getElementById("SeoUrl").value = "";
                urlRecord.UrlRecordId = 0;
                Language.forEach(function (x) {
                    if (urlRecord.LanguageId == x.LanguageId) {
                        document.getElementById("LanguageName").value = x.LanguageName;
                    }
                });
            }
            document.getElementById("SubCategoryName").value = data.mainCategoryName;
        },
        error: function () {
            Swal.fire("Hata !", "Beklenmedik bir hata ile karşılaşıldı.", "error");
        }

    })
}

function OtoUrl() {
    var slan = "";
    Language.forEach(function (x) {
        if (document.getElementById("LanguageName").value == x.LanguageName) {
            slan = url_slug(x.ShortCode);
        }
    });

    if (document.getElementById("SubCategoryName").value.length > 3) {
        document.getElementById("SeoUrl").value = '/' + slan + "/" + url_slug(document.getElementById("SubCategoryName").value) + '/' + url_slug($("#SubTitle").val())
    }
    else {
        document.getElementById("SeoUrl").value = '/' + slan + "/" + url_slug($("#SubTitle").val() )
    }
}

function SubPostData() {
 
    urlRecord.Keywords = document.getElementById("Keywords").value;
    urlRecord.Description = document.getElementById("Description").value;
    urlRecord.SeoUrl = document.getElementById("SeoUrl").value;
    urlRecord.Title = document.getElementById("SubTitle").value;
    urlRecord.Content = $('#Content').summernote('code');
    console.clear();
    console.log(urlRecord);
    if (urlRecord.UrlRecordId != 0) {
        $.ajax({
            url: '/api/UpdateSubProduct',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(urlRecord),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Ürün Güncelleme!",
                        text: "Ürün başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire("Ürün Günceleme!", x.message, "warning");
                }
            },
            error: function (x) {
                Swal.fire("Ürün Günceleme!", x.message, "error");
            }

        });
    }
    else {
        $.ajax({
            url: '/api/AddSubProduct',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(urlRecord),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Ürün Ekleme!",
                        text: "Ürün başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire("Ürün Ekleme!", x.message, "warning");
                }
            },
            error: function (x) {
                Swal.fire("Ürün Ekleme!", x.message, "error");
            }
        });
    }
}
