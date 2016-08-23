<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAlarmas.aspx.vb" Inherits="GPSWeb.pAlarmas" MasterPageFile="~/Panel_Control/SiteMaster.Master" uiCulture="es" culture="es-AR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Trirand.Web" TagPrefix="trirand" Namespace="Trirand.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- The jQuery UI theme extension jqGrid needs -->
   

     <script src="../scripts/trirand/i18n/grid.locale-sp.js" type="text/javascript"></script>
    <!-- The jqGrid client-side javascript -->
      <script src="../scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../scripts/ui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../scripts/ui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <script src="../scripts/ui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../scripts/trirand/jquery.jqGrid.min.js" type="text/javascript"></script>   
     <script src="../scripts/trirand/jquery.jqDatePicker.min.js" type="text/javascript"></script>     
     <script src="../scripts/ui/jquery.ui.tabs.min.js" type="text/javascript"></script>
      <script type="text/javascript">
          $(function () {
              $("#tabs").tabs();
          });

          $(function () {
              $("#tabsA").tabs();
          });
           

        function formatmapa(cellValue, options, rowObject) {
            var imageHtml = "<a title='Ver Mapa' href='#' onclick='verMapa(" + cellValue + "); ' originalValue=''><img src='../images/view.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatOcultar(cellValue, options, rowObject) {
            var imageHtml = "<a title='Ocultar' href='#' onclick='ocultar(" + cellValue + "); ' originalValue=''>Si</a>";
            return imageHtml;
        }


        function unformatmapa(cellValue, options, cellObject) {
            return $(cellObject.html()).attr("originalValue");
        }

        function ocultar(alar_id) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/ocultarAlarma",
                data: "{'alar_id': '" + alar_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
            var gridAlarmas = jQuery("#<%= JQGridAlarmas.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            
        }

        function filtrar() {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/filtrarAlarma1",
                data: "{'alar_id': '1','alar2': '2' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    result = response.d ? response.d : response;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown); 
                }
            });
            var gridAlarmas = jQuery("#<%= JQGridAlarmas.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            
            
        }

        function verMapa(lat, lng, tipo, codigo) {

            $('<iframe id="mapa" border="1" frameborder="1" framespacing="0" src="verMapa.aspx?tipo=' + tipo + '&codigo=' + codigo + '&lat=' + lat + '&lng=' + lng + '" />').dialog({

                title: "",
                autoOpen: true,
                width: 550,
                height: 450,
                modal: true,
                resizable: false,
                autoResize: true,
                position: [200,370],
                close: function() {
                    $(this).dialog('destroy').remove();
                }
            });
            $('#mapa').dialog('open').width(550).height(450);
           
        }


        function cerrar(name) {
            $('#' + name + '').dialog('destroy').remove();
        }


        function BuscarAlarmas(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPantenteZ.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;

            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasZonas",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmasZona.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmZona");
        }
        function BuscarAlarmasRec(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPantenteR.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasRecorridos",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
            var gridAlarmas = jQuery("#<%= JQGridAlarmaRecorrido.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmRecorrido");
        }

        function BuscarAlarmasDir(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteD.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasDirecciones",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridDireccion.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmDirecciones");
        }

        function BuscarAlarmasRecFe(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteF.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasRecordatorioFecha",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridRecordatorioFecha.ClientID %>");
            gridAlarmas.trigger("reloadGrid");

            /* recordatorio por km */
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasRecordatorioKm",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridRecordatorioKm.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmRecFecha");
        }


        function BuscarAlarmasHorario(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteH.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasFueraHorario",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridFueraHorario.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmHorario");
        }

        function BuscarAlarmasInicio(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteI.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasInicioActividad",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmasInicio.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmInicio");
        }

        function BuscarAlarmasInactivo(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteIn.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasInactividad",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmasInactividad.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmInactivo");
        }

        function BuscarAlarmasKms(movil, patente) {
            document.getElementById("<%= hdnMovil.ClientID %>").value = movil;
            document.getElementById("<%= lblPatenteK.ClientID %>").innerHTML = 'Vehiculo Patente: ' + patente;
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/gAlarmasExcesos",
                data: "{'veh_id': '" + movil + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var algo = '';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmasExceso.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmKms");
        }

        function MostrarGrilla(id) {
            $(id).dialog({
                autoOpen: true,
                title: '',
                width: 850,
                height: 500,
                position: ["center"],
                buttons: {
                    Cerrar: function () {
                        $(this).dialog('close');
                    }
                }
            });

        }

        function activaDesacAlertaR(veh_id, id, estado) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/activaDesacAlertaR",
                data: "{'id': '" + id + "','estado': '" + estado + "','veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    result = response.d ? response.d : response;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmaRecorrido.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmRecorrido");
        }

        function activaDesacAlertaZ(veh_id, id, estado) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/activaDesacAlertaZ",
                data: "{'id': '" + id + "','estado': '" + estado + "','veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    result = response.d ? response.d : response;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridAlarmasZona.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmZona");
        }

        function activaDesacAlertaD(veh_id, id, estado) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "wsDatos.asmx/activaDesacAlertaD",
                data: "{'id': '" + id + "','estado': '" + estado + "','veh_id': '" + veh_id + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    result = response.d ? response.d : response;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            var gridAlarmas = jQuery("#<%= JQGridDireccion.ClientID %>");
            gridAlarmas.trigger("reloadGrid");
            MostrarGrilla("#abmDirecciones");
        }

        //funciones para la grilla de jQuery

        function formatEditKms(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaKmsAlcanzados.aspx?alak_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditRec(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmasRecorrido.aspx?rec_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditHorario(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaFueraHorario.aspx?alah_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditInactivo(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaInactividad.aspx?alah_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditInicio(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaIncioActividad.aspx?alah_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditRecFecha(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaRecordatorioFecha.aspx?recf_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditRecKm(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmaRecordatorioKm.aspx?reck_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditZona(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmasZonas.aspx?veh_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function formatEditDirecc(cellValue, options, rowObject) {
            var imageHtml = "<a title='Editar Alarma' href='pAlarmasDireccion.aspx?dir_id=" + cellValue + "' ' originalValue=''><img src='../images/edit.gif' border='0' /></a>";
            return imageHtml;
        }

        function unformatEdit(cellValue, options, cellObject) {
            return $(cellObject.html()).attr("originalValue");
        }

        function formatDelete(cellValue, options, rowObject) {
            var imageHtml = "<a title='Borrar Alarma' href='pAlarmas.aspx?alar_id=" + cellValue + "';' originalValue=''><img src='../images/delete.gif' border='0' /></a>";
            return imageHtml;
        }


        function unformatDelete(cellValue, options, cellObject) {
            return $(cellObject.html()).attr("originalValue");
        }

        function nuevaAlarma() {
            $("#menuAlarma").dialog({
                title: 'Seleccione Alarma',
                autoOpen: true,
                modal: true,
                resizable: false,
                autoResize: true,
                position: [800, 250],
                buttons: {
                    Cerrar: function () {
                        $(this).dialog('close');
                    }
                }
            });
        }

        function adminZona() {
            window.location.href = 'pAlarmasZonas.aspx?veh_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminDesvio() {
            window.location.href = 'pAlarmasRecorrido.aspx?rec_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminHorario() {
            window.location.href = 'pAlarmaFueraHorario.aspx?alah_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminInactivo() {
            window.location.href = 'pAlarmaInactividad.aspx?alah_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminDirecc() {
            window.location.href = 'pAlarmasDireccion.aspx?dir_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminRecorrid() {
            window.location.href = 'pAlarmaRecordatorio.aspx?veh_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminRecord() {
            window.location.href = 'pAlarmaRecordatorio.aspx?veh_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminInicio() {
            window.location.href = 'pAlarmaIncioActividad.aspx?alah_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        function adminkms() {
            window.location.href = 'pAlarmaKmsAlcanzados.aspx?alak_id=' + document.getElementById('<%= hdnMovil.ClientID %>').value;
        }

        </script>
       

  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
      <asp:HiddenField ID="hdnTab" runat="server" Value="#tabs-1" />
    <asp:HiddenField ID="hdncli_id" runat="server" Value="0" />
     <asp:HiddenField ID="hdnveh_id" runat="server" Value="0" />
      <asp:HiddenField ID="hdncat_usu" runat="server" Value="0" />
      <asp:HiddenField ID="hdncat_id" runat="server" Value="0" />
    <asp:HiddenField ID="hdnMovil" runat="server" Value="0" />
 <div style="float: left; width:100%; height:auto;">
 
  <div id="tabs" style="width:100%">
    <br />
                            <ul>
                            <li><a href="#tabs-1">Alarmas Reportadas</a></li>  
                                <li><a href="#tabs-2">Configurar Alarmas</a></li>
                             <li><a href="#tabs-3">Nueva Alarma</a></li>
                            </ul>
  <div id="tabs-1" style="width:100%;">
  <div style="margin-left:50px; width:90%;height:auto;">
 <h3>Alarmas Reportadas</h3>
    
   <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
    <asp:Panel ID="PanelPatente" runat="server" Visible="false">
     <asp:Label ID="Label7" runat="server" Text="Para el Movil Patente:" Font-Bold="true" Font-Size="14px"></asp:Label>
      <asp:Label ID="lblPatente" runat="server" Text="" Font-Size="14px"></asp:Label>
       <br />
  <br />
    </asp:Panel>
    
    </div>
    <div style="margin-left:30px; width:95%; z-index:9999; padding:5px; vertical-align:middle;">
     <asp:Label ID="Label1" runat="server" Text="Fecha Desde: " Font-Size="14px" Font-Names="Arial"></asp:Label>
    <asp:TextBox ID="txtfechaDesde" runat="server" Width="86px"></asp:TextBox>
       &nbsp;&nbsp;
          <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="!" ForeColor="Red" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))" ControlToValidate="txtfechaDesde" Font-Size="13px" Font-Bold="true"></asp:RegularExpressionValidator>
       <ajaxtoolkit:calendarextender ID="CalendarExtender1" runat="server" CssClass="black" 
             TargetControlID="txtfechaDesde" PopupButtonID="txtfechaDesde" Format="dd/MM/yyyy"/>
              <asp:Label ID="Label3" runat="server" Text="Fecha Hasta: " Font-Size="14px" Font-Names="Arial"></asp:Label>
    &nbsp;<asp:TextBox ID="txtfechaHasta" runat="server" Width="86px"></asp:TextBox>
     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="!" ForeColor="Red" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))" ControlToValidate="txtfechaHasta" Font-Size="13px" Font-Bold="true"></asp:RegularExpressionValidator>
       &nbsp;
       <ajaxtoolkit:calendarextender ID="CalendarExtender2" runat="server" CssClass="black" Format="dd/MM/yyyy"
             TargetControlID="txtfechaHasta" PopupButtonID="txtfechaHasta"/>    
        <asp:Button   ID="btBuscar" runat="server" Text="Filtrar" CssClass="button2" Font-Names="Arial"
            Height="29px" Width="103px" /> &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <asp:LinkButton ID="linkOcultar" runat="server" Font-Bold="true" ForeColor="#D85639" Font-Size="10px" Font-Names="Helvetica Neue,Helvetica,Arial,sans-serif" OnClientClick="alert('Las Alarmas dejaran de verse en este Listado. Para consultarlas use la opción reportes');">Ocultar Todas</asp:LinkButton>
            
    </div>
    <div style="width:99%;z-index:0;">
    <br />
     <trirand:jqgrid runat="server" ID="JQGridAlarmas" AutoWidth="true" Height="500px"
         OnDataRequesting="JQGridAlarmas_DataRequesting"  >
                <Columns>                    
                    <trirand:JQGridColumn DataField="Alarma" HeaderText="Alarma" Width="220" Searchable="true" DataType="String" SearchToolBarOperation="Contains"/>
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="60" Searchable="true"  DataType="String" SearchToolBarOperation="Contains"/>
                    <trirand:JQGridColumn DataField="Conductor" HeaderText="Conductor" Width="110" DataType="String" Searchable="true" SearchToolBarOperation="Contains" />
                    <trirand:JQGridColumn DataField="Fecha" HeaderText="Fecha" Width="80" Editable="false" DataFormatString="{0:dd-MM-yyyy}" DataType="DateTime" SearchType="DatePicker" SearchControlID="DatePicker1" Searchable="false" SearchToolBarOperation="IsEqualTo" />
                   <trirand:JQGridColumn DataField="Hora" HeaderText="Hora" Width="60" Editable="false" Searchable="true" DataType="String" SearchToolBarOperation="Contains"/>
                    <trirand:JQGridColumn DataField="Duracion" HeaderText="Duración" Width="70" Editable="false" Searchable="true" DataType="String"  TextAlign="Right" SearchToolBarOperation="Contains"/>
                   <trirand:JQGridColumn DataField="Ubicacion" HeaderText="Ubicación" Width="210" Editable="false" Searchable="true"  DataType="String" SearchToolBarOperation="Contains"/>
                   <trirand:JQGridColumn DataField="Valor" HeaderText="Velocidad/Kms" Width="95" Editable="false" Searchable="true" DataType="Int" TextAlign="Right" SearchToolBarOperation="Contains"/>
                   <trirand:JQGridColumn DataField="Limite" HeaderText="Limite" Width="55" Editable="false" Searchable="true" DataType="Int" SearchToolBarOperation="Contains" TextAlign="Right"/>
                   
                    <trirand:JQGridColumn DataField="Mail_Enviado" HeaderText="Mail Enviado" Width="90" Editable="false" Searchable="true" />
                     <trirand:JQGridColumn DataField="SMS_Enviado" HeaderText="SMS Enviado" Width="85" Editable="false" Searchable="true"  />
                    <trirand:JQGridColumn DataField="latLng" Width="45" Searchable="false" HeaderText="Mapa" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatmapa" UnFormatFunction="unformatmapa" />
                        </Formatter>
                      </trirand:JQGridColumn>
                       <trirand:JQGridColumn  Width="60" Searchable="false" HeaderText="Ocultar" DataField="alar_id" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatOcultar" UnFormatFunction="unformatmapa" />
                        </Formatter>
                      </trirand:JQGridColumn>
                </Columns>
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false" ShowSearchToolBar="true">                   
                </ToolBarSettings>
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />

                <PagerSettings PageSize="20" NoRowsMessage="No se encontraron Alarmas reportadas en el Ultimo Mes"  />
               
            </trirand:jqgrid>
                        <trirand:JQDatePicker DisplayMode="ControlEditor" runat="server" ID="DatePicker1" DateFormat="dd-MM-yyyy" MinDate="01-01-2010" MaxDate="01-01-2030" ShowOn="Focus" />

    </div>
  </div>

  <div id="tabs-2">
  <div style="margin-left:50px; width:90%;height:auto; font-family:Arial;">
 <h3>Alarmas configuradas por Móvil</h3>
 <h5>Para editar seleccione sobre cada celda la alarma y el móvil.</h5>
 </div>
  
 
   <div style="margin-left:10px; width:95%;">  

  <asp:GridView ID="GridViewAlarmas" runat="server" DataKeyNames="veh_id" Font-Names="Arial"  
          AutoGenerateColumns="False" Width="95%" HorizontalAlign="Center" 
          OnRowDataBound="GridViewAlarmas_RowDataBound" AllowSorting="True" RowStyle-Font-Size="9"
 HeaderStyle-Font-Size="10" HeaderStyle-Font-Bold="False" BackColor="White" 
          BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
          EnableModelValidation="True" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
           <asp:BoundField HeaderText="Patente" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="veh_patente" >
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>       
                        <asp:BoundField HeaderText="Conductor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left"  DataField="veh_nombre_conductor" >
<HeaderStyle Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>
                            <asp:TemplateField HeaderText="Exc.Velocidad" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>  
                                <a href="pAlarmaEdit.aspx?veh_id=<%# Eval("veh_id")%>"><asp:Label ID="lblVelocidad" runat="server" Text="Sin Configurar"></asp:Label></a>                              
                                    </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                         
                              <asp:TemplateField HeaderText="Sensores" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                 <a href="pAlarmaSensorEdit.aspx?veh_id=<%# Eval("veh_id")%>" ><asp:Label ID="lblSensor" runat="server" Text="Sin Configurar"></asp:Label></a>                              
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Uso Fuera Horario" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                 <a href="#" onclick="BuscarAlarmasHorario('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantFueraHora" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Inicio Act. Diaria" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                 <a href="#" onclick="BuscarAlarmasInicio('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantInicio" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle></asp:TemplateField>
 <asp:TemplateField HeaderText="Inactividad" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                 <a href="#" onclick="BuscarAlarmasInactivo('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantInactivo" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="Excesos Kms" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                 <a href="#" onclick="BuscarAlarmasKms('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantKms" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Recorridos" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <a href="#" onclick="BuscarAlarmasRec('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantRecorrido" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Entrada/Salida Zona" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <a href="#" onclick="BuscarAlarmas('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantZona" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>

<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Entrada/Salida Direcc" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <a href="#" onclick="BuscarAlarmasDir('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantDireccion" runat="server" Text="Sin Configurar"></asp:Label></a>
                               </ItemTemplate>
<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recordatorios" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="#" onclick="BuscarAlarmasRecFe('<%# Eval("veh_id")%>','<%# Eval("veh_patente")%>');"><asp:Label ID="lblCantRecordatorio" runat="server" Text="Sin Configurar"></asp:Label></a>                              
                               </ItemTemplate>
<HeaderStyle HorizontalAlign="Center" Width="10%" Font-Bold="False"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>             
                       </Columns>       
        <FooterStyle BackColor="#CCCCCC" />
<HeaderStyle Font-Bold="False" Font-Size="10pt" BackColor="#343535" ForeColor="White"></HeaderStyle>
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
<RowStyle Font-Size="9pt"></RowStyle>
        <SelectedRowStyle BackColor="#000099" Font-Bold="False" ForeColor="White" />
        </asp:GridView>   
  </div>
  </div>
   <div id="tabs-3">
    <table style="width:100%; font-family:Arial; vertical-align:middle;" cellspacing="8" cellpaging="5">
     <tr><td><a href="pAlarmaFueraHorario.aspx" style="font-size:12px; font-weight:bold; text-decoration:underline;">Uso Móvil Fuera Horario</a></td>
     </tr>
      <tr><td><a href="pAlarmaIncioActividad.aspx" style="font-size:12px; font-weight:bold; text-decoration:underline;">Comienzo de Actividad Diaria</a></td>
     </tr>
      <tr><td><a href="pAlarmaInactividad.aspx" style="font-size:12px; font-weight:bold; text-decoration:underline;">Inactividad</a></td>
     </tr>
      <tr><td><a href="pAlarmaKmsAlcanzados.aspx" style="font-size:12px; font-weight:bold; text-decoration:underline;">Exceso de kms Recorridos</a></td>
     </tr>
     <tr><td><a href="pAlarmaAdd.aspx" style="font-size:12px; font-weight:bold; text-decoration:underline;">Excesos de Velocidad</a></td>
     </tr>
      <tr><td><a href="pAlarmaSensorAdd.aspx"  style="font-size:12px; font-weight:bold; text-decoration:underline;">Sensores</a></td>
     </tr>
      <tr><td><a href="pAlarmasZonas.aspx"  style="font-size:12px; font-weight:bold; text-decoration:underline;">Entrada/Salida Zonas</a></td>
     </tr>
      <tr><td><a href="pAlarmasDireccion.aspx"  style="font-size:12px; font-weight:bold; text-decoration:underline;">Entrada/Salida Direcciones</a></td>
     </tr>
      <tr><td><a href="pAlarmasRecorrido.aspx"  style="font-size:12px; font-weight:bold; text-decoration:underline;">Recorridos</a></td>
     </tr>
       <tr><td><a href="pAlarmaRecordatorio.aspx"  style="font-size:12px; font-weight:bold; text-decoration:underline;">Recordatorios</a></td>
     </tr>
     </table>
 
  </div>
  </div>
        </div> 
         <!-- Pop Up Configurar Kms Exceso -->
 <div id="abmKms"  style="display: none">                   
                       <asp:Label ID="Label11" runat="server" Text="Alarma Excesos de Kms" Font-Bold="true" Font-Size="12px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteK" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="12px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridAlarmasExceso" Width="95%" LoadOnce="true"  Height="250px" OnDataRequesting="JQGridAlarmasExceso_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Detalle" HeaderText="Descripción" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Tipo" HeaderText="Frecuencia" PrimaryKey="false" Width="100" />
                     <trirand:JQGridColumn DataField="Kms" HeaderText="Cant.Kms" Width="80" />
                   <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditKms" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>                      
                   
                       <trirand:JQGridColumn DataField="veh_id" HeaderText="veh_id" Width="10" Visible="false" />            
                        </Columns>   
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />             
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button9" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminkms();" />                  
           </div>                                   
                 </div> 
        </div>  
          <!-- Pop Up Configurar Inactivadad -->
 <div id="abmInactivo"  style="display: none">                   
                       <asp:Label ID="Label10" runat="server" Text="Alarma Inactividad" Font-Bold="true" Font-Size="12px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteIn" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="12px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridAlarmasInactividad" Width="95%" LoadOnce="true"  Height="250px" OnDataRequesting="JQGridAlarmasInactividad_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Detalle" HeaderText="Descripción" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Tiempo" HeaderText="Tiempo Inactivo" Width="80" />
                   <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditInactivo" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>                      
                   
                       <trirand:JQGridColumn DataField="veh_id" HeaderText="veh_id" Width="10" Visible="false" />            
                        </Columns>   
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />             
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button8" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminInactivo();" />                  
           </div>                                   
                 </div> 
        </div>   
          <!-- Pop Up Configurar Inicio Actividad -->
 <div id="abmInicio"  style="display: none">                   
                       <asp:Label ID="Label9" runat="server" Text="Alarma Inicio Actividad Diaria" Font-Bold="true" Font-Size="12px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteI" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="12px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridAlarmasInicio" Width="95%" LoadOnce="true"  Height="250px" OnDataRequesting="JQGridAlarmasInicio_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Detalle" HeaderText="Descripción" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Horario" HeaderText="Hora Inicio" Width="80" />
                   <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditInicio" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>                      
                   
                       <trirand:JQGridColumn DataField="veh_id" HeaderText="veh_id" Width="10" Visible="false" />            
                        </Columns>   
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />             
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button7" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminInicio();" />                  
           </div>                                   
                 </div> 
        </div>    
                    <!-- Pop Up Configurar Fuera Horario -->
 <div id="abmHorario"  style="display: none">                   
                       <asp:Label ID="Label5" runat="server" Text="Alarma Uso Fuera de Horario" Font-Bold="true" Font-Size="12px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteH" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="12px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridFueraHorario" Width="95%" LoadOnce="true"  Height="250px" OnDataRequesting="JQGridAlarmasHorario_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Detalle" HeaderText="Descripción" PrimaryKey="false" Width="150" />
                   <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditHorario" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>                      
                   
                       <trirand:JQGridColumn DataField="veh_id" HeaderText="veh_id" Width="10" Visible="false" />            
                        </Columns>   
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />             
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button6" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminHorario();" />                  
           </div>                                   
                 </div> 
        </div>  
            <!-- Pop Up Configurar Zonas -->
 <div id="abmZona"  style="display: none">                   
                       <asp:Label ID="Label2" runat="server" Text="Alarma Desvío de Zonas" Font-Bold="true" Font-Size="12px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPantenteZ" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="12px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridAlarmasZona" Width="95%" LoadOnce="true" OnDataRequesting="JQGridAlarmasZona_DataRequesting" OnCellBinding="JQGridAlarmasZona_CellBinding" Height="250px">
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Detalle" HeaderText="Detalle" PrimaryKey="false" Width="150" />
                  
                      <trirand:JQGridColumn DataField="zon_activa" HeaderText="Activa" Width="50" Editable="true" />
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="85" Editable="true" />
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditZona" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="veh_id" Width="60" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>                      
                      <trirand:JQGridColumn DataField="zon_activa" HeaderText="Act/DesAct" Width="60" Editable="true"  TextAlign="Center"/>   
                       <trirand:JQGridColumn DataField="veh_id" HeaderText="veh_id" Width="10" Visible="false" />            
                        </Columns>   
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />             
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button1" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminZona();" />                  
           </div>                                   
                 </div> 
        </div>  
      <!-- Pop Up Configurar Recorridos -->
 <div id="abmRecorrido"  style="display: none">                   
                       <asp:Label ID="Label6" runat="server" Text="Alarma Desvío de Recorridos" Font-Bold="true" Font-Size="14px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPantenteR" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="14px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridAlarmaRecorrido" Width="100%" OnDataRequesting="JQGridAlarmasRecorrido_DataRequesting" OnCellBinding="JQGridAlarmasRecorrido_CellBinding" Height="250px">
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Descripcion" HeaderText="Descripción" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Origen" HeaderText="Origen" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="Destino" HeaderText="Destino" PrimaryKey="false" Width="150" />
                     <trirand:JQGridColumn DataField="rec_activa" HeaderText="Activa" Width="40" Editable="true" />  
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Mail" Width="40" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="SMS" Width="40" Editable="true" />
                    <trirand:JQGridColumn DataField="rec_id" Width="50" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditRec" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="rec_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>   
                       <trirand:JQGridColumn DataField="rec_activa" HeaderText="Act/DesAct" Width="60" Editable="true" TextAlign="Center" /> 
                            <trirand:JQGridColumn DataField="rec_id" HeaderText="rec_id" Width="10" Visible="false" />       
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
                   <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button2" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminDesvio();" />       
                   
                 </div>                                   
                 </div>  
        </div>  
     <!-- Pop Up Configurar Direcciones -->
 <div id="abmDirecciones"  style="display: none">                   
                       <asp:Label ID="Label4" runat="server" Text="Alarma Entrada y Salida de Direcciones" Font-Bold="true" Font-Size="14px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteD" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="14px"></asp:Label>
    <br />
     <br />                    <div>  
            <trirand:JQGrid runat="server" ID="JQGridDireccion" Width="100%" OnDataRequesting="JQGridAlarmasDirecc_DataRequesting" OnCellBinding="JQGridAlarmasDirecc_CellBinding" Height="250px" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="Id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Direccion" HeaderText="Direccion" PrimaryKey="false" Width="250" />
                                
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Notificar Mail" Width="75" Editable="true" />
                    <trirand:JQGridColumn DataField="Notificar_SMS" HeaderText="Notificar SMS" Width="75" Editable="true" />
                    <trirand:JQGridColumn DataField="dir_id" Width="55" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditDirecc" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="dir_id" Width="40" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                           
                    </trirand:JQGridColumn> 
                      <trirand:JQGridColumn DataField="dir_activa" HeaderText="Act/DesAct" Width="60" Editable="true" TextAlign="Center"/>   
                       <trirand:JQGridColumn DataField="dir_id" HeaderText="dir_id" Width="10" Visible="false" />           
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
             <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button3" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminDirecc();" />       
                   
                 </div>           
                                               
                 </div>  
        </div>
   <!-- Pop Up Configurar Recordatorios -->
 <div id="abmRecFecha"  style="display: none">                   
                       <asp:Label ID="Label8" runat="server" Text="Alarma de Recordatorios" Font-Bold="true" Font-Size="14px" ForeColor="#D85639"></asp:Label><br />
    <asp:Label ID="lblPatenteF" runat="server" Text="Vehículo Patente: " Font-Bold="true" Font-Size="14px"></asp:Label>
    <br />
     <br /> 
    <div id="tabsA"><br />
                            <ul>
                            <li><a href="#tabs-11">Para una Fecha</a></li>  
                                <li><a href="#tabs-21">Por Kilometros Acumulados</a></li>
                            </ul>
                            <div id="tabs-11">  
                <trirand:JQGrid runat="server" ID="JQGridRecordatorioFecha" AutoWidth="true" OnDataRequesting="JQGridAlarmasRecFecha_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="recf_id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Descripcion" HeaderText="Descripción" PrimaryKey="false" Width="200" />
                     <trirand:JQGridColumn DataField="Perciocidad" HeaderText="Periodicidad" PrimaryKey="false" Width="100" /> 
                     <trirand:JQGridColumn DataField="Ocurrencia" HeaderText="Prox.Ocurrencia" PrimaryKey="false" Width="100" /> 
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Notificar Mail" Width="75" Editable="true" />
                
                    <trirand:JQGridColumn DataField="recf_id" Width="55" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditRecFecha" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="recf_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>            
                        </Columns>  
                         <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />              
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
            <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button4" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminRecorrid();" />       
                   
                 </div>
                                                  
                 </div>
  <div id="tabs-21">  
  <trirand:JQGrid runat="server" ID="JQGridRecordatorioKm" AutoWidth="true" OnDataRequesting="JQGridAlarmasRecKm_DataRequesting" >
                <Columns>                    
                 <trirand:JQGridColumn DataField="reck_id" HeaderText="Id" Width="10" Visible="false" />
                    <trirand:JQGridColumn DataField="Patente" HeaderText="Patente" Width="80" />
                    <trirand:JQGridColumn DataField="Descripcion" HeaderText="Descripción" PrimaryKey="false" Width="200" />
                     <trirand:JQGridColumn DataField="Kms" HeaderText="Kms Inic." PrimaryKey="false" Width="100" /> 
                     <trirand:JQGridColumn DataField="Ocurrencia" HeaderText="Prox.Ocurrencia" PrimaryKey="false" Width="100" />                                    
                     <trirand:JQGridColumn DataField="Notificar_Mail" HeaderText="Notificar Mail" Width="75" Editable="true" />
                 
                    <trirand:JQGridColumn DataField="reck_id" Width="55" Searchable="false" HeaderText="Cambiar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatEditRecKm" UnFormatFunction="unformatEdit" />
                        </Formatter>
                      </trirand:JQGridColumn>
                    <trirand:JQGridColumn DataField="reck_id" Width="50" Searchable="false" HeaderText="Eliminar" TextAlign="Center">
                        <Formatter>
                            <trirand:CustomFormatter FormatFunction="formatDelete" UnFormatFunction="unformatDelete" />
                        </Formatter>
                    </trirand:JQGridColumn>            
                        </Columns>                
                <ToolBarSettings ShowRefreshButton="true" ShowViewRowDetailsButton="false">                   
                </ToolBarSettings>
                 <AppearanceSettings AlternateRowBackground="true" ShrinkToFit="false"  />
                <PagerSettings NoRowsMessage="No hay Alertas asociadas al Vehiculo"  />               
            </trirand:JQGrid>
         <div style="text-align:right">                     
                     <br />
                     <asp:Button ID="Button5" runat="server" Text="Nueva Alarma" CssClass="button2" OnClientClick="adminRecord();" />       
                   
                 </div>
                                                 
                 </div>  
                </div>
                <a name="abajo"></a> 
        </div>        
   <script>


       $(function () {
           $('#tabs').tabs('select', document.getElementById("<%= hdnTab.ClientID %>").value);
       });
 </script>
     
</asp:Content>