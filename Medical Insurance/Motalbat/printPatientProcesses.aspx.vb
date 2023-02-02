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
            ViewState("invoice_type") = Val(Request.QueryString("invType"))

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

        Dim ss As String
        If ViewState("invoice_type") = 2 Then
            ss = "SELECT Processes_ID, Processes_Date, Processes_Residual, Processes_Price, Processes_Paid, INVOICE_NO,CARD_NO,NAME_ARB,BAGE_NO,CONVERT(VARCHAR, BIRTHDATE, 111) AS BIRTHDATE, Clinic_AR_Name,SubService_AR_Name, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, INC_COMPANY_DATA.C_Name_Arb,SubService_Code FROM INC_IvoicesProcesses
                INNER JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_IvoicesProcesses.Processes_Reservation_Code
                INNER JOIN Main_Clinic ON Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc
                INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices
                INNER JOIN EWA_Processes ON EWA_Processes.ewa_process_id = INC_IvoicesProcesses.Processes_ID
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = EWA_Processes.EWA_Record_Doctor
                INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID "
        Else
            ss = "SELECT Processes_ID, Processes_Date, Processes_Residual, Processes_Price, Processes_Paid, INVOICE_NO,CARD_NO,NAME_ARB,BAGE_NO,CONVERT(VARCHAR, BIRTHDATE, 111) AS BIRTHDATE, Clinic_AR_Name,SubService_AR_Name, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, INC_COMPANY_DATA.C_Name_Arb,SubService_Code FROM INC_IvoicesProcesses
                INNER JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_IvoicesProcesses.Processes_Reservation_Code
                INNER JOIN Main_Clinic ON Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc
                INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices
                LEFT JOIN HAG_Processes_Doctor ON HAG_Processes_Doctor.Doctor_Processes_ID = INC_IvoicesProcesses.Processes_ID AND ISNULL(HAG_Processes_Doctor.doc_type, 0) = 0
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = HAG_Processes_Doctor.Processes_Doctor_ID
                INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID "

        End If

        ss += " WHERE INVOICE_NO = " & ViewState("invoice_no") & " AND  INC_PATIANT.PINC_ID = " & Val(ViewState("patiant_id"))

        If DropDownList1.SelectedValue = 0 Then
            ss += " order by Processes_ID"
        Else
            ss += " order by SubService_Code,Processes_ID"
        End If
        Dim cmd As New SqlCommand(ss, insurance_SQLcon)
        If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
        dt_result.Load(cmd.ExecuteReader)
        insurance_SQLcon.Close()

        'Using main_ds
        '    Dim cmd As New SqlCommand(ss)
        '    Using sda As New SqlDataAdapter()
        '        cmd.Connection = insurance_SQLcon
        '        sda.SelectCommand = cmd
        '        sda.Fill(main_ds, "INC_IvoicesProcessesPatient")
        '        sda.Fill(dt_result)
        '        'Return main_ds
        '    End Using
        'End Using

        Dim viewer As ReportViewer = New ReportViewer()

        Dim datasource As New ReportDataSource("patientMotalbaDataSet", dt_result)
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/patientMotalbaDetailes.rdlc")
        ReportViewer1.LocalReport.DataSources.Add(datasource)
        ReportViewer1.LocalReport.Refresh()

        'Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
        'Dim r As String = "~/Reports/patientMotalbaDetailes.rdlc"
        '' Page.Controls.Add(rv)

        'Dim warnings As Warning() = Nothing
        'Dim streamids As String() = Nothing
        'Dim mimeType As String = Nothing
        'Dim encoding As String = Nothing
        'Dim extension As String = Nothing
        'Dim bytes As Byte()
        'Dim FolderLocation As String
        'FolderLocation = Server.MapPath("~/Reports")
        'Dim filepath As String = FolderLocation & "/patientMotalbaDetailes" & Session("INC_User_Id") & ".pdf"
        'If Directory.Exists(filepath) Then
        '    File.Delete(filepath)
        'End If
        'bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType, _
        '    encoding, extension, streamids, warnings)
        'Dim fs As New FileStream(FolderLocation & "/patientMotalbaDetailes" & Session("INC_User_Id") & ".pdf", FileMode.Create)
        'fs.Write(bytes, 0, bytes.Length)
        'fs.Close()
        ''Response.Redirect("~/Reports/patientMotalbaDetailes.pdf")
        'ltEmbed.Text = "<embed src='../Reports/patientMotalbaDetailes" & Session("INC_User_Id") & ".pdf' width='100%' height='600'>"
    End Sub

    Private Sub DropDownList1_CallingDataMethods(sender As Object, e As CallingDataMethodsEventArgs) Handles DropDownList1.CallingDataMethods

    End Sub

    Private Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        getPatientData()
    End Sub
End Class