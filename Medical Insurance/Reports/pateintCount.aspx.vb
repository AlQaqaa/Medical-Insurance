Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.Reporting.WebForms

Public Class pateintCount
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

            GetData()
        End If
    End Sub

    Private Sub GetData()
        Try
            Dim sql_str As String = "SELECT COUNT(INC_PATIANT.PINC_ID) AS P_COUNT, INC_COMPANY_DATA.C_Name_Arb FROM INC_PATIANT 
INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID
WHERE P_STATE = 0 GROUP BY INC_COMPANY_DATA.C_Name_Arb ORDER BY INC_COMPANY_DATA.C_Name_Arb"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ds.Tables("PATEINT_COUNT").Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            Dim datasource As New ReportDataSource("DataSet1", ds.Tables("PATEINT_COUNT"))
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/pateintCount.rdlc")
            ReportViewer1.LocalReport.DataSources.Add(datasource)

            Dim rp1 As ReportParameter

            rp1 = New ReportParameter("user_name", Session("INC_User_name").ToString)


            ReportViewer1.LocalReport.SetParameters(New ReportParameter() {rp1})
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class