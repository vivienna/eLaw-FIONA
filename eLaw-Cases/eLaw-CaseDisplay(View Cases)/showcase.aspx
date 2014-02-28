<%@ Page Language="VB" AutoEventWireup="false" CodeFile="showcase.aspx.vb" Inherits="showcase" %>
<%@ Register Assembly="AjaxControlToolkit" 
Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
         <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        
         <title>Case View | eLaw.my</title>
         <link id="Link1" runat="server" rel="shortcut icon" href="../GUI/favicon.ico" type="image/x-icon"/>
         <link id="Link2" runat="server" rel="icon" href="../GUI/favicon.ico" type="image/ico"/>
         <link id="MyStyleSheet" rel="stylesheet" type="text/css" runat="server" />

   <script src="../include/css-pop.js" type="text/javascript"></script>
    <script src="../js/libs/jquery-1.7.1.js" type="text/javascript"></script>
  
    <script src="../js/script.js" type="text/javascript"></script>
    <link href="../css/style_New.css" rel="stylesheet" type="text/css" />
   
<script type="text/javascript">
    
    
     /////////////////////////////////////////BeforeSubmit
    
   //abdo work start
    function resetddl() {



    var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
       lstddlrbs.options[lstddlrbs.selectedIndex].value=1;
        
    var lstddlproxmity = document.getElementById('<%=ddlproxmity.ClientID%>');
       lstddlproxmity.options[lstddlproxmity.selectedIndex].value=3;
        
        var lstddlprox = document.getElementById('<%=ddlprox.ClientID%>');
       lstddlprox.options[lstddlprox.selectedIndex].value="";
       lstddlprox.options[lstddlprox.selectedIndex].text="";
       }//end of function reset drop down list to their 

   //abdo work end
       function CheckSubjectIndex() {

           //abdo work alertstart
           resetddl(); //reset drop down list srch options


           var ddlvalue = "";
           var ddltext = "";
           var lstddlsrch = document.getElementById("<%=ddlsrch.ClientID%>");
           ddlvalue = lstddlsrch.options[lstddlsrch.selectedIndex].value;
           ddltext = lstddlsrch.options[lstddlsrch.selectedIndex].text;

           ddltext = ddltext.toLowerCase();


           if (ddltext.indexOf("judgment") > -1) {
               var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
               lstddlrbs.options[lstddlrbs.selectedIndex].value = 1;
               // alert("you choose judment");
           }

           else
               if (ddltext.indexOf("title") > -1) {

                   var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
                   lstddlrbs.options[lstddlrbs.selectedIndex].value = 2;
                   //alert("you choose case title");
               }
               else
                   if (ddltext.indexOf("word") > -1) {
                       var lstddlprox = document.getElementById('<%=ddlprox.ClientID%>');
                       lstddlprox.options[lstddlprox.selectedIndex].value = ddlvalue;
                       lstddlprox.options[lstddlprox.selectedIndex].text = ddltext;

                       //alert("you choose within words");
                   }

                   else
                       if (ddltext.indexOf("same") > -1) {

                           var lstddlproxmity = document.getElementById('<%=ddlproxmity.ClientID%>');
                           lstddlproxmity.options[lstddlproxmity.selectedIndex].value = ddlvalue;

                           //   alert("you choose same ");

                       }


          
       }//
       //abdo work end
    
    ///////////////////////////////////////////////////////////////64
       function validate() {
           CheckSubjectIndex();
        
        if ($('#txtExactPhrase').val().trim().length == 0 ) {
            $('#txtExactPhrase').css('background-color', '#ffd0d0');
        } else
                     {
            $('#txtExactPhrase').css('background-color', '#ffffff');
            
            __doPostBack('LinkButton1', '');
        }
    }
    $(document).ready(function () {
        var oldrep = "";
        var curr = 0;
        var fndsz = 0;
        function callback(match, p1) {
            return ((p1 == undefined) || (p1 == '')) ? match : '<span class="found">' + match + '</span>';
        }
        function surroundTags(search) {
            $('.boxDisplay').html($('.boxDisplay').html().replace(new RegExp("<[^>]+>|&nbsp;|&amp;|(" + search + ")", 'ig'), callback));
        }

        function remTags() {
            $('.found').contents().unwrap();
        }
        //boxDisplay has the contents
        $('#findNext,#findPrev').click(function (ev) {
            var search = $('#findWord').val().trim();
            if (search != "")
            //if (replaced) {
                if (search != oldrep) {
                    //clear first
                    remTags();
                    oldrep = search;
                    //add tags and scroll to them
                    surroundTags(search);
                    if ($('.found').size()) {
                        fndsz = $('.found').size();
                        if (ev.target.id == 'findPrev') {
                            curr = fndsz - 1;
                            $('body,html').stop();
                            $('body,html').animate({ scrollTop: $('.found:eq(' + curr + ')').offset().top - 90 }, 300);
                            $('.found:eq(' + curr + ')').addClass('currentFound');
                            $('#FCount').html((curr + 1) + ' of ' + (fndsz));

                        } else {
                            $('body,html').stop();
                            $('body,html').animate({ scrollTop: $('.found:eq(0)').offset().top - 90 }, 300);
                            $('.found:eq(0)').addClass('currentFound');
                            $('#FCount').html('1 of ' + (fndsz));
                            curr = 0;
                        }
                    } else {
                        $('#FCount').html('');
                        curr = 0;
                        fndsz = 0;
                    }

                } else {
                    if ($('.found').size()) {
                        //jump to the next/prev and add currentFound class + remove the prev class
                        $('.found:eq(' + curr + ')').removeClass('currentFound');
                        if (ev.target.id == 'findPrev') {
                            curr = (--curr);
                            if (curr < 0)
                                curr = fndsz - 1;
                        } else
                            curr = (++curr) % fndsz;
                        $('body,html').stop();
                        $('body,html').animate({ scrollTop: $('.found:eq(' + curr + ')').offset().top - 90 }, 300);
                        $('.found:eq(' + curr + ')').addClass('currentFound');
                        $('#FCount').html((curr + 1) + ' of ' + (fndsz));
                    }

                }
        });




        if ($('#nofityBox').html().toString().trim().length < 1) {
            //alert('empty');
            $('#notification').hide();
        }

        $('#findX').click(function () {
            $('.findDiv').fadeOut('fast');
            $('.findToggle').slideDown('fast');
            remTags();
            oldrep = "";
            $('#findWord').val('');
            $('#FCount').html('');
            curr = 0;
            fndsz = 0;
        });

        $('.findToggle').hover(function () {
            $(this).css({ 'color': '#1E84C7', 'cursor': 'Pointer' });
        }, function () {
            $(this).css({ 'color': 'white', 'cursor': 'auto' });
        });

        $('.findToggle').click(function () {
            $('.findDiv').slideDown('fast');
            $(this).slideUp('fast');
            $('#findWord').focus();
        });

        $('#findWord').keypress(function (event) {
            if (event.which == 13) {
                event.preventDefault();
                $('#findNext').trigger('click');
            }
        });
        var ctrl = 17, f = 70, esc = 27, ctrlDn = false;
        $(window).keydown(function (event) {
            if (event.which == ctrl)
                ctrlDn = true;
            else if (event.which == f && ctrlDn) {
                event.preventDefault();
                $('.findToggle').click();
                return false;
            } else if (event.which == esc) {
                event.preventDefault();
                $('#findX').click();
                return false;
            }
        }).keyup(function (event) {
            if (event.which == ctrl)
                ctrlDn = false;
        });


        //        $(window).keydown(function (event) {
        //            if (!(event.which == 102 && event.ctrlKey) && !(event.which == 19)) { alert(event.which + ''); return true; }
        //            alert(event.which + '');
        //            event.preventDefault();
        //            $('.findToggle').click();
        //            return false;
        //        });

        //////////////////////////////////////////////// --
        //Print page 
        window.printItn = function () {

            var printContent = document.getElementById('mydiv');
            var title = document.getElementById('title').innerHTML;
           
            var windowUrl = "";
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();

            //  you should add all css refrence for your html. something like.

            var WinPrint = window.open(windowUrl, windowName, 'left=30,top=30,right=500,bottom=500,width=700,height=500');
            WinPrint.document.write('<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">');
            WinPrint.document.write('<' + 'html' + '><head><link href="../css/style_New.css" rel="stylesheet" type="text/css" media="print" /></head><' + 'body style="background:none !important"' + '>');
            WinPrint.document.write('<style> P{text-align:justify;display:block;line-height:25px;font-family:Georgia, Times New Roman, Serif;margin:5px 0;color:#000; background-image:url(../img/bg1.gif); } </style>');
            WinPrint.document.write(' <div > <img border="0" src="../img/logoHome.png" align="middle"/></div>');
            WinPrint.document.write(printContent.innerHTML);

            WinPrint.document.write('<' + '/body' + '><' + '/html' + '>');
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }
        //////////////////////////////////////////////////////////////64
        $('#saving').click(function () {
            if ($(this).attr('Saved') == "notSaved") {
                $.ajax({
                    type: "POST",
                    url: 'showcase.aspx/saveDoc',
                    data: '{ pageid : "' + $('#fileNameStore').html().trim() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d) {
                            $('#saveMessage').html("Document Successfully Saved!");
                            $('#saving').html("<img border='0' src='../img/icoSave.gif'/>Saved Already");
                            $('#saving').attr('Saved', 'saved');
                            $('#saving').css({ 'text-decoration': 'none', 'cursor': 'default' });
                        } else {
                            $('#saveMessage').html("Saving Failed!");
                        }
                    },
                    failure: function (response) {
                        $('#saveMessage').html("Saving Failed! not connected to the server");
                    }
                });
                popup('popupDiv');
            }
        });
        //////////////////////////////////////////////////////////// --
        $("#btnShowModal").click(function (e) {
            $('#txtEmail').val('');
            $("#msg").html('');
            $("#subject").val('');
            $('#message').val('');
            ShowDialog(true);
            e.preventDefault();
        });

        $("#btnClose").click(function (e) {

            HideDialog();
            e.preventDefault();
        });

        ///////////////////////////////////////////////////////////////////64
        $('#panel').keypress(function (event) {
            if (event.which == 13) {
                event.preventDefault();
                __doPostBack('LinkButton1', '');
            }
        });

        $("#btnSubmit").click(function (e) {
            var sEmail = $('#txtEmail').val();
            var sSubject = $("#subject").val();
            var sMessage = $("#message").val();
            var selectformate = document.getElementById("SelectFormate").value;
            if ($('#txtEmail').val().length == 0) {
                $("#msg").html("Please Provide your Email");
                return false;
            } else {
                var id = $("#lblid").html();
                var result = email(sEmail);
                var Idtitle = $("#title").html();
                if (result) {
                    var dataString = {
                        email: sEmail,
                        mypageid: id,
                        title: Idtitle,
                        format: selectformate,
                        sub: sSubject,
                        msg: sMessage

                    };
                   
                    $.ajax({
                        type: "POST",
                        url: "../emial.aspx",
                        data: dataString,
                        cache: false,
                        success: function (html) {
                            if (html) {
                              
                                $("#msg").html(html);
                                setTimeout(HideDialog, 2000); // 64
                                //e.preventDefault();
                            }
                        }, failure: function (html) {
                            if (html) {
                                $("#msg").html(html);
                                setTimeout(HideDialog, 2000); // 64
                                //e.preventDefault();
                            }
                        }
                    }); //end of ajax


                }
            }
        });
    });
    /////////////////////////////////////////////////////// --
    function email(value) {
        return /^[\w\d][\w\d\-\._]+[\w\d]@[\w\d][\w_\-\.]*[\w\d]\.[\w]+/.test(value); // 64
        //return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value);
    }



    function ShowDialog(modal) {
        $("#overlay").show();
        $("#form2").fadeIn(300);

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
        $("#form2").fadeOut(300);

    }
    //////////////////Show CaseProgression
    function CaseProgression() {
        $("#nofityBox").slideToggle();
    }
    $(document).mouseup(function () {
        $("#nofityBox").slideUp();
    });

    </script>
    <style type="text/css">
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
            left: 42%;
            
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
		    /*scrol top*/
