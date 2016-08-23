<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaEdit.aspx.vb" Inherits="GPSWeb.pAlarmaEdit" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" /> 
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" /> 
<div style="float: left; width:100%; height:100%;">
  <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Alarmas de Excesos de Velocidad</h3>
 <h5>Marque en Avisarme las alarmas que quiere recibir. Puede cambiar en el campo valor por defecto los valores de velocidad permitidos para cada Vía.</h5>
 </div>
   
     <div style="text-align:center;">
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     </div> 
 <div style="width:60%; margin:0 0 0 200px;">
      <!--litado vehiculos-->
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:Label ID="Label1" runat="server" Text="Configuración para el Móvil:" Font-Bold="true" Font-Size="14px"  ForeColor="#D85639"></asp:Label>   
       &nbsp;&nbsp;   
       <asp:Label ID="lblMovil" runat="server" Text="" Font-Bold="true" Font-Size="14px"  ForeColor="#D85639"></asp:Label>   
         <br />
         <br />
     </div>
 <div>
  <asp:DataGrid ID="datagridAlarmas" DataKeyField="vel_id" runat="server"  
         AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center" 
         OnItemDataBound="grid_DataBound" RowStyle-Font-Size="9"
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
                            <asp:BoundColumn HeaderText="Alarma" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="left"  DataField="vel_descripcion" >
                                <HeaderStyle Width="20%" HorizontalAlign="Center"></HeaderStyle>
                                
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                
                            </asp:BoundColumn>
                          
                             <asp:TemplateColumn HeaderText="Valor Por Defecto" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtValor" runat="server" 
                                        Text='<%# Eval("vel_valor_por_defecto")%>' Width="92px"></asp:TextBox><asp:Label ID="Label3" runat="server" Text='<%# Eval("vel_unidadmedida")%>'></asp:Label></ItemTemplate><HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn> 
                            
                             <asp:TemplateColumn HeaderText="Enviar Mail" HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                                   
                                    <asp:CheckBox ID="chkMail" runat="server" Text=""/>
                               </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
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
                      
     <br /><br />
     </div> 
      <div style="text-align:center;">
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false"  PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" CssClass="button2" Width="150px" Height="30px" Font-Size="14px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" ValidationGroup="alerta" CssClass="button2" Width="160px" Height="30px" Font-Size="14px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" />
      </div>       
 </div>
</asp:Content>