Type.registerNamespace("Telerik.Sitefinity.HubSpotConnector.Web.UI");

Telerik.Sitefinity.HubSpotConnector.Web.UI.HubSpotConnectorSettings = function (element) {
    Telerik.Sitefinity.HubSpotConnector.Web.UI.HubSpotConnectorSettings.initializeBase(this, [element]);

    this._connectButtonClickedDelegate = null;
    this._changeConnectionButtonClickedDelegate = null;
    this._disconnectReconnectButtonClickedDelegate = null;

    this._portalIdTextBox = null;
    this._apiKeyTextBox = null;

    this._hubSpotModuleEnabled = null;
    this._connectButton = null;
    this._changeConnectionButton = null;
    this._disconnectReconnectButton = null;
    this._connectText = null;
    this._disconnectText = null;
    this._errorMessageWrapperId = null;
    this._loadingViewId = null;

    this._clientManager = null;

    this._connectSuccessDelegate = null;
    this._disconnectReconnectSuccessDelegate = null;
    this._operationFailDelegate = null;
}

Telerik.Sitefinity.HubSpotConnector.Web.UI.HubSpotConnectorSettings.prototype = {
    initialize: function () {
        Telerik.Sitefinity.HubSpotConnector.Web.UI.HubSpotConnectorSettings.callBaseMethod(this, 'initialize');
        
        this._updateVisualControls(true);

        this._clientManager = new Telerik.Sitefinity.Data.ClientManager();
        this._operationFailDelegate = Function.createDelegate(this, this._operationFail);

        this._connectButtonClickedDelegate = Function.createDelegate(this, this._connectButtonClicked);
        this._connectSuccessDelegate = Function.createDelegate(this, this._connectSuccess);
        $addHandler(this._connectButton, "click", this._connectButtonClickedDelegate);

        this._changeConnectionButtonClickedDelegate = Function.createDelegate(this, this._changeConnectionButtonClicked);
        $addHandler(this._changeConnectionButton, "click", this._changeConnectionButtonClickedDelegate);

        this._disconnectReconnectButtonClickedDelegate = Function.createDelegate(this, this._disconnectReconnectButtonClicked); 
        this._disconnectReconnectSuccessDelegate = Function.createDelegate(this, this._disconnectReconnectSuccess);
        $addHandler(this._disconnectReconnectButton, "click", this._disconnectReconnectButtonClickedDelegate);
    },

    dispose: function () {
        if (this._connectButtonClickedDelegate) {
            delete this._connectButtonClickedDelegate;
        }
        if (this._changeConnectionButtonClickedDelegate) {
            delete this._changeConnectionButtonClickedDelegate;
        }
        if (this._disconnectReconnectButtonClickedDelegate) {
            delete this._disconnectReconnectButtonClickedDelegate;
        }
        if (this._connectSuccessDelegate) {
            delete this._connectSuccessDelegate;
        }
        if (this._disconnectReconnectSuccessDelegate) {
            delete this._disconnectReconnectSuccessDelegate;
        }
        if (this._operationFailDelegate) {
            delete this._operationFailDelegate;
        }

        Telerik.Sitefinity.HubSpotConnector.Web.UI.HubSpotConnectorSettings.callBaseMethod(this, 'dispose');
    },

    _updateVisualControls: function (updateViews) {
        var disconnectReconnectButtonTextControl = $("#disconnectReconnectButtonText");
        var serverDecore = $("#serverDecore");
        if (this._hubSpotModuleEnabled) {
            if (updateViews) {
                $("#configurationForm").hide();
                $("#connectionForm").show();
            }

            serverDecore.attr("class", "sfConnectedToDecore");
            disconnectReconnectButtonTextControl.html(this._disconnectText);
        } else {
            if (updateViews) {
                $("#connectionForm").hide();
                $("#configurationForm").show();
            }

            serverDecore.attr("class", "sfConnectedToDecore sfNotConnectedToServerDecore");
            disconnectReconnectButtonTextControl.html(this._connectText);
        }
    },

    _connectButtonClicked: function () {
        $("#" + this._errorMessageWrapperId).hide();
        $("#" + this._connectButton.id).hide();
        $("#" + this._loadingViewId).show();
        var data = {};
        data.PortalId = this._portalIdTextBox.value;
        data.ApiKey = this._apiKeyTextBox.value;
        this._clientManager.InvokePost("/restapi/hubSpot/config", null, null, data, this._connectSuccessDelegate, this._operationFailDelegate, this);
    },

    _connectSuccess: function () {
        $("#" + this._loadingViewId).hide();
        $("#" + this._connectButton.id).show();
        this._hubSpotModuleEnabled = true;
        this._updateVisualControls(true);
    },

    _operationFail: function (responce, sender) {
        $("#" + this._loadingViewId).hide();
        $("#" + this._connectButton.id).show();
        var message = "Operation failed!";
        if (responce && responce.ResponseStatus && responce.ResponseStatus.Message) {
            message = responce.ResponseStatus.Message;
        }

        $("#" + this._errorMessageWrapperId).html(message);
        $("#" + this._errorMessageWrapperId).show();
    },

    _changeConnectionButtonClicked: function () {
        $("#configurationForm").show();
        $("#connectionForm").hide();
    },

    _disconnectReconnectButtonClicked: function () {
        var data = (!this._hubSpotModuleEnabled).toString();
        this._clientManager.InvokePost("/restapi/hubSpot/status", null, null, data, this._disconnectReconnectSuccessDelegate, null, this);
    },

    _disconnectReconnectSuccess: function (component, result) {
        this._hubSpotModuleEnabled = result === "true";
        this._updateVisualControls();
    },

    /* Properties */
    get_portalIdTextBox: function () {
        return this._portalIdTextBox;
    },
    set_portalIdTextBox: function (value) {
        this._portalIdTextBox = value;
    },
    get_apiKeyTextBox: function () {
        return this._apiKeyTextBox;
    },
    set_apiKeyTextBox: function (value) {
        this._apiKeyTextBox = value;
    },
    get_connectButton: function () {
        return this._connectButton;
    },
    set_connectButton: function (value) {
        this._connectButton = value;
    },
    get_changeConnectionButton: function () {
        return this._changeConnectionButton;
    },
    set_changeConnectionButton: function (value) {
        this._changeConnectionButton = value;
    },
    get_disconnectReconnectButton: function () {
        return this._disconnectReconnectButton;
    },
    set_disconnectReconnectButton: function (value) {
        this._disconnectReconnectButton = value;
    }
}

Telerik.Sitefinity.HubSpotConnector.Web.UI.HubSpotConnectorSettings.registerClass('Telerik.Sitefinity.HubSpotConnector.Web.UI.HubSpotConnectorSettings', Sys.UI.Control);