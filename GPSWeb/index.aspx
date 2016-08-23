<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="index.aspx.vb" Inherits="GPSWeb.index" MasterPageFile="~/Site3.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="box" style="background-color: #FFF; clear: both; font-family: Helvetica, sans-serif, 'Gill Sans MT'; color: #696868;">
 <div id="slideshow" style="width: 1000px;position: relative;margin: 0px auto">
   <ul class="bxslider">
       <li>
         <a href="contacto.aspx"><img src="img/promo-instalacion.png" width="1000" border="0" height="330" alt="Promocion" title="Rastreo Urbano Promocion en Instalacion"  /> </a>
      
      </li>
      <li>
      <a href="servicios.aspx"><img src="img/flota-autos.png" width="1000" height="330"  border="0" alt="Flota Autos" /> </a></li> 
       <li>
         <a href="servicios.aspx"> <img src="img/flota-utilitarios.png" width="1000"  border="0" height="330" alt="Flota Utilitarios" title="Flota Utilitarios"/></a>
    </li>
    <li>
         <a href="servicios.aspx"> <img src="img/particulares.png" width="1000" height="330"  border="0" alt="particulares" title="Particulares"/></a>
    </li>
      <li>
         <a href="prodmodulos.aspx"> <img src="img/equipo-gps.png" width="1000" height="330"  border="0" alt="Equipos GPS" title="Equipos GPS"/></a>
    </li>
      
  </ul>
 </div>

 <div id="blanco" style="height:auto; width:auto;">
<div style="width:100%; height:5px;background: #F60; clear:both; margin-top:5px"></div>
 <div style="clear: both">
  
 </div>
<div style="width:80%; margin-right:10%;margin-left:13%; font-size:12px; ">
  <div style="float: left; width: 20%; margin-right:0px;color:#696868; font-family:Verdana; ">
    <h4><a href="contacto.aspx"><img src="img/emprendedor_.gif" width="179" height="205" alt="Emprendedores" border="0" /></a></h4>
     <h4>Emprendedores</h4>
<p>Iniciá tu propio negocio, siendo un distribuidor de <span style="color: #F60; font-weight: bold;">Rastreo Urbano</span></p>
</div>
   <div style="float: left;margin-left:4%;color:#696868; font-family:Verdana; width: 23%;">
     <h4><a href="contacto.aspx"><img src="img/mecanico_.gif" width="180" height="205"  alt="Taller"  border="0" /></a></h4>
     <h4>Sos dueño de un taller mecánico? </h4>
  <p>Agrandá tu negocio generando mayores ingresos! Súmate a la red de instaladores de dispositivos de <span style="color: #F60; font-weight: bold;">Rastreo Urbano</span>.</p> 
  </div>
<div style="float: left; margin-left:4%;color:#696868; font-family:Verdana; width: 22%;">
    <h4><a href="contacto.aspx"><img src="img/entrega-a-domicilio1.png" width="184" height="205" alt="Instalacion a domicilio" border="0" /></a></h4>
   <h4>Aprovecha esta promo!!</h4>
   <p>30% de descuento en instalación y primer mes bonificado.   
    <span style="color: #F60; font-weight: bold;">Pedí tu cotización on-line.</span> </p></div>
   <div style="float: left; margin-left:1%; margin-top:12px; background-color:#333; color: #FFF; font-family: Verdana; width:19%; height:250px; text-align: center;">
   <h4>Pedir Presupuesto:</h4>
   <table class="cotizarlista" cellspacing="2" cellpadding="2" style=" text-align:center;  height:200px;">
   
    <tr><td>
      <label for="cotizarlista"></label>

    
      <asp:DropDownList ID="cboPresupuesto" runat="server" Width="80%">
          <asp:ListItem Value="">Seleccione</asp:ListItem>
          <asp:ListItem>Particular(Auto/Moto)</asp:ListItem>
          <asp:ListItem>Logística y Reparto(Auto/Moto/Camioneta/Camión)</asp:ListItem>
          <asp:ListItem>Transporte Público(Taxi/Remis/Colectivos)</asp:ListItem>
          <asp:ListItem>Transporte de Pasajeros Larga Distancia(Micros/Combis)</asp:ListItem>
          <asp:ListItem>Máquinas rurales</asp:ListItem>
          <asp:ListItem>Acceso a Demo</asp:ListItem>
           <asp:ListItem>Consulta General sobre el Servicio</asp:ListItem>
      </asp:DropDownList>
   </td></tr>
     <tr><td>
      <asp:TextBox ID="nombre" runat="server" placeholder="Nombre" runat="server" required="" Width="80%"></asp:TextBox></td></tr>
      <tr><td><asp:TextBox ID="mail" runat="server" placeholder="E-Mail" runat="server" required="" Width="80%"></asp:TextBox></td></tr>
       <tr><td><asp:TextBox ID="tel" runat="server" placeholder="Tel." runat="server" required="" Width="80%"></asp:TextBox></td></tr>
     <tr><td> <asp:Label ID="lblResultado" runat="server" Text="" ForeColor="#A8CF45" Font-Bold="true" Font-Size="10px"></asp:Label></td></tr>
     <tr><td>
     
       <asp:Button ID="Button1" CssClass="button3" runat="server" Text="Enviar" />

         </td></tr>
   </table>
 </div>
     <div style="clear: both"></div>
 </div>
