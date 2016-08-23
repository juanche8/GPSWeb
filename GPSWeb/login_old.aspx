<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login_old.aspx.vb" Inherits="GPSWeb._Default" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>LOGIN</title>
    <link href="css/login_admin.min.css"rel="stylesheet" type="text/css" />
	<link href="http://i.s-microsoft.com/en-us/home/style.cssx?k=~/shared/templates/Styles/reset-css.aspx;~/shared/templates/Styles/viewport-css.aspx;~/shared/templates/Styles/cols-css.aspx;~/shared/templates/Styles/webfont-css.aspx;~/shared/templates/Styles/type-css.aspx;~/shared/templates/Styles/layout-css.aspx;~/shared/templates/Styles/header-css.aspx;~/shared/templates/Styles/_search-css.aspx;~/shared/templates/Styles/_listoflinks-css.aspx;~/shared/templates/Styles/_menu-css.aspx;~/shared/templates/Styles/hero-css.aspx;~/shared/templates/Styles/_pivot-css.aspx;~/shared/templates/Styles/_features-css.aspx;~/shared/templates/Styles/_news-css.aspx;~/shared/templates/Styles/prefooter-css.aspx;~/shared/templates/Styles/footer-css.aspx;~/shared/templates/Styles/animations-css.aspx;~/shared/templates/Styles/feedback-css.aspx&sc=/en-us/home/site.config&pc=&v=615325067" rel="stylesheet" type="text/css"/>
</head>


<body>

    <form id="form1" runat="server">
    <div class="barra Orange">
    </div>

    
    
	

	<H3 class=heading bi:title="t1" bi:titleflag="t1" style="padding:20px;border:0px solid black;">   Sistema de control de flotas</H3>
	
	
	
	
	<section class="row-blue row-padded">
    <div class="grid-container">
        <div class="grid-row row-4 "><div class="grid-unit col-3 col-flow-opposite">
				<div bi:type="slideshow" class="slideshow slideshow-news">
				
						<ul class="slides">
						   <asp:Login ID="Login1" runat="server" Width="628px" 
                    FailureText="Usuario o Contraseña incorrectos.">
                    <LayoutTemplate>
							<div bi:type="highlight">

								<a>
									<img  src="http://i.s-microsoft.com/global/en-us/news/publishingimages/homepage/highlights/misc_rothschild_hl.jpg" alt="Predicting this year's Oscar winners" bi:mimiclink="true" bi:type="img">
									<h3>Ingreso de usuarios</h3>
														<TABLE BORDER="0"> 
														<TR> 
														   <TH>Usuario</TH> 
														   <TH><asp:TextBox ID="UserName" runat="server" /></TH> 
														</TR> 
														<TR> 
														   <TD>Contraseña</TD> 
														   <TD><asp:TextBox ID="Password" runat="server" TextMode="Password" /></TD> 
														 
														</TR> 
														<TR> 
														   <TD><asp:CheckBox ID="RememberMe" runat="server" /></TD> 
														   <TD>Recordarme</TD> 
														  
														</TR> 
														<TR> 
														
														<TD> <asp:Button ID="btnLogin" runat="server" SkinID="pixelLoginButton" Text="Ingresar" onclick="Login_Click" cssClass="pivot-switch" />
														</TD>
														<td><asp:Label ID="lblError" Visible="false" runat="server" Font-Bold="true" ForeColor="Red" Text="Usuario o Contraseña Incorrectos."></asp:Label></td>
														</TR>
														
														</TABLE> 								
									
								</a>
								
								
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" runat="server">Recuperar Contraseña</asp:LinkButton>
								
								
								
							</div>
							  </LayoutTemplate>
						 </asp:Login>
						</ul>
		 
		 
				</div>

    </div>
		
			<div class="grid-unit col-1">
					<div bi:type="list" class="list-of-links list-of-links-lg list-array">
								<h2 class="heading">Síganos en								
								<ul bi:parenttitle="t1">
									<li><a bi:index="0" bi:linkid="SOC-00-000000" bi:cpid="hpSocial" href="http://www.facebook.com/microsoft">Facebook</a>
									</li>
									<li><a bi:index="1" bi:linkid="SOC-00-000000" bi:cpid="hpSocial" href="http://www.twitter.com/microsoft">Twitter</a>
									</li>
									<li><a bi:index="2" bi:linkid="SOC-00-000000" bi:cpid="hpSocial" href="http://www.microsoft.com/news">News Center</a>
									</li>
								</ul>
					</div>
			</div>
		</div>
    </div>
</section>


    </form>
</body>
</html>

