<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.legislationSearch" CodeFile="legislationSearch.aspx.vb"  MaintainScrollPositionOnPostback="true"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>Legislation | eLaw.my</title>
<link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="GUI/css/jquery.ui.tabs.css" rel="stylesheet" type="text/css" />
    <link href="css/cass.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/style_New.css" />
    <script src="js/libs/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="include/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="include/jquery.ui.tabs.js" type="text/javascript"></script>

    <script src="include/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <link href="css/AutoComplate.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">

function resetddl(){
    var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
       lstddlrbs.options[lstddlrbs.selectedIndex].value=1;
        
    var lstddlproxmity = document.getElementById('<%=ddlproxmity.ClientID%>');
       lstddlproxmity.options[lstddlproxmity.selectedIndex].value=3;
        
        var lstddlprox = document.getElementById('<%=ddlprox.ClientID%>');
       lstddlprox.options[lstddlprox.selectedIndex].value="";
       lstddlprox.options[lstddlprox.selectedIndex].text="";
       }//end of function reset drop down list to their 

   //abdo work end
       function CheckSubjectIndex() {
           
           //abdo work start
           resetddl(); //reset drop down list srch options


           var ddlvalue = "";
           var ddltext = "";
           var lstddlsrch = document.getElementById('<%=ddlsrch.ClientID%>');
           ddlvalue = lstddlsrch.options[lstddlsrch.selectedIndex].value;
           ddltext = lstddlsrch.options[lstddlsrch.selectedIndex].text;

           ddltext = ddltext.toLowerCase();

          

           if (ddltext.indexOf("legislation") > -1) {
               var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
               lstddlrbs.options[lstddlrbs.selectedIndex].value = 1;
               // alert("you choose judment");
           }
               else
               if (ddltext.indexOf("act") > -1) {

                   var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
                   lstddlrbs.options[lstddlrbs.selectedIndex].value = 2;
                  
               }
               else
                   if (ddltext.indexOf("section") > -1) {

                       var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
                       lstddlrbs.options[lstddlrbs.selectedIndex].value = 3;
                       //alert("you choose case title");
                   }
               else
                   if (ddltext.indexOf("word") > -1) {
                       var lstddlprox = document.getElementById('<%=ddlprox.ClientID%>');
                       lstddlprox.options[lstddlprox.selectedIndex].value = ddlvalue;
                       lstddlprox.options[lstddlprox.selectedIndex].text = ddltext;

                       //alert("you choose within words");
                   }

                   else
                       if (ddltext.indexOf("same") > -1) {

                           var lstddlproxmity = document.getElementById('<%=ddlproxmity.ClientID%>');
                           lstddlproxmity.options[lstddlproxmity.selectedIndex].value = ddlvalue;

                           //   alert("you choose same ");

                       }

                      // alert("check before submit");
       }//end of javascritp funtion to update            //abdo work end

       

    $(document).ready(function () {

        //////////////////////////////////////////////Exact Phrase
        $('#txtFTS').on('keypress', function () {
            var auto = $("#<%=txtFTS.ClientID%>")[0].value;
            if (auto.length >= 2) {
                $.ajax({
                    type: "POST",
                    url: "legislationSearch.aspx/Getautocomplate",
                    data: '{auto: "' + $("#<%=txtFTS.ClientID%>")[0].value + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        
                        $("#txtFTS").autocomplete({

                            source: data.d
                        });
                    },
                    failure: function (response) {
                    }
                });
            }
        });
        //////////////////////////////////////////////
        //////////////////////////////////////////////Act
        $('#txtActNo').on('keyup', function () {
            var auto = $("#<%=txtActNo.ClientID%>")[0].value;

            $.ajax({
                type: "POST",
                url: "legislationSearch.aspx/GetautocomplateAct",
                data: '{auto: "' + $("#<%=txtActNo.ClientID%>")[0].value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    
                    $("#txtActNo").autocomplete({
                        //                        source: function (req, responseFn) {
                        //                            var re = $.ui.autocomplete.escapeRegex(req.term);
                        //                            var matcher = new RegExp("^" + re, "i");
                        //                            var a = $.grep(data.d, function (item, index) {
                        //                                return matcher.test(item);
                        //                            });
                        //                            responseFn(a);
                        //                        }
                        source: data.d
                    });
                },
                failure: function (response) {
                }
            });
        });
        //////////////////////////////////////////////



        $(".search").click(function () {
            if ($(".scrollBoxWrapper2").is(":hidden")) {
                $('.arrow2').html('▼');
            }
            else {
                $('.arrow2').html('▲');

            }
            $(".scrollBoxWrapper2").slideToggle("slow");

        });
        $(".hide_search_cri").click(function () {
            $(".search_cri").slideToggle("slow");
        });


        //        $("#tabcss0").addClass("current1");
        //        $('#tabsBox').children('li').hover(function(){

        //            $(this).addClass("current1");
        //        },function(){
        //            $(this).removeClass("current1");
        //        });

        $("#tabs").tabs();

        $('#tabs').bind('tabsselect', function (event, ui) {

            var selectedTab = ui.index;
            $("#<%= hidLastTab.ClientID %>").val(selectedTab);
            $(".tabsBox li").removeClass("current1");
            $("#tabcss" + selectedTab).addClass("current1");


        });

    });
    
 
   
   $("#ddl_year").change(function() {

            var load_year = $('#ddl_year option:selected').html();
            
               var year_link = "LegislationBrowseResult.aspx?Y=" + load_year + "&tp=sa&tab=3"
               $("#loadactbyyear").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            
            $("#loadactbyyear").load(year_link);
            
            //            });
            
                  

//            $.ajax({
//                type: "POST",
//                url:  "LegislationBrowseResult.aspx?Y=" + load_year + "&tp=sa",
//                data: load_year,
//                success: function(msg) {
//                alert(msg);
//                   // $('#loadactbyyear').html(msg);

//                }
//            });
//            return false;
//   
        });

        function loadResults(p) { /////////////////////////////////////////////////////////////// 64
            $currentPage = p
            $("#legislation_tab").html("<br/><center>Legislation Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            $.ajax({
                type: "POST",
                //url: "LegislationBrowseResults.aspx/getResult",
                url: "legislationSearch.aspx/getResult",
                data: '{type: "' + $('[name="browseType"]').val() + '", range: "' + $('#actRange').val() + '", subj: "' + $('#ddlSubject').children(':selected').text().replace(' ', '_') + '", title: "' + $('#alphabet').val() + '",page: "' + $currentPage + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#legislation_tab").hide();

                    $("#legislation_tab").html("");
                    $("#legislation_tab").css('background-color', 'white');
                    
                    if (data.d.length < 2)
                        $("#legislation_tab").html("<div class='topBar2'><center>Sorry No Record Found in our Library!</center></div>");

                    $.each(data.d, function (dd, ddd) {
                        if (dd == 0) {

                            $maxPage = Math.ceil(parseFloat(ddd) / 10.0);
                            $minPage = (parseInt($currentPage / 10)) * 10;
                            //if ($minPage == 0) $minPage = 1;
                            $('#lblBottomNavigation').html("");
                            if ($currentPage >= 10) {
                                $('#lblBottomNavigation').html($('#lblBottomNavigation').html() + " <a style='cursor:pointer;padding:3px;' onclick='loadResults(" + 1 + ")'><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a>");
                                $('#lblBottomNavigation').html($('#lblBottomNavigation').html() + " <a style='cursor:pointer;padding:3px;' onclick='loadResults(" + ($currentPage - 1) + ")'><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a>");
                            }
                            for (var i = $minPage; i <= $minPage + 10 && i <= $maxPage; i++) {
                                if (i != 0)
                                    if (i == $currentPage)
                                        $('#lblBottomNavigation').html($('#lblBottomNavigation').html() + '<a style="cursor:pointer" onclick="loadResults(' + i + ')" class="currentPage" value="' + i + '">' + i + '</a>');
                                    else
                                        $('#lblBottomNavigation').html($('#lblBottomNavigation').html() + '<a style="cursor:pointer;padding:3px;" onclick="loadResults(' + i + ')" value="' + i + '"> ' + i + ' </a>');
                            }
                            if ($minPage + 10 < $maxPage) {
                                $('#lblBottomNavigation').html($('#lblBottomNavigation').html() + " <a style='cursor:pointer;padding:3px;' onclick='loadResults(" + ($currentPage + 1) + ")'><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>");
                                $('#lblBottomNavigation').html($('#lblBottomNavigation').html() + " <a style='cursor:pointer;padding:3px;' onclick='loadResults(" + $maxPage + ")'><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>");
                            }
                            $('.wrapperAddfooter').slideDown();
                        } else {

                            var x = "";
                            x += "<div class='boxResult'><div class='boxCheck1' id ='divPreamle" + dd + "' onclick='show_legis_preamble(" + dd + ")'><img src='img/arrowEx1.png'/></div><div class='resultTitleCase' style='width:90%;'><p class='tTitleCase'>&nbsp;<a href='LegislationMainDisplayed.aspx?info=" + ddd[0] + "' style='color: #00648E'>" + ddd[1] + "</a></p>";
                            x += "<div class='title2Case'  id ='ShowPreamle" + dd + "' style=' display:none; '> Date of Preamble:" + ddd[2] + "</div>";
                            x += "<p class='title2Case'><span style='color:#f47200;font-size:14px;font-weight:700'>&nbsp;" + ddd[3] + "&nbsp;Subject;&nbsp;" + ddd[4] + "</span></p>";
                            x += "<p class='tItalic'>&nbsp;" + ddd[5] + "</p></div><div class='clear'></div></div>";
                            $("#legislation_tab").html($("#legislation_tab").html() + x);
                        }
                    });


                    $("#legislation_tab").slideDown("slow");
                    //$(".div_refresh").show();
                },
                failure: function (response) {
                }
            });
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////// ----

        $(document).ready(function () {
            $('.wrapperAddfooter').hide();
            $("#ddl_role").change(function () {
                $("#loadactbyyear").html();
                var load_role = $('#ddl_role option:selected').val();

                var role_link = "LegislationBrowseResult.aspx?tp=" + load_role;

                $("#loadactbyyear").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
                $("#loadactbyyear").load(role_link);


            });
            $("#ddl_year").change(function () {

                var load_year = $('#ddl_year option:selected').html();

                var year_link = "LegislationBrowseResult.aspx?Y=" + load_year + "&tp=sa&tab=3"
                $("#loadactbyyear").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");

                $("#loadactbyyear").load(year_link);






            });


            //       $("#ddlSubject").change(function () {


            //           var load_subject = $('#ddlSubject option:selected').html();
            //           alert(load_subject);
            //           var mydata = "";
            //           mydata = "sub=" + load_subject;
            //           $("#legislation_tab").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            //           $.ajax({
            //               type: "GET",
            //               cache: false,
            //               url: "LegislationBrowseResultBySubject.aspx",
            //               data: mydata,
            //               success: function (msg) {
            //                   $("#legislation_Amen").slideUp("slow");
            //                   $("#legislation_tab").html(msg);
            //                   $(".div_refresh").show();

            //               }
            //           });






            //       });
            function show_legis_preamble(preample) {

                var show_id = "ShowPreamle" + preample;

                if ($("#" + show_id).is(":hidden")) {
                    $('#divPreamle' + preample).html('<img src="img/arrowEx2.png"/>');
                }
                else {
                    $('#divPreamle' + preample).html('<img src="img/arrowEx1.png"/>');
                }

                $("#" + show_id).slideToggle();
            }
            //       $maxPage = 1;
            //       $minPage = 1;
            //       $currentPage = 1;


            ////////////////////////////////////////////////////// 64

            $('#subjectSearch, [name="pages"]').click(function () {

                loadResults(1);
            });

            $('.ul_link_tab').click(function () {
                if ($(this).html() != "BROWSE") {
                    $('.wrapperAddfooter').slideUp();
                    $('#legislation_tab').html('');
                }
                if ($(this).html() != "BILLS") {
                    $('#load_bill').html('');
                }
            });

            //////////////////////////////////////////////////////////----

        });



