Imports System.Data.SqlClient

Public Class EwaReturn
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
            '    ' If Session("User_per")("missing") = False Then
            '    Response.Redirect("Default.aspx", True)
            '    Exit Sub
            '    ' End If
            'End If
            GetDate(0)
        End If
    End Sub

    Private Sub GetDate(ByVal invNo As Integer)
        Try
            Dim query As String = "select EWA_Record_ID as inv_id, (select NAME_ARB from INC_PATIANT where INC_Patient_Code = EWA_Record_Code) as p_name ,convert(varchar, Ewa_Record.EWA_Racode_Date, 103) as EWA_Racode_Date
from  Ewa_Record 
where  EWA_Record_ID in (
Select Ewa_Exit_ID from Ewa_Exit  where  Ewa_Exit.Ewa_Exit_ID = EWA_Record_ID
) and substring (EWA_Record_Code , 8,1)=1
and (select count(ewa_process_id)  from EWA_Processes where ewa_patient_id = EWA_Record_ID and ewa_process_id not in (select Processes_Id from INC_MOTALBAT))>0"

            If invNo <> 0 Then
                query += " and EWA_Record_ID = " & invNo
            End If

            query += " order by EWA_Record_ID"

            Dim selComm As New SqlCommand(query, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            Dim dtResult As New DataTable
            dtResult.Load(selComm.ExecuteReader)
            insurance_SQLcon.Close()

            If dtResult.Rows.Count > 0 Then
                GridView1.DataSource = dtResult
                GridView1.DataBind()
            Else
                dtResult.Rows.Clear()
                GridView1.DataSource = dtResult
                GridView1.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
        End Try
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        GetDate(Val(TextBox1.Text))
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "return") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim delComm As New SqlCommand("delete  Ewa_Exit  where  Ewa_Exit.Ewa_Exit_ID = " & row.Cells(0).Text, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            delComm.ExecuteNonQuery()
            insurance_SQLcon.Close()

            GetDate(0)

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية إرجاع الفاتورة بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
        End If
    End Sub
End Class