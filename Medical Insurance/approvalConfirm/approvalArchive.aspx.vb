Imports System.Data.SqlClient

Public Class approvalArchive
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Session("User_Id") Is Nothing Or Session("User_Id") = 0 And Session("systemlogin") <> "401" Then
                Response.Redirect("http://10.10.1.10", True)
            End If

            getPendingRequests(0)
        End If
    End Sub

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        getPendingRequests(ddl_companies.SelectedValue)

    End Sub

    Sub getPendingRequests(ByVal com_no As Integer)
        Try

            Dim sql_str As String = "SELECT TOP(1500) CONFIRM_ID, APPROVED_VALUE, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS P_NAME, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS BAGE_NO, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = ((SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID))) AS C_NAME, (SELECT SUM(SERVICE_PRICE) FROM INC_CONFIRM_DETAILS WHERE INC_CONFIRM_DETAILS.CONFIRM_ID = INC_CONFIRM.CONFIRM_ID GROUP BY CONFIRM_ID) AS TOTAL_VALUE, CONFRIM_END_DATE, PENDING_VALUE, (CASE WHEN (REQUEST_UNIT = 1) THEN 'قسم التأمين الصحي' ELSE 'الإيواء والعمليات' END) AS REQUEST_TYPE, (CASE WHEN REQUEST_STS = 0 THEN 'معلقة' WHEN REQUEST_STS = 1 THEN 'تمت الموافقة' ELSE 'مرفوضة' END) AS REQUEST_STS FROM INC_CONFIRM WHERE 1=1"

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

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "CONFIRM_DETAILS") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Response.Redirect("approvalDetails.aspx?confID=" & Val(row.Cells(0).Text), False)
        End If
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cell As TableCell = e.Row.Cells(1)
            Dim pet_state As String = cell.Text

            If pet_state = "تمت الموافقة" Then
                cell.CssClass = "bg-success text-white"
            ElseIf pet_state = "معلقة" Then
                cell.CssClass = "bg-secondary text-white"
            Else
                cell.CssClass = "bg-danger text-white"
            End If
        End If
    End Sub

End Class