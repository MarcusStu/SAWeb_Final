// On load
$(window).load(function () {
    $("#info_rightdiv_skicka").hide();
    $("#info_table_right_btns_skicka").hide();
    document.getElementById("loader").style.display = "none"; //Tar bort laddnings animation när allt är laddat.
});

// Function to pop out infobox from info_btn
function deselect(e) {
    $('.pop').slideFadeToggle(function () {
        e.removeClass('selected');
    });
}


var toggled = true;
$(function () {

    $('#info_btn').on('click', function () {

        if (!toggled) {
            $("#infoclose").attr("src", "../../Content/img/Info.png");
            toggled = true;
        } else {
            $("#infoclose").attr("src", "../../Content/img/Info_Close_trans.png");
            toggled = false;
        }


        //if (!toggled) {
        //    $("#infoclose").attr("src", "../../Content/img/Info.png");
        //    $("#infoclose").attr("src", "../../Content/img/Info.png").removeClass("close_icon");
        //    toggled = true;
        //} else {
        //    $("#infoclose").attr("src", "../../Content/img/Info.png").addClass("close_icon");
        //    toggled = false;
        //}

        if ($(this).hasClass('selected')) {
            deselect($(this));
        } else {
            $(this).addClass('selected');
            $('.pop').slideFadeToggle();
        }

        return false;

    });

    $('.close').on('click', function () {
        deselect($('#info_btn'));
        return false;
    });


});

$.fn.slideFadeToggle = function (easing, callback) {
    return this.animate({ opacity: 'toggle', height: 'toggle' }, 'fast', easing, callback);
};



// -- Time, Date, Week scripts -- //

// Time Script
function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    h = checkTime(h);
    m = checkTime(m);
    s = checkTime(s);
    document.getElementById('clock').innerHTML =
    h + ":" + m + ":" + s;
    var t = setTimeout(startTime, 500);
}
function checkTime(i) {
    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    return i;
}

// Date Script
function startDate() {
    var today = new Date();
    var y = today.getFullYear();
    var m = today.getMonth() + 1;
    var d = today.getDate();
    y = checkDate(y);
    m = checkDate(m);
    d = checkDate(d);
    document.getElementById('date').innerHTML =
    y + "-" + m + "-" + d;
    var t = setTimeout(startDate, 500);
}
function checkDate(i) {
    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    return i;
}

// Week Script
Date.prototype.getWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    var today = new Date(this.getFullYear(), this.getMonth(), this.getDate());
    var dayOfYear = ((today - onejan + 1) / 86400000);
    return Math.ceil(dayOfYear / 7)
};

function startWeek() {
    $(function startWeek() {
        var today = new Date();
        var weekno = today.getWeek();
        $("#week").html("V." + weekno);
    });
    var t = setTimeout(startWeek, 500);
}


// -- Information -- //
// -- Clicked buttons scripts - //

//Information Right Buttons
$(document).ready(function () {
    $('#info_table_right_btns_skicka_administration').click(function () {
        $('#info_table_right_btns_skicka_produktion').removeClass('clicked_right').addClass('info_table_right_btns_skicka_produktion');
        $('#info_table_right_btns_skicka_administration').removeClass('info_table_right_btns_skicka_administration').addClass('clicked_right');
    });

    $('#info_table_right_btns_skicka_produktion').click(function () {
        $('#info_table_right_btns_skicka_administration').removeClass('clicked_right').addClass('info_table_right_btns_skicka_administration');
        $('#info_table_right_btns_skicka_produktion').removeClass('info_table_right_btns_skicka_produktion').addClass('clicked_right');
    });

    $('#info_table_right_btns_nya').click(function () {
        $('#info_table_right_btns_skickat').removeClass('clicked_right').addClass('info_table_right_btns_skickat');
        $('#info_table_right_btns_lasta').removeClass('clicked_right').addClass('info_table_right_btns_lasta');
        $('#info_table_right_btns_nya').removeClass('info_table_right_btns_nya').addClass('clicked_right');
    });

    $('#info_table_right_btns_lasta').click(function () {
        $('#info_table_right_btns_skickat').removeClass('clicked_right').addClass('info_table_right_btns_skickat');
        $('#info_table_right_btns_nya').removeClass('clicked_right').addClass('info_table_right_btns_nya');
        $('#info_table_right_btns_lasta').removeClass('info_table_right_btns_lasta').addClass('clicked_right');
    });

    $('#info_table_right_btns_skickat').click(function () {
        $('#info_table_right_btns_nya').removeClass('clicked_right').addClass('info_table_right_btns_nya');
        $('#info_table_right_btns_lasta').removeClass('clicked_right').addClass('info_table_right_btns_lasta');
        $('#info_table_right_btns_skickat').removeClass('info_table_right_btns_skickat').addClass('clicked_right');
    });
});

