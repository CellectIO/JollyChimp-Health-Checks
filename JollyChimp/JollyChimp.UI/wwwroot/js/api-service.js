const api = {

    baseUrl: '/api',

    _execute: function (endpoint, method, successCallback, errorCallback) {
        $.ajax({
            type: method,
            url: this.baseUrl + endpoint,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            success: function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            },
            error: function (xhr) {
                if (typeof errorCallback === 'function') {
                    errorCallback(xhr);
                }
            }
        });
    },

    _executeWithData: function (endpoint, method, data, successCallback, errorCallback) {
        $.ajax({
            type: method,
            url: this.baseUrl + endpoint,
            data: JSON.stringify(data),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            success: function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            },
            error: function (xhr) {
                if (typeof errorCallback === 'function') {
                    errorCallback(xhr);
                }
            }
        });
    },

    serverSettings: {
        get: function (successCallback, errorCallback) {
            api._execute('/serversettings', 'GET', function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        update: function (data, successCallback, errorCallback) {
            api._executeWithData('/serversettings', 'POST', data, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        }
    },
    endPoints: {
        update: function (formData, successCallback, errorCallback) {
            api._executeWithData('/endpoints', 'PUT', formData, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        validatePredicate: function (formData, successCallback, errorCallback) {
            api._executeWithData('/endpoints/validate/predicate', 'POST', formData, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        validateApiPath: function (formData, successCallback, errorCallback) {
            api._executeWithData('/endPoints/validate/apipath', 'POST', formData, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        delete: function (endPointId, successCallback, errorCallback) {
            api._execute(`/endpoints/${endPointId}`, 'DELETE', function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        }
    },
    webHooks: {
        definitions: function (successCallback, errorCallback) {
            api._execute('/webhooks/definitions', 'GET', function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        validatePredicate: function (formData, successCallback, errorCallback) {
            api._executeWithData('/webhooks/validate/predicate', 'POST', formData, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        update: function (formData, successCallback, errorCallback) {
            api._executeWithData('/webhooks', 'PUT', formData, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        delete: function (webHookId, successCallback, errorCallback) {
            api._execute(`/webhooks/${webHookId}`, 'DELETE', function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        }
    },
    healthChecks: {
        definitions: function (successCallback, errorCallback) {
            api._execute('/healthchecks/definitions', 'GET', function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        validateName: function (formData, successCallback, errorCallback) {
            api._executeWithData('/healthchecks/validate/name', 'POST', formData, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        update: function (formData, successCallback, errorCallback) {
            api._executeWithData('/healthchecks', 'PUT', formData, function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        },
        delete: function (healthCheckId, successCallback, errorCallback) {
            api._execute(`/healthchecks/${healthCheckId}`, 'DELETE', function (response) {
                if (typeof successCallback === 'function') {
                    successCallback(response);
                }
            }, errorCallback);
        }
    }
};
