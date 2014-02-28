<%@ Page Language="vb" AutoEventWireup="false" Inherits="membersarea.LegislationMainDisplayed1" CodeFile="LegislationMainDisplayed.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<link id="Link1" runat="server" rel="shortcut icon" href="GUI/favicon.ico" type="image/x-icon"/>
<link id="Link2" runat="server" rel="icon" href="GUI/favicon.ico" type="image/ico"/>
   
        <link rel="stylesheet" href="css/style_New.css" />
    <script src="js/libs/jquery-1.7.1.min.js" type="text/javascript"></script>
     <script type="text/javascript" src="include/jquery-ui-1.10.2.custom.min.js"></script>
    <link rel="stylesheet" href ="include/jquery-ui-1.10.2.custom.min.css" />
    <style type="text/css">
   .ui-autocomplete
        {
       z-index: 9003;
       }
    .SDclear
    {color:#000;font-family:Arial, Helvetica, sans-serif;font-size:14px;font-weight:500;margin:0 0 0 8px}
    
    .gotoPagelegis{margin-left:100px;color:#fff;font-size:12px;float:left;font-weight:400;width:100%;text-align:center;padding:0}
    .gotoPagelegis ul{list-style:none}
    .gotoPagelegis span a{text-decoration:none;color:#fff}
    .gotoPagelegis span a{display:inline;float:left;padding:5px 5px 0}
    

  
   .info 
   {

 
    border:solid 1px #DEDEDE;
    background:#EFEFEF;
    color:#222222;
    padding:0 0px 0 0;
    margin-right:33px;
    text-align:center;
    float:right;
    }
    
    #blanket {
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
     
    #popUpDiv {
   background-color:#757575;
        font-size:14px;
    padding:0px 0px 5px  0px;
        -webkit-box-shadow:0 1px 1px #000;
  -moz-box-shadow:0 1px 1px #000;
  box-shadow:0 1px 1px #000;
  border-radius:5px;
 position:absolute;
width:320px;
z-index: 9002;
	}
   


   .found{
            background-color: Yellow;
            font: inherit;
            }
        .currentFound {
            background-color: orange;
            }
        
        

   
        #amend
        {
            display:none;
            text-align:center;
            cursor:pointer;
            }
        #amend font
        {
            
            }
            
        .hTitle
        {
            background-color: #ffffdd;
            font-weight:bold;
            }
            
         .hWord
         {
            background-color: yellow;
            font-weight:bold;
            color:Red;
             }
        .titleBox
        {
            width:100%;
            padding-top:10px;
            padding-bottom:10px;
            }
            
    </style>
    
    
		<title>Legislation View | Legislation | eLaw.my</title>
        <script src="include/css-pop.js" type="text/javascript"></script>
        <link rel="stylesheet" type="text/css" href="css/jquery-ui.slider.min.css"/>
        <link rel="stylesheet" href="css/TimeLine64.min.css" />
        
        <script type="text/javascript" src="js/jquery.easing.min.js"></script>
	          <script type="text/javascript"  src="js/TimeLine64.min.js"></script>
		<script type="text/javascript">
		
    /////////Print Single Section 
    
    function printDiv(divID) {
                var divElements = document.getElementById(divID).innerHTML;
                var divTitle = document.getElementById("lblPageTop").innerHTML;
                var windowUrl = 'about:blank';
	            var uniqueName = new Date();
	            var windowName = 'Print' + uniqueName.getTime();
                var WinPrint = window.open(windowUrl, windowName, 'left=30,top=30,right=500,bottom=500,width=700,height=500');
	            WinPrint.document.write('<' + 'html' + '><head><link href="../css/style_New.css" rel="stylesheet" type="text/css" /><style> #printdiv { display:none;}</style></head><' + 'body style="background:none !important"' + '>');
	            WinPrint.document.write(divTitle + '<BR>');
                 WinPrint.document.write(divElements);
                  WinPrint.document.write('<BR><BR><BR><BR><BR><div>Copyright © 2013 The Digital Library Sdn. Bhd. All rights reserved. Terms of Use | Privacy Statement</div>');
	            WinPrint.document.write('<' + '/body' + '><' + '/html' + '>');
	            WinPrint.document.close();
	            WinPrint.focus();
	            WinPrint.print();
	            WinPrint.close();
               
          
        }
    
    
    
    
    //////////////////////////   

    function remvTags(str) { //64
        //return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
        return String(str).replace('</p>','\n').replace(/<(.|\n)*?>/gi,'');
    }


    //var checkboxes = document.getElementsByName("sendItems");
    //var mail="";  
    //var htmlmail="";  
    var sectsIds="";
    var sectsFile="";
    
    function printDoc() { // can be optimized in the future by loading ajax once - but the structure of the response must be changed. 64
        var printConts = "";
        var printReqLink;
        var sectsFile1;
        sectsFile1 = $('[name="sendItems"]:checked:first').val();
        if(sectsFile1 !=""  &&  sectsFile1 !==undefined){
            $('[name="sendItems"]:checked').each(function(){
                var idd= $(this).attr('id');
                sectsIds+=idd+"|";
                printReqLink="LegislationSectionDisplayed.aspx?info=" + $(this).val();
                $.ajax({
                    type: "POST",
                    url: printReqLink,
                    data: {load_tab:"1"},
                    success: function(msg) {
                        //console.log('ok so far');
                        var divload = $('#lblXml', $(msg));
                        printConts+=divload.html().replace(/<sno>/i,'').replace(/<\/sno>/i,'').replace(/<st>/i,'').replace(/<\/st>/i,'')+"\n\n";
                        //printConts+=remvTags(divload.html())+"\n\n";
                    },

                    complete: function(msg){
                        var last = $('[name="sendItems"]:checked:last').attr('id');
                        if (printConts.length == 0) {
                            alert("Please Select Sections");
                            return false;
                        }else{
                            if(idd==last){
                                if (printConts == 0) {
                                    alert("Please Select Sections");
                                }else{
                                    var windowUrl = 'about:blank';
                                    var uniqueName = new Date();
                                    var windowName = 'Print' + uniqueName.getTime();

                                    //  you should add all css refrence for your html. something like.

                                    var WinPrint = window.open(windowUrl, windowName, 'left=30,top=30,right=500,bottom=500,width=700,height=500');
                                    WinPrint.document.write('<' + 'html' + '><head><link href="../css/style_New.css" rel="stylesheet" type="text/css" /><style> #printdiv { display:none;}</style></head><' + 'body style="background:none !important"' + '>');
                                    WinPrint.document.write("<h2 align='center'>"+$('#lblPageTop').html()+"</h2>\n\n");
                                    WinPrint.document.write(printConts);
                                    WinPrint.document.write('<' + '/body' + '><' + '/html' + '>');
                                    WinPrint.document.close();
                                    WinPrint.focus();
                                    WinPrint.print();
                                    WinPrint.close();
                                    }
                                //console.log(printConts);
                                //console.log(htmlmail);
                                }
                                
                        }
                    }
                    
                });
            });
        }
        else  alert("Please select sections for printing");
    }

    function sendEmail(last) { //64
        sectsIds="";
        sectsFile="";
        sectsFile = $('[name="sendItems"]:checked:first').val();
       
        if(sectsFile !="" &&  sectsFile !==undefined){
            $('[name="sendItems"]:checked').each(function(){
                    var idd= $(this).attr('id');
                    sectsIds+=idd+"|";
                });
                sectsIds=sectsIds.substr(0,sectsIds.length-1);
                //console.log(sectsIds);
              
                popup('popUpDiv');
            }else
              
                 popup('popupDiv3');
                return false;
        
    }
/////////////////////////////////////////////
    $(document).ready(function(){ 

   
        //////////////////////////////////////64
        $('.hWord').closest('.SDclear').each(function(){
            $(this).siblings('.titleBox').addClass('hTitle');
            $('#a'+$(this).attr('id').substring(2)).addClass('hTitle');
        });
        $('.hTitle:first').click();
        if ($('#TLF').html().trim() == "")
        $.ajax({
                type:'POST',
                url: 'LegislationMainDisplayed.aspx/getAmendments',
                data: '{ActNo: "'+$('#Id').html().trim()+'"}',
                contentType: "application/json; charset=utf-8",
                datatype:'JSON',
                success:function(data){
                    var lnk = 'LegislationMainDisplayed.aspx?id=';
                    var ids = [];

                    for(var i = 0 ; i < data.d.length;i++){
                        ids.push(data.d[i][3]);
                    }
                    function clbk(idx){
                    
                  var hdnmainid= document.getElementById("<%=mainacthidden.ClientID %>")

                        window.location = lnk+ids[idx]+'&TLF='+$('#Id').html().trim()+'&TLS='+idx+'&mainid='+hdnmainid.value;
                    }
                    initTimeLine(data.d,clbk);
                    if(data.d.length > 0)
                        $('#amend').show();
                    else{
                        $('.timeLine').hide();
                         $('#TlTrigger').hide();

                    }
                        
                },failure: function(){
                    $('.timeLine').hide();
                     $('#TlTrigger').hide();
                }
            });
            else
            $.ajax({
                type:'POST',
                url: 'LegislationMainDisplayed.aspx/getAmendments',
                data: '{ActNo: "'+$('#TLF').html().trim()+'"}',
                contentType: "application/json; charset=utf-8",
                datatype:'JSON',
                success:function(data){
                    var lnk = 'LegislationMainDisplayed.aspx?id=';
                    var ids = [];

                    for(var i = 0 ; i < data.d.length;i++){
                        ids.push(data.d[i][3]);
                    }
                    function clbk(idx){
                       var hdnmainid= document.getElementById("<%=mainacthidden.ClientID %>")
                        window.location = lnk+ids[idx]+'&TLF='+$('#TLF').html().trim()+'&TLS='+idx+'&mainid='+hdnmainid.value;
                    }
                    initTimeLine(data.d,clbk,parseInt($('#TLF').attr('sel')));
                    if(data.d.length <= 0){
                        $('#TlTrigger').hide();
                        $('.timeLine').hide();
                        }
                        
                },failure: function(){
                    $('.timeLine').hide();
                     $('#TlTrigger').hide();
                }
            });
        $('#TlTrigger').click(function(){
             if($('.timeLine').css('display')=='none')
                $('.timeLine').slideDown();
             else
                $('.timeLine').slideUp();
        });

        /////////////////////////////////////64
        var oldrep = "";
        var curr = 0;
        var fndsz = 0;
        function Matchcallback(match, p1) {
            return ((p1 == undefined) || (p1 == '')) ? match : '<span class="found">' + match + '</span>';
        }
        function surroundTags(search) {
            $('.boxDisplay').children(':not(.timeLine)').each(function(){
                $(this).html($(this).html().replace(new RegExp("<[^>]+>|&nbsp;|&amp;|(" + search + ")", 'ig'), Matchcallback));
            });
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
                            if ($('.found:eq(' + curr + ')').closest('.SDclear').css('display') == 'none')
                                $('.found:eq(' + curr + ')').closest('.SDclear').siblings('.titleBox').click();
                            $('.found:eq(' + curr + ')').closest('.SDclear').promise().done(function(){
                                $('body,html').stop();
                                $('body,html').animate({ scrollTop: $('.found:eq(' + curr + ')').offset().top - 90 }, 300);
                                $('.found:eq(' + curr + ')').addClass('currentFound');
                                $('#FCount').html((curr + 1) + ' of ' + (fndsz));
                            });

                        } else {
                            curr = 0;
                            if ($('.found:eq(' + curr + ')').closest('.SDclear').css('display') == 'none')
                                $('.found:eq(' + curr + ')').closest('.SDclear').siblings('.titleBox').click();
                            $('.found:eq(' + curr + ')').closest('.SDclear').promise().done(function(){
                                $('body,html').stop();
                                $('body,html').animate({ scrollTop: $('.found:eq(0)').offset().top - 90 }, 300);
                                $('.found:eq(0)').addClass('currentFound');
                                $('#FCount').html('1 of ' + (fndsz));
                                
                            });
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

                        if ($('.found:eq(' + curr + ')').closest('.SDclear').css('display') == 'none')
                            $('.found:eq(' + curr + ')').closest('.SDclear').siblings('.titleBox').click();
                        $('.found:eq(' + curr + ')').closest('.SDclear').promise().done(function(){
                            $('body,html').stop();
                            $('body,html').animate({ scrollTop: $('.found:eq(' + curr + ')').offset().top - 90 }, 300);
                            $('.found:eq(' + curr + ')').addClass('currentFound');
                            $('#FCount').html((curr + 1) + ' of ' + (fndsz));
                        });
                        
                    }

                }
        });


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

    //////////////////////////--



    $("#jumpto").click(function() {
    var jumpto = $("#txtjumpto").val();
  
    scrollTo("SDfind"+jumpto);
   // show_sec_content('MY_AMEN_2007_1300',jumpto);
      e.preventDefault();
     return false;
  });
    /////////////////
    $('#gotosch').click(function(e){
     scrollTo("SHimg1");
     return false;
    });
    function scrollTo(id)
    { 
  // Scroll
  $('html,body').animate({scrollTop: $("#"+id).offset().top-20},'slow');
     }
        ////////////////////////
        $('#emailing').click(function(){
         $.ajax({
                    type: "POST",
                    url: "EmailGroup.aspx/getEmails",
                    //data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        console.log(data.d);
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
                    },
                    failure: function (response) {
                    }
});


                            document.getElementById("Close").style.display = "none";
                           document.getElementById("mainTable").style.display="block";
        //console.log($('[name="sendItems"]:checked:last').attr('id'));
        //alert($('input:checked').last().id);
        sendEmail($('[name="sendItems"]:checked:last').attr('id'));
        });

        $('#checkAll').click(function(){
            //alert("checking all");
            $('input[name="sendItems"]').prop('checked', $(this).attr('checked')?true:false);
        });

        $('input[name="sendItems"]').click(function(){
            if(!$(this).attr('checked'))
                $('#checkAll').prop('checked',false);
        });

        $('#printing').click(function(){printDoc();});
    });

    //////////////////////////////////////////////////
    function Checkemail(value) { //64

       return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(value);
   }
   ///////////////////////////////////////////////////

    function displayResult() { //64
        //////////////////////////////////////////////
       // document.getElementById("SendEmail").style.display = "block";
        var email = document.getElementById("Email").value;
        var selectformate = document.getElementById("SelectFormate").value;
        var message = document.getElementById("message").value;
        var subject = document.getElementById("subject").value;
       
        if (email.length == 0) {
            $('#Email').css('background-color','#ffd0d0');
                return false;
       }
       if (selectformate.length == 0) {
           $('#SelectFormate').css('background-color', '#ffd0d0');
                return false;
       }
       if (subject.length == 0) {
          $('#subject').css('background-color', '#ffd0d0');
                return false;
       }
       if (message.length == 0) {
            $('#message').css('background-color', '#ffd0d0');
                return false;
       }
       var Result = Checkemail(email)
           if (Result) {
         
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
                     
                           document.getElementById("Close").style.display = "block";
                           document.getElementById("mainTable").style.display="none";
                           document.getElementById("popUpDiv").style.height = "200px";
                       

                   }
               }
               xmlhttp.open("POST", "EmailGroup.aspx?email=" + email + "&mypageid=" + sectsFile +"&sectno=" + sectsIds + "&sbj=" + subject + "&msgf=" + selectformate + "&msg=" + message +"&pageType=leg", true);
               xmlhttp.send();
               //mail="";
           }
           else
           {alert("Please Type Correct Email");}
   }



   //////////////////////////////////////////////////////////////////////////
    $(document).ready(function() {
        
        
        
        //function for load subsidary page 
        $(".load_subsidary").click(function(){
       
        var link = $(".load_subsidary").attr("title");
       
        var load_tab = 1;
        
         $.ajax({
                type: "POST",
                url: link,
                data: {load_tab:"1"},
                success: function(msg) {
               

                $(".feedback-tab").hide();
             $( ".case_citation_panel" ).animate({
               right: 0
             }, {
               duration: 1000,
              
              });
               $(".tickBox_1").html("Search Is Loading.....<img src='GUI/images/loading.gif' />").fadeIn(30000);
                var div = $('#lblTb1Jquery', $(msg)).addClass('done');
        
                   $('.tickBox_1').html(div);
                   
                    return false;
                }
            });
            return false;
        
        });
        //function for loading amending
         $(".load_amending").click(function(){
   
        var link = $(".load_amending").attr("title");
     
        var load_tab = 1;
        
         $.ajax({
                type: "POST",
                url: link,
                data: {load_tab:"1"},
                success: function(msg) {

                $(".feedback-tab").hide();
             $( ".case_citation_panel" ).animate({
               right: 0
             }, {
               duration: 1000,
              
              });
              $(".tickBox_1").html();
               $(".tickBox_1").html("Search Is Loading.....<img src='GUI/images/loading.gif' />").fadeIn(30000);
                 var div = $('#lblTb1Jquery', $(msg)).addClass('done');
        
                   $('.tickBox_1').html(div);
                    return false;
                }
            });
            return false;
            });
       
    });
    var GottoTop;
   function show_sec_content(filename,secno)
	{
	var SDdisplay= "#SD" + secno.replace(".","");
    var legdiv = "#printdiv" + secno.replace(".","");
    if($(SDdisplay).is(':hidden')){
       $('.SDclear').hide();
       $(SDdisplay).slideDown("fast",'linear',function(){  
       $('html,body').animate({scrollTop: $(SDdisplay).position().top - 40},'fast');
       $.ajax({
                type: "POST",
                url: "LegislationMainDisplayed.aspx/sendJSON",
                data: '{id: "' + filename + '", sec: "' + secno + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                       $(legdiv + " span").html('');    
                    $(legdiv + " span").append(data.d);            
                        },
                failure: function (response) {
                }
            });
       }); 
     } else $(SDdisplay).slideUp("fast");
             
    }
     //function List of Amendment
        ////////////////Load SH 
        function show_am_content(filename,type,No)
	{
	
	  
	 var strsecid="LegislationSectionDisplayed.aspx?fn=" + filename +"&tp=" +type +"&schNO=" +No;
	var SHdisplay= "#SH" + No;
     if (!$(SHdisplay).is(':empty') )
	 {
    
     $('#SHimg' +No).html('<img src="img/arrowEx1.png"/>');
     $(SHdisplay).html("");
      $('.SHclear').html("");
       return false;
     }
     else{
          $('.boxCheck1').html('<img src="img/arrowEx1.png"/>');


                          $.ajax({
                type: "POST",
                url: strsecid,
                data: {load_tab:"1"},
                success: function(msg) {
             
                 var divload = $('#lblXml', $(msg)).addClass('done');
                 $('.SHclear').html("");
                 
                  $(SHdisplay).html(divload.html());
                   $(SHdisplay).slideDown("slow");
                   $('#SHimg' + No).html('<img src="img/arrowEx2.png"/>');
                  
                    return false;
                }
                
            });
           
           }
	  
		}

        /////////



    //End function of List of Amendment
	
    //function for scrol top elaw legislation section display
    $(function () {
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
	});
    
