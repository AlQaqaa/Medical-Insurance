Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Public Class subServicesPrice
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)


    Dim main_ds As New DataSet1

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

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged

        Try
            Dim sel_com As New SqlCommand("SELECT 0 AS [Service_ID], N'يرجى اختيار قسم' AS [Service_AR_Name] FROM [Main_Services] UNION SELECT [Service_ID], [Service_AR_Name] FROM [Main_Services] where Service_State = 0 and Service_Clinic = " & ddl_clinics.SelectedValue, insurance_SQLcon)
            Dim dt_result As New DataTable
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            ddl_services.DataSource = dt_result
            ddl_services.DataValueField = "Service_ID"
            ddl_services.DataTextField = "Service_AR_Name"
            ddl_services.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Try
            Dim query As String = "select Main_SubServices.SubService_AR_Name, Main_SubServices.SubService_Code, Main_SubServices.SubService_EN_Name, Main_Clinic.Clinic_AR_Name, ISNULL(INC_SERVICES_PRICES.INS_PRS, 0) as service_price from INC_SERVICES_PRICES
inner join Main_SubServices on Main_SubServices.SubService_ID = INC_SERVICES_PRICES.SER_ID
inner join Main_Clinic on Main_Clinic.clinic_id = Main_SubServices.SubService_Clinic where PROFILE_PRICE_ID = " & ddl_prices_profile.SelectedValue & " AND DOCTOR_ID = " & dll_doctors.SelectedValue

            If ddl_clinics.SelectedValue <> 0 Then
                query += " and SubService_Clinic = " & ddl_clinics.SelectedValue
            End If

            If Val(ddl_services.SelectedValue) <> 0 Then
                query += " and SubService_Service_ID = " & ddl_services.SelectedValue
            End If

            query += " ORDER BY SubService_Code"

            Dim selComm As New SqlCommand(query, insurance_SQLcon)
            If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            main_ds.Tables("subServicesPrice").Load(selComm.ExecuteReader)
            insurance_SQLcon.Close()

            If main_ds.Tables("subServicesPrice").Rows.Count > 0 Then
                Dim datasource As New ReportDataSource("DataSet1", main_ds.Tables("subServicesPrice"))
                ReportViewer1.LocalReport.DataSources.Clear()
                ReportViewer1.ProcessingMode = ProcessingMode.Local
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/subServicesPrice.rdlc")
                ReportViewer1.LocalReport.DataSources.Add(datasource)

                Dim rp1 As ReportParameter
                Dim rp2 As ReportParameter
                Dim rp3 As ReportParameter

                Dim docName As String = ""
                If dll_doctors.SelectedValue <> 0 Then
                    docName = " اسم الطبيب: " & dll_doctors.SelectedItem.Text
                End If

                rp1 = New ReportParameter("pricesProfile", ddl_prices_profile.SelectedItem.Text)
                rp2 = New ReportParameter("doctor", docName)
                rp3 = New ReportParameter("user_name", Session("INC_User_name").ToString)


                ReportViewer1.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3})
            Else

            End If
        Catch ex As Exception

        End Try
    End Sub
End Class