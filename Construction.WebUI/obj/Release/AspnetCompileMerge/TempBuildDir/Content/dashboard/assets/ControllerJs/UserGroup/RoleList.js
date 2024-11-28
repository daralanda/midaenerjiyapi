var modalTab = document.getElementById("exampleModal");

var RoleTD = [];
var RolePluginsTD = [];
var category = document.getElementById("category");
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllRoles',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                RoleTD = data.data;
                var columns = [
                    { "data": "RoleName" },
                    {
                        "data": "IsActive",
                        render: function (data) {
                            return Chekbox(data, true);
                        }
                    },
                    {
                        "data": "RoleId",
                        render: function (data) {
                            return "<a onclick='btnClick(this)' class='btn btn-primary btn-sm waves-effect waves-light text-white edit'  id='" + data + "'  data-toggle='modal' data-target='#exampleModal'><i class='fas fa-pencil-alt'></i> Düzenle</a> ";                        }
                    }
                ];
                DatatablesLoad("datatables",data.data, columns);
                category.innerHTML = "";
                category.innerHTML = "<option> Lütfen Kategori Seçiniz..</option>";
                data.main.forEach(function (element) {
                    category.innerHTML += "<option value='" + element.SecurityObjectId + "'>" + element.SecurityObjectName + "</option>";
                });
                RolePluginsTD = data.plugins;
            }

        }
    });
}

function btnClick(obj) {
    var Role = {
        "RoleId": obj.id,
        "RoleName": "",
        "IsActive": "",
    };
    if (obj.className.includes("insert")) {
        document.getElementById("modalTitle").innerHTML = "Yetki Grubu Ekle";
        document.getElementById("btnSumbit").innerHTML = "Yeni Yetki Grubu Kaydet";
        FormClean();
    }
    else if (obj.className.includes("edit")) {
        document.getElementById("modalTitle").innerHTML = "Yetki Grubu Güncelleme";
        document.getElementById("btnSumbit").innerHTML = "Yetki Grubu Güncelle";
        FindRole(obj.id);
    }
}

function FormClean() {
    PageLoad();
    document.getElementById("rolename").value = "";
    document.getElementById("rolename").disabled = false;
    document.getElementById("isactive").value = 0;
    document.getElementById("category").selectedIndex = 0;
    $('#plugin-datatable').DataTable().clear().draw();
}
var RoleId = 0;
function FindRole(obj) {
    RoleId = obj;
    $('#plugin-datatable').DataTable().clear().draw();
    RoleTD.forEach(function (element) {
        if (element.RoleId == obj) {
            document.getElementById("rolename").value = element.RoleName;
            document.getElementById("rolename").disabled = true;
            document.getElementById("category").selectedIndex = 0;
            if (element.IsActive) {
                document.getElementById("isactive").value = 1;
            }
            else {
                document.getElementById("isactive").value = 0;
            }
        }
    })
    var xRole = {
        "RoleId": obj,
        "RoleName": "",
        "IsActive": "",
    };
    $.ajax({
        url: '/api/FindRolePermission',
        type: 'Post',
        dataType: 'Json',
        data: JSON.stringify(xRole),
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                if (data.data.length>0) {
                    data.data.find(DataAdd);
                }
                else {
                    PageLoad();
                }
            }
        }
    });

}

function DataAdd(element) {
    RolePluginsTD.forEach(function (item) {
        if (item.SecurityObjectId == element.SecurityObjectId) {
            item.IsSelect = element.IsSelect;
            item.IsUpdate = element.IsUpdate;
            item.IsDelete = element.IsDelete;
            item.IsInsert = element.IsInsert;
        }
    });
}
var pluginsSessionData = [];
function DataFind() {
    pluginsSessionData = [];
    var Id = document.getElementById("category").value;
    RolePluginsTD.forEach(function (element) {
        if (element.SecurityObjectId == Id ) {
            pluginsSessionData.push(element);
        }
    });
    RolePluginsTD.forEach(function (element) {
        if (element.MainSecurityObjectId == Id) {
            pluginsSessionData.push(element);
        }
    });
    PlugigTableLoad(pluginsSessionData);
}