</script>
		<script>
		    //////script for tab 
		    function tabSwitch_2(active, number, tab_prefix, content_prefix) {

		        for (var i = 1; i < number + 1; i++) {
		            document.getElementById(content_prefix + i).style.display = 'none';
		            document.getElementById(tab_prefix + i).className = '';
		        }
		        document.getElementById(content_prefix + active).style.display = 'block';
		        document.getElementById(tab_prefix + active).className = 'active';

		    }
		    ///////////////////////////////////////////////////////////////64
		    //abdo star sub srch

		    function resetddl() {
		        var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
		        lstddlrbs.options[lstddlrbs.selectedIndex].value = 1;

		        var lstddlproxmity = document.getElementById('<%=ddlproxmity.ClientID%>');
		        lstddlproxmity.options[lstddlproxmity.selectedIndex].value = 3;

		        var lstddlprox = document.getElementById('<%=ddlprox.ClientID%>');
		        lstddlprox.options[lstddlprox.selectedIndex].value = "";
		        lstddlprox.options[lstddlprox.selectedIndex].text = "";
		    } //end of function reset drop down list to their 

		    //abdo work end
		    function CheckSubjectIndex() {

		        //abdo work start
		        resetddl(); //reset drop down list srch options


		        var ddlvalue = "";
		        var ddltext = "";
		        var lstddlsrch = document.getElementById('<%=ddlsrch.ClientID%>');
		        ddlvalue = lstddlsrch.options[lstddlsrch.selectedIndex].value;
		        ddltext = lstddlsrch.options[lstddlsrch.selectedIndex].text;

		        ddltext = ddltext.toLowerCase();



		        if (ddltext.indexOf("legislation") > -1) {
		            var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
		            lstddlrbs.options[lstddlrbs.selectedIndex].value = 1;
		            // alert("you choose judment");
		        }
		        else
		            if (ddltext.indexOf("act") > -1) {

		                var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
		                lstddlrbs.options[lstddlrbs.selectedIndex].value = 2;

		            }
		            else
		                if (ddltext.indexOf("section") > -1) {

		                    var lstddlrbs = document.getElementById('<%=rbs.ClientID%>');
		                    lstddlrbs.options[lstddlrbs.selectedIndex].value = 3;
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

		        // alert("check before submit");
		    } //end of javascritp funtion to update            //abdo work end

       
            /////end of abdo sub srch
            
            function validate() {
                CheckSubjectIndex();
		        if ($('#txtFTS').val().trim().length == 0) {
		            $('#txtFTS').css('background-color', '#ffd0d0');
		            
		        } else {
		            $('#txtFTS').css('background-color', '#ffffff');

		            __doPostBack('LinkButton1', '');
		        }
		    }
        </script>
		<style type="text/css">
		    /*scrol top*/
#back-top {
	position: fixed;
	bottom: 5px;
	margin-left: 10px;
	margin-bottom:40px;
	
}		    


