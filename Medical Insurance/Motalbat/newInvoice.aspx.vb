﻿Imports System.Data.SqlClient
Imports System.Globalization

Public Class newInvoice
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT Processes_ID, Processes_Reservation_Code, SUBSTRING([Processes_Reservation_Code],9 , 6) AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id), '') AS MedicalStaff_AR_Name, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,9 , 6)), '') AS PATIENT_NAME FROM INC_CompanyProcesses WHERE SUBSTRING([Processes_Reservation_Code],7 , 2) = " & ddl_companies.SelectedValue & " AND Processes_State in (0, 2) AND Processes_Residual <> 0 AND CONVERT(VARCHAR, Processes_Date, 103) >= '" & txt_start_dt.Text & "' AND CONVERT(VARCHAR, Processes_Date, 103) <= '" & txt_end_dt.Text & "' AND Processes_ID NOT IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID)"

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_ID NOT IN (SELECT ewa_process_id FROM EWA_Processes WHERE EWA_Processes.ewa_process_id = INC_CompanyProcesses.Processes_ID)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Ewa_Exit_ID FROM Ewa_Exit WHERE Ewa_Exit.Ewa_Exit_ID = INC_CompanyProcesses.Processes_ID)"
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
                CheckBox1.Visible = True
                btn_create.Enabled = True
            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
                CheckBox1.Visible = False
                btn_create.Enabled = False
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

            Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
            Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

            Dim result As Integer = 0

            Dim invoice_id As String
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = insurance_SQLcon
            sqlComm.CommandText = "INC_addNewInvoice"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.AddWithValue("@c_id", ddl_companies.SelectedValue)
            sqlComm.Parameters.AddWithValue("@from_dt", start_dt)
            sqlComm.Parameters.AddWithValue("@to_dt", end_dt)
            sqlComm.Parameters.AddWithValue("@invoiceType", ddl_invoice_type.SelectedValue)
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
                        Dim ins_motalba As New SqlCommand("INSERT INTO INC_MOTALBAT (INVOICE_NO,Processes_ID,MOTALABA_STS,USER_ID,USER_IP) VALUES (@INVOICE_NO,@Processes_ID,@MOTALABA_STS,@USER_ID,@USER_IP)", insurance_SQLcon)
                        ins_motalba.Parameters.AddWithValue("@INVOICE_NO", invoice_id)
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

                Response.Redirect("invoiceContent.aspx?invID=" & invoice_id, False)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")

            If CheckBox1.Checked = True Then
                ch.Checked = True
            Else
                ch.Checked = False

            End If
        Next
    End Sub

End Class