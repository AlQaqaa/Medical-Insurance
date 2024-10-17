Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.Reporting.WebForms

Public Class dailyProcesses
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("reports_per") = False Then
                    Response.Redirect("../Default.aspx", True)
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()
    End Sub

    Private Sub getData()

        Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

        Try
            Dim sql_str As String = "SELECT pros_code, Processes_Reservation_Code, PINC_ID AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, ISNULL(INC_MOTALBA_PRICES.Processes_Price, INC_CompanyProcesses.Processes_Price) AS Processes_Price, ISNULL(INC_MOTALBA_PRICES.Processes_Paid, INC_CompanyProcesses.Processes_Paid) AS Processes_Paid, INC_CompanyProcesses.Processes_Residual, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id), '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME, (case when Processes_State = 4 then 'تمت التسوية' else 'لم تتم التسوية' end) AS Processes_State, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_CompanyProcesses.C_ID) AS C_NAME FROM INC_CompanyProcesses
LEFT JOIN INC_MOTALBA_PRICES ON INC_MOTALBA_PRICES.Processes_ID = INC_CompanyProcesses.Processes_ID
 WHERE Processes_Date = '" & start_dt & "' AND Processes_State in (2,4) AND SUBSTRING([Processes_Reservation_Code],8 , 1) <> 0"


            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ds.Tables("dailyProcesses").Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            Dim datasource As New ReportDataSource("DPR_DS", ds.Tables("dailyProcesses"))
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/dailyProcesses.rdlc")
            ReportViewer1.LocalReport.DataSources.Add(datasource)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class