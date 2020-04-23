Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.IO

Public Class newApproval
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        Try
            ' جلب بيانات دفع الشركة 
            Dim sel_company_info As New SqlCommand("SELECT TOP(1) PYMENT_TYPE, PROFILE_PRICE_ID FROM INC_COMPANY_DETIAL WHERE C_ID = " & ddl_companies.SelectedValue & " ORDER BY N DESC", insurance_SQLcon)
            Dim dt_comp_info As New DataTable
            dt_comp_info.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_comp_info.Load(sel_company_info.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_comp_info.Rows.Count > 0 Then
                Dim dr_pay_info = dt_comp_info.Rows(0)
                ViewState("payment_type") = dr_pay_info!PYMENT_TYPE
                ViewState("profile_no") = dr_pay_info!PROFILE_PRICE_ID
            End If

            ' جلب بيانات المنتفعين بحسب رقم البطاقة أو الرقم الوظيفي أو الأسم
            Dim sel_com As New SqlCommand("SELECT PINC_ID, NAME_ARB, NAME_ENG, CARD_NO, BAGE_NO, CONST_ID FROM INC_PATIANT WHERE C_ID =" & ddl_companies.SelectedValue & " AND NAME_ARB LIKE '%" & txt_search_box.Text & "%' OR CARD_NO LIKE '" & txt_search_box.Text & "' OR BAGE_NO LIKE '" & txt_search_box.Text & "'", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                Panel1.Visible = True
                Panel2.Visible = True
                source_list.DataSource = dt_result
                source_list.DataValueField = "PINC_ID"
                source_list.DataTextField = "NAME_ARB"
                source_list.DataBind()
                For i = 0 To dt_result.Rows.Count - 1
                    Dim dr_pet = dt_result.Rows(i)
                    If dr_pet!CONST_ID = 0 Then
                        Label1.Text = "اسم المشترك: " & dr_pet!NAME_ARB
                        Exit For
                    End If
                Next
            Else
                Panel1.Visible = False
                Panel2.Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub source_list_SelectedIndexChanged(sender As Object, e As EventArgs) Handles source_list.SelectedIndexChanged
        'Label1.Text = source_list.SelectedValue

        Dim sel_com As New SqlCommand("SELECT PINC_ID, NAME_ARB, NAME_ENG, CARD_NO, BAGE_NO, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID, P_STATE FROM INC_PATIANT WHERE PINC_ID =" & source_list.SelectedValue, insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_result.Rows.Count > 0 Then
            Dim dr_result = dt_result.Rows(0)

            If dr_result!P_STATE = 0 Then
                Label2.Text = "الحالة: مفعل"
                Label2.CssClass = "text-success"
            Else
                Label2.Text = "الحالة: موقوف"
                Label2.CssClass = "text-danger"
            End If
            Label3.Text = "للمريض: " & source_list.SelectedItem.Text
            Label4.Text = "الصلة بالمشترك: " & dr_result!CONST_ID
            Label5.Text = "رقم البطاقة: " & dr_result!CARD_NO
            Label6.Text = "الرقم الوظيفي: " & dr_result!BAGE_NO
        End If
        bindGridview()
    End Sub

    Private Sub ddl_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_type.SelectedIndexChanged

        If ddl_type.SelectedValue = 1 Then
            ddl_clinics.Enabled = True
            ddl_services.Enabled = True
            ddl_sub_service.Enabled = True
            btn_chose.Enabled = True
        ElseIf ddl_type.SelectedValue = 2 Then
            btn_chose.Enabled = True
            ddl_clinics.Enabled = False
            ddl_services.Enabled = False
            ddl_sub_service.Enabled = True

            Dim sel_com As New SqlCommand("SELECT Ewa_Add_id, Ewa_Add_name FROM Ewa_Additionals WHERE Ewa_Add_Type = 1", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                ddl_sub_service.DataSource = dt_result
                ddl_sub_service.DataValueField = "Ewa_Add_id"
                ddl_sub_service.DataTextField = "Ewa_Add_name"
                ddl_sub_service.DataBind()
            End If
        Else
            ddl_clinics.Enabled = False
            ddl_services.Enabled = False
            ddl_sub_service.Enabled = False
            btn_chose.Enabled = False
        End If

    End Sub

    Private Sub ddl_services_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services.SelectedIndexChanged
        If ddl_type.SelectedValue = 1 Then
            Dim sel_com As New SqlCommand("SELECT SubService_ID, SubService_AR_Name FROM Main_SubServices WHERE SubService_Service_ID = " & ddl_services.SelectedValue, insurance_SQLcon)
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
        Else

        End If
    End Sub

    Private Sub btn_chose_Click(sender As Object, e As EventArgs) Handles btn_chose.Click

        If ddl_sub_service.SelectedValue = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('يرجى اختيار خدمة واحدة على الأقل'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            Exit Sub
        End If

        If source_list.SelectedValue = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('خطأ! يرجى اختيار منتفع'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            Exit Sub
        End If

        Try
            Dim services_dt As New DataTable
            Dim dr As DataRow = Nothing
            services_dt.Columns.Add("SubService_ID", GetType(Integer))
            services_dt.Columns.Add("SubService_Code", GetType(String))
            services_dt.Columns.Add("SubService_AR_Name", GetType(String))
            services_dt.Columns.Add("SubService_Price", GetType(Decimal))

            Dim pay_type As String = ""
            If ViewState("payment_type") = 1 Then
                pay_type = "CASH_PRS"
            Else
                pay_type = "INS_PRS"
            End If

            If ddl_type.SelectedValue = 1 Then
                Dim sel_service As New SqlCommand("SELECT SubService_ID, SubService_Code, SubService_AR_Name, (SELECT " & pay_type & " FROM INC_SERVICES_PRICES WHERE INC_SERVICES_PRICES.SER_ID = Main_SubServices.SubService_Service_ID AND INC_SERVICES_PRICES.PROFILE_PRICE_ID = " & ViewState("profile_no") & ") AS SubService_Price FROM Main_SubServices WHERE SubService_ID = " & ddl_sub_service.SelectedValue, insurance_SQLcon)
                Dim dt_service_info As New DataTable
                dt_service_info.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_service_info.Load(sel_service.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_service_info.Rows.Count > 0 Then
                    Dim dr_ser_info = dt_service_info.Rows(0)

                    Dim insToConfirm As New SqlCommand
                    insToConfirm.Connection = insurance_SQLcon
                    insToConfirm.CommandText = "INC_EWA_addNewConfirm"
                    insToConfirm.CommandType = CommandType.StoredProcedure
                    insToConfirm.Parameters.AddWithValue("@service_id", ddl_sub_service.SelectedValue)
                    insToConfirm.Parameters.AddWithValue("@patient_id", source_list.SelectedValue)
                    insToConfirm.Parameters.AddWithValue("@service_price", dr_ser_info!SubService_Price)
                    insToConfirm.Parameters.AddWithValue("@iec_state", 0)
                    insToConfirm.Parameters.AddWithValue("@ewa_no", 0)
                    insToConfirm.Parameters.AddWithValue("@ins_date", Date.Now.Date)
                    insToConfirm.Parameters.AddWithValue("@req_no", 0)
                    insToConfirm.Parameters.AddWithValue("@iec_type", ddl_type.SelectedValue)
                    insToConfirm.Parameters.AddWithValue("@userId", 1)
                    insurance_SQLcon.Open()
                    insToConfirm.ExecuteNonQuery()
                    insurance_SQLcon.Close()

                    bindGridview()
                End If
            ElseIf ddl_type.SelectedValue = 2 Then
                Dim sel_service As New SqlCommand("SELECT Ewa_Add_id, SubService_Code, Ewa_Add_name, Ewa_Add_Price AS SubService_Price FROM Ewa_Additionals WHERE Ewa_Add_id = " & ddl_sub_service.SelectedValue, insurance_SQLcon)
                Dim dt_service_info As New DataTable
                dt_service_info.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_service_info.Load(sel_service.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_service_info.Rows.Count > 0 Then
                    Dim dr_ser_info = dt_service_info.Rows(0)

                    Dim insToConfirm As New SqlCommand
                    insToConfirm.Connection = insurance_SQLcon
                    insToConfirm.CommandText = "INC_EWA_addNewConfirm"
                    insToConfirm.CommandType = CommandType.StoredProcedure
                    insToConfirm.Parameters.AddWithValue("@service_id", ddl_sub_service.SelectedValue)
                    insToConfirm.Parameters.AddWithValue("@patient_id", source_list.SelectedValue)
                    insToConfirm.Parameters.AddWithValue("@service_price", dr_ser_info!SubService_Price)
                    insToConfirm.Parameters.AddWithValue("@iec_state", 0)
                    insToConfirm.Parameters.AddWithValue("@ewa_no", 0)
                    insToConfirm.Parameters.AddWithValue("@ins_date", Date.Now.Date)
                    insToConfirm.Parameters.AddWithValue("@req_no", 0)
                    insToConfirm.Parameters.AddWithValue("@iec_type", ddl_type.SelectedValue)
                    insToConfirm.Parameters.AddWithValue("@userId", 1)
                    insurance_SQLcon.Open()
                    insToConfirm.ExecuteNonQuery()
                    insurance_SQLcon.Close()

                    bindGridview()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub bindGridview()
        Try
            If source_list.SelectedValue <> 0 Then
                Dim sel_com As New SqlCommand("SELECT IEC_SID AS SubService_ID, IEC_Price AS SubService_Price, (CASE WHEN (IEC_Type = 1) THEN 'خدمة/عملية' ELSE 'إضافة على عملية' END) AS IEC_Type, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE '/' END) AS SubService_Code, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE (SELECT Ewa_Add_name FROM Ewa_Additionals WHERE Ewa_Additionals.Ewa_Add_id = INC_EWA_Confirm.IEC_SID) END) AS SubService_AR_Name FROM INC_EWA_Confirm WHERE IEC_PID = " & source_list.SelectedValue & " AND IEC_State = 0 AND IEC_RNo = 0", insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_result.Rows.Count > 0 Then
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                Else
                    dt_result.Rows.Clear()
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "delete_ser") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim del_com As New SqlCommand("DELETE FROM INC_EWA_Confirm WHERE IEC_State = 0 AND IEC_RNo = 0 AND IEC_SID = " & (row.Cells(1).Text) & " AND IEC_PID = " & source_list.SelectedValue, insurance_SQLcon)
            insurance_SQLcon.Open()
            del_com.ExecuteNonQuery()
            insurance_SQLcon.Close()
            bindGridview()
        End If
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Try

            If GridView1.Rows.Count = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('يرجى اختيار خدمة واحدة على الأقل'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                Exit Sub
            End If

            If source_list.SelectedValue = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('خطأ! يرجى اختيار منتفع'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                Exit Sub
            End If

            Using main_ds
                Dim sel_com As New SqlCommand("SELECT IEC_SID AS SubService_ID, IEC_Price AS SubService_Price, (CASE WHEN (IEC_Type = 1) THEN 'خدمة/عملية' ELSE 'إضافة على عملية' END) AS IEC_Type, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE '/' END) AS SubService_Code, (CASE WHEN (IEC_Type = 1) THEN (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_EWA_Confirm.IEC_SID) ELSE (SELECT Ewa_Add_name FROM Ewa_Additionals WHERE Ewa_Additionals.Ewa_Add_id = INC_EWA_Confirm.IEC_SID) END) AS SubService_AR_Name FROM INC_EWA_Confirm WHERE IEC_PID = " & source_list.SelectedValue & " AND IEC_State = 0 AND IEC_RNo = 0")
                Using sda As New SqlDataAdapter()
                    sel_com.Connection = insurance_SQLcon
                    sda.SelectCommand = sel_com
                    sda.Fill(main_ds, "INC_EWA_Confirm")
                End Using
            End Using

            Dim viewer As ReportViewer = New ReportViewer()

            Dim datasource As New ReportDataSource("confirmDS", main_ds.Tables("INC_EWA_Confirm"))
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

            rp1 = New ReportParameter("company_name", ddl_companies.SelectedItem.Text)
            rp2 = New ReportParameter("pat_name", Label3.Text)
            rp3 = New ReportParameter("card_no", Label5.Text)
            rp4 = New ReportParameter("emp_no", Label6.Text)
            rp5 = New ReportParameter("moshtark_name", Label1.Text)
            rp6 = New ReportParameter("relation_m", Label4.Text)

            viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4, rp5, rp6})

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
            Dim filepath As String = FolderLocation & "/confirmApproval.pdf"
            If Directory.Exists(filepath) Then
                File.Delete(filepath)
            End If
            bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
                encoding, extension, streamids, warnings)
            Dim fs As New FileStream(FolderLocation & "/confirmApproval.pdf", FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
            fs.Close()
            Response.Redirect("~/Reports/confirmApproval.pdf", False)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class