Imports System.Data.SqlClient
Imports System.Globalization

Public Class SERVICES_PRICES
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Val(Session("profile_no")) = 0 Then
                Response.Redirect("createProfilePrices.aspx", False)
            Else
                Dim sel_profile As New SqlCommand("SELECT PROFILE_NAME FROM INC_PRICES_PROFILES WHERE PROFILE_ID = " & Session("profile_no"), insurance_SQLcon)
                Dim dt_name As New DataTable
                dt_name.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_name.Load(sel_profile.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_name.Rows.Count > 0 Then
                    Dim dr_name = dt_name.Rows(0)
                    txt_profile_name.Text = dr_name!PROFILE_NAME
                End If
            End If
        End If
    End Sub

    Protected Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        

        Try
            For Each dd As GridViewRow In GridView1.Rows
                Dim txt_private_prc As TextBox = dd.FindControl("txt_private_price")
                Dim txt_inc_prc As TextBox = dd.FindControl("txt_inc_price")
                Dim txt_invoice_prc As TextBox = dd.FindControl("txt_invoice_price")

                'If ch.Checked = True Then
                Dim insClinic As New SqlCommand
                insClinic.Connection = insurance_SQLcon
                insClinic.CommandText = "INC_addServicesPrice"
                insClinic.CommandType = CommandType.StoredProcedure
                insClinic.Parameters.AddWithValue("@service_id", dd.Cells(0).Text)
                insClinic.Parameters.AddWithValue("@private_prc", CDec(txt_private_prc.Text))
                insClinic.Parameters.AddWithValue("@inc_prc", CDec(txt_inc_prc.Text))
                insClinic.Parameters.AddWithValue("@inv_prc", CDec(txt_invoice_prc.Text))
                insClinic.Parameters.AddWithValue("@user_id", 1)
                insClinic.Parameters.AddWithValue("@user_ip", GetIPAddress())
                insClinic.Parameters.AddWithValue("@profile_price_id", Val(Session("profile_no")))
                insurance_SQLcon.Open()
                insClinic.ExecuteNonQuery()
                insurance_SQLcon.Close()
                insClinic.CommandText = ""
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub

    Private Sub ddl_services_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services.SelectedIndexChanged
        Try

            Dim CASH_PRS As String = "ISNULL((SELECT top(1) CASH_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND PROFILE_PRICE_ID = " & Val(Session("profile_no")) & " order by n DESC), 0) AS CASH_PRS,"
            Dim INS_PRS As String = "ISNULL((SELECT top(1) INS_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND PROFILE_PRICE_ID = " & Val(Session("profile_no")) & " order by n DESC), 0) AS INS_PRS,"
            Dim INVO_PRS As String = "ISNULL((SELECT top(1) INVO_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND PROFILE_PRICE_ID = " & Val(Session("profile_no")) & " order by n DESC), 0) AS INVO_PRS "

            Dim sel_com As New SqlCommand("SELECT SubService_ID, SubService_Code, SubService_AR_Name, SubService_EN_Name, " & CASH_PRS & INS_PRS & INVO_PRS & " FROM Main_SubServices WHERE SubService_Service_ID = " & ddl_services.SelectedValue, insurance_SQLcon)
            Dim dt_res As New DataTable
            dt_res.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_res.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_res.Rows.Count > 0 Then
                GridView1.DataSource = dt_res
                GridView1.DataBind()
                For i = 0 To dt_res.Rows.Count - 1
                    Dim dd As GridViewRow = GridView1.Rows(i)

                    Dim txt_private_prc As TextBox = dd.FindControl("txt_private_price")
                    Dim txt_inc_prc As TextBox = dd.FindControl("txt_inc_price")
                    Dim txt_invoice_prc As TextBox = dd.FindControl("txt_invoice_price")

                    txt_private_prc.Text = dt_res.Rows(i)("CASH_PRS")
                    txt_inc_prc.Text = dt_res.Rows(i)("INS_PRS")
                    txt_invoice_prc.Text = dt_res.Rows(i)("INVO_PRS")
                Next
            Else
                dt_res.Rows.Clear()
                GridView1.DataSource = dt_res
                GridView1.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub
End Class