<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminClientes.aspx.vb" Inherits="GPSWeb.pAdminClientes" MasterPageFile="~/CMS/SitePages.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(function() {

    $('#<%=GridClientes.ClientID %> img').click(function() {

            var img = $(this)
            var orderid = $(this).attr('orderid');

            var tr = $('#<%=GridClientes.ClientID %> tr[orderid =' + orderid + ']')
            tr.toggle();

            if (tr.is(':visible'))
                img.attr('src', '../images/minus.gif');
            else
                img.attr('src', '../images/plus.gif');

        });

    });
    
    
    </script>
</asp:Content>
 
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
   
 <div class="inline" style="float: left; width: 80%; height: 100%;  font-family:Arial; vertical-align:middle; font-size:11px;">
     <h3>Clientes</h3>
    <br />
     <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="12px" ForeColor="Red"></asp:Label>
     <br />
     <div>
       <asp:Label ID="Label3" runat="server" Text="Apellido/Nombre: " Font-Size="12px"></asp:Label>
    <asp:TextBox ID="txtNombre" runat="server" Width="154px"></asp:TextBox>
    
    &nbsp;
    
    &nbsp;<asp:Label ID="Label4" runat="server" Text="E-mail: " Font-Size="12px"></asp:Label>
    <asp:TextBox ID="txtMail" runat="server" Width="154px"></asp:TextBox>
         &nbsp;&nbsp;<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="button2" />
          &nbsp;
          <asp:Button ID="btnNuevo" runat="server" Text="Agregar Nuevo" PostBackUrl="~/CMS/pClientes.aspx" CssClass="button2" />
     </div>

 <div style="width:100%; height:100%; margin-left:2%">
     <br />
 
  <asp:GridView ID="GridClientes" DataKeyField="cli_id" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="center" 
                    AllowSorting="true" OnRowCommand="grid_rowCommand" OnRowDataBound="gvCliente_RowDataBound" OnPageIndexChanged="Alarmas_PageIndexChanging"
                  Font-Size="11px" AllowPaging="true" PageSize="20"                     
 HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" OnSorting="SortRecords">
        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                          <asp:TemplateField>
                            <ItemTemplate>
                                <img alt="" src="../images/plus.gif" orderid="<%# Eval("cli_id") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField HeaderText="Apellido" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="cli_apellido" >
<HeaderStyle Width="10%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Nombre" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="cli_nombre" >
<HeaderStyle Width="10%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                             <asp:TemplateField HeaderText="Tipo Cliente" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Tipos_Clientes.tipo_cli_nombre%>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:TemplateField>
                                <asp:BoundField HeaderText="Fecha Alta" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"  DataField="cli_fecha_creacion" DataFormatString="{0:dd/MM/yyyy}" >
<HeaderStyle Width="10%"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Direccion" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Left" DataField="cli_direccion" >
<HeaderStyle Width="25%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="CP" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="cli_cp" >
<HeaderStyle Width="10%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Telefono" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="cli_telefono" >
<HeaderStyle Width="15%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="E-Mail" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="cli_email" >
<HeaderStyle Width="10%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                                    <asp:TemplateField HeaderText="Total Moviles" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Vehiculos.Count%>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:TemplateField>      
                               <asp:BoundField HeaderText="Activo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="cli_activo" >
<HeaderStyle Width="5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                              <asp:HyperLinkField  DataNavigateUrlFields="cli_id" ItemStyle-HorizontalAlign="Center" DataNavigateUrlFormatString="pClientes.aspx?cli_id={0}" HeaderText="Editar" Text="<img src='../images/edit.gif' border='0'>" />
                              <asp:HyperLinkField DataNavigateUrlFields="cli_id" ItemStyle-HorizontalAlign="Center" DataNavigateUrlFormatString="pAdminVehiculos.aspx?cli_id={0}" HeaderText="Moviles" Text="<img src='../images/iconos_movil/autito_gris.png' border='0'>" />
                              <asp:HyperLinkField  DataNavigateUrlFields="cli_id"  ItemStyle-HorizontalAlign="Center" DataNavigateUrlFormatString="pAdminAlarmas.aspx?cli_id={0}" HeaderText="Alarmas" Text="<img src='../images/menu/marcador_alarma.png' border='0' width='20px' height='20px'>" />
                             
                             <asp:TemplateField HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("cli_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Cliente Seleccionado?');"/>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                 <asp:TemplateField>
                    <ItemTemplate>
                        <tr style="display:none;" orderid="<%# Eval("cli_id") %>">
                            <td colspan="100%">
                                <div style="position:relative;left:25px;">
                                 <asp:Label ID="lblTitulo" runat="server" Text="Datos Tipo de Cliente" Font-Size="12px"></asp:Label>
                                    <asp:GridView ID="gvEmpresa" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="center" 
                    AllowSorting="false" OnSorting="SortRecords">                                       
                                        <Columns>                                            
                                            <asp:BoundField DataField="empr_razon_social" HeaderText="Razón Social"  />
                                            <asp:BoundField DataField="empr_CUIT" HeaderText="CUIT" ItemStyle-HorizontalAlign="center" />
                                            <asp:BoundField DataField="empr_Domicilio" HeaderText="Domicilio"  />
                                            <asp:BoundField DataField="empr_CP" HeaderText="CP" ItemStyle-HorizontalAlign="center"  />
                                             <asp:BoundField DataField="empr_telefono_1" HeaderText="Telefono 1" ItemStyle-HorizontalAlign="center"  />
                                              <asp:BoundField DataField="empr_telefono_2" HeaderText="Telefono 2" ItemStyle-HorizontalAlign="center" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvParticular" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="center" 
                    AllowSorting="false">                                       
                                        <Columns>                                            
                                            <asp:BoundField DataField="part_nombre" HeaderText="Nombre"  />
                                            <asp:BoundField DataField="part_apellido" HeaderText="Apellido"  />
                                            <asp:BoundField DataField="part_domicilio" HeaderText="Domicilio"  />                                           
                                             <asp:BoundField DataField="part_telefono_1" HeaderText="Telefono 1" ItemStyle-HorizontalAlign="center" />
                                              <asp:BoundField DataField="part_telefono_2" HeaderText="Telefono 2"  ItemStyle-HorizontalAlign="center"/>
                                              <asp:BoundField DataField="part_telefono_3" HeaderText="Telefono 3"  ItemStyle-HorizontalAlign="center"/>
                                        </Columns>
                                    </asp:GridView>
                                    <br/>
                                     <asp:Label ID="Label1" runat="server" Text="Datosde Contacto:" Font-Size="12px"></asp:Label>
                                     <asp:GridView ID="gvContacto" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="center" 
                    AllowSorting="false">                                       
                                        <Columns>                                            
                                            <asp:BoundField DataField="cont_nombre" HeaderText="Nombre"  />
                                            <asp:BoundField DataField="cont_apellido" HeaderText="Apellido"  />
                                           <asp:BoundField DataField="cont_telefono_1" HeaderText="Telefono 1"  ItemStyle-HorizontalAlign="center"/>
                                                                                     
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
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
    
 </div>
                   
 </div>
</asp:Content>