//   });

//   });


  function showlegcases(srch_char,tp) {

            var myurl = "LegislationBrowseResult.aspx?srt=" + srch_char + "&tp="+ tp;

           
            $("#legislation_Amen").slideUp("slow");
           $("#legislation_tab").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
           $("#legislation_tab").load(myurl).slideDown("slow");
           $(".div_refresh").show();
           
        }
        
        function load_pages(linkid)
        {
        linkid= "#" + linkid;
        var link =  $(linkid).attr("title");
        
        var load_tab = 1;
        
         $.ajax({
                type: "POST",
                url: link,
                data: {load_tab:"1"},
                success: function(msg) {
                    $('#legislation_tab').html(msg).slideDown();
                   
                    return false;
                }
            });
            return false;



        }
        var yr = "";
        var bsrt = "";
        function showbill(year)
        {
            bsrt = 'ta';
            yr = year;
         var mybill = "LegislationBrowseResultByBill.aspx?Y=" + year + "&tp=Bls";
            $("#load_bill").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            $("#load_bill").load(mybill);
            
        }
        //sort by title bill (title and act)*
         function show_Bill_sort(year,bb) {
             bsrt = bb;
         var mybill = "LegislationBrowseResultByBill.aspx?Y=" + year + "&tp=Bls&srtp="+bb;
           
            
            $("#load_bill").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            $("#load_bill").load(mybill).slideDown("slow");
            
        }
        //sort by act bill
        