#back-top a {
	display: block;
	text-align: center;
	margin:0 0 15px 27px;
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
	width: 60px;
	height: 80px;
	display: block;
	margin-bottom: 7px;
	background: #ddd url(GUI/images/up-arrow.png) no-repeat center center;
	/* rounded corners */
	-webkit-border-radius: 15px;
	-moz-border-radius: 15px;
	border-radius: 15px;
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
<body ><!--onunload="check()">-->

<form id="Form1" method="post" runat="server">
	
<!-- #include file="include/headerMainLegis.aspx" -->
<div class="findToggle">
    FIND &nbsp; <img src="../img/arrowDn.png" />
 </div>
<!-- Content 2 -->
<div id="content2">
	
    <div id="showPanel">
	<div class="leftQuicksearch" style="margin-top:18px;">
    <div class="btnShowSearch"><img style="margin:8px 0 0 8px;" align="top" src="img/ico2.png"/></div>
    <p>Browse</p></div>
    </div>
    
    <div id="colleft1">	
	<div id="panel">	
	
    <div id="tabbed_box" class="tabbed_box">
    <div class="tabbed_area">
        <ul class="tabs">
          <li><a href="javascript:tabSwitch_2(1, 4, 'tab_', 'content_');" id="tab_1" class="active"><img align="middle" src="GUI/NewDesign/img/iconSectionNo.png" alt="View all Sections"/></a></li>
            <li><a href="javascript:tabSwitch_2(2, 4, 'tab_', 'content_');" id="tab_2"><img align="middle" src="GUI/NewDesign/img/iconSearch.png" alt="Search"/></a></li>
            <li><a href="javascript:tabSwitch_2(3, 4, 'tab_', 'content_');" id="tab_3">Sub</a></li>
			<li><a href="javascript:tabSwitch_2(4, 4, 'tab_', 'content_');" id="tab_4">Amd</a></li>
			<li ><a href="#"  id="hidePanel2"><img  align="middle" src="img/ico1.png"/></a></li>
        </ul>    
        <div id="content_1" class="content">
        	<ul>
            	<%Response.Write(ListSection) %>
			</ul>
                    </div>
        <div id="content_2" class="content">:
        	<table width="100%" border="0" cellpadding="4" cellspacing="1" style="color:#3e4346;">
            
  <tr>
 
          <td><asp:TextBox ID="txtFTS" runat="server" class="advBoxS"></asp:TextBox>
