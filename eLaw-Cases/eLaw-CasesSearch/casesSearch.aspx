<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.casesSearch" CodeFile="casesSearch.aspx.vb"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
 <link rel="shortcut icon" href="GUI/NewDesign/favicon.ico" type="image/x-icon" /> 
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
     
     <link rel="stylesheet" href="css/style_New.css" />
   <link rel="stylesheet" href="css/subjectIndexBox.css" />
    
    <script src="js/libs/jquery-1.7.1.min.js" type="text/javascript"></script>
   <script src="include/jquery.ui.widget.js" type="text/javascript"></script>
       <script src="include/jquery.ui.tabs.js" type="text/javascript"></script>
        <script src="include/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <link href="css/AutoComplate.css" rel="stylesheet" type="text/css" />
    <link href="GUI/css/jquery.ui.tabs.css" rel="stylesheet" type="text/css" />
    

<title>Case Law | eLaw.my</title>
<link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>

<style type="text/css">
.ui-menu-item
{
    font-family: Arial, Helvetica, sans-serif; font-size: 14px
  }

    
    .style1
    {
        width: 46px;
    }

    
    .style2
    {
        width: 94px;
    }

    
    .style3
    {
        color: #000;
        font-size: 14px;
        direction: ltr;
        width: 274px;
    }

    
a.tip {
    cursor: pointer;
	display: inline-block;
	width: 16px;
	height: 16px;
	background-color: #89A4CC;
	line-height: 16px;
	color: White;
	font-size: 13px;
	font-weight: bold;
	border-radius: 8px;
	text-align: center;
	position: relative;
	text-decoration: none;
}
a.tip:hover {
    cursor: help;
    position: relative;
	background-color: #3D6199;
}
a.tip span {
    display: none
}
a.tip:hover span {
    border: #c0c0c0 1px dotted;
    padding: 5px 20px 5px 5px;
    display: block;
    z-index: 100;
	background-color:#cce7f7;
    left: -380px;
    margin: 10px;
    width: 350px;
    position: absolute;
    top: 10px;
    text-decoration: none;
	border-radius: 5px;
	text-align:justify;
	color: black;
	font-family: Arial, Helvetica, sans-serif;
	font-size:12px;
	font-weight: lighter;
}
a.tip b{padding: 5px 5px 5px 5px;}


</style>

