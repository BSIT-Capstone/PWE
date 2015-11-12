jQuery(document).ready(function(){
    $('#report_selecter').change(function () {
        var report_name_selected = $('#report_selecter').val();
        
        $(".reportParameters").each(function (i) {
            var report_name_div = $(this).attr('name');
            if (report_name_selected == report_name_div) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    $('#reportForm').submit(function (e) {
        e.preventDefault();
        var formData = $(this).serializeArray();
        //var formSerial = $(this).serialize();
        var reportName = formData[0].value;
        var reportP = "";
        var reportN = "";
        //check if report value is selected
        if (reportName == "") {
            alert("Need to select a report");
            return false;
        }
        //start building url string
        var urlString = "/Reports/DownloadReport?";
        //add report to string
        urlString += "reportname=" + reportName;

        len = formData.length,
        dataObj = {};
        paramObj = {};
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
                    alert("Need to select a parameter");
                    return false;
                } else {
                    urlString += formData[i].value;
                }
               
            }
        }

         //alert(urlString);
        
        window.location.href = urlString;
    });
});