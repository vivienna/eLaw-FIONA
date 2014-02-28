<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ElawSubscribe.aspx.vb" Inherits="ElawSubscribe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
 <link rel="shortcut icon" href="GUI/NewDesign/favicon.ico" type="image/x-icon" /> 
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>eLaw</title>
<meta name="keywords" content="Elaw"/>
<meta name="description" content="Elaw"/>
<link rel="stylesheet" href="GUI/NewDesign/css/login.css"/>
<link href="GUI/NewDesign/css/video.css" rel="stylesheet" type="text/css" >
    <script src="GUI/NewDesign/js/libs/jquery-1.7.1.js"></script>
    <script src="GUI/NewDesign/js/jquery.youtubepopup.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.1/jquery-ui.js"/></script>
    <style>
     input.error { border: 1px solid red; }
        input.valid { border: 1px solid #1FFF00; }
    </style>

<script type="text/javascript">
    $(function () {
        $("a.youtube").YouTubePopup({ autoplay: 0, hideTitleBar: true });
    });
</script>


    
    <style type="text/css">
*{
	padding:0;
	margin:0;
}

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrappeLog">
	<div class="logoElaw"><a href="index.aspx"><img src="GUI/NewDesign/img/elawLogo.png" border="0"/></a></div>
    <div class="socialMedia">
 <a href="https://twitter.com/eLawMalaysia" target="_blank"><img style="margin-right:5px;" src="img/twit.png" border="0"/></a>
    <a href="https://www.facebook.com/pages/eLaw-Malaysia/162895963906088?ref=hl" target="_blank"><img style="margin-right:5px;" src="img/fb.png" border="0"/></a>
    <a href="https://www.linkedin.com/company/TheDigitalLibrary" target="_blank"><img src="img/in.png" border="0"/></a>
    </div>

	<div class="clear"></div>
    
      
  <div class="leftCont">
    
    <div class="logData">
    
    
    <div class="validationLogIn" style="display:none;" id="error">
    <img src="GUI/NewDesign/img/error.png"/>

	<ul style="padding-left:34px;" >
	</ul>
    
    </div>
    
    <div class="stepLogIn">
    <span class="tOrange" id="step1">STEP 1</span>
    <span><a class="tWhite4" href="#" id="step2">STEP 2</a></span>
    <span><a class="tWhite4" href="#" id="step3">STEP 3</a></span>
     <span><a class="tWhite4" href="#" id="A1">STEP 4</a></span>
    </div>
      
    <table width="94%" border="0" cellpadding="0" cellspacing="5" style="margin:8px 0 20px 0; " id="tb1">
  <tr>
    <td width="40%" class="tWhite">&nbsp;</td>
    <td width="61%">
    <asp:label id="lblErrMsg" runat="server"  ForeColor="Crimson" 
            EnableViewState="False" CssClass="validationLogInTrial" Visible="False"></asp:label>
      
     </td>
     <td width="8%" id="mesg">&nbsp;</td>
  </tr>
  <tr>
    <td width="40%" class="tWhite">User Name</td>
    <td width="61%"><label for="textfield"></label>
    <span class="validationLogInTrial" id="valus" style="display:none;">User Name Already Taken </span>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="sub_txtUserName" Display="Dynamic" ErrorMessage= "" ValidationGroup="Group1"><span class="validationLogInTrial">Please enter any unique ID </span></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="sub_txtUserName" Display="Dynamic" ErrorMessage="RegularExpressionValidator" ValidationExpression="^[a-zA-Z-0-9]{7,16}$" ValidationGroup="Group1"><span class="validationLogInTrial">User Name Should Contain Only 8-16 Letters And Numbers</span></asp:RegularExpressionValidator>
        
    <asp:textbox id="sub_txtUserName" runat="server" class="boxLog" MaxLength="30" ValidationGroup="Group1"></asp:textbox>
     </td>
     <td width="8%" id="mesg"></td>
  </tr>
  <tr>
    <td class="tWhite">Password</td>
   
    <td> <span class="validationLogInTrial" id="valpw" style="display:none;">Please enter password </span>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="sub_txtPassword" Display="Dynamic" ErrorMessage="password" ValidationGroup="Group1"><span class="validationLogInTrial">Please enter your password</span></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="sub_txtPassword" Display="Dynamic" ErrorMessage="RegularExpressionValidator" ValidationExpression="^[a-zA-Z-0-9]{7,16}$" ValidationGroup="Group1"> <span class="validationLogInTrial">Password should be 8-15 characters .</span></asp:RegularExpressionValidator>
    <asp:textbox id="sub_txtPassword" runat="server" class="boxLog" 
                        MaxLength="16" TextMode="Password" ValidationGroup="Group1"></asp:textbox></td>
  </tr>
  
  <tr>
    <td class="tWhite">Re-type Password</td>
    <td><span class="validationLogInTrial" id="valrpw" style="display:none;">Please re-enter password </span>
    
        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="sub_re_txtpassword" Display="Dynamic" ErrorMessage="Re-password" ValidationGroup="Group1"><span class="validationLogInTrial">Please Re-type Your Password</span></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="sub_txtPassword" ControlToValidate="sub_re_txtpassword" Display="Dynamic" ErrorMessage="CompareValidator" ValidationGroup="Group1"><span class="validationLogInTrial">Password did not match</span></asp:CompareValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="sub_re_txtpassword" Display="Dynamic" ErrorMessage="RegularExpressionValidator" ValidationExpression="^[a-zA-Z-0-9]{7,16}$" ValidationGroup="Group1"><span class="validationLogInTrial">Re-Password should be 8-15 characters .</span></asp:RegularExpressionValidator>
    <asp:textbox id="sub_re_txtpassword" runat="server" 
                        class="boxLog" MaxLength="16" TextMode="Password" ValidationGroup="Group1"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">Email</td>
    <td>
    <span class="validationLogInTrial" id="valemail" style="display:none;">Email Id Already Taken </span>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email" ValidationGroup="Group1"><span class="validationLogInTrial">Please enter Your Email</span></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Incorrect Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Group1"><span class="validationLogInTrial">Please enter Correct Email</span></asp:RegularExpressionValidator>
    <asp:textbox id="txtEmail" runat="server" class="boxLog" onclick="ShowAvailabilityEmail()"
                        MaxLength="80" ValidationGroup="Group1"></asp:textbox></td>
                        <td  id="mesg1"></td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td><table width="100%" border="0" cellpadding="0" cellspacing="0">
      
    </table></td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td class="tWhite3">
        <asp:Button ID="Button2" runat="server" Text="Next" ValidationGroup="Group1" class="btnOrange" style=" width:120px;"/>
    
      </td>
      <td class="tWhite">&nbsp;</td>
     
  </tr>
  <tr>
  <td></td>
  <td><a class="tBlue" href="UpgradeAccount.aspx">I want to reuse my trial account details</a></td>
  <td></td>
  </tr>
    </table>
    
    <div class="clear"></div>
	 <table width="94%" border="0" cellpadding="0" cellspacing="5" style="margin:8px 0 20px 0; display:none;" id="tb2">
  <tr>
    <td width="32%" class="tWhite">First Name</td>
    <td width="68%"><label for="textfield"></label>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter First Name </span></asp:RequiredFieldValidator>
        
        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="RegularExpressionValidator" ValidationExpression="^[a-zA-Z]{3,16}$" ValidationGroup="Group2"><span class="validationLogInTrial">First Name Should Contain Only 3-16 Letters only</span></asp:RegularExpressionValidator>
        
    <asp:textbox id="txtFirstName" runat="server"  MaxLength="40" class="boxLog" ValidationGroup="Group2"></asp:textbox>
     </td>
  </tr>
  <tr>
    <td class="tWhite">Last Name</td>
    <td> 
        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtLastName" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter Last Name </span></asp:RequiredFieldValidator>
        
        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtLastName" Display="Dynamic" ErrorMessage="RegularExpressionValidator" ValidationExpression="^[a-zA-Z]{3,16}$" ValidationGroup="Group2"><span class="validationLogInTrial">First Name Should Contain Only 3-16 Letters only</span></asp:RegularExpressionValidator>
        
        <asp:textbox id="txtLastName" runat="server"  MaxLength="40" 
                      class="boxLog" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">Company</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtFirmName" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter company name </span></asp:RequiredFieldValidator>
        <asp:textbox id="txtFirmName" runat="server"  MaxLength="40" 
                        class="boxLog" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">Profession</td>
    <td><asp:dropdownlist id="ddlProfession" 
                        runat="server" class="advBoxS6" style="width:289px;" ValidationGroup="Group2"></asp:dropdownlist></td>
  </tr>
 <!-- <tr>
    <td class="tWhite">Industry</td>
    <td><asp:dropdownlist id="ddlIndustry" runat="server" class="advBoxS6" style="width:289px;" ValidationGroup="Group2" ></asp:dropdownlist></td>
  </tr>-->
  <tr>
    <td class="tWhite">Address1</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtAddress1" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter your address </span></asp:RequiredFieldValidator>
        <asp:textbox id="txtAddress1" runat="server" class="boxLog" 
                        MaxLength="180" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">Address2</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtAddress2" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please complete your address </span></asp:RequiredFieldValidator>
        <asp:textbox id="txtAddress2" runat="server"  class="boxLog" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">City</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtCity" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter city name </span></asp:RequiredFieldValidator>
        <asp:textbox id="txtCity" runat="server" class="boxLog" 
                        MaxLength="20" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">State</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtState" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter state name </span></asp:RequiredFieldValidator>
        <asp:textbox id="txtState" runat="server" class="boxLog" 
                        MaxLength="20" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">Postal/ZIP Code</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtPostCode" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter the Postal/ZIP Code </span></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="Zip Code" ValidationExpression="\d{5,8}" ControlToValidate="txtPostCode" Display="Dynamic" ValidationGroup="Group2"><span class="validationLogInTrial"> Postal/ZIP Code accepts only 5-8 numbers and letters </span></asp:RegularExpressionValidator>
        <asp:textbox id="txtPostCode" runat="server" class="boxLog" 
                        MaxLength="8" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">Telephone Number</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtTelephone" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter  Phone No </span></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="Zip Code" ValidationExpression="\d{8,14}" ControlToValidate="txtTelephone" Display="Dynamic" ValidationGroup="Group2"><span class="validationLogInTrial"> Phone number accepts only 8-14 numbers </span></asp:RegularExpressionValidator>
        <asp:textbox id="txtTelephone" runat="server" class="boxLog" 
                        MaxLength="18" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  <tr>
    <td class="tWhite">Fax Number</td>
    <td>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtFax" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ValidationGroup="Group2"><span class="validationLogInTrial">Please enter Fax No </span></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Zip Code" ValidationExpression="\d{8,14}" ControlToValidate="txtFax" Display="Dynamic" ValidationGroup="Group2"><span class="validationLogInTrial"> Fax number accepts only 8-14 numbers </span></asp:RegularExpressionValidator>
        <asp:textbox id="txtFax" runat="server" class="boxLog" 
                        MaxLength="18" ValidationGroup="Group2"></asp:textbox></td>
  </tr>
  
  
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td><span class="tWhite3">
     
        <asp:Button ID="Button3" runat="server" Text="Next" ValidationGroup="Group2" class="btnOrange" style=" width:120px;" />
    </span></td>
  </tr>
    </table>
    
    <div class="clear"></div>
    
	
	 <table width="94%" border="0" cellpadding="0" cellspacing="5" style="margin:8px 0 20px 0; display:none;" id="tb3">
  <tr>
    <td width="32%" class="tWhite">&nbsp;</td>
    <td width="68%">
        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator" ClientValidationFunction = "ValidateCheckBox"></asp:CustomValidator>
      </td>
  </tr>
  <tr>
    <td width="32%" class="tWhite">Account  Type</td>
    <td width="68%"><span class="tGreen" style="font-size:14px;">Single Account<asp:dropdownlist 
                        id="ddlAccountType" runat="server" Width="48px" 
            AutoPostBack="True" Visible="False"
Enabled="False"></asp:dropdownlist>
                    <asp:dropdownlist id="ddlSiteName" runat="server" Width="40px" 
                        Visible="False"></asp:dropdownlist>
        </span></td>
  </tr>
  <tr>
  <td class="tWhite">Site Name</td>
  <td><asp:Label ID="lbl_site_name" runat="server" Text="" class="tGreen" style="font-size:14px;"></asp:Label>
                    <asp:listbox id="lbSiteNames" runat="server" Width="62px" 
                        Height="23px" Visible="False"></asp:listbox><asp:button id="btnRemove" runat="server" 
                        Text="Remove" UseSubmitBehavior="False" Visible="False"></asp:button>
                
                </td>
  
  </tr>
  
  
  <tr>
    <td class="tWhite" align="right" valign="top"><asp:checkbox id="CheckBox1" runat="server" Text=" " ValidationGroup="Group2" Checked="True"></asp:checkbox></td>
    <td class="tWhite3">I agree that The Digital Library Sdn. Bhd. is not liable for any use or misuse of the information on this site and that I have read the Terms &amp; Conditions of Use.</td>
  </tr>
  <tr>
    <td class="tWhite">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td class="tWhite">
    <asp:label id="lblTotalAmmount" runat="server" 
                         Height="16px" class="tGreen" style="font-size:14px;" 
            Visible="False"></asp:label>
      </td>
    <td><table width="100%" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td width="51%"><span class="tWhite3">
        <asp:button id="btnSubmit" runat="server" Text="Submit My Order" 
                        class="btnOrange" style=" width:180px;" ValidationGroup="Group2"></asp:button>
                                         </span></td>
        <td width="49%">
        
        <asp:button id="Button1" runat="server" Text="Clear" 
                       class="btnWhite" style="margin-left:5px;" ></asp:button></td>
      </tr>
    </table></td>
  </tr>
    </table>
    
    <div class="clear"></div>
	
	
    </div>
  </div>
  
  <div class="rightCont">
  <a class="youtube" href="https://youtu.be/rcKEmUTzhnU" title="Elaw Promo"><img style="margin-bottom:12px;" border="0" src="img/vidClick.jpg"/></a>
  
  <!--<div class="boxTextR">
  <span class="tRed">WHAT&#39;S NEW?</span><br />
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
  

  <div class="boxTextSub" style="margin-top:60px;">Copyright © 2013 <br />
  <span class="tOrangeB">The Digital Library Sdn. Bhd. (1055606-P)</span> <br />
All rights reserved. <br />
<br />Tel: <span class="tOrangeB">1300 88 3529 (eLaw)</span> <br />
Fax: <span class="tOrangeB">+603 2117 5203</span><br />
  </div>

</div>

<div class="clear"></div>

</div>
<script type="text/javascript">
    
    $(document).ready(function () {
        var isValid = false;
        var isValid1 = false;
        var isValid2 = false;
        
        var Emailvalidation;
           $("#Button2").click(function () {
            isValid = Page_ClientValidate('Group1');
            if (isValid) {
                var usvalidation;
                    //////////////////////////////////
                    $.ajax({
                        type: "POST",
                        url: "ElawSubscribe.aspx/CheckUserName",
                        data: '{userName: "' + $("#<%=sub_txtUserName.ClientID%>")[0].value + '" }',
                         contentType: "application/json; charset=utf-8",
                            dataType: "json",
                             success: function (data) {
                                 usvalidation = data.d;
                          if (usvalidation == "1") {
                        /////////////////////////////////
                        $("#valus").slideUp();
                        ShowAvailabilityEmail();
                        /////////////////////////////////
                    }
                    else { $("#valus").slideDown(); }
               },
                failure: function (response) {
                }
                     });

                    /////////////////////////////////
                return false;
            }

           });
        ////////////////////////////////
           function ShowAvailabilityEmail() {
               var email = $("#txtEmail").val();
               $.ajax({
                   type: "POST",
                   url: "ElawSubscribe.aspx/CheckUserEmail",
                   data: '{U_Email: "' + $("#<%=txtEmail.ClientID%>")[0].value + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                Emailvalidation = data.d;
                    if (Emailvalidation == "1") {
                    $("#valemail").slideUp();
                    $("#tb1").slideUp();
                    $("#tb2").slideDown();
                }
                else { $("#valemail").slideDown(); }
            },
            failure: function (response) { }
             });
    }

        //////////////////////////////

        $("#Button3").click(function () {
            isValid1 = Page_ClientValidate('Group2');
            if (isValid1) {
                $("#valemail").slideUp();
                $("#tb2").slideUp();
                $("#tb3").slideDown();
                return false;
            }
        });

        $("#btnSubmit").click(function () {
            isValid1 = Page_ClientValidate('Group3');
            if (document.getElementById("<%=CheckBox1.ClientID%>").checked == true) {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }


            if (isValid1) {
                $("#valemail").slideUp();
                $("#tb1").slideUp();
                $("#tb2").slideUp();
                $("#tb3").slideDown();
                return false;
            }
            return isValid2;
        });

       
    });
    
    
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
<!-- End Piwik Code -->

</body>
</html>
