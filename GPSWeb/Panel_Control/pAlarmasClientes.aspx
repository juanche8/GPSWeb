<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmasClientes.aspx.vb" Inherits="GPSWeb.pAdminAlarmasClientes" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>      
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">  
    <ContentTemplate>
    <asp:HiddenField ID="hdncli_id" runat="server" />    
    <div class="inline" style="border: thin solid #00A6C6; float: left; width: 25%; height: 700px">
    <div>
        <asp:Label ID="Label5" runat="server" Text="Categorias Predeterminadas:" Font-Bold="true" Font-Size="16px"></asp:Label><br />
 <!--litado vehiculos-->
     <asp:DataList ID="DataListCategorias" runat="server" DataKeyField="cat_id" OnItemCommand="DataListCategorias_Command">
     <ItemTemplate>
          <asp:LinkButton ID="lbtnCategoria" runat="server" Text='<%# Eval("cat_descripcion")%>' CommandName="Filtrar"></asp:LinkButton>
     </ItemTemplate>
     </asp:DataList>
    </div>
        <br />
    <br />
     <div>    
        <asp:Label ID="Label7" runat="server" Text="Categorias Customizadas:" Font-Bold="true" Font-Size="16px"></asp:Label><br />
 <!--litado vehiculos-->
     <asp:DataList ID="DataListCategoriaCliente" runat="server" DataKeyField="cat_usu_id" >
     <ItemTemplate>
          <asp:LinkButton ID="lbtnCategoria" runat="server" Text='<%# Eval("cat_usu_descripcion")%>' CommandName="Filtrar"></asp:LinkButton>
     </ItemTemplate>
     </asp:DataList>
    </div>
    <div>
    <br />
        <asp:Button ID="Button3" runat="server" Text="Crear Nueva Categoria" PostBackUrl="~/Panel_Control/pCategoriasCliente.aspx" />
    </div>
</div>
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 74%;height: 700px">
 <div>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="16px"></asp:Label>
 </div>
    <asp:Label ID="Label2" runat="server" Text="Configuración:" Font-Bold="true" Font-Size="16px"></asp:Label><br />
  <asp:Label ID="Label4" runat="server" Text="Categoria:" Font-Size="16px"></asp:Label>
  <asp:Label ID="lblCategoria" runat="server" Text="" Font-Size="16px"></asp:Label>
     <br />
     <br />
    <asp:Panel ID="PanelAlarmasGenericas" runat="server">
     <div style="text-align:center;">
      <!--litado vehiculos-->
      <asp:Label ID="Label1" runat="server" Text="Vehiculos:" Font-Bold="true"></asp:Label>
         &nbsp; <asp:DropDownList ID="ddlVehiculo" runat="server" DataTextField="veh_descripcion" DataValueField="veh_id" AutoPostBack="true" >
         </asp:DropDownList>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Seleccione Movil" ValidationGroup="alerta" ControlToValidate="ddlVehiculo" InitialValue="0"></asp:RequiredFieldValidator>
         <br />
     </div>
 <div>  <asp:Label ID="Label6" runat="server" Text="Alarmas:" Font-Bold="true"></asp:Label><br />
  <asp:DataGrid ID="datagridAlarmas" DataKeyField="subcat_id" runat="server"  AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" >
                        <Columns>
                         <asp:TemplateColumn HeaderText="Avisarme" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAvisarme" runat="server"/>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="Alarma" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="subcat_descripcion" >
<HeaderStyle Width="20%"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundColumn>
                             <asp:TemplateColumn HeaderText="Valor Por Defecto" HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtValor" runat="server" 
                                        Text='<%# Eval("subcat_valor_por_defecto")%>' Width="92px"></asp:TextBox>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("subcat_unidadmedida")%>'></asp:Label>
                               </ItemTemplate>
<HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn> 
                             <asp:TemplateColumn HeaderText="Vía Notificación" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSMS" runat="server" Text="SMS"/>                       
                                    <asp:CheckBox ID="chkMail" runat="server" Text="E-Mail"/>
                               </ItemTemplate>
