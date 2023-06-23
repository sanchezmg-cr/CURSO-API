DevExpress.localization.locale("es");

DevExpress.config({
    editorStylingMode: "outlined"
});

function PageLoadDefinition(htmlContainerId, pageUrl, loadControlFunction) {
    this.HtmlContainerId = htmlContainerId;
    this.PageUrl = pageUrl;
    this.LoadControlFunction = loadControlFunction;
    this.Parameters = Array.prototype.slice.call(arguments, 3);
    this.ExecuteLoadFunction = function () {
        //if (typeof this.LoadControlFunction === "function") {
        //}
        window[this.LoadControlFunction].apply(null, this.Parameters);
    };
}

function CreatePageMessage(typeMessage, textMessage) {

    return JSON.stringify({ messageType: typeMessage, message: textMessage });
}


//Presenta una notificación en la aplicación a partir de un objeto tipo PageMessage
function ShowPageMessage(pageMessage) {
    //Se contempla un problema de camelCase o PascalCase dependiendo del serializador del mensaje (JsonResult o JsonConvert)
    let messageType = pageMessage.messageType;
    if (messageType == null) messageType = pageMessage.MessageType;

    let message = pageMessage.message;
    if (message == null) message = pageMessage.Message;

    ShowMessage(messageType, message);
}

//Presenta una notificación en la aplicación a partir de un objeto tipo PageMessage
function ShowPageMessageModal(pageMessage, alertContentId, alertMessageId) {
    //Se contempla un problema de camelCase o PascalCase dependiendo del serializador del mensaje (JsonResult o JsonConvert)
    let messageType = pageMessage.messageType;
    if (messageType == null) messageType = pageMessage.MessageType;

    let message = pageMessage.message;
    if (message == null) message = pageMessage.Message;

    ShowMessageModal(messageType, message, alertContentId, alertMessageId);
}

//Presenta una notificación segun los parametros recibidos
function ShowMessage(messageType, message) {
    let alertClass;
    let iconClass;
    let alertDiv = $("#alert_content");
    let alertMessageDiv = $("#alert_message");
    let icon = $("div.alert-icon > i");

    if (alertMessageDiv)
        alertMessageDiv.html(message);
    switch (messageType) {
        case 0:
            iconClass = "flaticon-exclamation-1";
            alertClass = "alert alert-info fade show";
            break;
        case 1:
            iconClass = "flaticon-interface-5";
            alertClass = "alert alert-success fade show";
            break;
        case 2:
            iconClass = "flaticon-warning";
            alertClass = "alert alert-warning fade show";
            break;
        case 3:
            iconClass = "flaticon-questions-circular-button";
            alertClass = "alert alert-info fade show";
            break;
        default:
            iconClass = "flaticon-cancel";
            alertClass = "alert alert-danger fade show";
            break;
    }
    //Indica la clase del icono
    if (icon) {
        icon.removeClass();
        icon.addClass(iconClass);
    }

    //Indica la clase del div según el tipo de alerta, y lo muestra
    if (alertDiv) {
        alertDiv.removeClass();
        alertDiv.addClass(alertClass);
        alertDiv.fadeIn("slow");
        if (messageType < 3)
            setTimeout(HideAlert, 5000);
    }
}

function ShowMessageModal(messageType, message, alertContentId, alertMessageId) {
    let alertClass;
    let iconClass;
    let alertDiv = $("#" + alertContentId);
    let alertMessageDiv = $("#" + alertMessageId);
    let icon = $("div.alert-icon > i");

    if (alertMessageDiv)
        alertMessageDiv.html(message);
    switch (messageType) {
        case 0:
            iconClass = "flaticon-exclamation-1";
            alertClass = "alert alert-info fade show";
            break;
        case 1:
            iconClass = "flaticon-interface-5";
            alertClass = "alert alert-success fade show";
            break;
        case 2:
            iconClass = "flaticon-warning";
            alertClass = "alert alert-warning fade show";
            break;
        case 3:
            iconClass = "flaticon-questions-circular-button";
            alertClass = "alert alert-info fade show";
            break;
        default:
            iconClass = "flaticon-cancel";
            alertClass = "alert alert-danger fade show";
            break;
    }
    //Indica la clase del icono
    if (icon) {
        icon.removeClass();
        icon.addClass(iconClass);
    }

    //Indica la clase del div según el tipo de alerta, y lo muestra
    if (alertDiv) {
        alertDiv.removeClass();
        alertDiv.addClass(alertClass);
        alertDiv.fadeIn("slow");
        if (messageType < 4) {
            //Not supported in IE9 and earlier
            setTimeout(HideAlertModal, 5000, alertContentId);
        }            
    }
}

