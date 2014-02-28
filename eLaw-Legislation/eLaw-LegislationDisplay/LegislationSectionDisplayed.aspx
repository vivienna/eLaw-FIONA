<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.LegislationSectionDisplayed1" CodeFile="LegislationSectionDisplayed.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

   
   
<link rel="stylesheet" href="css/style_New.css" />
    <script src="js/libs/jquery-1.7.1.js" type="text/javascript"></script>
     <script src="include/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <link href="css/AutoComplate.css" rel="stylesheet" type="text/css" />
    
		
		<title>eLaw  Legislation - Section Display</title>
        <link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
	<script type="text/jscript">

	    $(document).ready(function () {
	        ////////////////////////section No
           
	        
	        //////////////////////
	        //min font size
	        var min = 12;

	        //max font size
	        var max = 18;

	        //grab the default font size
	        var reset = $('p').css('fontSize');

	        //font resize these elements
	        var elm = $('div.intro, p.ending');

	        //set the default font size and remove px from the value
	        var size = str_replace(reset, 'px', '');

	        //Increase font size
	        $('a.fontSizePlus').click(function () {

	            //if the font size is lower or equal than the max value
	            if (size <= max) {


	                //increase the size
	                size++;

	                //set the font size
	                elm.css({ 'fontSize': size });
	            }

	            //cancel a click event
	            return false;

	        });

	        $('a.fontSizeMinus').click(function () {

	            //if the font size is greater or equal than min value
	            if (size >= min) {

	                //decrease the size
	                size--;

	                //set the font size
	                elm.css({ 'fontSize': size });
	            }

	            //cancel a click event
	            return false;

	        });

	        //Reset the font size
	        $('a.fontReset').click(function () {

	            //set the default font size	
	            elm.css({ 'fontSize': reset });
	        });
	        ////////////////////////////////////////////////
	        //Print page 
	        window.printItn = function () {

	            var printContent = document.getElementById('mydiv');

	            var windowUrl = 'about:blank';
	            var uniqueName = new Date();
	            var windowName = 'Print' + uniqueName.getTime();

	            //  you should add all css refrence for your html. something like.

	            var WinPrint = window.open(windowUrl, windowName, 'left=30,top=30,right=500,bottom=500,width=700,height=500');
	            WinPrint.document.write('<' + 'html' + '><head><link href="../css/style_New.css" rel="stylesheet" type="text/css" /><style> #printdiv { display:none;}</style></head><' + 'body style="background:none !important"' + '>');
	            WinPrint.document.write(printContent.innerHTML);

	            WinPrint.document.write('<' + '/body' + '><' + '/html' + '>');
	            WinPrint.document.close();
	            WinPrint.focus();
	            WinPrint.print();
	            WinPrint.close();
	        }
	        //////////////////////////////////////////

	    });

	    //A string replace function
	    function str_replace(haystack, needle, replacement) {
	        var temp = haystack.split(needle);
	        return temp.join(replacement);
	    }
	     ///////////////////////
         function LoadContent(id) {
            
             $.ajax({
                 type: "POST",
                 url: "legislationSectionDisplayed.aspx/getSectionCont",
                 data: '{SecReceiv: "' + id + '" }',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                    
                     $("#lblXml").html('');
                     $("#lblXml").html(data.d);
                                     },
                 failure: function (response) {
                    
                 }
             });

         }
         //////////////////////////
	   
</script>
<style >
 .hWord
         {
            background-color: yellow;
            font-weight:bold;
            color:Red;
             }
        
</style>
</head>
<body>

<form id="Form1" method="post" runat="server">
		

<!-- #include file="include/headerMainLegis.aspx" -->


