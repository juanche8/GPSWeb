

Imports GPS.Data
Imports System.Collections.Generic
Imports System.Linq

Public Class clsPlan

    Public Shared Function List() As List(Of Plane)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Planes.ToList()

    End Function

    Public Shared Function ListParametros(ByVal plan_id As Integer) As List(Of Planes_Parametro)
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Planes_Parametros.Where(Function(v) v.plan_id = plan_id).ToList()

    End Function

    Public Shared Function SearchParametro(ByVal plan_id As Integer, ByVal paramName As String) As Planes_Parametro
        Dim DB As New DataClassesGPSDataContext()
        Return DB.Planes_Parametros.Where(Function(v) v.plan_id = plan_id And v.plan_item_nombre = paramName).FirstOrDefault()

    End Function

    Public Shared Function Insert(ByVal plan As Plane) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Planes.InsertOnSubmit(plan)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function InsertParametros(ByVal parametro As Planes_Parametro) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                DB.Planes_Parametros.InsertOnSubmit(parametro)
                DB.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception

            Throw ex
        End Try
    End Function

    Public Shared Function Update(ByVal plan As Plane) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Plane = DB.Planes.Where(Function(b) b.plan_id = plan.plan_id).FirstOrDefault()

                oOriginal.plan_nombre = plan.plan_nombre
               
                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateParametro(ByVal plan As Plane) As [Boolean]
        Try
            Using DB As New DataClassesGPSDataContext()
                Dim oOriginal As Plane = DB.Planes.Where(Function(b) b.plan_id = plan.plan_id).FirstOrDefault()

                oOriginal.plan_nombre = plan.plan_nombre

                DB.SubmitChanges()

                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
