var datapost;
$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetUserInfo',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                datapost = data.data;
                console.log(datapost);
                document.getElementById("LastName").value = datapost.LastName;
                document.getElementById("Password").value = datapost.Password;
                document.getElementById("Phone").value = datapost.Phone;
                document.getElementById("FirstName").value = datapost.FirstName;
            }
        }
    });
   
}




function PostData() {
    datapost.LastName = document.getElementById("LastName").value;
    datapost.Password = document.getElementById("Password").value;
    datapost.Phone = document.getElementById("Phone").value;
    datapost.FirstName = document.getElementById("FirstName").value;

    $.ajax({
        url: '/api/UpdateProfile',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(datapost),
        success: function (data) {
            if (data.state) {
                Swal.fire({
                    title: "Profile Bilgileri Güncelleme!",
                    text: "Profile Bilgileri  başarıyla güncellendi.",
                    type: "success"
                })
                PageLoad();
            }
            else {
                Swal.fire({
                    title: "Profile Bilgileri Güncelleme!",
                    text: "Profile Bilgileri bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                    type: "warning"
                })
            }
        }
    });
   
}