Imports System.Data.SqlClient

Public Class listApprovedPendingRequests
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            getApprovedPendingRequests()
        End If
    End Sub

    'خدمات قدمت وفي انتظار الموافقة
    Sub getApprovedPendingRequests()
        Try

            Dim sql_str As String = "SELECT CONFIRM_ID, APPROVED_VALUE, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS P_NAME, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS BAGE_NO, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = ((SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID))) AS C_NAME, (SELECT SUM(SERVICE_PRICE) FROM INC_CONFIRM_DETAILS WHERE INC_CONFIRM_DETAILS.CONFIRM_ID = INC_CONFIRM.CONFIRM_ID GROUP BY CONFIRM_ID) AS TOTAL_VALUE, CONFRIM_END_DATE, PENDING_VALUE, (CASE WHEN (REQUEST_UNIT = 1) THEN 'قسم التأمين الصحي' ELSE 'الإيواء والعمليات' END) AS REQUEST_TYPE FROM INC_CONFIRM WHERE REQUEST_STS = 1 AND EWA_NO <> 0 ORDER BY CONFIRM_ID DESC"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                GridView2.DataSource = dt_result
                GridView2.DataBind()
            Else
                dt_result.Rows.Clear()
                GridView2.DataSource = dt_result
                GridView2.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand
        If (e.CommandName = "approval_req") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView2.Rows(index)

            Dim confirm_val As TextBox = row.FindControl("txt_confirm_price")

            If confirm_val.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('خطأ! يجب إدخال قيمة الموافقة');", True)
                Exit Sub
            End If

            If CDec(confirm_val.Text) < CDec(row.Cells(7).Text) Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "Confirm();", True)
            End If

            If CDec(confirm_val.Text) > CDec(row.Cells(7).Text) Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('خطأ! قيمة الموافقة أكبر من قيمة العملية');", True)
                Exit Sub
            End If

            Dim update_sts As New SqlCommand("UPDATE INC_CONFIRM SET PENDING_VALUE = " & (CDec(row.Cells(7).Text) - CDec(confirm_val.Text)) & ", APPROVED_VALUE = " & (CDec(row.Cells(6).Text) + confirm_val.Text) & " WHERE REQUEST_STS = 1 AND CONFIRM_ID = " & (row.Cells(0).Text), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getApprovedPendingRequests()

        End If

        If (e.CommandName = "reject_req") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView2.Rows(index)

            Dim update_sts As New SqlCommand("UPDATE INC_CONFIRM SET REQUEST_STS = 2 WHERE REQUEST_STS = 1 AND PENDING_VALUE <> 0 AND EWA_NO <> 0 AND CONFIRM_ID = " & (row.Cells(0).Text), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getApprovedPendingRequests()

        End If
    End Sub

End Class