</td>
        </tr>
        <tr>
          <td>With in:</td>
        </tr>
        <tr>


          <td>
          
          <asp:DropDownList ID="ddlsrch" runat="server" class="advBoxS6">
                    <asp:ListItem Selected="True" Value="1">Legislation</asp:ListItem>
                    <asp:ListItem Value="2">Act Title</asp:ListItem>
                    <asp:ListItem Value="3">Section Title</asp:ListItem>

                    <asp:ListItem Value="3">3 Words</asp:ListItem>
                    <asp:ListItem Value="4">4 Words</asp:ListItem>
                    <asp:ListItem Value="5">5 Words</asp:ListItem>
                    <asp:ListItem Value="1">same sentence</asp:ListItem>
                    <asp:ListItem Value="2">same paragraph</asp:ListItem>
                    <asp:ListItem Value="3">same page</asp:ListItem>
                    </asp:DropDownList>
          
          <asp:DropDownList ID="rbs" runat="server" class="advBoxS" style="visibility:hidden;  height:25px;  ">
              <asp:ListItem Selected="True" Value="1">Full Document</asp:ListItem>
                      <asp:ListItem Value="2">Act Title</asp:ListItem>
                      <asp:ListItem Value="3">Section Title</asp:ListItem>

              </asp:DropDownList>
            </td>
        </tr>
        
        <tr>
          <td>Without the words:</td>
        </tr>
        <tr>
          <td><asp:TextBox 
                  ID="txtNotCases" runat="server" class="advBoxS"></asp:TextBox>
            </td>
        </tr>

        <tr>
          <td><table width="100%" border="0" cellpadding="5" cellspacing="0">
            <tr>
              <td width="25%"></td>
              <td width="75%" colspan="2"><asp:DropDownList 
                          ID="ddlproxmity" runat="server" CssClass="advBoxS6" Width="160px" style="visibility:hidden;">
                      <asp:ListItem Value="3"> Same Page</asp:ListItem>
                      <asp:ListItem Value="2"> Same Paragraph</asp:ListItem>
                      <asp:ListItem Value="1"> Same Sentence</asp:ListItem>
                      </asp:DropDownList></td>
            </tr>

            <tr>
              <td width="25%"></td>
              <td width="50%" ><asp:dropdownlist id="ddlprox" runat="server" 
       class="advBoxS6" style="visibility:hidden;  height:25px; width:100px;" EnableViewState="False">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="100">10</asp:ListItem>
                    <asp:ListItem Value="200">20</asp:ListItem>
                    <asp:ListItem Value="300">30</asp:ListItem>
                    
                      </asp:dropdownlist></td>
                      <td></td>
            </tr>
            
          </table></td>
        </tr>
        <tr>
          <td align="center" ><div class="btnDefine" onclick="validate()">Search<asp:LinkButton ID="LinkButton1" runat="server" style="display:none;">Search</asp:LinkButton>
          </div></td>
        </tr>
      </table>
        </div>
        <div  id="content_3" class="content">
        	<ul>
            <span id="lbllinks" style="display:none;"><a href="<%Response.Write(GetSubSidairy)%>"></a></span>   
        <div id="GetsubResult"></div>
        <div class="boxRecentResult"> <h2 class="title2Case2"><%Response.Write(MoreSubsiary) %></h2></div>
     
			</ul>
        </div>
        <div id="content_4" class="content">
        	<ul>
             <span id="lbllinks1" style="display:none;"><a href="<%Response.Write(GetAmdment)%>"></a></span>   
        <div id="GetAmdment"></div>
        <div class="boxRecentResult"> <h2 class="title2Case2"><%Response.Write(MoreAmd)%></h2></div>
        
        
			</ul>
        </div>
    
    </div></div>
	
	</div>  
	</div>
    
	<div id="contRight">
    
    <section class="resultsRight">&nbsp;<div class="clear"></div>
    
 	<!-- left Bar1 Results Elements -->
    
    <div class="topBar1">
    <div class="leftResultBox"><strong style="text-transform:capitalize;"><asp:label id="lblPageTop" runat="server"></asp:label>
																</strong></div>
    
    <!-- Right Bar1 Results Elements -->
    
    
    
    
    
    <div class="clear"></div>
    </div>
    
    <!-- left Bar1 Results Elements 2 -->
    
    <div class="topBar2">
    
    <div class="leftResultBox2">
    <div class="funtionResults">
     <a href="#" style="text-decoration:none;"><img src="GUI/NewDesign/img/iconSectionNo.png"/>Section No</a>
                <input type="text"  id="txtjumpto" runat="server"  style="width:30px;" maxlength="4" />
                <a href="#" id="jumpto"><img src="GUI/NewDesign/img/iconNextPage.png" /></a>
               
    <%Response.Write(ListSchudle) %>
        <%Response.Write(ListPremple)%>
            <%Response.Write(ListAmendments) %>
    </div>
    </div>
    
    <div class="rightResultBox">
    <div class="funtionResults">
    <a style="cursor:pointer" id="TlTrigger"><img src="img/tl2.png"/></a>
      <a href="#" id="emailing"><img border="0" src="img/icoEmail.gif"/>Email</a>
      </div>
     </div>
    
    <div class="clear">
        <asp:HiddenField ID="mainacthidden" runat="server" />
        </div>
    </div>
    
    <!-- Box Display Cases -->
    <div class="findDiv" style="display:none">
    <a style="margin-right:5px;padding:1px;cursor:pointer;" id="findX"><img src="img/x.png" /></a>
    <span id="FCount"></span>
    <input type="text" size="30" id="findWord"/>
    <button type="button" id="findPrev">▲</button>
    <button type="button" id="findNext">▼</button>
    </div>
    <div class="boxDisplay">

        <%--<div id="amend"><font size="3" face="Vardana" color="#465877"><b>AMENDMENTS</b></font></div>--%>
        <div class="timeLine" style="width:100%;margin-left:0%;"></div>
        <asp:Label id="Id" runat="server" style="display:none;"></asp:Label>
        <asp:Label ID="TLF" runat="server" style="display:none"></asp:Label>
         <div class="clear"></div>
         <div class="clear"></div>
		<asp:label id="lblXml" runat="server" CssClass="tTextNorm" Font-Bold="False" style="padding-bottom:200px;"></asp:label>
        <asp:Label ID="lblMsg" runat="server" Font-Bold="False"></asp:Label>
        <br /><br /><br />
    </div>
    <div class="SDclear">
    
    
    
    </div>
    </section>
    
    <div class="clear"></div>
  	</div>
