$(document).ready(function() {
    $(document).on("click", ".ukol", function(){
        UkazFormular(this);
    });
    $(document).on("click", "#zpet", function(){
        UkazUkoly();
    });
    $(document).on("click", "#pridat", function () {
       
        /*
        fetch('/Ukoly/Seznam')
            .then(odpoved => odpoved.json())
            .then(data => console.log(data));
        */
        
        fetch('/Ukoly/Pridat', {
            method: 'POST',
            body: JSON.stringify(ZiskatUkol()),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(odpoved => odpoved.json())
        .then(data => console.log(data));
    });
});

function UkazFormular(element){
    let telo = "";

    if(element.value == 1 || element.value == 4){
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label for="Hodnota">Počet včel: </label>
            </div>
            <div class="sloupec2">
                <input type="number" name="Hodnota" value="@ViewBag.Ukol.PocetVcel" max="" min="1">
            </div>
        </div>`;
    }else if(element.value == 2){
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
    }else if(element.value == 3){
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
            <input type="hidden" name="Id" value="` + element.value +`">
            `+ telo +`
        </div>
        <div id="tlacitka">
            <button id="zrusit"">Smazat úkol</button>
            <button id="pridat">Přidat úkol</button>
        </div>
    `);
}

function UkazUkoly(){
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

function ZiskatUkol() {
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
    console.log(ukol);
    return ukol;
}