//Presenta mensaje de confirmación para eliminar un elemento
function ShowDeleteConfirmationButtonMessage(form) {
    swal.fire({
        title: '¿Está seguro de eliminar el elemento indicado?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
        reverseButtons: false
    }).then(function (result) {
        if (result.value) {
            $("#" + form).submit();
        } else if (result.dismiss === 'cancel') {
            return false;
        }
    });
}


//Presenta mensaje de confirmación para redirigir a cambio de compañia
function ShowConfirmationMessageCompanyChange(url, title, type, user) {
    swal.fire({
        title: title,
        type: type,
        showCancelButton: true,
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            if (url != null) {
                window.location.href = url + "?companyChange=true" + "&user=" + user;
            }
        } else if (result.dismiss === 'cancel') {
            return false;
        }
    });
}

//Presenta mensaje de confirmación para redirigir a cambio de centro
function ShowConfirmationMessageCenterChange(url, title, type) {
    swal.fire({
        title: title,
        type: type,
        showCancelButton: true,
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            if (url != null) {
                window.location.href = url + "?centerChange=true";
            }
        } else if (result.dismiss === 'cancel') {
            return false;
        }
    });
}

//Presenta mensaje de confirmación para redirigirse a otra página
function ShowConfirmationNavigationMessage(url, title, text) {
    swal.fire({
        title: title,
        text: text,
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            if (url != null) {
                window.location.href = url;
            }
        } else if (result.dismiss === 'cancel') {
            return false;
        }
    });
}

function HideAlert() {
    $("#alert_content").fadeOut("slow");
}

function HideAlertModal(alertContentId) {
    $("#" + alertContentId).fadeOut("slow");
}

function getTreeViewSelectedItemsKeys(items) {
    var result = [];
    items.forEach(function (item) {
        if (item.selected) {
            result.push(item.key);
        }
        if (item.items.length) {
            result = result.concat(getTreeViewSelectedItemsKeys(item.items));
        }
    });
    return result;
};

function syncTreeViewSelection(treeView, value) {
    if (!value) {
        treeView.unselectAll();
    } else {
        treeView.selectItem(value);
    }
};

function ParseViewModelPropertyToJson(property) {
    return JSON.parse(JSON.stringify(property));
}

function ParseObjectToJson(object) {
    return JSON.parse(JSON.stringify(object));
}

function SetDefaultFormValidation(formName) {
    $("#" + formName).on("submit", function (e) {
        let result = DevExpress.validationEngine.validateGroup();
        if (!result.isValid) {
            return false;
        }
    });
}

function SetSaveActionButtonsEvent(formName, saveActionHidenId) {
    $("a[saveAction]").click(function () {
        let saveAction = $(this).attr("saveAction");
        SubmitForm(saveActionHidenId, saveAction, formName);
    });

    $("button[saveAction]").click(function () {
        let saveAction = $(this).attr("saveAction");
        SubmitForm(saveActionHidenId, saveAction, formName);
    });
}

function SubmitForm(saveActionHidenId, saveAction, formName) {
    $("#" + saveActionHidenId).val(saveAction);
    let result = DevExpress.validationEngine.validateGroup();
    if (result.isValid)
        $("#" + formName).submit();
}

function SubmitFormSingle(formName) {
    let result = DevExpress.validationEngine.validateGroup();
    if (result.isValid)
        $("#" + formName).submit();
}


function ParseDataSourceToJson(dataSource) {
    return JSON.parse(JSON.stringify(dataSource));
}

function SetHtmlContent(pageToLoad, content) {
    var contentDiv = $("#" + pageToLoad.HtmlContainerId);
    contentDiv.empty();
    contentDiv.html(content);
    pageToLoad.ExecuteLoadFunction();
}

function ApplyFormSubmitContentPanel(postUrl, formId, pageToLoad) {
    if (postUrl != null) {
        $.ajax({
            type: "POST",
            url: postUrl,
            data: $("#" + formId).serialize()
        }).done(function (response) {
            SetHtmlContent(pageToLoad, response);
        }).fail(AjaxFailMessage);
    }
}

function SubmitFormEventContentPanel(formId, pageToLoad) {
    let result = DevExpress.validationEngine.validateGroup();
    if (!result.isValid) {
        return false;
    }
    //Atributo asp-page indicado en el form
    let postUrl = $("#" + formId).attr("action");
    ApplyFormSubmitContentPanel(postUrl, formId, pageToLoad);
}

function SetSubmitFormEventContentPanel(formId, pageToLoad) {
    $("#" + formId).submit(function (e) {
        //let result = DevExpress.validationEngine.validateGroup();
        //if (!result.isValid) {
        //    return false;
        //}
        ////Atributo asp-page indicado en el form
        //let postUrl = $(this).attr("action");
        //ApplyFormSubmitContentPanel(postUrl, $(this).attr("id"), pageToLoad);
        SubmitFormEventContentPanel(formId, pageToLoad);
        e.preventDefault();
    });
}

