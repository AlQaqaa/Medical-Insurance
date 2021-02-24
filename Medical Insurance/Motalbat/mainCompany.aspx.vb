Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Globalization

Public Class mainCompany
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("print_motalba") = False Then
                    Response.Redirect("../Default.aspx")
                End If
            End If

        End If
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        Try
            Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
            Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

            Dim sel_com As New SqlCommand("SELECT INC_INVOICES.C_ID, INC_COMPANY_DATA.C_Name_Arb ,SUM(INC_IvoicesProcesses.Processes_Residual) AS TOTAL_VAL FROM [INC_INVOICES]
INNER JOIN INC_MOTALBAT ON INC_MOTALBAT.INVOICE_NO = INC_INVOICES.INVOICE_NO
INNER JOIN INC_IvoicesProcesses ON INC_IvoicesProcesses.Processes_ID = INC_MOTALBAT.Processes_ID
INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_INVOICES.C_ID
WHERE INC_INVOICES.DATE_FROM <= '" & start_dt & "' AND INC_INVOICES.DATE_TO >= '" & end_dt & "' AND INC_COMPANY_DATA.C_Level = " & ddl_companies.SelectedValue & "
GROUP BY  INC_INVOICES.C_ID, INC_COMPANY_DATA.C_Name_Arb", insurance_SQLcon)
            Dim dt_result As New DataTable
            If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                GridView1.DataSource = dt_result
                GridView1.DataBind()
                btn_print.Visible = True
            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
                Label1.Text = "<div class='alert alert-danger' role='alert'>لا يوجد بيانات لعرضها</div>"
                btn_print.Visible = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('" & ex.Message & "')", True)
        End Try
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Dim ch_counter As Integer = 0
        Dim total_val As Decimal = 0

        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")

            If ch.Checked = True Then
                main_ds.Tables("DataTable4").Rows.Add(dd.Cells(0).Text, dd.Cells(3).Text, CDec(dd.Cells(4).Text))
                ch_counter = ch_counter + 1
                total_val = total_val + CDec(dd.Cells(4).Text)
            End If
        Next

        If ch_counter = 0 Then
            lbl_msg.Visible = True
            lbl_msg.Text = "خطأ! لم يتم اختيار فاتورة، يرجى تحديد فاتورة أو أكثر"
            lbl_msg.ForeColor = Drawing.Color.Red
            Exit Sub
        Else
            lbl_msg.Visible = False
        End If

        Dim motalba_type As String = ""

        If ddl_invoice_type.SelectedValue = 1 Then
            motalba_type = "قائمة فواتير الخدمات الطبية"
        ElseIf ddl_invoice_type.SelectedValue = 2 Then
            motalba_type = "قائمة فواتير العمليات والإيواء"
        Else
            motalba_type = "قائمة فواتير الخدمات الطبية والعمليات والإيواء"
        End If

        Dim value_word As String = GetNumberToWord(total_val)

        Dim viewer As ReportViewer = New ReportViewer()

        Dim datasource As New ReportDataSource("DSmain", main_ds.Tables("DataTable4"))
        viewer.LocalReport.DataSources.Clear()
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/mainCompanyMotalba.rdlc")
        viewer.LocalReport.DataSources.Add(datasource)

        Dim rp1 As ReportParameter
        Dim rp2 As ReportParameter
        Dim rp3 As ReportParameter
        Dim rp4 As ReportParameter
        Dim rp5 As ReportParameter
        Dim rp6 As ReportParameter
        Dim rp7 As ReportParameter

        rp1 = New ReportParameter("company_name", ddl_companies.SelectedItem.Text)
        rp2 = New ReportParameter("value_text", value_word)
        rp3 = New ReportParameter("motalba_type", motalba_type.ToString)
        rp4 = New ReportParameter("mang_name", txt_mang_name.Text)
        rp5 = New ReportParameter("date_from", txt_start_dt.Text)
        rp6 = New ReportParameter("date_to", txt_end_dt.Text)
        rp7 = New ReportParameter("Adjective", txt_Adjective.Text)

        viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4, rp5, rp6, rp7})

        Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
        Dim r As String = "~/Reports/mainCompanyMotalba.rdlc"
        ' Page.Controls.Add(rv)

        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim FolderLocation As String
        FolderLocation = Server.MapPath("~/Reports")
        Dim filepath As String = FolderLocation & "/mainCompanyMotalba" & Session("INC_User_Id") & ".pdf"
        If Directory.Exists(filepath) Then
            File.Delete(filepath)
        End If
        bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
            encoding, extension, streamids, warnings)
        Dim fs As New FileStream(FolderLocation & "/mainCompanyMotalba" & Session("INC_User_Id") & ".pdf", FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        Response.Redirect("~/Reports/mainCompanyMotalba" & Session("INC_User_Id") & ".pdf", False)
    End Sub


End Class