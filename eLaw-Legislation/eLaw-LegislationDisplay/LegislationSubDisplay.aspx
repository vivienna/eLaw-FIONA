<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.LegislationSubDisplay1" CodeFile="LegislationSubDisplay.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<link rel="stylesheet" href="css/style_New.css" />
<link href="css/cass.css" rel="stylesheet" type="text/css" />

		<title>eLaw - Legislation Search Result</title>
		<link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
		<script src="js/libs/jquery-1.7.1.min.js" type="text/javascript"></script>
		<title> Legislation - Search Result</title>
        <script src="include/css-pop.js" type="text/javascript"></script>
		<script type="text/javascript">

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
        
        </script>
        <style type="text/css">
        .SDclear
        {color:#736F6E;font-family:Arial, Helvetica, sans-serif;font-size:13px;font-weight:500;margin:0 0 0 8px; padding-left:10px;}
     
        .gotoPage span a
        {
            float:none;
            }
        </style>
	</head>
<body >
	
	
	
	
		<form id="Form2" method="post" runat="server">

		<div class="header">

<div class="home"><a  href="index1.aspx"><img border="0" src="img/logoHome.png" align="middle"/></a></div>
 
 <div class="menu">
 <ul>
 
 <li ><a href="casesSearch.aspx">CASE LAW</a></li>
 <li class="current"> <a href="legislationSearch.aspx">LEGISLATION</a></li>
 </ul>
   
 </div>

 <div class="nav3">
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
     <li style="border:none;"><a href="#">Profile</a></li>
     <li><a href="#">Setting</a></li>
      <li><a  href="AccountDetail.aspx">Account Detail</a></li>
            <li><a  href="changePassword.aspx">Change Password</a></li>
            <li><a  href="UpdateAddress.aspx">Update Address</a></li>
            <li><a  href="AccountUsage.aspx">Account Usage</a></li>
	 </ul>
 	</li>
 </ul>
 <span class="twelcome">Welcome Back! <strong><asp:label  id="lbl_username" runat="server" ></asp:label></strong> </span>
 </div>
 
 
<div class="clear"></div>
</div>
<div class="clear"></div>
<!--close Head-->
<div id="content3">
	
    <!-- Box Search Here -->
    
    <div class="wrapperContCase" style="margin-bottom:100px;">
    
    <div class="topBar4" 
            style=" border-left-style: none; border-right-style: none;">
    
    <div class="leftResultBox" 
              
              style="padding-left: 50px;  width:100%" 
              align="left"><span class="tfont1"><asp:label id="lblPgNo" 
            runat="server" Font-Bold="True" Visible="False"></asp:label></span>About 
                 <span class="tfont1"> &nbsp;<asp:label id="lblLegislationFound1" runat="server" 
            EnableViewState="False"></asp:label></span>

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
                
 </div>
    
    <!-- Right Bar1 Results Elements -->
    <div class="clear"></div>
    </div>
    <!-- Nav Tabs -->
       
        
    	  <div class="topBar2">
    
    <div class="leftResultBox">
    <span class="tfont4">Sort results by</span>
    <asp:dropdownlist ID="ddlTitle"  runat="server" class="advBoxS5" 
            style=" height:25px; margin:0 5px; width:180px;" AutoPostBack="True"></asp:dropdownlist>
	  <span class="tfont4">Search Within Result</span>
      <asp:textbox  id="txtSearch" runat="server"  ToolTip="Search Within these documents"
                    MaxLength="200" 
                    class="advBoxS1"  
                    ValidationGroup="submit"> </asp:textbox>
        <%-- <asp:TextBox ID="TextBox2" runat="server" class="advBoxS5" style="margin:0px 0 5px"></asp:TextBox>--%>&nbsp;<asp:button id="btnSearch" runat="server" Text="Search" class="btnSave" 
                      style=" width:120px; " ValidationGroup="submit"></asp:button>
              </div>
              <div class="rightResultBox funtionResults" style="cursor:pointer"><a onclick="popup('popupDiv2')"><img border="0" src="img/icoSave.gif"/>Save Search</a></div>
    <div class="clear"></div>
  </div>
        
        <!-- Box Results 1 -->
    
    <asp:label id="lblTb" runat="server" Width="100%"></asp:label>
    
    
   
        
    	  <div class="clear">
                 <asp:label id="lblTb1Jquery" runat="server"></asp:label>
                </div>
    

  </div>
    
    <div class="clear"></div>
</div>

<footer> 
	<div class="wrapperFooter">
	<section class="wrapperAddfooter">

    <aside class="gotoPage" style="margin-left:0;width:100%">

          <asp:label style="width:100%;text-align:center;" id="lblBottomNavigation" runat="server"></asp:label>

    </aside>
    
    <div class="clear"></div>
    </section>
	<aside class="copyrightfooter">Copyright © <% Response.Write(DateTime.Now.Year)%> Elaw. All rights reserved. <a href="#">Terms of Use</a> | <a href="#">Privacy Policy</a></aside>
    </div>
</footer>
		        <div id="blanket" style="display:none;"></div>
	        <div id="popupDiv2" style="display:none;">
	
	 
            <div id='form3'>	
	        <h3><span>Save Search Result</span></h3>
                    <p> <span id='Span1'>Name Your Search Result :</span> </p>
			        <fieldset id="fieldset1">
			        <p><label for="message">Result Name</label>
			        <!--<input type="text" name="subject" id="saveWithName" maxlength="2" size="40" />-->
                    <asp:TextBox runat="server" Width="195px" name="subject" id="saveWithName" size="40"></asp:TextBox>
			        </p>

			        <p class='submit'><button  type='button' id='saveSearch' runat="server">Save</button>
                    <button  type='button' value='Submit' id='Button4' onclick="popup('popupDiv2')">Cancel</button>
                    </p>
                    </fieldset>
                    </div>
	        </div>
            
            	
		</form>
        <script src="js/script.js" type="text/javascript"></script><!-- Piwik -->
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
