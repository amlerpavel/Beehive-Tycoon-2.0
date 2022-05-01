$(document).ready(function () {
    VyberZebricku();
});

function ZobrazitZebricek(dokonceneHry) {
    let radky = "";
    let i = 0;
    let predchoziHrac = undefined;

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

    tabulka = `
        <table>
            <tbody>
                <tr>
                    <th>Umístění</th>
                    <th>Jméno hráče</th>
                    <th>Rok</th>
                    <th>Měsíc</th>
                    <th>Datum</th>
                </tr>
                ${radky}
            </tbody>
        </table>
    `;

    $("#tabulka").html(tabulka);
}

function VyberZebricku() {
    $(document).on("click", ".obtiznost", function () {
        fetch('/Zebricek/Obtiznost', {
            method: 'POST',
            body: JSON.stringify(this.value),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => odpoved.json())
        .then(data => {
            ZobrazitZebricek(data);
        });
    });
}