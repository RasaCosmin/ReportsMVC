    $(document).ready(function () {
    	$("#generate-report-btn").click(function () {
    	    var customers = getCustomers();
    	    var jsonObjects = {
    	        Customers : [ { Name: "Aaaa", Checked: false }]
    	    };

    	    var jsonObject = {
    	        Customer :"Aaaa"
    	    };

            //$.ajax({
            //    url: "Home/GenerateReport",
            //    type: "GET",
            //    data: JSON.stringify(jsonObject),
            //    dataType: "json",
            //    contentType: "application/json; charset=utf-8",
            //    success: function (result) {
                    
            //        $("#partial-customer").html(result);
            //    },
            //    error: function (response) {
     
            //    }
            //});

    	    //$.getJSON("/Home/GenerateReport", { customers: JSON.stringify(customers) }, function (result) {
    	    //    $("#partial-customer").html(result);
    	    //});

    	    $.post("/Home/GenerateReport",{ customers: JSON.stringify(customers) }, function (result) {
    	        $("#report-center").html(result);
    	    });
        });
    });

    function getCustomers() {
        var customers = [];
        var startDate = $("#datepickerStart").val();
        var endDate = $("#datepickerEnd").val();

    	$("#customers-list li").each(function () {
    		var isChecked = $(this).find("#customer-check").val();
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