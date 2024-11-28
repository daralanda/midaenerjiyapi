var datapost = [];

$(document).ready(function () {
    PageLoad();
});

function PageLoad() {
    $.ajax({
        url: '/api/GetAllSeoInfo',
        type: 'Get',
        dataType: 'Json',
        contentType: 'application/json',
        success: function (data) {
            if (data.state) {
                datapost = data.data;
                datapost.forEach(function (x) {
                    if (x.CategoryId==1) {
                        document.getElementById("Keywords").value = x.Keywords;
                        document.getElementById("Description").value = x.Description;
                    }
                    else if (x.CategoryId == 2) {
                        document.getElementById("cKeywords").value = x.Keywords;
                        document.getElementById("cDescription").value = x.Description;
                    }
                });
            }
        }
    });
   
}





function PostData() {
    console.clear();
    datapost[0].CategoryId = 1;
    datapost[0].Keywords = document.getElementById("Keywords").value;
    datapost[0].Description = document.getElementById("Description").value;
    datapost[1].CategoryId = 2;
    datapost[1].Keywords = document.getElementById("cKeywords").value;
    datapost[1].Description = document.getElementById("cDescription").value;
    $.ajax({
        url: '/api/UpdateSeoInfo',
        type: 'Post',
        dataType: 'Json',
        contentType: 'application/json',
        data: JSON.stringify(datapost),
        success: function (data) {
            if (data.state) {
                Swal.fire({
                    title: "Sayfa Seo Güncelleme!",
                    text: "Sayfa Seo  başarıyla güncellendi.",
                    type: "success"
                })
                PageLoad();
            }
            else {
                Swal.fire({
                    title: "Sayfa Seo Güncelleme!",
                    text: "Sayfa Seo bilgilerini lütfen kontrol ediniz. İşlem tamamlanamadı",
                    type: "warning"
                })
            }
        }
    });
   
}