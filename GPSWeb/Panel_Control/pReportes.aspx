<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pReportes.aspx.vb" Inherits="GPSWeb.pReportes1" Culture="es-ES" UICulture="es-ES" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    
     <link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
    <!-- The jQuery UI theme extension jqGrid needs -->
    <link rel="stylesheet" type="text/css" media="screen" href="../css/azul/ui.jqgrid.min.css" />  
     <!-- The localization file we need, English in this case -->
    <script src="../scripts/trirand/i18n/grid.locale-sp.min.js" type="text/javascript"></script>
    <!-- The jqGrid client-side javascript -->
    <script src="../scripts/trirand/jquery.jqGrid.min.js" type="text/javascript"></script>
       <script src="../scripts/ui/jquery.ui.tabs.min.js" type="text/javascript"></script>

       <style type="text/css">
  .a:visited 
    {
    color: White;
    }
   .a:active 
    {
    color: Red;
    }
.buttonClass:active
{   
    background-color: White;
}

.buttonClass:hover
{   
    background-color: White;
}

       </style>
      <script type="text/javascript">

         

          $(function () {
              $("#tabs").tabs();
          });
           
           
           function SelectRadioButton(regexPattern, selectedRadioButton) {
               regex = new RegExp(regexPattern);
               for (i = 0; i < document.forms[0].elements.length; i++) {
                   element = document.forms[0].elements[i];
                   if (element.type == 'radio' && regex.test(element.name)) {
                       element.checked = false;
                   }
               }
               selectedRadioButton.checked = true;
           }
           function mostrar(div,div2) {

               if (document.getElementById(div).style.display == "block") {
                   document.getElementById(div).style.display = "none";
                   document.getElementById(div2).style.display = "block";
               }
               else {
                   document.getElementById(div2).style.display = "none";
                   document.getElementById(div).style.display = "block";
               }
           }

           function seleccionarTodos() {

               var checkboxes = document.getElementById("aspnetForm").ctl00_ContentPlaceHolder1_DataListVehiculos1_ctl08_rdnMovil;
               alert(checkboxes.length);
               if (checkboxes != null)
                   for (var x = 0; x < checkboxes.length; x++) {
                   checkboxes[x].checked = true;
               }
           }

           function DeseleccionarTodos() {
               var checkboxes = document.getElementById("aspnetForm").rdnMovil;
               if (checkboxes != null)
                   for (var x = 0; x < checkboxes.length; x++) {
                   checkboxes[x].checked = false;

               }
           }

          
         