</div>


 <footer> 
	<div class="wrapperFooter">
	<section class="wrapperAddfooter">
    
    <p id="back-top" style="display:none;">
		<a href="#top"><img src="../GUI/images/up-arrow.png" border="0" /></a>
	</p>
    
    <%If PartStyle <> "" Then%>
    <div class="gotoPage" align="center">
    <div class="numPage">
     <ul><li>Part</li>
     <%Response.Write(PartStyle) %></ul>
     </div></div>
     <%End If%>
    <div class="clear"></div>
    </section>
	
    <!-- #include file="include/footerMain.aspx" -->

    </div>
</footer>
	
    <div id="blanket" style="display:none;"></div>
	<div id="popUpDiv" style="display:none;width:380px;">
	
        <div id='form2' >	
	<h3><span>Send Case(s) To</span></h3>
    <div style="margin:10px 30px;">
            <p style="margin:0">  <span id='msg'></span>  </p>
            <p class="submit" style="display:none;" id="Close">
                 <button  type='button' value='Submit' id='Button2' onclick="popup('popUpDiv')">Close & Continue!</button>
            </p>
	       <table style="width: 100%;" id="mainTable">
                    <tr>
                        <td>Email</td>
                         <td> <input type='text' name='name' id='Email' maxlength='20' style="width:222px;" /></td>
                        
                        </tr>
                        <tr>
                        <td>Email Format</td>
                        <td><select id="SelectFormate" style = "width:228px;" >
                <option value="3">Message </option>
			    <option value="2">HTML</option>
			    </select></td>
                        </tr>
                        <tr>
                        <td>Subject</td>
                        <td> <input type="text" name="subject" id="subject" maxlength="50" style="width:222px;" /></td>
                        </tr>
                        <tr>
                        <td>Message</td>
                        <td><textarea rows="3" cols="40" id="message" style="width:222px;resize:none;"></textarea></td>
                        </tr>
                        <tr>
                        <td></td>
                        <td>
                        <p class='submit'><button id='btnSubmit' type='button' value='Submit' onclick="displayResult()" class="btnGo">Send</button>
                <button  type='button' value='Submit' id='btnClose' onclick="popup('popUpDiv')" class="btnGo">Cancel</button>
                        </td>
                        </tr>
                        </table>
                        </div>
   </div>
	</div>		
				  <div id="popupDiv3" style="display:none;background-color:#757575;font-size:14px;padding:0px 0px 5px  0px;-webkit-box-shadow:0 1px 1px #000;-moz-box-shadow:0 1px 1px #000;box-shadow:0 1px 1px #000;border-radius:5px;position:absolute;width:350px;z-index: 9002;" >
        <div class="form4">
        <h3><span>Error!</span></h3>
        <fieldset>
            <p style="text-align:center">Please Select Section(s)</p>
            <button type="button" onclick="popup('popupDiv3')" class="btnGo" style="margin-left:130px;margin-top:20px;">Close</button>
        </fieldset>
        </div>
    </div>

		</form>
        <script src="js/script.js" type="text/javascript"></script>

         <script type="text/javascript">
             $(document).ready(function () {
                 $('.content ul').height($(window).innerHeight() * 0.9 - 110);
                 //min font size
                 var min = 12;
                 //max font size
                 var max = 18;
                 $('a.fontSizePlus').click(function () {
                     var reset = $('div.intro').css('fontSize');
                     var elm = $('div.intro');
                     //set the default font size and remove px from the value
                     var size = str_replace(reset, 'px', '');
                     if (size <= max) {
                         //increase the size
                         size++;

                         //set the font size
                         elm.css({ 'fontSize': size });
                     }

                     //cancel a click event
                     return false;

                 });

                 $('a.fontSizeMinus').click(function () {
                     var reset = $('div.intro').css('fontSize');
                     var elm = $('div.intro');

                     //set the default font size and remove px from the value
                     var size = str_replace(reset, 'px', '');
                     if (size >= min) {

                         //decrease the size
                         size--;

                         //set the font size
                         elm.css({ 'fontSize': size });
                     }

                     //cancel a click event
                     return false;

                 });

                 //Reset the font size
                 $('a.fontReset').click(function () {

                     var elm = $('div.intro');
                     elm.css({ 'fontSize': 14 });
                     return false;
                 });
                 //A string replace function
                 function str_replace(haystack, needle, replacement) {
                     var temp = haystack.split(needle);
                     return temp.join(replacement);
                 }


                 var Getsub;
                 var Getamd;
                 $("#panel").animate({ marginLeft: "-266px" }, 5);
                 //$("#colleft").animate({ width: "0px", opacity: 0 }, 4
                 $("#colleft1").css({ 'width': '0px', 'opacity': '0' });
                 $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
                 $("#contRight").animate({ marginLeft: "50px" }, 5);
                 $("#hidePanel2").click(function () {

                     $("#panel").animate({ marginLeft: "-266px" }, 500);
                     $("#colleft1").animate({ width: "0px", opacity: 0 }, 400);
                     $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
                     $("#contRight").animate({ marginLeft: "50px" }, 500, function () {
                         if ($("#printing").html("<img src='img/icoPrint.gif' border='0'>")) {
                             $("#printing").html("<img src='img/icoPrint.gif' border='0'>Print");
                             $("#emailing").html("<img src='img/icoEmail.gif' border='0'>Email");
                             $("#Download").html("<img src='img/icoDownload.gif' border='0'>Download");
                             $("#SaveS").html("<img src='img/icoSave.gif'border='0'>Save Search");
                         }

                     });


                 });
                 $("#showPanel").click(function () {
                    
                     tabSwitch_2(1, 4, 'tab_', 'content_');

                     $("#contRight").animate({ marginLeft: "280px" }, 200);
                     $("#panel").animate({ marginLeft: "0px" }, 400);
                     $("#colleft1").show().animate({ width: "266px", opacity: 1 }, 400).show("slow");
                     $("#showPanel").animate({ width: "0px", opacity: 0 }, 600).hide("slow");
                     $("#colleft1").css({ 'top': '16px', 'position': 'fixed' });
                     if ($("#printing").html("<img src='img/icoPrint.gif' border='0'>Print")) {
                         $("#printing").html("<img src='img/icoPrint.gif' border='0'>");
                         $("#emailing").html("<img src='img/icoEmail.gif' border='0'>");
                         $("#Download").html("<img src='img/icoDownload.gif' border='0'>");
                         $("#SaveS").html("<img src='img/icoSave.gif'border='0'>");
                     }

                 });
                 $('#tab_3').click(function () {
                     tabSwitch_2(3, 4, 'tab_', 'content_');
                     Getsub = $("#lbllinks a").attr('href');
                     Getsub = Getsub + "&j=sub";

                     $.ajax({
                         type: "POST",
                         url: Getsub,
                         data: { load_tab: "1" },
                         success: function (msg) {

                             $("#GetsubResult").html("Search Is Loading.....<img src='GUI/images/loading.gif' />").fadeIn(30000);
                             var div = $('#lblTb1Jquery', $(msg)).addClass('done');
                             $('#GetsubResult').html(div);
                             return false;
                         }
                     });
                     return false;
                 });
                 //////////////////////////////////////////
                 $('#tab_4').click(function () {
                     tabSwitch_2(4, 4, 'tab_', 'content_');
                     Getamd = $("#lbllinks1 a").attr('href');
                     Getamd = Getamd + "&j=sub";
                     $.ajax({
                         type: "POST",
                         url: Getamd,
                         data: { load_tab: "1" },
                         success: function (msg) {
                             $("#GetAmdment").html("Search Is Loading.....<img src='GUI/images/loading.gif' />");//.fadeIn(30000);
                             var div = $('#lblTb1Jquery', $(msg)).addClass('done');
                             $('#GetAmdment').html(div);
                             return false;
                         }
                     });
                     return false;
                 });

             });
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
