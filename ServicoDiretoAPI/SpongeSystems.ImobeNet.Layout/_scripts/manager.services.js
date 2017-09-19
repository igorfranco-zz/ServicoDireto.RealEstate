var Manager =
{
    BaseURL: null,
    Services:
    {
        DeactivateIcon: function (params, callback) {
            NBN.REST.Execute("/icon.aspx/delete", params, callback, "POST");
        },
        DeactivateCulture: function (params, callback) {
            NBN.REST.Execute("/culture.aspx/delete", params, callback, "POST");
        },
        DeactivateCountry: function (params, callback) {
            NBN.REST.Execute("/country.aspx/delete", params, callback, "POST");
        },
        DeactivateStateProvince: function (params, callback) {
            NBN.REST.Execute("/stateprovince.aspx/delete", params, callback, "POST");
        },
        ListStateProvinceByCountry: function (params, callback) {
            NBN.REST.Execute("/stateprovince.aspx/listbycountry", params, callback, "POST");
        },
        DeactivateCity: function (params, callback) {
            NBN.REST.Execute("/city.aspx/delete", params, callback, "POST");
        },
        DeactivateAlert: function (params, callback) {
            NBN.REST.Execute("/alert.aspx/delete", params, callback, "POST");
        },
        DeactivateElement: function (params, callback) {
            NBN.REST.Execute("/element.aspx/Inactivate", params, callback, "POST");
        },
        DeactivatePurpose: function (params, callback) {
            NBN.REST.Execute("/purpose.aspx/delete", params, callback, "POST");
        },
        DeactivateAttribute: function (params, callback) {
            NBN.REST.Execute("/attribute.aspx/delete", params, callback, "POST");
        },
        DeactivateAttributeType: function (params, callback) {
            NBN.REST.Execute("/attributetype.aspx/delete", params, callback, "POST");
        },
        DeactivateFilter: function (params, callback) {
            NBN.REST.Execute("/filter.aspx/delete", params, callback, "POST");
        },
        DeactivateAdsCategory: function (params, callback) {
            NBN.REST.Execute("/advertisement.aspx/delete", params, callback, "POST");
        },
        DeleteElement: function (params, callback) {
            NBN.REST.Execute("/element.aspx/delete", params, callback, "POST");
        },
        DeactivateEmail: function (params, callback) {
            NBN.REST.Execute("/email.aspx/delete", params, callback, "POST");
        },
        MarkAsUnread: function (params, callback) {
            NBN.REST.Execute("/email.aspx/MarkAsUnread", params, callback, "POST");
        }
    }
}