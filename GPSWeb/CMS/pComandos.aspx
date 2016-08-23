<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pComandos.aspx.vb" Inherits="GPSWeb.pComandos" MasterPageFile="~/CMS/SitePages.Master" Culture="Auto" UICulture="Auto"%>
<%@ Register Assembly="Trirand.Web" TagPrefix="trirand" Namespace="Trirand.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <!-- The jQuery UI theme extension jqGrid needs -->
   <link rel="stylesheet" type="text/css" media="screen" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.16/themes/redmond/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/azul/ui.jqgrid.min.css" /> 
      <script src="../scripts/trirand/i18n/grid.locale-sp.js" type="text/javascript"></script>
    <!-- The jqGrid client-side javascript -->
      <script src="../scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../scripts/ui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../scripts/ui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <script src="../scripts/ui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../scripts/trirand/jquery.jqGrid.min.js" type="text/javascript"></script>   
     <script src="../scripts/trirand/jquery.jqDatePicker.min.js" type="text/javascript"></script>    
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
  
    <ContentTemplate>   
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
   
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 80%; height: auto; font-family:Arial; vertical-align:middle; font-size:11px; margin-left:1%">
  <div style="margin-left:30px;">
    <asp:Label ID="lbltitulo" runat="server" Text="ENVIAR COMANDO A MODULO" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br /> 
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="16px"></asp:Label>  
      <br />
 <div>
 <div> <asp:Label ID="Label1" runat="server" Text="Seleccione él o los Módulos a los que se enviara el comando." Font-Size="14px"></asp:Label>  </div>
     <br />
     <div style="text-align:center;font-size:12px;">
     
      <asp:Label ID="Label3" runat="server" Text="Cliente:" Font-Bold="true"></asp:Label>
         &nbsp;&nbsp;<asp:DropDownList ID="ddlCliente" runat="server" DataTextField="cli_Nombre" DataValueField="cli_id" >
         </asp:DropDownList>
          &nbsp;<asp:Label ID="Label8" runat="server" Text="Movil:" Font-Bold="true"></asp:Label>
         &nbsp;&nbsp;<asp:DropDownList ID="ddlMovil" runat="server" DataTextField="cli_Nombre" DataValueField="cli_id" >
         </asp:DropDownList>
          &nbsp;&nbsp;<asp:Label ID="Label5" runat="server" Text="Patente:" Font-Bold="true"></asp:Label>
    <asp:TextBox ID="txtPatente" runat="server" Width="100px"></asp:TextBox>
    &nbsp;&nbsp;<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="button2" />    &nbsp;<br />
    <asp:Label ID="Label7" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
     </div>
     <asp:Panel ID="PanelModulos" runat="server">
          <asp:CheckBoxList ID="chkMoviles" runat="server" DataTextField="mod_numero" DataValueField="mod_id" RepeatDirection="Horizontal" RepeatColumns="3" >
     </asp:CheckBoxList>
            <br />
            <asp:CustomValidator
          OnServerValidate="valInquiry_ServerValidation"
          ID="valInquiry"
          EnableClientScript="true"
          ClientValidationFunction="verifyCheckboxList"
          ErrorMessage="Seleccione el/los Módulos"
          runat="server"/><br />       
       <asp:Panel ID="PanelTildar" runat="server">
          <a href="#" onclick="seleccionarTodos();" style="color:#ccc000; font-size:12px;">Tildar Todos</a>
          <a href="#" onclick="DeseleccionarTodos();" style="color:#ccc000; font-size:12px;"> / Destildar Todos</a>
      </asp:Panel>
     </asp:Panel>
     <br />
          
          <div>
         <div><asp:Label ID="Label11" runat="server" Text="Comando:" Font-Bold="true"></asp:Label></div>      
         <div><asp:DropDownList ID="ddlComandos" runat="server" DataTextField="com_nombre" DataValueField="com_id" AutoPostBack="true">
             </asp:DropDownList>
             <br />
             <br />
              </div>      
   </div>  
          <div>
         <div><asp:Label ID="Label4" runat="server" Text="Mensaje a Enviar:" Font-Bold="true"></asp:Label></div>      
         <div><asp:TextBox ID="txtMensaje" runat="server" Width="350px" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator
                 ID="RequiredFieldValidator1" runat="server" ErrorMessage="Comando Requerido." ControlToValidate="txtMensaje"></asp:RequiredFieldValidator></div>      
   </div>   
   </div>
  
     <br />
     &nbsp;
     <asp:Button ID="btnAceptar" runat="server" Text="Enviar Comando" />
     
      <br />
      <br />
 </div>
 <div style="margin-left:30px;">
 <br />
  <asp:Label ID="Label2" runat="server" Text="RESPUESTAS DE LOS MODULOS" Font-Bold="true" Font-Size="16px"></asp:Label>
     <br />
     <br />
   <trirand:jqgrid runat="server" ID="JQGridRespuestas" Width="650px" Height="300px"
         OnDataRequesting="JQGridRespuestas_DataRequesting" >
                <Columns>                    
                    <trirand:JQGridColumn DataField="mod_id" HeaderText="Modulo" Width="50" Searchable="true" DataType="Integer" SearchToolBarOperation="Contains"/>
               <trirand:JQGridColumn DataField="cliente" HeaderText="Cliente" Width="100" Searchable="true" DataType="String" SearchToolBarOperation="Contains"/>
               <trirand:JQGridColumn DataField="patente" HeaderText="Patente" Width="50" Searchable="true" DataType="String" SearchToolBarOperation="Contains"/>
                    <trirand:JQGridColumn DataField="men_fecha" HeaderText="Fecha" Width="90" Editable="false" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" DataType="DateTime" SearchType="DatePicker" SearchControlID="DatePicker1" Searchable="true" SearchToolBarOperation="IsEqualTo" />
                                    <trirand:JQGridColumn DataField="men_respuesta" HeaderText="Respuesta" Width="150" Searchable="true"  DataType="String" SearchToolBarOperation="Contains"/>

                </Columns>
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false" ShowSearchToolBar="true">                   
                </ToolBarSettings>
                <PagerSettings PageSize="20" NoRowsMessage="No se encontraron Respuestas de los modulos"  />
               
            </trirand:jqgrid>
                        <trirand:JQDatePicker DisplayMode="ControlEditor" runat="server" ID="DatePicker1" DateFormat="dd/MM/yyyy" MinDate="01/01/2010" MaxDate="01/01/2030" ShowOn="Focus" />
 </div>
 </div> 
 </ContentTemplate>
 </asp:UpdatePanel>              

 
</asp:Content>
