$(document).ready(function () {
    VyberZebricku(2);
    $(document).on("click", ".obtiznost", function () {
        VyberZebricku(this.value);
    });
});

function ZobrazitZebricek(dokonceneHry) {
    let radky = "";
    let i = 0;
    let predchoziHrac = undefined;

    if (dokonceneHry.length == 0) {
        radky += `<tr><th colspan="5">Databáze zatím neobsahuje žádná data.</th></tr>`;
        /*
        for (let i = 0; i < 99; i++) {
            radky+= `<tr><th colspan="5">Databáze zatím neobsahuje žádná data.</th></tr>`;
        }
        */
    } else {
        for (let dokoncenaHra of dokonceneHry) {
            const datum = new Date(dokoncenaHra.datum);

            if (predchoziHrac == undefined || (predchoziHrac.rok != dokoncenaHra.rok && predchoziHrac.mesic != dokoncenaHra.mesic)) {
                i += 1;
            }

            predchoziHrac = dokoncenaHra;

            radky += `
                <tr>
                    <th>${i}</th>
                    <td>${dokoncenaHra.jmenoUzivatele}</td>
                    <td>${dokoncenaHra.rok}</td>
                    <td>${dokoncenaHra.mesic}</td>
                    <td>${datum.toLocaleString()}</td>
                </tr>
            `;
        }
    }

    $("#tabulka table tbody").html(radky);
}

function VyberZebricku(id) {
    fetch('/Zebricek/Obtiznost', {
        method: 'POST',
        body: JSON.stringify(id),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(odpoved => odpoved.json())
    .then(data => {
        ZobrazitZebricek(data);
    });

    $(".obtiznost").css({ "font-size": "10pt" });

    $(`.obtiznost[value='${id}']`).css({ "font-size": "16pt" });
}