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
            Else
                Panel1.Visible = True
                Panel2.Visible = False
                btn_create.Text = "إنشاء"
                btn_create.CssClass = "btn btn-outline-dark btn-block"
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
            txt_search.Focus()
            Me.txt_search.Attributes.Add("onkeypress", "button_click(this,'" + Me.btn_search.ClientID + "')")
        End If

    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT pros_code, INC_CompanyProcesses.C_ID, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, 
                ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME FROM INC_CompanyProcesses
                LEFT JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                LEFT JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_CompanyProcesses.PINC_ID
                WHERE Processes_State = 2 AND Processes_Residual <> 0 AND NOT EXISTS (SELECT Processes_ID FROM INC_MOTALBAT WHERE INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID)"

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

                sql_str = sql_str & " AND Processes_Cilinc = " & ddl_clinics.SelectedValue
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
            });", True)
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
                timer: 1500
            });", True)

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


                If GridView1.Rows.Count = 0 Then
                    Session("dt") = dt_result
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                Else

                    For Each dd As GridViewRow In GridView1.Rows

                        If dd.Cells(1).Text = dt_result.Rows(0)("Processes_ID") Then
                            txt_search.Text = ""
                            txt_search.Focus()
                            Exit Sub
                        End If

                    Next
                    Dim dt As New DataTable
                    dt = Session("dt")
                    dt.Merge(dt_result)
                    Session("dt") = dt
                    GridView1.DataSource = dt
                    GridView1.DataBind()
                End If

                Label1.Text = "الإجمالي: " & GridView1.Rows.Count
            End If

            txt_search.Text = ""
            txt_search.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'Try
        '    Dim sql_str As String = "SELECT pros_code, Processes_ID, Processes_Reservation_Code, INC_CompanyProcesses.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, 
        '        ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME FROM INC_CompanyProcesses
        '        LEFT JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
        '        LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
        '        LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
        '        LEFT JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_CompanyProcesses.PINC_ID
        '        WHERE Processes_State = 2 AND Processes_Residual <> 0 AND NOT EXISTS (SELECT Processes_ID FROM INC_MOTALBAT WHERE INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID)"

        '    If ddl_companies.SelectedValue <> "" Then
        '        If ddl_companies.SelectedValue <> 0 Then
        '            sql_str = sql_str & " AND INC_CompanyProcesses.C_ID = " & ddl_companies.SelectedValue
        '        End If
        '    End If


        '    If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
        '        sql_str = sql_str & " And CONVERT(VARCHAR, INC_CompanyProcesses.Processes_Date, 103) >= '" & txt_start_dt.Text & "' AND CONVERT(VARCHAR, INC_CompanyProcesses.Processes_Date, 103) <= '" & txt_end_dt.Text & "'"
        '    End If

        '    If ddl_invoice_type.SelectedValue = 1 Then
        '        sql_str = sql_str & " AND Processes_ID NOT IN (SELECT ewa_process_id FROM EWA_Processes WHERE EWA_Processes.ewa_process_id = INC_CompanyProcesses.Processes_ID)"
        '    ElseIf ddl_invoice_type.SelectedValue = 2 Then
        '        sql_str = sql_str & " AND Processes_ID IN (SELECT Ewa_Exit_ID FROM Ewa_Exit WHERE Ewa_Exit.Ewa_Exit_ID = INC_CompanyProcesses.Processes_ID)"
        '    End If

        '    If ddl_clinics.SelectedValue <> 0 Then
        '        sql_str = sql_str & " AND Processes_Cilinc = " & ddl_clinics.SelectedValue
        '    End If

        '    Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
        '    Dim dt_result As New DataTable
        '    dt_result.Rows.Clear()
        '    insurance_SQLcon.Close()
        '    insurance_SQLcon.Open()
        '    dt_result.Load(sel_com.ExecuteReader)
        '    insurance_SQLcon.Close()

        '    If dt_result.Rows.Count > 0 Then
        '        GridView1.DataSource = dt_result
        '        GridView1.DataBind()
        '        CheckBox1.Visible = True
        '        btn_create.Enabled = True

        '    Else
        '        dt_result.Rows.Clear()
        '        GridView1.DataSource = dt_result
        '        GridView1.DataBind()
        '        CheckBox1.Visible = False
        '        btn_create.Enabled = False
        '    End If
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try

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

            'If RadioButton1.Checked = True Then
            '    start_dt = ViewState("pros_dt")
            '    end_dt = ViewState("pros_dt")
            'Else
            '    start_dt = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
            '    end_dt = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
            'End If

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

    'Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
    '    For Each dd As GridViewRow In GridView1.Rows
    '        Dim ch As CheckBox = dd.FindControl("CheckBox2")

    '        If CheckBox1.Checked = True Then
    '            ch.Checked = True
    '        Else
    '            ch.Checked = False

    '        End If
    '    Next
    'End Sub

End Class