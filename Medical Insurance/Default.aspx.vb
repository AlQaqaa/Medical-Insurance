Imports System.Data.SqlClient
Imports System.Globalization

Public Class _Default1
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            'If Session("User_Id") Is Nothing Or Session("User_Id") = 0 Then
            '    Response.Redirect("http://10.10.1.10", True)
            'End If

            'If Session("systemlogin") <> "401" Then
            '    Response.Redirect("http://10.10.1.10", True)
            'End If

            getCompanyData()
            Session.Remove("profile_no")

            txt_pat_search.Focus()
        End If

    End Sub

    Private Sub getCompanyData()
        Dim sel_com As New SqlCommand("select *,(case when (select c_name_arb from [INC_COMPANY_DATA] as x where x.c_id=[INC_COMPANY_DATA].c_level ) is null then  '-' else (select c_name_arb from [INC_COMPANY_DATA] as x where x.c_id=[INC_COMPANY_DATA].c_level) end)as MAIN_COMPANY, (select top (1) contract_no from INC_COMPANY_DETIAL where INC_COMPANY_DETIAL.c_id = INC_COMPANY_DATA.c_id order by n desc) as contract_no from INC_COMPANY_DATA WHERE C_STATE = 0", insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            dt_GridView.DataSource = dt_result
            dt_GridView.DataBind()
            'Dim strbody As New StringBuilder()

            'strbody.Append("<tbody>")

            'For i = 0 To dt_result.Rows.Count - 1
            '    Dim dr_result = dt_result.Rows(i)
            '    strbody.Append("<tr>")
            '    strbody.Append("<td style='width:20px;text-align:center !important'>" & dr_result!C_id & "</td>")
            '    strbody.Append("<td>" & dr_result!C_NAME_ARB & "</td>")
            '    strbody.Append("<td>" & dr_result!C_NAME_ENG & "</td>")
            '    strbody.Append("<td>" & dr_result!xx & "</td>")
            '    strbody.Append("<td style='width:220px;text-align:center !important'><a href='EDITCOMPANY.aspx?comID=" & dr_result!C_id & "' data-toggle='tooltip' data-placement='top' title='تعديل'><i class='fas fa-edit'></i></a> | <a href='INC_PATIANT.aspx?comID=" & dr_result!C_id & "' data-toggle='tooltip' data-placement='top' title='إضافة مشتركين'><i class='fas fa-user-plus'></i></a> | <a href='employees.aspx?EmpID=" & dr_result!C_id & "' data-toggle='tooltip' data-placement='top' title='تمديد/تجديد العقد'><i class='fas fa-file-signature'></i></a> | <a href='employees.aspx?EmpID=" & dr_result!C_id & "' data-toggle='tooltip' data-placement='top' title='العيادات والمنافع'><i class='fas fa-stethoscope'></i></a></td>")
            '    strbody.Append("</tr>")
            'Next
            'strbody.Append("</tbody>")

            'LtlTableBody.Text = strbody.ToString()
        End If
    End Sub

    Private Sub dt_GridView_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dt_GridView.RowCommand

        '################ When User Press On Company Name ################
        If (e.CommandName = "com_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("Companies/Default.aspx", True)
        End If

        '################ When User Press On Edit Button ################
        If (e.CommandName = "edit_com") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("Companies/EDITCOMPANY.aspx", True)
        End If


        '################ When User Press On Add Patiant Button ################
        If (e.CommandName = "add_pat") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("Companies/INC_PATIANT.aspx", True)
        End If

        '################ When User Press On Add New Contract ################
        If (e.CommandName = "new_contract") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("Companies/addNewContract.aspx", True)
        End If

        '################ When User Press On Stop Button ################
        If (e.CommandName = "stop_com") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.
            Dim row As GridViewRow = dt_GridView.Rows(index)

            Try
                Dim stopCompany As New SqlCommand
                stopCompany.Connection = insurance_SQLcon
                stopCompany.CommandText = "INC_stopCompany"
                stopCompany.CommandType = CommandType.StoredProcedure
                stopCompany.Parameters.AddWithValue("@comID", (row.Cells(0).Text))
                stopCompany.Parameters.AddWithValue("@user_id", Session("User_Id"))
                stopCompany.Parameters.AddWithValue("@user_ip", GetIPAddress())
                insurance_SQLcon.Open()
                stopCompany.ExecuteNonQuery()
                insurance_SQLcon.Close()
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.set('notifier','position', 'top-right'); alertify.success('تم إيقاف الشركة بنجاح');", True)
                getCompanyData()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If

        '################ When User Press On End Company Contract ################
        If (e.CommandName = "end_contract") Then

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Dim end_contract As New SqlCommand("UPDATE INC_COMPANY_DETIAL SET DATE_END=@DATE_END AND STOP_DATE=@DATE_END WHERE C_ID = " & (row.Cells(0).Text) & " AND CONTRACT_NO = " & (row.Cells(1).Text), insurance_SQLcon)
            end_contract.Parameters.AddWithValue("@DATE_END", Date.Now.Date)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            end_contract.ExecuteNonQuery()
            insurance_SQLcon.Close()

            add_action(1, 2, 2, "إنهاء عقد الشركة رقم: " & (row.Cells(0).Text), Session("User_Id"), GetIPAddress())
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تم إنهاء عقد الشركة بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End If

    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click

        Try

            Dim search_flag As Char = txt_pat_search.Text.Substring(0, 1)

            Dim query_str As String = "SELECT PINC_ID, CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, C_ID, PHONE_NO, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID) AS C_NAME, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, NAT_NUMBER, (CASE WHEN (P_STATE) = 0 THEN 'مفعل' ELSE 'موقوف' END) AS P_STATE, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID FROM INC_PATIANT WHERE 1=1"

            Select Case search_flag
                Case "1"
                    query_str = query_str & " AND CARD_NO = '" & txt_pat_search.Text.Remove(0, 1) & "'"
                Case "2"
                    query_str = query_str & " AND BAGE_NO = '" & txt_pat_search.Text.Remove(0, 1) & "'"
                Case Else
                    query_str = query_str & " AND NAME_ARB LIKE '%" & txt_pat_search.Text & "%'"
            End Select

            Dim sel_com As New SqlCommand(query_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Panel1.Visible = True
                btn_clear.Visible = True
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            Else
                Panel1.Visible = False
                btn_clear.Visible = False
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Patiant Name ################
        If (e.CommandName = "pat_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Session.Item("patiant_id") = (row.Cells(0).Text) ' 13471
            Session.Item("company_id") = (row.Cells(1).Text) '56 

            Response.Redirect("Companies/patientInfo.aspx")
        End If
    End Sub

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click
        txt_pat_search.Text = ""
        txt_pat_search.Focus()
        GridView1.DataBind()
        btn_clear.Visible = False
    End Sub
End Class