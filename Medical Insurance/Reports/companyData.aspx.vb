Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.Reporting.WebForms

Public Class companyData
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then


            Dim sql_str As String = "SELECT [C_NAME_ARB],(CASE WHEN (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL ) IS NULL THEN  '-' ELSE (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL) END)AS MAIN_COMPANY, (SELECT TOP (1) convert(varchar, [DATE_START], 23) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS [DATE_START], (SELECT TOP (1) convert(varchar, [DATE_END], 23) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS [DATE_END], (SELECT TOP (1) [MAX_VAL] FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS [MAX_VAL] FROM INC_COMPANY_DATA WHERE C_STATE = 0"


            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ds.Tables("INC_COMPANY_DATA").Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            Dim datasource As New ReportDataSource("companyDataDS", ds.Tables("INC_COMPANY_DATA"))
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/companyDataReport.rdlc")
            ReportViewer1.LocalReport.DataSources.Add(datasource)

            'Dim rp1 As ReportParameter
            'Dim rp2 As ReportParameter
            Dim rp3 As ReportParameter

            'rp1 = New ReportParameter("from_dt", start_dt)
            'rp2 = New ReportParameter("to_dt", end_dt)
            rp3 = New ReportParameter("user_name", Session("INC_User_name").ToString)


            ReportViewer1.LocalReport.SetParameters(New ReportParameter() {rp3})
        End If

    End Sub

End Class