//         function show_act_sort(year) {
//             bsrt = 'a';
//            
//         var mybill = "LegislationBrowseResultByBill.aspx?Y=" + year + "&tp=Bls&srtp=a";
//           
//            
//            $("#load_bill").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
//            $("#load_bill").load(mybill);
//            
//        }
         //sort by title legislation
         function show_title_sort_legis(srt,tp,mn,max)
        {
            
       
              var myurl = "LegislationBrowseResult.aspx?srt=" + srt + "&tp=" + tp + "&mn=" + mn+ "&mx=" + max+ "&srtp=ta"
          
            $("#legislation_tab").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            $("#legislation_tab").load(myurl);
            
        }
        //sort by title legislation by act
         function show_act_sort_legis(srt,tp,mn,max)
        {
            
           
              var myurl = "LegislationBrowseResult.aspx?srt=" + srt + "&tp=" + tp + "&mn=" + mn+ "&mx=" + max+ "&srtp=a"
           
            $("#legislation_tab").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            $("#legislation_tab").load(myurl);
            
        }
        //load act by year !!!!!
         function load_year(actbyyear)
        {
        
        actbyyear= "#" + actbyyear;
        var actbyyear_ =  $(actbyyear).attr("title");
        
        var load_tab = 1;
        
         $.ajax({
                type: "POST",
                url: actbyyear_,
                data: {load_tab:"1"},
                success: function(msg) {
              
                    $('#loadactbyyear').html(msg);
                   

                }
            });
            return false;
        
        
        
        }
        //load bill pages !!!!!
        function ScrollToTop(el) {
            $('html, body').animate({ scrollTop: $(el).offset().top - 50 }, 'slow');
        }
         function load_bill_page(billyearpage)
        {
        
        //billyearpage= "#" + billyearpage;
            var billpage = "LegislationBrowseResultByBill.aspx?y=" + yr + "&tp=bls&srtp=" + bsrt + "&Page=" + billyearpage //$(billyearpage).attr("title");
       
        var load_tab = 1;
        
         $.ajax({
                type: "POST",
                url: billpage,
                data: {load_tab:"1"},
                success: function(msg) {
                    $("#load_bill").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
                    var focusElement = $("#load_bill");
                    $(focusElement).focus(); // could be annoying , if so then it should be removed
                    ScrollToTop(focusElement); // along with this

                    $('#load_bill').html(msg);
                    
                }
            });
            return false;
        
        
        
        }
        
        function load_act_page(acttitle)
        {
        
        acttitle= "#" + acttitle;
        var act_page =  $(acttitle).attr("title");
       
        var load_tab = 1;
        
         $.ajax({
                type: "POST",
                url: act_page,
                data: {load_tab:"1"},
                success: function(msg) {
              
                    $('#loadactbyyear').html(msg);
                   

                }
            });
            return false;
        
        
        
        }
        //load legislation by number  
        function get_link_by_no(mi,max_,pa)
        {
        
        var load_by_no = "LegislationBrowseResult.aspx?mn=" + mi + "&mx=" + max_ + "&tp=" + pa;
        
         $.ajax({
                type: "POST",
                url: load_by_no,
                data: {load_tab:"1"},
                success: function(msg) {


             $("#legislation_Amen").slideUp("slow");
           $("#legislation_tab").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
            $("#legislation_tab").html(msg).slideDown();
            $(".div_refresh").show();
           
              
                    
                   

                }
            });
            return false;
        
     //alert( ($(this).attr("title")));
     //load pages of legislation by no
        }
        function load_pages_by_no(counter_id)
        {
        
        counter_id= "#" + counter_id;
        var page_poistion =  $(counter_id).attr("title");
       
        var load_tab = 1;
        
         $.ajax({
                type: "POST",
                url: page_poistion,
                data: {load_tab:"1"},
                success: function(msg) {

                    $("#legislation_Amen").slideUp("slow");
                    $("#legislation_tab").html("<br/><center>Search Is Loading.....<img src='GUI/images/load.gif' /></center><br/>");
                    $("#legislation_tab").html(msg).slideDown();
                    $(".div_refresh").show();
           


                   
                   

                }
            });
            return false;
        
        
        
        }
        function replaceContent(show)
        {
        
        var show_id = show;
      
        //var div_id =document.getElementById("boxOrange").innerHTML
           
            var array_tab = new Array();
             $("#boxOrange").slideDown("slow");
           //$("#legislation_tab").html("Search Is Loading.....<img src='GUI/images/loading.gif' />");
            $("#legislation_tab").slideUp();
            
            
            array_tab[show_id]= div_id;
           
            //document.getElementById("boxOrange").innerHTML = display[show];
           // $("#boxOrange").load(array_tab[show_id]);
        
        }
        //function to show legislation preample
        function show_legis_preamble(preample)
        {
             var show_id = preample;

             $("#legislation" + show_id).hover(function () {
                 $("#preamble" + show_id).slideDown("slow");
                           }, function() {
                               $("#preamble" + show_id).slideUp(500);

             });
        
        
        }
        function show_legis_preamble_year(preample)
        {
             var show_id = preample;
             
       $("#legislation_year" + show_id).hover(function(){
           $("#preamble_year" + show_id).slideDown("slqw");
       }, function () {
           $("#preamble_year" + show_id).slideUp(500);
        });


   }

        function show_legis_preamble_bill(bill_id)
        {
            $("#legislation_bill" + bill_id).hover(function () {

                $("#preamble_bill" + bill_id).slideDown("slow");
       }, function () {
           $("#preamble_bill" + bill_id).slideUp(500);
    
        });
      }
      
       
        
        
        
        //legislation sections
        function showlegs_sections(divlegsections)
	{
	
//		//alert(divlegsections);
//		 $("#divlegsrchrslts").hide();
//		  $("#divshowlegs_sections").empty().html('<br><center><img src="GUI/images/loading.gif" /></center>');
//    var strlegisid="legis_sections.aspx?legis_id=" + divlegsections.toString();
//   // alert(strlegisid);
//    $("#divshowlegs_sections").load(strlegisid);

	}


	function show_legis_preamble(preampleid) {
	   
	    var show_id = "ShowPreamle" + preampleid;
	    $("#" + show_id).slideToggle();
	}

	function refresh() {
	   
	    $("#legislation_tab").slideUp("slow").html();
	    $("#legislation_Amen").slideDown("slow");
	    $(".div_refresh").hide();
	    
     }

     function button2_onclick() {

     }



