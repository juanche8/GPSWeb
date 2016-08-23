<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pMisMoviles.aspx.vb" Inherits="GPSWeb.pMisMoviles" MasterPageFile="~/Panel_Control/SiteMaster.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(function () {

        $('#<%=gridMoviles.ClientID %> img').click(function () {

            var img = $(this)
            var orderid = $(this).attr('orderid');

            var tr = $('#<%=gridMoviles.ClientID %> tr[orderid =' + orderid + ']')
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
 <div style="width: 100%; height:100%;">
 <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Mis Vehiculos</h3>
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
    </div>
     <div style="margin-left:40px; width:95%; vertical-align:middle;">
      <asp:Label ID="Label3" runat="server" Text="Patente: " Font-Size="14px" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtPantente" runat="server" Width="86px"></asp:TextBox>
     &nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Marca: " Font-Size="14px" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtMarca" runat="server" Width="154px"></asp:TextBox>
    &nbsp;&nbsp;<asp:Label ID="Label4" runat="server" Text="Modelo: " Font-Size="14px" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtModelo" runat="server" Width="154px"></asp:TextBox>
         &nbsp;&nbsp;
        &nbsp;&nbsp;<asp:Label ID="Label5" runat="server" Text="Conductor: " Font-Size="14px" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtConductor" runat="server" Width="154px"></asp:TextBox>
         &nbsp;&nbsp;
         <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="button2" 
             Width="99px"/>
      
          &nbsp;&nbsp;<asp:HyperLink ID="HyperLink1" Font-Bold="true" ForeColor="#D85639" Font-Size="14px" runat="server" Font-Names="Helvetica Neue,Helvetica,Arial,sans-serif"  NavigateUrl="~/Panel_Control/pAdminMoviles.aspx">Agregar Nuevo</asp:HyperLink>
     </div>

     <br />
   <div style="margin-left:10px; width:100%;z-index:0;">  
         <asp:GridView ID="gridMoviles" DataKeyField="veh_id" runat="server" 
             AutoGenerateColumns="False" Width="95%" HorizontalAlign="Center" 
                    AllowSorting="True" OnSorting="SortRecords"  AllowPaging="true" PageSize="20"
             OnRowCommand="grid_rowCommand" OnRowDataBound="grid_DataBound" CellPadding="4" 
             EnableModelValidation="True" BackColor="White"  OnPageIndexChanging="gridMoviles_PageIndexChanging"
                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                ForeColor="Black" GridLines="Vertical" Font-Size="11px">
                       <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                          <asp:TemplateField>
                            <ItemTemplate>
                                <img alt="" src="../images/plus.gif" orderid="<%# Eval("veh_id") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:BoundField HeaderText="Patente" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="Patente" SortExpression="Patente" >
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                           
                            <asp:BoundField HeaderText="Activo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"  DataField="Activo" SortExpression="Activo">
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Total Kms." HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="kms" SortExpression="kms">
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                                <asp:BoundField HeaderText="Ultimo Reporte" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left"  DataField="fecha" SortExpression="fecha" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" >
<HeaderStyle Width="15%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Ubicación Actual" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Left"  DataField="ubicacion" SortExpression="ubicacion">
<HeaderStyle Width="40%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Veloc." HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="velocidad" SortExpression="velocidad">
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Temp." HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="temp" SortExpression="temp">
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField> 
                               <asp:BoundField HeaderText="RPM." HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="rpm" SortExpression="rpm">
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField> 
                 <asp:BoundField HeaderText="Encendido" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="encendido" SortExpression="encendido">
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField> 
             <asp:BoundField HeaderText="Asiento" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left"  DataField="libre" SortExpression="libre">
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField> 
                              <asp:HyperLinkField DataNavigateUrlFields="veh_id" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" 
                                DataNavigateUrlFormatString="pAdminMoviles.aspx?veh_id={0}" HeaderText="Editar" 
                                Text="<img src='../images/edit.gif' border='0'>" >
<HeaderStyle Width="5%" Font-Bold="False"></HeaderStyle>
                            </asp:HyperLinkField>
                            <asp:TemplateField HeaderText="Corriente" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonCorriente" runat="server" CommandName="CortarCorriente" CommandArgument='<%# Eval("veh_id")%>' CausesValidation="false" ImageUrl="~/images/electricidad_no.jpg" Width="22px" Height="22px" ToolTip="Cortar Electricidad" OnClientClick="return confirm('Esta Seguro de Cortar la Corriente del Vehiculo Seleccionado?');"/>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="5%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                           
                             <asp:TemplateField>
                    <ItemTemplate>
                        <tr style="display:none;" orderid="<%# Eval("veh_id") %>">
                            <td colspan="100%">
                                <div style="position:relative;left:5px;">
                                
                                    <asp:GridView ID="gridDatos" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="center" BackColor="White"  
                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                ForeColor="Black" GridLines="Vertical" Font-Size="12px">
                       <AlternatingRowStyle BackColor="#CCCCCC" />
                                        <Columns>                                            
                                             <asp:BoundField HeaderText="Marca" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="Marca" SortExpression="Marca">
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Modelo" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="Modelo" SortExpression="Modelo">
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Color" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" DataField="Color" SortExpression="Color">
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                             <asp:BoundField HeaderText="Tipo Vehiculo" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Left"  DataField="Tipo_Movil" SortExpression="Tipo_Movil">
<HeaderStyle Width="7%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                              <asp:BoundField HeaderText="Tipo Uso" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Left"  DataField="Tipo_Uso" SortExpression="Tipo_Uso">
<HeaderStyle Width="7%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Conductor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="Conductor" SortExpression="Conductor">
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                                        </Columns>
                                          <FooterStyle BackColor="#CCCCCC" />
                      <HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
                        <PagerStyle BackColor="#343535" ForeColor="White" HorizontalAlign="Center" Font-Bold="true" Font-Size="12px" Font-Underline="true" />
                      <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                   
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:TemplateField>
                          </Columns>
                      
                         <FooterStyle BackColor="#CCCCCC" />
                      <HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
                        <PagerStyle BackColor="#343535" ForeColor="White" HorizontalAlign="Center" Font-Bold="true" Font-Size="12px" Font-Underline="true" />
                      <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                     </asp:GridView>    
                     </div> 
 </div>
</asp:Content>