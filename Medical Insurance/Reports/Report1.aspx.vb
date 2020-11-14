Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.Reporting.WebForms

Public Class Report1
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

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

    Private Sub getData()

        Dim ds As New DataSet1

        Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
        Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

        Try
            Dim sql_str As String = "SELECT HAG_Request.Req_Code, Processes_Reservation_Code, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, HAG_Processes.Processes_Cilinc, SubService_AR_Name, Processes_Price, CASH_PRS,ISNULL(NAME_ARB, '') AS PATIENT_NAME FROM HAG_Processes
LEFT JOIN HAG_Request ON HAG_Request.Req_PID = HAG_Processes.Processes_ID 
LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = HAG_Processes.Processes_SubServices
LEFT JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = HAG_Processes.Processes_Reservation_Code
LEFT JOIN INC_SERVICES_PRICES ON INC_SERVICES_PRICES.SER_ID = HAG_Processes.Processes_SubServices AND INC_SERVICES_PRICES.PROFILE_PRICE_ID = 3024
WHERE HAG_Processes.Processes_State = 2 AND HAG_Processes.Processes_Residual <> 0 AND Processes_Date BETWEEN '" & start_dt & "' AND '" & end_dt & "'"

            If ddl_clinics.SelectedValue <> 0 Then
                sql_str += " AND Processes_Cilinc = " & ddl_clinics.SelectedValue
            End If


            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ds.Tables("DataTable5").Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            Dim datasource As New ReportDataSource("DataSet10", ds.Tables("DataTable5"))
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report1.rdlc")
            ReportViewer1.LocalReport.DataSources.Add(datasource)

            Dim rp1 As ReportParameter
            Dim rp2 As ReportParameter
            Dim rp3 As ReportParameter

            rp1 = New ReportParameter("from_dt", start_dt)
            rp2 = New ReportParameter("to_dt", end_dt)
            rp3 = New ReportParameter("user_name", Session("INC_User_name").ToString)


            ReportViewer1.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3})

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()
    End Sub

End Class