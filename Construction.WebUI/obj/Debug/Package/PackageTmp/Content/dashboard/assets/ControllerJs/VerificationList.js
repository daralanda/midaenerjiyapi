var modalMain = document.getElementById("mainModal");
var Domain = document.getElementById("Domain").value;
var maindata = {
    VerificationId: 0,
    VerificationKey:"",
    VerificationValue: ""
}
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllVerification',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                var columns = [
                    { "data": "VerificationKey" },
                    { "data": "VerificationValue" },
                    {
                        "data": null,
                        render: function (data, type, row) {
                            return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light text-white edit'  id='" + data.VerificationId + "'  data-toggle='modal' data-target='#mainModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> " +
                                "<a onclick='Delete(this)' class='btn btn-danger btn-sm waves-effect waves-light text-white delete'  id='" + data.VerificationId + "' ><i class='fas fa-pencil-alt'></i> Sil</a> ";
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
        document.getElementById("modalTitle").innerHTML = "Doğrulama Ekle";
        document.getElementById("btnSumbit").innerHTML = "Yeni Doğrulama Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Doğrulama Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Doğrulama Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    
    
    document.getElementById("VerificationKey").value = "";
    document.getElementById("VerificationValue").value = "";
}
function Find(id) {
    maindata.VerificationId = id;
    $.ajax({
        url: '/api/FindVerification',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(maindata),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                maindata = data.data;
                document.getElementById("VerificationKey").value = maindata.VerificationKey;
                document.getElementById("VerificationValue").value = maindata.VerificationValue;
            }
        }
    });

}
function PostData(obj) {
    maindata.VerificationKey = document.getElementById("VerificationKey").value ;
    maindata.VerificationValue = document.getElementById("VerificationValue").value;

    if (obj.innerHTML == "Doğrulama Güncelle") {
        $.ajax({
            url: '/api/UpdateVerification',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Doğrulama Güncelleme!",
                        text: "Doğrulama başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Doğrulama Güncelleme!",
                        text: "Doğrulama bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        maindata.VerificationId = 0;
        $.ajax({
            url: '/api/AddVerification',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Doğrulama Kaydetme!",
                        text: "Doğrulama başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Doğrulama Kaydetme!",
                        text: "Doğrulama bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
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
                url: '/api/DeleteVerification',
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
