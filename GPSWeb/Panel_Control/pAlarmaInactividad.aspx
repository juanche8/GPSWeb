<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmaInactividad.aspx.vb" Inherits="GPSWeb.pAlarmaInactividad" MasterPageFile="~/Panel_Control/SiteMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
   <asp:HiddenField ID="hdncli_id" runat="server" />    
      <asp:HiddenField ID="hdnalah_id_id" runat="server" Value="0" />
        <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
       
<div style="margin-left:30px; width:100%; height:100%;">
   
   <div style="margin-left:50px; width:90%;height:auto;">
 <h3>&nbsp;Configurar Alarma de Inactividad</h3>
  <h5>Seleccione uno o más vehiculos a los que quiera asignarles la alarma, y los dias y rango de horario que se controlarán. Recibira una alarma si el movil esta parado en el mismo lugar por mas de los minutos configurados.</h5>
  <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="12px"></asp:Label>
 </div>
    
 
             <div id="tabs" style="width:60%; margin:0 0 0 50px;">
            
 
   <table style="width:100%; vertical-align:middle; font-size:12px; font-weight:bold; font-family:Arial;" cellspacing="8" cellpaging="5">
     <tr><td colspan="2">
         <asp:Label ID="lblMovil" runat="server" Text="Moviles:"></asp:Label>
     </td>
     </tr>
     <tr><td colspan="2">  
     <asp:Panel ID="PanelGrupo" runat="server" Visible ="true">
       <asp:Label ID="Label16" runat="server" Text="Filtrar por Grupo:" Font-Bold="true"></asp:Label>   
         &nbsp;<asp:DropDownList ID="ddlgrupo" runat="server" AutoPostBack="true" DataTextField="grup_nombre" DataValueField="grup_id">
         </asp:DropDownList>
         &nbsp;&nbsp; 
         <asp:CheckBox ID="chkTodos" runat="server" Text="Ver todos los Moviles" Font-Bold="true" AutoPostBack="true" />
         <br />
     </asp:Panel></td>
     <tr><td colspan="2">
      <asp:Panel ID="PanelMoviles" runat="server" Visible="false">
   <div style="height:200px;overflow-y: scroll; font-family:Arial;width:320px; border-color:LightGray; border-width:1px; border-style:solid;">       
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id" CellSpacing="5" CellPadding="5"  Font-Names="Arial" Font-Bold="false"   Width="300px">
             <ItemTemplate>           
              <asp:CheckBox ID="rdnMovil" runat="server" Text="" Font-Size="12px"  />
               <img src="../images/iconos_movil/autito_gris.png" alt="" /> 
                 <asp:Label ID="Label10" runat="server" Text='<%# Eval("veh_descripcion")%>' Font-Size="12px"></asp:Label>   -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>' Font-Size="12px"></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
      </div>
     <div> <br /><asp:LinkButton ID="LinkTildar" runat="server" ForeColor="#D85639" CausesValidation="false">Tildar Todos </asp:LinkButton>
       <asp:LinkButton ID="LinkDestildar" runat="server" ForeColor="#D85639" CausesValidation="false">/ Destildar Todos</asp:LinkButton></div>
        
      
