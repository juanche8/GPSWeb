<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmasSMS.aspx.vb" Inherits="GPSWeb.pAlarmasSMS" MasterPageFile="~/CMS/SitePages.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
   
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 80%; height: 700px">
    <asp:Label ID="Label2" runat="server" Text="CONFIGURAR ENVIO DE SMS ANTE ALERTAS DE SENSORES" Font-Bold="true" Font-Size="16px"></asp:Label>
   
    <br />
     <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="12px" ForeColor="Red"></asp:Label>
    

 <div>
  <div style="width:80%; margin:0 0 0 150px;">
      <!--litado vehiculos-->
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:Label ID="Label1" runat="server" Text="Seleccione Vehiculo para configurar el envio de SMS:" Font-Bold="true"></asp:Label>   
      <br />
      <br />
      <asp:Label ID="Label3" runat="server" Text="Cliente:"></asp:Label> 
       &nbsp;&nbsp; 
       <asp:DropDownList ID="ddlCliente" runat="server" AutoPostBack="true" DataTextField="cli_nombre" DataValueField="cli_id">
     </asp:DropDownList>
       &nbsp;&nbsp;&nbsp;&nbsp;
       <asp:Label ID="Label4" runat="server" Text="Vehiculo"></asp:Label>   
      <asp:DropDownList ID="ddlMoviles" runat="server" AutoPostBack="true">
     </asp:DropDownList>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Seleccione un Movil" ControlToValidate="ddlMoviles" InitialValue="0"></asp:RequiredFieldValidator>
     <br />         
     </div>
     <br />
 <asp:DataGrid ID="datagridAlarmas" DataKeyField="sen_id" runat="server"  AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnItemDataBound="datagrid_itemDataBound" >
                        <Columns>
                       
                            <asp:BoundColumn HeaderText="Sensores" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="left"  DataField="sen_nombre" >
                                <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
                                
                            </asp:BoundColumn>
                          
                           <asp:TemplateColumn HeaderText="Enviar SMS" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkEnviar" runat="server"/>
                               </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn>                         
                            
                       </Columns>
                     </asp:DataGrid>
                     <br /><br />
     </div> 
      <div style="text-align:center;">
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/CMS/pAdminAlarmas.aspx" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" ValidationGroup="alerta" />
      </div>    
    
 </div>
     

</asp:Content>