<!-- Content 2 --> 
<div id="content2" >


    <div id="showPanel">
	<div class="leftQuicksearch">
    <div class="btnShowSearch"><img style="margin:8px 0 0 8px;" align="top" src="img/ico2.png"/></div>
    <p>Browse</p></div>
    </div>
	<div id="colleft1">	
	<div id="panel">	
	
    <div id="tabbed_box" class="tabbed_box">
    <div class="tabbed_area">
        <ul class="tabs">
          <li><a href="javascript:tabSwitch_2(1, 4, 'tab_', 'content_');" id="tab_1" class="active"><img align="middle" src="GUI/NewDesign/img/iconSectionNo.png" alt="View all Sections"/></a></li>
            <li><a href="javascript:tabSwitch_2(2, 4, 'tab_', 'content_');" id="tab_2"><img align="middle" src="GUI/NewDesign/img/iconSearch.png" alt="Search"/></a></li>
            <li><a href="javascript:tabSwitch_2(3, 4, 'tab_', 'content_');" id="tab_3">Sub</a></li>
			<li><a href="javascript:tabSwitch_2(4, 4, 'tab_', 'content_');" id="tab_4">Amd</a></li>
			<li><a href="#" id="hidePanel"><img align="middle" src="img/ico1.png"/></a></li>
        </ul>    
        <div id="content_1" class="content">
        	<ul>
            	<%Response.Write(ListSection) %>
			</ul>
            
                    </div>
        <div id="content_2" class="content">:
        	        	<table width="100%" border="0" cellpadding="4" cellspacing="1" style="color:#3e4346;">
            
  <tr>
 
          <td><asp:TextBox ID="txtFTS" runat="server" class="advBoxS"></asp:TextBox>
</td>
        </tr>
        <tr>
          <td>Search Type:</td>
        </tr>
        <tr>
          <td><asp:DropDownList ID="rbs" runat="server" class="advBoxS" style=" height:25px;  ">
              <asp:ListItem Selected="True" Value="1">Full Document</asp:ListItem>
                      <asp:ListItem Value="2">Act Title</asp:ListItem>
                      <asp:ListItem Value="3">Section Title</asp:ListItem>

              </asp:DropDownList>
            </td>
        </tr>
        
        <tr>
          <td>Without these words:</td>
        </tr>
        <tr>
          <td><asp:TextBox 
                  ID="txtNotCases" runat="server" class="advBoxS"></asp:TextBox>
            </td>
        </tr>

        <tr>
          <td><table width="100%" border="0" cellpadding="5" cellspacing="0">
            <tr>
              <td width="25%">Within</td>
              <td width="75%" colspan="2"><asp:DropDownList 
                          ID="ddlproxmity" runat="server" CssClass="advBoxS6" Width="160px">
                      <asp:ListItem Value="3"> Same Page</asp:ListItem>
                      <asp:ListItem Value="2"> Same Paragraph</asp:ListItem>
                      <asp:ListItem Value="1"> Same Sentence</asp:ListItem>
                      </asp:DropDownList></td>
            </tr>

            <tr>
              <td width="25%">Within</td>
              <td width="50%" ><asp:dropdownlist id="ddlprox" runat="server" 
       class="advBoxS6" style=" height:25px; width:100px;" EnableViewState="False">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="100">10</asp:ListItem>
                    <asp:ListItem Value="200">20</asp:ListItem>
                    <asp:ListItem Value="300">30</asp:ListItem>
                    
                      </asp:dropdownlist></td>
                      <td>Words</td>
            </tr>
            
          </table></td>
        </tr>
        <tr>
          <td align="center" ><div class="btnDefine" onclick="validate()">Search<asp:LinkButton ID="LinkButton1" runat="server" style="display:none;">Search</asp:LinkButton>
          </div></td>
        </tr>
      </table>
        </div>
        <div  id="content_3" class="content">
        	
        </div>
        <div id="content_4" class="content">
        	
        </div>
    
    </div></div>
	











      
	
    

    
   
        
    
    
	</div>  
	</div>
    <!-- Box Search Here -->
    

    <div id="contRight">
    
    <section class="resultsRight">&nbsp;





    
    &nbsp;<!-- Nav Tabs --><div class="clear"></div>
		
    <div class="topBar2">
    
    <div class="leftResultBox2">
    <div class="funtionResults">
   
                 <%Response.Write(LegilationNavgation)%>
    </div>
    </div>
   

    



    <div class="rightResultBox">
    <div class="funtionResults"> 
  
   
     <span id="EmailCheck" style=" font-size:14px;"> <%Response.Write(ActNot)%> </span>
    <a href="#" id="print" onclick='printItn()'><img border="0" src="img/icoPrint.gif"/>Print</a>
    
  
   
    </div>
     </div>
    
    <div class="clear"></div>
    </div>
    	<section class="boxDisplay">
        
       
    	  
        <div id="mydiv">
        <div class="clear" align="center" style="padding-top: 5px; padding-bottom: 16px" ><asp:label id="lblMsg" runat="server"></asp:label>&nbsp;</div>
        <asp:label id="lblPageTop" runat="server"></asp:label>
        <asp:label id="lblXml" runat="server"></asp:label>
       
   
        </div>
    	  <div class="clear"></div>
    	</section>

  </div>
    
    <div class="clear"></div>