//Information Left Buttons
$(document).ready(function () {
    $('#info_leftdiv_btns_inkorg').click(function () {
        // Hide Skicka Div
        $("#info_rightdiv_skicka").hide();
        $("#info_table_right_btns_skicka").hide();
        // Show Right Div
        $("#info_rightdiv").show();
        $("#info_table_right_btns").show();
        $('#info_leftdiv_btns_inkorg').removeClass('info_leftdiv_btns_inkorg').addClass('clicked_left');
        $('#info_leftdiv_btns_skicka').removeClass('clicked_left').addClass('info_leftdiv_btns_skicka');
        $('#info_leftdiv_btns_tidbank').removeClass('clicked_left').addClass('info_leftdiv_btns_tidbank');
        $('#info_leftdiv_btns_senastestamp').removeClass('clicked_left').addClass('info_leftdiv_btns_senastestamp');
        $('#info_leftdiv_btns_tidrapport').removeClass('clicked_left').addClass('info_leftdiv_btns_tidrapport');
    });

    $('#info_leftdiv_btns_skicka').click(function () {
        // Hide right div
        $("#info_rightdiv").hide();
        $("#info_table_right_btns").hide();
        // Show Skicka Div
        $("#info_rightdiv_skicka").show();
        $("#info_table_right_btns_skicka").show();
        $('#info_leftdiv_btns_skicka').removeClass('info_leftdiv_btns_skicka').addClass('clicked_left');
        $('#info_leftdiv_btns_inkorg').removeClass('clicked_left').addClass('info_leftdiv_btns_inkorg');
        $('#info_leftdiv_btns_tidbank').removeClass('clicked_left').addClass('info_leftdiv_btns_tidbank');
        $('#info_leftdiv_btns_senastestamp').removeClass('clicked_left').addClass('info_leftdiv_btns_senastestamp');
        $('#info_leftdiv_btns_tidrapport').removeClass('clicked_left').addClass('info_leftdiv_btns_tidrapport');
    });

    $('#info_leftdiv_btns_tidbank').click(function () {
        // Hide Skicka Div
        $("#info_rightdiv_skicka").hide();
        $("#info_table_right_btns_skicka").hide();
        // Hides the right div when user press the "Tidbank button"
        $("#info_rightdiv").hide();
        $("#info_table_right_btns").hide();
        $('#info_leftdiv_btns_tidbank').removeClass('info_leftdiv_btns_tidbank').addClass('clicked_left');
        $('#info_leftdiv_btns_inkorg').removeClass('clicked_left').addClass('info_leftdiv_btns_inkorg');
        $('#info_leftdiv_btns_skicka').removeClass('clicked_left').addClass('info_leftdiv_btns_skicka');
        $('#info_leftdiv_btns_senastestamp').removeClass('clicked_left').addClass('info_leftdiv_btns_senastestamp');
        $('#info_leftdiv_btns_tidrapport').removeClass('clicked_left').addClass('info_leftdiv_btns_tidrapport');
    });

    $('#info_leftdiv_btns_senastestamp').click(function () {
        // Hide Skicka Div
        $("#info_rightdiv_skicka").hide();
        $("#info_table_right_btns_skicka").hide();
        // Hides the right div + it's relative btns when user press the "Tidbank button"
        $("#info_rightdiv").hide();
        $("#info_table_right_btns").hide();
        $('#info_leftdiv_btns_senastestamp').removeClass('info_leftdiv_btns_senastestamp').addClass('clicked_left');
        $('#info_leftdiv_btns_inkorg').removeClass('clicked_left').addClass('info_leftdiv_btns_inkorg');
        $('#info_leftdiv_btns_skicka').removeClass('clicked_left').addClass('info_leftdiv_btns_skicka');
        $('#info_leftdiv_btns_tidbank').removeClass('clicked_left').addClass('info_leftdiv_btns_tidbank');
        $('#info_leftdiv_btns_tidrapport').removeClass('clicked_left').addClass('info_leftdiv_btns_tidrapport');
    });

    $('#info_leftdiv_btns_tidrapport').click(function () {
        // Hide Skicka Div
        $("#info_rightdiv_skicka").hide();
        $("#info_table_right_btns_skicka").hide();
        // Show Right Div
        $("#info_rightdiv").show();
        $("#info_table_right_btns").show();
        $('#info_leftdiv_btns_tidrapport').removeClass('info_leftdiv_btns_tidrapport').addClass('clicked_left');
        $('#info_leftdiv_btns_inkorg').removeClass('clicked_left').addClass('info_leftdiv_btns_inkorg');
        $('#info_leftdiv_btns_skicka').removeClass('clicked_left').addClass('info_leftdiv_btns_skicka');
        $('#info_leftdiv_btns_senastestamp').removeClass('clicked_left').addClass('info_leftdiv_btns_senastestamp');
        $('#info_leftdiv_btns_tidbank').removeClass('clicked_left').addClass('info_leftdiv_btns_tidbank');
    });
});


