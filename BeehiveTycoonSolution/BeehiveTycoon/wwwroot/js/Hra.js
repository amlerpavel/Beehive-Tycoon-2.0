let hra;
let idLokaceUlu;
let vybranyUl;
let casovac;

$(document).ready(function () {
    Start();
    ZobrazitUl();
    OvladaniUkolu();
    DalsiKolo();
});

// Inicializace hry
function VytvoritHerniPlochu() {
    $("#ukoly").html(`
        <div id="dalsiKolo">
            <button>Další kolo</button>
        </div>
    `);
    ZobrazitDataUlu();
}
function ZobrazitVyberObtiznosti() {
    $("#container2").html(`
        <h1>Nová hra</h1>
        <h2>Zvolte si obtížnost: </h2>
        <div id="obtiznosti">
            <button class="obtiznost" value="1">Lehká</button>
            <button class="obtiznost" value="2">Normální</button>
            <button class="obtiznost" value="3">Těžká</button>
        </div>
    `);
}

function KontrolaUlu(data) {
    if (data == null) {
        ZobrazitVyberObtiznosti();
    } else {
        hra = data;
        vybranyUl = hra.uly[0];
        idLokaceUlu = hra.uly[0].lokace.id;
        VytvoritHerniPlochu();
    }
}

function Start() {
    fetch('/Hra/Nacist')
    .then(odpoved => odpoved.json())
    .then(data => {
        KontrolaUlu(data);
    });

    $(document).on("click", "#znovu", function () {
        ZobrazitVyberObtiznosti();
    });

    $(document).on("click", ".obtiznost", function () {
        fetch('/Hra/Nova', {
            method: 'POST',
            body: JSON.stringify(this.value),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => odpoved.json())
        .then(data => {
            if (typeof (data) == "string")
                console.log(data);
            else {
                KontrolaUlu(data);
            }
        });
    });
}

// Zobrazeni ulu
function PrepsatZakladniInformace() {
    let soucetVek = 0;
    let vypisGeneraci = "";

    for (let generaceVcel of vybranyUl.generaceVcelstva)
    {
        soucetVek += generaceVcel.vek;

        vypisGeneraci += `
            <tr>
                <td>${generaceVcel.pocet}</td>
                <td>${generaceVcel.vek }</td>
            </tr>
        `;
    }

    let prumerVek = Math.round(soucetVek / vybranyUl.generaceVcelstva.length * 100) / 100;
    //(soucetVek / hra.ul.generaceVcelstva.length).toFixed(2);

    $("#radek").html(`
        <div>Včelstvo: ${vybranyUl.vcelstvo}</div>
        <div id="Prumer">
            Průměrný věk včelstva: ${prumerVek}
            <div class="submenuG">
                Generace včel:
                <table id="generace">
                    <tbody>
                        <tr>
                            <th>Počet včel</th>
                            <th>Věk</th>
                        </tr>
                        ${vypisGeneraci}
                    </tbody>
                </table>
            </div>
        </div>
        <div>Med: ${vybranyUl.med}</div>
        <div>Plástve: ${vybranyUl.plastve.length} / ${vybranyUl.maxPlastvi}</div>
        <div>Rok: ${hra.datum.rok} Měsíc: ${hra.datum.mesic}</div>
        <div>Lokace: ${vybranyUl.lokace.nazev}</div>
        <div>Obtížnost: ${hra.obtiznost.nazev}</div>
        <button id="konec" onclick="location.href='/'">&#10006;</button>
    `);
}
function PrepsatSeznamUkolu() {
    let tabulka;
    let pocetVcel = 0;
    let pocetMedu = 0;

    if (vybranyUl.seznamUkolu.length == 0) {
        tabulka = `
            <ul>
                <li>Nejsou zadané žádné úkoly.</li>
            </ul>
        `;
    } else {
        let radky = "";
        let sloupecKusy = [];
        let sloupecVcely = [];
        let sloupecMed = [];

        for (let ukol of vybranyUl.seznamUkolu) {
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

            let i = vybranyUl.seznamUkolu.indexOf(ukol);

            radky += `
                <tr>
                    <th><a>${ukol.nazev}</a></th>
                    <td>${sloupecKusy[i]}</td>
                    <td>${sloupecVcely[i]}</td>
                    <td>${sloupecMed[i]}</td>
                </tr>
            `;

            for (let podrobnost of ukol.podrobnosti) {
                if (podrobnost.jmeno == "vcely") {
                    pocetVcel += podrobnost.hodnota;
                }
                else if (podrobnost.jmeno == "med") {
                    pocetMedu += podrobnost.hodnota;
                }
            }
        }

        tabulka = `
            <table>
                <tbody>
                    <tr>
                        <th>Název úkolu</th>
                        <td>Kusy</td>
                        <td>Včely</td>
                        <td>Med</td>
                    </tr>
                    ${radky}
                    <tr>
                        <th>Celkem</th>
                        <td></td>
                        <td>${pocetVcel}</td>
                        <td>${pocetMedu}</td>
                    </tr>
                </tbody>
            </table>
        `;
    }

    $("#seznamUkolu").remove();
    $(`
        <div id="seznamUkolu">
            <p>Úkoly:</p>
            ${tabulka}
        </div>
    `).insertBefore("#dalsiKolo");
}
function PrepsatUly() {
    let tlacitka = "";

    for (ul of hra.uly) {
        tlacitka += `<li><button class="ul" value="${ul.lokace.id}">Úl - ${ul.lokace.nazev}</button></li>`;
    }
    
    $("#uly").remove();
    $("#ukoly").prepend(`
        <div id="uly">
            <p>Včelí úly:</p>
            <ul>${tlacitka}</ul>
        </div>
    `);
}
function PrepsatNepritele() {
    if (vybranyUl.nepritel.porazen == false || vybranyUl.existujeMrtvyNepritel == true) {
        let telo = "";

        if (vybranyUl.nepritel.porazen == false) {
            telo = `
                <p>Nepřítel v úlu!</p>
                <ul>
                    <li>Jméno nepřítele: ${vybranyUl.nepritel.jmeno}</li>
                    <li>Počet nepřátel: ${vybranyUl.nepritel.pocet}</li>
                </ul>
            `;
        }
        else if (vybranyUl.existujeMrtvyNepritel == true) {
            telo = `<p>Nepřítel byl poražen.</p>`;
        }

        $("#nepritel").remove();
        $("#nepratele").prepend(`<div id="nepritel">${telo}</div>`);
    }
    else {
        $("#nepritel").remove();
    }
}
function UkazVyhruProhruUmrti() {
    if (hra.vyhra == false && hra.prohra == false) {
        if (vybranyUl.vcelstvo <= 0) {
            $("#container2").html(`
                <h1>V tomto úlu vám zemřelo včelstvo.</h1>
            `);
        } else {
            UkazVyberUkolu();
        }
    } else if (hra.vyhra == true) {
        $("#container2").html(`
            <h1>Vyhrál jsi.</h1>
            <button id="znovu">Hrát znovu</button>
        `);
    } else if (hra.prohra == true) {
        $("#container2").html(`
            <h1>Prohrál jsi.</h1>
            <button id="znovu">Hrát znovu</button>
        `);
    }
}

function ZobrazitDataUlu() {
    PrepsatZakladniInformace();
    PrepsatUly();
    PrepsatSeznamUkolu();
    PrepsatNepritele();
    UkazVyhruProhruUmrti();
}
function NajitUlPodleLokace(lokaceId) {
    for (let ul of hra.uly) {
        if (ul.lokace.id == lokaceId)
            return ul;
    }
}

function ZobrazitUl() {
    $(document).on("click", ".ul", function () {
        idLokaceUlu = this.value;
        vybranyUl = NajitUlPodleLokace(idLokaceUlu);

        ZobrazitDataUlu();
    });
}

// Ukoly
function UkazVyberUkolu() {
    $("#container2").html(`
        <h1>Přidat úkol</h1>
        <div id="seznam">
            <button class="ukol" value="1" popisek="Jak zístate med? Tím, že včely vyšlete na sbírání pylu.">Sbírání pylu</button>
            <button class="ukol" value="2" popisek="Včely nejsou nesmrtelné...">Nakladení vajíček</button>
            <button class="ukol" value="3" popisek="Pro uložení medu potřebujete dostatek pláství.">Vytvoření plástve</button>
            <button class="ukol" value="4" popisek="Nepřítel byl spatřen v blízkosti úlu! Je čas vyslat strážce.">Obrana úlu</button>
            <button class="ukol" value="5" popisek="V zimě včely hybernují a nemohou se bránit, proto byste měli úl včas zazimovat.">Zazimování úlu</button>
            <button class="ukol" value="6" popisek="Došlo vám místo v úlu pro další plástve? Neváhejte a založte si další úl.">Vyrojení včelstva</button>
        </div>
    `);
    ZobrazitPopisky();
}
function UkazUkol(element) {
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
        </div>`;
    } else if (element.value == 6) {
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label>Vyberte lokaci: </label>
            </div>
            <div class="sloupec2">
                <input type="radio" name="Hodnota" id="zahrada" value="1">
                <label for="zahrada">Zahrada</label><br>
                <input type="radio" name="Hodnota" id="les" value="2">
                <label for="les">Les</label><br>
                <input type="radio" name="Hodnota" id="louka" value="3">
                <label for="louka">Louka</label><br>
                <input type="radio" name="Hodnota" id="pole" value="4">
                <label for="pole">Pole</label><br>
                <input type="radio" name="Hodnota" id="mesto" value="5">
                <label for="mesto">Město</label><br>
            </div>
        </div>`;
    }

    $("#container2").html(`
        <h1>${element.innerText}</h1>
        <button id="zpet">&#10006;</button>
        <div id="formular">
            <input type="hidden" name="Id" value="${element.value}">
            ${telo}
        </div>
        <div id="tlacitka">
            <button id="zrusit">Smazat úkol</button>
            <button id="pridat">Přidat úkol</button>
        </div>
    `);
}
function ZobrazitPopisky() {
    $(".ukol").mouseenter(function () {
        let popisek = $(this).attr("popisek");
        $("body").append(`<div id="popisek" style="display:none;">${popisek}</div>`);

        casovac = setTimeout(function () {
            $("#popisek").css({ display: "block" });
        }, 500);

        $(this).on("mousemove", function (event) {
            $("#popisek").css({ top: event.pageY + 20, left: event.pageX + 20 });
        });

        $(this).mouseout(function () {
            $("#popisek").remove();
            clearTimeout(casovac);
        });
    });
}

