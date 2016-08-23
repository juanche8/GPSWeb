<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaRecordatorioKm.aspx.vb" Inherits="GPSWeb.pAlarmaRecordatorioKm" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

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
    $(function() {
        $("#tabs").tabs();
    });

    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <asp:HiddenField ID="hdncli_id" runat="server" />    
      <asp:HiddenField ID="hdnreck_id" runat="server" Value="0" />
        <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
<div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Modificar Alerta de Recordatorio Por Kms</h3>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
 </div>
    
   
 <div style="width:30%; margin:0 0 0 50px;">
     <asp:Label ID="Label1" runat="server" Text="Vehiculo:" Font-Size="14px" Font-Bold="true"></asp:Label> 
       &nbsp;<asp:Label ID="lblMovil" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label>     
         <br />      
      </div>  
          <div id="tabs" style="width:60%; margin:0 0 0 50px;">
              <br />
                            <ul>     <li><a href="#tabs-2">Por Kilometros Acumulados</a></li>
                            </ul>
  <div id="tabs-2">
   <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="8" cellpaging="5">
     <tr><td><span>Descripción para el Recordatorio:</span></td>
     </tr>
     <tr><td><asp:TextBox ID="txtDescripcion" runat="server" Width="320px" MaxLength="50"></asp:TextBox>
 <asp:RequiredFieldValidator ID="requiredfieldvalidatorName" runat="server" ControlToValidate="txtDescripcion" Display="Dynamic" ErrorMessage="Ingrese la Descripción." SkinID="Validator"></asp:RequiredFieldValidator></td></tr>
 
 <tr><td><span>Disparar Primer Alarma a los (kms):</span></td>
     </tr>
     <tr><td><asp:TextBox ID="txtPrimerReporte" runat="server" Width="170px" MaxLength="10" ReadOnly="true"></asp:TextBox>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtPrimerReporte"></asp:RegularExpressionValidator>  </td></tr>
          <tr><td><span>Disparar Alarmas restantes cada(kms):</span></td>
     </tr>
     <tr><td><asp:TextBox ID="txtFrecuencia" runat="server" Width="170px" MaxLength="10" ReadOnly="true"></asp:TextBox>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtFrecuencia"></asp:RegularExpressionValidator>  </td></tr>
 <tr><td><span>Notificar por e-mail:</span></td>
     </tr>
     <tr><td><asp:CheckBox ID="chkMail" runat="server" Text="Si" Checked="true"/></td></tr>
</table>
   
  <div style="text-align:center;">
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" CssClass="button2" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" CssClass="button2" Width="160px" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" />
      </div>   
  </div>

         
 </div>
 </div>
</asp:Content>