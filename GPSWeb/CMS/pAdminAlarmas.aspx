<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminAlarmas.aspx.vb" Inherits="GPSWeb.pAdminAlarmas1" MasterPageFile="~/CMS/SitePages.Master"  uiCulture="es" culture="es-AR"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />    
 <div class="inline" style="float: left; width: 100%;">
  <h3>Alarmas Reportadas por Cliente/Movil</h3>
  
        <br />
          <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     <br />
      
     <div style=" font-family:Arial; vertical-align:middle; font-size:11px; font-weight:bold;">
     <div style="width:50%; margin-left:30px;"><a  href="pAlarmasSMS.aspx" style="color:#D85639; font-size:12px; font-weight:bold;">Configurar Envio SMS</a></div>
     <br />
        
          &nbsp;
   <asp:Label ID="Label1" runat="server" Text="Cliente: " Font-Size="12px"></asp:Label>
   <asp:DropDownList ID="ddlCliente" runat="server" DataValueField="cli_id" DataTextField="cli_nombre" AutoPostBack="true">
    </asp:DropDownList>
   &nbsp;
   <asp:Label ID="Label5" runat="server" Text="Movil: " Font-Size="12px"></asp:Label>
  <asp:DropDownList ID="ddlMovil" runat="server" DataTextField="veh_patente" DataValueField="veh_id">
    </asp:DropDownList>
   &nbsp;
   <asp:Label ID="Label7" runat="server" Text="Fechas: " Font-Size="12px"></asp:Label>
    <asp:TextBox ID="txtfechaDesde" runat="server" Width="86px"></asp:TextBox>
       <ajaxtoolkit:calendarextender ID="CalendarExtender1" runat="server" 
             TargetControlID="txtfechaDesde" PopupButtonID="txtfechaDesde"/>
    &nbsp;<asp:TextBox ID="txtfechaHasta" runat="server" Width="86px"></asp:TextBox>
       &nbsp;
       <ajaxtoolkit:calendarextender ID="CalendarExtender2" runat="server" 
             TargetControlID="txtfechaHasta" PopupButtonID="txtfechaHasta"/>
         <asp:Button ID="btBuscar" runat="server" Text="Buscar" CssClass="button2" />
        
  
</div>
 <div style="width:100%; height:100%; margin-left:2%">
     <br />
    
      <asp:GridView ID="datagridAlertas" DataKeyField="patente" runat="server" 
             AutoGenerateColumns="False" Width="99%" HorizontalAlign="Left" OnPageIndexChanging="Alarmas_PageIndexChanging" 
                  Font-Size="12px" AllowSorting="true" AllowPaging="true" PageSize="20"                     
 HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" OnSorting="SortRecords">
        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                              <asp:BoundField DataField="Categoria" HeaderStyle-Width="10%" 
                                HeaderText="Categoria" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Alarma" HeaderStyle-Width="20%" HeaderText="Alarma" 
                                ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="20%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cliente" HeaderStyle-Width="15%" 
                                HeaderText="Cliente" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Patente" HeaderStyle-Width="10%" 
                                HeaderText="Patente" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" 
                                HeaderStyle-Width="5%" HeaderText="Fecha" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Hora" DataFormatString="{0:hh:MM:ss}" 
                                HeaderStyle-Width="5%" HeaderText="Hora" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="5%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Ubicacion" HeaderStyle-Width="50%" 
                                HeaderText="Ubicación" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="50%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Valor" HeaderStyle-Width="20%" 
                                HeaderText="Velocidad/Kms" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="20%" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
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
  </ContentTemplate>
         </asp:UpdatePanel>
</asp:Content>