<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminModulos.aspx.vb" Inherits="GPSWeb.pAdminModulos" MasterPageFile="~/CMS/SitePages.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
      function limpiar() {
         document.getElementById("<%= txtModulo.ClientID %>").value = "";
          document.getElementById("<%= txtCelular.ClientID %>").value = "";
      }
  </script>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="accion" runat="server" Value="alta" />
   <asp:HiddenField ID="hdnmod_id" runat="server" Value="0" />
 <div class="inline" style="float: left; width: 70%; height: 100%;font-family:Arial; vertical-align:middle; font-size:11px; margin-left:1%">
   <h3>Modulos</h3>
    <br />
     <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="12px" ForeColor="Red"></asp:Label>
      <br />
     <div style="width:70%; margin-left:300px;">
       <asp:Label ID="Label3" runat="server" Text="Número Módulo: " Font-Size="12px" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtNroMod" runat="server" Width="154px"></asp:TextBox>
    &nbsp;  &nbsp;&nbsp;<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="button2" />
    &nbsp;
    <asp:Button ID="btnNuevo" runat="server" Text="Agregar Nuevo" CssClass="button2" />
     </div>
 <div style="margin-left:350px;">
     <br />
     <asp:Label ID="Label4" runat="server" Text="Total de Modulos Creados: " Font-Size="12px"></asp:Label>
      &nbsp;<asp:Label ID="lbltotal" runat="server" Text="" Font-Size="12px"></asp:Label>

        <asp:GridView ID="gridviewModulos" DataKeyField="mod_id" runat="server" 
             AutoGenerateColumns="False" Width="95%" HorizontalAlign="Left" OnPageIndexChanging="Alarmas_PageIndexChanging" 
                  Font-Size="12px" AllowSorting="true" AllowPaging="true" PageSize="20"                     
 HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" OnRowCommand="grid_rowCommand" OnSorting="SortRecords">
        <AlternatingRowStyle BackColor="#CCCCCC" />
                          <Columns>
                         <asp:BoundField HeaderText="ID" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="mod_id" SortExpression="mod_id"></asp:BoundField>
                       <asp:BoundField HeaderText="Nro Módulo" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="mod_numero" SortExpression="mod_numero"></asp:BoundField>
                             <asp:BoundField HeaderText="Nro Celular" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="mod_nro_cel" SortExpression="mod_nro_cel"></asp:BoundField>
                      <asp:BoundField HeaderText="Version" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="mod_version_trama" SortExpression="mod_version_trama"></asp:BoundField>
                       <asp:BoundField HeaderText="En Uso" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="mod_en_uso" SortExpression="mod_en_uso"></asp:BoundField>
                              <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("mod_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Editar" />
                               </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("mod_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Eliminar" OnClientClick="return confirm('Esta Seguro de Eliminar el Modulo Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                       </Columns> 
                      
                         <FooterStyle BackColor="#CCCCCC" />
<HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
<RowStyle Font-Size="9pt"></RowStyle>
        <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
                        <EmptyDataTemplate>No se Encontraron Datos para la Busqueda Realizada</EmptyDataTemplate>
                      
                     </asp:GridView>

     
     <br />
             
 </div>
 </div>
   <!-- pop up alta y modificacion -->
     <asp:LinkButton ID="falseLinkbutton" runat="server" Style="display: none;"></asp:LinkButton>
     <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none;"></asp:LinkButton>
    <ajaxtoolkit:modalpopupextender ID="mpeEditar" runat="server" 
        TargetControlID="btnNuevo" PopupControlID="panelNuevo" 
        CancelControlID="btnCancelar" X="400" Y="100">
    </ajaxtoolkit:modalpopupextender>
       <asp:Panel ID="panelNuevo" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" Height="200px" Width="300px">
     
     <asp:Label ID="lblTitulo" runat="server" Text="Ingrese el Nro del Nuevo Módulo" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br /> <br />
    <div>  
                   <asp:Label ID="Label7" runat="server" Text="Nro Modulo:" Font-Bold="true"></asp:Label>
                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       &nbsp;<asp:TextBox ID="txtModulo" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtModulo"></asp:RequiredFieldValidator>
                   </div>  
                     <div>  
                   <asp:Label ID="Label1" runat="server" Text="Nro Celular Asociado:" Font-Bold="true"></asp:Label>
                       &nbsp;&nbsp;<asp:TextBox ID="txtCelular" runat="server"></asp:TextBox>
                   </div>  
                     <div>  
                   <asp:Label ID="Label5" runat="server" Text="Version de Trama:" Font-Bold="true"></asp:Label>
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<asp:DropDownList ID="ddlTrama" runat="server">
                             <asp:ListItem>RU1</asp:ListItem>
                             <asp:ListItem>RU2</asp:ListItem>
                             <asp:ListItem>RU3</asp:ListItem>
                             <asp:ListItem>RU4</asp:ListItem>
                         </asp:DropDownList>                         
                   </div>   
                   <div>                     
                     <br /> 
                       &nbsp;&nbsp;<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="limpiar();" CausesValidation="false" CssClass="button2" />
                       &nbsp;<asp:Button ID="btnGrabar" runat="server" Text="Grabar" UseSubmitBehavior="false" CssClass="button2"  />
                 </div> 
       </asp:Panel>
</asp:Content>