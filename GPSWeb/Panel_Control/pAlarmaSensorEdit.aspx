<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaSensorEdit.aspx.vb" Inherits="GPSWeb.pAlarmaSensorEdit" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>


 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" /> 
    <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
 <div style="float: left; width:100%; height:100%;">
  <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Alarmas de Sensores</h3>
 </div>
 
     <div style="text-align:center;">
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     </div> 
 <div style="width:30%; margin:0 0 0 350px;">
      <!--litado vehiculos-->
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:Label ID="Label1" runat="server" Text="Configuración para el Móvil:" Font-Bold="true" Font-Size="14px" ForeColor="#D85639"></asp:Label>   
        &nbsp;<asp:Label ID="lblMovil" runat="server" Text="" Font-Bold="true" Font-Size="14px"  ForeColor="#D85639"></asp:Label>
    <br />       
    
     <br />         
     </div>
 <div>
  <asp:DataGrid ID="datagridAlarmas" DataKeyField="sen_id" runat="server"  AutoGenerateColumns="False" Width="60%" HorizontalAlign="center" OnItemDataBound="datagrid_itemDataBound" RowStyle-Font-Size="9"
 HeaderStyle-Font-Size="10" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical">
                        <Columns>
                         <asp:TemplateColumn HeaderText="Avisarme" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAvisarme" runat="server"/>
                               </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="Sensor" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="left"  DataField="sen_nombre" >
                                <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
                                
                            </asp:BoundColumn>
                          
                            
                             <asp:TemplateColumn HeaderText="Enviar e-mail" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                                     
                                    <asp:CheckBox ID="chkMail" runat="server" Text="Si"/>
                               </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn>                         
                            
                       </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="#343535" Font-Bold="True" ForeColor="White" />
                        <ItemStyle BackColor="White" />
                        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" 
                            Mode="NumericPages" />
                        <SelectedItemStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                     </asp:DataGrid>
                     <br />
                      <div style="width:50%; margin-left:20%;"> <br /><asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton><br /><br /></div>
                      
     <br />
     </div> 
      <div style="text-align:center;">
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" CssClass="button2" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" Font-Size="14px" Width="130px" Height="35px" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" CssClass="button2" ValidationGroup="alerta" Font-Size="14px" Width="160px" Height="35px" />
      </div>       
 </div>
</asp:Content>