<script type="text/javascript">
    function citation() {
                
        $.ajax({
            type: "POST",
            url: "casesSearch.aspx/GetCitation",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{pubAjax: "' + $('#CitPub option:selected').html() + '", yearAjax: "' + $('#CitYear option:selected').html() + '", volAjax: "' + $('#CitVol option:selected').html() + '", pg: "' + $('#CitPn').val() + '" }',
            success: function (data) {
               
              
                window.location.href = data.d;
            },
            error: function () {
                alert("Sorry but there is some error with internet connection");
            }
        });
        // window.location.href = "CasesSearchResult1.aspx?yy=" + $('#CitYear option:selected').html() + "&vol=" + $('#CitVol option:selected').html() + "&pub=" + $('#CitPub option:selected').html() + "&pg=" + $('#CitPn').val() + "&srt=";

    }
    $(document).ready(function () {
        //////////////////////////////////////
        $("#tabs").tabs();

        $('#tabs').bind('tabsselect', function (event, ui) {

            var selectedTab = ui.index;
           
            $(".tabsBox li").removeClass("current1");
            $("#tabcss" + selectedTab).addClass("current1");


        });

        ///////////////////////////////////
        $('#CitPn').keypress(function (event) {
            if (event.which == 13) {
                event.preventDefault();
                citation();
            }
        });
        //$('#CitYear').on('change', function () { alert($('#CitYear').val()) });

        $(".search").click(function () {
            if ($(".scrollBoxWrapper2").is(":hidden")) {
                $('.arrow2').html('▼');
            }
            else {
                $('.arrow2').html('▲');

            }
            $(".scrollBoxWrapper2").slideToggle("slow");

        });

        $(".hide_search_cri").click(function () {
            $(".search_cri").slideToggle("slow");
        });

        $('#CitPn').on('focus', function () {
            $.ajax({
                type: "POST",
                url: "casesSearch.aspx/sendJSON",
                data: '{pubAjax: "' + $('#CitPub option:selected').html() + '", yearAjax: "' + $('#CitYear option:selected').html() + '", volAjax: "' + $('#CitVol option:selected').html() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    
                    $("#CitPn").autocomplete({
                        source: function (req, responseFn) {
                            var re = $.ui.autocomplete.escapeRegex(req.term);
                            var matcher = new RegExp("^" + re, "i");
                            var a = $.grep(data.d, function (item, index) {
                                return matcher.test(item);
                            });
                            responseFn(a);
                        }
                        //source: data.d
                    });
                },
                failure: function (response) {
                }
            });
        });
        //////////////////////////////////////////////
        $('#txtExactPhrase').on('keyup', function () {
            var auto = $("#<%=txtExactPhrase.ClientID%>")[0].value;
            if (auto.length >= 2) {
                $.ajax({
                    type: "POST",
                    url: "casesSearch.aspx/Getautocomplate",
                    data: '{auto: "' + $("#<%=txtExactPhrase.ClientID%>")[0].value + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $("#txtExactPhrase").autocomplete({

                            source: data.d
                        });
                    },
                    failure: function (response) {
                    }
                });
            }
        });
        //////////////////////////////////////////////

        //////////////////////////////////////////////
        $('#legTitle').on('keyup', function () {
            var auto = $("#<%=legTitle.ClientID%>")[0].value;
            if (auto.length >= 2) {
                $.ajax({
                    type: "POST",
                    url: "legislationSearch.aspx/Getautocomplate",
                    data: '{auto: "' + $("#<%=legTitle.ClientID%>")[0].value + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $("#legTitle").autocomplete({

                            source: data.d
                        });
                    },
                    failure: function (response) {
                    }
                });
            }
        });
        //////////////////////////////////////////////
    });

    /////////////////////////////////////////BeforeSubmit
    
            function checkBoxList2OnClick(elementRef) {


                var checkBoxArray = elementRef.getElementsByTagName('input');

                // 'alert(checkBoxArray.length)


                for (var i = 0; i < checkBoxArray.length; i++) {
                    var checkBoxRef = checkBoxArray[i];
                    var labelArray = checkBoxRef.parentNode.getElementsByTagName('label');
                    var chklbltext = "";
                    chklbltext = labelArray[0].innerHTML;



                    if (checkBoxRef.checked == true && chklbltext.indexOf("   ") > -1 && chklbltext.indexOf("     ") == -1) {
                        //checkBoxRef.checked = false;

                        var strtemp = "";
                        strtemp = chklbltext + " "
                        labelArray[0].innerHTML = strtemp;

                        for (var j = i; j < checkBoxArray.length; j++) {
                            var checkBoxRef1 = checkBoxArray[j];
                            if (checkBoxRef.value == checkBoxRef1.value)
                                checkBoxRef1.checked = true;

                        } //end of nested for
                        continue;
                    } //end of if 


                    if (checkBoxRef.checked == false && chklbltext.indexOf("   ") > -1 && chklbltext.indexOf("     ") > -1) {
                        //checkBoxRef.checked = false;

                        var strtemp = "";
                        strtemp = chklbltext.replace("     ", "    ");
                        labelArray[0].innerHTML = strtemp;

                        for (var j = i; j < checkBoxArray.length; j++) {
                            var checkBoxRef1 = checkBoxArray[j];
                            if (checkBoxRef.value == checkBoxRef1.value)
                                checkBoxRef1.checked = false;

                        } //end of nested for

                    } //end of if 
                }


            }
</script>
		



    


</head>
<body>

<form id="Form1" method="post" runat="server" > <%--defaultbutton="btnSearch"--%>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<!-- #include file="include/headerMain.aspx" -->

<!-- Close Header-->


