<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login_admin.aspx.vb" Inherits="GPSWeb.login_admin" %>

<!DOCTYPE HTML>
<html>

<head id="Head1" runat="server">

<meta charset="utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">  
  <title>Rastreo Urbano Administracion</title>  

       <link rel="stylesheet" href="css/menu/bootstrap.css" type="text/css" />
   
    <style>
    html {
	overflow-y: scroll;
	min-height: 100%;
}

/* Create a stacking context to prevent z-index issues */
body {
	position: relative;
	margin: 0;
	font: 16px/24px Calibri,Helvetica,sans-serif;
    color: #444;
}
#block-header {
    background: none repeat scroll 0% 0% #373435;
}

#logo {
    height: 72px;
}
#logo, #logo > img, #menu {
    float: left;
}
a, code {
    color: #0081C1;
}
a, a:hover {
    text-decoration: none;
}
a {
    color: #08C;
    text-decoration: none;
}

.bg-white {
    background: none repeat scroll 0% 0% #FFF;
}
.bg-white, .bg-colored {
    padding: 20px 0px;
}
.wrapper {
    max-width: 980px;
}
.wrapper {
    box-sizing: border-box;
    margin: auto;
}
#maininner {
    width: 100%;
}
.grid-box {
    float: left;
}
#breadcrumbs, #content {
    margin: 20px 10px;
}
article, aside, details, figcaption, figure, footer, header, hgroup, nav, section {
    display: block;
}
article, aside, details, figcaption, figure, footer, header, hgroup, nav, section {
    display: block;
}
#system .item > :last-child {
    margin-bottom: 0px;
}
.clearfix {
}

*::-moz-selection {
    color: #FFF;
}
.clearfix:after, .grid-block:after, .deepest:after {
    clear: both;
}
.clearfix:after {
    clear: both;
}
.clearfix:after {
    clear: both;
}
.clearfix:before, .clearfix:after, .grid-block:before, .grid-block:after, .deepest:before, .deepest:after {
    content: "";
    display: table;
}
.clearfix:before, .clearfix:after {
    display: table;
    content: "";
    line-height: 0;
}
.clearfix:before, .clearfix:after {
    content: "";
    display: table;
}
#system .item > .content > :last-child:not(.grid-gutter) {
    margin-bottom: 0px;
}
#system .item > .content > :first-child:not(.grid-gutter), #system .item > .content > [class*="align"]:first-child + * {
    margin-top: 0px;
}
form, textarea {
    margin: 0px;
}
form {
    margin: 0px 0px 20px;
}
p, hr, dl, blockquote, pre, fieldset, figure {
    margin: 15px 0px;
}
fieldset {
    padding: 0px;
    margin: 0px;
    border: 0px none;
}
table {
    max-width: 100%;
    background-color: transparent;
    border-collapse: collapse;
    border-spacing: 0px;
}
th, td {
    padding: 1px;
}
body-dark, .bg-dark {
    background: none repeat scroll 0% 0% #353535;
}
.bg-colored h1, .bg-colored h2, .bg-colored h3, .bg-colored h4, .bg-colored h5, .bg-colored h6, .bg-colored {
    color: #FFF;
}
.bg-colored {
    border-top: 1px solid rgba(0, 0, 0, 0.15);
    box-shadow: 0px 1px 0px rgba(255, 255, 255, 0.1) inset;
}
.bg-white, .bg-colored {
    padding: 20px 0px;
}
.body-dark, .bg-dark {
    background: none repeat scroll 0% 0% #353535;
}
.bg-colored h1, .bg-colored h2, .bg-colored h3, .bg-colored h4, .bg-colored h5, .bg-colored h6, .bg-colored {
    color: #FFF;
}
.bg-colored {
    border-top: 1px solid rgba(0, 0, 0, 0.15);
    box-shadow: 0px 1px 0px rgba(255, 255, 255, 0.1) inset;
}
.bg-white, .bg-colored {
    padding: 20px 0px;
}

