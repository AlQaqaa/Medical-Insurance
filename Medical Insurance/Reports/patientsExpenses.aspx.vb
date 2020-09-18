Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.Reporting.WebForms

Public Class patientsExpenses
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("reports_per") = False Then
                    Response.Redirect("../Default.aspx", True)
                    Exit Sub
                End If
            End If

        End If
    End Sub

    Private Sub getData()
        Try
            Dim sel_com As New SqlCommand("SELECT INC_Patient_Code, ISNULL(SUM(Processes_Price), 0) AS TOTAL_WITHDRAW, CARD_NO, NAME_ARB FROM INC_PATIANT
            LEFT JOIN HAG_Processes ON Processes_Reservation_Code = INC_PATIANT.INC_Patient_Code AND HAG_Processes.Processes_Date BETWEEN (SELECT MAX(DATE_START) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_PATIANT.C_ID AND DATE_END > GETDATE()) AND GETDATE()
            WHERE INC_Patient_Code IS NOT NULL AND INC_PATIANT.C_ID = " & ddl_companies.SelectedValue & "
            GROUP BY PINC_ID, INC_Patient_Code, CARD_NO, NAME_ARB", insurance_SQLcon)
            Dim dt_result As New DataTable
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                GridView1.DataSource = dt_result
                GridView1.DataBind()
                btn_print.Enabled = True
            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
                btn_print.Enabled = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        If ddl_companies.SelectedValue Then
            getData()

            Dim sel_com As New SqlCommand("SELECT TOP(1) DATE_START,MAX_PERSON FROM INC_COMPANY_DETIAL WHERE C_ID = " & ddl_companies.SelectedValue & " ORDER BY N DESC", insurance_SQLcon)
            Dim dt_result As New DataTable
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            ViewState("start_dt") = dt_result.Rows(0)("DATE_START")
            ViewState("MAX_PERSON") = dt_result.Rows(0)("MAX_PERSON")
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "printProcess") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Dim p_link As String = "patientProcesses.aspx?pID=" & (row.Cells(0).Text)
            Response.Write("<script type='text/javascript'>")
            Response.Write("window.open('" & p_link & "','_blank');")
            Response.Write("</script>")
            'Response.Redirect("printPatientProcesses.aspx?invID=" & Val(txt_invoice_no.Text) & "&pID=" & (row.Cells(1).Text), False)
        End If
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Try
            Dim sel_com As New SqlCommand("SELECT INC_Patient_Code, ISNULL(SUM(Processes_Price), 0) AS TOTAL_WITHDRAW, CARD_NO, NAME_ARB FROM INC_PATIANT
            LEFT JOIN HAG_Processes ON Processes_Reservation_Code = INC_PATIANT.INC_Patient_Code AND HAG_Processes.Processes_Date BETWEEN (SELECT MAX(DATE_START) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_PATIANT.C_ID AND DATE_END > GETDATE()) AND GETDATE()
            WHERE INC_Patient_Code IS NOT NULL AND INC_PATIANT.C_ID = " & ddl_companies.SelectedValue & "
            GROUP BY PINC_ID, INC_Patient_Code, CARD_NO, NAME_ARB", insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            main_ds.Tables("INC_PATIANT").Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            Dim viewer As ReportViewer = New ReportViewer()
            Dim datasource As New ReportDataSource("totalPatientExp", main_ds.Tables("INC_PATIANT"))
            viewer.LocalReport.DataSources.Clear()
            viewer.ProcessingMode = ProcessingMode.Local
            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/patientsExpenses.rdlc")
            viewer.LocalReport.DataSources.Add(datasource)

            Dim rp1 As ReportParameter
            Dim rp2 As ReportParameter
            Dim rp3 As ReportParameter
            Dim rp4 As ReportParameter

            rp1 = New ReportParameter("company_name", ddl_companies.SelectedItem.Text)
            rp2 = New ReportParameter("start_dt", ViewState("start_dt").ToString)
            rp3 = New ReportParameter("user_name", Session("INC_User_name").ToString)
            rp4 = New ReportParameter("max_val", ViewState("MAX_PERSON").ToString)

            viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4})

            Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
            Dim r As String = "~/Reports/patientsExpenses.rdlc"
            ' Page.Controls.Add(rv)

            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim bytes As Byte()
            Dim FolderLocation As String
            FolderLocation = Server.MapPath("~/Reports")
            Dim filepath As String = FolderLocation & "/patientsExpenses" & Session("INC_User_Id") & ".pdf"
            If Directory.Exists(filepath) Then
                File.Delete(filepath)
            End If
            bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
                encoding, extension, streamids, warnings)
            Dim fs As New FileStream(FolderLocation & "/patientsExpenses" & Session("INC_User_Id") & ".pdf", FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
            fs.Close()
            Response.Redirect("~/Reports/patientsExpenses" & Session("INC_User_Id") & ".pdf", False)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class