<div id="content3">
	
    <!-- Box Search Here -->
    
    <div class="wrapperContCase" style="margin-bottom:50px; margin-top: 70px; width:900px;">
    <div id="tabs">

	  <nav class="wrapperTabs">
        	<div class="tabsPosition">
        	<ul  class="tabsBox">
    		<li id="tabcss0" class="current1"><a href="#tabs-1" class="ul_link_tab">CASE SEARCH</a></li>
    		<li id="tabcss1"><a href="#tabs-2" class="ul_link_tab">CASE CITATION</a></li>     
			</ul>
            </div>
        </nav>
        <!--Content 1-->
    <div id="tabs-1">
    	<section class="caseBox">
        <div class="clear1" align="center">
     <span>
     
     
     
            <asp:regularexpressionvalidator id="Regularexpressionvalidator7" runat="server"
                                ValidationExpression="[A-Za-z0-9&amp;.,+'&quot;_\-\\\s\(\)\/]*" 
                                ErrorMessage="Invalid Phrase" 
                                ControlToValidate="TxtExactPhrase" Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="submit" Font-Size="Small"></asp:regularexpressionvalidator>
            <asp:regularexpressionvalidator id="RegularExpressionValidator3" runat="server"
                                            ValidationExpression="[A-Za-z0-9&amp;.,'&quot;_\s]*" 
                                            ErrorMessage="Invalid For 4th Input" 
                                            ControlToValidate="txtNotCases" Display="Dynamic" 
                      SetFocusOnError="True" ValidationGroup="submit" Font-Size="Small"></asp:regularexpressionvalidator>
            <asp:label id="lblMsg" runat="server" Font-Bold="False" ForeColor="Red" Font-Size="Small"></asp:label>
     </span>
     </div>

           <!-- zubair
            ToolTip="Elaw Case Law Search allows you to search through keywords to retrieve the most relevant documents from our digital library." 
            ToolTip="To search case law by exact phrase from Elaw library."
             -->
        <table border="0" cellpadding="0" cellspacing="0" 
            style="margin:20px 20px 0 20px; width: 92%;">
    	    <tr>
    	      <td><table width="95%" border="0" cellpadding="0">
    	        <tr>
    	          <td width="72%">
                  <asp:textbox id="txtExactPhrase" runat="server" 
        MaxLength="150"   
                       class="advBoxSfix" style="width:672px; padding:0 5px 5px 10px;"></asp:textbox></td>
    	          <td width="28%">

                  <asp:button id="btnSearch" 
                   class="btnSave" style="width:160px; margin-left:5px;"
                    runat="server" 
                    Text="Search" 
           
                ValidationGroup="submit" ></asp:button>
</td>
  	          </tr>
  	        </table></td>
             <td>
            
            
            <a href="#" class="tip">?<span>
            <p><b>Operators Tips:</b></p>
(<b>" "</b>)&nbsp;Find Exact Phrase eg:- ("case submission")<hr>
(<b>+</b>)&nbsp;Find All Words eg:- (case + submission) <hr />
(<b>-</b>)&nbsp;Find All Words without eg:- (case - submission) <hr />
(<b>\</b>)&nbsp;Find Any Words eg:-  (case \ submission) <hr />
(<b>w/p</b>)&nbsp;Find All Words Within Same Paragraph eg:- (case w/p submission)  or  (case  submission w/p )<hr />
(<b>w/s</b>)&nbsp;Find All Words Within Same Sentence eg:- (case w/s submission)  or  (case  submission w/s )<hr />
(<b>w/n</b>)&nbsp;Find All Words Within Given Distance(n) eg:- (case w/10 submission)  or  (case  submission w/10 )<hr />
            
            
            
            </span></a></td>


  	      </tr>
    	    <tr>
    	      <td><table width="100%" border="0" cellpadding="2" cellspacing="2">
    	        <tr>
    	          <td class="style1">
                  <span class="tTextNorm">Within:</span></td>
    	          
                  <td class="style2">
                  <asp:DropDownList ID="ddlsrch" runat="server" class="advBoxS6" ClientIDMode="Static">
                    <asp:ListItem Selected="True" Value="1">Judgment</asp:ListItem>
                    <asp:ListItem Value="2">Case title</asp:ListItem>
                    <asp:ListItem Value="3">3 Words</asp:ListItem>
                    <asp:ListItem Value="4">4 Words</asp:ListItem>
                    <asp:ListItem Value="5">5 Words</asp:ListItem>
                    <asp:ListItem Value="6">same sentence</asp:ListItem>
                    <asp:ListItem Value="7">same paragraph</asp:ListItem>
                    <asp:ListItem Value="8">same page</asp:ListItem>
                    </asp:DropDownList>
                    </td>
    	          
                  <td class="style3">
                  <asp:checkbox id="cbIndustrialCourt" runat="server" class="tick3" style="margin-left:15px;"
                  Text="Include Industrial Court Awards"  ToolTip="Check for including the industrial court cases for search"></asp:checkbox>
                  </td>

    	          <td width="0">
                  <asp:dropdownlist id="ddlprox" runat="server" 
                    class="advBoxSfix" style="visibility:hidden; height:25px; margin:11px 0 8px 0;">
                    <asp:ListItem Value=""></asp:ListItem>
