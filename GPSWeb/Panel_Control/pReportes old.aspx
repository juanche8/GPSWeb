<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pReportes.aspx.vb" Inherits="GPSWeb.pReportes"  MasterPageFile="~/Panel_Control/SiteMaster.Master" Culture="Auto" UICulture="Auto"%>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.0/jquery.min.js" language="javascript" type="text/javascript"></script>
<script type="text/javascript">
    /*  window.onload = function() {
    window.frames['ReportFrame<%= ReportViewer1.ClientID %>'].
    window.frames['report'].
    document.getElementById('oReportCell').
    style.width = '100%';
    }
   

    function $_create(elem, tag, target) { return addElem(elem, target, tag) }
    function $_add(elem, target) { return addElem(elem, target) }
    function $_GB() { return GetBrowser(); }

    function GetBrowser() {
    //JQuery Script;
    if ($.browser.mozilla)
    return 'FF';
    else if ($.browser.msie)
    return 'IE';
    else if ($.browser.webkit)
    return 'OP';
    else if ($.browser.opera)
    return 'WK';
    else
    return 'FF';
    }

    function addElem(elem, target, tag) {
    if (typeof elem === 'string') {
    var el = document.getElementById(elem);
    if (!el) {

                el = document.createElement(tag);

                el.id = elem;
    }
    elem = el;
    }
    if (target) {
    var dest;
    if (typeof target === 'string')
    dest = document.getElementById(target);
    else
    dest = target;

            dest.appendChild(elem);
    }

        return elem;
    }

    function insert(elem, target) {
    if (typeof target === 'string')
    target = document.getElementById(target);
    var myDoc = target.contentWindow || target.contentDocument;
    if (myDoc.document) {
    myDoc = myDoc.document;
    }
    var headLoc = myDoc.getElementsByTagName("head").item(0);
    var scriptObj = myDoc.createElement("script");
    scriptObj.setAttribute("type", "text/javascript");
    scriptObj.innerHTML = 'window.print();';
    if (elem)
    elem = document.getElementById(elem);

        if (elem)
    headLoc.appendChild(elem);
    else
    headLoc.appendChild(scriptObj);

    }*/

    //Fixing Report Viewer control toolbar in Google Chrome - para verlo en una linea
    $(document).ready(function() {
        if ($.browser.webkit) {
            $(".ms-report-viewer-control :nth-child(3) table").each(function(i, item) {
                $(item).css('display', 'inline-block');
            });
        }
    }); 
  </script>

       <script type="text/javascript">
           function SelectRadioButton(regexPattern, selectedRadioButton) {
               regex = new RegExp(regexPattern);
               for (i = 0; i < document.forms[0].elements.length; i++) {
                   element = document.forms[0].elements[i];
                   if (element.type == 'radio' && regex.test(element.name)) {
                       element.checked = false;
                   }
               }
               selectedRadioButton.checked = true;
           }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    
    <Triggers > 
    
        <asp:PostBackTrigger ControlID="btnReporteRutinaCompleto" />
        <asp:PostBackTrigger ControlID="btnReporteRutinaKms" />
         <asp:PostBackTrigger ControlID="btnReporteCustom" />
    </Triggers> 
    <ContentTemplate>
    
    <asp:HiddenField ID="hdncli_id" runat="server" />
     <asp:HiddenField ID="hdnveh_id" runat="server" />
    <asp:Label ID="Label2" runat="server" Text="Generar Reportes de Recorridos" Font-Bold="true" Font-Size="18px"></asp:Label><br />
      <asp:Panel ID="PanelPatente" runat="server" Visible="false">
     <asp:Label ID="Label7" runat="server" Text="Para el Movil Patente:" Font-Bold="true" Font-Size="18px"></asp:Label>
      <asp:Label ID="lblPatente" runat="server" Text="" Font-Size="18px"></asp:Label>
    </asp:Panel>
    <div>
     <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
    </div>
   <div style="border: thin solid #00A6C6;">
    <asp:Label ID="Label3" runat="server" Text="Reportes de Rutina:" Font-Bold="true" CssClass="fontRegion"></asp:Label><br /><br />
    
  
     <asp:Panel ID="PanelMoviles" runat="server">
        <asp:Label ID="Label4" runat="server" Text="Seleccione Movil"></asp:Label>
         <br />
         <asp:CheckBox ID="chkTodos1" runat="server" Text="Todos los Vehiculos" Checked="true" />
        <asp:DataList ID="DataListVehiculos1" runat="server" DataKeyField="veh_id">
             <ItemTemplate>           
              <asp:CheckBox ID="rdnMovil" runat="server" Text='<%# Eval("veh_descripcion")%>' OnCheckedChanged="DesmarcarTodo" AutoPostBack="true" />  -
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>'></asp:Label>   
              </ItemTemplate>
      </asp:DataList>
     </asp:Panel>
   
       <asp:RadioButtonList ID="rdnFrecuencia" runat="server" RepeatDirection="Horizontal">
           <asp:ListItem Value="1" Selected="True">Hoy</asp:ListItem>
           <asp:ListItem Value="2">Esta Semana</asp:ListItem>
          
       </asp:RadioButtonList>
       <br />
       <asp:Button ID="btnReporteRutinaCompleto" runat="server" Text="Ver Listado Completo" />
       &nbsp;&nbsp;
       <asp:Button ID="btnReporteRutinaKms" runat="server" Text="Ver Kms Recorridos" />
       &nbsp;&nbsp;
       <asp:Button ID="btnReporteAlertas" runat="server" Text="Ver Alertas" />
       <br />
       <br />
   </div>
      
           <div style="border: thin solid #00A6C6;">
           <br />
       <asp:Label ID="Label5" runat="server" Text="Reporte Customizado:" Font-Bold="true" CssClass="fontRegion"></asp:Label>
           <br />
           <br />
        <asp:RadioButtonList ID="rdnTipoReporte" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
           <asp:ListItem Value="1" Selected="True">Listado Completo</asp:ListItem>
           <asp:ListItem Value="2">Solo Km Recorridos</asp:ListItem>
           <asp:ListItem Value="3">Alarmas Reportadas</asp:ListItem>          
       </asp:RadioButtonList>
           <br />
              <asp:Panel ID="PanelCampos" runat="server">
                <asp:Label ID="Label6" runat="server" Text="Campos del Reporte" Font-Bold="true"></asp:Label>
           <br />
           <br />        
           
              <asp:CheckBox ID="chkPatente" runat="server" Text=" Patente"  Checked="true"/>
               <br />
               <asp:CheckBox ID="chkFecha" runat="server" Text=" Fecha" Checked="true" />
              <br />
              <asp:CheckBox ID="chkHora" runat="server" Text=" Hora" Checked="true"/>
               <br />
                <asp:CheckBox ID="chkDireccion" runat="server" Text=" Direccion" Checked="true"/>
             <br />
             <asp:CheckBox ID="chkLocalidad" runat="server" Text=" Localidad" Checked="true"/>
              <br />
              <asp:CheckBox ID="chkProvincia" runat="server" Text=" Provincia" Checked="true"/>
             <br />
               <asp:CheckBox ID="chkVelocidad" runat="server" Text=" Velocidad" Checked="true"/>
             <br />
              <asp:CheckBox ID="ChkKms" runat="server" Text=" Suma Kms" Checked="true"/>
             <br />
              <asp:CheckBox ID="ChkAlertas" runat="server" Text=" Alertas" Checked="true"/>
             <br />
           </asp:Panel>
         
           <br />
             <asp:Panel ID="PanelCustomizado" runat="server">
             <div>
             <asp:Label ID="Label1" runat="server" Text="Vehiculos" Font-Bold="true"></asp:Label>
                 <br />
                 <br />
                 <asp:RadioButton ID="chkTodos" runat="server"
             Text='Todos' GroupName="radiomovil" Checked="true"  onclick="SelectRadioButton('radiomovil$',this)"  />             
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <asp:TextBox ID="txtFechaDesde" runat="server" Width="80px"> </asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFechaDesde" PopupButtonID="txtFechaDesde"/>
        <ajaxtoolkit:textboxwatermarkextender id="TBWE3" runat="server" targetcontrolid="txtFechaDesde" watermarktext='FechaDesde' />
          <asp:TextBox ID="txtFechaHasta" runat="server" Width="80px"></asp:TextBox>
         <ajaxtoolkit:textboxwatermarkextender id="TBWE4" runat="server" targetcontrolid="txtFechaHasta" watermarktext='FechaHasta' />
           <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaHasta" PopupButtonID="txtFechaHasta"/>
         <asp:DropDownList ID="ddlhoraDesde" runat="server" Width="90px">
         <asp:ListItem Value="00">Hora Desde</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList> 
        <asp:DropDownList ID="ddlMinDesde" runat="server" Width="90px">
         <asp:ListItem Value="00">Min Desde</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>           
        </asp:DropDownList>
           <asp:DropDownList ID="ddlHoraHasta" runat="server" Width="90px">
         <asp:ListItem Value="23">Hora Hasta</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList> 
        <asp:DropDownList ID="ddlMinHasta" runat="server" Width="90px">
         <asp:ListItem Value="59">Min Hasta</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>           
        </asp:DropDownList><br />
           </div>
     
     <asp:DataList ID="DataListVehiculos" runat="server" DataKeyField="veh_id">
     <ItemTemplate>
         <asp:RadioButton ID="rdnMovil" runat="server"
             Text='<%# Eval("veh_descripcion")%>' GroupName="radiomovil" onclick="SelectRadioButton('radiomovil$',this)" /> -
     <asp:Label ID="Label4" runat="server" Text='<%# Eval("veh_patente")%>'></asp:Label>
       <asp:TextBox ID="txtFechaDesde" runat="server" Width="75px"> </asp:TextBox>
        <ajaxtoolkit:textboxwatermarkextender id="TBWE3" runat="server" targetcontrolid="txtFechaDesde" watermarktext='FechaDesde' />
          <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaDesde" PopupButtonID="txtFechaDesde"/>
          <asp:TextBox ID="txtFechaHasta" runat="server" Width="75px"></asp:TextBox>
          <ajaxtoolkit:textboxwatermarkextender id="TBWE4" runat="server" targetcontrolid="txtFechaHasta" watermarktext='FechaHasta' />
           <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaHasta" PopupButtonID="txtFechaHasta"/>
          <asp:DropDownList ID="ddlhoraDesde" runat="server" Width="90px">
         <asp:ListItem Value="00">Hora Desde</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList> 
        <asp:DropDownList ID="ddlMinDesde" runat="server" Width="90px">
         <asp:ListItem Value="00">Min Desde</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>           
        </asp:DropDownList>
           <asp:DropDownList ID="ddlHoraHasta" runat="server" Width="90px">
         <asp:ListItem Value="23">Hora Hasta</asp:ListItem>
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList> 
        <asp:DropDownList ID="ddlMinHasta" runat="server" Width="90px">
         <asp:ListItem Value="59">Min Hasta</asp:ListItem>
         <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>35</asp:ListItem>
            <asp:ListItem>40</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>55</asp:ListItem>           
        </asp:DropDownList>
     </ItemTemplate>
     </asp:DataList>
       </asp:Panel>
                 <br />
     <asp:Button ID="btnReporteCustom" runat="server" Text="Generar Reporte" />
     <br /><br />
</div>
    
    
 <div style="border: thin solid #00A6C6; height:100%;">

     <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" ProcessingMode="Remote" ShowPrintButton="true" ExportContentDisposition="AlwaysAttachment" ShowFindControls="False"
          Font-Size="8pt" Height="100%" SizeToReportContent="true" Width="990px" ShowExportControls="true" ShowParameterPrompts="false" ShowBackButton = "true" ZoomPercent="75" ZoomMode="Percent">         
       
     </rsweb:ReportViewer>
 </div>
 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>