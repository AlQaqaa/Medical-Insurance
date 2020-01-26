Public Class main
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            lbl_date_now.Text = Date.Now.ToLongDateString
            lbl_notification.Text = "<span class='badge badge-info'>2</span>"
        End If
    End Sub

End Class