</script>

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    
      <div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Generar Reportes de Recorridos</h3>
  
 </div>
    
      <asp:Panel ID="PanelPatente" runat="server" Visible="false">
     <asp:Label ID="Label7" runat="server" Text="Para el Movil Patente:" Font-Bold="true" Font-Size="14px"></asp:Label>
      <asp:Label ID="lblPatente" runat="server" Text="" Font-Size="14px"></asp:Label>
          <br />
    </asp:Panel>
   
    <div id="tabs" style="width:70%">
    <br />
                            <ul>
                            <li><a href="#tabs-1">Reporte de Hoy</a></li>  
                                <li><a href="#tabs-2">Reporte Customizado</a></li>
                             
                            </ul>
  <div id="tabs-1">
   <asp:UpdatePanel ID="UpdatePanel4" runat="server">  
    <ContentTemplate>
      
      <asp:Panel ID="PanelMoviles" runat="server">
      <asp:Label ID="Label2" runat="server" Text="Listar los Recorridos del Día." ForeColor="#373435" Font-Bold="true" Font-Size="14px"></asp:Label>
       <asp:Label ID="lblFechas" runat="server" Text="" ForeColor="#373435" Font-Bold="true" Font-Size="14px"></asp:Label>
          
      
      <br />
        <asp:Label ID="Label4" runat="server" Text="Seleccione Movil" ForeColor="#373435" Font-Bold="true" Font-Size="12px"></asp:Label>       
         <br /> <br /> 
           <div style="height:200px;overflow-y: scroll; font-family:Arial; width:320px; border-color:LightGray; border-width:1px; border-style:solid;">       
        <asp:DataList ID="DataListVehiculos1" runat="server" DataKeyField="veh_id" CellSpacing="8" CellPadding="5" OnItemDataBound="Vehiculo_itemDataBound" Font-Names="Arial"  Width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="rdnMovil" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
      <div><br />
         <asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639">/ Destildar Todos</asp:LinkButton></div>
       
           </asp:Panel>   
           
       <br />
       <asp:Button ID="btnReporteRutinaCompleto" runat="server" Text="Ver Listado Completo" CssClass="button2" Width="150px" Height="40px" Font-Size="14px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" />
       &nbsp;&nbsp;
       <asp:Button ID="btnReporteRutinaKms" runat="server" Text="Ver Kms Recorridos" CssClass="button2" Width="140px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
       &nbsp;&nbsp;
       <asp:Button ID="btnReporteAlertas" runat="server" Text="Ver Alarmas" CssClass="button2" Width="130px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
         &nbsp;
         <asp:Button ID="btnEstadisticas" runat="server" Text="Estadistícas" CssClass="button2" Width="130px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
       <br />
       <br />
       </ContentTemplate>
    </asp:UpdatePanel>
  </div>

  <div id="tabs-2">
    
   
              <asp:Panel ID="PanelCampos" runat="server">
                <asp:Label ID="Label6" runat="server" Text="Campos del Reporte" Font-Bold="true"></asp:Label>
           <br />
           <br />        
           <table style="width:70%; font-family:Arial; vertical-align:middle; font-size:11px; font-weight:bold;" cellspacing="8" cellpaging="5">
           <tr>
           <td> <asp:CheckBox ID="chkPatente" runat="server" Text=" Patente"  Checked="true"/></td>
           <td><asp:CheckBox ID="ChkKms" runat="server" Text=" Suma Kms" Checked="true"/></td>
           </tr>
           <tr>
           <td>   <asp:CheckBox ID="chkFecha" runat="server" Text=" Fecha" Checked="true" /></td>
           <td><asp:CheckBox ID="ChkAlertas" runat="server" Text=" Alertas" Checked="true"/></td>
           </tr>
           <tr>
           <td> <asp:CheckBox ID="chkHora" runat="server" Text=" Hora" Checked="true"/></td>
           <td><asp:CheckBox ID="chkEncendido" runat="server" Text=" Encendido" Checked="true"/></td>
           </tr>
           <tr>
           <td>  <asp:CheckBox ID="chkDireccion" runat="server" Text=" Direccion" Checked="true"/></td>
           <td><asp:CheckBox ID="chkOcupado" runat="server" Text=" Ocupado" Checked="true"/></td>
           </tr>
            <tr>
           <td>   <asp:CheckBox ID="chkLocalidad" runat="server" Text=" Localidad" Checked="true"/></td>
           <td><asp:CheckBox ID="chkRPM" runat="server" Text=" RPM" Checked="true"/></td>
          
           </tr>
            <tr>
           <td>  <asp:CheckBox ID="chkProvincia" runat="server" Text=" Provincia" Checked="true"/></td>
           <td><asp:CheckBox ID="chkTemp" runat="server" Text=" Temperatura" Checked="true"/></td>
           </tr>
            <tr>
           <td>   <asp:CheckBox ID="chkVelocidad" runat="server" Text=" Velocidad" Checked="true"/></td>
           <td>   <asp:CheckBox ID="chkbateria" runat="server" Text=" Bateria" Checked="true"/></td>
           </tr>
           
            <tr>
           <td>   <asp:CheckBox ID="chkEvento" runat="server" Text=" Evento" Checked="true"/></td>
           <td>   </td>
           </tr>
           </table>
           <hr />
             
           </asp:Panel>
         
               
             <asp:Panel ID="PanelCustomizado" runat="server">
           
             <asp:Label ID="Label1" runat="server" Text="Vehiculos" Font-Bold="true"></asp:Label>
               
                
     <div style="height:140px;overflow-y: scroll; width:60%;">
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="8" CellPadding="5" Font-Names="Arial">
     <ItemTemplate>
         <asp:RadioButton ID="rdnMovil" runat="server" Font-Names="Arial"
             Text="" GroupName="radiomovil" onclick="SelectRadioButton('radiomovil$',this)" /> 
              <img src="../images/iconos_movil/autito_gris.png" alt="" />
               <asp:Label ID="Label12" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>-
     <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>
     
     </ItemTemplate>
     </asp:DataList>
     </div>
      <hr />
    
   <asp:Label ID="Label5" runat="server" Text="Rango de Fechas (máximo 7 días):" Font-Bold="true" Font-Names="Arial"></asp:Label>
                
   <br />
   <table style="width:50%; vertical-align:middle; font-size:10px; font-weight:bold; font-family:Arial" >
     <tr style="height:30px;"><td><span> Día Desde:</span></td>
     <td>
      <asp:TextBox ID="txtFechaDesde" runat="server" Width="80px"></asp:TextBox> 
               <ajaxToolkit:calendarextender ID="CalendarExtender3" runat="server" CssClass="black" 
         TargetControlID="txtFechaDesde" PopupButtonID="txtFechaDesde" Format="dd/MM/yyyy" />  
          <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="!" ForeColor="Red" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))" ControlToValidate="txtFechaDesde" Font-Size="13px" Font-Bold="true"></asp:RegularExpressionValidator>
     </td>
     <td><span> Hora:</span></td>
     <td> 
     <asp:DropDownList ID="ddlhoraDesde" runat="server" Width="72px">
         <asp:ListItem Value="">Hora</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList>
         </td>
        <td>
        <asp:DropDownList ID="ddlMinDesde" runat="server" Width="72px">
         <asp:ListItem Value="">Min</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>    
             <asp:ListItem>59</asp:ListItem>        
        </asp:DropDownList>

        </td>
     </tr>
     <tr style="height:30px;">
     <td><span> Día Hasta:</span></td>
     <td>
   <asp:TextBox ID="txtFechaHasta" runat="server" Width="80px"></asp:TextBox>
                <ajaxToolkit:calendarextender ID="CalendarExtender4" runat="server" TargetControlID="txtFechaHasta" PopupButtonID="txtFechaHasta" Format="dd/MM/yyyy" CssClass="black"/>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="!" ForeColor="Red" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))" ControlToValidate="txtFechaHasta" Font-Size="13px" Font-Bold="true"></asp:RegularExpressionValidator>
     </td>
     <td ><span> Hora:</span></td>
     <td> 
    <asp:DropDownList ID="ddlHoraHasta" runat="server" Width="72px">
         <asp:ListItem Value="">Hora</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList> 
        </td>
        <td>
        <asp:DropDownList ID="ddlMinHasta" runat="server" Width="72px">
         <asp:ListItem Value="">Min</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>  
            <asp:ListItem>59</asp:ListItem>           
        </asp:DropDownList>

        </td>
     </tr>
    </table>
 
       </asp:Panel>
                
               <br />
     <div style="width:90%; text-align:left;">
       <asp:Button ID="btnListadoCustomAll" runat="server" Text="Ver Listado Completo" CssClass="button2" Width="150px" Height="40px" Font-Size="14px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" OnClientClick="javascript:proces.style.display='';"/>
       &nbsp;&nbsp;
       <asp:Button ID="btnKmsCustom" runat="server" Text="Ver Kms Recorridos" CssClass="button2" Width="140px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
       &nbsp;&nbsp;
       <asp:Button ID="btnAlertasCustom" runat="server" Text="Ver Alarmas" CssClass="button2" Width="130px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
        &nbsp;
        <asp:Button ID="btnEstadisticasCustom" runat="server" Text="Estadisticas" CssClass="button2" Width="130px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
      </div>
     
    
   
  </div>

 
  </div>
  <div id="proces2" style="width:30%; float:right;position:absolute; left:605px; top:295px; display:none;">
                <img src="../images/FhHRx.gif" alt="procesando" />
                 <span style="font-size:14px;color:Red; ">Pocesando, aguarde...</span></div>
  <div id="proces" style="width:30%; float:right;position:absolute; left:605px; top:595px; display:none;">
                <img src="../images/FhHRx.gif" alt="procesando" />
                 <span style="font-size:14px;color:Red; ">Pocesando, aguarde...</span></div>
   <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="12px"></asp:Label>
    <br />
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div style="width:30%; float:right;position:absolute; left:705px; top:500px;">
                <img src="../images/FhHRx.gif" alt="procesando" />
                 <span style="font-size:14px;color:Red; ">Pocesando, aguarde...</span></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
   <Triggers>
   <asp:PostBackTrigger  ControlID="btRecExport"/>
   <asp:PostBackTrigger  ControlID="btAlarmasExport"/>
   <asp:PostBackTrigger  ControlID="btKmsExport"/>
   </Triggers>
    <ContentTemplate>
    
 <div style="width:100%; height:100%;">
     <asp:Panel ID="PanelRutinaCompleto" runat="server" Visible="false">