//Allow only numeric input in inputfields
//Author: Joshua De Leon - File: numericInput.js - Description: Allows only numeric input in an element. 
//- If you happen upon this code, enjoy it, learn from it, and if possible please credit me: www.transtatic.com
(function (b) {
    var c = { allowFloat: false, allowNegative: false }; b.fn.numericInput = function (e) {
        var f = b.extend({}, c, e); var d = f.allowFloat;
        var g = f.allowNegative; this.keypress(function (j) {
            var i = j.which; var h = b(this).val();
            if (i > 0 && (i < 48 || i > 57)) {
                if (d == true && i == 46) {
                    if (g == true && a(this) == 0 && h.charAt(0) == "-") { return false }
                    if (h.match(/[.]/)) { return false }
                } else {
                    if (g == true && i == 45) { if (h.charAt(0) == "-") { return false } if (a(this) != 0) { return false } }
                    else { if (i == 8) { return true } else { return false } }
                }
            } else { if (i > 0 && (i >= 48 && i <= 57)) { if (g == true && h.charAt(0) == "-" && a(this) == 0) { return false } } }
        }); return this
    }; function a(d) {
        if (d.selectionStart) { return d.selectionStart } else {
            if (document.selection) {
                d.focus(); var f = document.selection.createRange();
                if (f == null) { return 0 } var e = d.createTextRange(), g = e.duplicate(); e.moveToBookmark(f.getBookmark()); g.setEndPoint("EndToStart", e);
                return g.text.length
            }
        } return 0
    }
}(jQuery));

$(function () {
    $("#info_senastestamp_search_order").numericInput({ allowFloat: true, allowNegative: true });
});


// Allow only numeric input in inputs
//$(document).ready(function () {
//    $("#info_senastestamp_search_order").keydown(function (e) {
//        // Allow: backspace, delete, tab, escape, enter and .
//        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
//            // Allow: Ctrl+A
//            (e.keyCode == 65 && e.ctrlKey === true) ||
//            // Allow: Ctrl+C
//            (e.keyCode == 67 && e.ctrlKey === true) ||
//            // Allow: Ctrl+X
//            (e.keyCode == 88 && e.ctrlKey === true) ||
//            // Allow: home, end, left, right
//            (e.keyCode >= 35 && e.keyCode <= 39)) {
//            // let it happen, don't do anything
//            return;
//        }
//        // Ensure that it is a number and stop the keypress
//        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
//            e.preventDefault();
//        }
//    });
//});
//$('tableFunction'(function ($scope) {}));
//$('tableRows').on('click', function () { rows.on('click', function (e) {ya-di-ya-da })

// Table row select-highlighter
//$(function () {

//    /* Get all rows from your 'table' but not the first one 
//     * that includes headers. */
//    var rows = $('tr').not(':first');

//    // Rows hover function
//    rows.on('mouseenter', function (e) {
//        /* Get current row */
//        var row = $(this);

//        if ($(this).hasClass('row_highlighted')) {
//            $(this).removeClass('row_hover');
//        }
//        else {
//            /* Highlight one row */
//            rows.removeClass('row_hover');
//            row.addClass('row_hover');
//        }

//    });

//    // Rows mouseleave function
//    rows.on('mouseleave', function (e) {
//        /* Get current row */
//        var row = $(this);

