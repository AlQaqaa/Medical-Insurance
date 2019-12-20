Imports System.Data.SqlClient
Public Class SERVICES_PRICES
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
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
            insClinic.Parameters.AddWithValue("@last_update", txt_start_dt.Text)
            insurance_SQLcon.Open()
            insClinic.ExecuteNonQuery()
            insurance_SQLcon.Close()
            insClinic.CommandText = ""
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

        Next
    End Sub

    Private Sub ddl_services_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services.SelectedIndexChanged
        Try

            Dim CASH_PRS As String = "ISNULL((SELECT CASH_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID), 0) AS CASH_PRS,"
            Dim INS_PRS As String = "ISNULL((SELECT INS_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID), 0) AS INS_PRS,"
            Dim INVO_PRS As String = "ISNULL((SELECT INVO_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID), 0) AS INVO_PRS "
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
        End Try
    End Sub
End Class