<div style="width:50%; margin-left:300px; vertical-align:middle;">      
<asp:Label ID="Label8" runat="server" Text="Informe Historico de Recorridos" Font-Bold="true" Font-Size="16px" ForeColor="#D85639" ></asp:Label>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
    <asp:ImageButton ID="btRecExport" runat="server" ImageUrl="~/images/excel.png" ToolTip="Exportar Listado a Excel" />

</div>
<br />
  <asp:GridView ID="gridRutinaCompleto" DataKeyField="patente" runat="server" 
             AutoGenerateColumns="False" Width="99%" HorizontalAlign="Left" 
                  Font-Size="12px" AllowSorting="true"                       
 HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" OnSorting="gridRutinaCompleto_SortRecords">
        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField HeaderText="Patente" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="patente" SortExpression="patente" >
                                <HeaderStyle Width="5%" Font-Bold="False" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Fecha" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="center" DataField="fecha" SortExpression="fecha" >
                                  <HeaderStyle Width="5%" Font-Bold="False" />
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Hora" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="center" DataField="hora" SortExpression="hora">
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Dirección" HeaderStyle-Width="13%" ItemStyle-HorizontalAlign="Left" DataField="nombre_via" SortExpression="nombre_via">
                                   <HeaderStyle Width="13%" Font-Bold="False"/>
                                   <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Localidad" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Left"  DataField="localidad" SortExpression="localidad">
                                 <HeaderStyle Width="8%" Font-Bold="False"/>
                                 <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Provincia" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="provincia" SortExpression="provincia">
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Velocidad" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="velocidad" SortExpression="velocidad">
                                   <HeaderStyle Width="5%" Font-Bold="False" />
                                   <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Kms. Rec.Parcial" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="total_kilometros" SortExpression="total_kilometros">
                                 <HeaderStyle Width="5%" Font-Bold="False"/>
                                 <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Alarmas" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="cant_alarmas" SortExpression="cant_alarmas">
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Encendido" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="encendido" SortExpression="encendido" >
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Ocupado" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="ocupado" SortExpression="ocupado">
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="RPM" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="RPM" SortExpression="RPM" >
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Temp." HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="TEMP" SortExpression="TEMP" >
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Bat." HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="BAT" SortExpression="BAT" >
                                  <HeaderStyle Width="5%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Evento" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="EVENTO" SortExpression="EVENTO" >
                                  <HeaderStyle Width="10%" Font-Bold="False"/>
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                                            </Columns>
                      
                         <FooterStyle BackColor="#CCCCCC" />
<HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
<RowStyle Font-Size="9pt"></RowStyle>
        <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
                        <EmptyDataTemplate>No se Encontraron Datos para la Busqueda Realizada</EmptyDataTemplate>
                      
                     </asp:GridView>
                      <br />
                      <div style="background-color:#999999; font-size:12px; font-weight:bold; height:30px; width:99%; float:left; color:White; font-family:Arial; text-align:center;" ><asp:Repeater ID="rptPager" runat="server" >
<ItemTemplate>
    <asp:LinkButton ID="lnkPage" CssClass="buttonClass" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed"></asp:LinkButton>
</ItemTemplate>
</asp:Repeater></div>



                    </asp:Panel>  
        
     <asp:Panel ID="PanelRutinaKmsRecorridos" runat="server" Visible="false">
        <div style="width:50%; margin-left:480px;">      
            <asp:Label ID="Label11" runat="server" Text="Informe Historico de Kms Totales Recorridos" Font-Bold="true" Font-Size="16px" ForeColor="#D85639" ></asp:Label>   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
              <asp:ImageButton ID="btKmsExport" runat="server" ImageUrl="~/images/excel.png" 
        ToolTip="Exportar Listado a Excel" />


        </div>
        <br />
            <asp:GridView ID="GridKmsRecorridos" DataKeyField="patente" 
             runat="server" AutoGenerateColumns="False" Width="30%" HorizontalAlign="Center" 
                  Font-Size="12px" ShowFooter="True" OnPageIndexChanging="GridKmsRecorridos_PageIndexChanging" 
             OnRowDataBound="KmsRecorridos_RowDataBound" AllowPaging="True" PageSize="20" 
            AllowSorting="true" OnSorting="gridKms_SortRecords"
 HeaderStyle-Font-Size="10" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:BoundField HeaderText="Patente" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="patente" >
                                <HeaderStyle Width="10%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Fecha" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="center" DataField="fecha" >
                                  <HeaderStyle Width="10%" />
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Kms Recorridos" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" DataField="kms" >
                                   <HeaderStyle Width="10%" />
                                   <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                              </Columns>
                       <FooterStyle BackColor="#CCCCCC" />
<HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
<RowStyle Font-Size="9pt"></RowStyle>
        <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
                      <EmptyDataTemplate>No se Encontraron Datos para la Busqueda Realizada</EmptyDataTemplate>
                     </asp:GridView> 
                    </asp:Panel>  
    <asp:Panel ID="PanelRutinaAlarmas" runat="server" Visible="false">
<div style="width:50%; margin-left:400px;">      
<asp:Label ID="Label9" runat="server" Text="Informe de Alarmas Reportadas" Font-Bold="true" Font-Size="16px" ForeColor="#D85639" ></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
              <asp:ImageButton ID="btAlarmasExport" runat="server" ImageUrl="~/images/excel.png" 
        ToolTip="Exportar Listado a Excel" />
</div>
<br />
  <asp:GridView ID="GridAlarmasRutina" DataKeyField="patente" runat="server" 
            AutoGenerateColumns="False" Width="95%" HorizontalAlign="Left" 
                    Font-Size="12px" AllowPaging="True" PageSize="20" OnPageIndexChanging="GridAlarmasRutina_PageIndexChanging"
 HeaderStyle-Font-Size="10" HeaderStyle-Font-Bold="False" BackColor="White"  AllowSorting="true" OnSorting="gridAlarmas_SortRecords"
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField HeaderText="Patente" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="patente" SortExpression="patente" >
                                <HeaderStyle Width="5%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Fecha" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="center" DataField="fecha" SortExpression="fecha">
                                  <HeaderStyle Width="5%" />
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Hora" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="center" DataField="hora" SortExpression="hora">
                                  <HeaderStyle Width="5%" />
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Duración" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="center" DataField="duracion" SortExpression="duracion">
                                  <HeaderStyle Width="10%" />
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Categoria" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" DataField="categoria" SortExpression="categoria">
                                   <HeaderStyle Width="15%" />
                                   <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Alarma" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="alarma" SortExpression="alarma">
                                 <HeaderStyle Width="15%" />
                                 <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Valor Reportado" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="center"  DataField="valor" SortExpression="valor">
                                  <HeaderStyle Width="5%" />
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Direccion" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="direccion" SortExpression="direccion">
                                   <HeaderStyle Width="10%" />
                                   <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Localidad" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="localidad" SortExpression="localidad">
                                 <HeaderStyle Width="10%" />
                                 <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Provincia" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="provincia" SortExpression="provincia">
                                  <HeaderStyle Width="10%" />
                                  <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                                            </Columns>
                       <FooterStyle BackColor="#CCCCCC" />
<HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
        <PagerStyle HorizontalAlign="Center" />
<RowStyle Font-Size="9pt"></RowStyle>
        <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
                      <EmptyDataTemplate>No se Encontraron Datos para la Busqueda Realizada</EmptyDataTemplate>
                      </asp:GridView> 
                    </asp:Panel>
                    <br />
                    <br />
                       
   
</div>      
  <asp:HiddenField ID="hdnOrden" runat="server" Value=" Fecha Desc"/>
    <asp:HiddenField ID="hdnPagina" runat="server" Value="1"/>
    <asp:HiddenField ID="hdnFechaDesde" runat="server" Value=""/>
     <asp:HiddenField ID="hdnFechaHasta" runat="server" Value="" /> 
      <asp:HiddenField ID="hdnFechaDesde1" runat="server" Value=""/>
     <asp:HiddenField ID="hdnFechaHasta1" runat="server" Value="" />    
      <asp:HiddenField ID="hdnmoviles" runat="server" Value="" />
      <asp:HiddenField ID="hdnCampos" runat="server" Value="" />
       <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" />
      <asp:HiddenField ID="hdnTab" runat="server" Value="#tabs-1" />
      <asp:HiddenField ID="hdntabDisable" runat="server" Value="1" />
 </ContentTemplate>
    </asp:UpdatePanel>   
     <br />
                    <br />    
  </div>
   <asp:Literal ID="LiteralDisabled" runat="server"></asp:Literal>


  <script type="text/javascript">
      
     
      $(function () {
          $('#tabs').tabs('select', document.getElementById("<%= hdnTab.ClientID %>").value);         
           
      });
 </script>
 <br />
<br />

</asp:Content>