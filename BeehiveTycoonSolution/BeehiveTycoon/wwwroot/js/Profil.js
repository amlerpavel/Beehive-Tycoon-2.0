$(document).ready(function () {
    Odhlasit();
});

function ZiskatUlozeneHry() {

}

function Odhlasit() {
    $(document).on("click", "#odhlasit", function () {
        location.href = 'Odhlasit'
    });
}
