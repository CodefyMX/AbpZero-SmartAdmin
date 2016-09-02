(function () {
    $("body")
        .on("click",
            ".toggle",
            function () {
                var id = $(this).data("id");
                var selector = "." + id;
                $(selector).toggle();
            });
})();