<%--                   <asp:ListItem Value="10">1</asp:ListItem>
                    <asp:ListItem Value="20">2</asp:ListItem>
                    <asp:ListItem Value="30">3</asp:ListItem>--%>
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                    
                      </asp:dropdownlist>
                      
                      
                  <asp:DropDownList 
                          ID="ddlproxmity" runat="server" class="advBoxSfix" style="visibility:hidden; height:25px; margin:11px 0 8px 0;">
                            <asp:ListItem Value="3">Within page</asp:ListItem>
                        <asp:ListItem Value="2">Within paragraph</asp:ListItem>
                        <asp:ListItem Value="1"> Within sentence</asp:ListItem>
                        </asp:DropDownList>

                        <asp:DropDownList ID="rbs" runat="server" class="advBoxSfix" style="visibility:hidden; height:25px; width:180px; margin-left:10px;">                  
                        <asp:ListItem Selected="True" Value="1">Full Document</asp:ListItem>
                        <asp:ListItem Value="2">Case Title</asp:ListItem>
                        </asp:DropDownList>
                      
                      </td>
   	            </tr>
  	        </table></td>
  	      </tr>
          <tr><td> 
              </td></tr>
  	    </table>
        
        <div class="clear"></div>


        <div id="AdvanceSearchAcc" style="padding:0; margin:0;">
        <div class="arrow2" style="padding:12px 10px 0 20px;">▲</div>
		<h1 class="search">Advanced Search</h1>
		
        <div class="scrollBoxWrapper2">
 
        <!--Expand Advance Search -->
        
        <div class="wrapperAdvCaseLaw">
        
        <!--Left Advance Search -->
         <aside class="leftAdvCaseLaw2">
          <table width="100%" border="0" cellpadding="2" cellspacing="2">
            <tr>
              <td width="134" class="tTextNorm">Without the word(s):</td>
              <td width="252">
            <asp:textbox id="txtNotCases" runat="server"
                         ToolTip="To search case law by none of these words from Elaw library."
                         MaxLength="150"
                          CssClass="advBoxS6"></asp:textbox></td>
            </tr>
            <tr>
              <td class="tTextNorm">Legislation Referred:</td>
              <td>
                    <asp:textbox id="legTitle" runat="server"  ToolTip="Search for case in legislation" MaxLength="150" CssClass="advBoxS6"></asp:textbox>
                </td>
            </tr>
            <tr>
              <td class="tTextNorm">Judge:</td>
              <td>
              
                  <asp:textbox id="txtJudgeName" runat="server" 
                        ToolTip="To search case law by Judge Name" MaxLength="75"
                         EnableViewState="False" class="advBoxS6"></asp:textbox>
                  
                  
                </td>
            </tr>
            <tr>
              <td class="tTextNorm">Case Number:</td>
              <td>
                 
            <asp:textbox id="txtCaseNumber" runat="server" ToolTip="To search case law by any of these words from Elaw library."
                             MaxLength="150"
                              CssClass="advBoxS6"></asp:textbox></td>
            </tr>
            <tr>
              <td class="tTextNorm">Counsel:</td>
              <td>
                 
            <asp:textbox id="txtCounsel" runat="server" ToolTip="To search case law by any of these words from Elaw library."
                             MaxLength="150"
                              CssClass="advBoxS6"></asp:textbox></td>
              </tr>


          </table>
        </aside>
        
         <aside class="rightAdvCaseLaw2">
          <table width="100%" border="0" cellpadding="2" cellspacing="2">
            <tr>
              <td width="385" height="41" valign="top">
              
              <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                  <td width="40" class="tTextNorm">Court Type:</td>
                  <td width="60%">
              
                    <asp:dropdownlist id="ddlCourts" runat="server" CssClass="advBoxS6" Width="160px">
                        <asp:ListItem Value="1">All Courts</asp:ListItem>
                        <asp:ListItem Value="2">Federal/Supreme Court</asp:ListItem>
                        <asp:ListItem Value="3">Court of Appeal</asp:ListItem>
                        <asp:ListItem Value="4">High Court</asp:ListItem>
                        <asp:ListItem Value="5">Industrial Court</asp:ListItem>
                    </asp:dropdownlist>
                  </td>
                </tr>
              </table>
              
              </td>
            </tr>
            <tr>
              <td>
              
              <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                  <td width="150px" class="tTextNorm">Judgment year(s):</td>
                  <td width="200px">
                  <asp:dropdownlist id="ddlDate1" runat="server" class="select_year" Width="68px" CssClass="advBoxS6"></asp:dropdownlist> To <asp:dropdownlist id="ddlDate2" runat="server" class="select_year" Width="68px" CssClass="advBoxS6"></asp:dropdownlist></td>
                  <td width="10px">&nbsp;</td>
                  <td width="10px" align="left">   &nbsp;         
                  </td>
                </tr>
              </table>
              
              </td>
            </tr>

            <tr>
            <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
            
            <td>
             <asp:DropDownList ID="CJudcinially" runat="server"  class="advBoxS6" Width="333">
                      <asp:ListItem Value="0" >Cases Judicially Considered</asp:ListItem>
                      <asp:ListItem Value="1">Applied / Appealed / Followed</asp:ListItem>
                      <asp:ListItem Value="2">Distinguished / Not Followed / Overruled</asp:ListItem>
                     </asp:DropDownList>
            </td>
            </tr>
            
            
            
            </table>
            
            </td>
            </tr>


            <tr>
              <td>
              
              <table width="100%" border="0" cellpadding="0" cellspacing="0">
               </td></tr>
                <tr>
                
                  <td width="94%" class="tTextNorm">
                   
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                   <ContentTemplate>
                  
                  <table>
                  
                  <tr>
                  <td style="padding:8px 0 0 0;">
                     <div class="wrapperdropdown" >
                        <div id="dd" class="wrapper-dropdown" > Subject Index
                          <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="True" 
                               RepeatLayout="UnorderedList" CssClass="dropdown" ClientIDMode="AutoID" style="text-transform:capitalize;">
                            <asp:ListItem Value="1">level1</asp:ListItem>
                            <asp:ListItem Value="2">level2</asp:ListItem>
                            <asp:ListItem Value="3">level3</asp:ListItem>
                            <asp:ListItem Value="4">level4</asp:ListItem>
                        </asp:CheckBoxList></div></div>
                        </td>
                        </tr>
                

                   <td>
                   <div class="wrapperdropdown">

                        <div id="dd1" class="wrapper-dropdown"  style="visibility:hidden;">Subject Index
             
                        <asp:CheckBoxList ID="CheckBoxList2" onclick = "checkBoxList2OnClick(this);" 
                            runat="server" CssClass="dropdown" RepeatLayout="UnorderedList">
                        </asp:CheckBoxList></div>
                    </div>
                    </td>

                    </table>
                      
                    </ContentTemplate>
                    </asp:UpdatePanel>

            &nbsp;  <asp:TextBox ID="Label1" runat="server" style="display:none;"></asp:TextBox>
                    
                    <div id="elawselect" style="visibility:hidden;">
                    <select multiple="multiple" style="width:430px; visibility:hidden" id="sub">
                    
                    <%--<asp:ListItem Value="10">1</asp:ListItem>
                    <asp:ListItem Value="20">2</asp:ListItem>
                    <asp:ListItem Value="30">3</asp:ListItem>--%>

			        </select>
                    </div>
                      
                      
                      
                      </td>
                </tr>
              </table></td>
            </tr>
          </table>
        
        </aside>





        
        
        <!--Right Advance Search -->
          <div class="clear"></div>
        </div>
        
        </div>
        
        </div>

    	</section>
        </div
        <!--Content 2--> 
        <div id="tabs-2">
         	<section class="caseBox">


            <div class="byCitation">
        
        <div class="clear1" align="center">
   
        <span class="search_cri">
            
                <asp:regularexpressionvalidator id="RegularExpressionValidator5" 
            runat="server"  ValidationExpression="[A-Za-z.\s]*"
                ErrorMessage=" Invalid Judge Name " ControlToValidate="txtJudgeName" 
            Display="Dynamic" SetFocusOnError="True" Font-Size="Small"></asp:regularexpressionvalidator>
                <asp:regularexpressionvalidator id="Regularexpressionvalidator4" 
            runat="server"  ValidationExpression="[A-Za-z0-9()-/&amp;.\s]*"
                ErrorMessage=" Invalid Case No. " ControlToValidate="txtCaseNumber" 
            Display="Dynamic" SetFocusOnError="True" Font-Size="Small"></asp:regularexpressionvalidator>
                
        </span>
    
    
    
    </div>
        
           
        <table width="870" border="0" cellpadding="2" cellspacing="2" style="margin:30px;">
  		<tr>
        <td width="470">
        <div id="citationDiv">
     
        <asp:UpdatePanel runat="server" ID="updatePan2" UpdateMode="Conditional">
        <ContentTemplate>
        <table width="470">
        <tr>
        
       <td width="5%" height="39" class="tTextNorm">Publications:</td>
    	<td  width="5%">
        		<asp:dropdownlist  name="select3" id="CitPub" runat="server" class="advBoxS6"  
                    style=" height:25px; width:70px; margin:0 5px;" AutoPostBack="True">
                <asp:ListItem Selected="true"></asp:ListItem>
                <asp:ListItem value="MLRA">MLRA</asp:ListItem>
                <asp:ListItem value="MLRH">MLRH</asp:ListItem>
                <asp:ListItem value="MELR">MELR</asp:ListItem>
                   <asp:ListItem value="CLJ">CLJ</asp:ListItem>
                   <asp:ListItem value="MLJ">MLJ</asp:ListItem>
                   <asp:ListItem value="AMR">AMR</asp:ListItem>
                    <asp:ListItem value="ILR">ILR</asp:ListItem>
              	</asp:dropdownlist>
        </td>
    	  <td width="4%" class="tTextNorm">Year:</td>
    	<td width="5%">
        		<asp:dropdownlist name="select4" id="CitYear" runat="server" class="advBoxS6"
                    style=" height:25px; width:70px; margin:0 5px;">
                <%--<asp:ListItem value="2012">2012</asp:ListItem>
                <asp:ListItem value="2011">2011</asp:ListItem>
                <asp:ListItem value="1964">1964</asp:ListItem>--%>
              	</asp:dropdownlist>
        </td>
    	<td width="5%" class="tTextNorm">Volume:</td>
    	<td width="5%">
        		<asp:dropdownlist name="select5" id="CitVol" runat="server" class="advBoxS6" 
                    style=" height:25px; width:70px; margin:0 5px;">
               
              	</asp:dropdownlist>
        </td>
        </tr>
        </table>
    	</ContentTemplate>
        </asp:UpdatePanel>
        </div>
        </td>
    	<td width="100px" class="tTextNorm">Page number:</td>
        
    	<td width="9%">
        
        
        <input id="CitPn" class="advBoxS6" style="width:70px; margin:0 5px;" />
     
        </td>
    	
        <td>
        <div class="btnDefine" style="padding:6px; width:160px;" onclick="citation()">
        <a   style="width:160px;" id="citationSearch" >Search</a></div></td>
  		</tr>
		</table>
        
        
        </div>




    	  <div class="clear"></div>
   	  </section>

        </div>

        </div><!--close tab-->
  </div>
    
    <div class="clear"></div>
