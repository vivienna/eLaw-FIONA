<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.FormMainDisplayed" CodeFile="FormMainDisplayed.aspx.vb" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
         <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        
         <title>From  Mian Display</title>
         <link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
         <link id="MyStyleSheet" rel="stylesheet" type="text/css" runat="server" />

   
    <script src="js/libs/jquery-1.7.1.js" type="text/javascript"></script>
  
    <script src="js/script.js" type="text/javascript"></script>
    <link href="css/style_New.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    $(document).ready(function () {

        ////////////////////////////////////////////////
        //Print page 
        window.printItn = function () {

            var printContent = document.getElementById('lblXml');

            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();

            //  you should add all css refrence for your html. something like.

            var WinPrint = window.open(windowUrl, windowName, 'left=30,top=30,right=500,bottom=500,width=700,height=500');
            WinPrint.document.write('<' + 'html' + '><head><link href="../css/style_New.css" rel="stylesheet" type="text/css" /></head><' + 'body style="background:none !important"' + '>');
            WinPrint.document.write(printContent.innerHTML);

            WinPrint.document.write('<' + '/body' + '><' + '/html' + '>');
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }

        $("#btnShowModal").click(function (e) {

            ShowDialog(true);
            e.preventDefault();
        });

        $("#btnClose").click(function (e) {

            HideDialog();
            e.preventDefault();
        });

        $("#btnSubmit").click(function (e) {

            var sEmail = $('#txtEmail').val();
            if ($('#txtEmail').val().length == 0) {
                $("#msg").html("Please Type your Email");
                return false;
            } else {
                var id = $("#lblid").html();
                var result = email(sEmail);
                var Idtitle = $("FORMTITLE").html();
                if (result) {
                    var dataString = {
                        email: sEmail,
                        mypageid: id,
                        title: Idtitle
                    };

                    $.ajax({
                        type: "POST",
                        url: "emial.aspx",
                        data: dataString,
                        cache: false,
                        success: function (html) {
                            if (html) {
                                $("#msg").html(html);

                                $('#txtEmail').val('');

                            }
                        }
                    }); //end of ajax

                    HideDialog();
                    e.preventDefault();
                }
            }
        });
    });

    function email(value) {

        return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value);
    }



    function ShowDialog(modal) {
        $("#overlay").show();
        $("#dialog").fadeIn(300);

        if (modal) {
            $("#overlay").unbind("click");
        }
        else {
            $("#overlay").click(function (e) {
                HideDialog();
            });
        }
    }

    function HideDialog() {
        $("#overlay").hide();
        $("#dialog").fadeOut(300);

    }
    
    </script>
    <style type="text/css">
        #lblXml p
        {
            padding:5px;
            line-height: 180%; 
            }
        
        
        #popup
        {
    	border-style:solid;
        border-width:1px;
        width:440px;
        margin-left:5px;
        padding:15px;
        background-color:red;
      
    	}
    	
    	 .web_dialog_overlay
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: 0.15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101;
            display: none;
        }
        .web_dialog
        {
             display: none;
            position:absolute;
            width: 290px;
           
            top: 13%;
            left: 35%;
            
            padding: 0px 0px 5px 0px;
            z-index: 102;
            
        
        }
        
        .align_right
        {
            text-align: right;
        }
    	
    #notification { position:relative; }
    #notification a { text-decoration:none;}
    #clickNotify { 
    display:inline-block;
    float:left;
	background-color:#f87104;
    border:#949494 1px solid;
    position:relative;
    z-index:40;
    cursor:pointer;
    
    
}
#nofityBox {
    position:absolute;
	background-color:#757575;font-size:14px;padding:5px;
	border:#949494 1px solid;
	width:400px;
	text-align:left;
	 display:none;
    top:35px;
    left:0;
   
    z-index:29;
}
.boxNotify a {
	font-family:Arial, Helvetica, sans-serif;
	font-size:12px;
	text-decoration:none;
	display:block;
	border-bottom:#ffffff 1px solid;
}
.boxNotify strong {
	font-family:Arial, Helvetica, sans-serif;
	font-size:12px;
	text-decoration:none;
	padding:10px;
	display:block;
}
.boxNotify strong:hover { background-color:#006390;}
#form2 
        {
           background-color:#757575;
           font-size:14px;
           padding:0px 0px 5px  0px;
           -webkit-box-shadow:0 1px 1px #000;
           -moz-box-shadow:0 1px 1px #000;
           box-shadow:0 1px 1px #000;
           border-radius:5px;
           position:absolute;
           width:380px;
           z-index: 9002;
           }
        


   </style>
    
</head>
<body >
    <form id="form1" runat="server">


<div class="header">

<div class="home"><a  href="../index1.aspx"><img border="0" src="img/logoHome.png" align="middle"/></a></div>
 
 <div class="menu">
 <ul>
 
 <li><a href="casesSearch.aspx">CASE LAW</a></li>
 <li> <a href="legislationSearch.aspx">LEGISLATION</a></li>
 </ul>

 <!-- Drop btn1 -->
 
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

<!--    Close header-->



<!-- Content 2 -->
 <div class="clear"></div>
<div id="content2">
	 
<div class="clear"></div>
   
	<div id="contRight">
    
    <section class="resultsRight">
    
    <div class="clear"></div>
    
 	<!-- left Bar1 Results Elements -->
    
    
    
    <!-- left Bar1 Results Elements 2 -->
    
    <div class="topBar2">
    
    <div class="leftResultBox2">
    
    </div>
   

    



    <div class="rightResultBox">
    <div class="funtionResults"> 
    
    
     <span id="EmailCheck"> <%Response.Write(EmailConfirm)%> </span>
    <a href="#" id="print" onclick='printItn()'><img border="0" src="img/icoPrint.gif"/>Print</a>
    
   <a href="#" id="btnShowModal"><img border="0" src="img/icoEmail.gif"/>Email</a>
   <asp:LinkButton ID="LinkButton2" runat="server" style="visibility:hidden;"><img border="0" src="img/icoDownload.gif"/>Download</asp:LinkButton>
   
    
    </div>
     </div>
    
    <div class="clear"></div>
    </div>
    
    <!-- Box Display Cases -->
    



        <div class="panelBar2">
        
        <asp:label id="lblXml" runat="server" style="padding:35px" Width="90%"></asp:label>
        <asp:Label ID="lblid" runat="server"></asp:Label>

        </div>


        
    </section>
    
    <div class="clear"></div>
  	</div>
</div>

     
                <%--<div id="notification" onclick="load_me()">
                <a href="#" id="clickNotify" >
                
                <asp:Label ID="lb_count_note" runat="server" Text=""></asp:Label><em></em></a>
                <div style="clear:both"></div>
                <div id="nofityBox">
                
                	<div class="boxNotify">
                    <a href="#"><strong>Text goes here...</strong>all the notification - text 1 - - text 1</a>
                    </div>
                    
                   <div class="boxNotify">
                    <a href="#"><strong>Text goes here...</strong>all the notification  - text 1 - - text 1</a>
                    </div>
                    
                    <div class="boxNotify">
                    <a href="#"><strong>Text goes here...</strong>all the notification  - text 1 - - text 1</a> 
                    </div>
                    
                </div>
                	
            </div> --%>
                 
 <div id="overlay" class="web_dialog_overlay"></div>
           
               <div id="dialog" class="web_dialog">
       <div id="form2" >	
		
			<h3><span>Send Form To</span></h3>
		
			<table width="100%" border="0" cellpadding="4" cellspacing="1" 
                style="padding-left: 20px">
                  <tr>
                    <td>Email</td>
                  </tr>
                  <tr>
                    <td>
                <input type="text" name="name" id="txtEmail" maxlength="50" class="advBoxS" style="width:90%;" /></td>
                  </tr>
                  <tr><td>Notes: Email will be in Doc format</td></tr>
                 <tr><td style="font-size: small">  <span id="msg"></span></td></tr>
                  <tr><td>
                  <p class="submit" align="center"><button id="btnSubmit" type="button" value="Submit" class="btnGo">Send</button>
                            <button  type="button" value="Submit" id="btnClose" class="btnGo" >Cancel</button>
                
                </p>
                  </td></tr>
                  </table>
						
		</div>	
    </div>
          
             

              <script type="text/javascript">
                  $(document).ready(function () {

                      $("#panel").animate({ marginLeft: "-266px" }, 5);
                      $("#colleft").animate({ width: "0px", opacity: 0 }, 4);
                      $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
                      $("#contRight").animate({ marginLeft: "50px" }, 5);

                      $("#hidePanel").click(function () {
                          $("#panel").animate({ marginLeft: "-266px" }, 500);
                          $("#colleft").animate({ width: "0px", opacity: 0 }, 400);
                          $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
                          $("#contRight").animate({ marginLeft: "50px" }, 500);
                      });
                      $("#showPanel").click(function () {
                          $("#contRight").animate({ marginLeft: "280px" }, 200);
                          $("#panel").animate({ marginLeft: "0px" }, 400);
                          $("#colleft").animate({ width: "266px", opacity: 1 }, 400);
                          $("#showPanel").animate({ width: "0px", opacity: 0 }, 600).hide("slow");
                      });
                      $('#expandResults li').click(function () {
                          var text = $(this).children('.scrollBoxWrapper');
                          if (text.is(':hidden')) {
                              text.slideDown('200');
                              $(this).children('.arrow').html('<img src="img/arrowEx1.png" />');
                          } else {
                              text.slideUp('200');
                              $(this).children('.arrow').html('<img src="img/arrowEx2.png" />');
                          }
                      });
                      $('.scrollBoxWrapper p').jScrollPane({
                          horizontalGutter: 5,
                          verticalGutter: 5,
                          'showArrows': false
                      });


                      ////////
                  });
            </script>
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


    