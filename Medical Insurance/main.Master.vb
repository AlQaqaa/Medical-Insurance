Imports System.Data.SqlClient
Public Class main
    Inherits System.Web.UI.MasterPage

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then


            If Session("INC_User_Id") Is Nothing Or Session("INC_User_Id") = 0 Then
                Response.Redirect("http://10.10.1.10", True)
            End If

            If Session("systemlogin") <> "401" Then
                Response.Redirect("http://10.10.1.10", True)
            End If

            Session("company_id") = Nothing

            lbl_date_now.Text = Date.Now.ToLongDateString
            lbl_user_name.Text = Session("INC_user_full_name")

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                HyperLink2.Visible = Session("User_per")("active_company")
                hl_listPatiant.Visible = Session("User_per")("active_card")
                hr_confirm.Visible = Session("User_per")("confirm_approval")
                ' hl_doctors_forms.Visible = Session("User_per")("doctors_settled")
                hl_Statistics.Visible = Session("User_per")("search")
                hl_reports.Visible = Session("User_per")("reports_per")

                If Session("User_per")("services_prices") = False And Session("User_per")("create_profile_prices") = False Then
                    hl_services_prices.Visible = False
                End If

                If Session("User_per")("new_invoice_opd") = False And Session("User_per")("new_invoice_ewa") = False And Session("User_per")("print_motalba") = False And Session("User_per")("return_motalba") = False Then
                    hl_invoices_motalbat.Visible = False
                End If
            End If

        End If


    End Sub

End Class