<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.PracticeNotesSearchresult" CodeFile="PracticeNotesSearchresult.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<link href="css/style_New.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="include/jquery.min.js"></script>
<script type="text/javascript" src="include/tytabs.jquery.min.js"></script>
<script src="js/libs/jquery-1.7.1.min.js" type="text/javascript"></script>
<script src="js/script.js" type="text/javascript"></script>
<script type="text/javascript">
<!--
    $(document).ready(function () {
        $("#tabsholder").tytabs({
            tabinit: "1",
            fadespeed: "fast"
        });
    });
-->
</script>
<title> Practice Notes - Search Result</TITLE>


<!--[if IE]>
<style>
    #nav li ul { display: none; }
    #nav li:hover ul, #nav li.over ul {display: block; }
    #nav li ul li { height: 30px; line-height: 30px; }
    </style>
<![endif]-->
<!--[if lte IE 6]>
    <script type="text/javascript">
        startList = function() {
            if (document.all && document.getElementById) {
                var navRoot = document.getElementById("nav");
                for (i=0; i<navRoot.childNodes.length; i++) {
                    var node = navRoot.childNodes[i];
                    if (node.nodeName=="LI") {
                        node.onmouseover=function() {
                            this.className+=" over";
                        }
                        node.onmouseout=function() {
                            this.className=this.className.replace(" over", "");
                        }
                    }
                }
            }
        }
        window.onload=startList;
   </script>
<![endif]-->

</head>
<body>

<form id="Form1" method="post" runat="server">

<!-- #include file="include/headerMain3.aspx" -->


<div id="content3">
	
    <!-- Box Search Here -->
    
    <div  style="margin-bottom:100px;width:100%;">
    
    <div class="topBar4" 
            style=" border-left-style: none; border-right-style: none;">
    
    <div class="leftResultBox" 
              
              style="padding-left: 50px;  width:100%" 
              align="left"><span class="tfont1"><asp:label id="lblPgNo" 
            runat="server" Font-Bold="True" Visible="False"></asp:label></span>About 
                  <asp:label id="lblLegislationFound2" runat="server" EnableViewState="False"></asp:label>

 <span class="tfont1">
    <asp:label id="Label1" runat="server" Visible="False"></asp:label></span>(<asp:label id="lblSec" runat="server" ></asp:label>s)
       <asp:label id="lblMsg" runat="server"></asp:label>
            
            
                    <asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" 
								ValidationExpression="[A-Z a-z 0-9 &amp; . ' &quot; _ - \s]*" 
								ErrorMessage="Invalid Search Words" 
								ControlToValidate="txtSearch"
								Display="Dynamic" 
								SetFocusOnError="True"></asp:regularexpressionvalidator>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                  ErrorMessage="Please Type Information " ControlToValidate="txtSearch" 
                  Display="Dynamic" ValidationGroup="submit"></asp:RequiredFieldValidator>
                
        <asp:button id="btnAct" runat="server" Text="Act" class="btnWhite" 
                      Visible="False"></asp:button>
 </div>
    
    <!-- Right Bar1 Results Elements -->
    <div class="clear"></div>
    </div>
    <!-- Nav Tabs -->
       
        
    	  <div class="topBar2" style="width:99%">
    
    <div class="leftResultBox">
    <span class="tfont4">Sort results by</span>
    <asp:dropdownlist ID="ddlTitle"  runat="server" class="advBoxS5" style=" height:25px; margin:0 5px; width:180px;">
    <asp:ListItem>Title Ascending </asp:ListItem>
    <asp:ListItem>Title Descending</asp:ListItem>
    <asp:ListItem>Relevance</asp:ListItem>
    
    </asp:dropdownlist>
	  <span class="tfont4">Search Within Result</span>
      <asp:textbox  id="txtSearch" runat="server"  ToolTip="Search Within these documents"
                    MaxLength="200" 
                    class="advBoxS1"  
                    ValidationGroup="submit">
                    </asp:textbox>
	    <%-- <asp:TextBox ID="TextBox2" runat="server" class="advBoxS5" style="margin:0px 0 5px"></asp:TextBox>--%>
	  
	     &nbsp;<asp:button id="btnSearch" runat="server" Text="Search" class="btnSave" 
                      style=" width:120px; " ValidationGroup="submit"></asp:button>
              </div>
    <div class="clear"></div>
  </div>
        
        <!-- Box Results 1 -->
    
    <asp:label id="lblTbl" runat="server" Width="100%"></asp:label>
    
    
   
        
    	  <div class="clear"></div>
    

  </div>
    
    <div class="clear"></div>
</div>


<footer> 
	<div class="wrapperFooter">
	    <section class="wrapperAddfooter">
    
    
    
        <!--<aside class="gotoPage" >
          <div class="gotoBox1" style="padding: 0px; margin-left: 35px">
           <span class="tfont2">
         New Search</span>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                  ControlToValidate="newSearchText" Display="Dynamic" ErrorMessage="*" 
                  Font-Size="Large" ForeColor="Red" ValidationGroup="new"></asp:RequiredFieldValidator>
          <asp:textbox  runat="server"  ToolTip="make a new search query" MaxLength="200" class="advBoxS5" ID="newSearchText"></asp:textbox>
          <asp:button id="newSearchButton" runat="server"  Text="Search" class="btnSave1" ValidationGroup="new"></asp:button>&nbsp;&nbsp;</div>
          <asp:label id="lblBottomNavigation" runat="server"></asp:label>
         </aside>-->
    
    <div class="clear"></div>
    </section>
	
    <!-- #include file="include/footerMain.aspx" -->

    </div>
</footer>





<!-- <TD vAlign="top" align="right" width="375" height="16"><SPAN class="style3"><asp:label id="lblTopNavigation" runat="server" Width="95%" Font-Names="Verdana" Font-Size="X-Small"
ForeColor="Black" BackColor="Transparent" BorderColor="White"></asp:label></SPAN></TD> -->

<!-- <TR><TD vAlign="top" width="89%"><asp:label id="lblSortBy" runat="server" Width="70%"></asp:label></TD></TR> -->

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





























