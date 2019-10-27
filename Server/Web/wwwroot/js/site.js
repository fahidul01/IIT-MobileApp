$(document).ready(function () {
    //Show modals if present
    $('#modal-notification').modal('show');

    //Expand if needed
    var url = window.location;
    $('.navbar-nav').find('.active').removeClass('active');
    $('.navbar-nav li a').each(function () {
        if (this.href === url.href) {
            var parent = $(this).parent();
            parent.addClass('active');
            var ulList = parent.closest("ul");
            if (ulList.hasClass("collapse")) {
                ulList.addClass("show");
                ulList.siblings(':first-child').attr("aria-expanded", "true");
            }
        }
    });
    if ($(".mypicker").length !== 0) {
        $('.mypicker').selectpicker({
            liveSearch: true,
            showSubtext: true
        });
    }
});

