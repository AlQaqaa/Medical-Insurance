Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Imports Microsoft.Reporting.WebForms
Imports System.IO

Public Class printPatientProcesses
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

        Using main_ds
            Dim cmd As New SqlCommand("SELECT Processes_ID, Processes_Date, Processes_Residual, INVOICE_NO, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_IvoicesProcesses.P_ID) AS CARD_NO, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_IvoicesProcesses.P_ID) AS NAME_ARB, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_IvoicesProcesses.P_ID) AS BAGE_NO, (SELECT BIRTHDATE FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_IvoicesProcesses.P_ID) AS BIRTHDATE, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc) AS Clinic_AR_Name, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices) AS SubService_AR_Name, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE HAG_Processes_Doctor.Doctor_Processes_ID = INC_IvoicesProcesses.Processes_ID)), '') AS MedicalStaff_AR_Name FROM INC_IvoicesProcesses WHERE P_ID = " & Val(ViewState("patiant_id")) & " AND INVOICE_NO = " & ViewState("invoice_no"))
            Using sda As New SqlDataAdapter()
                cmd.Connection = insurance_SQLcon
                sda.SelectCommand = cmd
                sda.Fill(main_ds, "INC_IvoicesProcessesPatient")
                sda.Fill(dt_result)
                'Return main_ds
            End Using
        End Using

        Dim viewer As ReportViewer = New ReportViewer()

        Dim datasource As New ReportDataSource("patientMotalbaDataSet", main_ds.Tables("INC_IvoicesProcessesPatient"))
        viewer.LocalReport.DataSources.Clear()
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/patientMotalbaDetailes.rdlc")
        viewer.LocalReport.DataSources.Add(datasource)

        'Dim rp1 As ReportParameter
        'Dim rp2 As ReportParameter
        'Dim rp3 As ReportParameter
        'Dim rp4 As ReportParameter
        'Dim rp5 As ReportParameter
        'Dim rp6 As ReportParameter
        'Dim rp7 As ReportParameter
        'Dim rp8 As ReportParameter
        'Dim rp9 As ReportParameter
        'Dim rp10 As ReportParameter
        'Dim rp11 As ReportParameter

        'viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4, rp5, rp6, rp7, rp8, rp9, rp10, rp11})

        Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
        Dim r As String = "~/Reports/patientMotalbaDetailes.rdlc"
        ' Page.Controls.Add(rv)

        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim FolderLocation As String
        FolderLocation = Server.MapPath("~/Reports")
        Dim filepath As String = FolderLocation & "/patientMotalbaDetailes" & Session("User_Id") & ".pdf"
        If Directory.Exists(filepath) Then
            File.Delete(filepath)
        End If
        bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType, _
            encoding, extension, streamids, warnings)
        Dim fs As New FileStream(FolderLocation & "/patientMotalbaDetailes" & Session("User_Id") & ".pdf", FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        'Response.Redirect("~/Reports/patientMotalbaDetailes.pdf")
        ltEmbed.Text = "<embed src='../Reports/patientMotalbaDetailes" & Session("User_Id") & ".pdf' width='100%' height='600'>"
    End Sub

End Class