$(document).ready(function () {
    Prihlasit();
});

function ZiskatData() {
    let dataPrihlaseni = { Jmeno: "", Heslo: "" };

    dataPrihlaseni.Jmeno = $('input[name="jmeno"]').val();
    dataPrihlaseni.Heslo = $('input[name="heslo"]').val();

    return dataPrihlaseni;
}

function Prihlasit() {
    $(document).on("click", "#prihlasit", function () {
        fetch('/Uzivatel/Prihlasit', {
            method: 'POST',
            body: JSON.stringify(ZiskatData()),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => odpoved.json())
        .then(data => {
            console.log(data);
        });
    });
}

