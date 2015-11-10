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
        var formSerial = $(this).serialize();
        var reportName = formData[0].value;
        var reportP = "";
        var reportN = "";
        if (reportName == "") {
            alert("Need to select a report");
            return false;
        }
        var urlString = "/Reports/DownloadReport?";

        len = formData.length,
        dataObj = {};
        for (i = 0; i < len; i++) {
            if (reportName == formData[i].name) {
                if (formData[i].value == "") {
                    alert("Need to select a parameter");
                    return false;
                }
            }
            if (formData[i].value != "") {
                dataObj[formData[i].name] = formData[i].value;
            }
        }

        var reportN = dataObj['report_selecter'];
        if (reportN != null) {
            urlString += "ReportName=" + reportN;
        }

        var reportP = dataObj[reportName];

        if (reportP == null) {
              reportP = "";
        } else {
            urlString += "&ReportParam="+reportP
        }
       // alert(urlString);
        window.location.href = urlString;
    });
});