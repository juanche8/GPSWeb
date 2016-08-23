<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaRecordatorioFecha.aspx.vb" Inherits="GPSWeb.pAlarmaRecordatorioFecha" MasterPageFile="~/Panel_Control/SiteMaster.Master" uiCulture="es" culture="es-AR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
    <!-- The jQuery UI theme extension jqGrid needs -->
    <link rel="stylesheet" type="text/css" media="screen" href="../css/azul/ui.jqgrid.min.css" />  
     <!-- The localization file we need, English in this case -->
    <script src="../scripts/trirand/i18n/grid.locale-sp.min.js" type="text/javascript"></script>
    <!-- The jqGrid client-side javascript -->
    <script src="../scripts/trirand/jquery.jqGrid.min.js" type="text/javascript"></script>
       <script src="../scripts/ui/jquery.ui.tabs.min.js" type="text/javascript"></script>
<script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });

       
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
   <asp:HiddenField ID="hdncli_id" runat="server" />    
      <asp:HiddenField ID="hdnrecf_id" runat="server" Value="0" />
        <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
       
<div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Modificar Alarma de Recordatorio por Fechas</h3>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
 </div>
    
   
 <div style="width:30%; margin:0 0 0 50px;font-size:12px; font-family:Arial;">
      <asp:Label ID="Label1" runat="server" Text="Vehiculo:" Font-Size="14px" Font-Bold="true"></asp:Label> 
       &nbsp;<asp:Label ID="lblMovil" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label>     
             <br />
             </div> 
             <div id="tabs" style="width:60%; margin:0 0 0 50px;font-size:12px; font-family:Arial;">
              <br />
             <ul>
                            <li><a href="#tabs-1">Para una Fecha</a></li>   </ul>
  
  <div id="tabs-1">
   <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="8" cellpaging="5">
     <tr><td><span>Descripción para el Recordatorio:</span></td>
     </tr>
     <tr><td><asp:TextBox ID="txtDescripcion" runat="server" Width="350px" MaxLength="50"></asp:TextBox></td></tr>
       <tr><td><span>Frecuencia:</span></td>
     </tr>
     <tr><td>  <asp:RadioButtonList ID="rdnFrecuencia" runat="server" AutoPostBack="true" Font-Names="Arial">
                 <asp:ListItem Value="dia" Selected="True">Una Fecha Especifica</asp:ListItem>
                 <asp:ListItem Value="mes">Todos los Meses</asp:ListItem>
                 <asp:ListItem Value="anio">Todos los Años</asp:ListItem>
             </asp:RadioButtonList></td></tr>
              <asp:Panel ID="PanelDia" runat="server">
     <tr><td><span>Disparar Alarma el Día:</span></td>
     </tr>
     <tr><td><asp:TextBox ID="txtFecha" runat="server" Width="116px" MaxLength="10"></asp:TextBox>
            <ajaxToolkit:calendarextender ID="CalendarExtender4" runat="server" TargetControlID="txtFecha" PopupButtonID="txtFechaHasta" Format="dd/MM/yyyy" CssClass="black"/>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="!" ForeColor="Red" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))" ControlToValidate="txtFecha" Font-Size="13px" Font-Bold="true"></asp:RegularExpressionValidator>
    </td></tr>
       <tr><td>   A las  <asp:DropDownList ID="ddlhora" runat="server" Width="60px">
             <asp:ListItem Value="00">00</asp:ListItem>  
            <asp:ListItem Value="1">01</asp:ListItem> 
            <asp:ListItem Value="2">02</asp:ListItem>
            <asp:ListItem Value="3">03</asp:ListItem>
            <asp:ListItem Value="4">04</asp:ListItem>
            <asp:ListItem Value="5">05</asp:ListItem> 
            <asp:ListItem Value="6">06</asp:ListItem>
            <asp:ListItem Value="7">07</asp:ListItem>
            <asp:ListItem Value="8">08</asp:ListItem>
            <asp:ListItem Value="9">09</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="11">11</asp:ListItem>      
             <asp:ListItem Value="12">12</asp:ListItem>
        <asp:ListItem Value="13">13</asp:ListItem>
        <asp:ListItem Value="14">14</asp:ListItem>
        <asp:ListItem Value="15">15</asp:ListItem>
        <asp:ListItem Value="16">16</asp:ListItem>
        <asp:ListItem Value="17">17</asp:ListItem>
        <asp:ListItem Value="18">18</asp:ListItem>
        <asp:ListItem Value="19">19</asp:ListItem>
        <asp:ListItem Value="20">20</asp:ListItem>
        <asp:ListItem Value="21">21</asp:ListItem>
        <asp:ListItem Value="22">22</asp:ListItem>  
        <asp:ListItem Value="23">23</asp:ListItem>
                 
         </asp:DropDownList> Hs
          <asp:DropDownList ID="ddlmin" runat="server" Width="60px">        
             <asp:ListItem Value="00">00</asp:ListItem> 
              <asp:ListItem Value="05">05</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="15">15</asp:ListItem>
            <asp:ListItem Value="20">20</asp:ListItem>
            <asp:ListItem Value="25">25</asp:ListItem> 
            <asp:ListItem Value="30">30</asp:ListItem>
            <asp:ListItem Value="35">35</asp:ListItem>
            <asp:ListItem Value="40">40</asp:ListItem>
            <asp:ListItem Value="45">45</asp:ListItem>
            <asp:ListItem Value="50">50</asp:ListItem>
            <asp:ListItem Value="55">55</asp:ListItem>   
            <asp:ListItem Value="55">59</asp:ListItem>      
          
         </asp:DropDownList>Minutos</td>
         </tr>
 </asp:Panel>
 <asp:Panel ID="PanelMes" runat="server" Visible="false">
  <tr><td><span>Ingrese el Día del Mes en que se dispara la Alarma (por Ej: El 15 de todos los meses):</span></td>
     </tr>
    
     <tr><td><asp:TextBox ID="txtDiaMes" runat="server" Width="110px" MaxLength="100"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtDiaMes"></asp:RegularExpressionValidator> </td></tr>
     </asp:Panel>
     <asp:Panel ID="PanelAnio" runat="server" Visible="false">
     <tr><td><span>Seleccione el Mes y Día en que se dispara la alarma:</span></td></tr>
     <tr><td> <asp:DropDownList ID="ddlMes" runat="server" Width="150px">        
             <asp:ListItem Value="1">Enero</asp:ListItem>
        <asp:ListItem Value="2">Febrero</asp:ListItem>
        <asp:ListItem Value="3">Marzo</asp:ListItem>
        <asp:ListItem Value="4">Abril</asp:ListItem>
        <asp:ListItem Value="5">Mayo</asp:ListItem>
        <asp:ListItem Value="6">Junio</asp:ListItem>
        <asp:ListItem Value="7">Julio</asp:ListItem>
        <asp:ListItem Value="8">Agosto</asp:ListItem>
        <asp:ListItem Value="9">Septiembre</asp:ListItem>
        <asp:ListItem Value="10">Octubre</asp:ListItem>
        <asp:ListItem Value="11">Noviembre</asp:ListItem>
        <asp:ListItem Value="12">Diciembre</asp:ListItem>
              
         </asp:DropDownList>
             &nbsp;
        <asp:TextBox ID="txtDia" runat="server" Width="80px" MaxLength="9"></asp:TextBox>
 <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtDia"></asp:RegularExpressionValidator>  </td></tr>
</asp:Panel>
<tr><td><span>Notificar por e-mail:</span></td></tr>
<tr><td><asp:CheckBox ID="chkMail" runat="server" Text="Si" Checked="true"/></td></tr>

       </table>
   
  
  </div>
      <div style="text-align:center;">
      <br /><br /><br /><br /> 
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" CssClass="button2" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" CssClass="button2" Width="160px" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" />
      </div>       
 </div>
 </div>
</asp:Content>
