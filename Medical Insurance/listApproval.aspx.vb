Imports System.Data.SqlClient
Public Class listApproval
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            getPendingRequests(0)
            getApprovedPendingRequests()
        End If
    End Sub

    Sub getPendingRequests(ByVal com_no As Integer)
        Try

            Dim sql_str As String = "SELECT CONFIRM_ID, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS P_NAME, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS BAGE_NO, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = ((SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID))) AS C_NAME, (SELECT SUM(SERVICE_PRICE) FROM INC_CONFIRM_DETAILS WHERE INC_CONFIRM_DETAILS.CONFIRM_ID = INC_CONFIRM.CONFIRM_ID GROUP BY CONFIRM_ID) AS TOTAL_VALUE, CONFRIM_END_DATE, PENDING_VALUE, (CASE WHEN (REQUEST_UNIT = 1) THEN 'قسم التأمين الصحي' ELSE 'الإيواء والعمليات' END) AS REQUEST_TYPE FROM INC_CONFIRM  WHERE REQUEST_STS = 0"

            If com_no <> 0 Then
                sql_str = sql_str & " AND (SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) = " & com_no & " ORDER BY CONFIRM_ID DESC"
            Else
                sql_str = sql_str & " ORDER BY CONFIRM_ID DESC"
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    'خدمات قدمت وفي انتظار الموافقة
    Sub getApprovedPendingRequests()
        Try

            Dim sql_str As String = "SELECT CONFIRM_ID, APPROVED_VALUE, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS P_NAME, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS BAGE_NO, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = ((SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID))) AS C_NAME, (SELECT SUM(SERVICE_PRICE) FROM INC_CONFIRM_DETAILS WHERE INC_CONFIRM_DETAILS.CONFIRM_ID = INC_CONFIRM.CONFIRM_ID GROUP BY CONFIRM_ID) AS TOTAL_VALUE, CONFRIM_END_DATE, PENDING_VALUE, (CASE WHEN (REQUEST_UNIT = 1) THEN 'قسم التأمين الصحي' ELSE 'الإيواء والعمليات' END) AS REQUEST_TYPE FROM INC_CONFIRM WHERE REQUEST_STS = 1 AND PENDING_VALUE <> 0 ORDER BY CONFIRM_ID DESC"

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

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        getPendingRequests(ddl_companies.SelectedValue)

    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "CONFIRM_DETAILS") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Response.Redirect("approvalDetails.aspx?confID=" & Val(row.Cells(0).Text), False)
        End If

        If (e.CommandName = "approval_req") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim totat_val As TextBox = row.FindControl("txt_confirm_price")

            If totat_val.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('خطأ! يجب إدخال قيمة الموافقة');", True)
                Exit Sub
            End If

            If CDec(totat_val.Text) < CDec(row.Cells(5).Text) Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "Confirm();", True)
            End If

            If CDec(totat_val.Text) > CDec(row.Cells(5).Text) Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('خطأ! قيمة الموافقة أكبر من قيمة العملية');", True)
                Exit Sub
            End If

            Dim update_sts As New SqlCommand("UPDATE INC_CONFIRM SET REQUEST_STS = 1, PENDING_VALUE = " & (CDec(row.Cells(5).Text) - CDec(totat_val.Text)) & ", APPROVED_VALUE = " & totat_val.Text & " WHERE REQUEST_STS = 0 AND EWA_NO = 0 AND CONFIRM_ID = " & (row.Cells(0).Text), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getPendingRequests(0)
            getApprovedPendingRequests()

        End If

        If (e.CommandName = "reject_req") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim update_sts As New SqlCommand("UPDATE INC_CONFIRM SET REQUEST_STS = 2 WHERE REQUEST_STS = 0 AND EWA_NO = 0 AND CONFIRM_ID = " & (row.Cells(0).Text), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getPendingRequests(0)
            getApprovedPendingRequests()

        End If
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

            getPendingRequests(0)
            getApprovedPendingRequests()

        End If

        If (e.CommandName = "reject_req") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim update_sts As New SqlCommand("UPDATE INC_CONFIRM SET REQUEST_STS = 2 WHERE REQUEST_STS = 1 AND PENDING_VALUE <> 0 AND EWA_NO <> 0 AND CONFIRM_ID = " & (row.Cells(0).Text), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getPendingRequests(0)
            getApprovedPendingRequests()

        End If
    End Sub
End Class