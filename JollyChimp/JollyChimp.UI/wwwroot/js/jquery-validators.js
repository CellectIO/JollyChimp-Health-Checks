//------------------------
// TODO:
// Figure out a way to reuse some of the copied code in these validators.
// I am too lazy to figure out a way right now.
//------------------------

$.validator.addMethod("regex", function (value, element, pattern) {
    return this.optional(element) || pattern.test(value);
}, "Invalid format.");

jQuery.validator.addMethod("hc_validators_endPointPredicate", function (value, element, param) {
    if (this.optional(element)) return "dependency-mismatch";

    var previous = this.previousValue(element);

    if (!this.settings.messages[element.name]) {
        this.settings.messages[element.name] = {};
    }

    previous.originalMessage = this.settings.messages[element.name].remote;
    this.settings.messages[element.name].remote = previous.message;

    param = typeof param == "string" && { url: param } || param;

    if (this.pending[element.name]){
        return "pending";
    }
    
    if (previous.old === value) return previous.valid;

    previous.old = value;

    var validator = this;
    this.startRequest(element);

    api.endPoints.validatePredicate(
        value,
        function (response) {
            validator.settings.messages[element.name].remote = "HealthChecks Predicate is not a valid predicate for type Func<HealthCheckRegistration, bool>. Try: hc => true";
            var valid = response === true;
            if (valid) {
                var submitted = validator.formSubmitted;
                validator.prepareElement(element);
                validator.formSubmitted = submitted;
                validator.successList.push(element);
                validator.showErrors();
            } else {
                var errors = {};
                var message = response || validator.defaultMessage(element, "remote");
                errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
                validator.showErrors(errors);
            }
            previous.valid = valid;
            validator.stopRequest(element, valid);
        },
        function (xhr) {
            var errors = {};
            var message = xhr.message || validator.defaultMessage(element, "remote");
            errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
            validator.showErrors(errors);
            previous.valid = false;
            validator.stopRequest(element, false);
        }
    );

    return "pending";
}, "");

jQuery.validator.addMethod("hc_validators_endPointApiPath", function (value, element, param) {
    if (this.optional(element)) return "dependency-mismatch";

    var previous = this.previousValue(element);

    if (!this.settings.messages[element.name]) {
        this.settings.messages[element.name] = {};
    }

    previous.originalMessage = this.settings.messages[element.name].remote;
    this.settings.messages[element.name].remote = previous.message;

    param = typeof param == "string" && { url: param } || param;

    if (this.pending[element.name]){
        return "pending";
    }
    
    if (previous.old === value) return previous.valid;

    previous.old = value;

    var validator = this;
    this.startRequest(element);

    api.endPoints.validateApiPath(
        value,
        function (response) {
            validator.settings.messages[element.name].remote = `HealthChecks Api Path (${value}) is already registered.`;
            var valid = response === true;
            if (valid) {
                var submitted = validator.formSubmitted;
                validator.prepareElement(element);
                validator.formSubmitted = submitted;
                validator.successList.push(element);
                validator.showErrors();
            } else {
                var errors = {};
                var message = response || validator.defaultMessage(element, "remote");
                errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
                validator.showErrors(errors);
            }
            previous.valid = valid;
            validator.stopRequest(element, valid);
        },
        function (xhr) {
            var errors = {};
            var message = xhr.message || validator.defaultMessage(element, "remote");
            errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
            validator.showErrors(errors);
            previous.valid = false;
            validator.stopRequest(element, false);
        }
    );

    return "pending";
}, "");

jQuery.validator.addMethod("hc_validators_webHookPredicate", function (value, element, param) {
    if (this.optional(element)) return "dependency-mismatch";

    var previous = this.previousValue(element);

    if (!this.settings.messages[element.name]) {
        this.settings.messages[element.name] = {};
    }

    previous.originalMessage = this.settings.messages[element.name].remote;
    this.settings.messages[element.name].remote = previous.message;

    param = typeof param == "string" && { url: param } || param;

    if (this.pending[element.name]){
        return "pending";
    }
    
    if (previous.old === value) return previous.valid;

    previous.old = value;

    var validator = this;
    this.startRequest(element);

    api.webHooks.validatePredicate(
        value,
        function (response) {
            validator.settings.messages[element.name].remote = "WebHook Predicate is not a valid predicate for type Func<string, UIHealthReport, bool>. Try: (reportName, report) => true";
            var valid = response === true;
            if (valid) {
                var submitted = validator.formSubmitted;
                validator.prepareElement(element);
                validator.formSubmitted = submitted;
                validator.successList.push(element);
                validator.showErrors();
            } else {
                var errors = {};
                var message = response || validator.defaultMessage(element, "remote");
                errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
                validator.showErrors(errors);
            }
            previous.valid = valid;
            validator.stopRequest(element, valid);
        },
        function (xhr) {
            var errors = {};
            var message = xhr.message || validator.defaultMessage(element, "remote");
            errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
            validator.showErrors(errors);
            previous.valid = false;
            validator.stopRequest(element, false);
        }
    );

    return "pending";
}, "");

jQuery.validator.addMethod("hc_validators_healthCheckName", function (value, element, param) {
    if (this.optional(element)){
        return "dependency-mismatch";
    }
    
    var previous = this.previousValue(element);
    
    if (!this.settings.messages[element.name]) {
        this.settings.messages[element.name] = {};
    }

    previous.originalMessage = this.settings.messages[element.name].remote;
    this.settings.messages[element.name].remote = previous.message;

    if (this.pending[element.name]){
        return "pending";
    }

    if (previous.old === value){
        return previous.valid;
    }
    
    previous.old = value;
    
    var validator = this;
    this.startRequest(element);

    api.healthChecks.validateName(
        {
            "Name": value,
            "HealthCheckId": ($(`#${param.idFormFieldIdentifier}`).val()) ? $(`#${param.idFormFieldIdentifier}`).val() : null
        },
        function (response) {
            validator.settings.messages[element.name].remote = "Health Check name is already in use.";
            var valid = response === true;
            if (valid) {
                var submitted = validator.formSubmitted;
                validator.prepareElement(element);
                validator.formSubmitted = submitted;
                validator.successList.push(element);
                validator.showErrors();
            } else {
                var errors = {};
                var message = response || validator.defaultMessage(element, "remote");
                errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
                validator.showErrors(errors);
            }
            previous.valid = valid;
            validator.stopRequest(element, valid);
        },
        function (xhr) {
            var errors = {};
            var message = xhr.message || validator.defaultMessage(element, "remote");
            errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
            validator.showErrors(errors);
            previous.valid = false;
            validator.stopRequest(element, false);
        }
    );

    return "pending";
}, "");