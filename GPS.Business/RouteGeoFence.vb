Public Class RouteGeoFence
    Public Class Point
        Public X As Double
        Public Y As Double
    End Class
    Public Class PolyGon
        Public myPts As New List(Of Point)()
        Public Sub New()
        End Sub
        Public Sub New(ByVal points As List(Of Point))
            For Each p As Point In points
                Me.myPts.Add(p)
            Next
        End Sub
        Public Sub Add(ByVal p As Point)
            Me.myPts.Add(p)
        End Sub
        Public Function Count() As Integer
            Return myPts.Count
        End Function


        '  The function will return true if the point x,y is inside the polygon, or
        '  false if it is not.  If the point is exactly on the edge of the polygon,
        '  then the function may return true or false.

        Public Function FindPoint(ByVal X As Double, ByVal Y As Double) As Boolean
            Dim sides As Integer = Me.Count() - 1
            Dim j As Integer = sides - 1
            Dim pointStatus As Boolean = False
            For i As Integer = 0 To sides - 1
                If ((myPts(i).Y > Y) <> (myPts(j).Y > Y)) AndAlso (X < (myPts(j).X - myPts(i).X) * (Y - myPts(i).Y) / (myPts(j).Y - myPts(i).Y) + myPts(i).X) Then

                    '  If myPts(i).Y < Y AndAlso myPts(j).Y >= Y OrElse myPts(j).Y < Y AndAlso myPts(i).Y >= Y Then
                    '  If myPts(i).X + (Y - myPts(i).Y) / (myPts(j).Y - myPts(i).Y) * (myPts(j).X - myPts(i).X) < X Then
                        pointStatus = Not pointStatus
                End If

                j = i
            Next

           

            Return pointStatus
        End Function
    End Class

End Class
