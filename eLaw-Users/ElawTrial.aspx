<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ElawTrial.aspx.vb" Inherits="ElawTrial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Trial Account</title>
    <link rel="shortcut icon" href="GUI/NewDesign/favicon.ico" type="image/x-icon" /> 
    <link href="GUI/NewDesign/css/login.css" rel="stylesheet" type="text/css" />
    <link href="GUI/NewDesign/css/video.css" rel="stylesheet" type="text/css" >
    <script src="GUI/NewDesign/js/libs/jquery-1.7.1.js"></script>
    <script src="GUI/NewDesign/js/jquery.youtubepopup.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.1/jquery-ui.js"></script>

    <script type="text/javascript">
        $(function () {
            $("a.youtube").YouTubePopup({ autoplay: 0, hideTitleBar: true });
        });
    </script>

    <script type="text/javascript">
        ///////////////////////////////////
        function Isemail(value) {
            return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value);
        }
        function UserNamevalidation(value) {
            return /^[a-zA-Z0-9._-]{8,16}$/i.test(value);
        }
        function IntOnly(value) {
            return /^[0-9]{5,16}$/i.test(value);
            //return /^[0-9]+$/.test(value);
        }
        /////////////////////////////////////
        
        ////////////////////////////////
        
    
    
    </script> 
   
</head>

<body>
<form id="form1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

<div class="bgTrial">    
<div class="wrappeLog2">
	<div class="logoElaw"><a href="index.aspx"><img src="img/elawLogo.png"/></a></div>
    <div class="socialMedia">
    <a href="http://twitter.com/eLawMalaysia" target="_blank"><img style="margin-right:5px;" src="img/twit.png" border="0"/></a>
    <a href="http://www.facebook.com/elaw.digitallibrary" target="_blank"><img style="margin-right:5px;" src="img/fb.png" border="0"/></a>
    <a href="http://my.linkedin.com/pub/the-digital-library/80/31a/b51" target="_blank"><img src="img/in.png" border="0"/></a>
    </div>

	<div class="clear"></div>
    
  <div class="leftCont"><img src="img/bannerLog.jpg" width="488" height="161"/>
    
    <div class="logData">
    <img src="img/arrowReg.jpg"/>
    
   
    
    <asp:label id="lblErrMsg" runat="server"   EnableViewState="False" 
            class="validationLogIn" Visible="False"></asp:label>
   
    <table width="94%" border="0" cellpadding="0" cellspacing="5" style="margin-top:8px;" runat="server" id="main">
      <tr>
    <td height="27" class="tWhite">User Name </td>
    <td><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" 
                                                  ControlToValidate="us"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please enter any unique ID </span></asp:requiredfieldvalidator>

        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ErrorMessage="User Name Should Contain Only Letter And Numbers" ControlToValidate="us" 
            Display="Dynamic" ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,16})$"><span class="validationLogInTrial">User Name should contain only 8-16 letters and numbers</span> </asp:RegularExpressionValidator>



    <asp:TextBox ID="us" runat="server" class="boxLog" MaxLength="20"></asp:TextBox></td>
    <td id="mesg">&nbsp;</td>
  </tr>
  <tr>
    <td height="27" class="tWhite">Password</td>
    <td><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" 
                                                  ControlToValidate="password"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please enter your password</span></asp:requiredfieldvalidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="Password"
                                        ValidationExpression="^[a-zA-Z0-9'@&#.\s]{8,15}$"
                                                Display="Dynamic"
                                            ErrorMessage="Password must be 8 characters and have both letters and numbers" 
                                            ValidationGroup="sumarrylogin">
                                            <span class="validationLogInTrial">Password should be 8-15 characters</span>
                                            </asp:RegularExpressionValidator>

                
    <asp:TextBox ID="password" runat="server" class="boxLog" TextMode="Password" 
            MaxLength="20"></asp:TextBox></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td class="tWhite">Re-type Password</td>
    <td>
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
            ControlToCompare="password" ControlToValidate="password1" Display="Dynamic" 
            ErrorMessage="CompareValidator"><span class="validationLogInTrial">Password did not match </span></asp:CompareValidator>
        <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" 
                                                  ControlToValidate="password1"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please re-type your password</span></asp:requiredfieldvalidator>

    <asp:TextBox ID="password1" runat="server" class="boxLog" TextMode="Password" 
            MaxLength="20"></asp:TextBox></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td class="tWhite">Email</td>
    <td><asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" 
                                                  ControlToValidate="txtEmail"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please enter your email address</span></asp:requiredfieldvalidator>

        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
            ControlToValidate="txtEmail" Display="Dynamic" 
            ErrorMessage="RegularExpressionValidator" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="validationLogInTrial">Please enter correct email address</span></asp:RegularExpressionValidator>

    <asp:TextBox  runat="server" class="boxLog" id="txtEmail" MaxLength="80"></asp:TextBox></td>
    <td id="mesg1"></td>
  </tr>
  <tr>
          <td width="31%" class="tWhite">First Name</td>
    <td width="61%">
    <asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" 
                                                  ControlToValidate="FirstName"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please enter your first name </span></asp:requiredfieldvalidator>

    <asp:TextBox ID="FirstName" runat="server" class="boxLog" MaxLength="50"></asp:TextBox></td>
    <td width="8%"></td>
  </tr>
   <tr>
    <td width="31%" class="tWhite">Last Name</td>
    <td width="61%"><asp:requiredfieldvalidator id="RequiredFieldValidator6" runat="server" 
                                                  ControlToValidate="LastName"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please enter your last name </span></asp:requiredfieldvalidator>

    <asp:TextBox ID="LastName" runat="server" class="boxLog" MaxLength="50"></asp:TextBox></td>
    <td width="8%"></td>
  </tr>
  <tr>
    <td class="tWhite">Company</td>
    <td><asp:requiredfieldvalidator id="RequiredFieldValidator7" runat="server" 
                                                  ControlToValidate="org"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please enter company name </span></asp:requiredfieldvalidator>

    <asp:TextBox  runat="server" class="boxLog" id="org" MaxLength="50"></asp:TextBox></td>
    <td></td>
  </tr>
  <tr>
    <td class="tWhite">Contact No</td>
    <td><asp:requiredfieldvalidator id="RequiredFieldValidator8" runat="server" 
                                                  ControlToValidate="ContactNo"
                                                  ErrorMessage="Email ID Required " 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogInTrial">Please enter your contact number </span></asp:requiredfieldvalidator>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator5" 
                                                      runat="server" Font-Size="10px" 
                                                      ControlToValidate="ContactNo" 
                                                      Display="Dynamic" 
                                                      ErrorMessage="<span class='validationLogInTrial'>Phone Number must be 10-14 digits</span>"
                                                       ValidationExpression="^[0-9]{10,14}$">
                                                      </asp:RegularExpressionValidator>
    <asp:TextBox  runat="server" class="boxLog" id="ContactNo" MaxLength="15"></asp:TextBox></td>
    <td></td>
  </tr>
  
  <tr>
    <td class="tWhite">Profession</td>
    <td><asp:dropdownlist class="advBoxS6" id="pro" runat="server" style="width:289px;"></asp:dropdownlist></td>
    <td></td>
  </tr>
    
    <!-- remove industry --->
 <!-- <tr>
  <td class="tWhite"></td>
  <td><asp:dropdownlist id="ddlIndustry" runat="server" class="advBoxS6"  ValidationGroup="Group2" style="width:289px;"  Visible="False"></asp:dropdownlist></td>
  <td class="tWhite">&nbsp;</td>
  </tr>-->
  
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td class="tWhite3"><asp:Button ID="Button1" runat="server" Text="Sign Up"  class="btnOrange" style=" width:120px;" ValidationGroup="sumarrylogin" /></td>
    <td class="tWhite3">&nbsp;</td>
  </tr>

  <tr>
    <td class="tWhite">&nbsp;</td>
    <td class="tWhite">&nbsp;</td>
    <td class="tWhite3">&nbsp;</td>
  </tr>

