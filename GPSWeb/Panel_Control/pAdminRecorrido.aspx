<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminRecorrido.aspx.vb" Inherits="GPSWeb.pAdminRecorrido" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />   
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 100%; height:auto;">
    <asp:Label ID="Label2" runat="server" Text="Adminsitración de Recorridos para uso Generico" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br />
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
     <br />
   
 <div>
    
   <div>    
         <asp:GridView ID="gridRecorrido" DataKeyField="zon_id" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="center" 
                    AllowSorting="true" OnSorting="SortRecords" OnRowCommand="grid_rowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="Recorrido" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="rec_nombre" ></asp:BoundField>
                            <asp:BoundField HeaderText="Origen" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="rec_origen" ></asp:BoundField>
                            <asp:BoundField HeaderText="Destino" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="rec_destino" ></asp:BoundField>
                              <asp:HyperLinkField DataNavigateUrlFields="zon_id" HeaderStyle-Width="5%" DataNavigateUrlFormatString="pAddRecorrido.aspx?rec_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                          <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("rec_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Recorrido Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                          </Columns>
                      
                     </asp:GridView>    
                     </div> 
   <div  style="float:right; width:30%;">
          <br />         
            <asp:HyperLink ID="HyperLink1" Font-Bold="true" ForeColor="DarkBlue" Font-Size="14px" runat="server" NavigateUrl="~/Panel_Control/pAddRecorrido.aspx">Agregar Nuevo Recorrido</asp:HyperLink>
          </div>
    
 </div>          
 </div>
</asp:Content>