#back-top {
	position: fixed;
	bottom: 5px;	
}		    


#back-top a {
	display: block;
	text-align: center;
	margin:0 0 40px 27px;
	/* background color transition */
	-webkit-transition: 1s;
	-moz-transition: 1s;
	transition: 1s;
}
#back-top a:hover {
	color: #000;
}
/* arrow icon (span tag) */
#back-top span {
	width: 65px;
	height: 60px;
	display: block;
	margin:0 0 15px 43px;
	padding:8px 0 0 0;
	font-size:10px;
	color:#000000;
	font-weight:bold;
	/*background: #ddd url(../GUI/images/up-arrow.png) no-repeat center center; */
	/* rounded corners */
	-webkit-border-radius: 5px;
	-moz-border-radius: 5px;
	border-radius: 5px;
	/* background color transition */
	-webkit-transition: 1s;
	-moz-transition: 1s;
	transition: 1s;
}
#back-top a:hover span {
	background-color: #777;
}



   </style>
</head>
<body >
    <form id="form1" runat="server">


<!-- #include file="../include/headerMain2.aspx" -->


<div class="findToggle">
    FIND &nbsp; <img src="../img/arrowDn.png" />
 </div>
<!--    Close header-->



<!-- Content 2 -->
 <div class="clear"></div>
