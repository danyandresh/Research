$(function () {
    var ajaxRequest = function () {
        var $form = $(this);

        var options = {
            url: $form.attr("action"),
            method: $form.attr("method"),
            data: $form.serialize()
        };

        $.ajax(options).done(function (data) {
            var $target = $($form.attr("data-dome-target"));

            var $highlightData = $(data);
            $target.replaceWith($highlightData);

            $highlightData.effect("highlight");
        });

        return false;
    };

    var submitAutocompleteForm = function (event, ui) {
        var $input = $(this);
        $input.val(ui.item.label);

        var $form = $input.parents("form:first");
        $form.submit();
    };

    var createAutocomplete = function () {
        var $input = $(this);

        var options = {
            source: $input.attr("data-dome-autocomplete"),
            select: submitAutocompleteForm
        };

        $input.autocomplete(options);
    }

    var getPage = function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            data: $("form").serialize(),
            type: "get"
        }

        $.ajax(options).done(function (data) {
            var target = $a.parents("div#pager").attr("data-domeapp-target");
            $(target).replaceWith(data);
        });

        return false;
    };

    $("form[data-dome-ajax='true']").submit(ajaxRequest);
    $("input[data-dome-autocomplete]").each(createAutocomplete);

    $("section#mainContent").on("click", "#pager a", getPage);

    $("form[data-dome-resultreplacestarget='true']").submit(ajaxRequest);
})