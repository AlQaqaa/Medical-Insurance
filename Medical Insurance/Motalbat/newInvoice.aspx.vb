Imports System.Data.SqlClient
Imports System.Globalization

Public Class newInvoice
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.txt_search.Attributes.Add("onkeypress", "button_click(this,'" + Me.btn_search.ClientID + "')")

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
                    TextBox1.Text = dr_inv!DATE_FROM
                    TextBox2.Text = dr_inv!DATE_TO
                End If

            Else
                Panel1.Visible = True
                Panel2.Visible = False
                btn_create.Text = "إنشاء"
                btn_create.CssClass = "btn btn-outline-dark btn-block"

            End If

            ViewState("counter") = 0
            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("new_invoice_opd") = True And Session("User_per")("new_invoice_ewa") = False Then
                    ddl_invoice_type.SelectedValue = 1
                    ddl_invoice_type.Enabled = False
                ElseIf Session("User_per")("new_invoice_ewa") = True And Session("User_per")("new_invoice_opd") = False Then
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
        Dim ss As String

        If ddl_invoice_type.SelectedValue = 1 Then
            ss = "SELECT INC_CompanyProcesses.pros_code, INC_CompanyProcesses.C_ID, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, 
                ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(INC_CompanyProcesses.NAME_ARB, '') AS PATIENT_NAME FROM INC_MOTALBA_TEMP
				INNER JOIN INC_CompanyProcesses ON INC_CompanyProcesses.pros_code = INC_MOTALBA_TEMP.Req_Code
                INNER JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                WHERE INC_MOTALBA_TEMP.User_Id = " & Session("INC_User_Id")
        Else
            ss = "SELECT INC_CompanyProcesses.pros_code, INC_CompanyProcesses.C_ID, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, 
                ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(INC_CompanyProcesses.NAME_ARB, '') AS PATIENT_NAME FROM INC_MOTALBA_TEMP
				INNER JOIN INC_CompanyProcesses ON INC_CompanyProcesses.pros_code = INC_MOTALBA_TEMP.Req_Code
                INNER JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                INNER JOIN EWA_Processes ON EWA_Processes.ewa_process_id = INC_MOTALBA_TEMP.Processes_ID
                INNER JOIN Ewa_Record ON Ewa_Record.EWA_Record_ID = EWA_Processes.ewa_patient_id
                WHERE INC_MOTALBA_TEMP.User_Id = " & Session("INC_User_Id")
        End If

        If DropDownList1.SelectedValue = 0 Then ss += " ORDER BY INC_MOTALBA_TEMP.Id DESC"
        If DropDownList1.SelectedValue = 1 Then ss += " ORDER BY Processes_Reservation_Code DESC"

        Dim sel_temp_data As New SqlCommand(ss, insurance_SQLcon)
        Dim dt_temp_result As New DataTable
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_temp_result.Load(sel_temp_data.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_temp_result.Rows.Count > 0 Then
            ddl_companies.SelectedValue = dt_temp_result.Rows(0)("C_ID")
            GridView1.DataSource = dt_temp_result
            GridView1.DataBind()
            btn_zclear.Enabled = True
            btn_search.Enabled = True
            txt_search.Focus()
        Else
            btn_zclear.Enabled = False
            btn_search.Enabled = True
        End If
    End Sub

    Private Sub getData()

        Try
            If ViewState("invoice_no") = 0 Then
                If txt_start_dt.Text = "" And txt_end_dt.Text = "" Then

                    txt_search.Text = ""
                    txt_search.Focus()
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                                position: 'center',
                                icon: 'error',
                                title: 'خطأ! يرجى إدخال التاريخ',
                                showConfirmButton: false,
                                timer: 2500
                            });
                               playSound('../Style/error.mp3');", True)
                    Exit Sub
                End If
            End If

            ' التحقق من الخدمة إذا تمت المطالبة بها من قبل أو لا
            Dim invoice_no As Integer = 0
            'If ddl_invoice_type.SelectedValue = 1 Then
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
            'End If

            'If txt_search.Text <> "" Then
            '    For Each dd As GridViewRow In GridView1.Rows
            '        If dd.Cells(3).Text = txt_search.Text Then
            '            txt_search.Text = ""
            '            txt_search.Focus()
            '            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
            '                    position: 'center',
            '                    icon: 'error',
            '                    title: 'خطأ! هذه الخدمة تمت إضافتها سابقاً',
            '                    showConfirmButton: false,
            '                    timer: 2500
            '                });
            '                   playSound('../Style/error.mp3');", True)
            '            Exit Sub
            '        End If
            '    Next
            'End If

            Dim sql_str As String

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = "SELECT pros_code, INC_CompanyProcesses.C_ID, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, Processes_Cilinc, 
                ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(INC_CompanyProcesses.NAME_ARB, '') AS PATIENT_NAME,Processes_State FROM INC_CompanyProcesses
                INNER JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                WHERE NOT EXISTS (SELECT Processes_ID FROM INC_MOTALBAT WHERE INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID)"
            Else
                sql_str = "SELECT pros_code, EWA_Code, INC_CompanyProcesses.C_ID, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, Processes_Cilinc, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(INC_CompanyProcesses.NAME_ARB, '') AS PATIENT_NAME,INC_CompanyProcesses.Processes_State FROM INC_CompanyProcesses
