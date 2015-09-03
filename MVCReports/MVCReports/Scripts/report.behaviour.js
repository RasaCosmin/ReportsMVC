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
    var list = [];

    switch (currentType) {
        case "Email":
            list = getEmail();
            break;
        case "Pie":
            list = getCustomers();
            break;
        case "Accuracy":
            break;
        case "Stacked":
            list = getCustomers();
            break;
        default:
            break;
    }
   

    $.post("/Home/GenerateReport", { customers: JSON.stringify(list) }, function (result) {
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

function getEmail() {
    var customers = [];
    var selectedOption = $("#email-list option:selected").text();
    var startDate = $("#datepickerStart").val();
    var endDate = $("#datepickerEnd").val();
    var customer = {
        Name: selectedOption,
        Checked : true
    };

    customers.push(customer);
    var customerObj = {
        StartDate: startDate,
        EndDate: endDate,
        Customers: customers
    };

    return customerObj;
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

        if(isChecked)
            customers.push(customer);
    });

    var customersObj = {
        StartDate: startDate,
        EndDate: endDate,
        Customers: customers
    };

    return customersObj;
}