<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmakmsAlcanzados.aspx.vb" Inherits="GPSWeb.pAlarmakmsAlcanzados" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
   <asp:HiddenField ID="hdncli_id" runat="server" />    
      <asp:HiddenField ID="hdnalah_id_id" runat="server" Value="0" />
        <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
       
<div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Configurar Alarma de Exceso de Kms Recorridos</h3>
  <h5>Seleccione uno o más vehiculos a los que quiera asignarles la alarma, y la frecuencia con la que se controlarán.<br /> Recibirá una alarma cuando el móvil alcance la cantidad de Kms configurados en la frecuencia establecida.</h5>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="12px"></asp:Label>
 </div>
    
 
             <div id="tabs" style="width:60%; margin:0 0 0 50px;">
            
 
   <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">
     <tr><td>
         <asp:Label ID="lblMovil" runat="server" Text="Moviles:"></asp:Label>
     </td>
     </tr>
     <tr><td>  
     <asp:Panel ID="PanelGrupo" runat="server" Visible ="true">
       <asp:Label ID="Label16" runat="server" Text="Filtrar por Grupo:" Font-Bold="true"></asp:Label>   
         &nbsp;<asp:DropDownList ID="ddlgrupo" runat="server" AutoPostBack="true" DataTextField="grup_nombre" DataValueField="grup_id">
         </asp:DropDownList>
         &nbsp;&nbsp; 
         <asp:CheckBox ID="chkTodos" runat="server" Text="Ver todos los Moviles" Font-Bold="true" AutoPostBack="true" />
         <br />
     </asp:Panel></td>
     <tr><td>
      <asp:Panel ID="PanelMoviles" runat="server" Visible="false">
   <div style="height:200px;overflow-y: scroll; font-family:Arial;width:320px; border-color:LightGray; border-width:1px; border-style:solid;"">       
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="5" CellPadding="5"  Font-Names="Arial" Font-Bold="false"  Width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="rdnMovil" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
     <div> <br /> <asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton></div>
</asp:Panel> 
     </td>
     
     </tr>

     <tr><td><span>Descripción para la Alarma:</span></td>
     </tr>
     <tr><td><asp:TextBox ID="txtDescripcion" runat="server" Width="350px" MaxLength="50"></asp:TextBox></td></tr>
     <tr><td><span>Controlar Alarma:</span></td>
     </tr>
     
      <tr><td> 
          <asp:RadioButtonList ID="rdbTipo" runat="server" CellSpacing="5" CellPadding="3"  Font-Names="Arial" >
              <asp:ListItem Value="1" Selected="True">Diariamente</asp:ListItem>
              <asp:ListItem Value="2">Semanalmente</asp:ListItem>
              <asp:ListItem Value="3">Mensualmente</asp:ListItem>
          </asp:RadioButtonList></td>
     </tr>
     

 <tr><td><span>Cantidad máxima de Kms Recorridos:</span></td>
     </tr>
  <tr><td><asp:TextBox ID="txtKms" runat="server" Width="100px" MaxLength="10" Text="100"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Ingrese Cantidad de Kms. Mayor a Cero." ControlToValidate="txtKms"></asp:RequiredFieldValidator>
  </td></tr>
    
<tr><td><span>Notificar por e-mail:</span></td></tr>
<tr><td><asp:CheckBox ID="chkMail" runat="server" Text="Si" Checked="true"/></td></tr>

       </table>
   
  

      <div style="text-align:center;">
      <br /><br /><br /><br /> 
      <asp:Button ID="Button1" runat="server" Text="Volver" CausesValidation="false" PostBackUrl="~/Panel_Control/pAlarmas.aspx?tab=tabs-2" CssClass="button2" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" />
        &nbsp;
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Configuración" CssClass="button2" Width="160px" Font-Size="14px" Height="30px" Font-Names="'Helvetica Neue', Helvetica, Arial, sans-serif" /><br /><br />
      </div>       
 </div>
 </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
   
</asp:Content>
