Imports System.Data.SqlClient

Public Class companyProcesses
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("search") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If

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
            Dim sql_str As String = "SELECT Processes_ID, Processes_Reservation_Code, PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name AS Processes_Cilinc, SubService_AR_Name AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME, (case when Processes_State = 4 then 'تمت التسوية' else 'لم تتم التسوية' end) AS Processes_State FROM INC_CompanyProcesses
LEFT JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
WHERE INC_CompanyProcesses.C_ID = " & Session("company_id") & " AND Processes_State in (2,4)"

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