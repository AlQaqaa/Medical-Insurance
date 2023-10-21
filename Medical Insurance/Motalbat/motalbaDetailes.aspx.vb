Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.Reporting.WebForms

Public Class motalbaDetailes
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not Me.IsPostBack Then
            ViewState("invoice_no") = Val(Request.QueryString("invID"))
            ViewState("oreder_by") = Val(Request.QueryString("order"))

            If ViewState("invoice_no") = 0 Then
                Response.Redirect("Default.aspx", False)
                Exit Sub
            End If

            Label1.Text = "مرفقات الفاتورة رقم: " & ViewState("invoice_no")
            Page.Title = "مرفقات الفاتورة رقم: " & ViewState("invoice_no")

            getPatientProcessesInvoice()

            Dim datasource As New ReportDataSource("patientProcesses", main_ds.Tables("patientProcessesInvoice"))
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/motalbaDetailes.rdlc")
            ReportViewer1.LocalReport.DataSources.Add(datasource)


        End If
    End Sub

    Sub getPatientProcessesInvoice()
        Using main_ds

            Dim ss As String
            ss = "SELECT Processes_ID, Processes_Date, Processes_Residual, Processes_Price, Processes_Paid, INVOICE_NO,CARD_NO,NAME_ARB,BAGE_NO,CONVERT(VARCHAR, BIRTHDATE, 111) AS BIRTHDATE, Clinic_AR_Name,SubService_AR_Name, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, INC_COMPANY_DATA.C_Name_Arb,SubService_Code FROM INC_IvoicesProcesses
                INNER JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_IvoicesProcesses.Processes_Reservation_Code
                LEFT JOIN Main_Clinic ON Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc
                LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices
                LEFT JOIN HAG_Processes_Doctor ON HAG_Processes_Doctor.Doctor_Processes_ID = INC_IvoicesProcesses.Processes_ID AND ISNULL(HAG_Processes_Doctor.doc_type, 0) = 0
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = HAG_Processes_Doctor.Processes_Doctor_ID
                INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID WHERE INVOICE_NO = " & ViewState("invoice_no")
            ' ss += " order by CARD_NO,NAME_ARB"

            If ViewState("oreder_by") = 0 Then ss += " ORDER BY Processes_ID"
            If ViewState("oreder_by") = 1 Then ss += " ORDER BY INC_Patient_Code"
            Dim sel_com As New SqlCommand(ss)
            Using sda As New SqlDataAdapter()
                sel_com.Connection = insurance_SQLcon
                sda.SelectCommand = sel_com
                sda.Fill(main_ds, "patientProcessesInvoice")
                'sda.Fill(dt_result)
                'Return main_ds
            End Using
        End Using
    End Sub

End Class