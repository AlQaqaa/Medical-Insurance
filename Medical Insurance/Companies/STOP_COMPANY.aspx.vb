Imports System.Data.SqlClient
Public Class STOP_COMPANY
    Inherits System.Web.UI.Page
    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            getCompanyData()

        End If
    End Sub

    Private Sub getCompanyData()
        Dim sel_com As New SqlCommand("select *,(case when (select c_name_arb from [INC_COMPANY_DATA] as x where x.c_id=[INC_COMPANY_DATA].c_level ) is null then  '-' else (select c_name_arb from [INC_COMPANY_DATA] as x where x.c_id=[INC_COMPANY_DATA].c_level) end)as MAIN_COMPANY from INC_COMPANY_DATA WHERE C_STATE = 1", insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            dt_GridView.DataSource = dt_result
            dt_GridView.DataBind()
            
        End If
    End Sub

    Private Sub dt_GridView_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dt_GridView.RowCommand

        '################ When User Press On Company Name ################
        If (e.CommandName = "com_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("companyInfo.aspx")
        End If

        '################ When User Press On Edit Button ################
        If (e.CommandName = "edit_com") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dt_GridView.Rows(index)
            Session.Item("company_id") = (row.Cells(0).Text)
            Response.Redirect("EDITCOMPANY.aspx")
        End If

        '################ When User Press On Avtive Button ################
        If (e.CommandName = "active_com") Then
            ' Retrieve the row index stored in the CommandArgument property.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button 
            ' from the Rows collection.
            Dim row As GridViewRow = dt_GridView.Rows(index)

            Try
                Dim c_level As Integer = 0
                Dim sel_com As New SqlCommand("SELECT C_LEVEL FROM INC_COMPANY_DATA WHERE C_ID = " & (row.Cells(0).Text), insurance_SQLcon)
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                c_level = sel_com.ExecuteScalar
                insurance_SQLcon.Close()

                If c_level <> 0 Then
                    Dim check_main_com As New SqlCommand("SELECT C_STATE FROM INC_COMPANY_DATA WHERE C_ID = " & c_level, insurance_SQLcon)
                    insurance_SQLcon.Close()
                    insurance_SQLcon.Open()
                    If sel_com.ExecuteScalar = 1 Then
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('خطأ! لا يمكن تفعيل هذه الشركة، الشركة الأم موقوفة حالياً');", True)
                        Exit Sub
                    End If
                    insurance_SQLcon.Close()
                End If

                Dim stopCompany As New SqlCommand
                stopCompany.Connection = insurance_SQLcon
                stopCompany.CommandText = "INC_startCompany"
                stopCompany.CommandType = CommandType.StoredProcedure
                stopCompany.Parameters.AddWithValue("@comID", (row.Cells(0).Text))
                stopCompany.Parameters.AddWithValue("@user_id", Session("User_Id"))
                stopCompany.Parameters.AddWithValue("@user_ip", GetIPAddress())
                insurance_SQLcon.Open()
                stopCompany.ExecuteNonQuery()
                insurance_SQLcon.Close()
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تم تفعيل الشركة بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
                getCompanyData()
                dt_GridView.DataBind()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End If

    End Sub

End Class