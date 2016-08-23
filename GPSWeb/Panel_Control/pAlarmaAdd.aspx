<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaAdd.aspx.vb" Inherits="GPSWeb.pAlarmaAdd" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">  
    <ContentTemplate>
    <asp:HiddenField ID="hdncli_id" runat="server" /> 
     <asp:HiddenField ID="hdncat_id" runat="server" Value="0" /> 
<div style="float: left; width:100%; height:100%;">
  <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Alarmas de Excesos de Velocidad</h3>
 <h5>Seleccione el Móvil para definir los valores de velocidades máximas para cada tipo de Vía. Marque en Avisarme las alarmas que quiere recibir.</h5>
 </div>
     <div style="text-align:center;">
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     </div> 
 <div style="width:60%; margin:0 0 0 250px;font-size:12px; font-family:Arial; ">
      <!--litado vehiculos-->
     <asp:Label ID="lblMovil" runat="server" Text="Moviles:"></asp:Label>
     <asp:Panel ID="PanelGrupo" runat="server">
       <asp:Label ID="Label4" runat="server" Text="Filtrar por Grupo:" Font-Bold="true"></asp:Label>   
         &nbsp;<asp:DropDownList ID="ddlgrupo" runat="server" AutoPostBack="true" DataTextField="grup_nombre" DataValueField="grup_id">
         </asp:DropDownList>
         &nbsp;&nbsp; 
         <asp:CheckBox ID="chkTodos" runat="server" Text="Ver todos los Moviles" Font-Bold="true" AutoPostBack="true" />
         <br />
     </asp:Panel>
     <asp:Panel ID="PanelMoviles" runat="server" Visible="false">
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
          <div style="height:200px;overflow-y: scroll; font-family:Arial; width:320px; border-color:LightGray; border-width:1px; border-style:solid;"><br />
                  <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="8" CellPadding="5"  Font-Names="Arial" Width="300px">
           
             <ItemTemplate>           
              <asp:CheckBox ID="chkMoviles" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
          
     <div><br /> <asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton></div>
         
      
     
      </asp:Panel>  
      <br />       
     </div>
 <div>
  <asp:DataGrid ID="datagridAlarmas" DataKeyField="vel_id" runat="server"  AutoGenerateColumns="False" Width="80%" HorizontalAlign="center" OnItemDataBound="grid_DataBound" RowStyle-Font-Size="9"
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
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" CssClass="button2" Width="150px" Height="30px" Font-Size="14px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" ValidationGroup="alerta" CssClass="button2" Width="160px" Height="30px" Font-Size="14px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif"/>
      </div>       
 </div>
 </ContentTemplate>
  </asp:UpdatePanel>    
</asp:Content>