</div>
  


<footer> 
	<div class="wrapperFooter">

	<!-- #include file="include/footerMain.aspx" -->

    </div>
</footer>
  
<script src="js/script.js" type="text/javascript"></script>
<script type="text/javascript">
    function DropDown(el) {
        this.dd = el;
        this.opts = this.dd.find('ul.dropdown > li');
        this.val = [];
        this.index = [];
        this.initEvents();
    }
    DropDown.prototype = {
        initEvents: function () {
            var obj = this;

            obj.dd.on('click', function (event) {
                $(this).toggleClass('active');
                event.stopPropagation();
            });

            obj.opts.children('label').on('click', function (event) {
                var opt = $(this).parent(),
							chbox = opt.children('input'),
							val = chbox.val(),
							idx = opt.index();

                ($.inArray(val, obj.val) !== -1) ? obj.val.splice($.inArray(val, obj.val), 1) : obj.val.push(val);
                ($.inArray(idx, obj.index) !== -1) ? obj.index.splice($.inArray(idx, obj.index), 1) : obj.index.push(idx);
            });
        },
        getValue: function () {
            return this.val;
        },
        getIndex: function () {
            return this.index;
        }
    }

    $(function () {

        var dd = new DropDown($('#dd'));

        $(document).click(function () {
            // all dropdowns
            $('.wrapper-dropdown').removeClass('active');
        });

    });

    $("li").each(function () {
        // $(this).addClass('link');
        var z = ($(this).text()).toString();
        if (z.indexOf("   ") > -1) {
            $(this).css("background-color", "#98AFC7");
            $(this).css("text-align", "center");
        }
    });


    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {

        function DropDown(el) {
            this.dd = el;
            this.opts = this.dd.find('ul.dropdown > li');
            this.val = [];
            this.index = [];
            this.initEvents();
        }
        DropDown.prototype = {
            initEvents: function () {
                var obj = this;

                obj.dd.on('click', function (event) {
                    $(this).toggleClass('active');
                    event.stopPropagation();
                });

                obj.opts.children('label').on('click', function (event) {
                    var opt = $(this).parent(),
							chbox = opt.children('input'),
							val = chbox.val(),
							idx = opt.index();

                    ($.inArray(val, obj.val) !== -1) ? obj.val.splice($.inArray(val, obj.val), 1) : obj.val.push(val);
                    ($.inArray(idx, obj.index) !== -1) ? obj.index.splice($.inArray(idx, obj.index), 1) : obj.index.push(idx);
                });
            },
            getValue: function () {
                return this.val;
            },
            getIndex: function () {
                return this.index;
            }
        }

        $(function () {

            var dd = new DropDown($('#dd'));

            $(document).click(function () {
                // all dropdowns
                $('.wrapper-dropdown').removeClass('active');
            });

        });


        $("li").each(function () {
            // $(this).addClass('link');
            var z = ($(this).text()).toString();
            if (z.indexOf("   ") > -1) {
                $(this).css("background-color", "#98AFC7");
                $(this).css("text-align", "center");
            }
        });
        // re-bind your jQuery events here
    });
