Imports System.Data.SqlClient

Public Class missingForms
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("doctors_settled") = False Then
                    Response.Redirect("Default.aspx", True)
                    Exit Sub
                End If
            End If

            getData()

        End If
    End Sub

    Private Sub getData()
        Dim sel_com As New SqlCommand("select Req_Code ,Processes_Date  from HAG_Processes 
inner join HAG_Request on HAG_Processes.Processes_ID =Req_PID 
inner join INC_PATIANT on  INC_PATIANT.INC_Patient_Code =HAG_Processes.Processes_Reservation_Code 
where Processes_State < 3 
and Processes_ID not in (select  INC_MOTALBAT.Processes_ID  from INC_MOTALBAT )
order by  Processes_Date", insurance_SQLcon)
        Dim dt_result As New DataTable
        If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView1.DataSource = dt_result
            GridView1.DataBind()
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "printProcess") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)


        End If
    End Sub

End Class