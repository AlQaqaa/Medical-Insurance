Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.Reporting.WebForms

Public Class totalExpenses
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub getData()

        Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
        Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

        Try
            Dim sql_str As String = "SELECT [C_NAME_ARB],(CASE WHEN (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL ) IS NULL THEN '/////' ELSE (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL) END) AS MAIN_COMPANY, (SELECT * FROM INC_TOTALCOMPANYEXPENSES(INC_COMPANY_DATA.C_ID, '" & start_dt & "','" & end_dt & "')) AS TOTAL_EXP FROM INC_COMPANY_DATA WHERE C_STATE = 0"


            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ds.Tables("totalCompanyExpenses").Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            Dim datasource As New ReportDataSource("totalCompanyExp", ds.Tables("totalCompanyExpenses"))
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/totalExpensesCompany.rdlc")
            ReportViewer1.LocalReport.DataSources.Add(datasource)

            Dim rp1 As ReportParameter
            Dim rp2 As ReportParameter
            Dim rp3 As ReportParameter

            rp1 = New ReportParameter("from_dt", start_dt)
            rp2 = New ReportParameter("to_dt", end_dt)
            rp3 = New ReportParameter("user_name", Session("User_name").ToString)


            ReportViewer1.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3})

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()
    End Sub
End Class