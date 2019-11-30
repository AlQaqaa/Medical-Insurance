Public Class companies
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then
            hl_company_users_list.NavigateUrl = "~/Companies/LISTPATIANT.aspx?cID=" & Session("company_id")
        End If

    End Sub

End Class