<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminAlarmas.aspx.vb" Inherits="GPSWeb.pAdminAlarmas2" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<%@ Register Assembly="Trirand.Web" TagPrefix="trirand" Namespace="Trirand.Web.UI.WebControls" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
   
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
         
       
        function BuscarAlarmas(movil,patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPantenteZ.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;

            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasZonas",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var algo = '';
                },
                error: function(jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmasZona.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmZona");
        }
        function BuscarAlarmasRec(movil,patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPantenteR.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasRecorridos",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var algo = '';
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
            var gridAlarmas = jQuery("#<%= JQGridAlarmaRecorrido.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmRecorrido");
        }

        function BuscarAlarmasDir(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteD.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasDirecciones",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var algo = '';
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridDireccion.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmDirecciones");
        }

        function BuscarAlarmasRecFe(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteF.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasRecordatorioFecha",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var algo = '';
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridRecordatorioFecha.ClientID %>");
            gridAlarmas.trigger("reloadGrid");

            /* recordatorio por km */
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasRecordatorioKm",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(response) {
                    var algo = '';
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridRecordatorioKm.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmRecFecha");
        }
        
        function MostrarGrilla(id) {
            $(id).dialog({
                autoOpen: true,
                title: '',
                width: 850,
                height: 460,
                position: ["center"],              
                buttons: {                    
                    Cerrar: function () {
                        $(this).dialog('close');                   
                                            }
                }
            });

        }

        function activaDesacAlertaR(veh_id,id,estado) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/activaDesacAlertaR",
                data: "{'id': '" + id + "','estado': '" + estado + "','veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    result = response.d ? response.d : response;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmaRecorrido.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmRecorrido");
        }

        function activaDesacAlertaZ(veh_id, id, estado) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/activaDesacAlertaZ",
                data: "{'id': '" + id + "','estado': '" + estado + "','veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    result = response.d ? response.d : response;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmasZona.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmZona");
        }

        function activaDesacAlertaD(veh_id, id, estado) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/activaDesacAlertaD",
                data: "{'id': '" + id + "','estado': '" + estado + "','veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    result = response.d ? response.d : response;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridDireccion.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmDirecciones");
        }

        //funciones para la grilla de jQuery

        function formatEditRec(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmasRecorrido.aspx?rec_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

      
        function formatEditRecFecha(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaRecordatorioFecha.aspx?recf_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditRecKm(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaRecordatorioKm.aspx?reck_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditZona(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmasZonas.aspx?veh_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditDirecc(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmasDireccion.aspx?dir_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }
     
        function unformatEdit(cellValue, options, cellObject) {
            return $(cellObject.html()).attr("originalValue");
        }

        function formatDelete(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAdminAlarmas.aspx?alar_id="+ cellValue + "';' originalValue=''><img src='../images/delete.gif' border='0' /></a>";
            return imageHtml;
        }


        function unformatDelete(cellValue, options, cellObject) {
            return $(cellObject.html()).attr("originalValue");
        }

        function nuevaAlarma() {          
            $("#menuAlarma").dialog({
                title: 'Seleccione Alarma',
                autoOpen: true,               
                modal: true,
                resizable: false,
                autoResize: true,                
                position: [800, 250],
                buttons: {
                    Cerrar: function () {
                    $(this).dialog('close');
                                            }
                }
            });
    }

    function adminZona() {
       window.location.href = 'pAlarmasZonas.aspx?movil=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
   }

   function adminDesvio() {
       window.location.href = 'pAlarmasRecorrido.aspx?movil=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
   }
        
        function adminDirecc(){
        window.location.href='pAlarmasDireccion.aspx?movil=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

    function adminRecorrid()  
    {
      window.location.href='pAlarmaRecordatorio.aspx?movil=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
    }

    function adminRecord()  
    {
      window.location.href='pAlarmaRecordatorio.aspx?movil=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
    }
       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
 <asp:HiddenField ID="hdnMovil" runat="server" Value="0" />
 <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
 <asp:HiddenField ID="hdnale_id" runat="server" />
  <asp:HiddenField ID="hdnsubcat_id" runat="server" />  
  <div style="float: left; width:100%; height:100%;">
  <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Alarmas configuradas por Móvil</h3>
 <h5>Para editar seleccione sobre cada celda la alarma y el móvil.</h5>
 </div>
  <div style="margin-left:40px; width:85%; z-index:9999; padding:5px; vertical-align:middle;">
  <div style="float:left; width:30%; text-align:center;"> 
        <asp:HyperLink ID="HyperLink1" Font-Bold="true" ForeColor="#D85639" Font-Size="14px" runat="server" Font-Names="Helvetica,Arial,sans-serif" NavigateUrl="~/Panel_Control/pAlarmas.aspx">Ir a Alarmas Reportadas</asp:HyperLink>
       </div> 

    <div style="float:right; width:30%;">
        <a id="A1" href="#" onclick="nuevaAlarma();" style="font-size:14px; font-weight:bold;Color:#D85639;" runat="server" Font-Names="Helvetica Neue,Helvetica,Arial,sans-serif">Nueva Alarma</a> 
          
    </div> 
    <br /> <br /> 
   </div>
 
   <div style="margin-left:10px; width:95%;z-index:0;">  
  <br /> <br />
  <asp:GridView ID="GridViewAlarmas" runat="server" DataKeyNames="veh_id"  
          AutoGenerateColumns="False" Width="90%" HorizontalAlign="Center" 
          OnRowDataBound="GridViewAlarmas_RowDataBound" AllowSorting="True" RowStyle-Font-Size="9"
 HeaderStyle-Font-Size="10" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
           <asp:BoundField HeaderText="Patente" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="veh_patente" >
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>       
                        <asp:BoundField HeaderText="Conductor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="veh_nombre_conductor" >
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>
                            <asp:TemplateField HeaderText="Exc.Velocidad" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>  
                                <a href="pAlarmaEdit.aspx?veh_id=<%# Eval("veh_id")%>"><asp:Label ID="lblVelocidad" runat="server" Text="Sin Configurar"></asp:Label></a>                              
                                    </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                         
                              <asp:TemplateField HeaderText="Sensores" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                 <a href="pAlarmaSensorEdit.aspx?veh_id=<%# Eval("veh_id")%>" ><asp:Label ID="lblSensor" runat="server" Text="Sin Configurar"></asp:Label></a>                              
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Desvio Recorrido" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <a href="#" onclick="BuscarAlarmasRec('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantRecorrido" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Entrada/Salida Zona" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <a href="#" onclick="BuscarAlarmas('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantZona" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Entrada/Salida Direcc" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <a href="#" onclick="BuscarAlarmasDir('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantDireccion" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>
<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recordatorios" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="#" onclick="BuscarAlarmasRecFe('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantRecordatorio" runat="server" Text="Sin Configurar"></asp:Label></a>                              
                               </ItemTemplate>
<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>             
                       </Columns>       
        <FooterStyle BackColor="#CCCCCC" />
<HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
<RowStyle Font-Size="9pt"></RowStyle>
        <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
        </asp:GridView>   
  </div>
   
    </div>
    
 
    <!-- Pop Up Configurar Zonas -->
 <div id="abmZona"  style="display: none">                   
                       <asp:Label ID="Label2" runat="server" Text="Alarma Desvio de Zonas" Font-Bold="true" Font-Size="12px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPantenteZ" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="12px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridAlarmasZona" Width="95%" LoadOnce="true" OnDataRequesting="JQGridAlarmasZona_DataRequesting" OnCellBinding="JQGridAlarmasZona_CellBinding" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Detalle" HeaderText="Detalle" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Tipo" HeaderText="Tipo" PrimaryKey="false" Width="100" />
                      <trirand:JQGridColumn DataField="zon_activa" HeaderText="Activa" Width="50" Editable="true" />
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditZona" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>                      
                      <trirand:JQGridColumn DataField="zon_activa" HeaderText="Act/DesAct" Width="60" Editable="true"  TextAlign="Center"/>   
                       <trirand:JQGridColumn DataField="veh_id" HeaderText="veh_id" Width="10" Visible="false" />            
                        </Columns>   
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />             
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button1" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminZona();" />                  
           </div>                                   
                 </div> 
        </div>  
      <!-- Pop Up Configurar Recorridos -->
 <div id="abmRecorrido"  style="display: none">                   
                       <asp:Label ID="Label6" runat="server" Text="Alarma Desvió de Recorridos" Font-Bold="true" Font-Size="14px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPantenteR" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="14px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridAlarmaRecorrido" Width="100%" OnDataRequesting="JQGridAlarmasRecorrido_DataRequesting" OnCellBinding="JQGridAlarmasRecorrido_CellBinding">
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Descripcion" HeaderText="Descripción" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Origen" HeaderText="Origen" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Destino" HeaderText="Destino" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="rec_activa" HeaderText="Activa" Width="40" Editable="true" />  
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="40" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="40" Editable="true" />
                    <trirand:JQGridColumn DataField="rec_id" Width="50" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditRec" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="rec_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>   
                       <trirand:JQGridColumn DataField="rec_activa" HeaderText="Act/DesAct" Width="60" Editable="true" TextAlign="Center" /> 
                            <trirand:JQGridColumn DataField="rec_id" HeaderText="rec_id" Width="10" Visible="false" />       
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:center">                     
                     <br />
                     <asp:Button ID="Button2" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminDesvio();" />       
                   
                 </div>                                   
                 </div>  
        </div>  
     <!-- Pop Up Configurar Direcciones -->
 <div id="abmDirecciones"  style="display: none">                   
                       <asp:Label ID="Label4" runat="server" Text="Alarma Entrada y Salida de Direcciones" Font-Bold="true" Font-Size="14px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteD" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="14px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridDireccion" Width="100%" OnDataRequesting="JQGridAlarmasDirecc_DataRequesting" OnCellBinding="JQGridAlarmasDirecc_CellBinding" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Direccion" HeaderText="Direccion" PrimaryKey="false" Width="250" />
                     <trirand:JQGridColumn DataField="Tipo" HeaderText="Tipo" PrimaryKey="false" Width="100" />        
                                
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Notificar Mail" Width="75" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="Notificar SMS" Width="75" Editable="true" />
                    <trirand:JQGridColumn DataField="dir_id" Width="55" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditDirecc" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="dir_id" Width="40" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                           
                    </trirand:JQGridColumn> 
                      <trirand:JQGridColumn DataField="dir_activa" HeaderText="Act/DesAct" Width="60" Editable="true" TextAlign="Center"/>   
                       <trirand:JQGridColumn DataField="dir_id" HeaderText="dir_id" Width="10" Visible="false" />           
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
             <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button3" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminDirecc();" />       
                   
                 </div>           
                                               
                 </div>  
        </div>
   <!-- Pop Up Configurar Recordatorios -->
 <div id="abmRecFecha"  style="display: none">                   
                       <asp:Label ID="Label8" runat="server" Text="Alarma de Recordatorios" Font-Bold="true" Font-Size="14px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteF" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="14px"></asp:Label>
    <br />
     <br /> 
    <div id="tabs"><br />
                            <ul>
                            <li><a href="#tabs-1">Para una Fecha</a></li>  
                                <li><a href="#tabs-2">Por Kilometros Acumulados</a></li>
                            </ul>
                            <div id="tabs-1">  
                <trirand:JQGrid runat="server" ID="JQGridRecordatorioFecha" AutoWidth="true" OnDataRequesting="JQGridAlarmasRecFecha_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="recf_id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Descripcion" HeaderText="Descripción" PrimaryKey="false" Width="200" />
                     <trirand:JQGridColumn DataField="Perciocidad" HeaderText="Periocidad" PrimaryKey="false" Width="100" /> 
                     <trirand:JQGridColumn DataField="Ocurrencia" HeaderText="Prox.Ocurrencia" PrimaryKey="false" Width="100" /> 
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Notificar Mail" Width="75" Editable="true" />
                
                    <trirand:JQGridColumn DataField="recf_id" Width="55" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditRecFecha" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="recf_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>            
                        </Columns>  
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />              
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
            <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button4" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminRecorrid();" />       
                   
                 </div>
                                                  
                 </div>
  <div id="tabs-2">  
  <trirand:JQGrid runat="server" ID="JQGridRecordatorioKm" AutoWidth="true" OnDataRequesting="JQGridAlarmasRecKm_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="reck_id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Descripcion" HeaderText="Descripción" PrimaryKey="false" Width="200" />
                     <trirand:JQGridColumn DataField="Kms" HeaderText="Kms Inic." PrimaryKey="false" Width="100" /> 
                     <trirand:JQGridColumn DataField="Ocurrencia" HeaderText="Prox.Ocurrencia" PrimaryKey="false" Width="100" />                                    
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Notificar Mail" Width="75" Editable="true" />
                 
                    <trirand:JQGridColumn DataField="reck_id" Width="55" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditRecKm" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="reck_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>            
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
         <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button5" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminRecord();" />       
                   
                 </div>
                                                 
                 </div>  
                </div>
        </div>        
         
        <!-- Pop Up Nueva Alarma -->
        <div style="display:none;" id="menuAlarma">
        <br />
        <asp:HyperLink ID="HyperLink2" Font-Size="12px" runat="server" NavigateUrl="~/Panel_Control/pAlarmaAdd.aspx?cat_id=0">Exceso Velocidad</asp:HyperLink><br />
           <asp:HyperLink ID="HyperLink6" Font-Size="12px" runat="server" NavigateUrl="~/Panel_Control/pAlarmaSensorAdd.aspx?cat_id=2">Sensores</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink7" Font-Size="12px" runat="server" NavigateUrl="~/Panel_Control/pAlarmasZonas.aspx">Entrada/Salida Zonas</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink8" Font-Size="12px" runat="server" NavigateUrl="~/Panel_Control/pAlarmasDireccion.aspx">Entrada/Salida Direcc</asp:HyperLink><br />
                <asp:HyperLink ID="HyperLink3" Font-Size="12px" runat="server" NavigateUrl="~/Panel_Control/pAlarmasRecorrido.aspx">Desvio Recorridos</asp:HyperLink><br />
                <asp:HyperLink ID="HyperLink9" Font-Size="12px" runat="server" NavigateUrl="~/Panel_Control/pAlarmaRecordatorio.aspx">Recordatorios</asp:HyperLink><br />
        </div>
</asp:Content>

