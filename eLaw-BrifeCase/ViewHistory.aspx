<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewHistory.aspx.vb" Inherits="ViewHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>History | myBriefcase | eLaw.my</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="../GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="../GUI/favicon.ico" type="image/ico"/>
    <link href="../css/style_New.css" rel="stylesheet" type="text/css" />
    <script src="../js/libs/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/script.js" type="text/javascript"></script>
    <script>
        function GetValue() {
            var GetChecked = $("input[name=check]:checked").map(function () { return this.value; }).get().join(",");
            var action = "deletecase.aspx?";
            var form_data = {
                Selectedvalues:GetChecked,
                is_ajax: 1
                            };
            $.ajax({
                type: "POST",
                url: action,
                data: form_data,
                success: function (response) {
                 window.location = "user_cases.aspx";
                    
                }
            });

            return false;


        }
        $(function () {

            // add multiple select / deselect functionality
            $("#selectall").click(function () {
                $('.case').attr('checked', this.checked);
            });
           
            // if all checkbox are selected, check the selectall checkbox
            // and viceversa
            $(".case").click(function () {

                if ($(".case").length == $(".case:checked").length) {
                    $("#selectall").attr("checked", "checked");
                    
                } else {
                    $("#selectall").removeAttr("checked");
                }

            });
        });
</script>
  
</head>
<body>





<form id="form1" runat="server">

<!-- #include file="../include/headerMain2.aspx" -->

<div id="content3">
	
    <!-- Box Search Here -->
    
    <div class="wrapperContCase" style="padding:36px 0 100px 0; width:900px;">
    
    
    <!-- Nav Tabs -->
    <div class="clear"></div>
		<nav class="wrapperTabs">
        	<div class="tabsPosition">
        	<ul class="tabsBox">
    		<li ><a href="user_cases.aspx">SAVED CASES</a></li> 		 
    		<li><a href="savedSearch.aspx">SAVED SEARCHES</a></li> 
            <li class="current1"><a href="#">VIEW HISTORY</a></li>     
			</ul>
            </div>
        </nav>
    
    	<section class="caseBox2">
        
        <nav class="topBar3">
    
        
    	<div class="leftResultBox3">
    	<div class="funtionResults">
    	
    	<!--<a href="#"><img border="0" src="../img/icoPrint.gif"/>Print</a>-->
        <!--<asp:LinkButton ID="Email" runat="server"><img border="0" src="../img/icoEmail.gif"/>Email</asp:LinkButton>-->
        <!--<a href="#"><img border="0" src="../img/icoSave2.gif"/>Save</a>-->
        <!--<a href="#"><img border="0" src="../img/icoWords.gif"/>Words</a>-->
    	<!--<asp:LinkButton ID="LinkButton1" runat="server" Text="PDF"><img border="0" src="../img/icoPDF2.gif"/>PDF</asp:LinkButton>-->
        <!--<a href="#"><img border="0" src="../img/icoJudgment.gif"/>Judgment</a>-->
        <!--<a href="#"><img border="0" src="../img/icoFB.gif"/>Facebook</a>-->
        <!--<a href="#"><img border="0" src="../img/icoTw.gif"/>Twitter</a>-->
        <!--<a href="#"><img border="0" src="../img/icoLinked.gif"/>Linked-In</a>-->
    	</div>
    	</div>
    
    	<div class="clear"></div>
    	</nav>
        
    	  <aside class="boxBgblue2"><span class="tTextNorm2">View History </span></aside>

  <asp:Label ID="lblTbl" runat="server"></asp:Label>
      
        <div class="clear"></div>
    	</section>

  </div>
    
    <div class="clear"></div>
</div>

 <!-- footer -->

<footer> 
	<div class="wrapperFooter">
	<section class="wrapperAddfooter">
  
    <aside class="gotoPage" style="padding-left:190px">
    
     <div class="numPageCase">
   <asp:Label ID="lblBottomNavigation" runat="server"></asp:Label>
     </div>
    </aside>
    
    <div class="clear"></div>
    </section>
	
    <!-- #include file="../include/footerMain.aspx" -->

    </div>
</footer>
  


    
    </form>
</body>
</html>
