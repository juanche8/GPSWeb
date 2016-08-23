<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaSensorAdd.aspx.vb" Inherits="GPSWeb.pAlarmaSensorAdd" MasterPageFile="~/Panel_Control/SiteMaster.Master"%>


 <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" /> 
     <asp:HiddenField ID="hdncat_id" runat="server" Value="0" /> 
 <div style="float: left; width:100%; height:100%;">
  <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Alarmas de Sensores</h3>
 <h5>Seleccione el Móvil al cual quiere configurar los Sensores. Marque en Avisarme las alarmas que quiere recibir.</h5>
 </div>
     <div style="text-align:center;">
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     </div> 
 <div style="width:40%; margin:0 0 0 350px;">
      <!--litado vehiculos-->
   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   
        
     <asp:DropDownList ID="ddlMoviles" runat="server" AutoPostBack="true" 
          Height="31px" Width="150px">
     </asp:DropDownList>
    &nbsp;       
     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Seleccione un Móvil" ControlToValidate="ddlMoviles" InitialValue="0"></asp:RequiredFieldValidator>
      <br />
     <br />         
     </div>
 <div>
  <asp:DataGrid ID="datagridAlarmas" DataKeyField="sen_id" runat="server"  AutoGenerateColumns="False" Width="70%" HorizontalAlign="center" RowStyle-Font-Size="9"
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
     </div> 
     <asp:Panel ID="PanelTildar" runat="server" Visible="false">
      <div style="width:50%; margin-left:20%;"> <br /><asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton><br /><br /></div>
     </asp:Panel>
     
      <div style="text-align:center;">
      <asp:Button ID="Button1" runat="server" Text="Volver" CssClass="button2" 
              CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" 
              Font-Size="14px" Width="130px" Height="35px" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" 
              ValidationGroup="alerta" CssClass="button2" Font-Size="14px" Width="160px" Height="35px"/>
      </div>       
 </div>
</asp:Content>
