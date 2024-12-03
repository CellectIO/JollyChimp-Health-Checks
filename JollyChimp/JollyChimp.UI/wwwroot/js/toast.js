//GLOBAL TOASTR SETTINGS
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "linear",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

function showSuccessToast(successMsg, callback){
    toastr.success(successMsg, 'Success!');
    toastr.warning('Changes will only take affect after the application has been restarted', 'Remember!');

    if (callback){
        callback();
    }
}

function showErrorToast(errorMsg, callback){
    toastr.error(errorMsg, 'Error!');

    if (callback){
        callback();
    }
}