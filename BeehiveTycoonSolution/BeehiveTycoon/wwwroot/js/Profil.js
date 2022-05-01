$(document).ready(function () {
    ZiskatUlozeneHry();
    VybratPozici();
    Smazat();
    Odhlasit();
});

function ZobrazitUlozeneHry(rozehraneHry) {
    console.log(rozehraneHry);
    let telo = "";

    for (let i = 1; i < 4; i++) {
        let informaceOHre = rozehraneHry.find(r => r.pozice == i);

        if (informaceOHre != undefined) {
            const datum = new Date(informaceOHre.datum);
            let day = datum.toLocaleDateString();

            telo += `
                <tr>
                    <th>Uložená pozice ${i}</th>
                    <td>${day}</td>
                    <td><button class="pozice" value="${i}">Pokračovat ve hře</button></td>
                    <td><button class="smazat" value="${i}">Smazat hru</button></td>
                </tr>
            `;
        }
        else {
            telo += `
                <tr>
                    <th>Prázdná pozice ${i}</th>
                    <td><button class="pozice" value="${i}">Založit hru</button></td>
                </tr>
            `;
        }
    }

    let tabulka = `
        <div id="ulozeneHry">
            <table>
                <thead>
                    <th>Tvoje uložené pozice:</th>
                </thead>
                <tbody>
                    ${telo}
                </tbody>
            </table>
        </div>
    `;
    $("#ulozeneHry").remove();
    $(tabulka).insertAfter("h1");
}

function ZiskatUlozeneHry() {
    fetch('/Hra/Rozehrane')
    .then(odpoved => odpoved.json())
    .then(data => {
        ZobrazitUlozeneHry(data);
    });
}

function VybratPozici() {
    $(document).on("click", ".pozice", function () {
        fetch('/Hra/Pozice', {
            method: 'POST',
            body: JSON.stringify(this.value),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => {
            if (odpoved.redirected) {
                location.href = odpoved.url;
            } else {
                odpoved.json().then(data => console.log(data));
            }
        });
    });
}

function Smazat() {
    $(document).on("click", ".smazat", function () {
        fetch('/Hra/Smazat', {
            method: 'POST',
            body: JSON.stringify(this.value),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => {
            if (odpoved.redirected) {
                location.href = odpoved.url;
            } else {
                odpoved.json().then(data => {
                    if (data != "OK") {
                        console.log(data);
                    } else {
                        ZiskatUlozeneHry();
                    }
                });
            }
        });
    });
}

function Odhlasit() {
    $(document).on("click", "#odhlasit", function () {
        location.href = 'Odhlasit'
    });
}
