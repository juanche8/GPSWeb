<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminMoviles.aspx.vb" Inherits="GPSWeb.pAdminMoviles" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
   
 <div style="float: left; width: 80%; height: 700px">
 <div style="margin-left:30px;">
 <div style="margin-left:50px; width:90%;height:auto;">
 <h3> <asp:Label ID="lbltitulo" runat="server" Text="Agregar Vehiculo"></asp:Label> 
 </h3>
 <hr style="border: 1px solid #F58634; height: 2px;"/>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
 </div>
   
   
   <div style="margin-left:50px; width:95%;z-index:0;">
    
    <table style="font-size:12px; font-weight:bold;" cellspacing="5" cellpadding="5">
    <tr>
    <td><asp:Label ID="Label1" runat="server" Text="Patente:"></asp:Label>
        <br />
    <asp:TextBox ID="txtPatente" runat="server" MaxLength="50"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtPatente"></asp:RequiredFieldValidator></td>
         <td><asp:Label ID="Label7" runat="server" Text="Marca:"></asp:Label>      
             <br />
         <asp:TextBox ID="txtMarca" runat="server" MaxLength="50"></asp:TextBox></td>
    </tr>
    <tr>
    <td><asp:Label ID="Label8" runat="server" Text="Modelo:"></asp:Label><br />
    <asp:TextBox ID="txtModelo" runat="server" MaxLength="50" ></asp:TextBox></td>
    <td><asp:Label ID="Label9" runat="server" Text="Color:"></asp:Label><br />
    <asp:TextBox ID="txtColor" runat="server" MaxLength="50"></asp:TextBox></td>
    </tr>
    <tr>
    <td><asp:Label ID="Label2" runat="server" Text="Nombre del Vehiculo:"></asp:Label><br />
    <asp:TextBox ID="txtNombre" runat="server" MaxLength="50"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtNombre"></asp:RequiredFieldValidator></td>
    <td><asp:Label ID="Label3" runat="server" Text="Nombre del Conductor:"></asp:Label><br />
    <asp:TextBox ID="txtConductor" runat="server" MaxLength="50"></asp:TextBox></td>
    </tr>
    <tr>
    <td><asp:Label ID="Label4" runat="server" Text="Tipo Vehiculo"></asp:Label><br/>
             <asp:DropDownList ID="ddlTipoMovil" runat="server" DataTextField="veh_tipo_detalle" DataValueField="veh_tipo_id" Enabled="false">
             </asp:DropDownList></td>
    <td><asp:Label ID="Label10" runat="server" Text="Tipo Uso"></asp:Label><br />
             <asp:DropDownList ID="ddlTipoUso" runat="server" DataTextField="tipo_uso_descripcion" DataValueField="tipo_uso_id">
             </asp:DropDownList></td>
    </tr>
    <tr>
    <td colspan="2"><asp:Label ID="Label11" runat="server" Text="Kilometros Iniciales:"></asp:Label><br />
    <asp:TextBox ID="txtKilometros" runat="server" MaxLength="9" Enabled="false"></asp:TextBox>
           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingrese solo Números." ValidationExpression="^\d+$" ControlToValidate="txtKilometros"></asp:RegularExpressionValidator></td>
    
    </tr>
    <tr>
    <td colspan="2"><asp:Label ID="Label5" runat="server" Text="Imagen:"></asp:Label><br/>  
       
             <asp:ImageButton ID="imgMovil" runat="server" Height="96px" Width="133px" ImageUrl="~/images/no-image.png" /></td>
    
    </tr>
    <tr><td colspan="2">
     <asp:Label ID="Label6" runat="server" Text="Seleccionar Nueva Imagen:"></asp:Label>&nbsp;
             <asp:FileUpload ID="FileUpload1" runat="server" />
    </td></tr>
    <tr>
    <td colspan="2" style="text-align:right;">  <asp:Button ID="Button2" runat="server" Text="Volver" PostBackUrl="~/Panel_Control/pMisMoviles.aspx" CausesValidation="false" CssClass="button2" />
     &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2"/></td>
    </tr>
    </table>
    
 </div> 
</div>                  
 </div> 
</asp:Content>