</div>

 <!-- footer -->

<footer> 
	<div class="wrapperFooter">
	
	<!-- #include file="include/footerMain.aspx" -->

    </div>
</footer>

			 
<script src="js/script.js" type="text/javascript"></script>
		</form>
         <script src="js/script.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(document).ready(function () {
           $('.content ul').height($(window).innerHeight() * 0.9 - 110);
           $("#panel").animate({ marginLeft: "-266px" }, 5);
           //$("#colleft").animate({ width: "0px", opacity: 0 }, 4
           $("#colleft1").css({ 'width': '0px', 'opacity': '0' });
           $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
           $("#contRight").animate({ marginLeft: "50px" }, 5);
           $("#hidePanel").click(function () {

               $("#panel").animate({ marginLeft: "-266px" }, 500);
               $("#colleft1").animate({ width: "0px", opacity: 0 }, 400);
               $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
               $("#contRight").animate({ marginLeft: "50px" }, 500, function () {
                   if ($("#printing").html("<img src='img/icoPrint.gif' border='0'>")) {
                       $("#printing").html("<img src='img/icoPrint.gif' border='0'>Print");
                       $("#emailing").html("<img src='img/icoEmail.gif' border='0'>Email");
                       $("#Download").html("<img src='img/icoDownload.gif' border='0'>Download");
                       $("#SaveS").html("<img src='img/icoSave.gif'border='0'>Save Search");
                   }

               });


           });
           $("#showPanel").click(function () {
               $('.content ul').height($(window).innerHeight() * 0.9 - 110);
               tabSwitch_2(1, 4, 'tab_', 'content_');

               $("#contRight").animate({ marginLeft: "280px" }, 200);
               $("#panel").animate({ marginLeft: "0px" }, 400);
               $("#colleft1").show().animate({ width: "266px", opacity: 1 }, 400).show("slow");
               $("#showPanel").animate({ width: "0px", opacity: 0 }, 600).hide("slow");
               $("#colleft1").css({ 'top': '16px', 'position': 'fixed' });
               if ($("#printing").html("<img src='img/icoPrint.gif' border='0'>Print")) {
                   $("#printing").html("<img src='img/icoPrint.gif' border='0'>");
                   $("#emailing").html("<img src='img/icoEmail.gif' border='0'>");
                   $("#Download").html("<img src='img/icoDownload.gif' border='0'>");
                   $("#SaveS").html("<img src='img/icoSave.gif'border='0'>");
               }

           });




       });
       /////////////////////////////////////////////////
       function tabSwitch_2(active, number, tab_prefix, content_prefix) {

           for (var i = 1; i < number + 1; i++) {
               document.getElementById(content_prefix + i).style.display = 'none';
               document.getElementById(tab_prefix + i).className = '';
           }
           document.getElementById(content_prefix + active).style.display = 'block';
           document.getElementById(tab_prefix + active).className = 'active';

       }

       /////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////64
       function validate() {
           if ($('#txtFTS').val().trim().length == 0) {
               $('#txtFTS').css('background-color', '#ffd0d0');
              
           } else {
               $('#txtFTS').css('background-color', '#ffffff');

               __doPostBack('LinkButton1', '');
           }
       }
         </script><!-- Piwik -->
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
