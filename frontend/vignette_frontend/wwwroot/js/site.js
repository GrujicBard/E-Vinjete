// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const fillCarsTable = (items) => {
    const table = document.getElementById("cars_tbody");

    items.forEach(item => {
        let row = table.insertRow();
        let registration = row.insertCell(0);
        let type = row.insertCell(1);
        let manufacturer = row.insertCell(2);
        let model = row.insertCell(3);
        let country = row.insertCell(4);

        registration.innerHTML = item.registration;
        type.innerHTML = item.type;
        manufacturer.innerHTML = item.manufacturer;
        model.innerHTML = item.model;
        country.innerHTML = item.country;
    });
}

const fillVignettesTable = (items) => {
    const table = document.getElementById("vignettes_tbody");

    items.forEach(item => {
        let row = table.insertRow();
        let registration = row.insertCell(0);
        let type = row.insertCell(1);
        let dateCreated = row.insertCell(2);
        let dateValid = row.insertCell(3);


        registration.innerHTML = item.registration;
        type.innerHTML = item.type;
        dateCreated.innerHTML = item.dateCreated;
        dateValid.innerHTML = item.dateValid;
    });
}

const fillReportsTable = (items) => {
    const table = document.getElementById("reports_tbody");

    items.forEach(item => {
        let row = table.insertRow();
        let registration = row.insertCell(0);
        let dateCreated = row.insertCell(1);
        let isSuccess = row.insertCell(2);


        registration.innerHTML = item.registration;
        dateCreated.innerHTML = item.dateCreated;
        isSuccess.innerHTML = item.isSuccess;
    });
}

const hideDiv = (elementId) => {
    var x = document.getElementById(elementId);
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}

const setCarType = (e) => {
    document.getElementById("input_carType").value = e.target.value
}

const setVigType = (e) => {
    document.getElementById("input_vigType").value = e.target.value
}

const setReg = (e) => {
    document.getElementById("input_reg").value = e.target.value
}