Imports System.Data.SqlClient
Imports System.Globalization

Public Class newInvoice
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            ViewState("invoice_no") = Val(Request.QueryString("invID"))

            If ViewState("invoice_no") <> 0 Then
                Panel1.Visible = False
                Panel2.Visible = True
                btn_create.Text = "حفظ"
                btn_create.CssClass = "btn btn-outline-success btn-block"
                Label2.Text = "إضافة خدمات للمطالبة رقم: " & ViewState("invoice_no")
                txt_invoice_no.Text = ViewState("invoice_no")
                Dim sel_com As New SqlCommand("SELECT INVOICE_NO, C_ID, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_INVOICES.C_ID) AS COMPANY_NAME, CONVERT(VARCHAR, DATE_FROM, 111) AS DATE_FROM, CONVERT(VARCHAR, DATE_TO, 111) AS DATE_TO FROM INC_INVOICES WHERE INVOICE_NO = " & ViewState("invoice_no"), insurance_SQLcon)
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
                    TextBox1.Text = dr_inv!DATE_FROM.ToString
                    TextBox2.Text = dr_inv!DATE_TO
                End If
            Else
                Panel1.Visible = True
                Panel2.Visible = False
                btn_create.Text = "إنشاء"
                btn_create.CssClass = "btn btn-outline-dark btn-block"
                Me.txt_search.Attributes.Add("onkeypress", "button_click(this,'" + Me.btn_search.ClientID + "')")
            End If

            ViewState("counter") = 0
            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("new_invoice_opd") = True Then
                    ddl_invoice_type.SelectedValue = 1
                    ddl_invoice_type.Enabled = False
                ElseIf Session("User_per")("new_invoice_ewa") = True Then
                    ddl_invoice_type.SelectedValue = 2
                    ddl_invoice_type.Enabled = False
                ElseIf Session("User_per")("new_invoice_ewa") = True And Session("User_per")("new_invoice_opd") = True Then
                    ddl_invoice_type.Enabled = True
                End If
            End If
            getTempData()
            txt_search.Focus()

        End If

    End Sub

    Private Sub getTempData()
        'جلب البيانات المحفوظة مؤقتاً في حال وجودها
        Dim sel_temp_data As New SqlCommand("SELECT INC_CompanyProcesses.pros_code, INC_CompanyProcesses.C_ID, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, 
                ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME FROM INC_MOTALBA_TEMP
				LEFT JOIN INC_CompanyProcesses ON INC_CompanyProcesses.pros_code = INC_MOTALBA_TEMP.Req_Code
                LEFT JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                LEFT JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_CompanyProcesses.PINC_ID
                WHERE INC_MOTALBA_TEMP.User_Id = " & Session("INC_User_Id") & " ORDER BY INC_MOTALBA_TEMP.Id DESC", insurance_SQLcon)
        Dim dt_temp_result As New DataTable
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_temp_result.Load(sel_temp_data.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_temp_result.Rows.Count > 0 Then
            ddl_companies.SelectedValue = dt_temp_result.Rows(0)("C_ID")
            GridView1.DataSource = dt_temp_result
            GridView1.DataBind()
            btn_clear.Visible = True
        Else
            btn_clear.Visible = False
        End If
    End Sub

    Private Sub getData()

        Try
            ' التحقق من الخدمة إذا تمت المطالبة بها من قبل أو لا
            Dim invoice_no As Integer = 0
            Dim sel_comm As New SqlCommand("SELECT ISNULL(INVOICE_NO, 0) AS INVOICE_NO FROM INC_IvoicesProcesses WHERE Req_Code = '" & txt_search.Text & "'", insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            invoice_no = sel_comm.ExecuteScalar
            insurance_SQLcon.Close()
            If invoice_no <> 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'خطأ! هذه الخدمة تم المطالبة بها في الفاتورة رقم " & invoice_no & "',
                            showConfirmButton: false,
                            timer: 3000
                        });playSound('../Style/error.mp3');", True)
                txt_search.Text = ""
                txt_search.Focus()
                Exit Sub
            End If

            Dim sql_str As String = "SELECT pros_code, INC_CompanyProcesses.C_ID, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, 
                ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME,Processes_State FROM INC_CompanyProcesses
                LEFT JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                LEFT JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_CompanyProcesses.PINC_ID
                WHERE NOT EXISTS (SELECT Processes_ID FROM INC_MOTALBAT WHERE INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID)"

            If ddl_clinics.SelectedValue <> 0 Then
                If ViewState("invoice_no") = 0 Then
                    If ddl_companies.SelectedValue <> 0 Then
                        sql_str = sql_str & " AND INC_CompanyProcesses.C_ID = " & ddl_companies.SelectedValue
                    End If
                Else
                    sql_str = sql_str & " AND INC_CompanyProcesses.C_ID = " & ViewState("company_no")
                End If
                If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
                    Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                    Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                    sql_str = sql_str & " And Processes_Date >= '" & start_dt & "' AND Processes_Date <= '" & end_dt & "'"
                End If

                If ddl_invoice_type.SelectedValue = 1 Then
                    sql_str = sql_str & " AND Processes_ID NOT IN (SELECT ewa_process_id FROM EWA_Processes WHERE EWA_Processes.ewa_process_id = INC_CompanyProcesses.Processes_ID)"
                ElseIf ddl_invoice_type.SelectedValue = 2 Then
                    sql_str = sql_str & " AND Processes_ID IN (SELECT Ewa_Exit_ID FROM Ewa_Exit WHERE Ewa_Exit.Ewa_Exit_ID = INC_CompanyProcesses.Processes_ID)"
                End If

                sql_str = sql_str & " AND Processes_State = 2 AND Processes_Cilinc = " & ddl_clinics.SelectedValue
            Else
                sql_str = sql_str & " AND pros_code = '" & txt_search.Text & "'"
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                Select Case dt_result.Rows(0)("Processes_State")
                    Case 0
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'خطأ! هذه الخدمة لم تتم تسويتها',
                            showConfirmButton: false,
                            timer: 2500
                        });playSound('../Style/error.mp3');", True)
                        txt_search.Text = ""
                        txt_search.Focus()
                        Exit Sub
                    Case 3
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'خطأ! هذه الخدمة ملغية',
                            showConfirmButton: false,
                            timer: 2500
                        });playSound('../Style/error.mp3');", True)
                        txt_search.Text = ""
                        txt_search.Focus()
                        Exit Sub
                End Select

                If dt_result.Rows(0)("Processes_Residual") = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'خطأ! سعر الشركة لهذه الخدمة صفر',
                            showConfirmButton: false,
                            timer: 2500
                        });playSound('../Style/error.mp3');", True)
                    txt_search.Text = ""
                    txt_search.Focus()
                    Exit Sub
                End If


                'التحقق من أن الحركة تابعة للشركة المختارة
                If ViewState("invoice_no") = 0 Then
                    If ddl_companies.SelectedValue <> 0 Then
                        If dt_result.Rows(0)("C_ID") <> ddl_companies.SelectedValue Then
                            txt_search.Text = ""
                            txt_search.Focus()
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                                position: 'center',
                                icon: 'error',
                                title: 'خطأ! هذه الخدمة غير مقدمة لهذه الشركة',
                                showConfirmButton: true
                            });playSound('../Style/error.mp3');", True)
                            Exit Sub
                        End If
                    Else
                        txt_search.Text = ""
                        txt_search.Focus()
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                            position: 'top-end',
                            icon: 'error',
                            title: 'خطأ! يجب اختيار شركة',
                            showConfirmButton: false,
                            timer: 2500
                        });
                        playSound('../Style/error.mp3');", True)

                        Exit Sub
                    End If
                Else
                    If dt_result.Rows(0)("C_ID") <> ViewState("company_no") Then
                        txt_search.Text = ""
                        txt_search.Focus()
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'خطأ! هذه الخدمة غير مقدمة لهذه الشركة',
                            showConfirmButton: true
                        });", True)
                        Exit Sub
                    End If
                End If

                If txt_search.Text <> "" Then
                    For Each dd As GridViewRow In GridView1.Rows
                        If dd.Cells(3).Text = txt_search.Text Then
                            txt_search.Text = ""
                            txt_search.Focus()
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                                position: 'center',
                                icon: 'error',
                                title: 'خطأ! هذه الخدمة تمت إضافتها سابقاً',
                                showConfirmButton: false,
                                timer: 2500
                            });
                               playSound('../Style/error.mp3');", True)
                            txt_search.Text = ""
                            txt_search.Focus()
                            Exit Sub
                        End If
                    Next

                    ' حفظ الحركة في جدول المطالبات المؤقت
                    Dim ins_com As New SqlCommand("INSERT INTO INC_MOTALBA_TEMP (Req_Code,User_Id) VALUES (@Req_Code,@User_Id)", insurance_SQLcon)
                    ins_com.Parameters.AddWithValue("Req_Code", SqlDbType.NVarChar).Value = txt_search.Text
                    ins_com.Parameters.AddWithValue("User_Id", SqlDbType.NVarChar).Value = Session("INC_User_Id")
                    insurance_SQLcon.Close()
                    insurance_SQLcon.Open()
                    ins_com.ExecuteNonQuery()
                    insurance_SQLcon.Close()

                    getTempData()
                Else
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                End If

                'If GridView1.Rows.Count = 0 Then
                '    Session("dt") = dt_result
                '    GridView1.DataSource = dt_result
                '    GridView1.DataBind()
                'Else

                '    For Each dd As GridViewRow In GridView1.Rows

                '        If dd.Cells(1).Text = dt_result.Rows(0)("Processes_ID") Then
                '            txt_search.Text = ""
                '            txt_search.Focus()
                '            Exit Sub
                '        End If

                '    Next
                '    Dim dt As New DataTable
                '    dt = Session("dt")
                '    dt.Merge(dt_result)
                '    Session("dt") = dt
                '    GridView1.DataSource = dt
                '    GridView1.DataBind()
                'End If

                Label1.Text = "الإجمالي: " & GridView1.Rows.Count
            End If

            txt_search.Text = ""
            txt_search.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()
    End Sub

    Private Sub btn_create_Click(sender As Object, e As EventArgs) Handles btn_create.Click

        'Dim ch_counter As Integer = 0

        'For Each dd As GridViewRow In GridView1.Rows
        '    Dim ch As CheckBox = dd.FindControl("CheckBox2")
        '    If ch.Checked = True Then
        '        ch_counter = ch_counter + 1
        '    End If
        'Next

        If GridView1.Rows.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('يرجى اختيار حركة واحدة على الأقل'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            Exit Sub
        End If


        Try

            If ViewState("invoice_no") = 0 Then

                Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

                Dim result As Integer = 0

                Dim invoice_id As String
                Dim sqlComm As New SqlCommand
                sqlComm.Connection = insurance_SQLcon
                sqlComm.CommandText = "INC_addNewInvoice"
                sqlComm.CommandType = CommandType.StoredProcedure
                sqlComm.Parameters.AddWithValue("@c_id", ddl_companies.SelectedValue)
                sqlComm.Parameters.AddWithValue("@from_dt", start_dt)
                sqlComm.Parameters.AddWithValue("@to_dt", end_dt)
                sqlComm.Parameters.AddWithValue("@invoiceType", ddl_invoice_type.SelectedValue)
                sqlComm.Parameters.AddWithValue("@user_id", Session("INC_User_Id"))
                sqlComm.Parameters.AddWithValue("@user_ip", GetIPAddress())
                sqlComm.Parameters.AddWithValue("@inv_id", SqlDbType.Int).Direction = ParameterDirection.Output
                insurance_SQLcon.Open()
                result = sqlComm.ExecuteNonQuery()
                invoice_id = sqlComm.Parameters("@inv_id").Value.ToString()
                sqlComm.CommandText = ""
                insurance_SQLcon.Close()

                If result > 0 Then
                    For Each dd As GridViewRow In GridView1.Rows
                        Dim ch As CheckBox = dd.FindControl("CheckBox2")

                        If ch.Checked = True Then
                            Dim ins_motalba As New SqlCommand("INSERT INTO INC_MOTALBAT (INVOICE_NO,Processes_ID,MOTALABA_STS,USER_ID,USER_IP) VALUES (@INVOICE_NO,@Processes_ID,@MOTALABA_STS,@USER_ID,@USER_IP)", insurance_SQLcon)
                            ins_motalba.Parameters.AddWithValue("@INVOICE_NO", invoice_id)
                            ins_motalba.Parameters.AddWithValue("@Processes_ID", dd.Cells(1).Text)
                            ins_motalba.Parameters.AddWithValue("@MOTALABA_STS", 1)
                            ins_motalba.Parameters.AddWithValue("@USER_ID", Session("INC_User_Id"))
                            ins_motalba.Parameters.AddWithValue("@USER_IP", GetIPAddress())
                            insurance_SQLcon.Close()
                            insurance_SQLcon.Open()
                            ins_motalba.ExecuteNonQuery()
                            insurance_SQLcon.Close()
                        End If
                    Next
                    Response.Redirect("invoiceContent.aspx?invID=" & invoice_id, False)
                End If
            Else
                Dim del_temp As New SqlCommand("DELETE FROM INC_MOTALBA_TEMP WHERE [User_Id] = @user_id", insurance_SQLcon)
                del_temp.Parameters.AddWithValue("@user_id", Session("INC_User_Id"))
                If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
                del_temp.ExecuteNonQuery()
                insurance_SQLcon.Close()

                For Each dd As GridViewRow In GridView1.Rows
                    Dim ch As CheckBox = dd.FindControl("CheckBox2")

                    If ch.Checked = True Then
                        Dim ins_motalba As New SqlCommand("INSERT INTO INC_MOTALBAT (INVOICE_NO,Processes_ID,MOTALABA_STS,USER_ID,USER_IP) VALUES (@INVOICE_NO,@Processes_ID,@MOTALABA_STS,@USER_ID,@USER_IP)", insurance_SQLcon)
                        ins_motalba.Parameters.AddWithValue("@INVOICE_NO", ViewState("invoice_no"))
                        ins_motalba.Parameters.AddWithValue("@Processes_ID", dd.Cells(1).Text)
                        ins_motalba.Parameters.AddWithValue("@MOTALABA_STS", 1)
                        ins_motalba.Parameters.AddWithValue("@USER_ID", Session("INC_User_Id"))
                        ins_motalba.Parameters.AddWithValue("@USER_IP", GetIPAddress())
                        insurance_SQLcon.Close()
                        insurance_SQLcon.Open()
                        ins_motalba.ExecuteNonQuery()
                        insurance_SQLcon.Close()
                    End If
                Next
                Response.Redirect("invoiceContent.aspx?invID=" & ViewState("invoice_no"), False)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click
        Dim del_com As New SqlCommand("DELETE FROM INC_MOTALBA_TEMP WHERE User_Id = " & Session("INC_User_Id"), insurance_SQLcon)
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        del_com.ExecuteNonQuery()
        insurance_SQLcon.Close()
        Dim dt_clear As New DataTable
        dt_clear.Rows.Clear()
        GridView1.DataSource = dt_clear
        GridView1.DataBind()
    End Sub
End Class