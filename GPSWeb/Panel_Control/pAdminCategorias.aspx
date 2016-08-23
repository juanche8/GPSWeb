<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminCategorias.aspx.vb" Inherits="GPSWeb.pAdminCategorias" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdncat_usu_id" runat="server" Value="0" />
   
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 80%; height: 700px">
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label><br />
    <asp:Label ID="lbltitulo" runat="server" Text="Agregar Categoria de Alerta" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br />
     <br />
 <div>
     <br />
  <div>         <div><asp:Label ID="Label1" runat="server" Text="Nombre:"></asp:Label></div>      
         <div><asp:TextBox ID="txtNombre" runat="server" Width="305px" MaxLength="50"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
         </div>      
   </div>
    <div>
         <div><asp:Label ID="Label7" runat="server" Text="Unidad de Medida:"></asp:Label></div>      
         <div><asp:RadioButtonList ID="rdnUnidad" runat="server" RepeatDirection="Horizontal">
             <asp:ListItem Selected="True" Value="Km/h">Velocidad</asp:ListItem>
             <asp:ListItem Value="kms">Kms Recorridos</asp:ListItem>
     </asp:RadioButtonList></div>      
   </div>     
    <div>
    <br />
         <div><asp:Label ID="Label8" runat="server" Text="Valor por Defecto:"></asp:Label></div>      
         <div><asp:TextBox ID="txtValor" runat="server" MaxLength="50"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ControlToValidate="txtValor"></asp:RequiredFieldValidator>
         </div>      
   </div>
     <br />
     <asp:Button ID="Button2" runat="server" Text="Cancelar" PostBackUrl="~/Panel_Control/pAdminCategorias.aspx" CausesValidation="false" />
     &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
 </div>                   
 </div> 
</asp:Content>
