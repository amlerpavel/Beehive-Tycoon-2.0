$(document).ready(function () {
    $(document).on("click", "#dalsiKolo", function () {
        fetch('/Ul/DalsiKolo')
        .then(odpoved => odpoved.json())
        .then(data => ZmenitUdaje(data));
    });
});

function ZmenitUdaje(hra) {
    let soucetVek = 0;
    let vypisGeneraci = "";

    for(let generaceVcel of hra.ul0.generaceVcelstva)
    {
        soucetVek += generaceVcel.vek;

        vypisGeneraci += `
            <tr>
                <td>`+ generaceVcel.pocet +`</td>
                <td>`+ generaceVcel.vek +`</td>
            </tr>`
    }

    let prumerVek = Math.round(soucetVek / hra.ul0.generaceVcelstva.length * 100) / 100;
    //(soucetVek / hra.ul0.generaceVcelstva.length).toFixed(2);

    $("#radek").html(`
        <div>Včelstvo: `+ hra.ul0.vcelstvo +`</div>
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
        <div>Med: `+ hra.ul0.med +`</div>
        <div>Plástve: `+ hra.ul0.plastve.length +`</div>
        <div>Měsíc: `+ hra.datum.mesic +`</div>
        <div>Lokace: `+ hra.ul0.lokace +`</div>
    `);
}