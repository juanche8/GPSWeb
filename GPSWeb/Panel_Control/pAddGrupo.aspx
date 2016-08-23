<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAddGrupo.aspx.vb" Inherits="GPSWeb.pAddGrupo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Grupos</title>
 <link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
    <!-- The jQuery UI theme extension jqGrid needs -->
     <link href="../css/azul/jquery-ui.css" rel="stylesheet" type="text/css" />
   
      <link rel="stylesheet" href="../css/main.css" type="text/css" />
     <script src="../scripts/jquery-1.7.2.js" type="text/javascript"></script>
      <script src="../scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../scripts/ui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../scripts/ui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <script src="../scripts/ui/jquery.ui.position.min.js" type="text/javascript"></script>
    <link href="../css/layout.css" rel="stylesheet" type="text/css" media="screen" />
<script type="text/javascript">
    function seleccionarTodos() {
        var chkListaTipoModificaciones = document.getElementById('<%= chkMoviles.ClientID %>');
        var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
        for (var i = 0; i < chkLista.length; i++) {
            chkLista[i].checked = true;
        }
    }

    function DeseleccionarTodos() {
        var chkListaTipoModificaciones = document.getElementById('<%= chkMoviles.ClientID %>');
        var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
        for (var i = 0; i < chkLista.length; i++) {
            chkLista[i].checked = false;
        }
    }

    function verifyCheckboxList(source, arguments) {
        var chkListaTipoModificaciones = document.getElementById('<%= chkMoviles.ClientID %>');
        var chkLista = chkListaTipoModificaciones.getElementsByTagName("input");
        for (var i = 0; i < chkLista.length; i++) {
            if (chkLista[i].checked) {
                args.IsValid = true;
                return;
            }
        }
        arguments.IsValid = false;
    }

   
   
 </script>
 </head>
<body style="width:400px; height: auto;">
    <form id="form1" runat="server" defaultbutton="">

    
 <div> 
 <asp:HiddenField ID="hdngrup_id" runat="server" Value="0" /> 
 <asp:HiddenField ID="hdncli_id" runat="server" /> 
    <asp:Label ID="lblTitulo" runat="server" Text="CREAR NUEVO GRUPO DE MOVILES " Font-Bold="true" Font-Size="16px"></asp:Label>
    <br />
     <br />
     <div style="text-align:center;">
      <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     </div> 
 <div style="width:100%; margin-left:5px;">
  <asp:Label ID="Label3" runat="server" Text="Ingrese un Nombre para el Grupo:" Font-Bold="true"></asp:Label>  
     <br />
     <asp:TextBox ID="txtnombre" runat="server" Width="258px" MaxLength="50"></asp:TextBox>
     <asp:RequiredFieldValidator
         ID="RequiredFieldValidator1" runat="server" ErrorMessage="Ingrese el Nombre" ControlToValidate="txtnombre"></asp:RequiredFieldValidator>
      <br />
     <br />
      <!--litado vehiculos-->
     
      <asp:Label ID="Label1" runat="server" Text="Seleccione los Vehiculos que van a pertenecer al Grupo:" Font-Bold="true"></asp:Label>   
         <br />
         <br />
     <asp:CheckBoxList ID="chkMoviles" runat="server" DataTextField="veh_patente" DataValueField="veh_id" RepeatDirection="Horizontal" RepeatColumns="2" >
     </asp:CheckBoxList>
     <asp:CustomValidator
          OnServerValidate="valInquiry_ServerValidation"
          ID="valInquiry"
          EnableClientScript="true"
          ClientValidationFunction="verifyCheckboxList"
          ErrorMessage="Seleccione el/los Vehiculos"
          runat="server"/><br />       
       <asp:Panel ID="PanelTildar" runat="server">
          <a href="#" onclick="seleccionarTodos();" style="color:#F58634; font-size:12px;">Tildar Todos</a>
          <a href="#" onclick="DeseleccionarTodos();" style="color:#F58634; font-size:12px;"> / Destildar Todos</a>
      </asp:Panel>
     <br />         
     </div>
 <div>
                     <br />
                      
     <br /><br />
     </div> 
      <div style="text-align:center;">
      <asp:Button ID="Button1" runat="server" Text="Cerrar" CausesValidation="false" OnClientClick="window.parent.cerrar();" CssClass="button2" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="button2" />
      </div>     
      <asp:literal ID="Literal1" runat="server"></asp:literal>  
 </div>
    
    </form>   
</body></html>

