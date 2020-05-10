Public Class motalbat
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("User_Id") = 1
        If IsPostBack = False Then
            If Session("User_Id") Is Nothing Or Session("User_Id") = 0 Then
                Response.Redirect("10.10.1.10", False)
            End If
        End If
    End Sub

End Class