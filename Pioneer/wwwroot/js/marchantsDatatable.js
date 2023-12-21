$(document).ready(function () {
    //$('#Marchants').DataTable({
    //    "processing": true,
    //    "serverSide": false,
    //    "filter": true,
    //    "bDestroy": true,
    //    "ajax": {
    //        "url": "https://localhost:7193/api/Account/GetMarchants",
    //        "type": "GET",
    //        "datatype": "json"
    //    },
    //    "columnDefs": [{
    //        "targets": [0],
    //        "visible": false,
    //        "searchable": false
    //    }],
    //    "columns": [
    //        { "data": "logDateTime", "autowidth": true },
    //        { "data": "firstName","autowidth": true },
    //        { "data": "lastName","autowidth": true },
    //        { "data": "email", "autowidth": true },
    //        //{ "data": "contact", "name": "Contact", "autowidth": true },
    //        //{ "data": "email", "name": "Email", "autowidth": true },
    //        //{ "data": "dateOfBirth", "name": "DateOfBirth", "autowidth": true },
    //        //{
    //        //    "render": function (data, type, row) { return '<span>' + row.dateOfBirth.split('T')[0] + '</span>' },
    //        //    "name": "DateOfBirth"
    //        //},
    //        //{
    //        //    "render": function (data, type, row) { return '<a href="#" class="btn btn-danger" onclick=DeleteCustomer("' + row.id + '"); > Delete </a>' },
    //        //    "orderable": false
    //        //},

    //    ]
    //});

    $.ajax({
        type: "GET",
        url: "https://localhost:7193/api/Account/GetMarchants",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            OnSuccess(response);
        },
        failure: function (response) {
            alert(response.d);
        },
        error: function (response) {
            alert(response.d);
        }
    });


});



function OnSuccess(response) {
    var table = $('#Marchants').DataTable({
        data: response,
        order: [[0, 'desc']],
        destroy: true,
        lengthMenu: [[50, -1], [50, "All"]],
        columns: [
            { "data": "logDateTime"},
            { "data": "firstName" },
            { "data": "lastName" },
            { "data": "email" },
        ]
    });
};