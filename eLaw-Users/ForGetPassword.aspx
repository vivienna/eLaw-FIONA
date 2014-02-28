<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ForGetPassword.aspx.vb" Inherits="ForGetPassword" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eLaw Password Request </title>
    <link href="GUI/NewDesign/css/login.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrappeLog3">
  
	<div class="logoElaw"><a href="index.aspx"><img src="img/elawLogo.png"/></a></div>
    <div class="socialMedia"><a href="#"><img src="img/twit.png"/></a>
    <a href="#"><img src="img/fb.png"/></a></div>

	<div class="clear"></div>
    
    
    
  <div class="thanksWrap">
  	
    
    <div class="tq1">
    <div class="ribbonEx"></div>
    
    <h1>Requesting Password</h1>
    
    <p>&nbsp;</p>
    <table width="94%" border="0" cellpadding="0" cellspacing="5" style="margin-top:8px;">
  <tr>
  <td></td>
    <td width="32%" class="" " style="width: 100%">
    
    
     <%Response.Write(errormsg)%>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txtemail" Display="Dynamic" 
            
            ErrorMessage="<span class='validationLogInTrial'>Please Type An Email</span>" 
            ValidationGroup="sumarrylogin"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="txtemail" Display="Dynamic" 
            ErrorMessage="<span class='validationLogInTrial'>Please Type Correct Email</span>" 
            ValidationExpression="^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$" 
            ValidationGroup="sumarrylogin"></asp:RegularExpressionValidator>
      </td>
  </tr>
 
  <tr>
    <td class="">Email</td>
    <td>
        <asp:TextBox ID="txtemail" runat="server" class="boxLog" ></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td><table width="100%" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td width="55%">
        
            <asp:Button ID="Button1" runat="server" Text="Submit" class="btnOrange" 
                style=" width:160px;" ValidationGroup="sumarrylogin" />
        </td>
        <td width="45%"></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td class="tBlue">&nbsp;</td>
  </tr>
    </table>
    
        <p><%Response.Write(E_Success) %></p>
        
  	</div>
    
    
    <div class="boxtq2">
    <table width="70%" border="0" cellspacing="0" cellpadding="0" align="center" >
          <tr>
            <td width="33%" align="right" style="padding-right:10px;"><a href="#"><img border="0" src="img/tqbox1.png"/></a></td>
            <td width="1%">&nbsp;</td>
   
            <td width="23%" align="center"><a href="ElawSubscribe.aspx"><img border="0" src="img/tqbox2.png"/></a></td>
            <td width="1%">&nbsp;</td>
            <td width="42%" align="left" style="padding-left:10px;"><a href="#"><img src="img/tqbox3.png" alt="" border="0"/></a></td>
          </tr>
        </table>
    </div>
  	
    
  
  </div>
  <div class="clear"></div>
<img src="img/bgbtmshadow.jpg"/>
</div>
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
<!-- End Piwik Code -->
</body>
</html>
