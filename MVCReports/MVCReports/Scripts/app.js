// Function for sticky footer

//$(window).bind("load", function () {

//    var footerHeight = 0,
//        footerTop = 0,
//        $footer = $("footer");

//    positionFooter();

//    function positionFooter() {

//        footerHeight = $footer.height();
//        footerTop = ($(window).scrollTop() + $(window).height() - footerHeight) + "px";

//        if (($(document.body).height() + footerHeight) < $(window).height()) {
//            $footer.css({
//                position: "absolute"
//            }).animate({
//                top: footerTop
//            }, 100,'linear' )
//        } else {
//            $footer.css({
//                position: "static"
//            })
//        }

//    }

//    $(window)
//            .scroll(positionFooter)
//            .resize(positionFooter)

//});

// Functions for sticky footer

$(window).bind("load", function () {
    var contentHeight = $(window).height();
    var footerHeight = $('footer').height();
    var footerTop = $('footer').position().top + footerHeight;
    if (footerTop < contentHeight) {
        $('footer').css('margin-top',  (contentHeight - footerTop) + 'px');
    }
});

$(window).bind('resize', function () {
    $(window).resize(function () {
        var contentHeight = $(window).height();
        var footerHeight = $('footer').height();
        var footerTop = $('footer').position().top + footerHeight;
        if (footerTop < contentHeight) {
            $('footer').css('margin-top', (contentHeight - footerTop) + 'px');
        }
    });
    
});


// Date picker & settings

$(function () {
    $("#datepickerStart").datepicker({ dateFormat: 'dd-mm-yy' }).val();
});

$(function () {
    $("#datepickerEnd").datepicker({ dateFormat: 'dd-mm-yy' }).val();
});

