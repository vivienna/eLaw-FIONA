<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.LawDictionary1" CodeFile="LawDictionary.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="GUI/css/jquery.ui.tabs.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="css/style_New.css" />
    <script src="js/libs/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="include/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="include/jquery.ui.tabs.js" type="text/javascript"></script>
    <script src="js/jquery.searchabledropdown-1.0.7.min.js" type="text/javascript"></script>
    <script src="include/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <link href="css/AutoComplate.css" rel="stylesheet" type="text/css" />
    <title> Dictionary | eLaw.my</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
    <link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
   
<script type="text/javascript">
    $(document).ready(function () {
        $("#lbWords_glossary").searchable();
        //$("#tabcss").addClass("current1");
        $("#tabs").tabs();

        $('#tabs').bind('tabsselect', function (event, ui) {

            var selectedTab = ui.index;

            // var current = $("#<%= hidLastTab.ClientID %>").val(selectedTab);

            $(".tabsBox li").removeClass("current1");
            $(".tabsBox li:eq(" + selectedTab + ")").addClass("current1");
        });
        ///////////////////////////////////////////////////////Get word meaning 
        $('#quickDrop1').change(function () {
            var lang = $('#quickDrop1 :selected').val();
           
            $.ajax({
                type: 'POST',
                url: 'lawDictionary.aspx/GetTranslator',
                data: '{auto: "A" ,lang : "' + lang + '", word : "" }',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                
                    $("#lbTranslate").empty();
                    $("#lbTranslate").html(data.d);
                }
            });
        });

        /////////////////////////////////////////////////////
        $('#lbTranslate').change(function () {
            var lang = $('#quickDrop1 :selected').val();
            var word = $('#lbTranslate').val();
          $.ajax({
                type: 'POST',
                url: 'lawDictionary.aspx/GetTranslator',
                data: '{ auto: "A" , lang : "' + lang + '"  , word : "' + word + '" }',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    $('#lblTranslate').empty(); ;
                    $('#lblTranslate').html(data.d);
                }
            });
        });
        ///////////////////////////////////////////////////////
        $("#lbWords_glossary").change(function () {
            var load_tab = $('#lbWords_glossary option:selected').html();
            var dataString = load_tab;
            $.ajax({
                type: "POST",
                url: "load_gloassry.aspx?tab=" + load_tab,
                data: load_tab,
                success: function (msg) {
                    $('#load_tab').html("");
                    $('#load_tab').html(msg);
                }
            });
            return false;
        });

        $("#lbWords").change(function () {
            var load_tab = $('#lbWords option:selected').html();
            var dataString = load_tab;
          $.ajax({
                type: "POST",
                url: "load_words.aspx?tab=" + load_tab,
                data: load_tab,
                success: function (msg) {
                    $('#load_words').html("");
                    $('#load_words').html(msg);
                    $('#lblMeaning').hide();

                }
            });
            return false;
        });

        $("#lbWords_word_ph").change(function () {
            var load_tab = $('#lbWords_word_ph option:selected').html();
            var dataString = load_tab;
            
            $.ajax({
                type: "POST",
                url: "load_phrase.aspx?tab=" + load_tab,
                data: load_tab,
                success: function (msg) {
                    $('#load_phrase').html("");
                    $('#load_phrase').html(msg);

                }
            });
            return false;
        });
        //////////////////////////////////////////////Trems
        $('#txtSearch_glossary').on('keyup', function () {
            var auto = $("#<%=txtSearch_glossary.ClientID%>")[0].value;
            if (auto.length >= 2) {
                $.ajax({
                    type: "POST",
                    url: "lawDictionary.aspx/GetautocomplateTrems",
                    data: '{auto: "' + $("#<%=txtSearch_glossary.ClientID%>")[0].value + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        console.log(data);
                        $("#txtSearch_glossary").autocomplete({
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
        $("#getTrem").live("click", function () {
            var load_tab = $('#txtSearch_glossary').val();
            var dataString = load_tab;
            $.ajax({
                type: "POST",
                url: "load_gloassry.aspx?tab=" + load_tab,
                data: load_tab,
                success: function (msg) {
                    $('#load_tab').html("");
                    $('#load_tab').html(msg);
                }
            });
            return false;
        });

    });
    ///////////////////////
    function LoadContentglo(id) {
       
        $.ajax({
            type: "POST",
            url: "lawDictionary.aspx/Getglo",
            data: '{auto: "' + id + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $("#lbWords_word_ph").empty();
                $("#lbWords_word_ph").html(data.d);
            },
            failure: function (response) {

            }
        });

    }
    //////////////////////////
    ///////////////////////
    function LoadContent(id) {

        $.ajax({
            type: "POST",
            url: "lawDictionary.aspx/Getlegd",
            data: '{auto: "' + id + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $("#lbWords").empty();
                $("#lbWords").html(data.d);
            },
            failure: function (response) {

            }
        });

    }
    //////////////////////////
    ///////////////////////
    function LoadContentwp(id) {

        $.ajax({
            type: "POST",
            url: "lawDictionary.aspx/Getwp",
            data: '{auto: "' + id + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $("#lbWords_glossary").empty();
                $("#lbWords_glossary").html(data.d);
            },
            failure: function (response) {

            }
        });

    }
    //////////////////////////
    ///////////////////////
    function LoadTranslator(id) {
        var lang = $('#quickDrop1 :selected').val();
        $.ajax({
            type: "POST",
            url: "lawDictionary.aspx/GetTranslator",
            data: '{auto: "' + id + '" ,lang : "' + lang  + '", word : "" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $("#lbTranslate").empty();
                $("#lbTranslate").html(data.d);
            },
            failure: function (response) {

            }
        });

    }
    //////////////////////////
   
</script>





</head>
<body>
<form id="Form1" method="post" runat="server">
    <asp:HiddenField ID="hidLastTab" runat="server" Value="0" />
    
    
    <!-- #include file="include/headerMain3.aspx" -->


    <div id="content3">

		<div class="wrapperContCase" style="padding:36px 0 100px 0; width:900px;">
		 
    
    
    <div id="tabs">

	<div class="tabsPosition">
    <ul  class="tabsBox">
		<li id="tabcss0"><a href="#tabs-1" class="ul_link_tab">LEGAL DICTIONARY</a></li>
		<li id="tabcss1"><a href="#tabs-2" class="ul_link_tab" >STATUTORY INTERPRETATIONS</a></li>
		<li id="tabcss2"><a href="#tabs-3" class="ul_link_tab" style="display:none;">GLOSSARY</a></li>
        <li id="tabcss3"><a href="#tabs-4" class="ul_link_tab">TRANSLATOR</a></li>
		
	</ul>
	</div>


	<div id="tabs-1">


    	<section class="caseBox2">
        
        <section class="listboxAct3" > 
           <asp:label id="lblSortBy" runat="server"></asp:label>
           <%--<span id="dicAlpha"></span>--%>
	
	&nbsp;</section><div class="clear"></div>
    <nav class="topBar3"><!-- Funtion List --><div class="leftResultBox3">
    	<div class="funtionResults">

    	
	<asp:textbox id="txtSearch" runat="server" CssClass="advBoxS6" 
                style=" margin:0 10px; width:250px;" ValidationGroup="dictionary"></asp:textbox>
	<asp:button id="Button1" runat="server" Text="Search" 
            CssClass="btnSave" ValidationGroup="dictionary"></asp:button>
	<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="Server" 
                ControltoValidate="txtSearch" Display="Dynamic" SetFocusOnError="true" 
	ErrorMessage="Please type the valid words" CssClass="tfont2" ValidationGroup="dictionary"></asp:requiredfieldvalidator>
	<asp:label id="lblMsg" runat="server" ForeColor="Red"></asp:label>
	
    	</div>
    	</div>
        
        <%--<div class="rightResultBox2">
        <div class="funtionResults">

    	<a href="#"><img border="0" src="img/icoPrint.gif"/>Print</a>
        <a href="#"><img border="0" src="img/icoEmail.gif"/>Email</a>

            </div>
            </div>
    --%>
    	<div class="clear"></div>
    	</nav>
        <table width="860" border="0" cellpadding="0" cellspacing="0" style="margin:20px;">
              <tr>
                <td width="30%" height="320" valign="top">
                <div class="scroll2"><asp:listbox id="lbWords" runat="server"  style="width:100%; height:100%; "></asp:listbox>
                
                </div></td>
                <td width="70%"><div class="scroll3">
                
    <div id="load_words"></div>
    <asp:label id="lblMeaning" runat="server"></asp:label>
                
                
                </div></td>
              </tr>
            </table>
          <div class="clear"></div>
    	
	
	</section>

	</div>




	<div id="tabs-2">
	 
	<div class="caseBox2">
	<section class="listboxAct3"> 
    <asp:label id="lblSortBy_glossary" runat="server"></asp:label>
    <%--<span id ="wordAlpha"></span>--%>
  
    </section>
    <div class="clear"></div>
    <nav class="topBar3"><!-- Funtion List --><div class="leftResultBox3">
    	<div class="funtionResults">


      

    	
	<asp:textbox id="txtSearch_glossary" runat="server" CssClass="advBoxS6" 
                style=" margin:0 10px; width:250px;" ValidationGroup="dictionary1"></asp:textbox>
                <input type="button" value="Search" id="getTrem" class="btnSave" />
	

	
	&nbsp;<asp:label id="lblMsg_glossary" runat="server" ForeColor="Red"></asp:label>

	
    	</div>
    	</div>
        
        <%--<div class="rightResultBox2">
        <div class="funtionResults">

    	<a href="#"><img border="0" src="img/icoPrint.gif"/>Print</a>
        <a href="#"><img border="0" src="img/icoEmail.gif"/>Email</a>

            </div>
            </div>--%>
    
    	<div class="clear">
           
        </div>
    	</nav>

         <table width="860" border="0" cellpadding="0" cellspacing="0" style="margin:20px;">
              <tr>
                <td width="30%"  valign="top">
                <div class="scroll2">
                 <asp:listbox ID="lbWords_glossary" runat="server" style="width:100%; height:100%; ">
            </asp:listbox></div>
                </td>
                <td width="70%" valign="top">
                <div class="scroll3">
                 <div id="load_tab" style="width:100%; height:100%;"></div></div></td>

                </tr>
              
            </table>

	
	<div class="clear" style="text-align: center">
	&nbsp;<br />
	</div>

	
    
</div>
	 
	 
	</div>





	<div id="tabs-3" style="display:none;">
	 
	<div class="caseBox2">
	
            <section class="listboxAct3"> 
            <asp:label id="lblSortByword_ph" runat="server" Font-Bold="True"></asp:label>
            <%--<span id="glossAlpha"></span>--%>
            </section><div class="clear"></div>
              <nav class="topBar3"><!-- Funtion List --><div class="leftResultBox3">
    	<div class="funtionResults">


      

    	
	<asp:textbox id="txtSearch_word_ph" runat="server" CssClass="advBoxS6" 
                style=" margin:0 10px; width:250px;" ValidationGroup="dictionary2"></asp:textbox>
	<asp:button id="btn_word_ph" runat="server" Text="Search" 
            CssClass="btnSave" ValidationGroup="dictionary2"></asp:button>

	<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="Server" 
        ControltoValidate="txtSearch_word_ph" 
        ErrorMessage="Please type the valid words" ValidationGroup="dictionary2"></asp:requiredfieldvalidator>
	&nbsp;&nbsp;
	&nbsp;<asp:label id="lblMsgword_ph" runat="server" 
        ForeColor="Red"></asp:label>

    	</div>
    	</div>
        
       <%-- <div class="rightResultBox2">
        <div class="funtionResults">

    	<a href="#"><img border="0" src="img/icoPrint.gif"/>Print</a>
        <a href="#"><img border="0" src="img/icoEmail.gif"/>Email</a>

            </div>
            </div>--%>
    
    	<div class="clear"></div>
    	</nav>

             <table width="860" border="0" cellpadding="0" cellspacing="0" 
                style="margin:5px 20px 5px 20px;">
              <tr>
                <td width="30%" height="320" valign="top">
                <div class="scroll2">
                <asp:listbox id="lbWords_word_ph" runat="server" style="width:100%; height:100%; "></asp:listbox>
                </div></td>
                <td width="70%">
                <div class="scroll2">
                <div id="load_phrase" style="margin-top:10px;"><% Response.Write(wordPhrase)%> </div></div>
                </td>
                </tr>
                
            </table>
          





	<div class="clear" style="text-align: center">
	&nbsp;<br />
	</div>
	
</div>
	 
	 
	</div>
    <div id="tabs-4">
    
         <!-------------------------------->
         	<div class="caseBox2">
<section class="listboxAct3"> 
        <asp:Label ID="SortBytranslate" runat="server" Text="" Width="100%"></asp:Label>
</section>
        
        <nav class="topBar3"><!-- Funtion List -->
        
        <div class="leftResultBox3">
    <div class="funtionResults">
    <span class="droplistbox">
            <select name="style1" id="quickDrop1" class="advBoxS6" style="width:200px; margin-left:12px;" >
               <option value="1">English to Malay</option>
                <option value="2">Malay to English</option>
              
            </select>
        </span>
    </div>
    </div>
        
        <%--<div class="rightResultBox2">
        <div class="funtionResults">
    <a href="#"><img border="0" src="img/icoPrint.gif"/>Print</a>
        <a href="#"><img border="0" src="img/icoEmail.gif"/>Email</a>
</div>
        </div>--%>
    
    <div class="clear"></div>
    </nav>
        <table width="860" border="0" cellpadding="0" cellspacing="0" style="margin:20px;">
          <tr>
            <td width="30%" height="320" valign="top">
            <div class="scroll2"><asp:listbox id="lbTranslate" runat="server"  style="width:100%; height:100%;"></asp:listbox></div></td>
            <td width="70%"><div class="scroll3">
               <div id="lblTranslate"></div></div></td>
          </tr>
        </table>
        <div class="clear" style="text-align: center">&nbsp;<br />
</div>
 	</div>
    <!-------------------------->
    </div>

    <!--close main content 2-->
    </div></div>


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
