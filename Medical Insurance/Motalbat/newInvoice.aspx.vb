Imports System.Data.SqlClient

Public Class newInvoice
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT Processes_ID, Processes_Reservation_Code, SUBSTRING([Processes_Reservation_Code],9 , 6) AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id), '') AS MedicalStaff_AR_Name, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,9 , 6)), '') AS PATIENT_NAME FROM INC_CompanyProcesses WHERE SUBSTRING([Processes_Reservation_Code],7 , 2) = " & ddl_companies.SelectedValue & " AND Processes_State = 2 AND CONVERT(VARCHAR, Processes_Date, 103) >= '" & txt_start_dt.Text & "' AND CONVERT(VARCHAR, Processes_Date, 103) <= '" & txt_end_dt.Text & "'"

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

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()
    End Sub

    Private Sub btn_create_Click(sender As Object, e As EventArgs) Handles btn_create.Click

        Dim ch_counter As Integer = 0

        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")
            If ch.Checked = True Then
                ch_counter = ch_counter + 1
            End If
        Next

        If ch_counter = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('يرجى اختيار حركة واحدة على الأقل'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            Exit Sub
        End If


        Try
            Dim result As Integer = 0

            Dim invoice_id As String
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = insurance_SQLcon
            sqlComm.CommandText = "INC_addNewInvoice"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.AddWithValue("@c_id", ddl_companies.SelectedValue)
            sqlComm.Parameters.AddWithValue("@user_id", 1)
            sqlComm.Parameters.AddWithValue("@user_ip", GetIPAddress())
            sqlComm.Parameters.AddWithValue("@inv_id", SqlDbType.Int).Direction = ParameterDirection.Output
            insurance_SQLcon.Open()
            result = sqlComm.ExecuteNonQuery()
            invoice_id = sqlComm.Parameters("@inv_id").Value.ToString()
            sqlComm.CommandText = ""
            insurance_SQLcon.Close()

            If result > 0 Then
                For Each dd As GridViewRow In GridView1.Rows
                    Dim ch As CheckBox = dd.FindControl("CheckBox2")

                    If ch.Checked = True Then
                        Dim ins_motalba As New SqlCommand("INSERT INTO INC_MOTALBAT (INCOICE_NO,Processes_ID,MOTALABA_STS,USER_ID,USER_IP) VALUES (@INCOICE_NO,@Processes_ID,@MOTALABA_STS,@USER_ID,@USER_IP)", insurance_SQLcon)
                        ins_motalba.Parameters.AddWithValue("@INCOICE_NO", invoice_id)
                        ins_motalba.Parameters.AddWithValue("@Processes_ID", dd.Cells(1).Text)
                        ins_motalba.Parameters.AddWithValue("@MOTALABA_STS", 1)
                        ins_motalba.Parameters.AddWithValue("@USER_ID", 1)
                        ins_motalba.Parameters.AddWithValue("@USER_IP", GetIPAddress())
                        insurance_SQLcon.Close()
                        insurance_SQLcon.Open()
                        ins_motalba.ExecuteNonQuery()
                        insurance_SQLcon.Close()
                    End If
                Next
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class