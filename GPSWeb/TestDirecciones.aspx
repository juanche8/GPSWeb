<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestDirecciones.aspx.vb" Inherits="GPSWeb.TestDirecciones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Label ID="Label1" runat="server" Text="latitud"></asp:Label>
        <asp:TextBox ID="txtLatitud" runat="server"></asp:TextBox>
       
        <br />
       
    <asp:Label ID="Label2" runat="server" Text="longitud"></asp:Label>
        <asp:TextBox ID="txtLongitud" runat="server"></asp:TextBox>
        &nbsp;
        <asp:Button ID="Button1" runat="server" Text="Buscar en OSM" />
                &nbsp;<asp:Button ID="Button2" runat="server" Text="Buscar en Google" />
        
        <br />
        <br />
     <asp:Label ID="Label3" runat="server" Text="Direccion"></asp:Label>
        <asp:TextBox ID="txtDireccion" runat="server" Width="639px"></asp:TextBox>
        <br />
        <asp:Button ID="Button3" runat="server" Text="Button" />
        <br />
         <asp:Label ID="lblResultado" runat="server" Text="" Font-Bold="true"></asp:Label>
    </div>
    </form>
</body>
</html>
