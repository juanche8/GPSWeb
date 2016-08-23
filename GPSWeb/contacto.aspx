<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="contacto.aspx.vb" Inherits="GPSWeb.contacto" MasterPageFile="~/Site3.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/form.css" rel="stylesheet" type="text/css" />

  
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="box" style="background-color: #FFF; clear: both; font-family: Helvetica, sans-serif, 'Gill Sans MT'; color: #B84237;">

 <div id="blanco" style="height:auto; width:auto;">
   <div style="clear: both">
     
   </div>
<div style="width:80%; margin-right:0px;margin-left:12%; font-size:14px; "> 
  <h2>CONTACTO</h2>
  <p>&nbsp;</p>
   <p style="font-size:18px;">Complete el formulario con sus datos, y su consulta sobre nuestros servicios, lo contactaremos a la brevedad.</p>
   <div style="margin-left:5%"><div style="width:551px; float:left; margin-top:30px"><img src="img/key.png" width="550" height="505" alt="" /></div>
  <div style="width:400px; float:left; margin-left:40px; margin-bottom:40px">
    <asp:Label ID="lblResultado" runat="server" Text="" ForeColor="#A8CF45" Font-Bold="true"></asp:Label>
<label for="nombre">Nombre:</label>
 
 <asp:TextBox ID="nombre" runat="server" placeholder="Nombre y Apellido" runat="server" required=""></asp:TextBox>
 <label for="email">Email:</label>
  <asp:TextBox ID="email" runat="server" placeholder="ejemplo@correo.com" runat="server" required=""></asp:TextBox>
  <label for="email">Telefono:</label>
  <asp:TextBox ID="telefono" runat="server" placeholder="0114567895" runat="server" required=""></asp:TextBox>
  <label for="email">Pedir Presupuesto para:</label>
 
      <asp:DropDownList ID="cboPresupuesto" runat="server" Width="400px" Height="45px" Font-Size="15px">
          <asp:ListItem Value="">Seleccione Servicio</asp:ListItem>
          <asp:ListItem>Particular(Auto/Moto)</asp:ListItem>
          <asp:ListItem>Logística y Reparto(Auto/Moto/Camioneta/Camión)</asp:ListItem>
          <asp:ListItem>Transporte Público(Taxi/Remis/Colectivos)</asp:ListItem>
          <asp:ListItem>Transporte de Pasajeros Larga Distancia(Micros/Combis)</asp:ListItem>
          <asp:ListItem>Máquinas rurales</asp:ListItem>
          <asp:ListItem>Acceso a Demo</asp:ListItem>
           <asp:ListItem>Consulta General sobre el Servicio</asp:ListItem>
      </asp:DropDownList>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cboPresupuesto" InitialValue=""
     runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
 <label for="mensaje">Consulta:</label>
      <asp:TextBox ID="txtmensaje" runat="server" TextMode="MultiLine" placeholder="Mensaje" required=""></asp:TextBox>

      <asp:Button ID="Button1" runat="server" Text="Enviar" CssClass="menuButton" Height="50px"  Width="404px"/>

    
</div>
</div>
  <div style="float: left; margin-right:0px; margin-left:12%; margin-bottom:90px"></div>
  <p>&nbsp;</p>
  <div></div>
 
  <div style="clear: both"></div>
 </div>
<div style="clear: both"></div>

</div>
</div>
</asp:Content>