function AktualizovatUkoly(data) {
    if (data != null) {
        if (typeof (data) == "string") {
            $("#hlaska").remove();
            $(`<div id="hlaska"><p>${data}</p></div>`).insertBefore("#tlacitka");
        }else {
            vybranyUl.seznamUkolu = data;
            hra.uly[hra.uly.indexOf(vybranyUl)].seznamUkolu = vybranyUl.seznamUkolu;

            PrepsatSeznamUkolu();
            UkazVyberUkolu();
        }
    } else
        location.href = "/";
}

function ZiskatDataUkolu() {
    let ukol = { Id: 0, Hodnota: 0, IdLokaceUlu: idLokaceUlu };

    ukol.Id = $('input[name ="Id"]').val();

    if (ukol.Id == 6) {
        ukol.Hodnota = $('input[name="Hodnota"]:checked').val();
    } else {
        ukol.Hodnota = $('input[name="Hodnota"]').val();
    }
    
    return ukol;
}
function NajitUkol(id) {
    for (let ukol of vybranyUl.seznamUkolu) {
        if (ukol.id == id)
            return ukol;
    }
}

function OvladaniUkolu() {
    $(document).on("click", ".ukol", function () {
        $("#popisek").remove();
        UkazUkol(this);
    });

    $(document).on("click", "#zpet", function () {
        UkazVyberUkolu();
    });

    $(document).on("click", "#pridat", function () {
        fetch('/Ukoly/Pridat', {
            method: 'POST',
            body: JSON.stringify(ZiskatDataUkolu()),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => odpoved.json())
        .then(data => {
            AktualizovatUkoly(data);
        });
    });

    $(document).on("click", "#zrusit", function () {
        let dataUkolu = ZiskatDataUkolu();
        dataUkolu.Hodnota = 0;

        if (vybranyUl.seznamUkolu.includes(NajitUkol(dataUkolu.Id))) {
            fetch('/Ukoly/Zrusit', {
                method: 'POST',
                body: JSON.stringify(dataUkolu),
                headers: {
                    "Content-Type": "application/json"
                }
            })
            .then(odpoved => odpoved.json())
            .then(data => {
                AktualizovatUkoly(data);
            });
        }
    });
}

// Dalsi kolo
function DalsiKolo() {
    $(document).on("click", "#dalsiKolo button", function () {
        fetch('/Hra/DalsiKolo')
        .then(odpoved => odpoved.json())
        .then(data => {
            if (data != null) {
                hra = data;
                vybranyUl = NajitUlPodleLokace(idLokaceUlu);

                if (vybranyUl == undefined) {
                    vybranyUl = hra.uly[0];
                    idLokaceUlu = hra.uly[0].lokace.id;
                }

                ZobrazitDataUlu();
            } else
                location.href = "/";
        });
    });
}
