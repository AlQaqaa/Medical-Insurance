Imports System.Data.SqlClient

Public Class Discount
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim sqlComm As New SqlCommand()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
            If Session("User_per")("discount") = False Then
                Response.Redirect("Default.aspx", True)
                Exit Sub
            End If
        End If

        If Not Me.IsPostBack Then
            Dim dtResult As New DataTable

            sqlComm.Connection = insurance_SQLcon
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "INC_Discount"
            sqlComm.Parameters.Clear()

            Try
                If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
                dtResult.Load(sqlComm.ExecuteReader)
                insurance_SQLcon.Close()

                If dtResult.Rows.Count > 0 Then
                    GridView1.DataSource = dtResult
                    GridView1.DataBind()
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Protected Sub imgbtndetails_Click(sender As Object, e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        hfewaId.Value = row.Cells(0).Text
        hfpCodeId.Value = row.Cells(1).Text
        mpePopUp.Show()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            sqlComm.Connection = insurance_SQLcon
            sqlComm.CommandText = "INC_InsertDiscount"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()
            sqlComm.Parameters.AddWithValue("@ewaId", hfewaId.Value)
            sqlComm.Parameters.AddWithValue("@pCode", hfpCodeId.Value)
            sqlComm.Parameters.AddWithValue("@Amount", txtAmount.Text)
            sqlComm.Parameters.AddWithValue("@Desc", txtDesc.Text)
            sqlComm.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            sqlComm.ExecuteNonQuery()
            insurance_SQLcon.Close()
            sqlComm.CommandText = ""

            txtAmount.Text = ""
            txtDesc.Text = ""

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
        Catch ex As Exception

        End Try
    End Sub
End Class