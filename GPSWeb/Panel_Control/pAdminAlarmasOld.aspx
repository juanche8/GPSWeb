<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminAlarmasOld.aspx.vb" Inherits="GPSWeb.pAdminAlarmas" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    
  
    <ContentTemplate>
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <asp:HiddenField ID="hdncat_id" runat="server" />
    
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
   
 <div>
     
       <asp:GridView ID="GridViewGenericas" runat="server" DataKeyField="ale_id"  AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" OnRowCommand="datagridGenerica_RowCommand" >
        <Columns>
           <asp:BoundField HeaderText="Movil" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="movil" ></asp:BoundField>       
                        <asp:BoundField HeaderText="Alarma" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="alarma" ></asp:BoundField>
                         <asp:BoundField HeaderText="Ultimo Cambio" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="ultimo_cambio" ></asp:BoundField>
                          <asp:BoundField HeaderText="Valor Limite" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="valor" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar E-Mail" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="enviar_mail" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar SMS" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="enviar_sms" ></asp:BoundField>
                       <asp:HyperLinkField DataNavigateUrlFields="ale_id" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" DataNavigateUrlFormatString="pAlarmaEdit.aspx?ale_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("ale_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                            
                       </Columns>
        <EmptyDataTemplate>No Existen Alertas Configuradas</EmptyDataTemplate>
        </asp:GridView>
                     <br />
        <asp:Button ID="btnNueva" runat="server" Text="Configurar Nueva" />
     <br /><br />
     </div>
     </asp:Panel>
 
 
    <asp:Panel ID="PanelRecorridos" runat="server" Visible="false">
    <div>
        <asp:GridView ID="datagridRecorridos" runat="server" DataKeyField="rec_id" runat="server"  AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" OnRowCommand="datagridRecorridos_RowCommand" >
        <Columns>
          <asp:TemplateField HeaderText="Patente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                  <%#(Container.DataItem).Vehiculos.veh_descripcion%> - <%#(Container.DataItem).Vehiculos.veh_patente%>
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
                                  <%#(Container.DataItem).Vehiculos.veh_descripcion%> - <%#(Container.DataItem).Vehiculos.veh_patente%>
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
                                 <%#(Container.DataItem).Vehiculos.veh_descripcion%> - <%#(Container.DataItem).Vehiculos.veh_patente%>
                               </ItemTemplate>
                               </asp:TemplateField>
                        <asp:BoundField HeaderText="Zona" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="zon_nombre" ></asp:BoundField>
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
     <asp:Panel ID="PanelAlarmasCliente" runat="server" Visible="false">
    <div>
        <asp:GridView ID="GridViewAlarmasCliente" runat="server" DataKeyField="ale_usu_id" AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" OnRowCommand="GridViewZonas_RowCommand" >
        <Columns>
       <asp:TemplateField HeaderText="Patente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                  <%#(Container.DataItem).Vehiculos.veh_descripcion%> - <%#(Container.DataItem).Vehiculos.veh_patente%>
                               </ItemTemplate>
                               </asp:TemplateField>
                       <asp:TemplateField HeaderText="Patente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Categorias_Alarmas_Clientes.cat_usu_descripcion%>
                               </ItemTemplate>
                               </asp:TemplateField>
                          <asp:TemplateField HeaderText="Unid. Medida" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Categorias_Alarmas_Clientes.cat_usu_unidadmedida%>
                               </ItemTemplate>
                               </asp:TemplateField>
                         <asp:BoundField HeaderText="Valor" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="ale_usu_valor_maximo" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar E-Mail" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="ale_usu_enviar_mail" ></asp:BoundField>
                          <asp:BoundField HeaderText="Notificar SMS" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="ale_usu_enviar_sms" ></asp:BoundField>
                       <asp:HyperLinkField DataNavigateUrlFields="ale_usu_id" HeaderStyle-Width="5%" DataNavigateUrlFormatString="pAlarmasZonas.aspx?ale_usu_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                        <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("ale_usu_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                            
                       </Columns>
        <EmptyDataTemplate>No Existen Alertas Configuradas</EmptyDataTemplate>
        </asp:GridView>    
                     <br />
                     <asp:Button ID="Button4" runat="server" Text="Configurar Nueva" PostBackUrl="~/Panel_Control/pAlarmasClientes.aspx" />
</div>
    </asp:Panel> 
    </div>
     </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>