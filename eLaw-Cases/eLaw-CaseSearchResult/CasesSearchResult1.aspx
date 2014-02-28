<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.CasesSearchResult11" SmartNavigation="True" CodeFile="CasesSearchResult1.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <link href="css/cass.css" rel="stylesheet" type="text/css" />
    <script src="include/css-pop.js" type="text/javascript"></script>
    <script src="js/libs/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="include/jquery-ui-1.10.2.custom.min.js"></script>
    <link rel="stylesheet" href ="include/jquery-ui-1.10.2.custom.min.css" />
   <style type="text/css">
   .ui-autocomplete
   {
       z-index: 9003;
       }
       
    .nav1 { list-style: none; float: left; }
    .nav1 li, #nav2 li { float:left; display: block; background:#000000; position: relative; z-index:100; border-right:none;  }
    .nav1 li a{
	   background-color:#000000;  
	   font-size:12px; 
	   text-decoration: none;  
	   color: #ffffff; 
	   zoom: 1; 
	   padding:11px 18px 0 18px; }
	.nav1 li a:hov { color:#ffffff; }
	.nav1 ul { position:absolute; display: none; margin:0; padding:0; list-style: none; border-bottom: 3px solid #00648e;  }
    .nav1 ul li { width: 160px; float: left; border-top:1px solid #333333; text-align:left; background-color: #ffffff; color:#333333; }
    .nav1 ul a { display:block; height:16px; line-height:20px; padding: 2px 5px 8px 16px; color:#333333; background-color:#ffffff; font-weight:normal;}
    .nav1 ul a:hover { text-decoration:none; background-color: #000000; color: #ffffff;  }
	   
	.hWord {
        background-color: yellow;
        font-weight:bold;
         color:Red;
           }
           
   .info  {
    border:solid 1px #DEDEDE;
    background:#EFEFEF;
    color:#222222;
    padding:0 0px 0 0;
    margin-right:33px;
    text-align:center;
    float:right;
    }
#notesRight{float:right;width:480px; display:none; }
#addNotes_div{position:absolute;width:450px;z-index:100 ;}
#addNotes_div.fixed{position:fixed;top:0}
.notesBox{width:420px;padding:10px; background-color:#cce7f7;position:absolute;z-index:999;margin:20px 0 0 33px; font-size:14px;border-radius:15px;}
  .cjc {margin:4px 0px 0px 11px; padding:4px 0px 2px 0px; border-top:solid 1px #C5D5E2; float:left; display:block; color:#f47200; font-size:0.8em }
.positive a {color:#009900}
.neutral a{color:#FF9933}
.negative a{color:#FF0000}
a {color:#0A59C5; text-decoration:none;}
	    
   </style>
   <title> Search Result | Case Law | eLaw.my</title>
   <link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
    <link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
      <script type="text/javascript">
          $(document).ready(function () {
              $('#txtSearch').keypress(function (event) {
                  if (event.which == 13) {
                      event.preventDefault();
                      LinkButton1.click();
                  }
              });

              if ($('#ddlTitle').is(':disabled')) {
                  $('.rightResultBox').hide();
                  setTimeout("location.href='casesSearch.aspx'", 3000);
              }
              /////////////////////////////////////////////////////////
              //Check if not mobile
              if (typeof window.orientation == 'undefined') {
                  $(".tTitleCase a").hover(
                function () {
                    var classid = $(this).attr("id");
                    var dd;
                    ///////////////// call ajax 
                    $.ajax({
                        type: "POST",
                        url: "casessearchresult1.aspx/getToolTip",
                        data: '{SecReceiv: "' + classid + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            dd = data.d;
                            var elementTitle = document.getElementById(classid).title = dd
                            //$('.notesBox').html(data.d);
                        },
                        failure: function (response) {

                        }
                    });
                    //////////////////////////
                    $("#notesRight").fadeIn("slow");
                },
            function () {
                $("#notesRight").fadeOut("slow");
            }
            );
              } //End If for Checking Mobile
              ////////////////////////////////////////
              var top = $('#addNotes_div').offset().top - parseFloat($('#addNotes_div').css('margin-top').replace(/auto/, 0));
              $(window).scroll(function (event) {
                  // what the y position of the scroll is
                  var y = $(this).scrollTop();

                  // whether that's below the form
                  if (y >= top) {
                      // if so, ad the fixed class
                      $('#addNotes_div').addClass('fixed');
                  } else {
                      // otherwise remove it
                      $('#addNotes_div').removeClass('fixed');
                  }
              });

          });
   </script>
</head>

<body onload="CallFunctionUncheckBoxs()">
<form id="Form1" method="post" runat="server" name="form">

<!-- #include file="include/headerMain.aspx" -->


<div id="content2">
	<div class="clear"></div>
   
   
	<div id="contRight" style="margin-right: 85px; margin-left: 85px;">
    <div class="clear"></div>
    <section class="resultsRight">
      
      <div class="topBar1" 
            style="padding-top: 30px; border-left-style: none; border-right-style: none;">
    
    <div class="leftResultBox">
              <div class="funtionResults">
            <span style="float:left; display:block; padding:5px 5px 0 0;">  About 
     <asp:label id="lblCaseFound2"  runat="server" EnableViewState="False"></asp:label>
        (<asp:label id="lblSec" runat="server" Font-Size="Small" ></asp:label>s)
        <asp:label id="lblMsg" runat="server" ></asp:label>
     
     <asp:RequiredFieldValidator 
             ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSearch" 
             Display="Dynamic" ErrorMessage="Filed Required for search Result " 
                      Font-Size="Small" ForeColor="Red" 
             ValidationGroup="within"></asp:RequiredFieldValidator>
                  </span>
         </div></div>
    
<%--<div class="rightResultBox"  id="SendEmail">
    <div class="funtionResults">
    <span style="float:left; display:block; padding:5px 5px 0 0;" id="myDiv"></span>
    <input type="text" name="textfield" id="txtemail" class="advBoxS4" />
         
    </div>
     </div>--%>






    <!-- Right Bar1 Results Elements -->
    <div class="clear"></div>
    </div>


   
 	<!-- left Bar1 Results Elements -->
    
     <div class="topBar2">
    
    <div class="leftResultBox">
    <table width="840" border="0">
  <tr>
    <td width="10" style="padding-right: 0px"><input type="checkbox" name="item0[]" onclick="CheckAll(this)"></td>
    <td width="40" class="tfont2" style="padding-left: 0px"> Sort by</td>
    <td width="120">
     <asp:dropdownlist id="ddlTitle" runat="server" 
            class="advBoxS5" style=" height:25px; margin:0 5px; width:120px;">
            <asp:ListItem>Date</asp:ListItem>
            <asp:ListItem>Federal/Supreme Court</asp:ListItem>
              <asp:ListItem>Court of Appeal</asp:ListItem>
               <asp:ListItem>High Court</asp:ListItem>
                <asp:ListItem>Industrial Court</asp:ListItem>
                <asp:ListItem>Judge Name</asp:ListItem>
            <asp:ListItem>Title Ascending</asp:ListItem>
            <asp:ListItem>Title Descending</asp:ListItem>
                       </asp:dropdownlist>
    
    
    </td>   <td width="40" class="tfont2" style="padding-left: 0px"> Filter by</td>
    <td width="100">
     <asp:dropdownlist id="sfilter" runat="server" 
            class="advBoxS5" style=" height:25px; margin:0 5px; width:140px;">
            
            <asp:ListItem>None</asp:ListItem>
            <asp:ListItem>Federal/Supreme Court</asp:ListItem>
              <asp:ListItem>Court of Appeal</asp:ListItem>
               <asp:ListItem>High Court</asp:ListItem>
                <asp:ListItem>Industrial Court</asp:ListItem>
            </asp:dropdownlist>
    
    
   </td>
     <td class="tfont2" width="110">Search within
     
     results</td>
    <td width="240"><div class="btnGo" 
            style="float:right; padding:4px 12px;">
    <asp:LinkButton ID="LinkButton1" runat="server" ValidationGroup="within" >Search</asp:LinkButton>
    
    
    </div>
        <asp:TextBox id="txtSearch" runat="server" CssClass="advBoxS5" Width="180px"></asp:TextBox>
      </td>
           
  </tr>
  
</table>
    </div>
    
    
    <div class="rightResultBox">
    <div class="funtionResults">
   <!-- <a href="#" onclick='openmulti()'><img border="0" src="img/icoOpenMulti.gif"/>OpenMulti</a>-->
  <!--  <a href="#" onclick='printItn()'><img border="0" src="img/icoPrint.gif"/>Print</a>-->
    <!-- <a href="#" onclick="popupdiv()"><img border="0" src="img/icoEmail.gif" />Email</a>-->
    <%--<a href="#" ><img border="0" src="img/icoDownload.gif"/>Download</a>--%>
    <a style="cursor:pointer" onclick="popup('popupDiv2')"><img border="0" src="img/icoSave.gif"/>Save Search</a>
    </div>
     </div>
    
    <div class="clear"></div>
    </div>
    
    <!-- left Bar1 Results Elements 2 -->
    
   
   <div style="margin-bottom: 80px" id="me"> 
   <%--<div id="notesRight"><div id="addNotes_div"><div class="notesBox"> </div> </div> </div>--%>
     <asp:label id="lblTbl" runat="server" 
           Width="100%"></asp:label>
   <asp:label id="lblPgNo" runat="server" Visible="False"></asp:label>
   </div>

    
    </section>
    
    <div class="clear"></div>
  	</div>
</div>
                        

<!-- footer -->

<footer> 
	<div class="wrapperFooter">
	<section class="wrapperAddfooter">
   <aside class="gotoPage" >
     <div class="gotoBox1" style="padding: 2px; margin-left: 35px">
           <span class="tfont2">
         New Search</span>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
               ControlToValidate="txtExactPhrase" Display="Dynamic" ErrorMessage="*" 
               Font-Size="Large" ForeColor="Red" ValidationGroup="new"></asp:RequiredFieldValidator>
          <asp:textbox  runat="server"  ToolTip="Search Within these documents"
MaxLength="200" class="advBoxS5" ID="txtExactPhrase" style="width:180px" ></asp:textbox>
          <asp:button id="btnSearch" runat="server"  Text="Search" 
                 class="btnSave1" ValidationGroup="new" style="width:70px" ></asp:button>&nbsp;<asp:button 
               id="btnClearSearchHistroy" runat="server"  
        Text="Clear" class="btnSave1" Visible="False"></asp:button>
          </div>
          
     <ul>
     <asp:label id="lblBottomNavigation" runat="server" CssClass="SpanPage" ></asp:label>
     </ul>
    </aside>
   
    
    
    <div class="clear"></div>
    </section>

	<!-- #include file="include/footerMain.aspx" -->

    </div>
</footer>
      <div id="blanket" style="display:none;"></div>
	<div id="popUpDiv" style="display:none;width:500px;" >
	
	 
    <div id='form2' >	
	<h3><span>Send Case(s) To</span></h3>
    <div style="margin-left:50px;">
          <p>  <span id='msg'></span>  </p>
          <p class='submit' style="display:none;" id="Close">
             <button  type='button' value='Submit' id='Button2' onclick="popup('popUpDiv')">Close & Continue!</button>
                </p>
		
                <table style="width: 100%;">
                    <tr>
                        <td>
                            Email
                        </td>
                        <td>
                           <input type="text" name="name" id="Email" maxlength="50" class="advBoxS5" style="width:200px;"/>
                        </td>
                        
                    </tr>
                    <tr>
                        
                        <td>
                           Format
                        </td>
                        <td>
                           <select id="SelectFormate" class="advBoxS5" style="width:202px;">
				<option value="1">PDF</option>
				<option value="2">HTML</option>
				<option value="3">Message </option>
				</select>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                           Subject
                        </td>
                        <td>
                           <input type="text" name="subject" id="subject" maxlength="50" class="advBoxS5" style="width:200px;"/>
                        </td>
                    </tr>
                    <tr>
                    <%--<td colspan="2"> <div><input name="saveEmail" id="sve" type="checkbox" style="width:10px;height:10px;margin-left:40px;margin-right:10px;"/><span>Save Email</span></div></td>--%>
                    </tr>
                    <tr>
                    <td valign="top"">Message</td>
                    <td><textarea rows="3" cols="40" id="message" class="advBoxS5" style="width:200px;resize:none;" ></textarea></td>
                    </tr>
                    <tr>
                    <td></td>
                    <td><p class='submit'><button id='btnSubmit' type='button' value='Submit' onclick="displayResult()" class="btnGo">Send</button>
                            <button  type='button' value='Submit' id='btnClose' onclick="popup('popUpDiv')" class="btnGo">Cancel</button>
                </p>
                </td>
                    </tr>
                </table>
					
				
	        </div>
               </div>

	</div>	

    <%--<div id="blanket2" style="display:none;"></div>--%>
	<div id="popupDiv2" style="display:none;width:300px;">
	
	 
    <div id='form3' >	
	<h3><span>Save Search Result</span></h3>
			<fieldset id="fieldset1">
            <table style="margin-left:25px;">
            <tr>
            <td style="padding:5px 10px;">
			<span id='Span1' >Name Your Search Result :</span>
            </td>
            </tr>
            <tr>
			<!--<input type="text" name="subject" id="saveWithName" maxlength="2" size="40" />-->
            <td><asp:TextBox runat="server" Width="195px" name="subject" id="saveWithName" size="40" CssClass="advBoxS5"></asp:TextBox></td>
            </tr>
			</table>
			<p class='submit' style="margin-left:40px;"><button  type='button' id='saveSearch' runat="server" class="btnGo">Save</button>
            <button  type='button' value='Submit' id='Button4' onclick="popup('popupDiv2')" class="btnGo">Cancel</button>
            </p>
            </fieldset>
            </div>
	</div>

    <div id="popupDiv3" style="display:none;background-color:#757575;font-size:14px;padding:0px 0px 5px  0px;-webkit-box-shadow:0 1px 1px #000;-moz-box-shadow:0 1px 1px #000;box-shadow:0 1px 1px #000;border-radius:5px;position:absolute;width:350px;z-index: 9002;" >
        <div class="form4">
        <h3><span>Error!</span></h3>
        <fieldset>
            <p style="text-align:center">Please Select Case(s)</p>
            <button type="button" onclick="popup('popupDiv3')" class="btnGo" style="margin-left:130px;margin-top:20px;">Close</button>
        </fieldset>
        </div>
    </div>


</form>
    <script src="js/script.js" type="text/javascript"></script>

<script type="text/javascript">
    var total = "";
    var checkboxLength = document.getElementById("me");
    var checkboxes = checkboxLength.getElementsByTagName("input");
    function CallFunctionUncheckBoxs() {
        for (var i = 0; i < checkboxes.length; ++i) {
            checkboxes[i].checked = false;
        }
    }

    //////////////check all boxs 
    function CheckAll(x) {
        var allInputs = document.getElementsByName(x.name);
        for (var i = 0, max = allInputs.length; i < max; i++) {
            if (allInputs[i].type == 'checkbox') {
                if (x.checked == true) {
                    allInputs[i].checked = true;

                } else
                { allInputs[i].checked = false; }
            }
        }
    }
    ////////////end of check all 
    ////Get all checked boxs

    function popupdiv() {
        $('#Email').val('');
        $('#SelectFormate').val($("#SelectFormate option:eq(1)").val());
        $('#subject').val('');
        $('#message').val('');
        $('#msg').html('');
        total = "";
        for (var i = 0; i < checkboxes.length; ++i) {
            if (checkboxes[i].checked) {
                if (checkboxes[i].value != "on") {
                    total += checkboxes[i].value + ",";
                }

            }
        }
        if (total.length == 0) {
            //alert("Please Select Cases");
            popup('popupDiv3');
            return false;
        }
        else {
            popup('popUpDiv');
            $.ajax({
                type: "POST",
                url: "EmailGroup.aspx/getEmails",
                //data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //console.log(data.d);
                    $("#Email").autocomplete({
                        //                            source: function (req, responseFn) {
                        //                                var re = $.ui.autocomplete.escapeRegex(req.term);
                        //                                var matcher = new RegExp("^" + re, "i");
                        //                                var a = $.grep(data.d, function (item, index) {
                        //                                    return matcher.test(item);
                        //                                });
                        //                                responseFn(a);
                        //                            }
                        source: data.d
                    });
                }
            });
        }

    }

    function displayResult() {


        //////////////////////////////////////////////
        // document.getElementById("SendEmail").style.display = "block";
        var email = document.getElementById("Email").value;
        var selectformate = document.getElementById("SelectFormate").value;
        var message = document.getElementById("message").value;
        var subject = document.getElementById("subject").value;
        var sve = "";
        if (email.length == 0) {
            //alert("Please Type your Email");
            $('#Email').css('background-color', '#ffd0d0');
            return false;
        }
        if (selectformate.length == 0) {
            //alert("Please Select Message Format");
            $('#SelectFormate').css('background-color', '#ffd0d0');
            return false;
        }
        if (subject.length == 0) {
            //alert("Please Type Subject");
            $('#subject').css('background-color', '#ffd0d0');
            return false;
        }
        if (message.length == 0) {
            //alert("Please Type Message");
            $('#message').css('background-color', '#ffd0d0');
            return false;
        }
        var Result = Checkemail(email)
        if (Result) {
            //sve = (document.getElementById("sve").checked) ? "Y" : "N";
            sve = "Y";
            document.getElementById("msg").innerHTML = "Sending Email";

            var xmlhttp;
            if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp = new XMLHttpRequest();
            }
            else {// code for IE6, IE5
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                    document.getElementById("msg").innerHTML = xmlhttp.responseText;
                    //document.getElementById("fieldset").style.display = "none";
                    if (document.getElementById("msg").innerHTML = "Email successfully sent.") {
                        //document.getElementById("fieldset").style.display = "none";
                        //document.getElementById("Close").style.display = "block";
                        //document.getElementById("popUpDiv").style.height = "200px";
                        setTimeout(function () { popup('popUpDiv'); }, 2000);
                    } else document.getElementById("msg").innerHTML = "Failed, Please check your connection";

                }
            }
            xmlhttp.open("POST", "EmailGroup.aspx?email=" + email + "&mypageid=" + total + "&sbj=" + subject + "&msgf=" + selectformate + "&msg=" + message + "&sve=" + sve, true);
            xmlhttp.send();



            //////////////////////////////////////////


        }
        else
        { alert("Please Type Correct Email"); }


    }
    ///////////////////////////////////end of collect value

    function openmulti() {
        var flagchecked = false;
        for (var i = 0; i < checkboxes.length; ++i) {
            if (checkboxes[i].checked) {
                flagchecked = true;
                var myurl = "case_notes/showcase.aspx?id=" + checkboxes[i].value;
                var win = window.open(myurl, '_blank');
                win.focus();
            }
        }
        if (!flagchecked)
            alert('at least check a case');
        return false;
    } //end of open multi function 


    //////check Emmail 



    function Checkemail(value) {

        return /^[\w\d][\w\d\-\._]+[\w\d]@[\w\d][\w_\-\.]*[\w\d]\.[\w]+/.test(value); //64
        //return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value);
    }

    /////////////////////////////////////////
    ////////////////////////////////////////////////
    //Print page
    window.printItn = function () {
        total = "";
        //var printContent = document.getElementById('mydiv');
        var printContent = "";
        for (var i = 0; i < checkboxes.length; ++i) {
            if (checkboxes[i].checked) {
                if (checkboxes[i].value != "on") {
                    total += checkboxes[i].value + ",";
                }

            }
        }
        /////////////////////////////////////////////
        if (total == 0) {
            //alert("Please Select Files");
            popup('popupDiv3');
        }
        else {
            var xmlhttp;
            if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp = new XMLHttpRequest();
            }
            else {// code for IE6, IE5
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                    printContent = xmlhttp.responseText;

                    var windowUrl = 'about:blank';
                    var uniqueName = new Date();
                    var windowName = 'Print' + uniqueName.getTime();

                    //  you should add all css refrence for your html. something like.

                    var WinPrint = window.open(windowUrl, windowName, 'left=30,top=30,right=500,bottom=500,width=700,height=500');
                    WinPrint.document.write('<' + 'html' + '><head><link href="../css/style_New.css" rel="stylesheet" type="text/css" /></head><' + 'body style="background:none !important"' + '>');
                    WinPrint.document.write('<style> P{text-align:justify;display:block;line-height:25px;font-family:Georgia, Times New Roman, Serif;margin:5px 0;color:#000;} #bgid{background-image:url(../img/logoHome.png);}</style>');
                    WinPrint.document.write(printContent);

                    WinPrint.document.write('<' + '/body' + '><' + '/html' + '>');
                    WinPrint.document.close();
                    WinPrint.focus();
                    WinPrint.print();
                    WinPrint.close();
                    return false;
                }
            }
            xmlhttp.open("POST", "PrintGroup.aspx?mypageid=" + total, true);
            xmlhttp.send();

        }
    }
    /////////////////////////////////////////////


        
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