</asp:Panel> 
     </td>
     
     </tr>

     <tr><td colspan="2"><span>Descripción para la Alarma:</span></td>
     </tr>
     <tr><td colspan="2"><asp:TextBox ID="txtDescripcion" runat="server" Width="350px" MaxLength="50"></asp:TextBox></td></tr>
     <tr><td colspan="2"><span>Controlar Alarma:</span></td>
     </tr>
     
      <tr><td colspan="2"> 
          <asp:RadioButtonList ID="rdbTipo" runat="server" CellSpacing="5" CellPadding="3"  Font-Names="Arial" AutoPostBack="true">
              <asp:ListItem Value="1">En una Fecha Especifica</asp:ListItem>
              <asp:ListItem Value="2">En Días de la Semana</asp:ListItem>
          </asp:RadioButtonList></td>
     </tr>
      <asp:Panel ID="PanelDias" runat="server" Visible="false">
        <tr><td colspan="2"><span>Disparar Alarma si el movil se uso los días:</span></td>
     </tr>
     <tr><td colspan="2" >  

         <asp:CheckBoxList ID="chkDias" runat="server" CellSpacing="5" CellPadding="3"  Font-Names="Arial">        
             <asp:ListItem Value="1">Lunes</asp:ListItem>
             <asp:ListItem Value="2">Martes</asp:ListItem>
             <asp:ListItem Value="3">Miercoles</asp:ListItem>
             <asp:ListItem Value="4">Jueves</asp:ListItem>
             <asp:ListItem Value="5">Viernes</asp:ListItem>
             <asp:ListItem Value="6">Sabado</asp:ListItem>
             <asp:ListItem Value="7">Domingo</asp:ListItem>
         </asp:CheckBoxList>

     </td></tr>
      </asp:Panel>
     
      <asp:Panel ID="PanelFecha" runat="server" Visible="false">
        <tr><td colspan="2"><span>Disparar Alarma si el vehiculo se usa en el siguiente rango de Fechas:</span></td>
     </tr>
    
       <tr><td>   Desde <asp:TextBox ID="txtFechaDesde" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="El formato de fecha es invalido"
                                                        ControlToValidate="txtFechaDesde" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
          <ajaxtoolkit:calendarextender ID="CalendarExtender3" runat="server" 
                 TargetControlID="txtFechaDesde" PopupButtonID="txtFecha"/> </td>
         <td>   Hasta  
         <asp:TextBox ID="txtFechaHasta" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="El formato de fecha es invalido"
                                                        ControlToValidate="txtFechaHasta" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
          <ajaxtoolkit:calendarextender ID="CalendarExtender1" runat="server" 
                 TargetControlID="txtFechaHasta" PopupButtonID="txtFecha"/>
         </td>
         </tr> 
      </asp:Panel>
           
     <tr><td colspan="2"><span>En el siguiente Rango Horario:</span></td>
     </tr>
    
       <tr><td>   Desde las  <asp:DropDownList ID="ddlhoraDesde" runat="server" Width="60px">
             <asp:ListItem Value="00">00</asp:ListItem>  
            <asp:ListItem Value="1">01</asp:ListItem> 
            <asp:ListItem Value="2">02</asp:ListItem>
            <asp:ListItem Value="3">03</asp:ListItem>
            <asp:ListItem Value="4">04</asp:ListItem>
            <asp:ListItem Value="5">05</asp:ListItem> 
            <asp:ListItem Value="6">06</asp:ListItem>
            <asp:ListItem Value="7">07</asp:ListItem>
            <asp:ListItem Value="8">08</asp:ListItem>
            <asp:ListItem Value="9">09</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="11">11</asp:ListItem>      
             <asp:ListItem Value="12">12</asp:ListItem>
        <asp:ListItem Value="13">13</asp:ListItem>
        <asp:ListItem Value="14">14</asp:ListItem>
        <asp:ListItem Value="15">15</asp:ListItem>
        <asp:ListItem Value="16">16</asp:ListItem>
        <asp:ListItem Value="17">17</asp:ListItem>
        <asp:ListItem Value="18">18</asp:ListItem>
        <asp:ListItem Value="19">19</asp:ListItem>
        <asp:ListItem Value="20">20</asp:ListItem>
        <asp:ListItem Value="21">21</asp:ListItem>
        <asp:ListItem Value="22">22</asp:ListItem>  
        <asp:ListItem Value="23">23</asp:ListItem>
                 
         </asp:DropDownList> Hs
          <asp:DropDownList ID="ddlminDesde" runat="server" Width="60px">        
             <asp:ListItem Value="00">00</asp:ListItem> 
              <asp:ListItem Value="05">05</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="15">15</asp:ListItem>
            <asp:ListItem Value="20">20</asp:ListItem>
            <asp:ListItem Value="25">25</asp:ListItem> 
            <asp:ListItem Value="30">30</asp:ListItem>
            <asp:ListItem Value="35">35</asp:ListItem>
            <asp:ListItem Value="40">40</asp:ListItem>
            <asp:ListItem Value="45">45</asp:ListItem>
            <asp:ListItem Value="50">50</asp:ListItem>
            <asp:ListItem Value="55">55</asp:ListItem>      
          
         </asp:DropDownList>Minutos</td>
         <td>   Hasta las  <asp:DropDownList ID="ddlHoraHasta" runat="server" Width="60px">
             <asp:ListItem Value="00">00</asp:ListItem>  
            <asp:ListItem Value="1">01</asp:ListItem> 
            <asp:ListItem Value="2">02</asp:ListItem>
            <asp:ListItem Value="3">03</asp:ListItem>
            <asp:ListItem Value="4">04</asp:ListItem>
            <asp:ListItem Value="5">05</asp:ListItem> 
            <asp:ListItem Value="6">06</asp:ListItem>
            <asp:ListItem Value="7">07</asp:ListItem>
            <asp:ListItem Value="8">08</asp:ListItem>
            <asp:ListItem Value="9">09</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="11">11</asp:ListItem>      
             <asp:ListItem Value="12">12</asp:ListItem>
        <asp:ListItem Value="13">13</asp:ListItem>
        <asp:ListItem Value="14">14</asp:ListItem>
        <asp:ListItem Value="15">15</asp:ListItem>
        <asp:ListItem Value="16">16</asp:ListItem>
        <asp:ListItem Value="17">17</asp:ListItem>
        <asp:ListItem Value="18">18</asp:ListItem>
        <asp:ListItem Value="19">19</asp:ListItem>
        <asp:ListItem Value="20">20</asp:ListItem>
        <asp:ListItem Value="21">21</asp:ListItem>
        <asp:ListItem Value="22">22</asp:ListItem>  
        <asp:ListItem Value="23">23</asp:ListItem>
                 
         </asp:DropDownList> Hs
          <asp:DropDownList ID="ddlMinHasta" runat="server" Width="60px">        
             <asp:ListItem Value="00">00</asp:ListItem> 
              <asp:ListItem Value="05">05</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="15">15</asp:ListItem>
            <asp:ListItem Value="20">20</asp:ListItem>
            <asp:ListItem Value="25">25</asp:ListItem> 
            <asp:ListItem Value="30">30</asp:ListItem>
            <asp:ListItem Value="35">35</asp:ListItem>
            <asp:ListItem Value="40">40</asp:ListItem>
            <asp:ListItem Value="45">45</asp:ListItem>
            <asp:ListItem Value="50">50</asp:ListItem>
            <asp:ListItem Value="55">55</asp:ListItem>   
             <asp:ListItem Value="59">59</asp:ListItem>    
          
         </asp:DropDownList>Minutos</td>
         </tr>

 <tr><td colspan="2"><span>Cantidad de Minutos de Inactividad:</span></td>
     </tr>
 
  <tr><td colspan="2"><asp:TextBox ID="txtMinutos" runat="server" Width="100px" MaxLength="10" Text="30"></asp:TextBox></td></tr>
<tr><td colspan="2"><span>Notificar por e-mail:</span></td></tr>
<tr><td colspan="2"><asp:CheckBox ID="chkMail" runat="server" Text="Si" Checked="true"/></td></tr>

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