INNER JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
INNER JOIN EWA_Processes ON EWA_Processes.ewa_process_id = INC_CompanyProcesses.Processes_ID
INNER JOIN Ewa_Record ON Ewa_Record.EWA_Record_ID = EWA_Processes.ewa_patient_id
WHERE NOT EXISTS (SELECT Processes_ID FROM INC_MOTALBAT WHERE INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID) AND EWA_Record_ID =" & txt_search.Text & "
 and (EWA_Record_ID IN (SELECT Ewa_Exit_ID FROM dbo.Ewa_Exit where EWA_Record_ID =" & txt_search.Text & " )) and INC_CompanyProcesses.Processes_State <> 3"
            End If

            If ViewState("invoice_no") = 0 Then
                Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                sql_str += " And Processes_Date BETWEEN '" & start_dt & "' AND '" & end_dt & "'"
            Else
                Dim start_dt As String = DateTime.ParseExact(TextBox1.Text, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                Dim end_dt As String = DateTime.ParseExact(TextBox2.Text, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                sql_str += " And Processes_Date BETWEEN '" & start_dt & "' AND '" & end_dt & "'"
            End If



            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str += " AND Processes_ID NOT IN (SELECT ewa_process_id FROM EWA_Processes WHERE EWA_Processes.ewa_process_id = INC_CompanyProcesses.Processes_ID)"
            End If

            If ddl_clinics.SelectedValue <> 0 Then
                If ViewState("invoice_no") = 0 Then
                    If ddl_companies.SelectedValue <> 0 Then
                        sql_str += " AND INC_CompanyProcesses.C_ID = " & ddl_companies.SelectedValue
                    End If
                Else
                    sql_str += " AND INC_CompanyProcesses.C_ID = " & ViewState("company_no")
                End If

                sql_str += " AND Processes_Residual <> 0 AND Processes_Cilinc = " & ddl_clinics.SelectedValue
                If Val(ddl_service.SelectedValue) <> 0 Then
                    sql_str += " AND Processes_Residual <> 0 AND Processes_Services = " & ddl_service.SelectedValue
                End If
            Else
                If ddl_invoice_type.SelectedValue = 1 Then sql_str += " AND pros_code = '" & txt_search.Text & "'"
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                If ddl_invoice_type.SelectedValue <> 1 Then
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                    Exit Sub
                End If

                If ddl_clinics.SelectedValue = 0 Then
                    'If dt_result.Rows(0)("Processes_State") = 0 And dt_result.Rows(0)("Processes_Cilinc") <> 1 Then
                    '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                    '        position: 'center',
                    '        icon: 'error',
                    '        title: 'خطأ! هذه الخدمة لم تتم تسويتها',
                    '        showConfirmButton: false,
                    '        timer: 2500
                    '    });playSound('../Style/error.mp3');", True)
                    '    txt_search.Text = ""
                    '    txt_search.Focus()
                    '    Exit Sub
                    'Else
                    If dt_result.Rows(0)("Processes_State") = 3 Then
                        Dim sel_del As New SqlCommand("select Orginal_UserName  from HAG_Return as x  inner join HAG_Return_Process as y  on x.Return_ID =y.Process_Return_ID 
inner join User_Table as z on z.user_id =x.Return_User  and y.Return_Process_ID in (select top 1 HAG_Request.Req_PID  from HAG_Request  where req_code ='" & txt_search.Text & "')", insurance_SQLcon)
                        Dim dt_user_name As New DataTable
                        If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
                        insurance_SQLcon.Open()
                        dt_user_name.Load(sel_del.ExecuteReader)
                        insurance_SQLcon.Close()

                        If dt_user_name.Rows.Count > 0 Then
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                                    position: 'center',
                                    icon: 'error',
                                    title: 'خطأ! هذه الخدمة تم إلغائها من قبل المستخدم: " & dt_user_name.Rows(0)("Orginal_UserName") & "',
                                    showConfirmButton: false,
                                    timer: 2500
                                });playSound('../Style/error.mp3');", True)
                        Else
                            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                                    position: 'center',
                                    icon: 'error',
                                    title: 'خطأ! هذه الخدمة تم إلغائها من قبل المستخدم: لا يمكن عرض اسم المستخدم',
                                    showConfirmButton: false,
                                    timer: 2500
                                });playSound('../Style/error.mp3');", True)
                        End If
                        txt_search.Text = ""
                        txt_search.Focus()
                        Exit Sub
                    End If


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

                    ' حفظ الحركة في جدول المطالبات المؤقت
                    Dim sel_tem As New SqlCommand("select * from INC_MOTALBA_TEMP where Req_Code=@Req_Code", insurance_SQLcon)
                    sel_tem.Parameters.AddWithValue("Req_Code", SqlDbType.NVarChar).Value = txt_search.Text
                    If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
                    insurance_SQLcon.Open()
                    Dim dt_tem As New DataTable
                    dt_tem.Load(sel_tem.ExecuteReader)
                    insurance_SQLcon.Close()

                    If dt_tem.Rows.Count = 0 Then
                        Dim ins_com As New SqlCommand("INSERT INTO INC_MOTALBA_TEMP (Req_Code,User_Id) VALUES (@Req_Code,@User_Id)", insurance_SQLcon)
                        ins_com.Parameters.AddWithValue("Req_Code", SqlDbType.NVarChar).Value = txt_search.Text
                        ins_com.Parameters.AddWithValue("User_Id", SqlDbType.NVarChar).Value = Session("INC_User_Id")
                        insurance_SQLcon.Close()
                        insurance_SQLcon.Open()
                        ins_com.ExecuteNonQuery()
                        insurance_SQLcon.Close()
                    Else
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                                position: 'center',
                                icon: 'error',
                                title: 'خطأ! هذه الخدمة تمت إضافتها سابقاً',
                                showConfirmButton: false,
                                timer: 2500
                            });
                               playSound('../Style/error.mp3');", True)
                        Exit Sub
                    End If

                    getTempData()
                    Label1.Text = "الإجمالي: " & GridView1.Rows.Count
                Else
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                End If
            Else
                txt_search.Text = ""
                txt_search.Focus()
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                     position: 'center',
                     icon: 'error',
                     title: 'خطأ! يرجى التأكد من تاريخ الحركة',
                     showConfirmButton: false,
                     timer: 2500
                     });
                     playSound('../Style/error.mp3');", True)
                Exit Sub
            End If

            txt_search.Text = ""
            txt_search.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

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

    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()

        Me.txt_search.Attributes.Remove("onkeypress")
    End Sub

    Private Sub btn_create_Click(sender As Object, e As EventArgs) Handles btn_create.Click

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

            Me.txt_search.Attributes.Remove("onkeypress")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_zclear_Click(sender As Object, e As EventArgs) Handles btn_zclear.Click
        Dim del_com As New SqlCommand("DELETE FROM INC_MOTALBA_TEMP WHERE User_Id = " & Session("INC_User_Id"), insurance_SQLcon)
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        del_com.ExecuteNonQuery()
        insurance_SQLcon.Close()
        Dim dt_clear As New DataTable
        dt_clear.Rows.Clear()
        GridView1.DataSource = dt_clear
        GridView1.DataBind()
        Label1.Text = ""
        Me.txt_search.Attributes.Remove("onkeypress")
    End Sub

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        Dim del_com As New SqlCommand("DELETE FROM INC_MOTALBA_TEMP WHERE User_Id = " & Session("INC_User_Id"), insurance_SQLcon)
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        del_com.ExecuteNonQuery()
        insurance_SQLcon.Close()
        Dim dt_clear As New DataTable
        dt_clear.Rows.Clear()
        GridView1.DataSource = dt_clear
        GridView1.DataBind()
        Me.txt_search.Attributes.Remove("onkeypress")
    End Sub

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        Dim sel_com As New SqlCommand("SELECT 0 AS Service_ID, '- تحديد قسم -' AS Service_AR_Name FROM Main_Services UNION SELECT Service_ID,Service_AR_Name FROM Main_Services WHERE Service_State=0 AND Service_Clinic=" & ddl_clinics.SelectedValue, insurance_SQLcon)
        Dim dt_result As New DataTable
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_result.Rows.Count > 0 Then
            ddl_service.DataSource = dt_result
            ddl_service.DataValueField = "Service_ID"
            ddl_service.DataTextField = "Service_AR_Name"
            ddl_service.DataBind()
        End If
        Me.txt_search.Attributes.Remove("onkeypress")
    End Sub
End Class