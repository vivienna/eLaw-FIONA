<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.formsSearch" CodeFile="formSearch.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
 <link rel="stylesheet" href="css/style_New.css" />
   <script src="js/libs/jquery-1.7.1.min.js" type="text/javascript"></script>
<title> Forms | eLaw.my</title>
<link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
<link href="css/AutoComplate.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="include/jquery-ui-1.10.2.custom.min.js"></script>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $(".search").click(function () {
            if ($(".scrollBoxWrapper2").is(":hidden")) {
                $('.arrow2').html('▼');
            }
            else {
                $('.arrow2').html('▲');

            }
            $(".scrollBoxWrapper2").slideToggle("slow");

        });
        ////////////////////////////////////////////////////////////////64
        $txtFTSConts = $('#txtFTS').val();
        $('#txtFTS').on('keyup', function () {
            if ($(this).val() != $txtFTSConts) {
                $txtFTSConts = $(this).val();
                $(this).change();
            }

        });


        $('#txtTitle').on('keyup', function () {
            if ($(this).val() != $txtTitleConts) {
                $txtTitleConts = $(this).val();
                $(this).change();
            }

        });

        


        $('#txtFTS').on('change', function () {
            var auto = $("#<%=txtFTS.ClientID%>")[0].value;
            if (auto.length > 1)
                if (auto.length >= 2) {
                    $.ajax({
                        type: "POST",
                        url: "FormSearch.aspx/exactAutoComplete",
                        data: '{auto: "' + auto + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            console.log(data);
                            $("#txtFTS").autocomplete({

                                source: data.d
                            });
                        },
                        failure: function (response) {
                        }
                    });
                }
        });


      
        ////////////////////////////////////////////////////////////--
        });



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
       function chkformsubmit() {

           //abdo work alertstart
           resetddl(); //reset drop down list srch options


           var ddlvalue = "";
           var ddltext = "";
           var lstddlsrch = document.getElementById("<%=ddlsrch.ClientID%>");
           ddlvalue = lstddlsrch.options[lstddlsrch.selectedIndex].value;
           ddltext = lstddlsrch.options[lstddlsrch.selectedIndex].text;

           ddltext = ddltext.toLowerCase();


           if (ddltext.indexOf("full") > -1) {
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


           
                   } //end of chkformsubmit
                   //abdo work end


		</script>

</head>
<body">
<form id="Form1" method="post" runat="server">


<!-- #include file="include/headerMain3.aspx" -->

<div id="content3">
	
    <!-- Box Search Here -->
    
    <div class="wrapperContCase" style="margin-bottom:70px; margin-top: 70px; width:900px;">
    &nbsp;<!-- Nav Tabs --><div class="clear"></div>
		<nav class="wrapperTabs">
        	<div class="tabsPosition">
        	<ul class="tabsBox">
            <li class="current1"><a href="formSearch.aspx">FORMS</a></li>
           
    		<li ><a href="practiceNotesSearch.aspx">PRACTICE NOTES</a></li>
               
			</ul>
            </div>
        </nav>
    
    	

		





        <div class="caseBox2">

         <table width="96%" border="0" cellpadding="0" cellspacing="0" style="margin:20px;">
    	    <tr>
    	      <td><table width="100%" border="0" cellpadding="0">
    	        <tr>
    	          <td width="72%">
                  
                  <asp:textbox id="txtFTS" runat="server" MaxLength="150" class="advBoxSfix" style="width:665px; padding:0 5px 5px 10px;"></asp:textbox>
                  </td>
    	          <td width="28%"><asp:button id="btnSearch" runat="server" 
         
 Text="Search" class="btnSave" style=" width:160px; margin-left:10px;" OnClientClick="chkformsubmit()"></asp:button>
 </td>
  	          </tr>
  	        </table></td>
  	      </tr>
          
    	    <tr>
    	      <td><table width="100%" border="0" cellpadding="2" cellspacing="2">
    	        <tr>
    	          <td width="62"><span class="tTextNorm">Within:&nbsp;&nbsp;&nbsp; </span></td>
    	          <td width="126">


                  <asp:DropDownList ID="ddlsrch" runat="server" class="advBoxS6" ClientIDMode="Static">
                    <asp:ListItem Selected="True" Value="1">Full Form</asp:ListItem>
                    <asp:ListItem Value="2">Form title</asp:ListItem>
                    <asp:ListItem Value="3">3 Words</asp:ListItem>
                    <asp:ListItem Value="4">4 Words</asp:ListItem>
                    <asp:ListItem Value="5">5 Words</asp:ListItem>
                    <asp:ListItem Value="1">same sentence</asp:ListItem>
                    <asp:ListItem Value="2">same paragraph</asp:ListItem>
                    <asp:ListItem Value="3">same page</asp:ListItem>
                    </asp:DropDownList>


                  
                  
                 </td>
    	          
    	          <td width="123">
                  <asp:dropdownlist id="ddlprox" runat="server" 
       class="advBoxSfix" style="visibility:hidden; height:25px; margin:11px 0 8px 0; width:160px;" EnableViewState="False">
                    <asp:ListItem Value=""></asp:ListItem>
                    
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                    
                      </asp:dropdownlist>
                  
                  </td>
    	          <td width="211" class="tTextNorm"><asp:DropDownList 
                          ID="ddlproxmity" runat="server" class="advBoxSfix" style="visibility:hidden; height:25px; margin:11px 0 8px 0;">
                      <asp:ListItem Value="3">within Same Page</asp:ListItem>
                      <asp:ListItem Value="2"> within Same Paragraph</asp:ListItem>
                      <asp:ListItem Value="1"> within Same Sentence</asp:ListItem>
                      </asp:DropDownList></td>
    	          <td width="167" class="tTextNorm">
                      <asp:DropDownList ID="rbs" runat="server" class="advBoxSfix" style="visibility:hidden; height:25px; width:180px; margin-left:10px;">                  
                      <asp:ListItem Selected="True" Value="1">Full Document</asp:ListItem>
                      <asp:ListItem Value="2">Form Title</asp:ListItem>
                  </asp:DropDownList></td>
    	          
   	            </tr>
  	        </table></td>
  	      </tr>
  	    </table>

        <div style="text-align: center; text-transform: capitalize; font-weight: bold; font-size:14px;">
    <asp:label id="lblMsg" runat="server" ForeColor="Red"></asp:label>
    
    <asp:regularexpressionvalidator id="Regularexpressionvalidator7" runat="server"  ErrorMessage="Invalid Details"
ControlToValidate="txtFTS" ValidationExpression="[A-Za-z0-9&amp;.,'&quot;_\s]*" 
                SetFocusOnError="true" Display="Dynamic" CssClass="advBoxS6"></asp:regularexpressionvalidator>
    
    


    </div>
   
<div class="clear"></div>


                            <div class="wrapperAdvCaseLaw" style="padding:0; margin:0;">
        <div class="arrow2" style="padding:12px 10px 0 20px;">▲</div>
		<h1 class="search">&nbsp;</h1>
		 <div class="scrollBoxWrapper21">
        <!--Left Advance Search -->
        <div class="wrapperAdvCaseLaw">
        
        <aside class="leftAdvCaseLaw1">

        <table width="99%" border="0" cellpadding="2" cellspacing="2">
            
            <tr>
              <td class="tTextNorm">Without the words:</td>
              <td>
            <asp:textbox id="txtNotform" runat="server"
                         ToolTip="To search case law by none of these words from Elaw library."
                         MaxLength="150"
                          CssClass="advBoxS6"></asp:textbox></td>
            <td class="tTextNorm"></td>
            <td></td>
            </tr>
            </table>
        </aside>
        
        <!--Right Advance Search -->
        
        
        </div>
        </div>
        <div class="clear"></div>
        </div>



    <div class="clear1"></div>
    </div> 







    	</section>

  </div>
    
    <div class="clear"></div>
</div>

<footer> 
	<div class="wrapperFooter">
	
	<!-- #include file="include/footerMain.aspx" -->

    </div>
</footer>
<script src="js/script.js" type="text/javascript"></script>

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
