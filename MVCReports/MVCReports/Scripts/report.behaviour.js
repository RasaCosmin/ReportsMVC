    $(document).ready(function () {
    	$("#generate-report-btn").click(function () {
    	    var customers = getCustomers();

    	    $.post("/Home/GenerateReport",{ customers: JSON.stringify(customers) }, function (result) {
    	        $("#report-center").html(result);
    	    });
    	});

    	selectCheckBoxOnNameClick();
    });

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
                Checked : isChecked
    		};

    		customers.push(customer);
    	});

    	var customersObj = {
    	    StartDate: startDate,
            EndDate : endDate,
            Customers : customers
    	};

    	return customersObj;
    }