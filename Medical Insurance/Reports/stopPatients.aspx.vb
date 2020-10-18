Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.Reporting.WebForms

Public Class stopPatients
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
        Try
            Dim sql_str As String = "SELECT PINC_ID, CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID) AS C_Name_Arb, PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID FROM INC_PATIANT WHERE 1=1"

            Select Case DropDownList1.SelectedValue
                Case 1
                    sql_str = sql_str & " AND P_STATE = 1"
                    ViewState("report_title") = "تقرير عن المنتفعين الموقوفين"
                Case 2
                    sql_str = sql_str & " AND EXP_DATE < '" & Date.Now.Date & "'"
                    ViewState("report_title") = "تقرير عن المنتفعين الموقوفين"
                Case 3
                    sql_str = sql_str & " AND P_STATE = 0"
                    ViewState("report_title") = "تقرير عن المنتفعين المفعلين"
                Case Else
                    ViewState("report_title") = "تقرير عن جميع المنتفعين"
            End Select

            If DropDownList2.SelectedValue <> 0 Then
                sql_str = sql_str & " AND C_ID = " & DropDownList2.SelectedValue
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ds.Tables("INC_STOP_PATIANT").Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            Dim datasource As New ReportDataSource("patDataDS", ds.Tables("INC_STOP_PATIANT"))
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/stopPatientsReport.rdlc")
            ReportViewer1.LocalReport.DataSources.Add(datasource)

            Dim rp1 As ReportParameter
            'Dim rp2 As ReportParameter
            Dim rp3 As ReportParameter

            rp1 = New ReportParameter("report_title", ViewState("report_title").ToString)
            'rp2 = New ReportParameter("to_dt", end_dt)
            rp3 = New ReportParameter("user_name", Session("INC_User_name").ToString)


            ReportViewer1.LocalReport.SetParameters(New ReportParameter() {rp1, rp3})
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class