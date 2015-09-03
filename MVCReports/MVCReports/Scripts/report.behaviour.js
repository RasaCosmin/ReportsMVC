$(document).ready(function () {
    var currentType = "";

    $("#report-type-list li").each(function () {
        var itemClass = $(this).attr('class');
        var name = $(this).find('a').attr('name');

        if (itemClass == "active") {
            currentType = $(this).find('a').attr('name');
        }
    });

    //if (currentType == "Stacked") {
    //    $("#datepickerEnd").css("display", "none");
    //    $("#datepickerEndText").css("display", "none");
    //} else if (currentType == "Pie") {
    //    $("#datepickerEnd").css("display", "block");
    //    $("#datepickerEndText").css("display", "block");
    //} else if (currentType == "Email") {
    //    $("#datepickerEnd").css("display", "block");
    //    $("#datepickerEndText").css("display", "block");
    //}

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
            list = getCustomers(currentType);
            break;
        case "Accuracy":
            list = getCustomer();
            break;
        case "Stacked":
            list = getCustomers(currentType);
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
        var checkBtn = $(this).find("#customer-check");
        $(this).find("#customer-name").click(function () {
            var isChecked = checkBtn.is(":checked");
            var name = $(this).find("#customer-name").text();

            if (isChecked == true) {
                checkBtn.prop('checked', false);
            } else {
                checkBtn.prop('checked', true);
            }
        });

        //$(this).find("#customer-check").click(function () {
        //    var isChecked = $(this).find("#customer-check").is(":checked");
        //    var name = $(this).find("#customer-name").text();

        //    if (isChecked == true) {
        //        $(this).find("#customer-check").prop('checked', false);
        //    } else {
        //        $(this).find("#customer-check").prop('checked', true);
        //    }
        //});
    });
};

function getCustomer() {
    var customers = [];
    var selectedOption = $("#customer-dropdown-list option:selected").text();

    var customer = {
        Name: selectedOption,
        Checked: true
    };

    customers.push(customer);
    var customerObj = {
        StartDate: "",
        EndDate: "",
        Customers: customers
    };

    return customerObj;
}

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

function getCustomers(currentType) {
    var customers = [];
    var startDate = $("#datepickerStart").val();
    var endDate = $("#datepickerEnd").val();;

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

    var customersObj = {}

    if (currentType != "Stacked") {
        customersObj = {
            StartDate: startDate,
            Customers: customers
        };
    } else {
        customersObj = {
            StartDate: startDate,
            EndDate: endDate,
            Customers: customers
        };
    }



    return customersObj;
}