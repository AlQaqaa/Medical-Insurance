Imports System.Data.SqlClient

Public Class companyStatistics
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            Dim company_no As Integer = Val(Request.QueryString("cID"))

            If company_no <> 0 Then
                main_company_panel.Visible = False

            Else
                main_company_panel.Visible = True

                Page.Title = "إحصائيات"
                Label1.Text = "إحصائيات"

                Dim pnl As Panel = DirectCast(Master.FindControl("panel1"), Panel)
                pnl.Visible = False

            End If

        End If

    End Sub

End Class