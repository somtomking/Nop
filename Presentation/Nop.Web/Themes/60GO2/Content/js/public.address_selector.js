var AddressSelector = {
    getCitiesByStateProvinceUrl: "",
    getDistrictsByCityUrl: "",
    stateProvinceControlId: "",
    cityControlId: "",
    districtControlId: "",

    init: function (getCitiesByStateProvinceUrl, getDistrictsByCityUrl,
        stateProvinceControlId, cityControlId, districtControlId) {

        this.getCitiesByStateProvinceUrl = getCitiesByStateProvinceUrl;
        this.getDistrictsByCityUrl = getDistrictsByCityUrl;
        this.stateProvinceControlId = stateProvinceControlId;
        this.cityControlId = cityControlId;
        this.districtControlId = districtControlId;

        $(AddressSelector.stateProvinceControlId).change(function () {
            var selectedItem = $(this).val();
            AddressSelector.loadCityList(selectedItem);

            var ddlDistricts = $(AddressSelector.districtControlId);
            ddlDistricts.html('');
            //ddlDistricts.append($('<option></option>').val(0).html("区"));
        });

        $(AddressSelector.cityControlId).change(function () {
            var selectedItem = $(this).val();
            AddressSelector.loadDistrictList(selectedItem);
        });
    },

    loadCityList: function (provinceId, selectedCityId) {
        var ddlCities = $(AddressSelector.cityControlId);
        var statesProgress = $("#states-loading-progress2");
        //statesProgress.show();

        $.ajax({
            cache: false,
            type: "GET",
            url: AddressSelector.getCitiesByStateProvinceUrl,
            data: { "stateProvinceId": provinceId, "addEmptyCityIfRequired": "false" },
            success: function (data) {
                ddlCities.html('');
                $.each(data, function (id, option) {
                    var item = $("<option>").text(option.name).val(option.id);
                    if (option.id == selectedCityId) { item.attr("selected", "selected"); }
                    ddlCities.append(item);
                });
                //statesProgress.hide();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve states.');
                //statesProgress.hide();
            }
        });
    },

    loadDistrictList: function (cityId, selectedDistrictId) {
        var ddlDistricts = $(AddressSelector.districtControlId);
        var statesProgress = $("#states-loading-progress3");
        //statesProgress.show();
        $.ajax({
            cache: false,
            type: "GET",
            url: AddressSelector.getDistrictsByCityUrl,
            data: { "cityId": cityId, "addEmptyDistrictIfRequired": "false" },
            success: function (data) {
                ddlDistricts.html('');
                $.each(data, function (id, option) {
                    var item = $("<option>").text(option.name).val(option.id);
                    if (option.id == selectedDistrictId) { item.attr("selected", "selected"); }
                    ddlDistricts.append(item);
                });
                //statesProgress.hide();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve states.');
                //statesProgress.hide();
            }
        });
    }

}