function PlugigTableLoad(data) {
    $('#plugin-datatable').DataTable({
        "data": data,
        "destroy": true,
        "createdRow": function (row, data) {
            if (data.MainSecurityObjectId == null) {
                $(row).addClass('important');
            }
        },
        "searching": false,
        "ordering": false,
        "columns": [
            {
                "data": "SecurityObjectName"
            },
            {
                "data": function (data) {
                    var Nesne = "IsSelect";
                    if (data.IsSelect) {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" class="' + Nesne + '" checked>' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                    else {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" class="' + Nesne + '">' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                }
            },
            {
                "data": function (data) {
                    var Nesne = "IsUpdate";
                    if (data.IsUpdate) {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" checked class="' + Nesne + '">' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                    else {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" class="' + Nesne + '">' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                }
            },
            {
                "data": function (data) {
                    var Nesne = "IsInsert";
                    if (data.IsInsert) {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" checked class="' + Nesne + '">' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                    else {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" class="' + Nesne + '">' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                }
            },
            {
                "data": function (data) {
                    var Nesne = "IsDelete";
                    if (data.IsDelete) {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" checked class="' + Nesne + '">' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                    else {
                        return '<label class="container-checkbox">' +
                            '<input type="checkbox" id="' + data.SecurityObjectId + '" onclick="chkClick(this)" class="' + Nesne + '">' +
                            ' <span class="checkmark"></span>' +
                            '</label>';
                    }
                }
            }
        ],
        language: {
            "url": '/Content/Dashboard/assets/language/tr.json',
            paginate: {
                previous: "<i class='mdi mdi-chevron-left'>",
                next: "<i class='mdi mdi-chevron-right'>"
            }
        }
    });
}

function chkClick(obj) {
    RolePluginsTD.forEach(function (element) {
        if (element.SecurityObjectId == obj.id) {
            switch (obj.className) {
                case "IsSelect":
                    element.IsSelect = obj.checked;
                    break;
                case "IsInsert":
                    element.IsInsert = obj.checked;
                    break;
                case "IsUpdate":
                    element.IsUpdate = obj.checked;
                    break;
                case "IsDelete":
                    element.IsDelete = obj.checked;
                    break;
            }
        }
    });

}

function PostData(obj) {
    var IsActive = false;
    if (document.getElementById("isactive").value == 1) {
        IsActive = true;
    }
    var role={
        'RoleId': RoleId,
        'IsActive': IsActive,
        'RoleName': document.getElementById("rolename").value,
        'RolePermissions': RolePluginsTD
    }
    console.clear();
    console.log(role);
    if (obj.innerHTML == "Yetki Grubu Güncelle") {
        $.ajax({
            url: '/api/RoleUpdate',
            type: 'Put',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(role),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Yetki Grubu Güncelleme!",
                        text: "Yetki grubu başarıyla güncellenmiştir.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Yetki Grubu Güncelleme!",
                        text: "Yetki grubu bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
    else {
        $.ajax({
            url: '/api/RoleAdd',
            type: 'Post',
            dataType: 'Json',
            contentType: 'application/json',
            data: JSON.stringify(role),
            success: function (data) {
                if (data.state) {
                    modalTab.style = "display:none;";
                    document.getElementsByClassName("modal-backdrop")[0].hidden = true;
                    Swal.fire({
                        title: "Yetki Grubu Kaydetme!",
                        text: "Yetki grubu başarıyla kaydedildi.",
                        type: "success"
                    })
                    PageLoad();
                }
                else {
                    Swal.fire({
                        title: "Yetki Grubu Kaydetme!",
                        text: "Yetki grubu bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                        type: "warning"
                    })
                }
            }
        });
    }
}