Imports System.Data.SqlClient
Public Class SERVICES_PRICES
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        Dim xx As Integer = ddl_clinics.SelectedValue

        Dim sel_com As New SqlCommand("select * FROM  MAIN_SUBSERVIES WHERE CLINIC_ID = " & xx, insurance_SQLcon)

        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()
       
        If dt_result.Rows.Count > 0 Then
            Dim strbody As New StringBuilder()


            '      For i = 0 To dt_result.Rows.Count - 1
            For Each dd As GridViewRow In GridView1.Rows
                Dim Tx As TextBox = dd.FindControl("TextBox3")
                Dim Tx1 As TextBox = dd.FindControl("TextBox2")
                Dim Tx2 As TextBox = dd.FindControl("TextBox4")
                Dim CASH_PRS As Decimal = CDec(Tx.Text)
                Dim INS_PRS As Decimal = CDec(Tx1.Text)
                Dim INVO_PRS As Decimal = CDec(Tx2.Text)

                Try

                    Dim ins_PAT As New SqlCommand
                    ins_PAT.Connection = insurance_SQLcon
                    ins_PAT.CommandText = "INC_addPRICE"
                    ins_PAT.CommandType = CommandType.StoredProcedure
                  
                    ins_PAT.Parameters.AddWithValue("@SER_ID", (dd.Cells(0).Text))
                    ins_PAT.Parameters.AddWithValue("@CASH_PRS", CASH_PRS)
                    ins_PAT.Parameters.AddWithValue("@INS_PRS", INS_PRS)
                    ins_PAT.Parameters.AddWithValue("@INVO_PRS", INVO_PRS)
                    ins_PAT.Parameters.AddWithValue("@USER_ID", 1)
                    ins_PAT.Parameters.AddWithValue("@USER_IP", 1)
                    ins_PAT.Parameters.AddWithValue("@LAST_EDIT", txt_start_dt)
                    insurance_SQLcon.Close()
                    insurance_SQLcon.Open()
                    ins_PAT.ExecuteNonQuery()
                    insurance_SQLcon.Close()

                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            Next
        End If

    End Sub




    '        For Each dd As GridViewRow In GridView1.Rows
    'Dim Tx As TextBox = dd.FindControl("txt_cashier_value")
    '            If Tx.Text <> "" Then
    '                total_gross_value = total_gross_value + CDec(Tx.Text)
    '            Else
    '                ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alertify.error('خطأ! يجب إدخال قيمة الصرّاف'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
    '            End If
    '        Next





    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        End Sub

   
 
End Class