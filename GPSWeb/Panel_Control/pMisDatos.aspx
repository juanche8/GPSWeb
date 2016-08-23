<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pMisDatos.aspx.vb" Inherits="GPSWeb.pMisDatos" MasterPageFile="~/Panel_Control/SiteMaster.Master" Culture="Auto" UICulture="Auto"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdncli_id" runat="server" />
   
 <div style="float: left; width: 80%; height: 700px">
 <div style="margin-left:30px;">
 <div style="margin-left:50px; width:90%;height:auto;">
 <h3> <asp:Label ID="lbltitulo" runat="server" Text="Mis Datos"></asp:Label> 
 </h3>
 <hr style="border: 1px solid #F58634; height: 2px;"/>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
 </div>
   
   <div style="margin-left:50px; width:95%;z-index:0;">

 <table style="font-size:12px; font-weight:bold;" cellspacing="5" cellpadding="5">
    <tr>
    <td>
    <asp:Label ID="Label2" runat="server" Text="Nombre:" Font-Bold="true"></asp:Label><br />      
        <asp:TextBox ID="txtNombre" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
    </td>
    <td><asp:Label ID="Label22" runat="server" Text="Apellido:" Font-Bold="true"></asp:Label><br />   
                    <asp:TextBox ID="txtApellido" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtApellido"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td><asp:Label ID="Label25" runat="server" Text="Nro. Documento:" Font-Bold="true"></asp:Label><br />   
                             <asp:TextBox ID="txtDni" runat="server" Width="100px" MaxLength="50"></asp:TextBox></td>
<td><asp:Label ID="Label3" runat="server" Text="Dirección:" Font-Bold="true"></asp:Label><br />  
      <asp:TextBox ID="txtDireccion" runat="server" Width="301px" MaxLength="100"></asp:TextBox></td>
</tr>
<tr>
<td><asp:Label ID="Label26" runat="server" Text="Fecha Nacimiento:" Font-Bold="true"></asp:Label><br />     
                             <asp:TextBox ID="txtFechaNac" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                              <ajaxtoolkit:calendarextender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                                     TargetControlID="txtFechaNac" PopupButtonID="txtFechaNac"/>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El formato de fecha es invalido"
                                                        ControlToValidate="txtFechaNac" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator></td>
<td><asp:Label ID="Label28" runat="server" Text="Código Postal:" Font-Bold="true"></asp:Label></br>     
                             <asp:TextBox ID="txtCP" runat="server" Width="100px" MaxLength="100"></asp:TextBox></td>
</tr>
<tr>
<td><asp:Label ID="Label4" runat="server" Text="Telefono Primario" Font-Bold="true"></asp:Label><br />      
        <asp:TextBox ID="txtTelefono" runat="server" Width="100px" MaxLength="50"></asp:TextBox></td>
<td><asp:Label ID="Label9" runat="server" Text="E-Mail Primario (User Login)" Font-Bold="true"></asp:Label><br />
   <asp:TextBox ID="txtMail" runat="server" Width="301px" MaxLength="50" Enabled="false"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtMail"></asp:RequiredFieldValidator>
          <asp:RegularExpressionValidator ID="regularExpressionValidator1" runat="server" ErrorMessage="Formato Mail Incorrecto" SkinID="regularexpressionvalidator" Display="Static" ControlToValidate="txtMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
</tr>
<tr>
<td colspan="2"> <asp:Label ID="Label11" runat="server" Text="Contraseña" Font-Bold="true"></asp:Label><br />
     <asp:TextBox ID="txtContrasenia" runat="server" Width="100px" MaxLength="50"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtContrasenia"></asp:RequiredFieldValidator></td>

</tr>
<tr style="text-align:right;"><td colspan="2">    &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Grabar" CssClass="button2" /></td></tr>
</table>
     

 

 </div>
 </div>                  
 </div>
 
</asp:Content>