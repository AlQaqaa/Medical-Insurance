Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Globalization

Public Class newApproval2
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("confirm_approval") = False Then
                    Response.Redirect("../Default.aspx")
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click

        Using main_ds
            Dim sel_com As New SqlCommand("select C_Name_Arb from INC_COMPANY_DATA WHERE C_ID = " & ddl_companies.SelectedValue)
            Using sda As New SqlDataAdapter()
                sel_com.Connection = insurance_SQLcon
                sda.SelectCommand = sel_com
                sda.Fill(main_ds, "INC_COMPANY_DATA1")
            End Using
        End Using

        Dim viewer As ReportViewer = New ReportViewer()

        Dim datasource As New ReportDataSource("DataSet11", main_ds.Tables("INC_COMPANY_DATA1"))
        viewer.LocalReport.DataSources.Clear()
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/confirmApproval2.rdlc")
        viewer.LocalReport.DataSources.Add(datasource)

        Dim rp1 As ReportParameter
        Dim rp2 As ReportParameter
        Dim rp3 As ReportParameter
        Dim rp4 As ReportParameter

        rp1 = New ReportParameter("company_name", ddl_companies.SelectedItem.Text)
        rp2 = New ReportParameter("pat_name", txt_name.Text)
        rp3 = New ReportParameter("pros_name", txt_service_name.Text)
        rp4 = New ReportParameter("price", CDec(txt_value.Text))

        viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4})

        Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
        Dim r As String = "~/Reports/confirmApproval2.rdlc"
        ' Page.Controls.Add(rv)

        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim FolderLocation As String
        FolderLocation = Server.MapPath("~/Reports")
        Dim filepath As String = FolderLocation & "/confirmApproval2" & Session("INC_User_Id") & ".pdf"
        If Directory.Exists(filepath) Then
            File.Delete(filepath)
        End If
        bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
            encoding, extension, streamids, warnings)
        Dim fs As New FileStream(FolderLocation & "/confirmApproval2" & Session("INC_User_Id") & ".pdf", FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        Response.Redirect("~/Reports/confirmApproval2" & Session("INC_User_Id") & ".pdf", True)
    End Sub
End Class