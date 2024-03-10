// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function flipCard(cardIndex) {
    var cards = document.querySelectorAll('.card');
    cards[cardIndex].style.transform = cards[cardIndex].style.transform === 'rotateY(180deg)' ? 'rotateY(0deg)' : 'rotateY(180deg)';
}
var BASE_API_URI = "https://localhost:7075";

function Get(action, success) {
    $.ajax({
        type: "GET",
        url: `${BASE_API_URI}/${action}`,
        //beforeSend: function (xhr) {
        //    xhr.setRequestHeader('Authorization', `Bearer ${TOKEN}`);
        //},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.success) {
                success(response.data);
            }
            else {
                alert(response.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + "-" + textStatus + "-" + errorThrown);
        }
    });
}

function Post(action, data, success) {
    $.ajax({
        type: "POST",
        url: `${BASE_API_URI}/${action}`,
        //beforeSend: function (xhr) {
        //    xhr.setRequestHeader('Authorization', `Bearer ${TOKEN}`);
        //},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                success(response.data);
            }
            else {
                alert(response.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + "-" + textStatus + "-" + errorThrown);
        }
    });
}

function Delete(action, success, ask = true) {
    var confirmed = true;
    if (ask) {
        confirmed = confirm("Kaydı silmek istediğinizden emin misiniz?");
    }
    if (confirmed) {
        $.ajax({
            type: "DELETE",
            url: `${BASE_API_URI}/${action}`,
            //beforeSend: function (xhr) {
            //    xhr.setRequestHeader('Authorization', `Bearer ${TOKEN}`);
            //},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    success(response.data);
                }
                else {
                    alert(response.message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + "-" + textStatus + "-" + errorThrown);
            }
        });
    }
}