//        //if ($(this).hasClass('row_highlighted')) {
//        //    $(this).removeClass('row_highlighted');
//        //}
//        if ($(this).hasClass('row_hover')) {
//            $(this).removeClass('row_hover');
//        }

//    });

//    // Rows click function
//    rows.on('click', function (e) {

//        /* Get current row */
//        var row = $(this);

//        /* Highlight one row */
//        rows.removeClass('row_highlighted');
//        row.addClass('row_highlighted');

//    });

//    /* This 'event' is used just to avoid that the table text 
//     * gets selected (just for styling). 
//     * For example, when pressing 'Shift' keyboard key and clicking 
//     * (without this 'event') the text of the 'table' will be selected.
//     * You can remove it if you want, I just tested this in 
//     * Chrome v30.0.1599.69 */
//    $(document).bind('selectstart dragstart', function (e) {
//        e.preventDefault(); return false;
//    });

//});


function inputFilter() {
    // Declare variables 
    var input, filter, table, tr, td, i;
    // Check which input has focus
    //if ($("#info_senastestamp_search_order").is(":focus")) {
    //    input = document.getElementById("info_senastestamp_search_order");
    //}
    //if ($("#info_senastestamp_search_resurs").is(":focus")) {
    //    input = document.getElementById("info_senastestamp_search_resurs");
    //}
    //if ($("#info_senastestamp_search_operation").is(":focus")) {
    //    input = document.getElementById("info_senastestamp_search_operation");
    //}
    //if ($("#info_senastestamp_search_start").is(":focus")) {
    //    input = document.getElementById("info_senastestamp_search_start");
    //}
    //if ($("#info_senastestamp_search_stop").is(":focus")) {
    //    input = document.getElementById("info_senastestamp_search_stop");
    //}
    //if ($("#info_senastestamp_search_total").is(":focus")) {
    //    input = document.getElementById("info_senastestamp_search_total");
    //}
    input = document.getElementById("info_senastestamp_search_order");
    filter = input.value.toUpperCase();
    table = document.getElementById("table_left_div");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }

    // When the clear field link is clicked do the following:
    $("#info_resetbtn").on("click", function () {

        clearField();

        return false;

    });

    // The clear field function which clears the search field
    function clearField() {
        $(".filterInput").val('');

        inputFilter();
    }
}

//var swiper = new Swiper('.swiper-container', {
//    pagination: '.swiper-pagination',
//    paginationClickable: true,
//    paginationBulletRender: function (swiper, index, className) {
//        return '<span class="' + className + '">' + (index + 1) + '</span>';
//    }
//});



//Angular
var app = angular.module('SystemAndersson', ["ngAnimate"]);

