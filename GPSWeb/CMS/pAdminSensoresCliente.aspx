<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminSensoresCliente.aspx.vb" Inherits="GPSWeb.pAdminSensores" MasterPageFile="~/CMS/SitePages.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdnveh_id" runat="server" />
   
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 80%; height: 700px">
    <asp:Label ID="Label2" runat="server" Text="CONFIGURAR SENSORES PARA EL MOVIL:" Font-Bold="true" Font-Size="16px"></asp:Label>
     <asp:Label ID="lblCliente" runat="server" Text="" Font-Size="16px"></asp:Label>
    <br />
     <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="12px" ForeColor="Red"></asp:Label>
    

 <div>
     <br />
 <asp:DataGrid ID="datagridSensores" DataKeyField="sen_id" runat="server"  AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnItemDataBound="datagrid_itemDataBound" >
                        <Columns>
                       
                            <asp:BoundColumn HeaderText="Sensor" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="left"  DataField="sen_nombre" >
                                <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
                                
                            </asp:BoundColumn>
                          
                           <asp:TemplateColumn HeaderText="Habilitar" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAvisarme" runat="server" />
                               </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn>   
                            <asp:TemplateColumn HeaderText="Notificación p/SMS" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSMS" runat="server" />
                               </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn>                         
                            
                       </Columns>
                     </asp:DataGrid>
                     <br /><br />
     </div> 
      <div style="text-align:center;">
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/CMS/pAdminVehiculos.aspx" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" ValidationGroup="alerta" />
      </div>    
    
 </div>
     
                   
 </div>
</asp:Content>