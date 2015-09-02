// Function for sticky footer

$(window).bind("load", function () {

    var footerHeight = 0,
        footerTop = 0,
        $footer = $("footer");

    positionFooter();

    function positionFooter() {

        footerHeight = $footer.height();
        footerTop = ($(window).scrollTop() + $(window).height() - footerHeight) + "px";

        if (($(document.body).height() + footerHeight) < $(window).height()) {
            $footer.css({
                position: "absolute"
            }).animate({
                top: footerTop
            }, 100,'linear' )
        } else {
            $footer.css({
                position: "static"
            })
        }

    }

    $(window)
            .scroll(positionFooter)
            .resize(positionFooter)

});

$(function () {
    $("#datepickerStart").datepicker({ dateFormat: 'dd-mm-yy' });
});
$(function () {
    $("#datepickerEnd").datepicker({ dateFormat: 'dd-mm-yy' });
});

$(function () {
    $("#tabs").tabs();
});