</script>
<script type="text/javascript">
    function DropDown(el) {
        this.dd1 = el;
        this.opts = this.dd1.find('ul.dropdown > li');
        this.val = [];
        this.index = [];
        this.initEvents();
    }
    DropDown.prototype = {
        initEvents: function () {
            var obj = this;

            obj.dd1.on('click', function (event) {
                $(this).toggleClass('active');
                event.stopPropagation();
            });

            obj.opts.children('label').on('click', function (event) {
                var opt = $(this).parent(),
							chbox = opt.children('input'),
							val = chbox.val(),
							idx = opt.index();

                ($.inArray(val, obj.val) !== -1) ? obj.val.splice($.inArray(val, obj.val), 1) : obj.val.push(val);
                ($.inArray(idx, obj.index) !== -1) ? obj.index.splice($.inArray(idx, obj.index), 1) : obj.index.push(idx);
            });
        },
        getValue: function () {
            return this.val;
        },
        getIndex: function () {
            return this.index;
        }
    }

    $(function () {

        var dd1 = new DropDown($('#dd1'));

        $(document).click(function () {
            // all dropdowns
            $('.wrapper-dropdown').removeClass('active');
        });

    });



    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {



        function DropDown(el) {
            this.dd1 = el;
            this.opts = this.dd1.find('ul.dropdown > li');
            this.val = [];
            this.index = [];
            this.initEvents();
        }
        DropDown.prototype = {
            initEvents: function () {
                var obj = this;

                obj.dd1.on('click', function (event) {
                    $(this).toggleClass('active');
                    event.stopPropagation();
                });

                obj.opts.children('label').on('click', function (event) {
                    var opt = $(this).parent(),
							chbox = opt.children('input'),
							val = chbox.val(),
							idx = opt.index();

                    ($.inArray(val, obj.val) !== -1) ? obj.val.splice($.inArray(val, obj.val), 1) : obj.val.push(val);
                    ($.inArray(idx, obj.index) !== -1) ? obj.index.splice($.inArray(idx, obj.index), 1) : obj.index.push(idx);
                });
            },
            getValue: function () {
                return this.val;
            },
            getIndex: function () {
                return this.index;
            }
        }

        $(function () {

            var dd1 = new DropDown($('#dd1'));

            $(document).click(function () {
                // all dropdowns
                $('.wrapper-dropdown').removeClass('active');
            });

        });


        $("li").each(function () {
            // $(this).addClass('link');
            var z = ($(this).text()).toString();
            if (z.indexOf("   ") > -1) {
                $(this).css("background-color", "#98AFC7");
                $(this).css("text-align", "center");
            }
        });
        // re-bind your jQuery events here
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