</table>
    
    <div class="clear"></div>
    
    </div>
  </div>

  <div class="rightCont">

  <a class="youtube" href="http://youtu.be/rcKEmUTzhnU" title="Elaw Promo"><img style="margin-bottom:12px;" border="0" src="img/vidClick.jpg"/></a>
  
  <!--<div class="boxTextR">
  <span class="tRed">WHAT&#39;S NEW?</span><br />
  <span class="tGrey">Watch our promotional video to find out more <br/>about our features!</span>
  </div>-->
  
   <!--<div class="boxTextSub" style="margin-bottom:40px;">
   <span class="tRed">TRIAL VERSION ACCOUNT</span><br />
  	<a href="#" class="tGrey2">[+] Trial Version</a><br>
  </div>-->
  
  
  <div class="boxTextSub">
  <span class="tRed">CONTACT US</span><br />
  <div class="boxTextSub">
  	<a href="mailto:sales@elaw.my" class="tGrey2">[+] Sales</a><br>
	<a href="mailto:techsupport@elaw.my" class="tGrey2">[+] Technical Support</a><br />
	<a href="mailto:enquiries@elaw.my" class="tGrey2">[+] Customer Services</a></span>
  </div>
  </div>
  

  <div class="boxTextSub" style="margin-top:50px;">Copyright © 2013 <span class="tOrangeB"><br/>The Digital Library Sdn. Bhd. (1055606-P)</span> <br />
All rights reserved. <br />
<br />Tel: <span class="tOrangeB">1300 88 3529 (eLaw)</span><br/> Fax: <span class="tOrangeB">+603 2117 5203</span><br />
  </div>

</div>

<div class="clear"></div>

</div>
</div>
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
