$(document).ready(function () {
    Registrovat();
});

function ZiskatData() {
    let registrace = { Jmeno: "", Heslo: "", HesloZnovu: "" };

    registrace.Jmeno = $('input[name="jmeno"]').val();
    registrace.Heslo = $('input[name="heslo"]').val();
    registrace.HesloZnovu = $('input[name="hesloZnovu"]').val();

    return registrace;
}

function Registrovat() {
    $(document).on("click", "#registrovat", function () {
        fetch('/Uzivatel/Zaregistrovat', {
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