</script>

</head>
<body>
		
<form id="Form1" method="post" runat="server">
<asp:HiddenField ID="hidLastTab" runat="server" Value="0" />

        
<!-- #include file="include/headerMainLegis.aspx" -->

<div id="content3">

		<div class="wrapperContCase" style="margin-bottom:70px; margin-top: 70px; width:900px;">
		 
    
    
    
	<div id="tabs">
    <nav class="wrapperTabs">
    <div class="tabsPosition">
    <ul  class="tabsBox">
		<li id="tabcss0" class="current1"><a href="#tabs-1" class="ul_link_tab">LEGISLATION</a></li>
		<li id="tabcss1"><a href="#tabs-2" class="ul_link_tab">BROWSE</a></li>
		<li id="tabcss2"><a href="#tabs-3" class="ul_link_tab" style="display:none">COURT RULES</a></li>
		<li id="tabcss3"><a href="#tabs-4" class="ul_link_tab" style="display:none">SUBSIDIARY ACTS</a></li>
		<li id="tabcss4"><a href="#tabs-5" class="ul_link_tab" style="display:none">BILLS</a></li>
	</ul>
    </div>
    </nav>

    
   
    
    
    <!--Content 1-->
    
    <div id="tabs-1">
     
    <div class="caseBox2">
    <div class="clear" align="center" style="padding-top:10px"><asp:label id="lblMsg" runat="server" 
            Font-Bold="False" ForeColor="Red"></asp:label>
																																				
																			
		<asp:regularexpressionvalidator id="RegularExpressionValidator3" runat="server" 
            ErrorMessage="Invalid Act No" ControlToValidate="txtActNo" 
            ValidationExpression="[A-Za-z0-9&amp;.,'&quot;_\s\(\)\/]*" Display="Dynamic" 
            SetFocusOnError="True"></asp:regularexpressionvalidator>
                															</div>
    <!-- zubair