//dynamicParametersLinksParent: se utiliza si se quire limitar a un subconjunto de links con la propiedad data-dynamic-parameters dentro de la página,
//                              para reemplazarle dinámicamente los parámetros en el href.  Puede ser nulo o no enviarse el parámetro si se aplica la funcionalidad a toda la página.
function GetGridBasicFormatOptions(dynamicParametersLinksParent, customSelectionChanged) {
    return {
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        searchPanel: {
            visible: false,
            width: 240,
            placeholder: "Buscar..."
        },
        headerFilter: {
            visible: true
        },
        scrolling: {
            showScrollbar: "onHover",
            scrollByContent: true,
            scrollByThumb: true
        },
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            var actions;
            //debugger;
            if (data) {
                //Si se especifica un parent para el bloque de links a actualizar dinámicamente, entonces se utiliza, de lo contrario se cambian todos los links de la página.
                if (dynamicParametersLinksParent == null) {
                    actions = $("a[data-dynamic-parameters]");
                }
                else
                    actions = $("#" + dynamicParametersLinksParent).find("a[data-dynamic-parameters]");
                UpdateDynamicUrlActions(actions, "href", data);

                //Si se especifica un parent para el bloque de buttons a actualizar dinámicamente, entonces se utiliza, de lo contrario se cambian todos los buttons de la página.
                if (dynamicParametersLinksParent == null) {
                    actions = $("a[data-dynamic-parameters]");
                }
                else
                    actions = $("#" + dynamicParametersLinksParent).find("a[data-dynamic-parameters]");
                UpdateDynamicUrlActions(actions, "panelContentPageUrl", data);
            }
            //Funcion custom que puede ser implementada en la página que sea requerido
            if (typeof customSelectionChanged == 'function')
                customSelectionChanged(selectedItems);
        }
    };
}

function GetGridEditBasicFormatOptions() {
    return {
        showRowLines: true,
        paging: {
            enabled: true,
            pageIndex: 0,
            pageSize: 7
        },
        editing: {
            mode: "row",
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true
        },
        showBorders: true,
        selection: {
            mode: "single"
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        headerFilter: {
            visible: true
        }
    };
}

//dynamicParametersLinksParent: se utiliza si se quire limitar a un subconjunto de links con la propiedad data-dynamic-parameters dentro de la página,
//                              para reemplazarle dinámicamente los parámetros en el href.  Puede ser nulo o no enviarse el parámetro si se aplica la funcionalidad a toda la página.
function GetTreeListBasicFormatOptions(dynamicParametersLinksParent) {
    return {
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        headerFilter: {
            visible: true
        },
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            var actions;
            if (data) {
                //Si se especifica un parent para el bloque de links a actualizar dinámicamente, entonces se utiliza, de lo contrario se cambian todos los links de la página.
                if (dynamicParametersLinksParent == null) {
                    actions = $("a[data-dynamic-parameters]");
                }
                else
                    actions = $("#" + dynamicParametersLinksParent).find("a[data-dynamic-parameters]");
                UpdateDynamicUrlActions(actions, "href", data);

                //Si se especifica un parent para el bloque de buttons a actualizar dinámicamente, entonces se utiliza, de lo contrario se cambian todos los buttons de la página.
                if (dynamicParametersLinksParent == null) {
                    actions = $("button[data-dynamic-parameters]");
                }
                else
                    actions = $("#" + dynamicParametersLinksParent).find("button[data-dynamic-parameters]");
                UpdateDynamicUrlActions(actions, "panelContentPageUrl", data);
            }
        }
    };
}

//actions: hiperlinks o buttons que deben actualizarse
//urlTag: define el tipo de tag html que va a actualizarse, ya sea href o urlPage
function UpdateDynamicUrlActions(actions, urlTag, data) {
    actions.each(function (index) {
        //data-dynamic-parameters pueden definirse separados por coma, por ejemplo: "id,name,customerId" o "id".
        //el nombre de cada parámetro debe venir tal cual se encontrará en el grid y en el url, incluyendo el Pascal Case o Camel Case según sea el caso
        var url = $(this).attr(urlTag);

        if (url == null) {
            return true;
        }

        var parameters = $(this).attr("data-dynamic-parameters").split(",");
        for (i = 0; i < parameters.length; i++) {
            var parameter = parameters[i];
            var initialPosition = url.indexOf("?" + parameter + "=");  //Asume que el parámetro sería el primero del URL.
            if (initialPosition == -1) initialPosition = url.indexOf("&" + parameter + "=");  //Si no lo encuentra asume que sería un parámetro posterior al primero del URL.
            if (initialPosition == -1) {
                alert("Se esperaba el parámetro dinámico [" + parameter + "] y no fue encontrado.");
                return false;
            }
            var finalPosition = url.indexOf("&", initialPosition + 1);
            if (finalPosition == -1) finalPosition = url.length; //Si no hay más parámetros entonces la posición final a reemplazar es el último caracter del URL.
            var searchText = url.substring(initialPosition, finalPosition);
            var parameterValue = searchText.substring(searchText.indexOf("="));
            var replacedText = searchText.replace(parameterValue, "=" + data[parameter]);
            url = url.replace(searchText, replacedText);
        }
        $(this).attr(urlTag, url);
    });
}

