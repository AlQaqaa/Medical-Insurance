Imports System.Data.SqlClient
Public Class main
    Inherits System.Web.UI.MasterPage

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            lbl_date_now.Text = Date.Now.ToLongDateString

        End If
        'calculateCompanyExpenses()

        'lbl_notification.Text = "<span class='badge badge-info'>" & getNotificationCount() & "</span>"

    End Sub

    'Sub calculateCompanyExpenses()
    '    Try
    '        Dim get_company As New SqlCommand("SELECT C_ID, C_Name_Arb, (SELECT TOP(1) MAX_VAL FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS MAX_VAL, (SELECT TOP(1) CONTRACT_NO FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS CONTRACT_NO FROM INC_COMPANY_DATA WHERE C_State = 0", insurance_SQLcon)
    '        Dim dt_company As New DataTable
    '        dt_company.Rows.Clear()
    '        insurance_SQLcon.Close()
    '        insurance_SQLcon.Open()
    '        dt_company.Load(get_company.ExecuteReader)
    '        insurance_SQLcon.Close()

    '        If dt_company.Rows.Count > 0 Then
    '            For i = 0 To dt_company.Rows.Count - 1
    '                Dim dr_com = dt_company.Rows(i)
    '                Dim total_expenses As Decimal = 0
    '                Dim get_total_expens As New SqlCommand("SELECT TOTAL_EXPENSES FROM INC_totalCompanyExpenses(" & dr_com!C_ID & ",(select DATE_START from INC_COMPANY_DETIAL where C_ID = " & dr_com!C_ID & " and CONTRACT_NO = " & dr_com!CONTRACT_NO & "),(SELECT DATE_END from INC_COMPANY_DETIAL where C_ID = " & dr_com!C_ID & " and CONTRACT_NO = " & dr_com!CONTRACT_NO & "))", insurance_SQLcon)
    '                insurance_SQLcon.Close()
    '                insurance_SQLcon.Open()
    '                total_expenses = get_total_expens.ExecuteScalar
    '                insurance_SQLcon.Close()

    '                If dr_com!MAX_VAL <> 0 And total_expenses <> 0 Then
    '                    If CInt((total_expenses / dr_com!MAX_VAL) * 100) >= 90 Then
    '                        Dim expens_percent As Decimal = CInt((total_expenses / dr_com!MAX_VAL) * 100)
    '                        Dim noti_content As String = "شركة " & dr_com!C_Name_Arb & " أنفقت " & expens_percent & "% من إجمالي السقف المخصص لها"
    '                        Dim ins_noti As New SqlCommand("INSERT INTO INC_NOTIFICATIONS (C_ID, NOTI_CONTENT, NOTI_STS, USER_SHOW_ID) VALUES (@C_ID, @NOTI_CONTENT, 1, 0)", insurance_SQLcon)
    '                        ins_noti.Parameters.AddWithValue("@C_ID", dr_com!C_ID)
    '                        ins_noti.Parameters.AddWithValue("@NOTI_CONTENT", noti_content.ToString)
    '                        insurance_SQLcon.Close()
    '                        insurance_SQLcon.Open()
    '                        ins_noti.ExecuteNonQuery()
    '                        insurance_SQLcon.Close()
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    'Function getNotificationCount() As Integer
    '    Dim noti_count As Integer = 0
    '    Dim sel_com As New SqlCommand("SELECT COUNT(*) FROM INC_NOTIFICATIONS WHERE NOTI_STS = 1", insurance_SQLcon)
    '    insurance_SQLcon.Close()
    '    insurance_SQLcon.Open()
    '    noti_count = sel_com.ExecuteScalar
    '    insurance_SQLcon.Close()

    '    Return noti_count
    'End Function

End Class