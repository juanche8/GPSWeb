<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TreeExample.aspx.vb" Inherits="GPSWeb.TreeExample" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

   	<meta http-equiv="content-type" content="text/html; charset=iso-8859-1"/>
	
	
	<link rel="stylesheet" href="scripts/jquery-treeview/jquery.treeview.css" />
	<link rel="stylesheet" href="scripts/jquery-treeview/demo/screen.css" />
	
	<script src="scripts/jquery-treeview/lib/jquery.js" type="text/javascript"></script>
	<script src="scripts/jquery-treeview/lib/jquery.cookie.js" type="text/javascript"></script>
	<script src="scripts/jquery-treeview/jquery.treeview.js" type="text/javascript"></script>
	
	<script type="text/javascript" src="scripts/jquery-treeview/demo/demo.js"></script>
	
	</head>
	<body>
	
	<h1 id="banner"><a href="http://bassistance.de/jquery-plugins/jquery-plugin-treeview/">jQuery Treeview Plugin</a> Demo</h1>
	<div id="main">

		
	<h4>MOVILES</h4>
	
	<ul id="navigation">
		<li><a href="?1" style="text-decoration:none; font-weight:bold; color:Black;">Grupo 1</a>
			<ul>
				<li>
                <img alt='auto'  src='images/iconos_movil/autito_verde.gif' style='border-width:0px;' />
                <a onclick='javascript:mostrarVehiculos(1);' href='#' style="text-decoration:none; font-weight:bold; color:Black;">AUTO 1 - MOV187</a>
                 <a title='Reportes' href='pReportes.aspx?veh_id=1'><img alt='Reporte'  src='images/report_ico.png' style='border-width:0px;' /></a>
                  <a title='Alarmas' href='pAlarmas.aspx?veh_id=1'><img alt='Alarmas' src='images/icoWarning.png' style='border-width:0px;' /></a>
                 <a title='Recorridos' href='pHistorialRecorridos.aspx?veh_id=1'><img alt='Recorrido' src='../images/recorrido.jpg' style='border-width:0px;' /></a>
                   <a title='Seguimiento' href='pSeguimientos.aspx?veh_id=1'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>
              
                <ul><li><div>Miguel Font
                04/09/2014-18:28:58<br />
1,046 Kms/H - RPM:120 - Temp: 60 Grados<br />
Buenos Aires ,Monte Grande-Partido de Esteban Echeverría ,Melitón Legarreta ,N</div> 
</li></ul>
                </li>
				<li> <img alt='auto'  src='images/iconos_movil/autito_rojo.gif' style='border-width:0px;' />
                <a onclick='javascript:mostrarVehiculos(1);' href='#' style="text-decoration:none; font-weight:bold; color:Black;">AUTO 2 - NRE589</a>
                 <a title='Reportes' href='pReportes.aspx?veh_id=1'><img alt='Reporte'  src='images/report_ico.png' style='border-width:0px;' /></a>
                  <a title='Alarmas' href='pAlarmas.aspx?veh_id=1'><img alt='Alarmas' src='images/icoWarning.png' style='border-width:0px;' /></a>
                 <a title='Recorridos' href='pHistorialRecorridos.aspx?veh_id=1'><img alt='Recorrido' src='../images/recorrido.jpg' style='border-width:0px;' /></a>
                   <a title='Seguimiento' href='pSeguimientos.aspx?veh_id=1'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>
              
                <ul><li><div>Juan<br/> 
10/06/2014-12:54:14<br/> 
0,000 Kms/H - RPM: 1200 - Temp: 25 Grados</br> 
Buenos Aires ,Monte Grande ,M. Legarreta 400-498 ,SE<br/> 
casa juan</div></li></ul>
                </li>
				<li>
                <img alt='auto'  src='images/iconos_movil/autito_amarillo.gif' style='border-width:0px;' />
                <a onclick='javascript:mostrarVehiculos(1);' href='#' style="text-decoration:none; font-weight:bold; color:Black;">AUTO 3 - LOB478</a>
                 <a title='Reportes' href='pReportes.aspx?veh_id=1'><img alt='Reporte'  src='images/report_ico.png' style='border-width:0px;' /></a>
                  <a title='Alarmas' href='pAlarmas.aspx?veh_id=1'><img alt='Alarmas' src='images/icoWarning.png' style='border-width:0px;' /></a>
                 <a title='Recorridos' href='pHistorialRecorridos.aspx?veh_id=1'><img alt='Recorrido' src='../images/recorrido.jpg' style='border-width:0px;' /></a>
                   <a title='Seguimiento' href='pSeguimientos.aspx?veh_id=1'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>
              
                <ul><li><div>Martin Gomez
                04/09/2014-18:28:58<br />
