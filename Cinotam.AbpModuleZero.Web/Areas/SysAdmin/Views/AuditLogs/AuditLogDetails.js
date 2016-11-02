(function () {

    $(document)
        .ready(function () {
            var $body = $("body");

            $body
       .on("click",
           ".toggle",
           function () {


               var id = $(this).data("id");
               
               var selector = "." + id;



               console.log("Click",selector);

               $(selector).toggle();
           });

        });


})();