<div id="naranja" style="font-size: 16px; background: #F60; color: #FFFFFF; height:auto;">
 <div style="margin-top: 0; margin-left: 20%; margin-right: 20%; font-size: 16spx; text-align:center; padding:10px"> <p style="font-size:32px">CÓMO FUNCIONA NUESTRO SERVICIO?
  </p>
   <p>Administrá tu flota, inventario y personal y asegurá el máximo rendimiento de tu empresa. </p>
   <div style="width:auto; height:201px; margin-top: 20px; margin-bottom:20px;clear:both;">
     <div id="iconogrande1" style="width: 20%; height: 100%; float: left; margin-left:5%;margin-right:5%; text-align: center;"><a href="servicios.aspx"><img class="agrandar" src="img/como_auto.png" border="0" width="111" height="138" alt="habitaciones"  />
       </a>
       <p><a href="servicios.aspx" style="color:White;">Nuestros Servicios</a></p>
     </div>
     <div id="iconogrande2" style="width: 22%; height: 100%; float: right; margin-right: 40px; text-align: center; font-size: 14px;"><a href="logistica.aspx"><img class="agrandar" alt="mapa" src="img/como_solucion.png" width="110" height="138" border="0" />
       </a>
       <p><a href="logistica.aspx" style="color:White;">Busca la mejor solución<br />
         para tu empresa </a></p>
     </div>
     <div id="iconogrande3" style="width: 20%; height: 100%; float: right; margin-right:15%; text-align: center;"><a href="#"><img class="agrandar" src="img/como_video.png" width="111" height="138" border="0" alt="Reservas"/></a>
       <p>Video Demostración</p>
     </div>
   </div>
 </div>
</div>
 
 
    <h2 style="margin-left:15%; color:#F60;">RASTREO URBANO</h2>
   <div style="float: left; width: 30%; margin-left:12%;color:#696868; font-family:Verdana; ">
  
    <p style="text-align:justify">Le permite monitorear su flota en tiempo real las 24hs del dia los 365 dias del año.
Podrá configurar alarmas para conocer el estado y la ubicación de su vehículo en todo momento y obtener reportes y estadísticas detalladas con indicadores claves.
Reciba notificaciones de alarmas por email y recordatorios de servicios y trámites adminsitrativos.</p>
    <p> También prestamos soluciones de software y hardware  exclusivos para la necesidad de cada empresa.</p>
    <p>Nuestros equipos cuentan con diversos sensores y botón de panico, configurables a las necesidades de cada flota. Te ayudamos y asesoramos en la puesta en marcha del sistema. </p>
    <p style="color:#F60;"><b><a href="Contacto.aspx"><span style="color: #F60; font-weight: bold;">Consultanos y proba la Demo Gratis!</span></a></b></p> <p></p>
     <p style="color:#A8CF45"><b>INSTALACIÓN A DOMICILIO</b></p>
    </div>
   
   
   <div style="float:left;  padding-top:10px;margin-left:3%;margin-right:5%; margin-bottom:2%"><img src="img/panel.gif"  alt="Consola" width="600" height="401"/> </div>
   
   <div style="clear: both"></div>

</div>   
</div>

</asp:Content>