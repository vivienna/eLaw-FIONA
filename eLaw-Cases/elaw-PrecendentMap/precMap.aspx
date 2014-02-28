<script runat="server">
    Public fileName As String
    Public dataType As String
    Public NewSearch As String
    Public SearchFiled As String
    Dim classicVsMod As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs)
        If Request.QueryString("id") = "" Then
            Response.Redirect("~/login.aspx")
        End If
        fileName = Server.UrlDecode(Request.QueryString("id"))
        dataType = Server.UrlDecode(Request.QueryString("t"))
        NewSearch = Server.UrlDecode(Request.QueryString("sIn"))
        SearchFiled = Server.UrlDecode(Request.QueryString("SearchFiled"))
        classicVsMod = Server.UrlDecode(Request.QueryString("cls"))
    End Sub
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
		<meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no"/>
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
		<meta name="HandheldFriendly" content="true"/>
        <title>Precedent Map | eLaw.my</title>
        <!-- New CSS -->
        
        <!-- Close New CSS-->
		<link rel="stylesheet" href="css/PrecReset.css"/>
		<link rel="stylesheet" href="css/PrecStyle.css"/>
        <link rel="stylesheet" href="css/datavis.css"/>
        <script type="text/javascript" src="js/libs/jquery-1.7.1.min.js"></script>
        <script type="text/javascript" src="js/libs/d3.v2.js"></script>
        <script type="text/javascript" src="js/Tooltip.js"></script>
        <script type="text/javascript" src="js/datavis.js"></script>
        <script type="text/javascript">
            $(document).ready(jqUpdateSize);
            $(window).resize(jqUpdateSize);

            $(document).ready(function () {
                if ($('#orientation').html() == 'ref')
                { $('#mapType').val('1'); $('#mapTypeClassic').val('1'); }

                $('#mapType').change(function () {
                    var url = 'precMap.aspx?id=' + $('#mapTitle').attr('data-id').trim() + '&cls=0&t=';
                    if ($(this).val() == '1') { url += 'ref'; $("#Referred").text("Referring To"); }
                    else { url += 'refd'; $("#Referred").text("Referred By"); }
                    //$.post('precMap.aspx',{id: $('#mapTitle').attr('data-id').trim(), t: url});
                    window.location = url;
                });

                ////////////////////////////////////////////////////////////'

                $('#mapTypeClassic').change(function () {
                    var url = 'precMap.aspx?id=' + $('#mapTitle').attr('data-id').trim() + '&cls=1&t=';
                    if ($(this).val() == '1') { url += 'ref'; }
                    else { url += 'refd'; }
                    window.location = url;

                });


                ////////////////////////////////////////////////////////
                var conceptName = $('#mapType').find(":selected").val();
                $("#Div1").html("<a href='CaseCitator.aspx?fn=" + $('#mapTitle').attr('data-id').trim() + "&t=" + conceptName + "'>Classic View</a>");
               
               
                $("#btnSearch").click(function () {
                    var txt = $("#SearchIn").val();
                    var CaseIde = $("#mapTitle").attr("data-id");
                    var OldsIn;
                    if (typeof getUrlVars()["sIn"] != "undefined") {
                        OldsIn = getUrlVars()["sIn"] + ",";
                    } else { OldsIn = "" }
                    var sInNew = OldsIn + " " + txt
                    var url = 'precMap.aspx?id=' + $('#mapTitle').attr('data-id').trim() + '&cls=0&sIn=' + sInNew;
                    window.location = url;
                });
                /////////////////////////////////////////////////
                $("#ClassicSearch").click(function () {
                    var txt = $("#Classictxt").val();
                    var CaseIde = $("#mapTitle").attr("data-id");
                    var OldsIn;
                    if (typeof getUrlVars()["sIn"] != "undefined") {
                        OldsIn = getUrlVars()["sIn"] + ",";
                    } else { OldsIn = "" }
                    var sInNew = OldsIn + " " + txt
                    var url = 'precMap.aspx?id=' + $('#mapTitle').attr('data-id').trim() + '&cls=1&sIn=' + sInNew;
                    window.location = url;
                });
                //////////////////////////////////////////////////
                $("#btnClear").click(function () {
                    var url = 'precMap.aspx?id=' + $('#mapTitle').attr('data-id').trim() + '&cls=0';
                    window.location = url;
                });
                //////////////////////////////////////////////////////////////////////
                $("#ClassicClear").click(function () {
                    var url = 'precMap.aspx?id=' + $('#mapTitle').attr('data-id').trim() + '&cls=1';
                    window.location = url;
                });
                //////////////////////////////////////////////////////////////////////

                $(".title2Case2 a").live('hover', function () {
                    var classid = $(this).attr("id");
                    var classtitle = $(this).attr("title");
                    var dd;
                    ///////////////// call ajax 
                    $.ajax({
                        type: "POST",
                        url: "casessearchresult1.aspx/getToolTip",
                        data: '{SecReceiv: "' + classid + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            dd = data.d;
                            var elementTitle = document.getElementById(classid).title = dd
                        },
                        failure: function (response) {

                        }
                    });

                });



                ////////////////////////////
                //////////////////////////////////////////////////////////////////////////////
                $("#LeftExpand").click(function () {

                    $("#contRight").slideUp("slow");
                    $("#colleft").slideUp("slow", function () { $('#classic').slideDown("slow"); $(".tooltip").hide(); });


                });
                //////////////////////////////////////////////////////////////////////////////
                $("#ExpandNew").click(function () {
                    $("#classic").slideUp("slow");
                    $("#colleft").slideDown("slow");
                    $("#contRight").slideDown("slow");
                });

                ////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////

            });
            function getUrlVars() {
                var vars = {};
                var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }
            function jqUpdateSize() {
                // Get the dimensions of the viewport
                var width = $(window).width();
                var height = $(window).height();

                $('#cloud').width(width - 300);
                $('#cloud').height(height);
            };

        </script>
       <style type="text/css">
      html,body,div,span,h1,h2,h3,h4,h5,h6,p,ol,ul,li,form,label,legend,caption,aside,details,figcaption,figure,footer,header,hgroup,menu,nav,section,summary{border:0;outline:0;font-weight:inherit;font-style:inherit;font-size:100%;font-family:Arial, Helvetica, sans-serif;vertical-align:baseline;margin:0;padding:0}
        a img{border:none}
        html{width:100%;height:100%}
        body{margin:0}
       #colleft{float:left;overflow:hidden;background:#f9f9f9;
                width:266px;border:1px solid #c2c2c2;border-radius:0 5px 5px 0;
                -moz-border-radius:0 5px 5px 0;-webkit-border-radius:0 5px 5px 0;-o-border-radius:0 5px 5px 0;color:#fff; text-align:left;
                 }
        #colleft h1{height:30px;background-color:#e5e5e5;color:#000;border-bottom:1px solid #c2c2c2;font-weight:400;font-size:14px;
              padding:12px 0 0 20px}
        #colleft ul{margin-bottom:20px}
        #colleft ul li{border-bottom:1px solid #c2c2c2;font-family:Arial, Helvetica, sans-serif;font-size:12px;color:#000;padding:10px 0 2px 5px}
        #colleft #hidePanel a{color:#FFF}
        #colleft #hidePanel a:hover{color:#999}
        #showPanel{position:inherit;z-index:2;left:0;float:left;padding-top:30px;
           display:none;width:0;height:200px;cursor:pointer}
        #showPanel .leftQuicksearch{display:block;font-size:24px;height:170px;width:30px;
                                background:#e5e5e5;border:1px solid #c2c2c2;
                                border-radius:0 5px 5px 0;-moz-border-radius:0 5px 5px 0;
                                -webkit-border-radius:0 5px 5px 0;
                                -o-border-radius:0 5px 5px 0;padding:5px}
        #showPanel p{color:#000;border:0 solid red;writing-mode:tb-rl;-webkit-transform:rotate(90deg);-moz-transform:rotate(90deg);-o-transform:rotate(90deg);white-space:nowrap;display:block;bottom:0;width:20px;height:20px;font-family:Arial, Helvetica, sans-serif;font-size:14px;font-weight:700;padding:20px 0 0 22px}
        .boxRecentResult1{border-bottom:1px #000 dotted;padding:10px 15px 10px 17px;text-align:left; }
        .title2Case2{font-family:Arial, Helvetica, sans-serif;font-size:14px;font-weight:700;}
        .title2Case2 a:hover,.title3Case a:hover{text-decoration:underline}
        .title2Case2 a{text-decoration:none;color:#00648e}
        .title3Case{font-family:Arial, Helvetica, sans-serif;font-size:12px;font-style:italic}
        .title2Case a,.title3Case a{text-decoration:none;color:#090909}
        .title3Case{font-family:Arial, Helvetica, sans-serif;font-size:12px;font-style:italic}
        #colleft #hidePanel,#showPanel .btnShowSearch{height:28px;width:28px;border-radius:5px;background-color:red;
                                                      -webkit-box-shadow:0 1px 1px #000;-moz-box-shadow:0 1px 1px #000;
                                                        box-shadow:0 1px 1px #000;padding:0}
         .signOut{float:right;display:block;border-left:#fff solid 1px;padding:8px 18px}
        .signOut a{font-family:Arial, Helvetica, sans-serif;font-size:12px;color:#ff9455;text-decoration:none;font-weight:700}
        .signOut a:hover{color:#fff;text-decoration:underline}

         .header{background-color:#000;position:absolute;width:100%;top:0;left:0;right:0;z-index:10;margin:0;padding:0;}
         .header,.menu { display:block}
          .home{float:left;border-right:#fff 1px solid;padding:6px 8px}                                              
           .menu { float:left; width:431px; font-family:Arial, Helvetica, sans-serif; font-size:12px;}
            .menu ul { list-style:none; margin:0; padding:0;}
            .menu ul li, .menu2 ul li { float:left; border-right:#ffffff 1px solid;}
            .menu ul li a, .menu2 ul li a { text-decoration:none; display:block; padding:12px 18px; background-color:#00648e; color:#ffffff; font-weight:bold;}
            .menu ul li a:hover, .menu2 ul li a:hover { background-color:#ffffff; color:#333333; display:block;border-right:#b9e4fb 1px solid}
            .menu ul li ul li, .menu2 ul li ul li{ padding:0; float:none; margin:0 0 0 0px; width:100%;}
            .menu ul li ul, .menu2 ul li ul { position:absolute; border:1px solid #C3D1EC;
                                              -webkit-box-shadow:0 1px 5px #CCCCCC;
                                                -moz-box-shadow:0 1px 5px #CCCCCC;
                                                box-shadow:0 1px 5px #CCCCCC;
                                                margin-top:-1px;
                                                display:none;
                                                padding:0px 16px 0px 0;
                                                }
            .menu ul li.current a  { background-color:#ffffff; color:#333333; display:block; padding:12px 18px; cursor:pointer; font-weight:bold;}                                 
            .menu2 { float:right; width:280px; font-family:Arial, Helvetica, sans-serif; font-size:12px; color:#ffffff;}
            .menu2 ul { list-style:none; margin:0; padding:0;}
            .menu ul li.current a  { background-color:#ffffff; color:#333333; display:block; padding:12px 18px; cursor:pointer; font-weight:bold;}

       
       #nav1 { list-style: none; float: left; }
       #nav1 li, #nav2 li { float:left; display: block; background:#000000; position: relative; z-index:100; border-right:none;  }
       #nav1 li a{ background-color:#000000;  
	                font-size:12px; 
	                text-decoration: none;  
	                color: #ffffff; 
	                zoom: 1; 
	                padding:11px 18px 0 18px; }

        #nav1 li a:hov,#nav2 li a:hov2 { color:#ffffff; }
        #nav1 ul { position:absolute; display: none; margin:0; padding:0; list-style: none; border-bottom: 3px solid #00648e;  }
        #nav1 ul li { width: 160px; float: left; border-top:1px solid #333333; text-align:left; background-color: #ffffff; color:#333333; }
        #nav1 ul a { display:block; height:16px; line-height:20px; padding: 2px 5px 8px 16px; color:#333333; background-color:#ffffff; font-weight:normal;}
        #nav1 ul a:hover { text-decoration:none; background-color: #000000; color: #ffffff;  }


        #nav2 { list-style: none; float: right; }
        #nav2 li a{background-color:#000000;  font-size:12px; text-decoration: none;  color: #ffffff; zoom: 1; padding:11px 18px 0 11px; }
        #nav2 ul { position:absolute; right:0px; display: none; margin:0; padding:0; list-style: none; border-bottom: 3px solid #00648e;  }
        #nav2 ul li { width: 120px; float: left; border-top:1px solid #333333; text-align:left; background-color: #ffffff; color:#333333; }
        #nav2 ul a { display:block; height:16px; line-height:20px; padding: 2px 5px 8px 16px; color:#333333; background-color:#ffffff; font-weight:normal;}
        #nav2 ul a:hover { text-decoration:none; background-color: #000000; color: #ffffff;  }


        .twelcome{float:right;font-family:Arial, Helvetica, sans-serif;font-size:12px;color:#fff;text-align:right;padding:11px 0 0}
        .clear1{width:100%;clear:both}
        .advBoxS6{font-size:12px;height:22px;color:#757575;border:1px #bababa solid;margin:5px 0}
        
        .btnSave{width:80px;text-align:center;background-color:#f47200;-webkit-box-shadow:0 1px 1px #000;
         -moz-box-shadow:0 1px 1px #000;box-shadow:0 1px 1px #000;border-radius:5px;
         font-family:Arial, Helvetica, sans-serif;color:#fff;font-size:14px;cursor:pointer;border:0;padding:5px}
         .tfont2 { color:#ffffff; font-family:Arial, Helvetica, sans-serif; font-size:12px; font-weight: normal; margin:0 0 0 20px;}
         .advBoxS5 { width:273px; font-size:12px; height:20px; color:#757575; border:1px #bababa solid; margin:10px 0 5px 0; }
         
         
         .boxResult{background-color:#f2f2f2;border-radius:5px;display:block;margin:5px 0px 0 0}
         .resultTitleCase{display:block;border-left:#666 1px solid;padding:10px 0}
         .tTitleCase{font-family:Arial, Helvetica, sans-serif;font-size:16px;margin-left:10px;font-weight:700}
         .title2Case{font-family:Arial, Helvetica, sans-serif;font-size:12px;margin-left:10px;text-transform:capitalize;}
        .tItalic{font-style:italic;font-size:13px;margin-left:10px;padding-top:2px}
        .tTitleCase a{text-decoration:none;color:#00648e}
        .boxResult:hover { background-color:#e0eff7; cursor:pointer;}
        
        .rightResultBox{float:right;margin-right:5px}
        .funtionResults{margin-right:5px;font-size:12px}
        .funtionResults a img{border:none;vertical-align:middle;margin:0 10px}
        .funtionResults a{text-decoration:none;color:#fff}
         
       </style>
       <!-- tooltip-->
       <style>
a.tip {
        text-decoration: none
}
a.tip:hover {
    cursor: help;
    position: relative
}
a.tip span {
    display: none; background:red;
    
}
a.tip:hover span {
    border: #c0c0c0 1px dotted;
    padding: 5px 20px 5px 5px;
    display: block;
    z-index: 1px;
   
    left: 0px;
    margin: 10px;
    width: 250px;
    position: absolute;
    top: 10px;
    text-decoration: none
}
</style>
    </head>
    <body >
    <!-- New CSS-->
    <div class="header">

<div class="home"><a  href="index1.aspx"><img border="0" src="img/logoHome.png" align="middle"/></a></div>
 
 <div class="menu">
 <ul>
 
 <li class="current"><a href="casesSearch.aspx">CASE LAW</a></li>
 <li> <a href="legislationSearch.aspx">LEGISLATION</a></li>
 </ul>

 <!-- Drop btn1 -->
 
 <ul id="nav1"><li><a href="#">MORE <img style="margin-left:8px;" src="img/arrowDown.png" width="18" height="15" align="top" border="0" /></a>
     <ul>
      <li style="border:none;"><a href="formSearch.aspx">Forms</a></li>
     <li  > <a href="case_notes/user_cases.aspx">My Briefcase</a></li>
     <li><a href="lawDictionary.aspx">Dictionary</a></li>
   
    
      <li><a href="practiceNotesSearch.aspx">Practice Notes</a></li>
                <li><a href="#"  style="border-right:none;">Help</a></li>
	 </ul>
 	</li>
 </ul>
    
 </div>
 
 <div class="signOut"><a href="signout.aspx">Sign Out</a></div>
 
 <!-- Drop btn2 -->
 
 <div class="menu2">
 <ul id="nav2">
 <li><a href="#"><img style="margin-left:8px;" src="img/arrowDown.png" width="18" height="15" align="top" border="0" /></a>
     <ul>
     
      <li><a  href="AccountDetail.aspx">Account Detail</a></li>
            <li><a  href="changePassword.aspx">Change Password</a></li>
            <li><a  href="UpdateAddress.aspx">Update Address</a></li>
            <li><a  href="AccountUsage.aspx">Account Usage</a></li>
	 </ul>
 	</li>
 </ul>
 <span class="twelcome">Welcome Back! <strong><asp:label  id="lbl_username" runat="server" ></asp:label></strong> </span>
 </div>
 
 
<div class="clear1"></div>
</div>


    <!-- Close Header -->

    <div id="main-contenet" style="width:100%;height:100%;margin:45px 0">
	<div class="topBar1" 
            style="padding-top: 5px; padding-bottom:25px; border-left-style: none; border-right-style: none; margin-left:310px;">
              <div class="leftResultBox" style="float:left;margin-left:4px">
              <div class="funtionResults" style="margin-right:5px;font-size:12px">
              <span style="float:left; display:block; padding:5px 5px 0 0;" id="mapTitle" data-id= "<%= fileName%>">   </span>
              </div></div>
              </div>
              <div class="clear1"></div>




              <div id="classic" style=" margin-right: 85px; margin-left: 85px; text-align:left; display:none;">
              <div  style="background-color:#757575;font-size:14px; padding-top: 3px; padding-bottom:32px; border-left-style: none; border-right-style: none;">
    
    <div class="leftResultBox" style="float:left; margin-top:2px;">
     <span class="tfont2">Filter Results By</span>
     <select id="mapTypeClassic" name="sortResult" class="advBoxS5" style=" height:20px; margin:0 5px; width:180px;">
     <option value="0">case refers to</option>
            <option value="1">case referred by</option>
    </select>
    <input type="text"  id="Classictxt" class="advBoxS6" style=" height:20px; margin:0 5px; width:180px;"/>
    <input type="button"  id="ClassicSearch" value="Search Within" class="btnSave" style="width:120px; margin-left:5px;"/>
    <input type="button"  id="ClassicClear" value="Reset" class="btnSave" style="width:100px; margin-left:5px;"/>
                             <button id="nextClassic"  class="hidden">Next</button>
    </div>


    <div class="rightResultBox">
    <div class="funtionResults">
       <a href="#" id="ExpandNew"><img border="0" src="img/tl2.png"/>Map</a>
    </div>
     </div>

    </div>
    <div id="ListCases"></div>
    </div>




    <div id="showPanel">
	<div class="leftQuicksearch">
    <div class="btnShowSearch"><img style="margin:8px 0 0 8px;" align="top" src="img/ico2.png"/></div>
    <p>Advance Search</p></div>
    </div>
    <div id="colleft" style="width:290px">
      <div id="">
        <h1><span id="Referred">Case Refers To</span>
          <div id="hidePanel" style="position: relative; top:-25px; right:-240px;"><a href="#"><img style="margin:4px 0 0 8px;" align="middle" src="img/ico1.png"/></a></div>
        </h1>
      
        
         <div class="CasesCitation" style="height:750px;  overflow:auto;">
                
              </div>
        
      </div>
    </div>
    <div id="contRight" style="margin-left:300px;">
    
    <section class="resultsRight">
    
    <div class="clear"></div>
     <div  style="background-color:#757575;font-size:14px; padding-top: 3px; padding-bottom:32px; border-left-style: none; border-right-style: none;">
    
    <div class="leftResultBox" style="float:left; margin-top:2px;">
     <select id="mapType" name="sortResult" class="advBoxS5" style=" height:20px; margin:0 5px; width:180px;">
     <option value="0">case refers to</option>
            <option value="1">case referred by</option>
    </select>
     <span class="tfont2">Search within results</span>
    <input type="text"  id="SearchIn" class="advBoxS6" style=" height:20px; margin:0 5px; width:180px;"/>
    <input type="button"  id="btnSearch" value="Search" class="btnSave" style="width:120px; margin-left:5px;"/>
    <input type="button"  id="btnClear" value="Reset" class="btnSave" style="width:100px; margin-left:5px; display:none;"/>
                             <button id="next"  class="hidden">Next</button>
    </div>

      <div class="rightResultBox">
    <div class="funtionResults">
       <a href="#" id="LeftExpand"><img border="0" src="GUI/NewDesign/img/iconListOfPreamble.png"/>List</a>
    </div>
     </div>


    </div>
    <div class="topBar1" 
            style="padding-top: 5px; padding-bottom:25px; border-left-style: none; border-right-style: none; border-bottom:solid 1px #C5D5E2; ">
              <div class="leftResultBox" style="float:left;margin-left:4px">
               <div class="funtionResults" style="margin-right:5px;font-size:12px">
               <span style="float:left; display:block; padding:5px 5px 0 0;" id="RefinS">
               </span>
             </div>
              </div></div>
        <div class = "clear"></div>
      		<svg id="cloud" class = "svg" width="95%" height="750"></svg>
    <%--<button id="prev"  class="hidden">Previous</button>--%>
          

        <div id="start" class="hidden" style="display:none;">
            <button>Start</button>
        </div>
        
        <div id="dbData" class="hidden">
        </div>
        <p id="orientation" style="display:none"><%=dataType %></p>
        <p id="P1" style="display:none"><%=NewSearch %></p>
        <p id="P2" style="display:none"><%=SearchFiled%></p>
           <p id="P3" style="display:none"><%=classicVsMod%></p>
        <!-- Piwik -->


      </section>
    
    <div class="clear"></div>
     <div class='legend-scale' style=" padding-right:5px;" >
   <ul class='legend-labels' style="text-align:center;">
   <li><span style='background:#009900;'>Applied/ Appealed/ Followed</span></li>
    <li><span style='background:#FF9933;'>Referred Cases</span></li>
    <li><span style='background:#FF0000;'>Distinguished/ Not Followed/ Overruled</span></li>
    </ul>
        </div>
<span id=checkin>. </span>
  </div>

  </div>
           
        <script src="js/script.js" type="text/javascript"></script>
        <script type="text/javascript">
            var _paq = _paq || [];
            _paq.push(["setDocumentTitle", document.domain + "/" + document.title]);
            _paq.push(["setCookieDomain", "*.www.elaw.my"]);
            _paq.push(["trackPageView"]);
            _paq.push(["enableLinkTracking"]);

            (function () {
                var u = (("https:" == document.location.protocol) ? "https" : "http") + "://elaw.my/analytics/";
                _paq.push(["setTrackerUrl", u + "piwik.php"]);
                _paq.push(["setSiteId", "1"]);
                var d = document, g = d.createElement("script"), s = d.getElementsByTagName("script")[0]; g.type = "text/javascript";
                g.defer = true; g.async = true; g.src = u + "piwik.js"; s.parentNode.insertBefore(g, s);
            })();
            function btnClear_onclick() {

            }

        </script>
<!-- End Piwik Code -->
	 </body>
</html>