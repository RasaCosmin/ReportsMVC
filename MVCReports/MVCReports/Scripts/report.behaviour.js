$(document).ready(function () {
    var currentType = "";

    $("#report-type-list li").each(function () {
        var itemClass = $(this).attr('class');

        if (itemClass == "active") {
            currentType = $(this).find('a').attr('name');
        }
    });

    $("#generate-report-btn").click(function () {
        sendCustomersFromList(currentType);
    });

    selectCheckBoxOnNameClick();
});

function sendCustomersFromList(currentType) {
    var customers = [];

    switch (currentType) {
        case "Email":
            break;
        case "Pie":
            customers = getCustomers();
            break;
        case "Accuracy":
            break;
        case "Stacked":
            customers = getCustomers();
            break;
        default:
            break;
    }
   

    $.post("/Home/GenerateReport", { customers: JSON.stringify(customers) }, function (result) {
        $("#report-center").html(result);
    });
}

function selectCheckBoxOnNameClick() {
    $("#customers-list li").each(function () {
        $(this).click(function () {
            var isChecked = $(this).find("#customer-check").is(":checked");
            var name = $(this).find("#customer-name").text();

            if (isChecked == true) {
                $(this).find("#customer-check").prop('checked', false);
            } else {
                $(this).find("#customer-check").prop('checked', true);
            }
        });
    });
};

function getCustomers() {
    var customers = [];
    var startDate = $("#datepickerStart").val();
    var endDate = $("#datepickerEnd").val();

    $("#customers-list li").each(function () {
        var isChecked = $(this).find("#customer-check").is(":checked");
        var name = $(this).find("#customer-name").text();
        var customer = {
            Name: name,
            Checked: isChecked
        };

        customers.push(customer);
    });

    var customersObj = {
        StartDate: startDate,
        EndDate: endDate,
        Customers: customers
    };

    return customersObj;
}