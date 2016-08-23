Imports GPS.Data
Imports GPS.Business
Public Class pAddGrupo

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'cargo el listado de vehiculos y por cada uno tengo que buscar si tienen configurada la alarma
            If Session("Cliente") IsNot Nothing Then
                If Not IsPostBack Then

                    hdncli_id.Value = Session("Cliente").ToString()

                    'verifico si tengo q editar el grupo
                    If Request.Params("grup_id") IsNot Nothing Then
                        hdngrup_id.Value = Request.Params("grup_id").ToString()

                        txtnombre.Text = clsGrupo.SelectById(CInt(hdngrup_id.Value)).grup_nombre
                        lblTitulo.Text = "MODIFICAR GRUPO DE MOVILES"
                    End If
                    'muestro los moviles que no pertenecen a otros grupos
                    cargarMoviles()
                End If

            Else
                'no esta logeado
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "redirect", " <script>parent.iraLogin();</script>")
            End If


        Catch ex As Exception
            Funciones.WriteToEventLog("Add Grupo - " + ex.Message + " - " + ex.StackTrace)
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Try

            Dim seleccionoMovil = False

            For Each check As ListItem In chkMoviles.Items

                If check.Selected Then
                    seleccionoMovil = True
                End If
            Next

            If Not seleccionoMovil Then
                valInquiry.IsValid = False
                Exit Sub
            End If

            'valido que no exista el nombre del grupo
            Dim _grupo As Grupos = New Grupos()

            _grupo = clsGrupo.SelectByNombre(CInt(hdncli_id.Value), txtnombre.Text)
            If _grupo IsNot Nothing Then
                If _grupo.grup_id <> CInt(hdngrup_id.Value) Then
                    lblError.Text = "Ya existe un Grupo con igual Nombre."
                    Exit Sub
                End If
            End If


            'verifico si es alta o modificacion

            If hdngrup_id.Value = "0" Then
                _grupo = New Grupos()
                _grupo.cli_id = CInt(hdncli_id.Value)
                _grupo.grup_nombre = txtnombre.Text

                clsGrupo.Insert(_grupo)
            Else
                _grupo = clsGrupo.SelectById(CInt(hdngrup_id.Value))
                _grupo.grup_nombre = txtnombre.Text
                clsGrupo.Update(_grupo)

                'elimino los moviles y los agrego de nuevo
                clsGrupo.DeleteMoviles(_grupo.grup_id)

            End If


            'inserto la relacion con los moviles

            For Each check As ListItem In chkMoviles.Items

                If check.Selected Then
                    Dim _grupoMovil As Grupos_Vehiculos = New Grupos_Vehiculos()

                    _grupoMovil.grup_id = _grupo.grup_id
                    _grupoMovil.veh_id = CInt(check.Value)

                    clsGrupo.InsertGrupo(_grupoMovil)

                End If
            Next

            'cierro y actualizo el combo
            Literal1.Text = "<script language='javascript'>window.parent.cerrar();</script" + ">"
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub valInquiry_ServerValidation(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        args.IsValid = chkMoviles.SelectedItem IsNot Nothing
    End Sub



    Private Sub cargarMoviles()

        'los que no estan en grupos
        Dim vehiculosGrupo As List(Of Vehiculo)
        Dim vehiculos As List(Of Vehiculo) = clsVehiculo.ListNotGrupo(CInt(hdncli_id.Value))

        ' los que estan en este grupo si estoy editando
       
        If vehiculos.Count > 0 Then
            Dim cboitem As ListItem

            For Each movil As Vehiculo In vehiculos
                cboitem = New ListItem(movil.veh_descripcion + " - " + movil.veh_patente, movil.veh_id.ToString)
                'verifico si esta en el grupo y lo marco
                If clsGrupo.PerteneceGrupo(CInt(hdngrup_id.Value), movil.veh_id) IsNot Nothing Then
                    cboitem.Selected = True
                End If
                chkMoviles.Items.Add(cboitem)
            Next


        End If

        If hdngrup_id.Value <> "0" Then
            vehiculosGrupo = clsVehiculo.ListActivosGrupo(CInt(hdngrup_id.Value), "")

            ' los que estan en este grupo si estoy editando

            If vehiculosGrupo.Count > 0 Then
                Dim cboitem As ListItem

                For Each movil As Vehiculo In vehiculosGrupo
                    cboitem = New ListItem(movil.veh_descripcion + " - " + movil.veh_patente, movil.veh_id.ToString)
                    'verifico si esta en el grupo y lo marco
                    If clsGrupo.PerteneceGrupo(CInt(hdngrup_id.Value), movil.veh_id) IsNot Nothing Then
                        cboitem.Selected = True
                    End If
                    chkMoviles.Items.Add(cboitem)
                Next


            End If
        Else

            If vehiculos.Count = 0 Then lblError.Text = "No posee Moviles libres para Agregar al Grupo."
        End If



    End Sub
End Class