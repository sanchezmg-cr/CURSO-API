
function ParseDataSourceToJson(dataSource) {
    return JSON.parse(JSON.stringify(dataSource));
}

function ParseViewModelPropertyToJson(property) {
    return JSON.parse(JSON.stringify(property));
}

function SubmitFormSingle(formName) {
    let result = DevExpress.validationEngine.validateGroup();
    if (result.isValid)
        $("#" + formName).submit();
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


//pasar parametros dinamicamente
function GetGridBasicFormatOptions(dynamicParametersLinksParent, customSelectionChanged) {
    return {
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        filterRow: {
            visible: false,
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





function GetAllDataGrid(grid) {




    var grid = $(grid).dxDataGrid("instance");

    var data;

    grid.getDataSource().store().load().done((allData) => {

        data = allData;

    });




    return data;




};