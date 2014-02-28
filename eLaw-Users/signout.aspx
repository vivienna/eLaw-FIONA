<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signout.aspx.vb" Inherits="signout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>eLaw - The Largest Database of Malaysia Law</title>
<meta name="keywords" content="Elaw, Malaysian law, Court Judgments, Legal Cases, Legislation, eLaw.my">
<meta name="description" content="Looking for the largest database of Malaysia law? Search for court judgments, legal cases and legislation. Free 3-day trial. Experience the difference">
<meta content="Commonwealth law, malaysia law, Commonwealth legal information, Commonwealth court judgments, arbitration, banking law, company law, criminal law, family law, intellectual property law, industrial law, land law, maritime law, medical law, public law, tax law, tort, labour law, employment law, malaysia legislation, undang-undang malaysia, laws of malaysia, property law">
<meta content="ELaw" name="author" />
<meta content="Law" name="classification" />
<meta content="Global" name="distribution" />
<meta content="ELaw Sdn Bhd (545264 X)" name="copyright" />     
<meta content="1 days" name="revisit-after" />
<meta content="FOLLOW,INDEX" name="robots" />
<link id="Link1" runat="server" rel="shortcut icon" href="GUI/NewDesign/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/NewDesign/favicon.ico" type="image/ico"/>
<script src="include/jquery-1.4.2.min.js" type="text/javascript"></script>
<link rel="stylesheet" href="css/style_New.css">
</head>
<body>

    <form id="form1" runat="server">
    
<div id="content1">

	<div class="Logoquick"><a href="index.aspx"><img src="img/bigTLRlogo.png" border="0"/></a></div>
    
    <div class="boxSignOut">
    <h1>Thank you for using <a href="index.aspx" style="text-decoration:none; color:#383838;">eLaw</a></h1><br/><br/>
    
    <asp:label id="lblMsg" runat="server"></asp:label>
        <br />
        <br />
        eLaw is continuously adding to its extensive database of legal&nbsp;cases, legislation and other legal resources.<br/> <br/>
        
        For any assistance on eLaw, please feel free to call us at <b>1300 - 88 - 3529 (eLaw)</b>.<br/><br/>
		
		<br/>

	<a href="http://elaw.my/login.aspx" class="btnSave" style="width:145px; display:block; margin:20px auto; text-decoration:none;">Login</a>
        </div>
        
        <img src="img/boxShadow.jpg"/>
        
        <div class="clear"></div>

</div>

<script src="js/script.js"></script>
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
