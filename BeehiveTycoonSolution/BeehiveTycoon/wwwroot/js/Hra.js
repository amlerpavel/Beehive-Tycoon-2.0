let hra;
let idLokaceUlu;
let vybranyUl;

$(document).ready(function () {
    fetch('/Hra/JSON')
    .then(odpoved => odpoved.json())
    .then(data => {
        hra = data;
        vybranyUl = hra.uly[0];
        idLokaceUlu = hra.uly[0].lokace.id;
        NacistHerniPlochu();
    });
    $(document).on("click", "#znovu", function () {
        fetch('/Hra/Nova')
        .then(odpoved => odpoved.json())
        .then(data => {
            hra = data;
            vybranyUl = hra.uly[0];
            idLokaceUlu = hra.uly[0].lokace.id;
            NacistHerniPlochu();
        });
    });

    $(document).on("click", ".ul", function () {
        idLokaceUlu = this.value;
        vybranyUl = NajitUlPodleLokace(idLokaceUlu);
        
        ZobrazitDataUlu();
    });

    $(document).on("click", "#dalsiKolo", function () {
        fetch('/Hra/DalsiKolo')
        .then(odpoved => odpoved.json())
        .then(data => {
            hra = data;
            vybranyUl = NajitUlPodleLokace(idLokaceUlu);

            if (vybranyUl == undefined) {
                vybranyUl = hra.uly[0];
                idLokaceUlu = hra.uly[0].lokace.id;
            }

            ZobrazitDataUlu();
        });
    });

    $(document).on("click", ".ukol", function () {
        UkazFormular(this);
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
            if (typeof (data) == "string")
                console.log(data);
            else {
                vybranyUl.seznamUkolu = data;
                AktualizovatUkoly();
            }
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
                if (typeof (data) == "string")
                    console.log(data);
                else {
                    vybranyUl.seznamUkolu = data;
                    AktualizovatUkoly();
                }
            });
        }
    });
});

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
        <div>Plástve: ${vybranyUl.plastve.length}</div>
        <div>Měsíc: ${hra.datum.mesic}</div>
        <div>Lokace: ${vybranyUl.lokace.nazev}</div>
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
function ZobrazitUly() {
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
function ZobrazitNepritele() {
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
        console.log(vybranyUl.existujeMrtvyNepritel);
        $("#nepritel").remove();
        $("#nepratele").prepend(`<div id="nepritel">${telo}</div>`);
    }
    else {
        $("#nepritel").remove();
    }
}

function ZobrazitDataUlu() {
    PrepsatZakladniInformace();
    ZobrazitUly();
    PrepsatSeznamUkolu();
    ZobrazitNepritele();
    PrepsatContainer2();
}
function NacistHerniPlochu() {
    $("#ukoly").html(`
        <div id="dalsiKolo" class="tlacitko0">
            <a>Další kolo</a>
        </div>
    `);
    ZobrazitDataUlu();
}
function AktualizovatUkoly() {
    hra.uly[hra.uly.indexOf(vybranyUl)].seznamUkolu = vybranyUl.seznamUkolu;
    PrepsatSeznamUkolu();
    UkazVyberUkolu();
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
    } else if (element.value == 6) {
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label>Vyberte lokaci: </label>
            </div>
            <div class="sloupec2">
                <input type="radio" name="Hodnota" id="tady" value="1">
                <label for="tady">tady</label><br>
                <input type="radio" name="Hodnota" id="zde" value="2">
                <label for="zde">zde</label><br>
                <input type="radio" name="Hodnota" id="vedle" value="3">
                <label for="vedle">vedle</label><br>
                <input type="radio" name="Hodnota" id="tamhle" value="4">
                <label for="tamhle">támhle</label><br>
                <input type="radio" name="Hodnota" id="tam" value="5">
                <label for="tam">tam</label><br>
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
            <button id="zrusit"">Smazat úkol</button>
            <button id="pridat">Přidat úkol</button>
        </div>
    `);
}
function PrepsatContainer2(){
    if(hra.vyhra == false && hra.prohra == false){
        if(vybranyUl.vcelstvo <= 0){
            $("#container2").html(`
                <h1>V tomto úlu vám zemřelo včelstvo.</h1>
            `);
        } else {
            UkazVyberUkolu();
        }
    } else if (hra.vyhra == true){
        $("#container2").html(`
            <h1>Vyhrál jsi.</h1>
            <button id="znovu">Hrát znovu</button>
        `);
    } else if (hra.prohra == true){
        $("#container2").html(`
            <h1>Prohrál jsi.</h1>
            <button id="znovu">Hrát znovu</button>
        `);
    }
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
function NajitUlPodleLokace(lokaceId) {
    for (let ul of hra.uly) {
        if (ul.lokace.id == lokaceId)
            return ul;
    }
}
