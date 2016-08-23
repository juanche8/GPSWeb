<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pVehiculos.aspx.vb" Inherits="GPSWeb.pVehiculos" MasterPageFile="~/CMS/SitePages.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
   
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 80%; height: 700px">
  <div style="margin-left:30px;">
    <asp:Label ID="lbltitulo" runat="server" Text="Administrar Vehiculos" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br /> 
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="16px"></asp:Label>  
      <br />
 <div>
     <br />
     <div>
         <div><asp:Label ID="Label9" runat="server" Text="Cliente:"></asp:Label></div>      
         <div><asp:DropDownList ID="ddlCliente" runat="server" DataTextField="cli_nombre" DataValueField="cli_id">
             </asp:DropDownList></div>      
   </div>
  <div>
         <div><asp:Label ID="Label1" runat="server" Text="Patente:"></asp:Label></div>      
         <div><asp:TextBox ID="txtPatente" runat="server" Width="211px" MaxLength="50"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtPatente"></asp:RequiredFieldValidator>
         </div>      
   </div>
    <div>
         <div><asp:Label ID="Label7" runat="server" Text="Marca:"></asp:Label></div>      
         <div><asp:TextBox ID="txtMarca" runat="server" Width="211px" MaxLength="50"></asp:TextBox></div>      
   </div>
    <div>
         <div><asp:Label ID="Label8" runat="server" Text="Modelo:"></asp:Label></div>      
         <div><asp:TextBox ID="txtModelo" runat="server" Width="211px" MaxLength="50"></asp:TextBox></div>      
   </div>
     <div>
         <div><asp:Label ID="Label10" runat="server" Text="Color:"></asp:Label></div>      
         <div><asp:TextBox ID="txtColor" runat="server" Width="211px" MaxLength="50"></asp:TextBox></div>      
   </div>
   <div>
         <div><asp:Label ID="Label2" runat="server" Text="Nombre del Vehiculo:"></asp:Label></div>      
         <div><asp:TextBox ID="txtNombre" runat="server" Width="211px" MaxLength="50"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
         </div>      
   </div>
   <div>
         <div><asp:Label ID="Label3" runat="server" Text="Nombre del Conductor:"></asp:Label></div>      
         <div><asp:TextBox ID="txtConductor" runat="server" Width="211px" MaxLength="100"></asp:TextBox></div>      
   </div>
   <div>
         <div><asp:Label ID="Label4" runat="server" Text="Tipo Vehiculo"></asp:Label></div>      
         <div>
             <asp:DropDownList ID="ddlTipoMovil" runat="server" DataTextField="veh_tipo_detalle" DataValueField="veh_tipo_id">
             </asp:DropDownList>
             <br />
             <br />
         </div>      
   </div>
   <div>
         <div><asp:Label ID="Label11" runat="server" Text="Tipo Uso"></asp:Label></div>      
         <div>
             <asp:DropDownList ID="ddlTipoUso" runat="server" DataTextField="tipo_uso_descripcion" DataValueField="tipo_uso_id">
             </asp:DropDownList>
             <br />
             <br />
         </div>      
      
   </div>
    <div>
         <div><asp:Label ID="Label12" runat="server" Text="Kilometros Iniciales:"></asp:Label></div>      
         <div><asp:TextBox ID="txtKilometros" runat="server" MaxLength="9"></asp:TextBox> 
         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtKilometros"></asp:RegularExpressionValidator>
         </div>      
   </div>
     <div> <br />
                             <div><asp:Label ID="Label29" runat="server" Text="Modulo con Sensores" Font-Bold="true"></asp:Label></div>
                          <div>
                           <asp:RadioButtonList ID="rdnModuloSensor" runat="server" RepeatDirection="Horizontal">
                                     <asp:ListItem Value="True" Selected="True">Si</asp:ListItem>
                                     <asp:ListItem Value="False">No</asp:ListItem>
                                 </asp:RadioButtonList>
                           </div>      
                       </div>  
    <div>
         <div><asp:Label ID="Label5" runat="server" Text="Id Modulo Asociado:"></asp:Label></div>      
         <div>
             <asp:DropDownList ID="ddlModulo" runat="server" DataTextField="mod_numero" DataValueField="mod_numero"></asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Seleccionar Modulo" ControlToValidate="ddlModulo" InitialValue="0"></asp:RequiredFieldValidator>
             <br />
         </div>      
   </div>
   <div>
         <div><asp:Label ID="Label6" runat="server" Text="Activo"></asp:Label></div>      
         <div>          
             <asp:RadioButtonList ID="rdnActivo" runat="server" RepeatDirection="Horizontal">
                 <asp:ListItem Value="True" Selected="True">Si</asp:ListItem>
                 <asp:ListItem Value="false">No</asp:ListItem>
             </asp:RadioButtonList>
            </div>      
   </div>
     <br />
     <asp:Button ID="Button2" runat="server" Text="Volver" PostBackUrl="~/CMS/pAdminVehiculos.aspx" CausesValidation="false" />
     &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Grabar" style="height: 26px" />

 </div>
 </div>               
 </div>
 
</asp:Content>
