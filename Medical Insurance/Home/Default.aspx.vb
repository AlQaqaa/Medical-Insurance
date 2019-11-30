Imports System.Data.SqlClient
Public Class _Default1
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            getCompanyData()

        End If
    End Sub

    Private Sub getCompanyData()
        Dim sel_com As New SqlCommand("select *,(case when (select c_name_arb from [IBNSINAMAIN].[dbo].[INC_COMPANY_DATA] as x where x.c_id=[INC_COMPANY_DATA].c_level ) is null then  '-' else (select c_name_arb from [IBNSINAMAIN].[dbo].[INC_COMPANY_DATA] as x where x.c_id=[INC_COMPANY_DATA].c_level) end)as MAIN_COMPANY from INC_COMPANY_DATA WHERE C_STATE = 0", insurance_SQLcon)
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
            Response.Redirect("../Companies/Default.aspx")
        End If

        '################ When User Press On Edit Button ################
        If (e.CommandName = "edit_com") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("../Companies/EDITCOMPANY.aspx")
        End If


        '################ When User Press On Add Patiant Button ################
        If (e.CommandName = "add_pat") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("../Companies/INC_PATIANT.aspx")
        End If

        '################ When User Press On Add New Contract ################
        If (e.CommandName = "new_contract") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("../Companies/addNewContract.aspx")
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
                insurance_SQLcon.Open()
                stopCompany.ExecuteNonQuery()
                insurance_SQLcon.Close()
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.set('notifier','position', 'top-right'); alertify.success('تم إيقاف الشركة بنجاح');", True)
                getCompanyData()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End If
    End Sub

End Class