1,046 Kms/H - RPM:120 - Temp: 60 Grados<br />
Buenos Aires ,Monte Grande-Partido de Esteban Echeverría ,Melitón Legarreta ,N</div> 
</li></ul>
                </li>
			</ul>
		</li>
		<li><a href="?2" style="text-decoration:none; font-weight:bold; color:Black;">Grupo 2</a>
			<ul>
			
				<li> <img alt='auto'  src='images/iconos_movil/autito_rojo.gif' style='border-width:0px;' />
                <a onclick='javascript:mostrarVehiculos(1);' href='#' style="text-decoration:none; font-weight:bold; color:Black;">AUTO 2 - JUY058</a>
                 <a title='Reportes' href='pReportes.aspx?veh_id=1'><img alt='Reporte'  src='images/report_ico.png' style='border-width:0px;' /></a>
                  <a title='Alarmas' href='pAlarmas.aspx?veh_id=1'><img alt='Alarmas' src='images/icoWarning.png' style='border-width:0px;' /></a>
                 <a title='Recorridos' href='pHistorialRecorridos.aspx?veh_id=1'><img alt='Recorrido' src='../images/recorrido.jpg' style='border-width:0px;' /></a>
                   <a title='Seguimiento' href='pSeguimientos.aspx?veh_id=1'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>
              
                <ul><li><div>Javier Diez<br/> 
10/06/2014-12:54:14<br/> 
0,000 Kms/H - RPM: 1200 - Temp: 25 Grados</br> 
Buenos Aires ,Monte Grande ,M. Legarreta 400-498 ,SE<br/> 
casa juan</div></li></ul>
                </li>
				<li>
                <img alt='auto'  src='images/iconos_movil/autito_amarillo.gif' style='border-width:0px;' />
                <a onclick='javascript:mostrarVehiculos(1);' href='#' style="text-decoration:none; font-weight:bold; color:Black;">AUTO 3 - DER698</a>
                 <a title='Reportes' href='pReportes.aspx?veh_id=1'><img alt='Reporte'  src='images/report_ico.png' style='border-width:0px;' /></a>
                  <a title='Alarmas' href='pAlarmas.aspx?veh_id=1'><img alt='Alarmas' src='images/icoWarning.png' style='border-width:0px;' /></a>
                 <a title='Recorridos' href='pHistorialRecorridos.aspx?veh_id=1'><img alt='Recorrido' src='../images/recorrido.jpg' style='border-width:0px;' /></a>
                   <a title='Seguimiento' href='pSeguimientos.aspx?veh_id=1'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>
              
                <ul><li><div>Luis Luna
                04/09/2014-18:28:58<br />
1,046 Kms/H - RPM:120 - Temp: 60 Grados<br />
Buenos Aires ,Monte Grande-Partido de Esteban Echeverría ,Melitón Legarreta ,N</div> 
</li></ul>
                </li>
			</ul>
		</li>
		<li>
                <img alt='auto'  src='images/iconos_movil/autito_verde.gif' style='border-width:0px;' />
                <a onclick='javascript:mostrarVehiculos(1);' href='#' style="text-decoration:none; font-weight:bold; color:Black;">AUTO 12 -JKL123</a>
                 <a title='Reportes' href='pReportes.aspx?veh_id=1'><img alt='Reporte'  src='images/report_ico.png' style='border-width:0px;' /></a>
                  <a title='Alarmas' href='pAlarmas.aspx?veh_id=1'><img alt='Alarmas' src='images/icoWarning.png' style='border-width:0px;' /></a>
                 <a title='Recorridos' href='pHistorialRecorridos.aspx?veh_id=1'><img alt='Recorrido' src='../images/recorrido.jpg' style='border-width:0px;' /></a>
                   <a title='Seguimiento' href='pSeguimientos.aspx?veh_id=1'><img alt='Seguimiento' src='../images/i_map.png' style='border-width:0px;' /></a>
              
                <ul><li><div>Raul Roca
                04/09/2014-18:28:58<br />
1,046 Kms/H - RPM:120 - Temp: 60 Grados<br />
Buenos Aires ,Monte Grande-Partido de Esteban Echeverría ,Melitón Legarreta ,N</div> 
</li></ul>
                </li>
	</ul>

	
</div>

</body>
</html>
 