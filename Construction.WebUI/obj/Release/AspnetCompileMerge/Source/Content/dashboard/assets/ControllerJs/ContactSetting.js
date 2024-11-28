$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllContactSetting',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                var element = data.data;
                document.getElementById("Host").value = element.Host;
                document.getElementById("Email").value = element.Email;
                document.getElementById("Password").value = element.Password;
                document.getElementById("EnableSsl").checked = element.EnableSsl;
                document.getElementById("Port").value = element.Port;
            }
        }
    });
   
}
var datapost = {
    ContactSettingId:1,
    Host: "",
    Email: "",
    Password: "",
    EnableSsl: "",
    Port: "",
}




function PostData() {
    datapost.Host = document.getElementById("Host").value;
    datapost.Email = document.getElementById("Email").value;
    datapost.Password = document.getElementById("Password").value;
    datapost.EnableSsl = document.getElementById("EnableSsl").checked;
    datapost.Port = document.getElementById("Port").value;
   
    $.ajax({
        url: '/api/AddorUpdateContactSetting',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(datapost),
        success: function (data) {
            if (data.state) {
                Swal.fire({
                    title: "İletişim Ayarlar Güncelleme!",
                    text: "İletişim Ayarlar  başarıyla güncellendi.",
                    type: "success"
                })
                PageLoad();
            }
            else {
                Swal.fire({
                    title: "İletişim Ayarlar Güncelleme!",
                    text: "İletişim Ayarlar bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                    type: "warning"
                })
            }
        }
    });
   
}