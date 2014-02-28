<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="ElawLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">



<!--[if lte IE 8]>
 <div id="warning"><BR/>
<h4 class="red">Your current web browser is outdated!</h4>
<p>For the best experience, please use or upgrade either of the following browsers: 
 <a href=https://www.google.com/intl/en/chrome/browser/>Chrome, </a><a href='https://getfirefox.com'>FireFox</a>, <a href='https://www.opera.com/download/'>Opera</a>, or  <a href='https://www.apple.com/safari/'>Safari</a>. Thank You!&nbsp;&nbsp;&nbsp;<a href="#" onClick="document.getElementById('warning').style.display = 'none';"><b>Close Window</b></a></p>
</div>
<![endif]-->
<style type="text/css">
#warning     
   {
	   position:relative; 
	   top:0px; width:100%;
	   height:80px;
	   background-color:#C6DEFF;
	   margin-top:0px; 
	   padding:4px;
	   text-align:center;
	   border-bottom:solid 4px #000066
	   z-index:999;
   }
   </style>




<%
    Dim u As String = Request.ServerVariables("HTTP_USER_AGENT")
    'New Regex("(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase)
    Dim b As New Regex("(bb\d;).+mobile|blackberry.+Version\/\d\.|symbian", RegexOptions.IgnoreCase)
    'Dim d As New Regex("(android|meego).+mobile|avantgo|bada\/|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase)
    'Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)
    'If d.IsMatch(u) Or v.IsMatch(Left(u, 4)) Then Response.Redirect("https://elaw.my/Mobile/Mobiles/index.html") else
    If b.IsMatch(u) Then Response.Redirect("https://elaw.my/Mobile/BB/index.html")
%>
    <title>eLaw Login</title>
    <link rel="shortcut icon" href="GUI/NewDesign/favicon.ico" type="image/x-icon" /> 
    <link href="GUI/NewDesign/css/login.css" rel="stylesheet" type="text/css" />
    <link href="GUI/NewDesign/css/video.css" rel="stylesheet" type="text/css" >
    <script src="GUI/NewDesign/js/libs/jquery-1.7.1.js"></script>
    <script src="GUI/NewDesign/js/jquery.youtubepopup.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.1/jquery-ui.js"></script>

<script type="text/javascript">
    $(function () {
        $("a.youtube").YouTubePopup({ autoplay: 0, hideTitleBar: true });
    });
</script>
    
</head>
<body>
<form id="Form1" method="post" runat="server" defaultbutton="LinkButton1">

<div class="wrappeLog">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
	<div class="logoElaw"><a href="index.aspx" class="transition"><img src="GUI/NewDesign/img/elawLogo.png"/></a></div>
    
    <div class="socialMedia">
    <a href="https://twitter.com/eLawMalaysia" target="_blank"><img style="margin-right:5px;" src="img/twit.png" border="0"/></a>
    <a href="https://www.facebook.com/elaw.digitallibrary" target="_blank"><img style="margin-right:5px;" src="img/fb.png" border="0"/></a>
    <a href="https://my.linkedin.com/pub/the-digital-library/80/31a/b51" target="_blank"><img src="img/in.png" border="0"/></a> 
        i</div>

	<div class="clear"></div>
    
  <div class="leftCont"><img src="GUI/NewDesign/img/bannerLog.jpg" width="488" height="161"/>
    
    <div class="logData">
    <img src="GUI/NewDesign/img/arrow.jpg"/>
     
   <%-- <img src="GUI/NewDesign/img/error.png"/>--%>
   <asp:label id="lblMsg" runat="server" class="validationLogIn"></asp:label>
   
  
    
    <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" 
                                                  ControlToValidate="txtUsername"
                                                  ErrorMessage="No UserName Given" 
                                                  Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="sumarrylogin" ><span class="validationLogIn">No user name given</span></asp:requiredfieldvalidator>

                   
    
                      <asp:requiredfieldvalidator id="RequiredFieldValidator2" 
                                          runat="server" 
                                          ControlToValidate="txtPassword"
                                          ErrorMessage="No Password Given" Display="Dynamic" 
                                          SetFocusOnError="True" 
                      ValidationGroup="sumarrylogin" ><span class="validationLogIn">No password given</span></asp:requiredfieldvalidator>
    
    
               
    <table width="94%" border="0" cellpadding="0" cellspacing="5" style="margin:8px 0 30px 0;">
  <tr>
    <td width="32%" class="tWhite">ID</td>
    <td width="68%"><label for="textfield"></label>
     
    <asp:TextBox ID="txtUsername" runat="server" class="boxLog" 
            ValidationGroup="sumarrylogin"></asp:TextBox>
  </tr>
  <tr>
    <td class="tWhite">Password</td>
    <td>
        <asp:TextBox ID="txtPassword" runat="server" class="boxLog" 
            ValidationGroup="sumarrylogin" TextMode="Password"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td>
    
            <asp:LinkButton ID="LinkButton1" runat="server" ValidationGroup="sumarrylogin"><img src="GUI/NewDesign/img/btnSubmit.png"/></asp:LinkButton>
    
            <asp:LinkButton ID="LinkButton2" runat="server"> <img src="GUI/NewDesign/img/btnClear.png"/></asp:LinkButton>
    </td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td class="tBlue"><a href="ForGetPassword.aspx">Forgot your password?</a></td>
  </tr>
 
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td class="tBlue">  <asp:CheckBox ID="rememberme" runat="server" />Remember me</td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td class="tWhite2">Use of this service is subject to <a href="https://elaw.my/content/termsCondition.aspx" class="tWhite2">Terms &amp; Conditions</a> </br>
      Please review this information before proceeding.</td>
  </tr>
    </table>
    
    <div class="clear"></div>
    
    </div>

     
  </div>
  
  
  <div class="rightCont">
  <a class="youtube" href="https://youtu.be/rcKEmUTzhnU" title="Elaw Promo"><img style="margin-bottom:12px;" border="0" src="img/vidClick.jpg"/></a>
     
  <!--<div class="boxTextR">
  <span class="tRed">WHAT&#39;S&nbsp; NEW?</span><br />
  <span class="tGrey">Watch our promotional video to find out more about our features!</span>
  </div>-->
  
  <div class="boxTextSub">
  <span class="tRed">CONTACT US</span><br />
  <div class="boxTextSub">
  	<a href="mailto:sales@elaw.my" class="tGrey2" target="blank">[+] Sales</a><br>
	<a href="mailto:techsupport@elaw.my" class="tGrey2" target="blank">[+] Technical Support</a><br />
	<a href="mailto:enquiries@elaw.my" class="tGrey2" target="blank">[+] Customer Services</a></span>
  </div>
  </div>
  

  <div class="boxTextSub" style="margin-top:90px;">Copyright © 2013 <br />
  <span class="tOrangeB">The Digital Library Sdn. Bhd. (1055606-P)</span><br /> 
  All rights reserved. <br /><br />
  Tel: <span class="tOrangeB">1300 88 3529 (eLaw)</span><br /> 
  Fax: <span class="tOrangeB">+603 2117 5203</span>
  </div>

</div>

<div class="clear"></div>

<div class="signUp"><a href="ElawSubscribe.aspx"><img src="GUI/NewDesign/img/arrowSignUp.jpg" border="0"/> </a> &nbsp;&nbsp;&nbsp;&nbsp;<a href="elawtrial.aspx"><img src="GUI/NewDesign/img/arrowtrial.jpg" border="0"/> </a></div>

</div>

</form><!-- Piwik -->
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
</script>

<!-- End Piwik Code -->
</body>
</html>