ToolTip="To search Exact Phrase / All of The Words / Any of the Words from Elaw library."
 ToolTip="Elaw Legislation Search allows you to search through keywords to retrieve the most relevant documents from our digital library."
 -->
    	  <table width="96%" border="0" cellpadding="0" cellspacing="0" style="margin:20px;">
    	    <tr>
    	      <td><table width="100%" border="0" cellpadding="0">
    	        <tr>
    	          <td width="72%">
                  <div class="ui-widget"> 
              <asp:textbox id="txtFTS" runat="server" 
			MaxLength="150" class="advBoxSfix" style="width:640px; padding:0 5px 5px 10px;"></asp:textbox>

              </div>
          </td>
    	          <td width="28%">
                  
              

              <asp:button id="btnSearch" 
                  runat="server" 
                 
		class="btnSave" style=" width:160px; margin-left:10px" Text="Search"></asp:button>
                 </td>
  	          </tr>
  	        </table></td>
  	      </tr>
    	    <tr>
    	      <td><table width="100%" border="0" cellpadding="2" cellspacing="2">
    	        <tr>
                <!--abdo-->
                <td width="400">
                Within:
                  <asp:DropDownList ID="ddlsrch" runat="server" class="advBoxS6">
                    <asp:ListItem Selected="True" Value="1">Legislation</asp:ListItem>
                    <asp:ListItem Value="2">Act Title</asp:ListItem>
                    <asp:ListItem Value="3">Section Title</asp:ListItem>

                    <asp:ListItem Value="4">3 Words</asp:ListItem>
                    <asp:ListItem Value="5">4 Words</asp:ListItem>
                    <asp:ListItem Value="6">5 Words</asp:ListItem>
                    <asp:ListItem Value="7">same sentence</asp:ListItem>
                    <asp:ListItem Value="8">same paragraph</asp:ListItem>
                    <asp:ListItem Value="9">same page</asp:ListItem>
                    </asp:DropDownList>
                
                </td>

                <!-- end of abdo code-->

    	          <td width="6"><span class="tTextNorm"></span></td>
    	          <td width="126">
                  <asp:DropDownList 
                          ID="ddlproxmity" runat="server" class="advBoxSfix" style="visibility:hidden; height:25px; margin:11px 0 8px 0;">
                      <asp:ListItem Value="3">within Page</asp:ListItem>
                      <asp:ListItem Value="2"> within Paragraph</asp:ListItem>
                      <asp:ListItem Value="1"> within Sentence</asp:ListItem>
                      </asp:DropDownList>
                  </td>
    	          <td width="49" class="tTextNorm"></td>
    	          <td width="12">
                  <asp:dropdownlist id="ddlprox" runat="server" 
      class="advBoxSfix" style="visibility:hidden; height:25px; margin:11px 0 8px 0; width:160px;" EnableViewState="False">
                    <asp:ListItem Value=""></asp:ListItem>
                    <%--<asp:ListItem Value="10">1</asp:ListItem>
                    <asp:ListItem Value="20">2</asp:ListItem>
                    <asp:ListItem Value="30">3</asp:ListItem>--%>
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                    
                      </asp:dropdownlist>
                  </td>
    	          <td width="20" class="tTextNorm"></td>
                   <td width="80" class="tTextNorm"><asp:button id="btnReset" runat="server" ToolTip="Pressing this button will clear all textfields."
		class="btnWhite" Text="Clear" Visible="False"></asp:button></td>
    	          <td width="167" class="tTextNorm">
                  <asp:DropDownList ID="rbs" runat="server" class="advBoxSfix" style="visibility:hidden; height:25px; width:110px; margin-left:10px;">
              <asp:ListItem Selected="True" Value="1">Full Document</asp:ListItem>
                      <asp:ListItem Value="2">Act Title</asp:ListItem>
                      <asp:ListItem Value="3">Section Title</asp:ListItem>

              </asp:DropDownList></td>
    	         
   	            </tr>
  	        </table></td>
  	      </tr>
  	    </table>



   
    
