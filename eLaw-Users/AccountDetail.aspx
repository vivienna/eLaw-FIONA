<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.AccountDetail1" CodeFile="AccountDetail.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<title>eLaw Account Details</title>
<meta name="keywords" content="Elaw">
<meta name="description" content="Elaw">
<link rel="stylesheet" href="css/style_New.css">
<script src="js/libs/jquery-1.7.1.min.js"></script>
</head>

<body>

    <form id="form1" runat="server">

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
           <!-- <li><a  href="AccountUsage.aspx">Account Usage</a></li>-->
	 </ul>
 	</li>
 </ul>
 <span class="twelcome">Welcome Back! <strong><asp:Label id="lbl_username" runat="server" ></asp:Label></strong> </span>
 </div>
 
 
<div class="clear"></div>
</div>


 <!-- Content 2 -->
 
<div id="content4">
	
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
        
        <span class="twelcome2">Hi, <a href="#"><asp:label id="fullName" runat="server"></asp:label></a></span><br/>
        <span class="twelcomeDay"> <%= Date.Today.DayOfWeek.ToString %> </span>
        <span class="twelcomeDate"> <%= Date.Today.Date.ToString("d MMM yyyy")%> </span> 
        <span class="twelcomeDay">(<%= Date.Now.ToString("hh:mm tt") %>)</span>      
        
        <div class="editUser">
        <ul>
        <li class="current">My Account</li> |
        <li><a href="changePassword.aspx">Change Password</a></li> |
        <li><a href="UpdateAddress.aspx">Update Address</a></li>
        </ul>
        </div><br>
        
        <div class="dataUsage">
       <!-- <div class="dataUsageL">
        <span class="tfont6">Data usage left for today</span> <span class="tfont1">0 MB</span> <span class="tfont7">(Limit 20MB)</span></div>-->
        <!--<div class="btnProR"><input type="submit" name="search" id="button3" value="Upgrade Pro" class="btnPro"/></div>--->
        <div class="clear"></div>
        </div>
        
        <br />
        
        <div class="leftAdvCaseLaw2">
        <table width="98%" border="0" cellpadding="2" cellspacing="2">
          <tr>
            <td width="32%" class="tTextNorm"><span class="tfont6"><strong>Personal Details</strong></span></td>
            <td width="68%">&nbsp;</td>
          </tr>
          <tr>
            <td class="tTextNorm">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="tTextNorm">User Name</td>
            <td class="tTextNorm"><asp:Label id="userNameID" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">First Name</td>
            <td class="tTextNorm"><asp:Label id="firstName" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Last Name</td>
            <td class="tTextNorm"><asp:Label id="lastName" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Firm Name</td>
            <td class="tTextNorm"><asp:Label id="firmName" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Registration Date</td>
            <td class="tTextNorm"><asp:Label id="regDate" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Address 1</td>
            <td class="tTextNorm"><asp:Label id="addr1" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Address 2</td>
            <td class="tTextNorm"><asp:Label id="addr2" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">City</td>
            <td class="tTextNorm"><asp:Label id="city" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">State</td>
            <td class="tTextNorm"><asp:Label id="state" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Postal/ZIP Code</td>
            <td class="tTextNorm"><asp:Label id="PCode" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Country</td>
            <td class="tTextNorm"><asp:Label id="country" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Phone Number </td>
            <td class="tTextNorm"><asp:Label id="phone" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Fax Number</td>
            <td class="tTextNorm"><asp:Label id="fax" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Email : </td>
            <td class="tTextNorm"><asp:Label id="email" runat="server"></asp:Label></td>
          </tr>
        </table>
		</div>
        
        <div class="rightAdvCaseLaw2">
        <table width="100%" border="0" cellpadding="2" cellspacing="2">
          <tr>
            <td width="41%" class="tTextNorm"><strong>Subscription Details</strong></td>
            <td width="59%">&nbsp;</td>
          </tr>
          <tr>
            <td class="tTextNorm">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="tTextNorm">Subscription</td>
            <td class="tTextNorm"><asp:Label id="subsc" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Confirmation Date</td>
            <td class="tTextNorm"><asp:Label id="confDate" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Account Expiry Date</td>
            <td class="tRed3"><asp:Label id="expDate" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Mode Of Payment</td>
            <td class="tTextNorm"><asp:Label id="payMeth" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Card Type</td>
            <td class="tTextNorm"><asp:Label id="CardTyp" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Cheque Date</td>
            <td class="tTextNorm"><asp:Label id="chqDate" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Total Amount</td>
            <td class="tGreen"><asp:Label id="ttlAmount" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="tTextNorm">Amount Paid</td>
            <td class="tGreen"><asp:Label id="pdAmount" runat="server"></asp:Label></td>
          </tr>
        </table>
        
        </div>

        <div class="clear"></div>
        </div>
        
        <!-- right Log Details -->

        <div class="clear"></div>
        </div>

  </div>
    
    <div class="clear"></div>
</div>

 <!-- footer -->
 <div class="clear"></div>
<footer> 
	<div class="wrapperFooter">
	<aside class="copyrightfooter">
    Copyright © <% Response.Write(DateTime.Now.Year)%> The Digital Library Sdn. Bhd. All rights reserved. 
    <a href="http://www.elaw.my/content/termsCondition.aspx" target="_blank">Terms of Use</a> | 
    <a href="http://www.elaw.my/content/privacy.aspx" target="_blank">Privacy Statement</a></aside>
    </div>
</footer>
  
<script src="js/script.js"></script>


<!-- Asynchronous Google Analytics snippet. Change UA-XXXXX-X to be your site's id.
       mathiasbynens.be/notes/async-analytics-snippet -->
<script>
    var _gaq = [['_setAccount', 'UA-21306573-1'], ['_trackPageview']];
    (function (d, t) {
        var g = d.createElement(t), s = d.getElementsByTagName(t)[0];
        g.src = ('https:' == location.protocol ? '//ssl' : '//www') + '.google-analytics.com/ga.js';
        s.parentNode.insertBefore(g, s)
    } (document, 'script'));
</script>  
    </form>
<!-- Piwik -->
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
<!-- End Piwik Code --></body>
</html>
