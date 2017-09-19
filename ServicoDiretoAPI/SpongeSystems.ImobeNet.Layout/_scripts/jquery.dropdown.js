/// <reference path="jquery-1.4.1-vsdoc.js" />
var DropDown =
{
    SelectItem: function (item) {
        var text = item.html();
        var lang = item.attr('lang');

        $(".dropdown dt a span").html(text);
        $(".dropdown dd ul").hide();
        //$("#result").html(lang);
    },
    Load: function (initialLanguange) {
        if (initialLanguange != null)
            DropDown.SelectItem($(".dropdown dd ul li a:[lang=" + initialLanguange + "]"));

        $(".dropdown dt a").click(function () {$(".dropdown dd ul").toggle();});
        $(".dropdown dd ul li a").click(function () {DropDown.SelectItem($(this));});
        $(document).bind('click', function (e) {
            var $clicked = $(e.target);
            if (!$clicked.parents().hasClass("dropdown"))
                $(".dropdown dd ul").hide();
        });
    }

}

