<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminVehiculos.aspx.vb" Inherits="GPSWeb.pAdminVehiculos" MasterPageFile="~/CMS/SitePages.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
   
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 85%; height: 100%">
    <asp:Label ID="Label2" runat="server" Text="MOVILES" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br />
     <br />
<div style="text-align:center;">
     
      <asp:Label ID="Label1" runat="server" Text="Cliente:" Font-Bold="true"></asp:Label>
         &nbsp;&nbsp;<asp:DropDownList ID="ddlCliente" runat="server" DataTextField="cli_Nombre" DataValueField="cli_id" >
         </asp:DropDownList>
          &nbsp;
          <asp:Label ID="Label3" runat="server" Text="Patente:" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtPatente" runat="server" Width="100px"></asp:TextBox>
    &nbsp;
     <asp:Label ID="Label4" runat="server" Text="Conductor:" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtConductor" runat="server" Width="100px"></asp:TextBox>
    &nbsp;
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />    &nbsp;<asp:Button ID="Button1" runat="server" Text="Nuevo" />
         <br />
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     </div>
 <div>
     <br />
           <asp:GridView ID="gridviewMoviles" DataKeyField="veh_id" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="center" 
                    AllowSorting="false" OnRowCommand="grid_rowCommand" OnRowDataBound="grid_DataBound" >
                        <Columns>
                         <asp:BoundField HeaderText="Cliente" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="Cliente" ></asp:BoundField>
                       <asp:BoundField HeaderText="Patente" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="Patente" ></asp:BoundField>
                             <asp:BoundField HeaderText="Descrip." HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="Nombre" ></asp:BoundField>
                              <asp:BoundField HeaderText="Tipo Vehiculo" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="Tipo_Movil" ></asp:BoundField>
                             <asp:BoundField HeaderText="Tipo Uso" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="Tipo_Uso" ></asp:BoundField>
                             <asp:BoundField HeaderText="Conductor" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" DataField="Conductor" ></asp:BoundField>
                            <asp:BoundField HeaderText="Id Modulo" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="Modulo" ></asp:BoundField>
                            <asp:BoundField HeaderText="Cant. Alarmas" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="Alarmas" ></asp:BoundField>
                             <asp:BoundField HeaderText="Activo" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"  DataField="Activo" ></asp:BoundField>
                              <asp:BoundField HeaderText="Modulo c/Sensor" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"  DataField="Sensor" ></asp:BoundField>
                              <asp:HyperLinkField  DataNavigateUrlFields="veh_id" DataNavigateUrlFormatString="pVehiculos.aspx?veh_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                            <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("veh_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Vehiculo Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Corriente" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonCorriente" runat="server" CommandName="CortarCorriente" CommandArgument='<%# Eval("veh_id")%>' CausesValidation="false" ImageUrl="~/images/electricidad_no.jpg" Width="22px" Height="22px" ToolTip="Cortar Electricidad" OnClientClick="return confirm('Esta Seguro de Cortar la Corriente del Vehiculo Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateField>
                             <asp:HyperLinkField  DataNavigateUrlFields="veh_id"  ItemStyle-HorizontalAlign="Center" DataNavigateUrlFormatString="pAdminSensoresCliente.aspx?veh_id={0}" HeaderText="Sensores" Text="<img src='../images/sensor.jpg' border='0' width='25px' height='25px'>" />
                       </Columns> 
                        <EmptyDataTemplate>No Existen Vehiculos Creados</EmptyDataTemplate>              
                     </asp:GridView> 
     <br />
 

 </div>
                   
 </div>
</asp:Content>