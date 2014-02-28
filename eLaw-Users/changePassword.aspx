<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.changePassword" CodeFile="changePassword.aspx.vb" %>
<%@register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<title>eLaw Account Details</title>
<meta name="keywords" content="Elaw">
<meta name="description" content="Elaw">
<link rel="stylesheet" href="css/style_New.css">
<script src="js/libs/jquery-1.7.1.min.js"></script>

    <script type="text/javascript">
        
        $(document).ready(function () {
            $('#reset').click(function () {
                $('#txtOldPassword,#txtNewPassword,#txtReTypePassword').attr('value', '').css('background-color', 'white');
            });
            $('.validationLogIn').text().trim().length > 0 ? $('.validationLogIn').show() : $('.validationLogIn').hide();
            
        });
    </script>
</head>

<body>

<div class="header">

<div class="home"><a  href="index1.aspx"><img border="0" src="img/logoHome.png" align="middle"/></a></div>
 
 <div class="menu">
 <ul>
 
 <li ><a href="casesSearch.aspx">CASE LAW</a></li>
 <li> <a href="legislationSearch.aspx">LEGISLATION</a></li>
 </ul>

 <!-- Drop btn1 -->
 
 <ul id="nav1"><li><a href="#">MORE <img style="margin-left:8px;" src="img/arrowDown.png" width="18" height="15" align="top" border="0" /></a>
     <ul>
      <li style="border:none;"><a href="formSearch.aspx">Forms</a></li>
     <li> <a href="case_notes/user_cases.aspx">myBriefcase</a></li>
     <li> <a href="lawDictionary.aspx">Dictionary</a></li>
     <li> <a href="practiceNotesSearch.aspx">Practice Notes</a></li>
     <li> <a href="help/index.html" target="_blank"  style="border-right:none;">Help</a></li>
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
            <!---<li><a  href="AccountUsage.aspx">Account Usage</a></li>--->
	 </ul>
 	</li>
 </ul>
 <span class="twelcome">Welcome Back! <strong><asp:Label id="lbl_username" runat="server" ></asp:Label></strong> </span>
 </div>
 
 
<div class="clear"></div>
</div>


 <!-- Content 2 -->
