Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Linq.Expressions
Imports DocumentFormat.OpenXml.Spreadsheet
Imports ClosedXML.Excel.XLPredefinedFormat

Public Class DetailsPatientProcesses
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then


            ViewState("invoice_no") = Val(Request.QueryString("invID"))
            ' ViewState("invoice_no") = 15
            ViewState("patiant_id") = Val(Request.QueryString("pID"))

            If ViewState("invoice_no") <> 0 Then
                Page.Title = "محتويات الفاتورة رقم " & ViewState("invoice_no")
                txt_invoice_no.Text = ViewState("invoice_no")
                Dim sel_com As New SqlCommand("SELECT INVOICE_NO, C_ID, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_INVOICES.C_ID) AS COMPANY_NAME, CONVERT(VARCHAR, DATE_FROM, 23) AS DATE_FROM, CONVERT(VARCHAR, DATE_TO, 23) AS DATE_TO FROM INC_INVOICES WHERE INVOICE_NO = " & ViewState("invoice_no"), insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()

                If dt_result.Rows.Count > 0 Then
                    Dim dr_inv = dt_result.Rows(0)
                    txt_company_name.Text = dr_inv!COMPANY_NAME
                    ViewState("company_no") = dr_inv!C_ID
                    txt_start_dt.Text = dr_inv!DATE_FROM
                    txt_end_dt.Text = dr_inv!DATE_TO

                End If
                getPatientData()
            Else
                Response.Redirect("Default.aspx", False)
            End If

        End If
    End Sub

    Private Sub getPatientData()
        Dim dt_result As New DataTable

        Try
            Dim cmd As New SqlCommand("SELECT Processes_ID, convert(varchar, Processes_Date, 103) as Processes_Date, Processes_Residual, INVOICE_NO,CARD_NO,NAME_ARB, Clinic_AR_Name,SubService_AR_Name, SubService_Code, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, Processes_Price, Processes_Paid FROM INC_IvoicesProcesses
LEFT JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_IvoicesProcesses.Processes_Reservation_Code
INNER JOIN Main_Clinic ON Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc
INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices
LEFT JOIN HAG_Processes_Doctor ON HAG_Processes_Doctor.Doctor_Processes_ID = INC_IvoicesProcesses.Processes_ID AND HAG_Processes_Doctor.doc_type = 0
LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = HAG_Processes_Doctor.Processes_Doctor_ID WHERE INC_PATIANT.PINC_ID = " & Val(ViewState("patiant_id")) & " AND INVOICE_NO = " & ViewState("invoice_no"), insurance_SQLcon)
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            dt_result.Load(cmd.ExecuteReader)
            insurance_SQLcon.Close()
        Catch ex As Exception

        Finally
            If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
        End Try

        If dt_result.Rows.Count > 0 Then
            txtCardNo.Text = dt_result.Rows(0)("CARD_NO")
            txtName.Text = dt_result.Rows(0)("NAME_ARB")
            GridView1.DataSource = dt_result
            GridView1.DataBind()
        End If

    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If (e.CommandName = "printProcess") Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridView1.Rows(index)
                'Dim p_link As String = "printPatientProcesses.aspx?invID=" & Val(txt_invoice_no.Text) & "&pID=" & (row.Cells(1).Text)
                'Response.Write("<script type='text/javascript'>")
                'Response.Write("window.open('" & p_link & "','_blank');")
                'Response.Write("</script>")
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "window.open('" & p_link & "','_blank');", True)
                'Response.Redirect("printPatientProcesses.aspx?invID=" & Val(txt_invoice_no.Text) & "&pID=" & (row.Cells(1).Text), False)

                Dim viewer As ReportViewer = New ReportViewer()
                viewer.LocalReport.DataSources.Clear()
                viewer.ProcessingMode = ProcessingMode.Local
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PrintReciept.rdlc")

                Dim rp1 As ReportParameter
                Dim rp2 As ReportParameter
                Dim rp3 As ReportParameter
                Dim rp4 As ReportParameter
                Dim rp5 As ReportParameter
                Dim rp6 As ReportParameter
                Dim rp7 As ReportParameter
                Dim rp8 As ReportParameter

                rp1 = New ReportParameter("CompanyName", txt_company_name.Text)
                rp2 = New ReportParameter("CardNo", txtCardNo.Text)
                rp3 = New ReportParameter("RecieptDate", row.Cells(5).Text)
                rp4 = New ReportParameter("PatName", txtName.Text)
                rp5 = New ReportParameter("AmountWord", GetNumberToWord(row.Cells(8).Text))
                rp6 = New ReportParameter("ServiceName", row.Cells(4).Text)
                rp7 = New ReportParameter("Amount", row.Cells(8).Text)
                rp8 = New ReportParameter("AmountDerham", CDec(row.Cells(8).Text) - Math.Truncate(CDec(row.Cells(8).Text)))

                viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4, rp5, rp6, rp7, rp8})

                Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
                Dim r As String = "~/Reports/PrintReciept.rdlc"
                ' Page.Controls.Add(rv)

                Dim warnings As Warning() = Nothing
                Dim streamids As String() = Nothing
                Dim mimeType As String = Nothing
                Dim encoding As String = Nothing
                Dim extension As String = Nothing
                Dim bytes As Byte()
                Dim FolderLocation As String
                FolderLocation = Server.MapPath("~/Reports")
                Dim filepath As String = FolderLocation & "/PrintReciept" & Session("INC_User_Id") & ".pdf"
                If Directory.Exists(filepath) Then
                    File.Delete(filepath)
                End If
                bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
                encoding, extension, streamids, warnings)
                Dim fs As New FileStream(FolderLocation & "/PrintReciept" & Session("INC_User_Id") & ".pdf", FileMode.Create)
                fs.Write(bytes, 0, bytes.Length)
                fs.Close()
                'Response.Redirect("~/Reports/PrintReciept" & Session("INC_User_Id") & ".pdf", False)
                Dim p_link As String = "../Reports/PrintReciept" & Session("INC_User_Id") & ".pdf"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "window.open('" & p_link & "','_blank');", True)
                'Response.Write("<script type='text/javascript'>")
                'Response.Write("window.open('" & p_link & "','_blank');")
                'Response.Write("</script>")
            End If

            If (e.CommandName = "editPrice") Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridView1.Rows(index)

                ViewState("ProcessesId") = row.Cells(1).Text
                TextBox1.Text = row.Cells(2).Text & " - " & row.Cells(3).Text
                txtPatPrice.Text = row.Cells(8).Text
                txtCompanyPrice.Text = row.Cells(7).Text
                mpePopUp.Show()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        mpePopUp.Hide()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim cmd As New SqlCommand("UPDATE HAG_Processes SET Processes_Paid=@Processes_Paid, Processes_Residual=@Processes_Residual WHERE Processes_ID=@Processes_ID", insurance_SQLcon)
            cmd.Parameters.AddWithValue("Processes_Paid", CDec(txtPatPrice.Text))
            cmd.Parameters.AddWithValue("Processes_Residual", CDec(txtCompanyPrice.Text))
            cmd.Parameters.AddWithValue("Processes_ID", ViewState("ProcessesId"))
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            cmd.ExecuteNonQuery()
            insurance_SQLcon.Close()
            getPatientData()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
        End Try
    End Sub

End Class