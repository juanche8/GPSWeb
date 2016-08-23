// container is either the ReportViewer control itself, or a div containing it. 

function fixReportingServices(container) {

    if ($.browser.safari) { // toolbars appeared on separate lines. 

        $('#' + container + ' table').each(function(i, item) {

            if ($(item).attr('id') && $(item).attr('id').match(/fixedTable$/) != null)

                $(item).css('display', 'table');

            else

                $(item).css('display', 'inline-block');

        });

    }

}

// needed when AsyncEnabled=true. 

//Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() { fixReportingServices('rpt-container'); });

$(document).ready(function () { fixReportingServices('rpt-container');});