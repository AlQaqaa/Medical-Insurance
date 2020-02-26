Imports System.Data.SqlClient

Public Class newInvoice
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT Processes_ID, Processes_Reservation_Code, SUBSTRING([Processes_Reservation_Code],9 , 6) AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, MedicalStaff_AR_Name, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,9 , 6)), '') AS PATIENT_NAME FROM INC_CompanyProcesses WHERE SUBSTRING([Processes_Reservation_Code],7 , 2) = " & ddl_companies.SelectedValue & " AND Processes_State = 2 AND CONVERT(VARCHAR, Processes_Date, 103) between '" & txt_start_dt.Text & "' AND '" & txt_end_dt.Text & "'"

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
        Try
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
            sqlComm.ExecuteNonQuery()
            invoice_id = sqlComm.Parameters("@inv_id").Value.ToString()
            sqlComm.CommandText = ""
            insurance_SQLcon.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class