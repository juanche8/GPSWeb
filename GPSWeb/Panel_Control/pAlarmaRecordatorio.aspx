<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaRecordatorio.aspx.vb" Inherits="GPSWeb.pAlarmaRecordatorio" MasterPageFile="~/Panel_Control/SiteMaster.Master" uiCulture="es" culture="es-AR" %>
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

        function verifyCheckboxList(source, arguments) {
            var chkListaMoviles = document.getElementById('');
            var chkLista = chkListaMoviles.getElementsByTagName("input");
            for (var i = 0; i < chkLista.length; i++) {
                if (chkLista[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            arguments.IsValid = false;
        }

        function seleccionarTodos() {
            var chkListaTipoModificaciones = document.getElementById('');
            var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
            for (var i = 0; i < chkLista.length; i++) {

                chkLista[i].checked = true;
            }
        }


        function DeseleccionarTodos() {
            var chkListaTipoModificaciones = document.getElementById('');
            var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
            for (var i = 0; i < chkLista.length; i++) {
                chkLista[i].checked = false;

            }
        }
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdncli_id" runat="server" />   
 <div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Configurar Alertas de Recordatorios</h3>

 </div>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">  
    <ContentTemplate>
 <div style="width:50%; margin:0 0 0 50px;font-size:12px; font-family:Arial;">
      <!--litado vehiculos-->
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:Label ID="Label1" runat="server" Text="Vehiculos:" Font-Bold="true" Font-Size="12px" ForeColor="#D85639"></asp:Label>  
      <br />
      <asp:Panel ID="PanelGrupo" runat="server">
       <asp:Label ID="Label16" runat="server" Text="Seleccione Grupo:" Font-Bold="true" Font-Size="11px"></asp:Label>   
         &nbsp;<asp:DropDownList ID="ddlgrupo" runat="server" AutoPostBack="true" DataTextField="grup_nombre" DataValueField="grup_id">
         </asp:DropDownList>
         &nbsp;&nbsp; 
         <asp:CheckBox ID="chkTodos" runat="server" Text="Ver todos los Moviles" Font-Bold="true" AutoPostBack="true" Font-Size="9px" />
         <br />
     </asp:Panel>
      <asp:Panel ID="PanelMoviles" runat="server" Visible="false"> 
       <div style="height:200px;overflow-y: scroll; font-family:Arial;width:320px; border-color:LightGray; border-width:1px; border-style:solid;"">       
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="5" CellPadding="5"  Font-Names="Arial" Font-Bold="false" Width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="rdnMovil" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
   <div><br />  <asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639">/ Destildar Todos</asp:LinkButton></div>  
    </asp:Panel> <br />     
     </div>  
     </ContentTemplate>
      </asp:UpdatePanel>  
    
          <div id="tabs" style="width:90%; margin:0 0 0 50px;font-size:12px; font-family:Arial;"><br />
                            <ul>
                            <li><a href="#tabs-1">Para una Fecha</a></li>  
                                <li><a href="#tabs-2">Kilometros Acumulados</a></li>
                            </ul>
  <div id="tabs-2">
  <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="5" cellpaging="5">
     <tr><td><span>Descripción para el Recordatorio:</span></td>
     </tr>
     <tr><td>
     <asp:TextBox ID="txtNombrekm" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
     </td>
    </tr>
   
     <tr><td><span>Disparar Primer Alarma a los (kms):</span></td>
     </tr>
     <tr><td>
    <asp:TextBox ID="txtProximaOcurrencia" runat="server" Width="170px" MaxLength="9"></asp:TextBox>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtProximaOcurrencia"></asp:RegularExpressionValidator>
     </td>
    </tr>
     <tr><td><span>Disparar Alarmas restantes cada(kms):</span></td>
     </tr>
     <tr><td>
    <asp:TextBox ID="txtFrecuencia" runat="server" Width="170px" MaxLength="9"></asp:TextBox>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtFrecuencia"></asp:RegularExpressionValidator>
     </td>
    </tr>
     <tr><td><span>Vía Notificación:</span></td>
     </tr>
     <tr><td>
     <asp:CheckBox ID="chkSMS" runat="server" Text="SMS"/>                       
              &nbsp;                       
              <asp:CheckBox ID="chkMail" runat="server" Text="E-Mail" Checked="true"/>
     </td>
    </tr>
  </table>
    
   
  </div>
  <div id="tabs-1">
  <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold;" cellspacing="5" cellpaging="5">
     <tr><td><span>Descripción para el Recordatorio:</span></td>
     </tr>
     <tr><td>
     <asp:TextBox ID="txtNombreFecha" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
     </td>
    </tr>
    <tr><td><span>Frecuencia:</span></td>
     </tr>
     <tr><td>
     <asp:RadioButtonList ID="rdnFrecuencia" runat="server" AutoPostBack="true" Font-Size="10px" CellSpacing="5" CellPadding="5">
                 <asp:ListItem Value="dia" Selected="True">Una Fecha Especifica</asp:ListItem>
                 <asp:ListItem Value="mes">Todos los Meses</asp:ListItem>
                 <asp:ListItem Value="anio">Todos los Años</asp:ListItem>
             </asp:RadioButtonList>  
     </td>
    </tr>
     <asp:Panel ID="PanelDia" runat="server">
      <tr><td><span>Disparar Alarma el Día:</span></td>
     </tr>
     <tr><td>
   <asp:TextBox ID="txtFecha" runat="server" Width="116px" MaxLength="10"></asp:TextBox>
          <ajaxToolkit:calendarextender ID="CalendarExtender4" runat="server" TargetControlID="txtFecha" PopupButtonID="txtFechaHasta" Format="dd/MM/yyyy" CssClass="black"/>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="!" ForeColor="Red" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))" ControlToValidate="txtFecha" Font-Size="13px" Font-Bold="true"></asp:RegularExpressionValidator>
           <br /> <br />
            A las  <asp:DropDownList ID="ddlhora" runat="server" Width="60px" >
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
          
         </asp:DropDownList>Minutos
     </td>
    </tr>
   
    </asp:Panel>
     <asp:Panel ID="PanelMes" runat="server" Visible="false">
         <tr><td><span>Ingrese el Día del Mes en que se dispara la Alarma (por Ej: 15):</span></td></tr>      
        <tr><td><asp:TextBox ID="txtDiaMes" runat="server" Width="80px" MaxLength="2"></asp:TextBox>
                   <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtDiaMes"></asp:RegularExpressionValidator>
     </td></tr>
       </asp:Panel>
         <asp:Panel ID="PanelAnio" runat="server" Visible="false">
          <tr><td><span>Seleccione el Mes y Día en que se dispara la alarma (por Ej: 15):</span></td>
     </tr>
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
        <asp:TextBox ID="txtDia" runat="server" Width="80px" MaxLength="2"></asp:TextBox>
                   <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtDia"></asp:RegularExpressionValidator></td></tr>
            
      
       </asp:Panel>
        <tr><td><span>Notificar por e-mail:</span></td>
     </tr>
     <tr><td> <asp:CheckBox ID="chkMailF" runat="server" Text="Si" Checked="true"/></td></tr>
</table>
   
   
  
  
         
 </div>
 </div>
  <div style="text-align:center;">
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
      <br /><br /><br />
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" CssClass="button2" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" CssClass="button2" Width="160px" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
      </div>   
 </div>
</asp:Content>
