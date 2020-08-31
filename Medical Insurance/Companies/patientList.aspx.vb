Imports System.Data.SqlClient
Public Class patientList
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("active_card") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If

            Dim company_no As Integer = Val(Request.QueryString("cID"))

            If company_no <> 0 Then

                ' جلب بيانات الشركة
                Dim get_comp As New SqlCommand("SELECT C_NAME_ARB,C_NAME_ENG FROM INC_COMPANY_DATA WHERE C_id = " & company_no, insurance_SQLcon)
                Dim dt_comp As New DataTable
                dt_comp.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_comp.Load(get_comp.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_comp.Rows.Count > 0 Then
                    Dim dr_company = dt_comp.Rows(0)
                    Page.Title = "منتفعي شركة " & dr_company!C_NAME_ARB
                End If
            Else
                Session("company_id") = Nothing
                Page.Title = "المنتفعين"

                Dim pnl As Panel = DirectCast(Master.FindControl("panel1"), Panel)
                pnl.Visible = False
                Dim pnl_company_name As Panel = DirectCast(Master.FindControl("Panel_company_info"), Panel)
                pnl_company_name.Visible = False

            End If

        End If
    End Sub

End Class