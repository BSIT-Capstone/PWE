//Handles report form on Report View and re-writes URL for report download
jQuery(document).ready(function () { 
    $('#report_selecter').change(function () { //depending on the value selected display or hide parameter field from form
        var report_name_selected = $('#report_selecter').val();
        $('.paramerrormsg').hide();
        $('.productiderrormsg').hide();
        $(".reportParameters").each(function (i) {
            var report_name_div = $(this).attr('name');
            if (report_name_selected == report_name_div) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
    //validate and submit form for report download
    $('#reportForm').submit(function (e) {
        //ProductID
        e.preventDefault();
        var formData = $(this).serializeArray();
        var reportName = formData[0].value;
        var reportP = "";
        var reportN = "";
        //check if report value is selected
        if (reportName == "") {
            $('.reporterrormsg ').show();
            //alert("Need to select a report");
            return false;
        }
        //start building url string
        var urlString = "/PWE_reporting/Reports/DownloadReport?";
        //add report to string
        urlString += "reportname=" + reportName;

        len = formData.length;
        var myReport = "";
        //add parameter values to string
        for (i = 1; i < len; i++) {
            reportP = formData[i].name;
            reportP = reportP.toString().toLocaleLowerCase();
            //check if hidden input value is part of report 
            if (reportName == formData[i].value) {
                urlString += "&" + reportP + "=";
                myReport = formData[i].value;
            }
            //check if parameter select is part of report
            if (myReport == formData[i].name) {
                //check to if parameter value is selected
                if (formData[i].value == "") {
                    $('.paramerrormsg').show();
                    $('.productiderrormsg').show();
                    return false;
                }
                urlString += formData[i].value;
            }
        }
       window.location.href = urlString;
    });
});