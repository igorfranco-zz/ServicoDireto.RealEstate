var NBN =
{
    Helper:
        {
            LoadAutoComplete: function (fieldNameID, fieldNameValue, URL, additionalScript) {
                $("#" + fieldNameValue).autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: URL,
                            dataType: "json",
                            data: {
                                featureClass: "P",
                                style: "full",
                                minLength: 4,
                                maxRows: 12,
                                name_startsWith: request.term
                            },
                            success: function (data) {
                                response($.map(data.ResponseData.Result[0], function (item) {
                                    return {
                                        label: item.label,
                                        value: item.value,
                                        id: item.id
                                        //index: index
                                    }
                                }));
                            }
                        });
                    },
                    minLength: 0,
                    select: function (event, ui) {
                        $("#" + fieldNameID).val(ui.item.id);
                        if (additionalScript != undefined) {
                            additionalScript();
                        }
                    }
                });

            },
            RunExe: function (path, parameters) {
                try {
                    var ua = navigator.userAgent.toLowerCase();
                    if (ua.indexOf("msie") != -1) {
                        MyObject = new ActiveXObject("WScript.Shell")
                        MyObject.Run(path);
                    } else {
                        netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                        var exe = window.Components.classes['@mozilla.org/file/local;1'].createInstance(Components.interfaces.nsILocalFile);
                        exe.initWithPath(path);
                        var run = window.Components.classes['@mozilla.org/process/util;1'].createInstance(Components.interfaces.nsIProcess);
                        run.init(exe);
                        run.run(false, parameters, parameters.length);
                    }
                } catch (ex) {
                    alert(ex.toString());
                }
            },
            SelectOption: function (controlID, max, index, callback) {
                if ($(controlID + " option").length >= max) {
                    $(controlID + " option:eq(" + index + ")").attr("selected", "selected");
                    if (callback != undefined) {
                        callback.call(this, '');
                    }
                }
            },
            SwitcherOption:
            {
                _IDListBoxSource: 0,
                _IDListBoxDestiny: 0,
                Init: function (idListBoxSource, idListBoxDestiny) {
                    this._IDListBoxSource = "#" + idListBoxSource;
                    this._IDListBoxDestiny = "#" + idListBoxDestiny;
                },
                SelectAllDestiny: function () {
                    $(this._IDListBoxDestiny + " option").each(function (index, item) {
                        $(item).attr("selected", "selected");
                    });
                },
                Add: function () {
                    $(this._IDListBoxSource + " option:selected").appendTo(this._IDListBoxDestiny);
                    this.SelectAllDestiny();
                },
                AddAll: function () {
                    $(this._IDListBoxSource + " option").appendTo(this._IDListBoxDestiny);
                    this.SelectAllDestiny();
                },
                Remove: function () {
                    $(this._IDListBoxDestiny + " option:selected").appendTo(this._IDListBoxSource);
                    this.SelectAllDestiny();
                },
                RemoveAll: function () {
                    $(this._IDListBoxDestiny + " option").appendTo(this._IDListBoxSource);
                }
            },
            FillDropDown: function (idControl, data, initialItem, selectedValue) {
                $("#" + idControl).html("");
                if (initialItem != null) {
                    var option = $("<option/>").attr("value", initialItem.Value)
                                               .text(initialItem.Text);
                    $("#" + idControl).append(option);
                }
                if (data.ResponseData != null) {
                    for (var i = 0; i < data.ResponseData.Result[0].length; i++) {
                        var option = $("<option/>").attr("value", data.ResponseData.Result[0][i].Value)
                                                .attr("selected", data.ResponseData.Result[0][i].Selected)
                                                .text(data.ResponseData.Result[0][i].Text);
                        $("#" + idControl).append(option);
                    }

                    $("#" + idControl + " option[value='" + selectedValue + "']").attr('selected', 'selected');
                }
            },
            CheckAll: function (checked, prefix) {
                $("input[id*='" + prefix + "']").each(function () {
                    this.checked = checked;
                });
            },
            CheckAllByClass: function (checked, className) {
                $("input[class='" + className + "']").each(function () {
                    this.checked = checked;
                });
            },
            GetSelectedValuesAsArray: function (prefix) {
                var result = [];
                $("input[id*='" + prefix + "']").each(function () {
                    if ($(this).is(":checked"))
                        result.push($(this).val());
                });
                return result;
            },
            ShowModalWindow: function (content, title, attributes) {
                attributes = { maxHeight: 800, maxWidth: 600, autoResize: true };
                var idDiv = Math.ceil(Math.random() * 2)
                $("<div/>")
                       .attr("title", title)
                       .attr("id", idDiv)
                       .attr("class", "basic-modal-content")
                       .html(content)
                       .modal(attributes);
            },
            GetLatestIDFromWindowStack: function () {
                var value = $("#windowStack").val();
                if (value == "") {
                    return "";
                }
                else {
                    var list = value.split('|');
                    return list[list.length - 1];
                }
            },
            RemoveSpecificFromWindowStack: function (windowId) {
                var value = $("#windowStack").val();
                if (value.indexOf("|" + windowId) >= 0) {
                    $("#windowStack").val(value.replace("|" + windowId, ""));
                } else if (value.indexOf(windowId) >= 0) {
                    $("#windowStack").val(value.replace(windowId, ""));
                }
            },
            CloseLatestFromWindowStack: function () {
                if ($("#windowStack").val().length > 0) {
                    var list = $("#windowStack").val().split('|');
                    var latestWindowID = list[list.length - 1];
                    var element = $(".ui-dialog").has("#" + latestWindowID);
                    element.detach();
                    element.remove();
                    $(".ui-widget-overlay").detach();
                    $(".ui-widget-overlay").remove();
                    NBN.Helper.RemoveSpecificFromWindowStack(latestWindowID);
                }
            },
            CloseAllFromWindowStack: function () {
                $(".ui-dialog").detach();
                $(".ui-dialog").remove();
                $(".ui-widget-overlay").detach();
                $(".ui-widget-overlay").remove();
                $("#windowStack").val("");
            },
            CloseAll: function () {
                $(".ui-dialog-content").dialog("close");
            },
            OpenWindow: function (url, openWithIE) {
                if (openWithIE) {
                    baseUrl = (location.protocol + "//" + location.hostname + (location.port && ":" + location.port));
                    NBN.Helper.RunExe('C:\\Program Files\\Internet Explorer\\iexplore.exe', [baseUrl + url]);
                }
                else
                    window.open(url, name);
            },
            SetScrollingPosition: function () {
                $("#hscroll").val((document.all ? document.scrollLeft : window.pageXOffset));
                $("#vscroll").val((document.all ? document.scrollTop : window.pageYOffset));
            },
            PreserveScrollingPosition: function () {
                if ($("#hscroll").val() != "" && $("#vscroll").val() != "") {
                    window.scrollTo(eval($("#hscroll").val()), eval($("#vscroll").val()));
                }
            },
            ShowModalWindowJqueryUI: function (content, title, attributes, notShowCloseButton, allowCloseOnEscape) {
                var id = "messageBox" + Math.floor(Math.random() * 100);
                if (allowCloseOnEscape == null)
                    allowCloseOnEscape = false;

                var options =
                {
                    cache: false,
                    closeOnEscape: allowCloseOnEscape,
                    stack: true,
                    height: attributes.height,
                    width: attributes.width,
                    bgiframe: true,
                    modal: true,
                    resizable: true
                    //                    close: function (ev, ui) {
                    //                        NBN.Helper.PreserveScrollingPosition();
                    //                        NBN.Helper.RemoveSpecificFromWindowStack($(this).attr("id"));
                    //                        $(this).dialog("destroy");
                    //                        $(this).remove();
                    //                        $(this).detach();
                    //                    }
                }

                if ($("#windowStack").val().length == 0)
                    $("#windowStack").val(id);
                else
                    $("#windowStack").val($("#windowStack").val() + "|" + id);

                if (notShowCloseButton) {
                    options.resizable = false;
                    options.open = function (event, ui) { $(".ui-dialog-titlebar-close").hide(); };
                }
                $("<div/>")
                .attr("title", title)
                .attr("id", id)
                //.attr("class", "dialog")
                .html(content).dialog(options);
            },
            ShowLoadingWindow: function () {
                var html = "<div style=\"text-align:center\"><img src=\"/_images/loading.gif\"></div>";
                NBN.Helper.ShowModalWindowJqueryUI(html, "Carregando", { width: 81, heigth: 81 }, true);
            },
            ShowMessage: function (value, backUrl) {
                $("<div/>").html('<p><span class="ui-icon ui-icon-alert" style="float:left"></span>&nbsp;' + value + '</p>').dialog({
                    modal: true,
                    stack: true,
                    zIndex: 3999,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            if (backUrl != null)
                                location.href = backUrl;
                        }
                    }
                });
            },
            ShowSuccessMessage: function (action, backUrl) {
                NBN.Helper.ShowMessage("Registro " + action + " com sucesso.", backUrl);
            },
            ShowWarningMessage: function (text, backUrl) {
                NBN.Helper.ShowMessage(text, backUrl);
            },
            EnableContextMenu: function () {
                $('.context').contextMenu('menu', {
                    bindings: {
                        'repeat': function (t) {
                            var initialValue;
                            $("input", t).each(function (index, item) {
                                if (eval(index) == 0) {
                                    if ($(item).is("input:checkbox"))
                                        initialValue = $(item).is(":checked")
                                    else
                                        initialValue = $(item).val();
                                }
                                else {
                                    if ($(item).is("input:checkbox"))
                                        item.checked = initialValue;
                                    else
                                        $(item).val(initialValue);
                                }
                            })
                        },
                        'clear': function (t) {
                            $("input", t).each(function (index, item) {
                                if ($(item).is("input:checkbox"))
                                    item.checked = false;
                                else
                                    $(item).val("");
                            })
                        }
                    }
                });
            },
            EnableContextFilterMenu: function () {
                var div = $("<div/>").attr("class", "filter").html("&nbsp;")
                $(".grid thead tr th").first().append(div);
                $(".filter").contextMenu("selectMenu", { bindings: {} });
            },
            ApplyMask: function () {
                //aplicando as mascaras de edicao
                $.mask.options = {
                    attr: 'xmask', // an attr to look for the mask name or the mask itself
                    mask: null, // the mask to be used on the input
                    type: 'fixed', // the mask of this mask
                    maxLength: -1, // the maxLength of the mask
                    defaultValue: '', // the default value for this input
                    textAlign: true, // to use or not to use textAlign on the input
                    selectCharsOnFocus: true, //selects characters on focus of the input
                    setSize: false, // sets the input size based on the length of the mask (work with fixed and reverse masks only)
                    autoTab: true, // auto focus the next form element
                    fixedChars: '[(),.:/ -]', // fixed chars to be used on the masks.
                    onInvalid: function () { },
                    onValid: function () { },
                    onOverflow: function () { }
                };
                $('input:text').setMask();
            },
            SetDefaultIf: function (controlID, destinyControlID, value, label) {
                if ($(controlID).val() == value)
                    $(destinyControlID).val(label);
            },
            SetValueCheckBox: function (control, actionControl, checkedValue, unCheckedValue) {
                if (control.checked == true)
                    $("#" + actionControl).val(checkedValue);
                else
                    $("#" + actionControl).val(unCheckedValue);
            },
            TurnOnOffRelated: function (baseControl, controls, onChecked) {
                for (var i = 0; i < controls.length; i++) {
                    if (onChecked == null || onChecked == true) {
                        if ($(baseControl).is(":checked"))
                            $(controls[i].id).removeAttr("disabled");
                        else
                            $(controls[i].id).attr("disabled", "disabled");
                    }
                    else {
                        if (!$(baseControl).is(":checked"))
                            $(controls[i].id).removeAttr("disabled");
                        else
                            $(controls[i].id).attr("disabled", "disabled");
                    }
                }
            },
            AjaxPaging: function (_page, url, gridContainer) {
                url = url + ((url.indexOf('?') == 1) ? "?" : '&') + 'page=' + _page;
                //* -> não usar selector de corpo
                NBN.REST.Execute(url, {}, function (data) {
                    if (gridContainer.indexOf("*") > -1) {
                        $("#" + gridContainer.replace('*', '')).html(data);
                    }
                    else
                        $("#" + gridContainer).html($("#" + gridContainer, data).html());

                }, "GET");
            },
            CreateFooter: function (totalColuns, values) {
                var footer = "<tfoot><tr>";
                for (var i = 0; i < totalColuns; i++) {
                    var found = false;
                    for (var j = 0; j < values.length; j++) {
                        if (values[j].index == i) {
                            footer += "<td>" + values[j].content + "</td>";
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        footer += "<td>&nbsp;</td>";
                }
                footer += "</tr></tfoot>";
                return footer;
            },
            ApplyMaxLengthOnTextArea: function () {
                $('textarea[maxlength]').keyup(function () {
                    var limit = parseInt($(this).attr('maxlength'));
                    var text = $(this).val();
                    var chars = text.length;
                    if (chars > limit) {
                        var new_text = text.substr(0, limit);
                        $(this).val(new_text);
                    }
                });
            },
            CopyValuesFromFirst: function (total) {
                $("input[pindex='0']").each(function (indexBase, itemBase) {
                    for (var i = 1; i < total; i++) {
                        $("input[pindex='" + i + "']").each(function (indexChild, item) {
                            if (indexBase == indexChild) {
                                if ($(item).is("select"))
                                    $("#" + $(item).attr("id") + " option[value='" + $(itemBase).val() + "']").attr('selected', 'selected');
                                else
                                    $(item).val($(itemBase).val());
                            }
                        });
                    }
                });
            },
            CleanUpValues: function (total) {
                for (var i = 0; i < total; i++) {
                    $("input[pindex='" + i + "']").each(function (indexChild, item) {
                        $(item).val(0);
                    });
                }
            }
        },
    REST:
    {
        //Executa uma view remotamente - AJAX
        //view : caminho da view a ser executada
        //params: os parametros que podem ser passados à view
        //callback: metodo anonimo ou nao responsavel pelo retorno da view
        //ex: Helper.LoadContent("/REST/GetPosition", { filter: JSON.stringify(filter) }, function(data){xxx});
        Execute: function (view, params, callback, method) {
            $.ajax({
                dataType: "html", //Default: Intelligent Guess (text, xml, json, script, or html)
                type: method,
                url: view,
                data: params,
                error: function (jqXHR, textStatus, errorThrown) {
                    NBN.Helper.ShowModalWindowJqueryUI(jqXHR.responseText, jqXHR.status + ' - ' + jqXHR.statusText, { width: 800, heigth: 600 });
                },
                success: callback,
                error: function (xhr, status, err) {
                    $("<p>Error: Status = " + status + ", err = " + err + "</p>").appendTo(document.body);
                },
                traditional: true,
                async: false,
                cache: false
            });
        }
    }
}
