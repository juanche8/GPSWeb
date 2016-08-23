﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pAdminMarcadores.aspx.vb" Inherits="GPSWeb.pAdminMarcadores" MasterPageFile="~/CMS/SitePages.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?v=3.14&libraries=geometry&sensor=false"> 
</script>
<script type="text/javascript">
   //Declaramos las variables que vamos a user
var lat = null;
var lng = null;
var map = null;
var geocoder = null;
var marker = null;
var markersArray = [];
var infos = [];
var aLatlng = [];

jQuery(document).ready(function () {
    //obtenemos los valores en caso de tenerlos en un formulario ya guardado en la base de datos
    lat = jQuery('#<%= lat.ClientID %>').val();
    lng = jQuery('#<%= lng.ClientID %>').val();
 

    //Inicializamos la función de google maps una vez el DOM este cargado
    initialize();
});
     
    function initialize() {
        var nZoom = 6;
      geocoder = new google.maps.Geocoder();
        
       //Si hay valores creamos un objeto Latlng
       if(lat !='' && lng != '')
      {
         var latLng = new google.maps.LatLng(lat,lng);
      }
      else
      {
        //Si no creamos el objeto con una latitud cualquiera como la de Mar del Plata, Argentina por ej
         var latLng = new google.maps.LatLng(-35, -64);
      }
      //Definimos algunas opciones del mapa a crear
       var myOptions = {
          center: latLng,//centro del mapa
          zoom: 6,//zoom del mapa
          mapTypeId: google.maps.MapTypeId.ROADMAP //tipo de mapa, carretera, híbrido,etc
        };
        //creamos el mapa con las opciones anteriores y le pasamos el elemento div
        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

        searchMarcas(map);

       var latlngbounds = new google.maps.LatLngBounds();
            for (var i = 0; i < aLatlng.length; i++) {
                latlngbounds.extend(aLatlng[i]);
            }
            map.fitBounds(latlngbounds);
               map.setZoom(8);
        
        // Relación del evento de clic sobre el mapa con el
        // procedimiento de georreferenciación inversa

            google.maps.event.addListener(map, 'click', function(event) {
               clearOverlays();
            processReverseGeocoding(event.latLng, showMarker);
        });
        
        //creamos el marcador en el mapa
        marker = new google.maps.Marker({
            map: map,//el mapa creado en el paso anterior
            position: latLng,//objeto con latitud y longitud
            draggable: true //que el marcador se pueda arrastrar
        });
        markersArray.push(marker);
       //función que actualiza los input del formulario con las nuevas latitudes
       //Estos campos suelen ser hidden
        updatePosition(latLng);
       
        
    }

    //ejecuto consulta contra la componente de negocios para traer los marcadores existentes
    function searchMarcas(map) {

        $.ajax({
            async: false,
            type: 'POST',
            url: "../Panel_Control/wsDatos.asmx/GetMarcadoresGenericos",
            data: "",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function(response) {
                var marcas = response.d;
                $.each(marcas, function(index, marca) {

                    var htmlVentana = marca.nombre + "<br/>Direccion: " + marca.direccion;
                    var marker = createMarker2(map, marca.lng, marca.lat, htmlVentana, marca.icono);
                    aLatlng.push(new google.maps.LatLng(marca.lat, marca.lng));
                });

                /*if (marcas.length > 0) {
                map.setCenter(new google.maps.LatLng(marcas[0].lat, marcas[0].lng));
                map.setZoom(11);
                }*/
            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert(textStatus);
            }
        });


    }


    //funcion que traduce la direccion en coordenadas
    function codeAddress() {

        //obtengo la direccion del formulario
        var address = document.getElementById("<%= direccion.ClientID %>").value;

        if (address != '') {
            //elimino el market anterior
            clearOverlays();
            //hago la llamada al geodecoder
            geocoder.geocode({ 'address': address }, function (results, status) {
                //si el estado de la llamado es OK
                if (status == google.maps.GeocoderStatus.OK) {
                    //centro el mapa en las coordenadas obtenidas
                    showMarker(results);
                    // map.setCenter(results[0].geometry.location);
                    //cambio el zoom
                    map.setZoom(15);
                    //coloco el marcador en dichas coordenadas
                    /* marker.setPosition(results[0].geometry.location);
                    //actualizo el formulario     
                    updatePosition(results[0].geometry.location);*/

                    //Añado un listener para cuando el markador se termine de arrastrar
                    //actualize el formulario con las nuevas coordenadas
                    /*  google.maps.event.addListener(marker, 'dragend', function() {
                    updatePosition(marker.getPosition());
                    markersArray.push(marker);

                    });*/
                } else {
                    //si no es OK devuelvo error
                    alert("No podemos encontrar la direcci&oacute;n, error: " + status);
                }
            });
        }
    }

    //funcion que simplemente actualiza los campos del formulario
    function updatePosition(latLng) {
        if (latLng.lat() != "-35" && latLng.lng() != "-64")
            codeLatLng(latLng.lat(), latLng.lng());

        jQuery('#<%= lat.ClientID %>').val(latLng.lat());
        jQuery('#<%= lng.ClientID %>').val(latLng.lng());

    }



    function codeLatLng(lat, lng) {

        //voy ejecutar la busqueda de direccion desde el servidor
        //porque la api de js da error al presionar mas de 20 solicitudes por segundo

        $.ajax({
            async: false,
            type: 'POST',
            url: "../Panel_Control/wsDatos.asmx/GetDireccion",
            data: "{'lat': '" + lat + "','lng': '" + lng + "'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                var direccion = response.d;
                jQuery('#<%= direccion.ClientID %>').val(direccion);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(textStatus);
            }
        });

        /*  var latlng = new google.maps.LatLng(lat, lng);
        geocoder.geocode({ 'latLng': latlng }, function(results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
        if (results[0]) {
        jQuery('#<%= direccion.ClientID %>').val(results[0].formatted_address);
        } else {
        alert('No se encontro la Dirección');
        }
        } else {
        alert('Geocoder failed due to: ' + status);
        }
        });  */
    }

 

  function marcaZoom(lat, lng) {
      map.setCenter(new google.maps.LatLng(lat,lng));
      map.setZoom(16);
  }
  
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
 <div>
 <asp:Label ID="Label4" runat="server" Text="MARCADORES" Font-Bold="true" Font-Size="16px"></asp:Label>
 </div>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">  
  <Triggers> <asp:PostBackTrigger ControlID="btnGuardar" /></Triggers>
    <ContentTemplate>
    <asp:HiddenField ID="hdncli_id" runat="server" />
    <asp:HiddenField ID="lat" runat="server" />
    <asp:HiddenField ID="lng" runat="server" />
    <asp:HiddenField ID="hdnDireccion" runat="server" />
    <asp:HiddenField ID="hdnmarc_id" runat="server" Value="0" />
    <div class="inline" style="border: thin solid #00A6C6; float: left; width: 31%; height: 100%">
    <br />
    <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label><br />
     <asp:Label ID="Label1" runat="server" Text="Ingrese la Direcciones para ubicar el Marcador o seleccione un punto en el Mapa" Font-Bold="true"></asp:Label><br />
     <br />
     <asp:TextBox ID="direccion" runat="server" Width="259px"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="direccion"></asp:RequiredFieldValidator>
  
     <button id="pasar" onclick="codeAddress();">Marcar en Mapa</button>
   <br /><br />
     <asp:Label ID="Label3" runat="server" Text="Nombre Marcador:" Font-Bold="true"></asp:Label>
          <br />
        <asp:TextBox ID="txtNombreMarcador" runat="server" Width="235px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtNombreMarcador"></asp:RequiredFieldValidator>
          <br />
          
          <asp:Label ID="Label6" runat="server" Text="Tipo Marcador:" Font-Bold="true"></asp:Label>
     <br />
        <asp:DropDownList ID="ddlTipoMarcador" runat="server" DataTextField="tipo_marc_nombre" DataValueField="tipo_marc_id">
        </asp:DropDownList>
        <br />
         <br />
          
          <asp:Label ID="Label5" runat="server" Text="Imagen Customizada:" Font-Bold="true"></asp:Label>
     <br />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <br />
        <br />
          <asp:Label ID="Label7" runat="server" Text="Monstrar a:" Font-Bold="true"></asp:Label>
     <br />
        <asp:CheckBox ID="chkTodos" runat="server" Text="Todos los Clientes" Checked="true" />
        <br />
         <br />
          <asp:Label ID="Label8" runat="server" Text="Solo estos Clientes:" ></asp:Label>
        <br />
        <asp:ListBox ID="lstClientes" runat="server" Height="192px" Width="238px" SelectionMode="Multiple"></asp:ListBox>
         
        <br />
         
        <br />
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
        
        <br />
        
        <br />
        <asp:Label ID="Label2" runat="server" Text="Marcadores Creados:" Font-Bold="true"></asp:Label>
        <br />
        <br />
        <asp:DataGrid ID="datagridMarcadores" DataKeyField="marc_id" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="center" 
                    AllowSorting="false" OnItemCommand="datagridMarcadores_itemcommand" >
                        <Columns>
                            <asp:BoundColumn HeaderText="Nombre" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"  DataField="marc_nombre" ></asp:BoundColumn>
                              
                               <asp:TemplateColumn HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                               <ItemTemplate >
                                 <%#(Container.DataItem).Tipos_Marcadores.tipo_marc_nombre%>
                               </ItemTemplate>
                               </asp:TemplateColumn>
                             <asp:BoundColumn HeaderText="Direccion" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" DataField="marc_direccion" ></asp:BoundColumn>
                               <asp:TemplateColumn HeaderText="Ver" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonVer" runat="server" CommandName="Ver" CommandArgument='<%# Eval("marc_id")%>' CausesValidation="false" ImageUrl="~/images/view.gif" ToolTip="Ver"/>
                               </ItemTemplate>
                            </asp:TemplateColumn>
                               <asp:TemplateColumn HeaderText="Editar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonEdit" runat="server" CommandName="Editar" CommandArgument='<%# Eval("marc_id")%>' CausesValidation="false" ImageUrl="~/images/edit.gif" ToolTip="Editar"/>
                               </ItemTemplate>
                            </asp:TemplateColumn>
                               <asp:TemplateColumn HeaderText="Borrar" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imagebuttonBorrar" runat="server" CommandName="Borrar" CommandArgument='<%# Eval("marc_id")%>' CausesValidation="false" ImageUrl="~/images/delete.gif" ToolTip="Delete" OnClientClick="return confirm('Esta Seguro de Eliminar el Marcador Seleccionado?');"/>
                               </ItemTemplate>
                            </asp:TemplateColumn>
                       </Columns>
                      
                     </asp:DataGrid>
                     
      <div>
         <br />
         <a href="#" onclick="initialize();" style="color:#ccc000; font-size:12px;">Ver todos los Marcadores en el mapa</a>
      
      </div>
 </div>
       
    </ContentTemplate>
   </asp:UpdatePanel>
     
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 68%">
<div id="map_canvas" style="width: 95%; height: 700px">

    </div>

   
    </div>
</asp:Content>