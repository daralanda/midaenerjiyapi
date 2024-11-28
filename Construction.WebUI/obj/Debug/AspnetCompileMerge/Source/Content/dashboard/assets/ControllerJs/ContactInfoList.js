var modalMain = document.getElementById("mainModal");
var Domain = document.getElementById("Domain").value;
var maindata = {
    ContactInfoId:0,
    Name:"",
    Phone: "",
    Fax: "",
    Email:"",
    Address: "",
    MapFrame:"",
    IsActive: false,
    IsDefault: false
}
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllContactInfo',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                var columns = [
                    { "data": "Name" },
                    { "data": "Phone" },
                    { "data": "Email" },
                    {
                        "data": "IsActive",
                        render: function (x) {
                            return Chekbox(x, true);
                        }
                    },
                    {
                        "data": "IsDefault",
                        render: function (x) {
                            return Chekbox(x, true);
                        }
                    },
                    {
                        "data": null,
                        render: function (data, type, row) {
                            return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light text-white edit'  id='" + data.ContactInfoId + "'  data-toggle='modal' data-target='#mainModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> " +
                                "<a onclick='Delete(this)' class='btn btn-danger btn-sm waves-effect waves-light text-white delete'  id='" + data.ContactInfoId + "' ><i class='fas fa-pencil-alt'></i> Sil</a> ";
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
        document.getElementById("modalTitle").innerHTML = "İletişim Bilgisi Ekle";
        document.getElementById("btnSumbit").innerHTML = "Yeni İletişim Bilgisi Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "İletişim Bilgisi Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "İletişim Bilgisi Güncelle";
        Find(obj.id);
    }
}
function FormClean() {
    document.getElementById("Name").value = "";
    document.getElementById("Phone").value = "";
    document.getElementById("Fax").value = "";
    document.getElementById("Email").value = "";
    document.getElementById("Address").value = "";
    document.getElementById("MapFrame").value = "";
    document.getElementById("IsActive").value = false;
    document.getElementById("IsDefault").value = false;

}
function Find(id) {
    maindata.ContactInfoId = id;
    $.ajax({
        url: '/api/FindContactInfo',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(maindata),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                maindata = data.data;
                document.getElementById("Name").value = maindata.Name;
                document.getElementById("Phone").value = maindata.Phone;
                document.getElementById("Fax").value = maindata.Fax;
                document.getElementById("Email").value = maindata.Email;
                document.getElementById("Address").value = maindata.Address;
                document.getElementById("MapFrame").value = maindata.MapFrame;
                document.getElementById("IsActive").value = maindata.IsActive;
                document.getElementById("IsDefault").value = maindata.IsDefault;
            }
        }
    });

}
function PostData(obj) {
    maindata.Name = document.getElementById("Name").value ;
    maindata.Phone = document.getElementById("Phone").value;
    maindata.Fax = document.getElementById("Fax").value ;
    maindata.Email=document.getElementById("Email").value;
    maindata.Address = document.getElementById("Address").value;
     maindata.MapFrame=document.getElementById("MapFrame").value ;
    maindata.IsActive=document.getElementById("IsActive").value ;
    maindata.IsDefault = document.getElementById("IsDefault").value;
    if (obj.innerHTML == "İletişim Bilgisi Güncelle") {
        $.ajax({
            url: '/api/UpdateContactInfo',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "İletişim Bilgisi Güncelleme!",
                        text: "İletişim Bilgisi başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "İletişim Bilgisi Güncelleme!",
                        text: "İletişim Bilgisi bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        maindata.ContactInfoId = 0;
        $.ajax({
            url: '/api/AddContactInfo',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(maindata),
            success: function (data) {
                if (data.state) {
                    modalMain.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "İletişim Bilgisi Kaydetme!",
                        text: "İletişim Bilgisi başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "İletişim Bilgisi Kaydetme!",
                        text: "İletişim Bilgisi bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}
function Delete(obj) {
    maindata.ContactInfoId = obj.id;
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
                url: '/api/DeleteContactInfo',
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
