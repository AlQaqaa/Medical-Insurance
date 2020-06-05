Public Class motalbat
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then
            If Session("INC_User_Id") Is Nothing Or Session("INC_User_Id") = 0 Then
                Response.Redirect("http://10.10.1.10", True)
            End If

            If Session("systemlogin") <> "401" Then
                Response.Redirect("http://10.10.1.10", True)
            End If

            HyperLink2.Visible = Session("User_per")("active_company")
            hl_listPatiant.Visible = Session("User_per")("active_card")
            hl_services_prices.Visible = Session("User_per")("services_prices")
            hr_confirm.Visible = Session("User_per")("confirm_approval")
            hl_doctors_forms.Visible = Session("User_per")("doctors_settled")
            hl_Statistics.Visible = Session("User_per")("search")
            hl_reports.Visible = Session("User_per")("search")

            If Session("User_per")("new_invoice_opd") = False And Session("User_per")("new_invoice_ewa") = False And Session("User_per")("print_motalba") = False And Session("User_per")("return_motalba") = False Then
                hl_invoices_motalbat.Visible = False
            End If

        End If

    End Sub

End Class