textarea, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input {
    background-color: #FFF;
    border: 1px solid #CCC;
    box-shadow: 0px 1px 1px #F58634 inset;
    transition: border 0.2s linear 0s, box-shadow 0.2s linear 0s;
}
select, textarea, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input {
    display: inline-block;
    height: 20px;
    padding: 4px 6px;
    margin-bottom: 10px;
    font-size: 14px;
    line-height: 20px;
    color: #555;
    border-radius: 4px;
    vertical-align: middle;
}
input, textarea, .uneditable-input {
    margin-left: 0px;
}
input, textarea, .uneditable-input {
    width: 206px;
}
input, button, select, textarea {
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
}
label, input, button, select, textarea {
    font-size: 14px;
    font-weight: normal;
    line-height: 20px;
}
button, input {
    line-height: normal;
}
button, input, select, textarea {
    margin: 0px;
    font-size: 100%;
    vertical-align: middle;
}

button, input[type="button"], input[type="submit"] {
    padding: 5px 12px;
}
button, input[type="button"], input[type="submit"] {
    padding: 2px 3px;
}
input[type="file"], input[type="image"], input[type="submit"], input[type="reset"], input[type="button"], input[type="radio"], input[type="checkbox"] {
    width: auto;
}
label, select, button, input[type="button"], input[type="reset"], input[type="submit"], input[type="radio"], input[type="checkbox"] {
    cursor: pointer;
}
button, html input[type="button"], input[type="reset"], input[type="submit"] {
    cursor: pointer;
}
.button4 {
    background-color: #F58634;
    border-radius: 0px;
    text-indent: 0px;
    border: 1px solid #F58634;
    display: inline-block;
    color: #373435;
    font-family: Arial;
    font-size: 14px;
    font-weight: bold;
    font-style: normal;
    padding: 2px;
    text-decoration: none;
    text-align: center;    
    vertical-align: bottom;
}
input, textarea, .uneditable-input {
    margin-left: 0px;
}
input, textarea, .uneditable-input {
    width: 206px;
}
input, button, select, textarea {
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
}
label, input, button, select, textarea {
    font-size: 14px;
    font-weight: normal;
    line-height: 20px;
}
button, input {
    line-height: normal;
}
button, input, select, textarea {
    margin: 0px;
    font-size: 100%;
    vertical-align: middle;
}

ul, ol {
    padding: 0px;
    margin: 0px 0px 10px 25px;
}

h3 {
    font-size: 18px;
    line-height: 28px;
    color: #373435;
    font-family: "Verdana";
     margin: 25px 0px 15px;
}

</style>
</head>
<body >
	
		
    <form id="form1" runat="server" >
         <div style="text-align:center;color:#070;">
             <br />
             <br />
            <img alt="" src="images/logo-login.jpg" />

</div>
<table class="contentpane" style="width: 100%; margin-top:5px;" border="0" >
<tbody>
<tr valign="top">

<td>
<table style="font-size:12px; margin-left:400px; width:500px; border-left-style: solid; border-left-width: 1px; border-left-color: #d3d3d3; border-right-style:solid; border-right-width: 1px; border-right-color: #d3d3d3; border-top-style:solid; border-top-width: 1px; border-top-color: #d3d3d3; border-bottom-style:solid; border-bottom-width: 1px; border-bottom-color: #d3d3d3; font-family:Arial; text-align:center" >

<tbody>
<tr>
<td>
<h3>Acceso Panel de Administración<span style="font-size: 1.17em; text-align: left;">&nbsp;</span></h3>
</td>
</tr>
<tr valign="top" style="height:40px;">
<td valign="top">
<input id="username" runat="server" class="inputbox" style="margin: 0; height: 24px; width: 280px;" type="text" name="username" alt="User" placeholder="User" /></td>
</tr>
<tr valign="top" style="height:40px;">
<td valign="top"><input id="password" runat="server" class="inputbox" style="margin: 0; height: 24px; width: 280px;" type="password" name="password" alt="password" placeholder="Password" /></td>
</tr>
<tr valign="top">
  
<td valign="top">  <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
</tr>
<tr valign="top">
<td valign="bottom">
<div>
    <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="button4" />
<div id="com-form-login-error" style="color: red; font-size: small;">&nbsp;</div>
</div>
</td>
</tr>
<tr>
<td>
<div>&nbsp;</div>
</td>
</tr>
</tbody>

</table>
</td>
</tr>
</tbody>
</table>
<div style="text-align:center;color:#070;">
<p style="color:#17354D; font-family:Verdana; font-size:12px"><b></b></p>
<div><br /></div>
</div>
    </form>
		
</body>
</html>