<div id="content2">
	 
<div class="clear"></div>
    <div id="showPanel">
	<div class="leftQuicksearch">
    <div class="btnShowSearch"><img style="margin:8px 0 0 8px;" align="top" src="../img/ico2.png"/></div>
    <p> Search</p></div>
    </div>
    <div id="colleft">
      <div id="panel">
        <h1>Search
          <div id="hidePanel" class="hideClosePanel" style="position: relative; top:-22px; right:-210px;">
          <a href="#"><img style="margin:4px 0 0 8px;" align="middle" src="../img/ico1.png"/></a>
          </div>
        </h1>
        <section id="defineSearch">
                    <table width="100%" border="0" cellpadding="4" cellspacing="1">
          
            <tr>
              <td><asp:TextBox  ID="txtExactPhrase" runat="server" class="advBoxS" Width="100%"></asp:TextBox>
                </td>
            </tr>
             <tr><td>Within:</td></tr>
                  <tr>
                    <td> 
                    
                  <asp:DropDownList ID="ddlsrch" runat="server" class="advBoxS6" 
                          ClientIDMode="Static">
                    <asp:ListItem Selected="True" Value="1">Judgment</asp:ListItem>
                    <asp:ListItem Value="2">Case title</asp:ListItem>
                    <asp:ListItem Value="3">3 Words</asp:ListItem>
                    <asp:ListItem Value="4">4 Words</asp:ListItem>
                    <asp:ListItem Value="5">5 Words</asp:ListItem>
                    <asp:ListItem Value="1">same sentence</asp:ListItem>
                    <asp:ListItem Value="2">same paragraph</asp:ListItem>
                    <asp:ListItem Value="3">same page</asp:ListItem>
                    </asp:DropDownList>



                    <asp:DropDownList ID="rbs" runat="server" class="advBoxS" style="visibility:hidden; height:25px; width:100%; ">                  
                      <asp:ListItem Selected="True" Value="1">Full Document</asp:ListItem>
                      <asp:ListItem Value="2">Case Title</asp:ListItem>
                  </asp:DropDownList>
                    
                     </td>
                  </tr>
                  <tr>
                    <td>Without the words:</td>
                  </tr>
                  <tr>
                    <td>  <asp:TextBox ID="txtNotCases" runat="server" class="advBoxS" Width="100%"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td><table width="100%" border="0" cellpadding="5" cellspacing="0">
            <tr>
              <td width="25%"><!--Within--></td>
              <td width="75%" colspan="2"><asp:DropDownList 
                          ID="ddlproxmity" runat="server" CssClass="advBoxS6" Width="160px" style="visibility:hidden;">
                      <asp:ListItem Value="3"> Same Page</asp:ListItem>
                      <asp:ListItem Value="2"> Same Paragraph</asp:ListItem>
                      <asp:ListItem Value="1"> Same Sentence</asp:ListItem>
                      </asp:DropDownList></td>
            </tr>

            <tr>
              <td width="25%"><!--Within--></td>
              <td width="50%" ><asp:dropdownlist id="ddlprox" runat="server" 
       class="advBoxS6" style="visibility:hidden; height:25px; width:100px;" EnableViewState="False">
                    <asp:ListItem Value=""></asp:ListItem>
                   
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                    
                      </asp:dropdownlist></td>
                     
            </tr>
            
          </table></td>
                  </tr>
                  <tr>
                    <td><asp:checkbox id="cbIndustrialCourt" runat="server" class="tick3"
                  Text="Include Industrial Court Awards"  ToolTip="Check for including the industrial court cases for search"></asp:checkbox></td>
                  <tr>
              <td align="center"><div class="btnDefine" onclick="validate()">Search
              <asp:LinkButton ID="LinkButton1" 
                      runat="server"  ValidationGroup="extraSearch" style="display:none;">Search</asp:LinkButton>
                      </div>
                      </td>
            </tr>
          </table>

        </section>
        
        <%--<div id="expandResults">
          <li style="padding:0 0 10px 0; margin:0;">
            <h1>Recent Results <a href="#" style="color:#f00000; text-decoration:none; font-weight:bold;">(2)</a></h1>
            <div class="arrow"><img src="../img/arrowEx2.png"/></div>
            <div class="scrollBoxWrapper">
              <!-- Recent Result Box 1 -->
              <
              <!-- Recent Result Box 2 -->
              
            </div>
          </li>
        </div>
        <div id="expandResults">
          <li style="padding:0 0 10px 0; margin:0;">
            <h1>Recent Results <a href="#" style="color:#f00000; text-decoration:none; font-weight:bold;">(2)</a></h1>
            <div class="arrow"><img src="../img/arrowEx2.png"/></div>
            <div class="scrollBoxWrapper">
              <!-- Recent Result Box 1 -->
              
              <!-- Recent Result Box 2 -->
              
            </div>
          </li>
        </div>--%>
      </div>
    </div>
    
    
    
	<div id="contRight">
    
    <section class="resultsRight">
    <div class="findDiv" style="display:none">
    <a style="margin-right:5px;padding:1px;cursor:pointer;" id="findX"><img src="../img/x.png" /></a>
    <span id="FCount"></span>
    <input type="text" size="30" id="findWord"/>
    <button type="button" id="findPrev">▲</button>
    <button type="button" id="findNext">▼</button>
    </div>
    <div class="clear"></div>
    
 	<!-- left Bar1 Results Elements -->
    
    
    
    <!-- left Bar1 Results Elements 2 -->
    
    <div class="topBar2" id ="topBarCase" >
    
    <div class="leftResultBox2" id ="TopLeftCase" runat="server">
    <div class="funtionResults">
    <span id="notification" onclick="CaseProgression()">
     
               <a href="#" id="A1"><img border="0" src="../img/icoEmail.gif"/>Progression</a>
                <span style="clear:both"></span>
                <span id="nofityBox" style="text-decoration:none">
                
                	
                    <% =CasePrograssion%>
                </span>
            </span> 
   
    <%Response.Write(CaseCitator)%>
    <a href="../CasesPdfOpener.aspx?<% Response.Write(OpenFileName)%>"><img border="0" src="../img/icoPDF2.gif" alt="View Case as PDF. Elaw Library"/>PDF</a>
        <% =HeadNote%>
        
    </div>
    </div>
   

    



    <div class="rightResultBox" id="TopRightCase" runat="server">
    <div class="funtionResults"> 
    
    
         <a href="#" id="print" onclick='printItn()'><img border="0" src="../img/icoPrint.gif"/>Print</a>
    
   <a href="#" id="btnShowModal"><img border="0" src="../img/icoEmail.gif"/>Email</a>
    <a href="../CasesPdfOpener.aspx?<% Response.Write(DownloadFileName) %>"><img border="0" src="../img/icoDownload.gif"/>Download</a>
    <asp:Label ID="fileNameStore" runat="server" style="display:none"></asp:Label>
    <asp:Label ID="saving" runat="server" style="cursor:pointer;"></asp:Label>
    </div>
     </div>
    
    <div class="clear"></div>
    </div>
    
    <!-- Box Display Cases -->
    
    <div class="boxDisplay">





  
            <label for="notesArea"></label>
            <div class="clear1"></div> 
                <div name="notesArea" id="mydiv" ><!--<input type=text id="txtcurpage" value="here is the current page"/>-->
                
                <asp:Literal ID="ltrshowcase" runat="server"></asp:Literal>
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                  
                    
                  
                </div>
				<asp:Label ID="lblid" runat="server"></asp:Label>
