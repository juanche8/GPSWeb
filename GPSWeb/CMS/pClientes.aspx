<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pClientes.aspx.vb" Inherits="GPSWeb.pClientes" MasterPageFile="~/CMS/SitePages.Master" Culture="Auto" UICulture="Auto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
<script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdncli_id" runat="server" Value="0"/>
     <asp:HiddenField ID="hdnCont1" runat="server" Value="0" />
     <asp:HiddenField ID="hdnCont2" runat="server" Value="0" />
      <asp:HiddenField ID="hdnCont3" runat="server" Value="0" />
 <div class="inline" style="border: thin solid #00A6C6; float: left; width: 80%; height: 100%">
    <asp:Label ID="lbltitulo" runat="server" Text="ADMINISTRAR CLIENTES" Font-Bold="true" Font-Size="16px"></asp:Label>
    <br />
     <br />
          <asp:Label ID="lblerror" runat="server" Text="" Font-Bold="true" Font-Size="12px" ForeColor="Red"></asp:Label>
          <div id="tabs">
                            <ul>
                                <li><a href="#tabs-1">Datos del Cliente</a></li>                               
                                <li><a href="#tabs-3">Datos Contacto</a></li> 
                                                            
                            </ul>
                  <div id="tabs-1">
                  <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
                    <ContentTemplate>
                         <br />
                      <div>  <div><asp:Label ID="Label1" runat="server" Text="Nombre:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtNombre" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
                                
                             </div>
                       </div>    
                        <div>  <div><asp:Label ID="Label22" runat="server" Text="Apellido:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtApellido" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtApellido"></asp:RequiredFieldValidator>
                             </div>
                       </div> 
                       <div>  <div><asp:Label ID="Label25" runat="server" Text="Nro. Documento:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtDni" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                               </div>
                       </div> 
                        <div>  <div><asp:Label ID="Label26" runat="server" Text="Fecha Nacimiento:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtFechaNac" runat="server" Width="301px" MaxLength="10"></asp:TextBox>
                              <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaNac" PopupButtonID="txtFechaNac"/>
                               <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El formato de fecha es invalido"
                                                        ControlToValidate="txtFechaNac" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))"
                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                             </div>
                       </div>                     
                       <div><br />
                             <div><asp:Label ID="Label3" runat="server" Text="Direccion:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtDireccion" runat="server" Width="301px" MaxLength="100"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtDireccion"></asp:RequiredFieldValidator>
                              </div>
                          
                       </div>
                       <div><br />
                             <div><asp:Label ID="Label28" runat="server" Text="Código Postal:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtCP" runat="server" Width="301px" MaxLength="100"></asp:TextBox>
                             </div>
                          
                       </div>
                       <div><br />
                             <div><asp:Label ID="Label4" runat="server" Text="Telefono" Font-Bold="true"></asp:Label></div>      
                             <div>
                                 <asp:TextBox ID="txtTelefono" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtTelefono"></asp:RequiredFieldValidator>
                             </div>    
                       </div>
                       <div> <br />
                             <div><asp:Label ID="Label5" runat="server" Text="Tipo Cliente" Font-Bold="true"></asp:Label></div>      
                             <div> 
                                 <asp:DropDownList ID="ddlTipoCliente" runat="server" AutoPostBack="true" DataValueField="tipo_cli_id" DataTextField="tipo_cli_nombre">
                                 </asp:DropDownList>                            
                                 </div><br />  
                          </div>
                           <asp:Panel ID="PanelEmpresa" runat="server">
                             <div>  <div><asp:Label ID="Label8" runat="server" Text="Razón Social:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtRazonSocial" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                                  <br />
                             </div>
                       </div>
                       <div>
                             <div><asp:Label ID="Label10" runat="server" Text="CUIT:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtCUIT" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                 
                             </div>
                       </div>
                       <div><br />
                             <div><asp:Label ID="Label12" runat="server" Text="Direccion:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtDireccionEmpresa" runat="server" Width="301px" MaxLength="150"></asp:TextBox>
                                 <br />
                             </div>
                          
                       </div>
                       <div>
                             <div><asp:Label ID="Label13" runat="server" Text="Código Postal" Font-Bold="true"></asp:Label></div>      
                             <div>
                                 <asp:TextBox ID="txtCPEmpresa" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                                
                             </div>      
                       </div>
                       <div> <br />
                             <div><asp:Label ID="Label14" runat="server" Text="Teléfono 1" Font-Bold="true"></asp:Label></div>      
                             <div> 
                                <asp:TextBox ID="txtTel1Empresa" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                
                                 </div>
                          </div>
                             <div> <br />
                             <div><asp:Label ID="Label15" runat="server" Text="Teléfono 2" Font-Bold="true"></asp:Label></div>      
                             <div>
                                <asp:TextBox ID="txtTel2Empresa" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                                
                                 </div>
                          </div>       
                     </asp:Panel>
                         <div><br />
                             <div><asp:Label ID="Label27" runat="server" Text="Forma de Pago" Font-Bold="true"></asp:Label></div>      
                             <div>
                                 <asp:TextBox ID="txtFormaPago" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                              </div>      
                       </div>   
                        <div> <br />
                             <div><asp:Label ID="Label7" runat="server" Text="Activo" Font-Bold="true"></asp:Label></div>      
                             <div>          
                                 <asp:RadioButtonList ID="rdnActivo" runat="server" RepeatDirection="Horizontal">
                                     <asp:ListItem Value="True" Selected="True">Si</asp:ListItem>
                                     <asp:ListItem Value="False">No</asp:ListItem>
                                 </asp:RadioButtonList>
                                </div>      
                       </div>
                        <div>
                        <br />
                             <div><asp:Label ID="Label9" runat="server" Text="E-Mail (User Login)" Font-Bold="true"></asp:Label></div>
                          <div>
                             
                            <asp:TextBox ID="txtMail" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtMail"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="regularExpressionValidator1" runat="server" ErrorMessage="Formato Mail Incorrecto" SkinID="regularexpressionvalidator" Display="Static" ControlToValidate="txtMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

                            </div>      
                       </div>
                        <div> <br />
                             <div><asp:Label ID="Label11" runat="server" Text="Contraseña" Font-Bold="true"></asp:Label></div>
                          <div>
                            <asp:TextBox ID="txtContrasenia" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtContrasenia"></asp:RequiredFieldValidator>
                                 </div>      
                       </div>  
                     
                                   
                     </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                
                  <div id="tabs-3">
                  <div> <asp:Label ID="Label24" runat="server" Text="CONTACTO 1" Font-Bold="true"></asp:Label></div>
                         <div>  <div><asp:Label ID="Label20" runat="server" Text="Nombre:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtNombreContacto1" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                                  <br />
                             </div>
                       </div>
                       <div>
                             <div><asp:Label ID="Label21" runat="server" Text="Apellido" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtApellidoContacto1" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                 
                             </div>
                       </div>               
                       <div> <br />
                             <div><asp:Label ID="Label23" runat="server" Text="Teléfono" Font-Bold="true"></asp:Label></div>      
                             <div> 
                                <asp:TextBox ID="txtTel1Contacto" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                
                                 </div><br />
                          </div>
                        <div> <asp:Label ID="Label31" runat="server" Text="CONTACTO 2" Font-Bold="true"></asp:Label></div>
                          <div>  <div><asp:Label ID="Label2" runat="server" Text="Nombre:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtNombreContacto2" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                                  <br />
                             </div>
                       </div>
                       <div>
                             <div><asp:Label ID="Label6" runat="server" Text="Apellido" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtApellidoContacto2" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                 
                             </div>
                       </div>               
                       <div> <br />
                             <div><asp:Label ID="Label16" runat="server" Text="Teléfono" Font-Bold="true"></asp:Label></div>      
                             <div> 
                                <asp:TextBox ID="txtTelContacto2" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                
                                 </div><br />
                          </div>
                <div> <asp:Label ID="Label32" runat="server" Text="CONTACTO 3" Font-Bold="true"></asp:Label></div>
                          <div>  <div><asp:Label ID="Label17" runat="server" Text="Nombre:" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtNombreContacto3" runat="server" Width="301px" MaxLength="50"></asp:TextBox>
                                  <br />
                             </div>
                       </div>
                       <div>
                             <div><asp:Label ID="Label18" runat="server" Text="Apellido" Font-Bold="true"></asp:Label></div>      
                             <div><asp:TextBox ID="txtApellidoContacto3" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                 
                             </div>
                       </div>               
                       <div> <br />
                             <div><asp:Label ID="Label19" runat="server" Text="Teléfono" Font-Bold="true"></asp:Label></div>      
                             <div> 
                                <asp:TextBox ID="txtTelContacto3" runat="server" Width="301px" MaxLength="50"></asp:TextBox>                                
                                 </div>
                          </div>
                             
                 </div>
             
       
       <div>
       <br />
                         <asp:Button ID="Button2" runat="server" Text="Cancelar" PostBackUrl="~/CMS/pAdminClientes.aspx" CausesValidation="false" />
                         &nbsp;
                         <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
       </div>
                 
 </div>

 </div>
</asp:Content>