<div id="content3">
    <!-- Box Search Here -->
    
    <div class="wrapperContCase" style="padding:0 0 60px 0; width:900px;">
    
    <!-- Nav Tabs -->
    <div class="clear"></div>
	  <nav class="wrapperTabs" style="margin-top:100px;"></nav>
    
    	<div class="caseBox" style="padding:20px;">
        
        <!-- Left Log Details -->
        <div class="leftBoxUser2">
        <div class="sosialProfile">
      <!--  <a href="http://twitter.com/eLawMalaysia" target="_blank"><img style="margin-right:5px;" src="img/twit.png" border="0"/></a>
        <a href="http://www.facebook.com/elaw.digitallibrary" target="_blank"><img style="margin-right:5px;" src="img/fb.png" border="0"/></a>
        <a href="http://my.linkedin.com/pub/the-digital-library/80/31a/b51" target="_blank"><img src="img/in.png" border="0"/></a>-->
        </div>
        
        <span class="twelcome2">Hi, <a href="#"><asp:label id="fullName" runat="server"></asp:label></a></span><br>
        <span class="twelcomeDay"> <%= Date.Today.DayOfWeek.ToString %> </span>
        <span class="twelcomeDate"> <%= Date.Today.Date.ToString("d MMM yyyy")%> </span> 
        <span class="twelcomeDay">(<%= Date.Now.ToString("hh:mm tt") %>)</span>      
        
        <div class="editUser">
        <ul>
        <li><a href="AccountDetail.aspx">My Account</a></li> |
        <li class="current">Change Password</li> |
        <li><a href="UpdateAddress.aspx">Update Address</a></li>
        </ul>
        </div><br/>
        
        <div class="dataUsage">
      <!--  <div class="dataUsageL">
        <span class="tfont6">Data usage left for today</span> <span class="tfont1">0 MB</span> <span class="tfont7">(Limit 20MB)</span></div>-->
        <!--<div class="btnProR"><input type="submit" name="search" id="button3" value="Upgrade Pro" class="btnPro"/></div>--><div class="clear"></div>
        </div><br/>   
        
        <span class="tfont6"><strong>Change Password</strong></span><br />
       <span class="tGrey" style="font-size:13px;">*Passwords are case-sensitive and must be at least 6 characters.<br />
         *A good password should contain a mix of capital and lower-case letters, numbers and symbols.</span>
        
        <div class="validationLogIn" style="display:none">
        <asp:label ID="lblMsg" runat="server"></asp:label>
         <%--    	There were one or more errors in your submission. <br />
         Please correct the marked fields below.--%>
         <div class="clear"></div>
    	</div>
        <form id="frm" method="post" runat="server">
        <table width="100%" border="0" cellpadding="2" cellspacing="2">
          <tr>
            <td width="22%" class="tTextNorm">Old Password</td>
            <td width="25%"><asp:TextBox TextMode="Password" name="textfield2" id="txtOldPassword" class="advBoxS6" runat="server"></asp:TextBox><%--<input type="text" name="textfield2" id="phrase" class="advBoxS6">--%></td>
            <td class="tRed2">Please enter a value.</td>
          </tr>
          <tr>
            <td class="tTextNorm">New Password</td>
            <td><asp:TextBox TextMode="Password" name="phrase" id="txtNewPassword" class="advBoxS6" runat="server"></asp:TextBox><%--<input type="text" name="phrase" id="phrase2" class="advBoxS6"/>--%></td>
            <td class="tRed2">Please enter a value.</td>
          </tr>
          <tr>
            <td class="tTextNorm">Confirm New Password</td>
            <td><asp:TextBox TextMode="Password" name="phrase3" id="txtReTypePassword" class="advBoxS6" runat="server"></asp:TextBox><%--<input type="text" name="phrase2" id="phrase3" class="advBoxS6"/>--%></td>
            <td class="tRed2">Please enter the same password as above.</td>
          </tr>
          <tr>
            <td class="tTextNorm">&nbsp;</td>
            <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="41%"><asp:button type="button" name="button" id="btnChangePassword" Text="Change" class="btnSave" style=" width:145px;" runat="server"/></td>
                <td width="59%"><input type="reset" name="button"   id="reset" onclick="resetAll()" value="Reset" class="btnWhite" style=" width:100px; margin-left:6px;" /></td>
              </tr>
            </table></td>
            <td class="tRed2">&nbsp;</td>
          </tr>
        </table>
        </form>
<div class="clear"></div>
        <div class="clear"></div>
        </div>
        
        <!-- right Log Details -->

        <div class="clear"></div>
        </div>

  </div>
    
    <div class="clear"></div>
</div>

 <!-- footer -->

<footer> 
	<div class="wrapperFooter">
	<aside class="copyrightfooter">
    Copyright © <% Response.Write(DateTime.Now.Year)%> The Digital Library Sdn. Bhd. All rights reserved. 
    <a href="http://www.elaw.my/content/termsCondition.aspx" target="_blank">Terms of Use</a> | 
    <a href="http://www.elaw.my/content/privacy.aspx" target="_blank">Privacy Statement</a></aside>
    </div>
</footer>
  
<script src="js/script.js"></script>


<!-- Asynchronous Google Analytics snippet. Change UA-XXXXX-X to be your site's ID.
       mathiasbynens.be/notes/async-analytics-snippet -->
<script>
    var _gaq = [['_setAccount', 'UA-21306573-1'], ['_trackPageview']];
    (function (d, t) {
        var g = d.createElement(t), s = d.getElementsByTagName(t)[0];
        g.src = ('https:' == location.protocol ? '//ssl' : '//www') + '.google-analytics.com/ga.js';
        s.parentNode.insertBefore(g, s)
    } (document, 'script'));
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