<div class="clear"></div>


                            <div class="wrapperAdvCaseLaw" style="padding:0; margin:0;">
        <div class="arrow2" style="padding:12px 10px 0 20px;">▲</div>
		<h1 class="search">Advanced Search</h1>
		 <div class="scrollBoxWrapper2">
        <!--Left Advance Search -->
        <div class="wrapperAdvCaseLaw">
         <aside class="leftAdvCaseLaw2">
          <table width="100%" border="0" cellpadding="2" cellspacing="2">
            <tr>
              <td width="134" class="tTextNorm">Without the words:</td>
              <td width="252">
            <asp:textbox id="txtNotCases" runat="server"
                         ToolTip="To search case law by none of these words from Elaw library."
                         MaxLength="150"
                          CssClass="advBoxS6"></asp:textbox></td>
            </tr>
            <tr>
              <td class="tTextNorm">Act No:</td>
              <td>
                  <asp:TextBox ID="txtActNo" runat="server" MaxLength="150" class="advBoxS6"></asp:TextBox>
             
              
                </td>
            </tr>

          </table>
        </aside>
         <aside class="rightAdvCaseLaw2">
         
        </aside>
        
        <!--Right Advance Search -->
        
        
        </div>
        </div>
        <div class="clear"></div>
        </div>



    <div class="clear1"></div>
    </div> 
    </div>
    
    <!--Content 2-->
            
    <div id="tabs-2">
		
    <div class="caseBox2" id="boxOrange"> 
        <table width="800px" border="0" cellpadding="2" cellspacing="2" style="margin:20px;">
    	    <tr>
    	      <td width="80" class="tTextNorm" >Subject Area:</td>
    	        <td width="200">
                    <asp:DropDownList Width="200" ID="ddlSubject" runat="server" class="advBoxS5" style=" height:25px; margin:0 5px; text-transform:capitalize;"></asp:DropDownList>
                </td>
                <td width="50">Act No: </td>
                <td width="80"><select id="actRange" class="advBoxS2" style="width:80px">
                    <option value="0">*</option>
                    <option value="1">1-100</option>
                    <option value="2">101-200</option>
                    <option value="3">201-300</option>
                    <option value="4">301-400</option>
                    <option value="5">401-500</option>
                    <option value="6">501-600</option>
                    <option value="7">601+</option>
                    </select></td>
                <td width="35">Title:</td>
                <td><select id="alphabet" class="advBoxS2" style="width:40px">
                    <option value="">*</option>
                    <script type="text/javascript">
                    for(var i=65 ;i<91;i++)
                        document.write("<option value='" + String.fromCharCode(i) + "'>" + String.fromCharCode(i) + "</option>");
                    </script>
                    </select>
                </td>
                <td>
                    <input type="radio" value="1" name="browseType" checked="checked"/>Principal <br/>
                    <input type="radio" value="2" name="browseType"/>Amendment
                </td>
                <td width="40"><input type="button" name="search" id="subjectSearch" value="Display" class="btnSave"/></td>
                 
              <%--<td width="40"> <div onclick="refresh()" style="padding: 5px; float:left; width:120px;" class="div_refresh">Search Again</div></td>--%>
  
  
  
              
  	      </tr>
  	    </table>
   
       <div id="legislation_tab" style="padding-top:1px"></div>
    
    </div>
   
    </div>
      
      <!--Content 3-->      

    <div id="tabs-3">
       
	<div   class="caseBox2"><aside class="boxBgblue2" style="margin-top:10px;"><span class="tTextNorm2"><strong>Court Rules - Direct Links</strong></span></aside>


                 <table width="503" border="0" cellpadding="2" cellspacing="2" style="margin:20px;">
    	    <tr>
    	      <td width="167" class="tTextNorm">PU(A) 525/1994</td>
    	      <td width="322" class="tTextNorm4"><A  id="A4" href="LegislationMainDisplayed.aspx?id=MY_FS_ACT_1957_000">Federal Constitution</A></td>
            </tr>
    	    <tr>
    	      <td class="tTextNorm">PU(A)376/1995</td>
    	      <td class="tTextNorm4"><A  id="HyperLink2" href="LegislationMainDisplayed.aspx?id=MY_FS_NLC_1965_56">National Land Code</A></td>
   	        </tr>
    	    <tr>
    	      <td class="tTextNorm">PU(A) 524/1994</td>
    	      <td class="tTextNorm4"><A  id="HyperLink3" href="LegislationMainDisplayed.aspx?id=MY_FS_PUA_1967_406">Industrial Court Rule 1967</A></td>
   	        </tr>
    	    <tr>
    	      <td class="tTextNorm">PU(A)50/1980</td>
    	      <td class="tTextNorm4"><A  id="HyperLink4" href="LegislationMainDisplayed.aspx?MY_FS_PUA_1980_050">Rules of the High Court 1980</A></td>
  	      </tr>
    	    <tr>
    	      <td class="tTextNorm">PU(A)328/1980</td>
    	      <td class="tTextNorm4"><A  id="HyperLink5" href="LegislationMainDisplayed.aspx?id=MY_FS_PUA_1995_376">Rules of the Federal Court 1995</A></td>
  	      </tr>
    	    <tr>
    	      <td class="tTextNorm">PU 406/1967</td>
    	      <td class="tTextNorm4"><A  id="HyperLink6" href="LegislationMainDisplayed.aspx?id=MY_FS_PUA_1994_525">Rules of the Special Court 1994</A></td>
  	      </tr>
    	    <tr>
    	      <td class="tTextNorm">PU 406/1967</td>
    	      <td class="tTextNorm4"><A  id="HyperLink7" href="LegislationMainDisplayed.aspx?id=MY_FS_PUA_1980_328">Subordinate Court Rules 1980</A></td>
  	      </tr>
    	    <tr>
    	      <td class="tTextNorm">PU 406/1967</td>
    	      <td class="tTextNorm4"><A  id="HyperLink8" href="LegislationMainDisplayed.aspx?id=MY_FS_PUA_1994_524">Rules of the Court of Appeal 1994</A></td>
  	      </tr>
  	      </table>
    	  <div class="clear"></div>



    
    </div>
    
    </div>
	
    
    <div id="tabs-4" style="display:none">
    <div  class="caseBox2" id="subsidiary">

   
    	  <table width="860px" border="0" cellpadding="0" cellspacing="0" style="margin:20px;">
          <tr>
            <td width="42" class="tTextNorm">
            <table width="100%" border="0" cellpadding="2" cellspacing="2">
              <tr>
                <td width="76" class="tTextNorm">Type:</td>
                <td width="283">
                <asp:DropDownList ID="ddl_role" runat="server" 
                AppendDataBoundItems="True" CssClass="advBoxS5" style=" height:25px; margin:5px;">
        <asp:ListItem Selected="True">Select Type</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_RUL">Rules</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_REG">Regulations</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_ORD">Order</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_NOT">Notification</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_PRO">Proclaimation</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_APP">Appointment</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_BYL">By-Law</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_STA">Statute</asp:ListItem>
        <asp:ListItem Value="MY_FS_PUA_ORN">Ordinance</asp:ListItem>
    </asp:DropDownList></td>

    <td width="42" class="tTextNorm">Year:</td>
                <td width="433">
                                <asp:DropDownList ID="ddl_year" runat="server" 
                  AppendDataBoundItems="True" class="advBoxS2" style=" height:25px; width:90px; margin:5px;">
              </asp:DropDownList></td>

               </tr>
            </table></td>
          </tr>
          <tr>
            <td>
            <table width="100%" border="0" cellpadding="2" cellspacing="2">
              <tr>
                <td width="76" class="tTextNorm">PU(A)</td>
                <td width="102"><select name="select2" class="advBoxS2" style=" height:25px; width:90px; margin:5px;">
                                <option value="Year">Year</option>
                                <option value="2011">2011</option>
                                <option value="2010">2010</option>
                                <option value="2009">2009</option>
                                <option value="2008">2008</option>
                                <option value="1969">1969</option>
 								</select></td>
                <td width="44" class="tTextNorm">PU(B)</td>
                <td width="100"><select name="select4" class="advBoxS2" style=" height:25px; width:90px; margin:5px;">
                                <option value="Year">Year</option>
                                <option value="2010">2010</option>
                                <option value="2009">2009</option>
                                <option value="2008">2008</option>
                                <option value="2007">2007</option>
                                <option value="2006">2006</option>
                                <option value="2005">2005</option>
                                <option value="2004">2004</option>
                                <option value="2003">2003</option>
                                <option value="2002">2002</option>
                                <option value="2001">2001</option>
                                <option value="2000">2000</option>
                                <option value="1999">1999</option>
                                <option value="1998">1998</option>
                                <option value="1997">1997</option>
                                <option value="1996">1996</option>
                                <option value="1995">1995</option>
                                </select></td>
                <td width="27" class="tTextNorm">PU</td>
                <td width="473"><select name="select5" class="advBoxS2" style=" height:25px; width:90px; margin:0 5px;">
                                <option value="Year">Year</option>
                                <option value="1968">1968</option>
                                <option value="1967">1967</option>
                                <option value="1966">1966</option>
                                </select></td>
                
              </tr>
            </table></td>
          </tr>
          <tr>
            <td><input type="submit" name="button2" id="button2" value="View" class="btnSave" style=" width:160px; margin:5px 0 5px 91px;" onclick="return button2_onclick()"></td>
          </tr>
          </table>


   
      
   

      
        <div class="clear1"></div>
        <div id="loadactbyyear"></div>
    	<div class="clear1"></div>
    </div>
    
    </div>
    
    <div id="tabs-5">
    <div  class="caseBox2" id="bill_5">
    
     <section class="listboxAct3"> 
        <asp:Literal ID="ltr_bill" runat="server"></asp:Literal>
        <div class="clear"></div>
        </section>
    
    <div id="load_bill" style='background-color:white'></div>
    </div>
    
    </div>
        
		</div>
    </div>
   </div>
		<!-- footer -->

