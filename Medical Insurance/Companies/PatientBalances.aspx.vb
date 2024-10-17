Imports System.Data.SqlClient

Public Class PatientBalances
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim sqlComm As New SqlCommand()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
            If Session("User_per")("balnce") = False Then
                Response.Redirect("../Default.aspx", True)
            End If
        End If

        If Session("company_id") = Nothing Then
            Response.Redirect("Default.aspx")
        End If

        ViewState("company_no") = Val(Session("company_id"))

        If ViewState("company_no") = 0 Then
            Response.Redirect("Default.aspx")
        End If

        If Not Me.IsPostBack Then
            GetData()
        End If
    End Sub

    Private Sub GetData()
        Dim dtResult As New DataTable

        sqlComm.Connection = insurance_SQLcon
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.CommandText = "INC_PatientBalances"
        sqlComm.Parameters.Clear()
        sqlComm.Parameters.AddWithValue("cId", Session("company_id"))

        Try
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            dtResult.Load(sqlComm.ExecuteReader)
            insurance_SQLcon.Close()

            If dtResult.Rows.Count > 0 Then
                GridView1.DataSource = dtResult
                GridView1.DataBind()
                For i = 0 To dtResult.Rows.Count - 1
                    Dim dd As GridViewRow = GridView1.Rows(i)
                    Dim txtBalance As TextBox = dd.FindControl("txtBalance")
                    txtBalance.Text = dtResult.Rows(i)("Balance")
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand

        If (e.CommandName = "save") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim txt_Balance As TextBox = row.FindControl("txtBalance")

            sqlComm.Connection = insurance_SQLcon
            sqlComm.CommandText = "INC_InsertPatientBalances"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()
            sqlComm.Parameters.AddWithValue("@ewaId", Val(row.Cells(0).Text))
            sqlComm.Parameters.AddWithValue("@paCode", row.Cells(1).Text)
            sqlComm.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
            sqlComm.Parameters.AddWithValue("@balance", txt_Balance.Text)
            sqlComm.Parameters.AddWithValue("@cId", Session("company_id"))
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            sqlComm.ExecuteNonQuery()
            insurance_SQLcon.Close()
            sqlComm.CommandText = ""

            GetData()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت العملية بنجاح',
                showConfirmButton: false,
                timer: 1500
            });
            window.setTimeout(function () {
               location.reload();
               return false;
            }, 1500);", True)

        End If

    End Sub

End Class