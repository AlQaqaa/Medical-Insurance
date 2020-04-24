Imports System.Data.SqlClient
Public Class listApproval
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            getPendingRequests(0)

        End If
    End Sub

    Sub getPendingRequests(ByVal com_no As Integer)
        Try

            Dim sql_str As String = "SELECT IEC_ID, IEC_PID, IEC_SID AS SubService_ID, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS P_NAME_ARB, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS BAGE_NO, IEC_Price AS SubService_Price, (CASE WHEN (IEC_Type = 1) THEN 'خدمة/عملية' ELSE 'إضافة على عملية' END) AS IEC_Type_char, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE '/' END) AS SubService_Code, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE (SELECT Ewa_Add_name FROM Ewa_Additionals WHERE Ewa_Additionals.Ewa_Add_id = INC_EWA_Confirm.IEC_SID) END) AS SubService_AR_Name, (CASE WHEN (IEC_Req = 0) THEN 'التأمين الصحي' ELSE 'الإيواء' END) AS IEC_Req, IEC_Date, IEC_Type, IEC_RNo FROM INC_EWA_Confirm WHERE IEC_State = 0 AND IEC_RNo = 0"

            If com_no <> 0 Then
                sql_str = sql_str & " AND (SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) = " & com_no & " ORDER BY IEC_ID DESC"
            Else
                sql_str = sql_str & " ORDER BY IEC_ID DESC"
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

    'Sub getApprovedRequests(ByVal com_no As Integer)
    '    Try

    '        Dim sql_str As String = "SELECT IEC_ID, IEC_PID, IEC_SID AS SubService_ID, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS P_NAME_ARB, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS BAGE_NO, IEC_Price AS SubService_Price, (CASE WHEN (IEC_Type = 1) THEN 'خدمة/عملية' ELSE 'إضافة على عملية' END) AS IEC_Type_char, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE '/' END) AS SubService_Code, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE (SELECT Ewa_Add_name FROM Ewa_Additionals WHERE Ewa_Additionals.Ewa_Add_id = INC_EWA_Confirm.IEC_SID) END) AS SubService_AR_Name, (CASE WHEN (IEC_Req = 0) THEN 'التأمين الصحي' ELSE 'الإيواء' END) AS IEC_Req, IEC_Date, IEC_Type, IEC_RNo FROM INC_EWA_Confirm WHERE IEC_State = 1 AND IEC_RNo <> 0"

    '        If com_no <> 0 Then
    '            sql_str = sql_str & " AND (SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) = " & com_no & " ORDER BY IEC_ID DESC"
    '        Else
    '            sql_str = sql_str & " ORDER BY IEC_ID DESC"
    '        End If

    '        Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
    '        Dim dt_result As New DataTable
    '        dt_result.Rows.Clear()
    '        insurance_SQLcon.Close()
    '        insurance_SQLcon.Open()
    '        dt_result.Load(sel_com.ExecuteReader)
    '        insurance_SQLcon.Close()

    '        If dt_result.Rows.Count > 0 Then
    '            GridView2.DataSource = dt_result
    '            GridView2.DataBind()
    '        Else
    '            dt_result.Rows.Clear()
    '            GridView2.DataSource = dt_result
    '            GridView2.DataBind()
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    'Sub getRejectRequests(ByVal com_no As Integer)
    '    Try

    '        Dim sql_str As String = "SELECT IEC_ID, IEC_PID, IEC_SID AS SubService_ID, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS P_NAME_ARB, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) AS BAGE_NO, IEC_Price AS SubService_Price, (CASE WHEN (IEC_Type = 1) THEN 'خدمة/عملية' ELSE 'إضافة على عملية' END) AS IEC_Type_char, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE '/' END) AS SubService_Code, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE (SELECT Ewa_Add_name FROM Ewa_Additionals WHERE Ewa_Additionals.Ewa_Add_id = INC_EWA_Confirm.IEC_SID) END) AS SubService_AR_Name, (CASE WHEN (IEC_Req = 0) THEN 'التأمين الصحي' ELSE 'الإيواء' END) AS IEC_Req, IEC_Date, IEC_Type, IEC_RNo FROM INC_EWA_Confirm WHERE IEC_State = 2 AND IEC_RNo = 0"

    '        If com_no <> 0 Then
    '            sql_str = sql_str & " AND (SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_EWA_Confirm.IEC_PID) = " & com_no & " ORDER BY IEC_ID DESC"
    '        Else
    '            sql_str = sql_str & " ORDER BY IEC_ID DESC"
    '        End If

    '        Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
    '        Dim dt_result As New DataTable
    '        dt_result.Rows.Clear()
    '        insurance_SQLcon.Close()
    '        insurance_SQLcon.Open()
    '        dt_result.Load(sel_com.ExecuteReader)
    '        insurance_SQLcon.Close()

    '        If dt_result.Rows.Count > 0 Then
    '            GridView3.DataSource = dt_result
    '            GridView3.DataBind()
    '        Else
    '            dt_result.Rows.Clear()
    '            GridView3.DataSource = dt_result
    '            GridView3.DataBind()
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        getPendingRequests(ddl_companies.SelectedValue)

    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "approval_req") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim update_sts As New SqlCommand("UPDATE INC_EWA_Confirm SET IEC_State = 1 WHERE IEC_State = 0 AND IEC_RNo <> 0 AND IEC_ID = " & (row.Cells(0).Text), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getPendingRequests(0)

        End If

        If (e.CommandName = "reject_req") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim update_sts As New SqlCommand("UPDATE INC_EWA_Confirm SET IEC_State = 2 WHERE IEC_State = 0 AND IEC_RNo = 0 AND IEC_ID = " & (row.Cells(0).Text), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getPendingRequests(0)

        End If
    End Sub
End Class