<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminZonaRecorrido.aspx.vb" Inherits="GPSWeb.pAdminZona" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<%@ Register Assembly="Trirand.Web" TagPrefix="trirand" Namespace="Trirand.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" media="screen" href="../css/azul/ui.jqgrid.css" /> 
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

      

         function formatEditZona(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAddZona.aspx?zon_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditRec(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAddRecorrido.aspx?rec_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditDir(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAddDireccion.aspx?dir_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function unformatEdit(cellValue, options, cellObject) {
            return $(cellObject.html()).attr("originalValue");
        }

        function formatDeletez(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAdminZonaRecorrido.aspx?zon_id=" + cellValue + "';' originalValue=''><img src='../images/delete.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatDeleter(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAdminZonaRecorrido.aspx?rec_id=" + cellValue + "';' originalValue=''><img src='../images/delete.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatDeleteD(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAdminZonaRecorrido.aspx?dir_id=" + cellValue + "';' originalValue=''><img src='../images/delete.gif' border='0' /></a>";
            return imageHtml;
        }


        function unformatDelete(cellValue, options, cellObject) {
            return $(cellObject.html()).attr("originalValue");
        }
     </script>
     
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" /> 
      <asp:HiddenField ID="hdnTab" runat="server" Value="#tabs-1" />  
  <div style="float: left; width:100%; height:auto;">
 <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Zonas y Recorridos para uso Genérico</h3>
   
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
     <br />
    </div>
   <div id="tabs" style="float: left; width: 950px; height:auto;margin-left:50px;"><br />
                            <ul>
                            <li><a href="#tabs-1">Zonas</a></li>  
                                <li><a href="#tabs-2">Recorrido</a></li>
                               <li><a href="#tabs-3">Direcciones</a></li>
                            </ul>
                            <div id="tabs-1">  
                <trirand:JQGrid runat="server" ID="JQGridZona" Width="900px" Height="400px" OnDataRequesting="JQGridZona_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="zon_id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="zon_nombre" HeaderText="Nombre" Width="400" />
                       <trirand:JQGridColumn DataField="zon_id" Width="50" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditZona" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="zon_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDeletez" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>            
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Zonas Creadas"  PageSize="30"/> 
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
              
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                       <asp:Button ID="Button1" runat="server" Text="Nueva Zona" CssClass="button2" PostBackUrl="~/Panel_Control/pAddzona.aspx" Font-Size="14px" Width="110px" Height="35px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
                 </div>                                   
                 </div>
  <div id="tabs-2">  
  <trirand:JQGrid runat="server" ID="JQGridRecorrrido" Width="900px" Height="400px" OnDataRequesting="JQGridRecorrrido_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="rec_id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="rec_nombre" HeaderText="Nombre" Width="200" />
                    <trirand:JQGridColumn DataField="rec_origen" HeaderText="Origen" PrimaryKey="false" Width="280" />
                     <trirand:JQGridColumn DataField="rec_destino" HeaderText="Destino" PrimaryKey="false" Width="280" /> 
                      <trirand:JQGridColumn DataField="rec_id" Width="50" Searchable="false" HeaderText="Cambiar">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditRec" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="rec_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDeleter" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>            
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Recorridos Creados" PageSize="30" /> 
                  <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />              
            </trirand:JQGrid>        
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button2" runat="server" Text="Nuevo Recorrido" CssClass="button2" PostBackUrl="~/Panel_Control/pAddRecorrido.aspx" Font-Size="14px" Width="130px" Height="35px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
                 </div>                                   
                 </div>  
      <div id="tabs-3">  
  <trirand:JQGrid runat="server" ID="JQGridDireccion" Width="900px" Height="400px" OnDataRequesting="JQGridDireccion_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="dir_id" HeaderText="Id" Width="10" Visible="false" />
                  <trirand:JQGridColumn DataField="dir_nombre" HeaderText="Nombre" Width="200" />
                    <trirand:JQGridColumn DataField="dir_direccion" HeaderText="Dirección" Width="560" />
                       <trirand:JQGridColumn DataField="dir_id" Width="50" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditDir" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="dir_id" Width="55" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDeleteD" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>            
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                  <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
                <PagerSettings NoRowsMessage="No hay Direcciones Creadas"  PageSize="30" />               
            </trirand:JQGrid>        
                   <div style="text-align:right">                     
                     <br />
                      <asp:Button ID="Button3" runat="server" Text="Nueva Dirección" CssClass="button2" PostBackUrl="~/Panel_Control/pAddDireccion.aspx" Font-Size="14px" Width="130px" Height="35px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
                 </div>                                   
                 </div> 
                </div>
 <div>
   <script type="text/javascript">
    
       $(function () {
             $('#tabs').tabs('select', document.getElementById("<%= hdnTab.ClientID %>").value);
         });
    </script>
 </div>          
 </div>
</asp:Content>