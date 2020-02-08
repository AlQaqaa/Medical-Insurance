Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Globalization

Public Class servicesPricesShow
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim dt_result As New DataTable
    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            ViewState("profile_no") = Val(Request.QueryString("pID"))

            If ViewState("profile_no") = 0 Then
                Response.Redirect("createProfilePrices.aspx", True)
            End If

        End If
    End Sub

    Private Sub ddl_show_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_show_type.SelectedIndexChanged
        GridView1.DataSource = Nothing
        GridView1.DataBind()

        'If ddl_show_type.SelectedValue = 1 Then
        '    clinic_Panel.Visible = True
        '    groups_Panel.Visible = False
        'Else
        '    clinic_Panel.Visible = False
        '    groups_Panel.Visible = True

        'End If
    End Sub

    Private Sub btn_show_Click(sender As Object, e As EventArgs) Handles btn_show.Click
        Try

            Dim ser_price As String

            If ddl_price.SelectedValue = 1 Then
                ser_price = "INS_PRS"
            Else
                ser_price = "CASH_PRS"
            End If

            Dim sql_str As String = "SELECT SER_ID, ISNULL(" & ser_price & ", 0) AS SERVICE_PRICE, SubService_Code, SubService_AR_Name, SubService_EN_Name, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_servicesPrices.SubService_Clinic) AS CLINIC_NAME FROM INC_servicesPrices WHERE PROFILE_PRICE_ID = " & ViewState("profile_no")

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_res As New DataTable
            dt_res.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_res.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_res.Rows.Count > 0 Then
                btn_print.Enabled = True
                GridView1.DataSource = dt_res
                GridView1.DataBind()
            Else
                btn_print.Enabled = False
                dt_res.Rows.Clear()
                GridView1.DataSource = dt_res
                GridView1.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click

        Try

            Dim ser_price As String

            If ddl_price.SelectedValue = 1 Then
                ser_price = "INS_PRS"
            Else
                ser_price = "CASH_PRS"
            End If

            Dim sql_str As String = "SELECT SER_ID, ISNULL(" & ser_price & ", 0) AS SERVICE_PRICE, SubService_Code, SubService_AR_Name, SubService_EN_Name, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_servicesPrices.SubService_Clinic) AS CLINIC_NAME FROM INC_servicesPrices WHERE PROFILE_PRICE_ID = " & ViewState("profile_no")

            Using main_ds
                Dim cmd As New SqlCommand(sql_str)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = insurance_SQLcon
                    sda.SelectCommand = cmd
                    sda.Fill(main_ds, "INC_servicesPrices")
                    sda.Fill(dt_result)
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                End Using
            End Using


            Dim viewer As ReportViewer = New ReportViewer()

            Dim datasource As New ReportDataSource("serPricesReportDS", main_ds.Tables("INC_servicesPrices"))
            viewer.LocalReport.DataSources.Clear()
            viewer.ProcessingMode = ProcessingMode.Local
            viewer.LocalReport.ReportPath = Server.MapPath("Reports/servicesPrices.rdlc")
            viewer.LocalReport.DataSources.Add(datasource)

            Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
            Dim r As String = "~/Reports/servicesPrices.rdlc"
            Page.Controls.Add(rv)
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim bytes As Byte()
            Dim FolderLocation As String
            FolderLocation = Server.MapPath("~/Reports")
            Dim filepath As String = FolderLocation & "\servicesPrices.pdf"
            If Directory.Exists(filepath) Then
                File.Delete(filepath)
            End If
            bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType, _
                encoding, extension, streamids, warnings)
            Dim fs As New FileStream(FolderLocation & "\servicesPrices.pdf", FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
            fs.Close()
            Response.Redirect("~/Reports/servicesPrices.pdf", False)

        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub
End Class