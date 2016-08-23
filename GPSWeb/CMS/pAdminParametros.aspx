<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminParametros.aspx.vb" Inherits="GPSWeb.pAdminParametros" MasterPageFile="~/CMS/SitePages.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    
    <div class="inline" style="border: thin solid #00A6C6; float: left; width: 25%; height: 700px">
        <asp:Label ID="Label1" runat="server" Text="Tablas :" Font-Bold="true" Font-Size="16px"></asp:Label><br />
<div>  <br /><!-- Tablas con parametros predefinidos -->
          <asp:LinkButton ID="lbtnTClientes" runat="server" Text='Tipos de Clientes'></asp:LinkButton><br />
          <asp:LinkButton ID="lbtnTMarcadores" runat="server" Text='Tipos de Marcadores'></asp:LinkButton><br />
          <asp:LinkButton ID="lbtnTVehiculos" runat="server" Text='Tipos de Vehiculos'></asp:LinkButton><br />
          <asp:LinkButton ID="lbtnTUsos" runat="server" Text='Tipos de Usos de Moviles'></asp:LinkButton><br />
          <asp:LinkButton ID="lbtnTVias" runat="server" Text='Tipos de Vias'></asp:LinkButton><br />
          <asp:LinkButton ID="lbtnEspera" runat="server" Text='Tiempos Espera p/Recibir Trama'></asp:LinkButton><br />
          <asp:LinkButton ID="LinkButton2" runat="server" Text='Parametros Del Sistema'></asp:LinkButton>
     </div>
  
   
</div>
<asp:UpdatePanel ID="panelPrincipal" runat="server" >
<Triggers> <asp:PostBackTrigger ControlID="btnFinalizar" /></Triggers>
<ContentTemplate>

