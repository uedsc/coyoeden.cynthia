var this$ = function () {
    var p = {}, pub = {};
    //custom private methods
    p.showSearches = function () {
        if (p._search_cat.is(":animated") || p._search_cat.is(":visible")) return;
        p._search_cat.slideDown();
        p._search_icon.removeClass("arrow-up").addClass("arrow-down");
    };
    p.hideSearches = function (evt) {
        if (p._search_cat.is(":hidden")) return;
        p._search_cat.slideUp();
        p._search_icon.removeClass("arrow-down").addClass("arrow-up");
    };
    p.toggleSearchC = function (flag) {
        $("#frmC input.sub-cat").each(function (i, e) {
            $(this).attr("checked", flag == 0 ? false : true).attr("disabled", flag == 0 ? false : true);
        });
    };
    p.onSubmitSearch = function () {
        if (p._txtSearch.val() === p._searchMask || p._txtSearch.val() === "") {
            p.showSearches(); return false;
        };
        return true;
    };
    p.initNav = function () {
        $("#navbar li.submenu").hover(function (evt) { $(this).find(".inner-boundary").show(); }, function (evt) { $(this).find(".inner-boundary").hide(); });
        $("#categories").hover(function (evt) { $(this).children("ul").show(); }, function (evt) { $(this).children("ul").hide(); });
    };
    p.initMenuBar = function () {
        var h = function () { $("#toolbar").fadeOut(); $("#toolbarbut").fadeIn("slow"); Set_Cookie('openstate', 'closed'); return false; };
        var s = function () { $("#toolbar").fadeIn(); $("#toolbarbut").fadeOut("slow"); Set_Cookie('openstate', 'open'); return false; };
        $("span.downarr a").click(h);
        $("span.showbar a").click(s);
        var openState = Get_Cookie('openstate');
        if (openState != null) { if (openState == 'closed') { h(); return; }; if (openState == 'open') { s(); return; }; };
    };

    //default private methods
    p.initVar = function (opts) {
        p._search_cat = $("#search_cat");
        p._search_icon = $("#search_icon");
        p._txtSearch = $("#txtSearch");
        p._searchMask = opts.searchMask;
    };
    p.onLoaded = function () {
        p._txtSearch.preInput({ val: p._searchMask, afterfocus: p.showSearches }).click(p.showSearches).mousedown(function (e) { e.stopPropagation(); return true; });
        p.initNav();
        p.initMenuBar();
    };
    p.initEvents = function (opts) {
        $(document).ready(p.onLoaded);
        $(".searchfrm").submit(p.onSubmitSearch);
        $("#cbxscall").click(function (evt) {
            if ($(this).is(":checked")) { p.toggleSearchC(1); } else { p.toggleSearchC(0); };
        });
        $("#home_slide").imgSlide({});
        //搜索浮层处理
        p._search_cat.mousedown(function () { return false; });
        //点击页面时隐藏搜索蒙层
        $("body").mousedown(function (e) {
            p.hideSearches();
        });

    };

    pub.Init = function (opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} ();