//Funcion para establecer la navegación a nivel del panel de la derecha
function SetNavegationLinkButtonsClickEvent(htmlContentElementId, loadControlsFunctionName) {
    $("a[panelContentPageUrl]").click(function () {
        let url = $(this).attr("panelContentPageUrl");
        LoadPartialPageContent(htmlContentElementId, url, loadControlsFunctionName);
    });
}

//Funcion utilizada en las vistas index especiales que utilizan un tree view para navegacion
function SetTreeViewMenuItemUrl(aHtmlComponent, createUrl, editUrl, detailsUrl, deleteUrl) {
    let iHtmlComponent = aHtmlComponent.find("i");
    if (iHtmlComponent.hasClass("flaticon-add-circular-button")) {   //Nuevo
        aHtmlComponent.attr("pageUrl", createUrl);
    } else {
        if (iHtmlComponent.hasClass("flaticon-file-2")) {            //Detalle
            aHtmlComponent.attr("pageUrl", detailsUrl);
        } else {
            if (iHtmlComponent.hasClass("flaticon-edit")) {          //Editar
                aHtmlComponent.attr("pageUrl", editUrl);
            } else {
                if (iHtmlComponent.hasClass("flaticon-delete")) {    //Eliminar
                    aHtmlComponent.attr("pageUrl", deleteUrl);
                }
            }
        }
    }
}

function AjaxFailMessage(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status === 0) {

        alert('Not connect: Verify Network.');

    } else if (jqXHR.status == 404) {

        alert('Requested page not found [404]. ' + errorThrown);

    } else if (jqXHR.status == 500) {

        alert('Internal Server Error [500]. ' + errorThrown);

    } else if (textStatus === 'parsererror') {

        alert('Requested JSON parse failed. ' + errorThrown);

    } else if (textStatus === 'timeout') {

        alert('Time out error.');

    } else if (textStatus === 'abort') {

        alert('Ajax request aborted. ' + errorThrown);

    } else {

        alert('Uncaught Error: ' + jqXHR.responseText + '. ' + errorThrown);

    }
}

function GetHtmlEditorDefaultToolbarOptions() {
    return {
        items: [
            "undo", "redo", "separator",
            {
                formatName: "size",
                formatValues: ["8pt", "10pt", "12pt", "14pt", "18pt", "24pt", "36pt"]
            },
            {
                formatName: "font",
                formatValues: ["Arial", "Courier New", "Georgia", "Impact", "Lucida Console", "Tahoma", "Times New Roman", "Verdana"]
            },
            "separator", "bold", "italic", "strike", "underline", "separator",
            "alignLeft", "alignCenter", "alignRight", "alignJustify", "separator",
            {
                formatName: "header",
                formatValues: [false, 1, 2, 3, 4, 5]
            }, "separator",
            "orderedList", "bulletList", "separator",
            "color", "background", "separator",
            "link", "image", "separator",
            "clear", "codeBlock", "blockquote"
        ]
    }
}

function LoadPartialPageContent(htmlElementId, pageUrl, loadControlsFunctionName /*, dynamic arguments of loadControlsFunctionName*/) {
    if (pageUrl != null) {
        //Define los argumentos dinámicos de la función
        //Los argumentos dinámicos vienen desde el 4to argumento
        let args = Array.prototype.slice.call(arguments, 3);
        $.ajax({
            type: "GET",
            url: pageUrl,
            contentType: "application/json"
        }).done(function (response) {
            let htmlElementContent = $("#" + htmlElementId);
            htmlElementContent.empty();
            htmlElementContent.html(response);
            if (loadControlsFunctionName != "") {
                //Invoca la función pasando los argumentos como un arreglo
                window[loadControlsFunctionName].apply(null, args);
            }
        }).fail(AjaxFailMessage);
    }
}

//Obtiene toda la data dell grid contemplando la paginacion
function GetAllDataGrid(grid) {

    var grid = $(grid).dxDataGrid("instance");
    var data;
    grid.getDataSource().store().load().done((allData) => {
        data = allData;
    });

    return data;

};