app.controller('MainCtrl', function ($scope, $http) {
    //PartialView kallelse-funktioner.
    $scope.PartialViews = [true];

    

    //Kallar in information från partials.

    //Kallar fram 'UteOperationer'
    $scope.loopUteOperationerOrderHead = function () {

        $http({
            method: 'GET',
            url: 'Home/../UteOperationer'
        }).then(function (success){

            //Call get uteOperationer funktion.
            $scope.UteOperationer = success.data.loopUteOperationerOrderHead
                
            console.log(success);
            for (var i = 0; i < $scope.UteOperationer.length; i++) {
                //När man använder sig av datum, tar Json & AngularJs in all data, även det som inte ska vara med.
                //Med koden under kan vi bli av med texten som inte behövs.
                $scope.UteOperationer[i].OrderDate = new Date(parseInt($scope.UteOperationer[i].OrderDate.replace("/Date(", "").replace(")/", ""), 10));
                    
            }
            console.log(Date);
        },function (error){
            console.log(error.data);
        });
    }
    $scope.UteOperationer = []; //Den kallade informationen hamnar i dessa, som används till tabellerna.

    $scope.loopUteTillverkningsorderOrderHead = function () {

        $http({
            method: 'GET',
            url: 'Home/../UteTillverkningsorder'
        }).then(function (success) {

            //Call get uteTillverkningsorder funktion.
            $scope.UteTillverkningsorder = success.data.loopUteTillverkningsorderOrderHead

            console.log(success);
            for (var i = 0; i < $scope.UteTillverkningsorder.length; i++) {
                //När man använder sig av datum, tar Json & AngularJs in all data, även det som inte ska vara med.
                //Med koden under kan vi bli av med texten som inte behövs.
                $scope.UteTillverkningsorder[i].ReadyDate = new Date(parseInt($scope.UteTillverkningsorder[i].ReadyDate.replace("/Date(", "").replace(")/", ""), 10));
                $scope.UteTillverkningsorder[i].DeliveryDate = new Date(parseInt($scope.UteTillverkningsorder[i].DeliveryDate.replace("/Date(", "").replace(")/", ""), 10));

            }
            console.log(Date);
        }, function (error) {
            console.log(error.data);
        });
    }
    $scope.UteTillverkningsorder = [];

    //Kallar fram 'UteOrderPerson'
    $scope.loopUteOrderPersonOrderHead = function () {

        $http({
            method: 'GET',
            url: 'Home/../UteOrderPerson'
        }).then(function (success) {

            //Call get uteOrderPerson funktion.
            $scope.UteOrderPerson = success.data.loopUteOrderPersonOrderHead

            console.log(success);
            for (var i = 0; i < $scope.UteOrderPerson.length; i++) {
            }
            console.log(Date);
        }, function (error) {
            console.log(error.data);
        });
    }
    $scope.UteOrderPerson = [];

    //Kallar fram 'UteSnabborder'
    $scope.loopUteSnabborderOrderHead = function () {

        $http({
            method: 'GET',
            url: 'Home/../UteSnabborder'
        }).then(function (success) {

            //Call get uteOrderPerson funktion.
            $scope.UteSnabborder = success.data.loopUteSnabborderOrderHead

            console.log(success);
            for (var i = 0; i < $scope.UteSnabborder.length; i++) {
            }
            console.log(Date);
        }, function (error) {
            console.log(error.data);
        });
    }
    $scope.UteSnabborder = [];

    //Kallar fram 'UteImproduktiva'
    $scope.loopUteImproduktivaOrderList = function () {

        $http({
            method: 'GET',
            url: 'Home/../UteImproduktiva'
        }).then(function (success) {

            //Call get uteImproduktiva funktion.
            $scope.UteImproduktiva = success.data.loopUteImproduktivaOrderList

            console.log(success);
            for (var i = 0; i < $scope.UteImproduktiva.length; i++) {
            }

        }, function (error) {
            console.log(error.data);
        });
    }
    $scope.UteImproduktiva = [];

    //Kallar fram 'Material'
    $scope.loopMaterialOrderHead = function () {

        $http({
            method: 'GET',
            url: 'Home/../Material'
        }).then(function (success) {

            //Call get Material funktion.
            $scope.Material = success.data.loopMaterialOrderHead

            console.log(success);
            for (var i = 0; i < $scope.Material.length; i++) {
            }

        }, function (error) {
            console.log(error.data);
        });
    }
    $scope.Material = [];


    //Sortering
    $scope.sortType = ''; //Sätter 'default' sorterings typ.
    $scope.sortReverse = false;  //Sätter 'default' sorterings order.
    $scope.searchColumn = '';     //Sätter 'default' column för sortering.

    //Sök funktion reset(null).
    $scope.search = { searchResultClear: '' };
    $scope.clearSearch = function () {

        $scope.search = { searchResultClear: '' };
    };


    

    //Funktioner till knappar för Divs med tabeller.
    //Nummret visar vilken man ska använda för att kalla fram just den i en .cshtml.
    //För alla Stamping Partials:
    $scope.stampingKnappar =
        //NULL
   /*0*/[{ name: '', url: '' },
        //Inne-Partials                                                                 
   /*1*/{ name: 'InneInfo', url: 'Home/../InneInfoPartial' },
   /*2*/{ name: 'InneDokument', url: 'Home/../InneDokumentPartial' },
   /*3*/{ name: 'InneKommentar', url: 'Home/../InneKommentarPartial' },
   /*4*/{ name: 'InneArtiklar', url: 'Home/../InneArtiklarPartial' },
   /*5*/{ name: 'InneAvvikelse', url: 'Home/../InneAvvikelsePartial' },
        //Ute-Partials                                                                  
   /*6*/{ name: 'UteInfo', url: 'Home/../UteInfoPartial' },
   /*7*/{ name: 'UteOperationer', url: 'Home/../UteOperationerPartial' },
   /*8*/{ name: 'UteTillverkningsorder', url: 'Home/../UteTillverkningsorderPartial' },
   /*9*/{ name: 'UteOrderPerson', url: 'Home/../UteOrderPersonPartial' },
  /*10*/{ name: 'UteSnabborder', url: 'Home/../UteSnabborderPartial' },
  /*11*/{ name: 'UteImproduktiva', url: 'Home/../UteImproduktivaPartial' },
  /*12*/{ name: 'UteInstruktioner', url: 'Home/../UteInstruktionerPartial' },
  /*13*/{ name: 'UteDokument', url: 'Home/../Dokument' }];
    $scope.stampingKnapp = $scope.stampingKnappar[0];

    //För alla Information Partials:
    $scope.informationKnappar =
        //NULL
   /*0*/[{ name: '', url: '' },
        //Vänster-Partials
   /*1*/{ name: 'Inkorg', url: 'Home/../Inkorg' },
   /*2*/{ name: 'Skicka', url: 'Home/../Skicka' },
   /*3*/{ name: 'Tidbank', url: 'Home/../Tidbank' },
   /*4*/{ name: 'Senastestamp', url: 'Home/../Senastestamp'},
   /*5*/{ name: 'Skicka_Administration', url: 'Home/../Skicka_Administration'},
   /*6*/{ name: 'Skicka_Produktion', url: 'Home/../Skicka-Produktion' },
        //Höger-Partials
   /*7*/{ name: 'Nya', url: 'Home/../Nya' },
   /*8*/{ name: 'Lasta', url: 'Home/../Lasta' },
   /*9*/{ name: 'Skickat', url: 'Home/../Skickat' }]
  /*..*/
  /*..*/
  /*..*/
     $scope.informationKnapp = $scope.informationKnappar[0];

     $scope.jobbMaterialKnappar =
        //NULL
   /*0*/[{ name: '', url: '' },
   /*1*/{ name: 'Inkorg', url: 'Home/../JobbPartial' },
   /*2*/{ name: 'Skicka', url: 'Home/../MaterialPartial' }]
     $scope.jobbMaterialKnapp = $scope.jobbMaterialKnappar[0];



    //Tabell information.
    //Tabell: Stamping - Ute
    //$scope.operationer = [{ Order: 'INT', Benamning: 'STRING', Resurs: 'INT', ResursBen: 'STRING', Op: 'INT', OpBen: 'STRING', Prio: 'INT', Startdatum: 'DATE', Artikel: 'INT', Kund: 'STRING' },
    //                       { Order: 'HOW INT', Benamning: 'HOW STRING', Resurs: 'HOW INT', ResursBen: 'HOW STRING', Op: 'HOW INT', OpBen: 'HOW STRING', Prio: 'HOW INT', Startdatum: 'HOW DATE', Artikel: 'HOW INT', Kund: 'HOW STRING' },
    //                       { Order: 'MAYBE INT', Benamning: 'MAYBE STRING', Resurs: 'MAYBE INT', ResursBen: 'MAYBE STRING', Op: 'MAYBE INT', OpBen: 'MAYBE STRING', Prio: 'MAYBE INT', Startdatum: 'MAYBE DATE', Artikel: 'MAYBE INT', Kund: 'MAYBE STRING' },
    //                       { Order: 'NOT INT', Benamning: 'NOT STRING', Resurs: 'NOT INT', ResursBen: 'NOT STRING', Op: 'NOT INT', OpBen: 'NOT STRING', Prio: 'NOT INT', Startdatum: 'NOT DATE', Artikel: 'NOT INT', Kund: 'NOT STRING' },
    //{ Order: 'INT', Benamning: 'STRING', Resurs: 'INT', ResursBen: 'STRING', Op: 'INT', OpBen: 'STRING', Prio: 'INT', Startdatum: 'DATE', Artikel: 'INT', Kund: 'STRING' },
    //                       { Order: 'HOW INT', Benamning: 'HOW STRING', Resurs: 'HOW INT', ResursBen: 'HOW STRING', Op: 'HOW INT', OpBen: 'HOW STRING', Prio: 'HOW INT', Startdatum: 'HOW DATE', Artikel: 'HOW INT', Kund: 'HOW STRING' },
    //                       { Order: 'MAYBE INT', Benamning: 'MAYBE STRING', Resurs: 'MAYBE INT', ResursBen: 'MAYBE STRING', Op: 'MAYBE INT', OpBen: 'MAYBE STRING', Prio: 'MAYBE INT', Startdatum: 'MAYBE DATE', Artikel: 'MAYBE INT', Kund: 'MAYBE STRING' },
    //                       { Order: 'NOT INT', Benamning: 'NOT STRING', Resurs: 'NOT INT', ResursBen: 'NOT STRING', Op: 'NOT INT', OpBen: 'NOT STRING', Prio: 'NOT INT', Startdatum: 'NOT DATE', Artikel: 'NOT INT', Kund: 'NOT STRING' },
    //{ Order: 'INT', Benamning: 'STRING', Resurs: 'INT', ResursBen: 'STRING', Op: 'INT', OpBen: 'STRING', Prio: 'INT', Startdatum: 'DATE', Artikel: 'INT', Kund: 'STRING' },
    //                       { Order: 'HOW INT', Benamning: 'HOW STRING', Resurs: 'HOW INT', ResursBen: 'HOW STRING', Op: 'HOW INT', OpBen: 'HOW STRING', Prio: 'HOW INT', Startdatum: 'HOW DATE', Artikel: 'HOW INT', Kund: 'HOW STRING' },
    //                       { Order: 'MAYBE INT', Benamning: 'MAYBE STRING', Resurs: 'MAYBE INT', ResursBen: 'MAYBE STRING', Op: 'MAYBE INT', OpBen: 'MAYBE STRING', Prio: 'MAYBE INT', Startdatum: 'MAYBE DATE', Artikel: 'MAYBE INT', Kund: 'MAYBE STRING' },
    //                       { Order: 'NOT INT', Benamning: 'NOT STRING', Resurs: 'NOT INT', ResursBen: 'NOT STRING', Op: 'NOT INT', OpBen: 'NOT STRING', Prio: 'NOT INT', Startdatum: 'NOT DATE', Artikel: 'NOT INT', Kund: 'NOT STRING' }]

    $scope.tillverkningsOrder = [{ Order: 'INT', Prio: 'INT', Benamning: 'STRING', Kund: 'STRING', KundOrd: 'INT', Artikel: 'INT', Start: 'STRING', Startdatum: 'DATE', LevDatum: 'DATE', Antal: 'INT', Ritning: 'INT', RitningRev: 'STRING' },
                           { Order: 'HOW INT', Prio: 'HOW INT', Benamning: 'HOW STRING', Kund: 'HOW STRING', KundOrd: 'HOW INT', Artikel: 'HOW INT', Start: 'HOW DATE', Startdatum: 'HOW DATE', LevDatum: 'HOW DATE', Antal: 'HOW INT', Ritning: 'HOW INT', RitningRev: 'HOW STRING' },
                           { Order: 'NOT INT', Prio: 'NOT INT', Benamning: 'NOT STRING', Kund: 'NOT STRING', KundOrd: 'NOT INT', Artikel: 'NOT INT', Start: 'NOT DATE', Startdatum: 'NOT DATE', LevDatum: 'NOT DATE', Antal: 'NOT INT', Ritning: 'NOT INT', RitningRev: 'NOT STRING' }]

    // Table row select-highlighter
$scope.tableRowFunction = function () {

    /* Get all rows from your 'table' but not the first one 
     * that includes headers. */
    var rows = $('tr').not(':first');

    // Rows hover function
    rows.on('mouseenter', function (e) {
        /* Get current row */
        var row = $(this);

        if ($(this).hasClass('row_highlighted')) {
            $(this).removeClass('row_hover');
        }
        else {
            /* Highlight one row */
            rows.removeClass('row_hover');
            row.addClass('row_hover');
        }

    });

    // Rows mouseleave function
    rows.on('mouseleave', function (e) {
        /* Get current row */
        var row = $(this);

        //if ($(this).hasClass('row_highlighted')) {
        //    $(this).removeClass('row_highlighted');
        //}
        if ($(this).hasClass('row_hover')) {
            $(this).removeClass('row_hover');
        }

    });

    // Rows click function
    rows.on('click', function (e) {

        /* Get current row */
        var row = $(this);

        /* Highlight one row */
        rows.removeClass('row_highlighted');
        row.addClass('row_highlighted');

    });

    /* This 'event' is used just to avoid that the table text 
     * gets selected (just for styling). 
     * For example, when pressing 'Shift' keyboard key and clicking 
     * (without this 'event') the text of the 'table' will be selected.
     * You can remove it if you want, I just tested this in 
     * Chrome v30.0.1599.69 */
    $(document).bind('selectstart dragstart', function (e) {
        e.preventDefault(); return false;
    });

};


});