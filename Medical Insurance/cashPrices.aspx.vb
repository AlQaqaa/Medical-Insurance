Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography
Imports Microsoft.Reporting.WebForms

Public Class cashPrices
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)
    Dim profile_no As Integer = 3025

    Public Function GetIPAddress() As String


        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("pruser_id") = 1

        profile_no = 3025

    End Sub

    Protected Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        Try
            For Each dd As GridViewRow In GridView1.Rows
                Dim ch As CheckBox = dd.FindControl("CheckBox2")
                Dim txt_private_prc As TextBox = dd.FindControl("txt_private_price")
                'Dim txt_invoice_prc As TextBox = dd.FindControl("txt_invoice_price")
                'Dim txt_invoice_per As TextBox = dd.FindControl("txt_invoice_per")

                Dim is_per As Boolean = False
                Dim inv_val As Decimal = 0
                'If txt_invoice_prc.Text = "" And txt_invoice_per.Text <> "" Then
                '    is_per = True
                '    inv_val = Val(txt_invoice_per.Text)
                'ElseIf txt_invoice_prc.Text <> "" And txt_invoice_per.Text = "" Then
                '    is_per = False
                '    inv_val = CDec(txt_invoice_prc.Text)
                'End If

                If ch.Checked = True Then
                    Dim insClinic As New SqlCommand
                    insClinic.Connection = insurance_SQLcon
                    insClinic.CommandText = "INC_addServicesPrice"
                    insClinic.CommandType = CommandType.StoredProcedure
                    insClinic.Parameters.AddWithValue("@service_id", dd.Cells(0).Text)
                    insClinic.Parameters.AddWithValue("@private_prc", 0)
                    insClinic.Parameters.AddWithValue("@inc_prc", CDec(txt_private_prc.Text))
                    insClinic.Parameters.AddWithValue("@inv_prc", 0)
                    insClinic.Parameters.AddWithValue("@user_id", Session("INC_User_Id"))
                    insClinic.Parameters.AddWithValue("@user_ip", GetIPAddress())
                    insClinic.Parameters.AddWithValue("@profile_price_id", Val(Session("profile_no")))
                    insClinic.Parameters.AddWithValue("@cost_prc", 0)
                    insClinic.Parameters.AddWithValue("@doctor_id", dll_doctors.SelectedValue)
                    insurance_SQLcon.Open()
                    insClinic.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                    insClinic.CommandText = ""
                End If
            Next
            '       

            ''-------------------------------------------------------
            'If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            'Dim CD = New SqlCommand("INSERT INTO Main_History_Log (SystemNo,ProcessesLevel,ProcessesType, ProcessesDesc,UserId, UserIp)  VALUES (@SystemNo,@ProcessesLevel,@ProcessesType, @ProcessesDesc,@UserId, @UserIp)", insurance_SQLcon)
            'CD.Parameters.AddWithValue("@SystemNo", "15")
            'CD.Parameters.AddWithValue("@ProcessesLevel", "1")
            'CD.Parameters.AddWithValue("@ProcessesType", "1")
            'CD.Parameters.AddWithValue("@ProcessesDesc", " : تم التعديل على اسعار الخدمات   " & ddl_clinics.Text)
            'CD.Parameters.AddWithValue("@UserId", Session("pruser_id"))
            'CD.Parameters.AddWithValue("@UserIp", GetIPAddress())

            'CD.ExecuteNonQuery()
            'insurance_SQLcon.Close()
            ''-------------------------------------------------------

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({ position: 'top-end',icon: 'success', title: 'تمت عملية حفظ البيانات بنجاح',showConfirmButton: false,                timer: 1500      });", True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub

    Private Sub ddl_show_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_show_type.SelectedIndexChanged
        If ddl_show_type.SelectedValue = 1 Then
            ddl_group.Enabled = False
            ddl_services_group.Enabled = False

            'txt_private_all.Enabled = False
            'txt_inc_price_all.Enabled = False
            'txt_invoice_price_all.Enabled = False
            'txt_cost_price_all.Enabled = False
        Else

            ddl_group.Enabled = True
            ddl_services_group.Enabled = True
            'txt_private_all.Enabled = True
            'txt_inc_price_all.Enabled = True
            'txt_invoice_price_all.Enabled = True
            'txt_cost_price_all.Enabled = True
        End If

        GridView1.DataSource = Nothing
        GridView1.DataBind()
    End Sub

    Private Sub ddl_group_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_group.SelectedIndexChanged
        Try
            GridView1.DataSource = Nothing
            GridView1.DataBind()

            Dim sel_com As New SqlCommand("SELECT 0 AS SubGroup_ID, 'الكل' AS SubGroup_ARname FROM Main_SubGroup UNION SELECT SubGroup_ID, SubGroup_ARname FROM Main_SubGroup WHERE SubGroup_State = 0 AND MainGroup_ID = " & ddl_group.SelectedValue, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                ddl_services_group.DataSource = dt_result
                ddl_services_group.DataValueField = "SubGroup_ID"
                ddl_services_group.DataTextField = "SubGroup_ARname"
                ddl_services_group.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub ddl_services_group_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services_group.SelectedIndexChanged
        GridView1.DataSource = Nothing
        GridView1.DataBind()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")

            If CheckBox1.Checked = True Then
                ch.Checked = True
            Else
                ch.Checked = False

            End If
        Next
    End Sub

    Private Sub btn_apply_Click(sender As Object, e As EventArgs) Handles btn_apply.Click

        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")
            Dim txt_service_price As TextBox = dd.FindControl("txt_service_price")
            'Dim txt_invoice_price As TextBox = dd.FindControl("txt_invoice_price")
            'Dim txt_invoice_per As TextBox = dd.FindControl("txt_invoice_per")

            If ch.Checked = True Then
                If txt_private_all.Text = "" And txt_per_add.Text <> "" Then
                    Dim add_val As Decimal = CDec(txt_service_price.Text) + (CDec(txt_service_price.Text) * CDec(txt_per_add.Text) / 100)
                    txt_service_price.Text = CDec(add_val)
                End If

                If txt_private_all.Text <> "" And txt_per_add.Text = "" Then
                    txt_service_price.Text = CDec(txt_private_all.Text)
                End If

                'If txt_invoice_price_all.Text <> "" And txt_invoice_per_all.Text = "" Then
                '    txt_invoice_price.Text = CDec(txt_invoice_price_all.Text)
                'End If

                'If txt_invoice_price_all.Text = "" And txt_invoice_per_all.Text <> "" Then
                '    txt_invoice_per.Text = CDec(txt_invoice_per_all.Text)
                'End If
            End If

        Next

    End Sub

    Sub getSubServices()
        Try
            GridView1.DataSource = Nothing
            GridView1.DataBind()
            Dim CASH_PRS As String = "ISNULL(  (SELECT top(1) CASH_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND PROFILE_PRICE_ID = 3024   AND DOCTOR_ID = " & dll_doctors.SelectedItem.Value & " order by n DESC),0) AS CASH_PRS "
            'Dim CASH_PRS As String = "ISNULL((SELECT top(1) CASH_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND PROFILE_PRICE_ID = " & profile_no & " order by n DESC), ISNULL((SELECT top(1) CASH_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND DOCTOR_ID = " & dll_doctors.SelectedItem.Value & " AND PROFILE_PRICE_ID = (SELECT profile_Id FROM INC_PRICES_PROFILES WHERE is_default = 1) order by n DESC),0)) AS CASH_PRS,"

            'Dim INVO_PRS As String = "ISNULL((SELECT top(1) INVO_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND PROFILE_PRICE_ID = " & profile_no & " order by n DESC), ISNULL((SELECT top(1) INVO_PRS FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_ID AND DOCTOR_ID = " & dll_doctors.SelectedItem.Value & " AND PROFILE_PRICE_ID = (SELECT profile_Id FROM INC_PRICES_PROFILES WHERE is_default = 1) order by n DESC),0)) AS INVO_PRS "


            Dim sql_str As String = "SELECT SubService_ID, SubService_Code, SubService_AR_Name, SubService_EN_Name, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = Main_SubServices.SubService_Clinic) AS CLINIC_NAME, " & CASH_PRS & " FROM Main_SubServices WHERE SubService_State = 0 "

            If ddl_clinics.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SubService_Clinic = " & ddl_clinics.SelectedValue
            End If
            If ddl_services.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SubService_Service_ID = " & ddl_services.SelectedItem.Value
            End If
            If ddl_group.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SubService_Group in (SELECT SubGroup_ID FROM Main_SubGroup WHERE MainGroup_ID = " & ddl_group.SelectedItem.Value & ")"
                If ddl_services_group.SelectedValue <> 0 Then
                    sql_str = sql_str & " AND SubService_Group = " & ddl_services_group.SelectedItem.Value
                End If
            End If

            sql_str = sql_str & " ORDER BY SubService_AR_Name"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_res As New DataTable
            dt_res.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_res.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_res.Rows.Count > 0 Then
                Panel1.Visible = True
                btn_save.Visible = True
                GridView1.DataSource = dt_res
                GridView1.DataBind()
                For i = 0 To dt_res.Rows.Count - 1
                    Dim dd As GridViewRow = GridView1.Rows(i)

                    Dim txt_private_prc As TextBox = dd.FindControl("txt_service_price")
                    'Dim txt_invoice_prc As TextBox = dd.FindControl("txt_invoice_price")

                    txt_private_prc.Text = dt_res.Rows(i)("CASH_PRS")
                    'txt_invoice_prc.Text = dt_res.Rows(i)("INVO_PRS")
                Next
            Else
                Panel1.Visible = False
                btn_save.Visible = False
                dt_res.Rows.Clear()
                GridView1.DataSource = dt_res
                GridView1.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click

        getSubServices()


    End Sub

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        GridView1.DataSource = Nothing
        GridView1.DataBind()
    End Sub

    Private Sub ddl_services_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services.SelectedIndexChanged
        GridView1.DataSource = Nothing
        GridView1.DataBind()
    End Sub

    'Protected Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
    '    'Dim sql_str As String = "SELECT * FROM CashPrices WHERE clinic_id in ((select Clinic_id from UserClinicPermissions where user_id = " & Val(Session("pruser_id")) & "))"
    '    Dim sql_str As String = "SELECT * FROM CashPrices  where 1=1 "
    '    If ddl_clinics.SelectedValue > 0 Then sql_str = sql_str & " and Clinic_id=" & ddl_clinics.SelectedValue
    '    Dim ds As New DataSet2
    '    Dim viewer As ReportViewer = New ReportViewer()

    '    Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
    '    insurance_SQLcon.Close()
    '    insurance_SQLcon.Open()
    '    ds.Tables("INC_SERVICES_PRICES").Load(sel_com.ExecuteReader)
    '    insurance_SQLcon.Close()

    '    Dim datasource As New ReportDataSource("CashPricesDataset", ds.Tables("INC_SERVICES_PRICES"))
    '    viewer.LocalReport.DataSources.Clear()
    '    viewer.ProcessingMode = ProcessingMode.Local
    '    viewer.LocalReport.ReportPath = Server.MapPath("~/reports/pricesReport.rdlc")
    '    viewer.LocalReport.DataSources.Add(datasource)

    '    'Dim rp1 As ReportParameter
    '    'Dim rp2 As ReportParameter
    '    Dim rp3 As ReportParameter

    '    'rp1 = New ReportParameter("from_dt", start_dt)
    '    'rp2 = New ReportParameter("to_dt", end_dt)
    '    rp3 = New ReportParameter("user_name", Session("pruser_full_name").ToString)

    '    viewer.LocalReport.SetParameters(New ReportParameter() {rp3})

    '    Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
    '    Dim r As String = "~/reports/pricesReport.rdlc"
    '    ' Page.Controls.Add(rv)

    '    Dim warnings As Warning() = Nothing
    '    Dim streamids As String() = Nothing
    '    Dim mimeType As String = Nothing
    '    Dim encoding As String = Nothing
    '    Dim extension As String = Nothing
    '    Dim bytes As Byte()
    '    Dim FolderLocation As String
    '    FolderLocation = Server.MapPath("~/files")
    '    Dim filepath As String = FolderLocation & "/pricesReport1" & Session("pruser_id") & ".pdf"
    '    If Directory.Exists(filepath) Then
    '        File.Delete(filepath)
    '    End If
    '    bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
    '        encoding, extension, streamids, warnings)
    '    Dim fs As New FileStream(FolderLocation & "/pricesReport1" & Session("pruser_id") & ".pdf", FileMode.Create)
    '    fs.Write(bytes, 0, bytes.Length)
    '    fs.Close()
    '    Response.Redirect("~/files/pricesReport1" & Session("pruser_id") & ".pdf", True)
    'End Sub

    'Protected Sub btn_print1_Click(sender As Object, e As EventArgs) Handles btn_print1.Click
    '    'Dim sql_str As String = "SELECT * FROM CashPrices WHERE clinic_id in ((select Clinic_id from UserClinicPermissions where user_id = " & Val(Session("pruser_id")) & "))"
    '    Dim sql_str As String = "SELECT * FROM CashPrices  where 1=1 "
    '    sql_str = sql_str & " and CASH_PRS=0"
    '    If ddl_clinics.SelectedValue > 0 Then sql_str = sql_str & " and Clinic_id=" & ddl_clinics.SelectedValue
    '    Dim ds As New DataSet2
    '    Dim viewer As ReportViewer = New ReportViewer()

    '    Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
    '    insurance_SQLcon.Close()
    '    insurance_SQLcon.Open()
    '    ds.Tables("INC_SERVICES_PRICES").Load(sel_com.ExecuteReader)
    '    insurance_SQLcon.Close()

    '    Dim datasource As New ReportDataSource("CashPricesDataset", ds.Tables("INC_SERVICES_PRICES"))
    '    viewer.LocalReport.DataSources.Clear()
    '    viewer.ProcessingMode = ProcessingMode.Local
    '    viewer.LocalReport.ReportPath = Server.MapPath("~/reports/pricesReport.rdlc")
    '    viewer.LocalReport.DataSources.Add(datasource)

    '    'Dim rp1 As ReportParameter
    '    'Dim rp2 As ReportParameter
    '    Dim rp3 As ReportParameter

    '    'rp1 = New ReportParameter("from_dt", start_dt)
    '    'rp2 = New ReportParameter("to_dt", end_dt)
    '    rp3 = New ReportParameter("user_name", Session("pruser_full_name").ToString)

    '    viewer.LocalReport.SetParameters(New ReportParameter() {rp3})

    '    Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
    '    Dim r As String = "~/reports/pricesReport.rdlc"
    '    ' Page.Controls.Add(rv)

    '    Dim warnings As Warning() = Nothing
    '    Dim streamids As String() = Nothing
    '    Dim mimeType As String = Nothing
    '    Dim encoding As String = Nothing
    '    Dim extension As String = Nothing
    '    Dim bytes As Byte()
    '    Dim FolderLocation As String
    '    FolderLocation = Server.MapPath("~/files")
    '    Dim filepath As String = FolderLocation & "/pricesReport2" & Session("pruser_id") & ".pdf"
    '    If Directory.Exists(filepath) Then
    '        File.Delete(filepath)
    '    End If
    '    bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
    '        encoding, extension, streamids, warnings)
    '    Dim fs As New FileStream(FolderLocation & "/pricesReport2" & Session("pruser_id") & ".pdf", FileMode.Create)
    '    fs.Write(bytes, 0, bytes.Length)
    '    fs.Close()
    '    Response.Redirect("~/files/pricesReport2" & Session("pruser_id") & ".pdf", True)
    'End Sub
End Class