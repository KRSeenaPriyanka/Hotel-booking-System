function SetCheckinDate(executionContext) {
    try {
        var formContext = executionContext.getFormContext();
        var roomStatus = formContext.getAttribute("kopr_roomstatus").getValue();
        var currentDate = new Date();
        if (roomStatus == 1) {
            formContext.getAttribute("kopr_checkindate").setValue(currentDate);
        }
        else if (roomStatus == 2) {
            formContext.getAttribute("kopr_checkindate").setValue(null);
        }
    }
    catch (e) {
        xrm.Utility.alertDialog(e.message);
    }
}

function ValidationCheckoutDate(executionContext) {
    try {
        var formContext = executionContext.getFormContext();

        var checkinDate = formContext.getAttribute("kopr_checkindate").getValue();
        checkinDate = new Date(checkinDate);
        var plannedCheckoutDate = formContext.getAttribute("kopr_plannedcheckoutdate").getValue();
        plannedCheckoutDate = new Date(plannedCheckoutDate);
        if (plannedCheckoutDate < checkinDate) {

            alert("please select planned checkout Date after checkin Date");
            formContext.getAttribute("kopr_plannedcheckoutdate").setValue(null);
        }
    }
     catch (e) {
     xrm.Utility.alertDialog(e.message);
    }
}