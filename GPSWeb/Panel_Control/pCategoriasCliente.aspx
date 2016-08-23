<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pCategoriasCliente.aspx.vb" Inherits="GPSWeb.pCategoriasCliente" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" /> 
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 80%; height: 700px">
    <asp:Label ID="Label2" runat="server" Text="CATEGORIAS DE ALERTAS" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
     <br />
 <div>
     <br />
        <asp:GridView ID="GridViewCategorias" runat="server" DataKeyField="cat_usu_id" AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" OnRowCommand="GridView_RowCommand" >
        <Columns>  <asp:BoundField HeaderText="Nombre" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="cat_usu_descripcion" ></asp:BoundField>
                          <asp:BoundField HeaderText="Unidad de Medida" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="cat_usu_unidadmedida" ></asp:BoundField>
                          <asp:BoundField HeaderText="Valor Por Defecto" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="cat_usu_valor_defecto" ></asp:BoundField>
                       <asp:HyperLinkField DataNavigateUrlFields="cat_usu_id" HeaderStyle-Width="5%" DataNavigateUrlFormatString="pAdminCategorias.aspx?cat_usu_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("cat_usu_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                            
                       </Columns>
        <EmptyDataTemplate>No Existen Alertas Configuradas</EmptyDataTemplate>
        </asp:GridView>    
     <br />
      <asp:Button ID="Button2" runat="server" Text="Volver" PostBackUrl="~/Panel_Control/pAdminAlarmas.aspx" />
     <asp:Button ID="Button1" runat="server" Text="Nueva" PostBackUrl="~/Panel_Control/pAdminCategorias.aspx" />
 </div>          
 </div>
</asp:Content>
