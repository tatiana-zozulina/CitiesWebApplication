"use strict";

$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/cityHub").build();

    connection.on("CityDeleted", function (id) {
        $('#city-table #city-row-' + id).remove();
    });
    connection.on("CityEdited", function (id, name, date, population) {
        const city = $('#city-table #city-row-' + id);
        city.find('.city-name').html(name);
        city.find('.city-date').html(date);
        city.find('.city-population').html(population);
    });
    connection.on("CityCreated", function (id, name, date, population) {
        $('#city-table tbody').append(
            `<tr class="city-row" id="city-row-` + id + `">
                <td class="city-name">
                    ` + name + `
                </td>
                <td class="city-date">
                    ` + date + `
                </td>
                <td class="city-poulation">
                    ` + population + `
                </td>
                <td>
                    <a class="city-edit" href="City/Edit/` + id + `">Edit</a> |
                    <a class="delete-city disabled" rel="` + id + `" href="City/Delete/` + id + `">Delete</a>
                </td>
            </tr>`
        );
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

});