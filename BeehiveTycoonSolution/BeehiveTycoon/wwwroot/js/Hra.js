let hra;

$(document).ready(function () {
    fetch('/Hra/JSON')
    .then(odpoved => odpoved.json())
    .then(data => { hra = data; });

    $(document).on("click", "#dalsiKolo", function () {
        fetch('/Hra/DalsiKolo')
        .then(odpoved => odpoved.json())
        .then(data => {
            hra = data;
            PrepsatZakladniInformace();
            PrepsatSeznamUkolu();
            ZobrazitNepritele();
        });
    });

    $(document).on("click", ".ukol", function () {
        UkazFormular(this);
    });
    $(document).on("click", "#zpet", function () {
        UkazVyberUkolu();
    });
    $(document).on("click", "#pridat", function () {

        /*
        fetch('/Ukoly/Seznam')
            .then(odpoved => odpoved.json())
            .then(data => console.log(data));
        */

        fetch('/Ukoly/Pridat', {
            method: 'POST',
            body: JSON.stringify(ZiskatDataUkolu()),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => odpoved.json())
        .then(data => {
            if (typeof (data) == "string")
                console.log(data);
            else {
                hra.ul.seznamUkolu = data;
                PrepsatSeznamUkolu();
                UkazVyberUkolu();
            }
        });
    });
    $(document).on("click", "#zrusit", function () {
        let dataUkolu = ZiskatDataUkolu();
        if (hra.ul.seznamUkolu.includes(NajitUkol(dataUkolu.Id))) {
            fetch('/Ukoly/Zrusit', {
                method: 'POST',
                body: JSON.stringify(dataUkolu.Id),
                headers: {
                    "Content-Type": "application/json"
                }
            })
            .then(odpoved => odpoved.json())
            .then(data => {
                hra.ul.seznamUkolu = data;
                PrepsatSeznamUkolu();
                UkazVyberUkolu();
            });
        }
    });
});

function PrepsatZakladniInformace() {
    let soucetVek = 0;
    let vypisGeneraci = "";

    for(let generaceVcel of hra.ul.generaceVcelstva)
    {
        soucetVek += generaceVcel.vek;

        vypisGeneraci += `
            <tr>
                <td>`+ generaceVcel.pocet +`</td>
                <td>`+ generaceVcel.vek +`</td>
            </tr>`
    }

    let prumerVek = Math.round(soucetVek / hra.ul.generaceVcelstva.length * 100) / 100;
    //(soucetVek / hra.ul.generaceVcelstva.length).toFixed(2);

    $("#radek").html(`
        <div>Včelstvo: `+ hra.ul.vcelstvo +`</div>
        <div id="Prumer">
            Průměrný věk včelstva: `+ prumerVek +`
            <div class="submenuG">
                Generace včel:
                <table id="generace">
                    <tbody>
                        <tr>
                            <th>Počet včel</th>
                            <th>Věk</th>
                        </tr>
                        `+ vypisGeneraci +`
                    </tbody>
                </table>
            </div>
        </div>
        <div>Med: `+ hra.ul.med +`</div>
        <div>Plástve: `+ hra.ul.plastve.length +`</div>
        <div>Měsíc: `+ hra.datum.mesic +`</div>
        <div>Lokace: `+ hra.ul.lokace +`</div>
    `);
}
function PrepsatSeznamUkolu() {
    let tabulka;
    let pocetVcel = 0;
    let pocetMedu = 0;

    if (hra.ul.seznamUkolu.length == 0) {
        tabulka = "<ul><li>Nejsou zadané žádné úkoly.</li></ul>";

    } else {
        let radky = "";
        let sloupecKusy = [];
        let sloupecVcely = [];
        let sloupecMed = [];

        for (let ukol of hra.ul.seznamUkolu) {
            let kusy = ukol.podrobnosti.find(u => u.jmeno == "kusy");
            let vcely = ukol.podrobnosti.find(u => u.jmeno == "vcely");
            let med = ukol.podrobnosti.find(u => u.jmeno == "med");

            if (kusy == undefined)
                sloupecKusy.push(0);
            else
                sloupecKusy.push(kusy.hodnota);
            if (vcely == undefined)
                sloupecVcely.push(0);
            else
                sloupecVcely.push(vcely.hodnota);
            if (med == undefined)
                sloupecMed.push(0);
            else
                sloupecMed.push(med.hodnota);

            let i = hra.ul.seznamUkolu.indexOf(ukol);
            radky += "<tr><th><a>" + ukol.nazev + "</a></th><td>" + sloupecKusy[i] + "</td><td>" + sloupecVcely[i] + "</td><td>" + sloupecMed[i] + "</td></tr>";
            
            for (let podrobnost of ukol.podrobnosti) {
                if (podrobnost.jmeno == "vcely") {
                    pocetVcel += podrobnost.hodnota;
                }
                else if (podrobnost.jmeno == "med") {
                    pocetMedu += podrobnost.hodnota;
                }
            }
        }
        
        tabulka = "<table><tbody><tr><th>Název úkolu</th><td>Kusy</td><td>Včely</td><td>Med</td></tr>" + radky + "<tr><th>Celkem</th><td></td><td>" + pocetVcel + "</td><td>" + pocetMedu + "</td></tr></tbody></table>";
    }

    $("#seznamUkolu").html("<p>Úkoly:</p>" + tabulka);
}
function ZobrazitNepritele() {
    if (hra.ul.nepritel.porazen == false || hra.ul.existujeMrtvyNepritel == true) {
        let telo = "";

        if (hra.ul.nepritel.porazen == false) {
            telo = `
        <p>Nepřítel v úlu!</p>
        <ul>
            <li>Jméno nepřítele: `+ hra.ul.nepritel.jmeno + `</li>
            <li>Počet nepřátel: `+ hra.ul.nepritel.pocet + `</li>
        </ul>`;
        }
        else if (hra.ul.existujeMrtvyNepritel == true) {
            telo = "<p>Nepřítel byl poražen.</p>";
        }

        $("#nepritel").remove();
        $("#nepratele").prepend('<div id="nepritel">' + telo + '</div>');
    }
    else {
        $("#nepritel").remove();
    }
}

function UkazVyberUkolu() {
    $("#container2").html(`
        <h1>Přidat úkol</h1>
        <div id="seznam">
            <button class="ukol" value="1">Sbírání pylu</button>
            <button class="ukol" value="2">Nakladení vajíček</button>
            <button class="ukol" value="3">Vytvoření plástve</button>
            <button class="ukol" value="4">Obrana úlu</button>
            <button class="ukol" value="5">Zazimování úlu</button>
            <button class="ukol" value="6">Vyrojení včelstva</button>
        </div>
    `);
}
function UkazFormular(element) {
    let telo = "";

    if (element.value == 1 || element.value == 4) {
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label for="Hodnota">Počet včel: </label>
            </div>
            <div class="sloupec2">
                <input type="number" name="Hodnota" value="@ViewBag.Ukol.PocetVcel" max="" min="1">
            </div>
        </div>`;
    } else if (element.value == 2) {
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label for="Hodnota">Počet vajíček: </label>
            </div>
            <div class="sloupec2">
                <input type="number" name="Hodnota" value="@ViewBag.Ukol.PocetVajicek" max="" min="1">
            </div>
        </div>
        <div class="radek" id="posledni">
            <input type="submit" value="Zobrazit požadavky" name="tlacitko">
        </div>`;
    } else if (element.value == 3) {
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label for="Hodnota">Počet pláství: </label>
            </div>
            <div class="sloupec2">
                <input type="number" name="Hodnota" value="@ViewBag.Ukol.PocetPlastvi" max="" min="1">
            </div>
        </div>
        <div class="radek" id="posledni">
            <input type="submit" value="Zobrazit požadavky" name="tlacitko">
        </div>`;
    }

    $("#container2").html(`
        <h1>` + element.innerText + `</h1>
        <button id="zpet">&#10006;</button>
        <div id="formular">
            <input type="hidden" name="Id" value="` + element.value + `">
            `+ telo + `
        </div>
        <div id="tlacitka">
            <button id="zrusit"">Smazat úkol</button>
            <button id="pridat">Přidat úkol</button>
        </div>
    `);
}

function ZiskatDataUkolu() {
    let formular = $("#formular").find("input");
    let ukol = { Id: 0, Hodnota: 0 };

    for (let input of formular) {
        let jmeno = input.name;
        let hodnota = input.value;

        if (jmeno == "Id") {
            ukol.Id = hodnota;
        }
        else if (jmeno == "Hodnota") {
            ukol.Hodnota = hodnota;
        }
    }
    
    return ukol;
}
function NajitUkol(id) {
    for (let ukol of hra.ul.seznamUkolu) {
        if (ukol.id == id)
            return ukol;
    }
}
