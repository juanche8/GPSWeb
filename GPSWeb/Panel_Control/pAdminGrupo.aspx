<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminGrupo.aspx.vb" Inherits="GPSWeb.pAdminGrupo" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<%@ Register Assembly="Trirand.Web" TagPrefix="trirand" Namespace="Trirand.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- The jQuery UI theme extension jqGrid needs -->
   <link rel="stylesheet" type="text/css" media="screen" href="../css/azul/ui.jqgrid.css" /> 
    <link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
      <script src="../scripts/trirand/i18n/grid.locale-sp.js" type="text/javascript"></script>
    <!-- The jqGrid client-side javascript -->
      <script src="../scripts/trirand/jquery.jqGrid.min.js" type="text/javascript"></script>   
    
     <script type="text/javascript">


         function formatEditRec(cellValue, options, rowObject) {
         var url = "nuevoGrupo('pAddGrupo.aspx?grup_id=" + cellValue + "');";
         var imageHtml = '<a title="Editar Alarma" href="#" onclick="' + url + '"  originalValue=""><img src="../images/edit.gif" border="0" /></a>';
             return imageHtml;
         }

         function unformatEdit(cellValue, options, cellObject) {
             return $(cellObject.html()).attr("originalValue");
         }

         function formatDelete(cellValue, options, rowObject) {
             var imageHtml = "<a title='Editar Alarma' href='pAdminGrupo.aspx?grup_id=" + cellValue + "';' originalValue=''><img src='../images/delete.gif' border='0' /></a>";
             return imageHtml;
         }

         function nuevoGrupo(url) {


             $('<iframe id="NuevoGrupo" border="0" frameborder="0" framespacing="0"  src="' + url + '" />').dialog({
                 title: 'Nuevo Grupo',
                 autoOpen: true,
                 width: 'auto',
                 height: 400,
                 modal: true,
                 resizable: true,
                 autoResize: true,
                 close: function () {
                     $(this).dialog('destroy').remove();
                 },
                 overlay: {
                     opacity: 0.5,
                     background: "black"
                 }
             });
             $('#NuevoGrupo').dialog('open').width(400).height(400);

         }


         function unformatDelete(cellValue, options, cellObject) {
             return $(cellObject.html()).attr("originalValue");
         }

         function cerrar() {

             $('#NuevoGrupo').dialog('destroy').remove();
             window.location.reload();
         }
       
        </script>
       

  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
      <asp:HiddenField ID="hdncat_usu" runat="server" Value="0" />
      <asp:HiddenField ID="hdncat_id" runat="server" Value="0" />
    
 <div  style="float:left; width:90%; height:auto;">
 <div style="margin-left:50px; width:90%;height:auto;">
 
 <h3>Mis Grupos de Moviles</h3>
 <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
    
    <div  style="float:right; width:30%; margin-right:300px;"> 
    <input type="button" class="button2" value="Nuevo Grupo" onclick="javascript:nuevoGrupo('pAddGrupo.aspx');" />
    </div>
  <br />
    </div>
     
 
    <div style="margin-left:50px; width:50%;z-index:0;">
    <br />
     <br />

        <asp:GridView ID="gridGrupos" DataKeyField="grup_id" runat="server" 
             AutoGenerateColumns="False" Width="95%" HorizontalAlign="Center" 
                    AllowSorting="True" OnSorting="SortRecords"  AllowPaging="true" PageSize="20"
             OnRowCommand="grid_rowCommand" CellPadding="4" 
             EnableModelValidation="True" BackColor="White"  OnPageIndexChanging="gridGrupo_PageIndexChanging"
                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                ForeColor="Black" GridLines="Vertical" Font-Size="11px">
                       <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                       
                            <asp:BoundField HeaderText="Grupo" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Left"  DataField="grup_nombre" SortExpression="grup_nombre" >
<HeaderStyle Width="40%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>                         
                     <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <a href="#" onclick="javascript:nuevoGrupo('pAddGrupo.aspx?grup_id=<%# Eval("grup_id")%>');"><img src='../images/edit.gif' border='0'></a>                                </ItemTemplate>
                                </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonCorriente" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("grup_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" Width="16px" Height="16px" ToolTip="Cortar Electricidad" OnClientClick="return confirm('Esta Seguro de eliminar el Grupo Seleccionado?');"/>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="5%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                       
                          </Columns>
                      
                         <FooterStyle BackColor="#CCCCCC" />
                      <HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
                        <PagerStyle BackColor="#343535" ForeColor="White" HorizontalAlign="Center" Font-Bold="true" Font-Size="12px" Font-Underline="true" />
                      <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                     </asp:GridView>    

    </div>
    
        </div>         
   
     
</asp:Content>