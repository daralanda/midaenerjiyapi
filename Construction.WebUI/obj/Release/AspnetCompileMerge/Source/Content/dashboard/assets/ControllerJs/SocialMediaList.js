var modalMain = document.getElementById("mainModal");
var Domain = document.getElementById("Domain").value;
var maindata = {
    SocialMediaId:0,
    PostUrl:"",
    Icon:"",
    IsActive: false,
    Queno:0,
    SocialMediaName: "",
}
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllSocialMedia',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                var columns = [
                    { "data": "SocialMediaName"},
                    {
                        "data": "Icon",
                        render: function (x) {
                            return "<i class='" + x + "'></i>";
                        }
                    },
                    {
                        "data": "PostUrl",
                        render: function (x) {
                            return "<a class='btn btn-sm btn-success' href='"+x+"' target='_blank'> Yeni Sekmede Gör</a>";
                        }
                    },
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
                            return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light text-white edit'  id='" + data.SocialMediaId + "'  data-toggle='modal' data-target='#mainModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> " +
                                "<a onclick='Delete(this)' class='btn btn-danger btn-sm waves-effect waves-light text-white delete'  id='" + data.SocialMediaId + "' ><i class='fas fa-pencil-alt'></i> Sil</a> ";
                        }
                    }
                ];
                DatatablesLoad("datatables", data.data, columns);
             
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
        document.getElementById("modalTitle").innerHTML = "Sosyal Medya Ekle";
        document.getElementById("btnSumbit").innerHTML = "Yeni Sosyal Medya Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Sosyal Medya Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Sosyal Medya Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    document.getElementById("Icon").value = "";
    document.getElementById("PostUrl").value = "";
    document.getElementById("SocialMediaName").value = "";
    document.getElementById("Queno").value = 0;
    document.getElementById("IsActive").value = false;
}
function Find(id) {
    maindata.SocialMediaId = id;
    $.ajax({
        url: '/api/FindSocialMedia',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(maindata),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                maindata = data.data;
                document.getElementById("SocialMediaName").value = maindata.SocialMediaName;
                document.getElementById("Icon").value = maindata.Icon;
                document.getElementById("Queno").value = maindata.Queno;
                document.getElementById("PostUrl").value = maindata.PostUrl;               
                document.getElementById("IsActive").value = maindata.IsActive;
            }
        }
    });

}
function PostData(obj) {
    maindata.SocialMediaName = document.getElementById("SocialMediaName").value;
    maindata.Icon = document.getElementById("Icon").value;
    maindata.Queno = document.getElementById("Queno").value;
    maindata.IsActive = document.getElementById("IsActive").value;
    maindata.PostUrl = document.getElementById("PostUrl").value;
    if (obj.innerHTML == "Sosyal Medya Güncelle") {
        $.ajax({
            url: '/api/UpdateSocialMedia',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Sosyal Medya Güncelleme!",
                        text: "Sosyal Medya başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Sosyal Medya Güncelleme!",
                        text: "Sosyal Medya bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        maindata.SocialMediaId = 0;
        $.ajax({
            url: '/api/AddSocialMedia',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Sosyal Medya Kaydetme!",
                        text: "Sosyal Medya başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Sosyal Medya Kaydetme!",
                        text: "Sosyal Medya bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}
function Delete(obj) {
    maindata.SocialMediaId = obj.id;
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
                url: '/api/DeleteSocialMedia',
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
