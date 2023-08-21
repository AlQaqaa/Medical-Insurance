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
                Dim sel_com As New SqlCommand("SELECT INVOICE_NO, INC_INVOICES.C_ID, C_Name_Arb AS COMPANY_NAME, CONVERT(VARCHAR, DATE_FROM, 23) AS DATE_FROM, CONVERT(VARCHAR, DATE_TO, 23) AS DATE_TO, INC_COMPANY_DETIAL.CONTRACT_NO FROM INC_INVOICES
INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_INVOICES.C_ID
INNER JOIN INC_COMPANY_DETIAL ON INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID AND INC_COMPANY_DETIAL.DATE_START <= GETDATE() AND INC_COMPANY_DETIAL.DATE_END >= GETDATE() WHERE INVOICE_NO = " & ViewState("invoice_no"), insurance_SQLcon)
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
                    ViewState("contract_no") = dr_inv!CONTRACT_NO
                    txt_start_dt.Text = dr_inv!DATE_FROM
                    txt_end_dt.Text = dr_inv!DATE_TO

                End If
                getPatientData()
            Else
                Response.Redirect("Default.aspx", False)
            End If

        End If
    End Sub

    Private Sub getPatientData(Optional ByVal filter As Int16 = 0)
        Dim dt_result As New DataTable

        Dim str As String = ""
        If filter = 0 Then
            str = "SELECT INC_IvoicesProcesses.Processes_ID, convert(varchar, Processes_Date, 103) as Processes_Date, ISNULL(INC_MOTALBA_PRICES.Processes_Residual, INC_IvoicesProcesses.Processes_Residual) AS Processes_Residual, INVOICE_NO,CARD_NO,NAME_ARB, Clinic_AR_Name,SubService_AR_Name, SubService_Code, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(INC_MOTALBA_PRICES.Processes_Price, INC_IvoicesProcesses.Processes_Price) AS Processes_Price, ISNULL(INC_MOTALBA_PRICES.Processes_Paid, INC_IvoicesProcesses.Processes_Paid) AS Processes_Paid, (select top(1) [PERSON_PER] from [dbo].[INC_SUB_SERVICES_RESTRICTIONS] where C_ID = @company_id and SubService_ID = INC_IvoicesProcesses.Processes_SubServices and CONTRACT_NO = @contract_no) as [PERSON_PER] FROM INC_IvoicesProcesses
LEFT JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_IvoicesProcesses.Processes_Reservation_Code
INNER JOIN Main_Clinic ON Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc
INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices
LEFT JOIN HAG_Processes_Doctor ON HAG_Processes_Doctor.Doctor_Processes_ID = INC_IvoicesProcesses.Processes_ID AND HAG_Processes_Doctor.doc_type = 0
LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = HAG_Processes_Doctor.Processes_Doctor_ID 
LEFT JOIN INC_MOTALBA_PRICES ON INC_MOTALBA_PRICES.Processes_ID = INC_IvoicesProcesses.Processes_ID
WHERE INC_PATIANT.PINC_ID = " & Val(ViewState("patiant_id")) & " AND INVOICE_NO = " & ViewState("invoice_no")
        Else
            str = "select count(Processes_ID) as Processes_ID, Processes_Date ,CARD_NO  ,INVOICE_NO  ,NAME_ARB, N'-' as SubService_Code, N'المعمل' as SubService_AR_Name,
sum( Processes_Price  )  as Processes_Price ,
sum( Processes_Paid  ) as Processes_Paid ,
sum( Processes_Residual  ) as Processes_Residual ,
sum( [PERSON_PER]  ) as [PERSON_PER] 
,Clinic_AR_Name
from ( SELECT INC_IvoicesProcesses.Processes_ID, convert(varchar, Processes_Date, 103) as Processes_Date,
ISNULL(INC_MOTALBA_PRICES.Processes_Residual, INC_IvoicesProcesses.Processes_Residual) AS Processes_Residual, 
INVOICE_NO,CARD_NO,NAME_ARB, Clinic_AR_Name,SubService_AR_Name, SubService_Code,
ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, 
ISNULL(INC_MOTALBA_PRICES.Processes_Price, INC_IvoicesProcesses.Processes_Price) AS Processes_Price,
ISNULL(INC_MOTALBA_PRICES.Processes_Paid, INC_IvoicesProcesses.Processes_Paid) AS Processes_Paid, 
(select top(1) [PERSON_PER] from [dbo].[INC_SUB_SERVICES_RESTRICTIONS] where C_ID = @company_id and 
SubService_ID = INC_IvoicesProcesses.Processes_SubServices and CONTRACT_NO = @contract_no) as [PERSON_PER] 
FROM INC_IvoicesProcesses
LEFT JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_IvoicesProcesses.Processes_Reservation_Code
INNER JOIN Main_Clinic ON Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc
INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices
LEFT JOIN HAG_Processes_Doctor ON HAG_Processes_Doctor.Doctor_Processes_ID = INC_IvoicesProcesses.Processes_ID AND HAG_Processes_Doctor.doc_type = 0
LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = HAG_Processes_Doctor.Processes_Doctor_ID 
LEFT JOIN INC_MOTALBA_PRICES ON INC_MOTALBA_PRICES.Processes_ID = INC_IvoicesProcesses.Processes_ID
WHERE INC_PATIANT.PINC_ID = " & Val(ViewState("patiant_id")) & " AND INVOICE_NO = " & ViewState("invoice_no") & " and clinic_id =42) as ta group by  Processes_Date  ,CARD_NO  ,INVOICE_NO  ,NAME_ARB   ,Clinic_AR_Name"
        End If
        Try
            Dim cmd As New SqlCommand(str, insurance_SQLcon)
            cmd.Parameters.AddWithValue("@company_id", ViewState("company_no"))
            cmd.Parameters.AddWithValue("@contract_no", ViewState("contract_no"))
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

                Dim p_link As String = "../Reports/PrintReciept" & Session("INC_User_Id") & ".pdf"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "window.open('" & p_link & "','_blank');", True)

            End If

            If (e.CommandName = "editPrice") Then
                If ddlFilter.SelectedValue <> 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                                    position: 'center',
                                    icon: 'error',
                                    title: 'خطأ! لا يمكن تعديل الأسعار يرجى إلغاء التصفية وعرض جميع الخدمات أولا',
                                    showConfirmButton: false,
                                    timer: 3000
                                });playSound('../Style/error.mp3');", True)
                    Exit Sub
                End If
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridView1.Rows(index)

                ViewState("ProcessesId") = row.Cells(1).Text
                TextBox1.Text = row.Cells(2).Text & " - " & row.Cells(3).Text
                HiddenField1.Value = CDec(row.Cells(9).Text)
                'txtPatPrice.Text = CDec(row.Cells(6).Text) * CDec(row.Cells(9).Text) / 100
                'txtCompanyPrice.Text = CDec(row.Cells(6).Text) * (100 - CDec(row.Cells(9).Text)) / 100
                'txtPrice.Text = row.Cells(6).Text
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
            Dim delComm As New SqlCommand("DELETE FROM INC_MOTALBA_PRICES WHERE Processes_ID=" & ViewState("ProcessesId"), insurance_SQLcon)
            Dim cmd As New SqlCommand("INSERT INC_MOTALBA_PRICES (Processes_ID,Processes_Price,Processes_Paid,Processes_Residual) VALUES (@Processes_ID,@Processes_Price, @Processes_Paid, @Processes_Residual)", insurance_SQLcon)
            cmd.Parameters.AddWithValue("Processes_ID", ViewState("ProcessesId"))
            cmd.Parameters.AddWithValue("Processes_Price", CDec(txtPrice.Text))
            cmd.Parameters.AddWithValue("Processes_Paid", CDec(txtPatPrice.Text))
            cmd.Parameters.AddWithValue("Processes_Residual", CDec(txtCompanyPrice.Text))
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            delComm.ExecuteNonQuery()
            cmd.ExecuteNonQuery()
            insurance_SQLcon.Close()
            getPatientData()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
        End Try
    End Sub

    Private Sub ddlFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFilter.SelectedIndexChanged
        getPatientData(ddlFilter.SelectedValue)
    End Sub

    'Private Sub txtPrice_TextChanged(sender As Object, e As EventArgs) Handles txtPrice.TextChanged
    '    If txtPrice.Text <> "" Then
    '        txtPatPrice.Text = CDec(txtPrice.Text) * CDec(ViewState("patPer")) / 100
    '        txtCompanyPrice.Text = CDec(txtPrice.Text) * (100 - CDec(ViewState("patPer"))) / 100
    '    End If
    'End Sub
End Class