<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn> 
                       </Columns>
                     </asp:DataGrid>
                     <br />
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" ValidationGroup="alerta" />
     <br /><br />
     </div>
     </asp:Panel>
 
    <asp:Panel ID="PanelRecorridos" runat="server" Visible="false">
    <div>
        <asp:GridView ID="datagridRecorridos" runat="server" DataKeyField="rec_id" runat="server"  AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" OnRowCommand="datagridRecorridos_RowCommand" >
        <Columns>
          <asp:TemplateField HeaderText="Patente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Vehiculos.veh_patente%>
                               </ItemTemplate>
                               </asp:TemplateField>        
                        <asp:BoundField HeaderText="Nombre" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="rec_nombre" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar E-Mail" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="rec_enviar_mail" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar SMS" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="rec_enviar_sms" ></asp:BoundField>
                       <asp:HyperLinkField DataNavigateUrlFields="rec_id" HeaderStyle-Width="5%" DataNavigateUrlFormatString="pAlarmasRecorrido.aspx?rec_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("rec_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                            
                       </Columns>
        <EmptyDataTemplate>No Existen Alertas Configuradas</EmptyDataTemplate>
        </asp:GridView>
    
                     <br />
                     <asp:Button ID="btnNewAlertaRecorridos" runat="server" Text="Configurar Nueva" PostBackUrl="~/Panel_Control/pAlarmasRecorrido.aspx" />
</div>
    </asp:Panel>  
    
      <asp:Panel ID="PanelDirecciones" runat="server" Visible="false">
    <div>
        <asp:GridView ID="GridViewDirecciones" runat="server" DataKeyField="dir_id" AutoGenerateColumns="False" Width="70%" HorizontalAlign="center"  OnRowCommand="GridViewDirecciones_RowCommand" >
        <Columns>
       <asp:TemplateField HeaderText="Patente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Vehiculos.veh_patente%>
                               </ItemTemplate>
                               </asp:TemplateField>
                        <asp:BoundField HeaderText="Direccion" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="dir_direccion" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar E-Mail" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="dir_enviar_mail" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar SMS" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="dir_enviar_sms" ></asp:BoundField>
                       <asp:HyperLinkField DataNavigateUrlFields="dir_id" HeaderStyle-Width="5%" DataNavigateUrlFormatString="pAlarmasDireccion.aspx?dir_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("dir_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                            
                       </Columns>
        <EmptyDataTemplate>No Existen Alertas Configuradas</EmptyDataTemplate>
        </asp:GridView>
    
                     <br />
                     <asp:Button ID="Button1" runat="server" Text="Configurar Nueva" PostBackUrl="~/Panel_Control/pAlarmasDireccion.aspx" />
</div>
    </asp:Panel> 
         
            <asp:Panel ID="PanelZona" runat="server" Visible="false">
    <div>
        <asp:GridView ID="GridViewZonas" runat="server" DataKeyField="zon_id" AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" OnRowCommand="GridViewZonas_RowCommand" >
        <Columns>
       <asp:TemplateField HeaderText="Patente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Vehiculos.veh_patente%>
                               </ItemTemplate>
                               </asp:TemplateField>
                        <asp:BoundField HeaderText="Nombre" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="zon_nombre" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar E-Mail" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="zon_enviar_mail" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar SMS" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="zon_enviar_sms" ></asp:BoundField>
                       <asp:HyperLinkField DataNavigateUrlFields="zon_id" HeaderStyle-Width="5%" DataNavigateUrlFormatString="pAlarmasZonas.aspx?zon_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("zon_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                            
                       </Columns>
        <EmptyDataTemplate>No Existen Alertas Configuradas</EmptyDataTemplate>
        </asp:GridView>    
                     <br />
                     <asp:Button ID="Button2" runat="server" Text="Configurar Nueva" PostBackUrl="~/Panel_Control/pAlarmasZonas.aspx" />
</div>
    </asp:Panel> 
    </div>
     </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>