<footer> 
	<div class="wrapperFooter">
	 
    <p id="back-top" style="display:none;">
		<a href="#top"><img src="../GUI/images/up-arrow.png" border="0" /></a>
	</p>
   
    <div class="clear"></div>

    <!-- #include file="../include/footerMain.aspx" -->
  
	</div>
</footer>




                     
           
            
    </div>

    </section>
    
    <div class="clear"></div>
  	</div>
</div>



     
                
                 
 <div id="overlay" class="web_dialog_overlay"></div>
           
               <div id="form2" class="web_dialog">
                   <h3><span>Send Case To</span></h3>
                <div style="margin:10px 30px">
                <table style="width: 100%;">
                       <tr>
                           <td>
                               Email
                           </td>
                           <td>
                              <input type="text" name="name" id="txtEmail" maxlength="50"  class="advBoxS5" style="width:225px;" />
                           </td>           
                       </tr>
                        <tr>
                        
                        <td>
                           Format
                        </td>
                        <td>
                           <select id="SelectFormate" class="advBoxS5" style="width:225px;">
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
                           <input type="text" name="subject" id="subject" maxlength="50" class="advBoxS5" style="width:225px;"/>
                        </td>
                    </tr>
                    <tr>
                    <td valign="top">Message</td>
                    <td><textarea rows="3" cols="40" id="message" style="width:222px;resize:none;"></textarea></td>
                    </tr>
                       <tr>
                       <td></td>
                       <td > <span id="msg"></span></td></tr>
                       <tr> <td></td>
                       <td >
                       <p class="submit"><button id="btnSubmit" type="button" value="Submit" class="btnGo">Send</button>
                            <button  type="button" value="Submit" id="btnClose" class="btnGo">Cancel</button>
                
                       </p>
                       </td>
                       </tr>
                   </table>	
                   </div>
    


    </div>
          
             <div id="blanket" style="display:none;"></div>           																		
	        <div id="popupDiv" style="display:none;background-color:#757575;font-size:14px;padding:0px 0px 5px  0px;-webkit-box-shadow:0 1px 1px #000;-moz-box-shadow:0 1px 1px #000;box-shadow:0 1px 1px #000;border-radius:5px;position:absolute;width:350px;z-index: 9002;" >
                <div class="form4">
                <h3><span>Message</span></h3>
                <fieldset>
                    <p id="saveMessage" style="text-align:center"></p>
                    <button type="button" onclick="popup('popupDiv')" class="btnGo" style="margin-left:130px;margin-top:20px;">Close</button>
                </fieldset>
                </div>
            </div>

              <script type="text/javascript">
                  $(document).ready(function () {
                      ///////////////////////////////////
                      $("#panel").animate({ marginLeft: "-266px" }, 5);
                      //$("#colleft").animate({ width: "0px", opacity: 0 }, 4
                      $("#colleft").css({ 'width': '0px', 'opacity': '0' });
                      $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
                      $("#contRight").animate({ marginLeft: "50px" }, 5);
                      //////////////////////////////////


                      $(window).scroll(function () {
                          if ($(this).scrollTop() > 100) {
                              $('#back-top').fadeIn();
                          } else {
                              $('#back-top').fadeOut();
                          }
                      });
                      // scroll body to 0px on click
                      $('#back-top a').click(function () {
                          $('body,html').animate({
                              scrollTop: 0
                          }, 800);
                          return false;
                      });
                      /////////////////////////////////////
                      $("#hidePanel").click(function () {
                          $("#panel").animate({ marginLeft: "-266px" }, 500);
                          $("#colleft").animate({ width: "0px", opacity: 0 }, 400);
                          $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
                          $("#contRight").animate({ marginLeft: "50px" }, 500);
                      });
                      $("#showPanel").click(function () {
                          
                          $("#contRight").animate({ marginLeft: "280px" }, 200);
                          $("#panel").animate({ marginLeft: "0px" }, 400);
                          $("#colleft").show().animate({ width: "266px", opacity: 1 }, 400).show("slow");
                          $("#showPanel").animate({ width: "0px", opacity: 0 }, 600).hide("slow");
                          $("#txtExactPhrase").focus();
                      });
                      $('#expandResults li').click(function () {
                          var text = $(this).children('.scrollBoxWrapper');
                          if (text.is(':hidden')) {
                              text.slideDown('200');
                              $(this).children('.arrow').html('<img src="../img/arrowEx1.png" />');
                          } else {
                              text.slideUp('200');
                              $(this).children('.arrow').html('<img src="../img/arrowEx2.png" />');
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
    </form>
</body>
</html>