<footer> 
	<div class="wrapperFooter">
	    <section class="wrapperAddfooter">

        <aside class="gotoPage" style="margin-left:0px">
            <div style="text-align: center; width: 100%">
                <div id="lblBottomNavigation" style="font-size:12pt"></div>
            </div>
        </aside>
    
        <div class="clear"></div>
        </section>

	<!-- #include file="include/footerMain.aspx" -->

    </div>
</footer>  
		
			<script src="js/script.js" type="text/javascript"></script>
		</form><!-- Piwik -->
<script type="text/javascript">
  var _paq = _paq || [];
  _paq.push(["setDocumentTitle", document.domain + "/" + document.title]);
  _paq.push(["setCookieDomain", "*.www.elaw.my"]);
  _paq.push(["trackPageView"]);
  _paq.push(["enableLinkTracking"]);

  (function() {
    var u=(("https:" == document.location.protocol) ? "https" : "http") + "://elaw.my/analytics/";
    _paq.push(["setTrackerUrl", u+"piwik.php"]);
    _paq.push(["setSiteId", "1"]);
    var d=document, g=d.createElement("script"), s=d.getElementsByTagName("script")[0]; g.type="text/javascript";
    g.defer=true; g.async=true; g.src=u+"piwik.js"; s.parentNode.insertBefore(g,s);
  })();
</script>
<!-- End Piwik Code -->
	</body>
</html>
