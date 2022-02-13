$(document).ready(function() {
    $(document).on("click", ".ukol", function(){
        UkazFormular(this);
    });
    $(document).on("click", "#zpet", function(){
        UkazUkoly();
    });
});

function UkazFormular(element){
    let telo = "";

    if(element.value == 1 || element.value == 4){
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label for="vcely">Počet včel: </label>
            </div>
            <div class="sloupec2">
                <input type="number" name="pocetVcel" value="@ViewBag.Ukol.PocetVcel" max="" min="1">
            </div>
        </div>`;
    }else if(element.value == 2){
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label for="pocetVajicek">Počet vajíček: </label>
            </div>
            <div class="sloupec2">
                <input type="number" name="pocetVajicek" value="@ViewBag.Ukol.PocetVajicek" max="" min="1">
            </div>
        </div>
        <div class="radek" id="posledni">
            <input type="submit" value="Zobrazit požadavky" name="tlacitko">
        </div>`;
    }else if(element.value == 3){
        telo = `
        <div class="radek">
            <div class="sloupec1">
                <label for="pocetPlaství">Počet pláství: </label>
            </div>
            <div class="sloupec2">
                <input type="number" name="pocetPlastvi" value="@ViewBag.Ukol.PocetPlastvi" max="" min="1">
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
            <form action="/Ukoly/CoSUkolem" method="post">
                <input type="hidden" name="idUkolu" value="` + element.id +`">
                <input type="hidden" name="nazevUkolu" value="` + element.innerText +`">
                `+ telo +`
                <div id="tlacitka">
                    <div id="cerveneTlacitko" class="tlacitko">
                        <a href="/Ukoly/ZrusitUkol?idUkolu=1">Smazat úkol</a>
                    </div>
                    <input class="tlacitko" type="submit" value="Přidat úkol" name="tlacitko">
                </div>
            </form>
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