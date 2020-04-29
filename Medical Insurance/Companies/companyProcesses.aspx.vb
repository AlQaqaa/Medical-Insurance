Imports System.Data.SqlClient

Public Class companyProcesses
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            btn_apply.Enabled = False
            txt_start_dt.Enabled = False
            txt_end_dt.Enabled = False

            If Session("company_id") IsNot Nothing Then
                getData()
            Else
                Response.Redirect("../Default.aspx")

            End If

        End If

    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT Processes_Reservation_Code, SUBSTRING([Processes_Reservation_Code],9 , 6) AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id), '') AS MedicalStaff_AR_Name, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,9 , 6)), '') AS PATIENT_NAME FROM INC_CompanyProcesses WHERE SUBSTRING([Processes_Reservation_Code],7 , 2) = " & Session("company_id") & " AND Processes_State in (0,2)"

            Select Case ddl_time.SelectedValue
                Case 1
                    sql_str = sql_str & " AND [Processes_Date]>= DATEADD(day,-30,GETDATE()) and [Processes_Date] <= getdate()"
                Case 2
                    sql_str = sql_str & " AND [Processes_Date]>= DATEADD(day,-7,GETDATE()) and [Processes_Date] <= getdate()"
                Case 3
                    sql_str = sql_str & " AND MONTH(Processes_Date) = " & Date.Now.Month & " AND YEAR(Processes_Date) = " & Date.Now.Year
                Case 4
                    sql_str = sql_str & " AND YEAR(Processes_Date) = " & Date.Now.Year
                Case 5
                    sql_str = sql_str & " AND CONVERT(VARCHAR, Processes_Date, 103) between '" & txt_start_dt.Text & "' AND '" & txt_end_dt.Text & "'"
            End Select

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

    Private Sub ddl_time_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_time.SelectedIndexChanged
        If ddl_time.SelectedValue <> 5 Then
            btn_apply.Enabled = False
            txt_start_dt.Enabled = False
            txt_end_dt.Enabled = False
            getData()
        Else
            btn_apply.Enabled = True
            txt_start_dt.Enabled = True
            txt_end_dt.Enabled = True
        End If
    End Sub

    Private Sub btn_apply_Click(sender As Object, e As EventArgs) Handles btn_apply.Click
        getData()
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Patiant Name ################
        If (e.CommandName = "pat_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Session.Item("patiant_id") = (row.Cells(0).Text)
            Response.Redirect("patientInfo.aspx")
        End If
    End Sub
End Class