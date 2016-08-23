 <%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pEstaditicas.aspx.vb" Inherits="GPSWeb.pEstaditicas" Culture="es-ES" UICulture="es-ES"  MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

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
<script type="text/javascript" src="https://www.google.com/jsapi"></script>

       <style>
  .A:visited 
    {
    color: White;
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
          function mostrar(div, div2) {

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

        
         // google.load('visualization', '1.1', { packages: ['corechart'] });  
         // google.setOnLoadCallback(LineVelocidadChart);

      /*    function BarVelocidadChart() {

              <%=_dataVelocidad %>
              var data = google.visualization.arrayToDataTable([  
              ['Year', 'Auto1', 'Auto2', 'Auto3'],
          ['2004', 1000, 400, 200],
          ['2005', 1170, 460, 500],
          ['2006', 660, 1120, 600],
          ['2007', 1030, 540, 450]]);

              var options = {
                  title: 'Velociades Maximas',
                  hAxis: { title: 'Periodo', titleTextStyle: { color: 'red'} }
              };

              var chart = new google.visualization.ColumnChart(document.getElementById('barCharVelocidad'));

              chart.draw(data, options);

          }


          function LineVelocidadChart() {
              var data = google.visualization.arrayToDataTable([
          ['Year', 'Auto1'],
          ['2004', 1000],
          ['2005', 1170],
          ['2006', 660],
          ['2007', 1030]
        ]);

              var options = {
                  title: 'Velociades Maximas'
              };

              var chart = new google.visualization.LineChart(document.getElementById('barCharVelocidad'));

              chart.draw(data, options);
          }*/

         
</script>

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    
      <div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Generar Estadisticas</h3>
  
 </div>
    
      <asp:Panel ID="PanelPatente" runat="server" Visible="false">
     <asp:Label ID="Label7" runat="server" Text="Para el Movil Patente:" Font-Bold="true" Font-Size="14px"></asp:Label>
      <asp:Label ID="lblPatente" runat="server" Text="" Font-Size="14px"></asp:Label>
          <br />
    </asp:Panel>
   
    <div id="tabs" style="width:70%">
    <br />
                            <ul>
                            <li><a href="#tabs-1">Datos para consultar Estadisticas</a></li>  
                          
                            </ul>
  <div id="tabs-1">
 
          <asp:HiddenField ID="hdnFechaDesde" runat="server" Value=""/>
     <asp:HiddenField ID="hdnFechaHasta" runat="server" Value="" />    
      <asp:HiddenField ID="hdnmoviles" runat="server" Value="" />
      <asp:HiddenField ID="hdnCampos" runat="server" Value="" />
       <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" /><asp:HiddenField ID="hdnOrigen" runat="server" Value ="" />
       <asp:HiddenField ID="hdnOcultarProporcion" runat="server" Value="0" />
      <asp:HiddenField ID="hdnTab" runat="server" Value="#tabs-1" />
      <asp:Panel ID="PanelMoviles" runat="server">   
       <asp:Label ID="lblFechas" runat="server" Text="" ForeColor="#373435" Font-Bold="true" Font-Size="14px"></asp:Label>         
      
      <br />
        <asp:Label ID="Label4" runat="server" Text="Seleccione Movil" ForeColor="#373435" Font-Bold="true" Font-Size="12px"></asp:Label>       
         <br /> 
           <div style="height:200px;overflow-y: scroll; font-family:Arial;width:320px; border-color:LightGray; border-width:1px; border-style:solid;">       
        <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="8" CellPadding="5" OnItemDataBound="Vehiculo_itemDataBound" Font-Names="Arial" Width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="rdnMovil" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="lblPatente" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
      <div><br /><asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton></div>
          
           </asp:Panel>   
           
       <br />
           <hr />
   <br />

   <table style="width:100%; vertical-align:middle; font-size:10px; font-weight:bold; font-family:Arial" >
   <tr><td> <asp:Label ID="Label5" runat="server" Text="Rango de Fechas (máximo 3 meses):" Font-Bold="true" Font-Names="Arial"></asp:Label></td></tr>
   <tr><td><table>
      <tr style="height:30px;"><td><span> Día Desde:</span></td>
     <td>
      <asp:TextBox ID="txtFechaDesde" runat="server" Width="80px"></asp:TextBox> 
         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtFechaDesde"></asp:RequiredFieldValidator>
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
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtFechaHasta"></asp:RequiredFieldValidator>
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
   </table></td></tr>
  
     <tr>
     <td colspan="5">
     <br />
     <asp:Label ID="Label1" runat="server" Text="Ver Información agrupada por:" Font-Bold="true" Font-Names="Arial"></asp:Label>
         <br />
         <br />
     <asp:RadioButtonList ID="rdnPeriodo" runat="server" RepeatDirection="Horizontal" Font-Names="Arial" CellSpacing="3" AutoPostBack="true">
         <asp:ListItem Value="1" Selected="True">Días</asp:ListItem>
         <asp:ListItem Value="2">Semanas</asp:ListItem>
         <asp:ListItem Value="3">Meses</asp:ListItem>
         </asp:RadioButtonList>
          <br />
          <asp:Label ID="Label2" runat="server" Text="Para la agrupacioón por Días el rango máximo de fechas es de 7 días. Para agrupación por Mes debera elegir un rango de más de dos meses."  Font-Names="Arial"></asp:Label>
     </td>
        
         
     </tr>
     <tr>
     <td colspan="5" style="text-align:right;">
     <asp:Button ID="btnVerEstadisticas" runat="server" Text="Ver Estadisticas" CssClass="button2" Width="130px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/> 
  </td>
  <td colspan="5" style="text-align:right;">
     <asp:Button ID="ButtonReporte" runat="server" Text="Ir a Reportes" CssClass="button2" Width="130px" Font-Size="14px" Height="40px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/> 
  </td>
     </tr>
    </table>
       <br />
       <br />
  </div>

 </div>
   <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="12px"></asp:Label>
    <br />
       <br />
          <asp:Panel ID="PanelEstaditicas" runat="server" Visible="false" Height="100%">
         
           <table  cellSpacing="3" cellPadding="3" border="0" align="left" style="height:100%; width:100%">
<tr>
<td >

  <div id="barCharVelocidad" style="width:auto; height: auto;">
  </div>
  <div style="width:20%; margin-left:250px;"><a id="mostrarTable1" onclick="javascript:document.getElementById('DatosVelocidad').style.display = 'block'; document.getElementById('ocularTable1').style.display = 'block';document.getElementById('mostrarTable1').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable1" onclick="javascript:document.getElementById('DatosVelocidad').style.display = 'none';document.getElementById('mostrarTable1').style.display = 'block';document.getElementById('ocularTable1').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a>
  </div>
  <div id="DatosVelocidad" style="display:none;" >
  <br />
   <asp:Literal ID="ltGrillaVelocidad" runat="server"></asp:Literal></div>
</td>
<td >
  <div id="ComboCharParadas" style="width:auto; height: auto;"></div>
   <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable2" onclick="javascript:document.getElementById('DatosParadas').style.display = 'block'; document.getElementById('ocularTable2').style.display = 'block';document.getElementById('mostrarTable2').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable2" onclick="javascript:document.getElementById('DatosParadas').style.display = 'none';document.getElementById('mostrarTable2').style.display = 'block';document.getElementById('ocularTable2').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div>
  <div id="DatosParadas" style="display:none;" >
  <br />
   <asp:Literal ID="ltGrillaParadas" runat="server"></asp:Literal> 
   <br />
   
   <asp:Literal ID="ltGrillaPorcentajeParadas" runat="server"></asp:Literal>   </div>
</td>
</tr>
<tr>
<td colspan="2">
<asp:Literal ID="LiteralDivs" runat="server"></asp:Literal> 
</td>
</tr> 

<tr>
<td>
 <div id="ComboCharParadasDetallado" style="width:auto; height: auto;"></div>
  <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable3" onclick="javascript:document.getElementById('DatosParadaDetallado').style.display = 'block'; document.getElementById('ocularTable3').style.display = 'block';document.getElementById('mostrarTable3').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable3" onclick="javascript:document.getElementById('DatosParadaDetallado').style.display = 'none';document.getElementById('mostrarTable3').style.display = 'block';document.getElementById('ocularTable3').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosParadaDetallado" style="display:none;"><br />
 <asp:Literal ID="ltGrillaParadaDetallado" runat="server"></asp:Literal>  
 </div>
</td>
<td>
 <div id="ComboCharEncendidoDetallado" style="width:auto; height: auto;"></div>
  <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable8" onclick="javascript:document.getElementById('DatosEncendido').style.display = 'block'; document.getElementById('ocularTable8').style.display = 'block';document.getElementById('mostrarTable8').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable8" onclick="javascript:document.getElementById('DatosEncendido').style.display = 'none';document.getElementById('mostrarTable8').style.display = 'block';document.getElementById('ocularTable8').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosEncendido" style="display:none;"><br />
 <asp:Literal ID="ltGrillaEncendidoDetallado" runat="server"></asp:Literal>  
 </div>
</td>
</tr>

<tr>
<td>
 <div id="LineCharParadasPorce" style="width:auto; height: auto;"></div> 
 <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable4" onclick="javascript:document.getElementById('DatosParadasPorcen').style.display = 'block'; document.getElementById('ocularTable4').style.display = 'block';document.getElementById('mostrarTable4').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable4" onclick="javascript:document.getElementById('DatosParadasPorcen').style.display = 'none';document.getElementById('mostrarTable4').style.display = 'block';document.getElementById('ocularTable4').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosParadasPorcen" style="display:none;"><br />   <asp:Literal ID="ltGrillaParadaPorc" runat="server"></asp:Literal></div>
</td> 
<td>
 <div id="LineCharEncendidoPorce" style="width:auto; height: auto;"></div> 
 <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable9" onclick="javascript:document.getElementById('DatosPorcentajeEncendido').style.display = 'block'; document.getElementById('ocularTable9').style.display = 'block';document.getElementById('mostrarTable9').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable9" onclick="javascript:document.getElementById('DatosPorcentajeEncendido').style.display = 'none';document.getElementById('mostrarTable9').style.display = 'block';document.getElementById('ocularTable9').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosPorcentajeEncendido" style="display:none;"><br />   <asp:Literal ID="ltGrillaEncendidoPorc" runat="server"></asp:Literal></div>
</td> </tr>
<tr>
<td>
 <div id="CharTiemposHs" style="width:auto; height: auto;"></div>
 <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable5" onclick="javascript:document.getElementById('DatosTiempos').style.display = 'block'; document.getElementById('ocularTable5').style.display = 'block';document.getElementById('mostrarTable5').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable5" onclick="javascript:document.getElementById('DatosTiempos').style.display = 'none';document.getElementById('mostrarTable5').style.display = 'block';document.getElementById('ocularTable5').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosTiempos" style="display:none;">   <asp:Literal ID="ltGrillaTiempoHs" runat="server"></asp:Literal></div> 
</td>  
<td >
 <div id="CharKmsRecorridos" style="width:auto; height: auto;"></div> 
 <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable6" onclick="javascript:document.getElementById('DatosRecorridos').style.display = 'block'; document.getElementById('ocularTable6').style.display = 'block';document.getElementById('mostrarTable6').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable6" onclick="javascript:document.getElementById('DatosRecorridos').style.display = 'none';document.getElementById('mostrarTable6').style.display = 'block';document.getElementById('ocularTable6').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosRecorridos" style="display:none;">   <asp:Literal ID="ltGrillaKmsRecorridos" runat="server"></asp:Literal></div> 
</td>
</tr>
    <tr>
<td>
 <div id="CharAlarmas" style="width:auto; height: auto;"></div>
  <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable7" onclick="javascript:document.getElementById('DatosAlarmas').style.display = 'block'; document.getElementById('ocularTable7').style.display = 'block';document.getElementById('mostrarTable7').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable7" onclick="javascript:document.getElementById('DatosAlarmas').style.display = 'none';document.getElementById('mostrarTable7').style.display = 'block';document.getElementById('ocularTable7').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosAlarmas" style="display:none;">   <asp:Literal ID="ltGrillaAlarmas" runat="server"></asp:Literal></div>  
</td>
<td colspan="3">
<div id="CharSensores" style="width:auto; height: auto;"></div>
  <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable11" onclick="javascript:document.getElementById('DatosSensores').style.display = 'block'; document.getElementById('ocularTable11').style.display = 'block';document.getElementById('mostrarTable11').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable11" onclick="javascript:document.getElementById('DatosSensores').style.display = 'none';document.getElementById('mostrarTable11').style.display = 'block';document.getElementById('ocularTable11').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 <div  id="DatosSensores" style="display:none;">   <asp:Literal ID="ltGrillaSensores" runat="server"></asp:Literal> <br/> <br/>
 
<asp:Literal ID="ltGrillaSensoresDetallado" runat="server"></asp:Literal>
 </div>  
</td>
</tr> 

<tr>
<td colspan="2"><table style="width:100%"><tr>
<td>
 <div id="ChartBarraTemp" style="width:auto; height:auto;"></div>
  <div style="width:20%; margin-left:250px;">
   <a id="mostrarTable10" onclick="javascript:document.getElementById('DatosIndicadores').style.display = 'block'; document.getElementById('ocularTable10').style.display = 'block';document.getElementById('mostrarTable10').style.display = 'none';" style="color:#D85639; font-family:Arial; font-size:14px; font-weight:bold;">Ver Datos</a>
  <a id="ocularTable10" onclick="javascript:document.getElementById('DatosIndicadores').style.display = 'none';document.getElementById('mostrarTable10').style.display = 'block';document.getElementById('ocularTable10').style.display = 'none';" style="display:none;color:#D85639;font-family:Arial; font-size:14px; font-weight:bold;">Ocultar Datos</a></div> 
 
</td>
<td><div id="ChartBarraRPM" style="width:auto; height: auto;"></div></td>
<td><div id="ChartBarraBat" style="width:auto; height: auto;"></div></td>
</tr></table></td>

</tr>
<tr>
<td colspan="3">
 
  <div  id="DatosIndicadores" style="display:none; width:50%">  
   <asp:Literal ID="ltGrillaIndicadores" runat="server"></asp:Literal>
    <br/> <br/>
     <asp:Literal ID="ltGrillaIndicadoresDetalaldo" runat="server"></asp:Literal>
   </div>  
  <br/> 
</td>

</tr>

</table>
 </asp:Panel>

 <br /> <br /> 

    </div>
  <script type="text/javascript">
      $(function () {
          $('#tabs').tabs('select', document.getElementById("<%= hdnTab.ClientID %>").value);
      });

      if (document.getElementById("<%= hdnOcultarProporcion.ClientID %>").value == "1")
     {
     document.getElementById('mostrarTable4').style.display = 'none';
      document.getElementById('mostrarTable9').style.display = 'none';
  }

  
   </script>
 <br />
<br />

</asp:Content>