<asp:HiddenField ID="hdnTipo" runat="server" />
    <asp:HiddenField ID="hdnAccion" runat="server" Value="agregar" />
    <asp:HiddenField ID="hdnId" runat="server" Value="0" />
 <div class="inline" style=" float: left; width: 74%; height: 700px">
    <asp:Label ID="Label2" runat="server" Text="Valores Ingresados Para la Tabla: " Font-Bold="true" Font-Size="16px"></asp:Label>
    <asp:Label ID="lblTabla" runat="server" Text="" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br /><asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>  
     <br />
 <div>  
   <br />    
         <asp:GridView ID="datagridTipoCliente" DataKeyField="tipo_cli_id" runat="server" AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnRowCommand="datagridTipoCliente_Command" >
                        <Columns>
                        <asp:BoundField HeaderText="Valor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="tipo_cli_nombre" SortExpression="tipo_cli_nombre" ></asp:BoundField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("tipo_cli_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Editar"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("tipo_cli_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Registro Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>         
                       </Columns>
                      <EmptyDataTemplate>No se encontraron valores para mostrar</EmptyDataTemplate>
                     </asp:GridView>
          <asp:GridView ID="GridViewMarcador" Visible="false" DataKeyField="tipo_marc_id" runat="server" AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnRowCommand="GridViewMarcador_Command" >
                        <Columns>
                        <asp:BoundField HeaderText="Valor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="tipo_marc_nombre" SortExpression="tipo_marc_nombre" ></asp:BoundField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("tipo_marc_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Editar"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("tipo_marc_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Registro Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>         
                       </Columns>
                      <EmptyDataTemplate>No se encontraron valores para mostrar</EmptyDataTemplate>
                     </asp:GridView>
          <asp:GridView ID="GridViewVehiculo" Visible="false" DataKeyField="veh_tipo_id" runat="server" AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnRowCommand="GridViewVehiculo_Command" >
                        <Columns>
                        <asp:BoundField HeaderText="Valor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="veh_tipo_detalle" SortExpression="veh_tipo_detalle" ></asp:BoundField>
                          <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("veh_tipo_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Editar"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("veh_tipo_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Registro Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>         
                       </Columns>
                      <EmptyDataTemplate>No se encontraron valores para mostrar</EmptyDataTemplate>
                     </asp:GridView> 
          <asp:GridView ID="GridViewUsos" Visible="false" DataKeyField="tipo_uso_id" runat="server" AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnRowCommand="GridViewUsos_Command" >
                        <Columns>
                        <asp:BoundField HeaderText="Valor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="tipo_uso_descripcion" SortExpression="tipo_uso_descripcion" ></asp:BoundField>
                     <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("tipo_uso_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Editar"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("tipo_uso_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Registro Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>         
                       </Columns>
                      <EmptyDataTemplate>No se encontraron valores para mostrar</EmptyDataTemplate>
                     </asp:GridView>  
         <asp:GridView ID="GridViewVias" Visible="false" DataKeyField="tipo_via_id" runat="server" AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnRowCommand="GridViewVias_Command">
                        <Columns>
                        <asp:BoundField HeaderText="Valor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="tipo_via_nombre" SortExpression="tipo_via_nombre" ></asp:BoundField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("tipo_via_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("tipo_via_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Registro Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>         
                       </Columns>
                      <EmptyDataTemplate>No se encontraron valores para mostrar</EmptyDataTemplate>
                     </asp:GridView> 
                      <asp:GridView ID="GridViewTiempos" Visible="false" DataKeyField="par_id" runat="server" AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnRowCommand="GridViewTiempos_Command">
                        <Columns>
                         <asp:BoundField HeaderText="Nombre" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="par_nombre" SortExpression="tipo_via_nombre" ></asp:BoundField>
                        <asp:BoundField HeaderText="Valor/segundos" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="par_valor" SortExpression="tipo_via_nombre" ></asp:BoundField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("par_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                             
                       </Columns>
                      <EmptyDataTemplate>No se encontraron valores para mostrar</EmptyDataTemplate>
                     </asp:GridView> 
                      <asp:GridView ID="GridViewParametro" Visible="false" DataKeyField="par_id" runat="server" AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnRowCommand="GridViewParametro_Command">
                        <Columns>
                         <asp:BoundField HeaderText="Nombre" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="par_nombre" SortExpression="par_nombre" ></asp:BoundField>
                        <asp:BoundField HeaderText="Valor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="par_valor" SortExpression="par_valor" ></asp:BoundField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("par_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                             
                       </Columns>
                      <EmptyDataTemplate>No se encontraron valores para mostrar</EmptyDataTemplate>
                     </asp:GridView> 
     <div> <asp:Button ID="Button1" runat="server" Text="Agregar Nuevo" />
     </div>            
    
        

 </div>
                
 </div>
  <!-- pop up alta y modificacion -->
     <asp:LinkButton ID="falseLinkbutton" runat="server" Style="display: none;"></asp:LinkButton>
     <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none;"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="mpeEditar" runat="server" TargetControlID="falseLinkbutton" PopupControlID="panelPagar" CancelControlID="btnCancelar" X="600" Y="300">
    </ajaxToolkit:ModalPopupExtender>
       <asp:Panel ID="panelPagar" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" Height="300px" Width="350px">
        <div>
            <br />
            
            <div>
                <asp:Label ID="lblTablam" runat="server" SkinID="labelBackend" Text="" Font-Bold="true" Font-Size="14px" />
                <br />
                <br />
               &nbsp;&nbsp;<asp:Label ID="lblPrametro" runat="server" SkinID="labelBackend" Font-Bold="true"
                    Text="" />
                <br />
                &nbsp;&nbsp;<asp:Label ID="Label5" runat="server" SkinID="labelBackend" 
                    Text="Valor: " />
                &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtValor" runat="server" Width="162px"></asp:TextBox>
                <br />
                <br />
                <asp:Panel ID="PanelFile" runat="server">
                    &nbsp;&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" SkinID="labelBackend" 
                        Text="Imagen: " />
                    &nbsp;<asp:Image ID="Image1" runat="server" />
                    <br />
                    <br />
                    &nbsp;
                    <asp:Label ID="Label8" runat="server" SkinID="labelBackend" 
                        Text="Nueva Imagen: " />
                    &nbsp;&nbsp; <asp:FileUpload ID="FileUpload1" runat="server" />
                </asp:Panel>
                
            </div>           
            <div>
                <br />
                 <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                 &nbsp;<asp:Button ID="btnFinalizar" runat="server" Text="Grabar" OnClick="btnFinalizar_Click"/>               
            </div>
        </div>
    </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

 

</asp:Content>