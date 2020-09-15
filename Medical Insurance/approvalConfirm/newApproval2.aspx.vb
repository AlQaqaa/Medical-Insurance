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

    Private Sub ddl_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_type.SelectedIndexChanged

        If ddl_type.SelectedValue = 1 Then
            ddl_clinics.Enabled = True
            ddl_services.Enabled = True
            ddl_sub_service.Enabled = True
            txt_add_name.Visible = False
        ElseIf ddl_type.SelectedValue = 2 Then
            ddl_clinics.Enabled = False
            ddl_services.Enabled = False
            ddl_sub_service.Enabled = False
            txt_add_name.Visible = True

        Else
            ddl_clinics.Enabled = False
            ddl_services.Enabled = False
            ddl_sub_service.Enabled = False
        End If

    End Sub

    Private Sub ddl_services_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services.SelectedIndexChanged
        If ddl_type.SelectedValue = 1 Then
            Dim sel_com As New SqlCommand("SELECT SubService_ID, concat(SubService_Code, ' | ', SubService_AR_Name) AS SubService_AR_Name FROM Main_SubServices WHERE SubService_Service_ID = " & ddl_services.SelectedValue, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                ddl_sub_service.DataSource = dt_result
                ddl_sub_service.DataValueField = "SubService_ID"
                ddl_sub_service.DataTextField = "SubService_AR_Name"
                ddl_sub_service.DataBind()
            End If

        End If
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click

        If GridView1.Rows.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'يرجى اختيار خدمة واحدة على الأقل',
                showConfirmButton: false,
                timer: 1500
            });", True)
            Exit Sub
        End If

        add_action(1, 1, 1, "إنشاء طلب موافقة جديد للشركة: " & ddl_companies.SelectedItem.Text & " اسم المنتفع: " & txt_name.Text, Session("INC_User_Id"), GetIPAddress())

        Dim viewer As ReportViewer = New ReportViewer()

        Dim datasource As New ReportDataSource("confirmDS", Session("dt"))
        viewer.LocalReport.DataSources.Clear()
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/confirmApproval.rdlc")
        viewer.LocalReport.DataSources.Add(datasource)

        Dim rp1 As ReportParameter
        Dim rp2 As ReportParameter
        Dim rp3 As ReportParameter
        Dim rp4 As ReportParameter
        Dim rp5 As ReportParameter
        Dim rp6 As ReportParameter
        Dim rp7 As ReportParameter
        Dim rp8 As ReportParameter

        rp1 = New ReportParameter("company_name", ddl_companies.SelectedItem.Text)
        rp2 = New ReportParameter("pat_name", "للمنتفع: " & txt_name.Text)
        rp3 = New ReportParameter("card_no", " ")
        rp4 = New ReportParameter("emp_no", " ")
        rp5 = New ReportParameter("moshtark_name", " ")
        rp6 = New ReportParameter("relation_m", " ")
        rp7 = New ReportParameter("approv_no", " ")
        rp8 = New ReportParameter("notes", "ملاحظات: " & txt_notes.Text)

        viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4, rp5, rp6, rp7, rp8})

        Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
        Dim r As String = "~/Reports/confirmApproval.rdlc"
        ' Page.Controls.Add(rv)

        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim FolderLocation As String
        FolderLocation = Server.MapPath("~/Reports")
        Dim filepath As String = FolderLocation & "/confirmApproval" & Session("INC_User_Id") & ".pdf"
        If Directory.Exists(filepath) Then
            File.Delete(filepath)
        End If
        bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
            encoding, extension, streamids, warnings)
        Dim fs As New FileStream(FolderLocation & "/confirmApproval" & Session("INC_User_Id") & ".pdf", FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        Response.Redirect("~/Reports/confirmApproval" & Session("INC_User_Id") & ".pdf", True)
    End Sub

    Private Function getCompanyInfo() As DataTable
        Dim sel_com As New SqlCommand("SELECT TOP(1) PYMENT_TYPE,PROFILE_PRICE_ID FROM INC_COMPANY_DETIAL WHERE C_ID = " & ddl_companies.SelectedValue & " ORDER BY N DESC", insurance_SQLcon)
        Dim dt_result As New DataTable
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        Return dt_result
    End Function

    Private Sub btn_add_Click(sender As Object, e As EventArgs) Handles btn_add.Click

        Dim pay_type As String = ""
        If getCompanyInfo().Rows.Count > 0 Then
            If getCompanyInfo().Rows(0)("PYMENT_TYPE") = 1 Then
                pay_type = "CASH_PRS"
            Else
                pay_type = "INS_PRS"
            End If
        End If

        Dim service_price As Decimal = 0 

        If ddl_type.SelectedValue = 1 Then
            Dim query_str As String
            If ddl_companies.SelectedValue = 0 Then
                query_str = "SELECT SubService_ID, SubService_Code, SubService_AR_Name, 0 AS SubService_Price FROM Main_SubServices WHERE SubService_ID = " & ddl_sub_service.SelectedValue
            Else
                query_str = "SELECT SubService_ID, SubService_Code, SubService_AR_Name, ISNULL((SELECT " & pay_type & " FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND INC_SERVICES_PRICES.PROFILE_PRICE_ID = " & getCompanyInfo().Rows(0)("PROFILE_PRICE_ID") & "), 0) AS SubService_Price FROM Main_SubServices WHERE SubService_ID = " & ddl_sub_service.SelectedValue
            End If
            Dim sel_service As New SqlCommand(query_str, insurance_SQLcon)
            Dim dt_service_info As New DataTable
            dt_service_info.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_service_info.Load(sel_service.ExecuteReader)
            insurance_SQLcon.Close()

            If ddl_companies.SelectedValue = 0 Then
                service_price = CDec(txt_add_price.Text)
            Else
                service_price = dt_service_info.Rows(0)("SubService_Price")
            End If

            If GridView1.Rows.Count = 0 Then
                Dim dt As New DataTable
                dt.Columns.Add("SUB_SERVICE_ID")
                dt.Columns.Add("SERVICE_PRICE", System.Type.GetType("System.Decimal"))
                dt.Columns.Add("REQUEST_TYPE")
                dt.Columns.Add("SubService_Code")
                dt.Columns.Add("SubService_AR_Name")
                Dim dr = dt.NewRow()
                dr("SUB_SERVICE_ID") = ddl_sub_service.SelectedValue
                dr("SERVICE_PRICE") = service_price
                dr("REQUEST_TYPE") = "عملية/خدمة"
                dr("SubService_Code") = dt_service_info.Rows(0)("SubService_Code")
                dr("SubService_AR_Name") = dt_service_info.Rows(0)("SubService_AR_Name")
                dt.Rows.Add(dr)
                Session("dt") = dt
                GridView1.DataSource = dt
                GridView1.DataBind()

            Else
                Dim dt As New DataTable
                dt = Session("dt")
                Dim dr = dt.NewRow()
                dr("SUB_SERVICE_ID") = ddl_sub_service.SelectedValue
                dr("SERVICE_PRICE") = service_price
                dr("REQUEST_TYPE") = "عملية/خدمة"
                dr("SubService_Code") = dt_service_info.Rows(0)("SubService_Code")
                dr("SubService_AR_Name") = dt_service_info.Rows(0)("SubService_AR_Name")
                dt.Rows.Add(dr)
                Session("dt") = dt
                GridView1.DataSource = dt
                GridView1.DataBind()

            End If

        ElseIf ddl_type.SelectedValue = 2 Then
            If GridView1.Rows.Count = 0 Then
                Dim dt As New DataTable
                dt.Columns.Add("SUB_SERVICE_ID")
                dt.Columns.Add("SERVICE_PRICE", System.Type.GetType("System.Decimal"))
                dt.Columns.Add("REQUEST_TYPE")
                dt.Columns.Add("SubService_Code")
                dt.Columns.Add("SubService_AR_Name")
                Dim dr = dt.NewRow()
                dr("SUB_SERVICE_ID") = 999999
                dr("SERVICE_PRICE") = CDec(txt_add_price.Text)
                dr("REQUEST_TYPE") = "إضافة للعملية"
                dr("SubService_Code") = ""
                dr("SubService_AR_Name") = txt_add_name.Text
                dt.Rows.Add(dr)
                Session("dt") = dt
                GridView1.DataSource = dt
                GridView1.DataBind()

            Else
                Dim dt As New DataTable
                dt = Session("dt")
                Dim dr = dt.NewRow()
                dr("SUB_SERVICE_ID") = 999999
                dr("SERVICE_PRICE") = CDec(txt_add_price.Text)
                dr("REQUEST_TYPE") = "إضافة للعملية"
                dr("SubService_Code") = ""
                dr("SubService_AR_Name") = txt_add_name.Text
                dt.Rows.Add(dr)
                Session("dt") = dt
                GridView1.DataSource = dt
                GridView1.DataBind()

            End If
        End If

    End Sub

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        If ddl_companies.SelectedValue = 0 Then
            txt_company_name.Visible = True
            txt_add_price.Visible = True
        Else
            txt_company_name.Visible = False
            txt_add_price.Visible = False
        End If
    End Sub
End Class