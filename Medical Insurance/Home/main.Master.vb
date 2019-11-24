Public Class main
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lbl_date_now.Text = Date.Now.ToLongDateString
    End Sub

End Class