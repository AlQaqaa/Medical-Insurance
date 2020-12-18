Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting
Imports System.IO
Imports ClosedXML.Excel
Imports System.Globalization

Public Class statistics
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim dt_search_result As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("search") = False Then
                    Response.Redirect("Default.aspx", True)
                    Exit Sub
                End If
            End If
            Me.txt_processes_code.Attributes.Add("onkeypress", "button_click(this,'" + Me.btn_search.ClientID + "')")

        End If

    End Sub


    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click

        If ddl_search_field.SelectedValue <> 0 And txt_search_val.Text = "" Then
            Label1.Text = "مطلوب"
            Exit Sub

        End If
        Label1.Text = ""

        getData()
        If txt_processes_code.Text = "" Then
            bindChartsClinic()
            lbl_total.Text = Format(getTotalValue(), "0,0.000") & " د.ل"
            lbl_patient_count.Text = patientCount()
        End If


        If ddl_companies.SelectedValue <> 0 Then
            lbl_header_chart.Text = ddl_companies.SelectedItem.Text
        Else
            lbl_header_chart.Text = ""
        End If


    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT INC_CompanyProcesses.pros_code, Processes_Reservation_Code, ISNULL(C_Name_Arb, '') AS COMPANY_NAME, INC_PATIANT.PINC_ID, INC_PATIANT.CARD_NO,convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (Clinic_AR_Name) AS Processes_Cilinc, (SubService_AR_Name) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME, ISNULL(INVOICE_NO, 0) AS INVOICE_NO FROM INC_CompanyProcesses 
                LEFT JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_CompanyProcesses.C_ID
                LEFT JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                LEFT JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_CompanyProcesses.Processes_Reservation_Code
                LEFT JOIN INC_MOTALBAT ON INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID AND MOTALABA_STS = 1
                WHERE Processes_State = 2"

            If txt_processes_code.Text <> "" Then
                sql_str = "SELECT INC_CompanyProcesses.pros_code, Processes_Reservation_Code, ISNULL(C_Name_Arb, '') AS COMPANY_NAME, INC_PATIANT.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (Clinic_AR_Name) AS Processes_Cilinc, (SubService_AR_Name) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME, ISNULL(INVOICE_NO, 0) AS INVOICE_NO, pros_code FROM INC_CompanyProcesses 
                LEFT JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_CompanyProcesses.C_ID
                LEFT JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                LEFT JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                LEFT JOIN INC_PATIANT ON INC_PATIANT.INC_Patient_Code = INC_CompanyProcesses.Processes_Reservation_Code
                LEFT JOIN INC_MOTALBAT ON INC_MOTALBAT.Processes_ID = INC_CompanyProcesses.Processes_ID AND MOTALABA_STS = 1 
                WHERE pros_code = " & txt_processes_code.Text & " AND Processes_State = 2"
            End If

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE CARD_NO = '" & txt_card_no.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
            End If

            If ddl_clinics.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Cilinc = " & ddl_clinics.SelectedValue
            End If

            If ddl_services.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Services = " & ddl_services.SelectedValue
            End If

            If ddl_sub_service.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_SubServices = " & ddl_sub_service.SelectedValue
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
                Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                sql_str = sql_str & " And Processes_Date >= '" & start_dt & "' AND Processes_Date <= '" & end_dt & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If ddl_motalba.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            ElseIf ddl_motalba.SelectedValue = 2 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID NOT IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1) AND Processes_Cilinc <> 43"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.PINC_ID IN (SELECT PINC_ID FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            'If CheckBox2.Checked = True Then
            '    sql_str = sql_str & " AND Processes_State = 4"
            'Else
            '    sql_str = sql_str & " AND Processes_State in (2,4)"
            'End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            'Dim dt_result As New DataTable
            'dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_search_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_search_result.Rows.Count > 0 Then
                btn_export_excel.Enabled = True
                If txt_processes_code.Text = "" Then
                    Panel1.Visible = True
                End If

                Panel2.Visible = True
                lbl_services_count.Text = dt_search_result.Rows.Count
                GridView1.DataSource = dt_search_result
                GridView1.DataBind()
            Else
                btn_export_excel.Enabled = False
                Panel1.Visible = False
                Panel2.Visible = False
                lbl_services_count.Text = "0"
                dt_search_result.Rows.Clear()
                GridView1.DataSource = dt_search_result
                GridView1.DataBind()
            End If
        Catch ex As Exception
            MsgBox("1" & ex.Message)
        End Try

    End Sub

    Function patientCount() As Integer

        Dim count_val As Integer = 0

        Try
            Dim sql_str As String = "SELECT INC_CompanyProcesses.Processes_Reservation_Code FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,8 , 1 ) <> 0 AND Processes_State = 2"

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE CARD_NO = '" & txt_card_no.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
            End If

            If ddl_clinics.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Cilinc = " & ddl_clinics.SelectedValue
            End If

            If ddl_services.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Services = " & ddl_services.SelectedValue
            End If

            If ddl_sub_service.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_SubServices = " & ddl_sub_service.SelectedValue
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
                Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                sql_str = sql_str & " And Processes_Date >= '" & start_dt & "' AND Processes_Date <= '" & end_dt & "'"
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text = "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_start_dt.Text & "'"
            End If

            If txt_start_dt.Text = "" And txt_end_dt.Text <> "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_end_dt.Text & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If ddl_motalba.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            ElseIf ddl_motalba.SelectedValue = 2 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID NOT IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1) AND Processes_Cilinc <> 43"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            'If CheckBox2.Checked = True Then
            '    sql_str = sql_str & " AND Processes_State = 4"
            'Else
            '    sql_str = sql_str & " AND Processes_State in (2,4)"
            'End If

            sql_str = sql_str & " GROUP BY INC_CompanyProcesses.Processes_Reservation_Code"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            count_val = dt_result.Rows.Count

            Return count_val

        Catch ex As Exception
            MsgBox("2" & ex.Message)
        End Try
    End Function

    Sub bindChartsClinic()
        Try
            Dim sql_str As String = "SELECT (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_CompanyProcesses.Processes_Cilinc) AS CLINIC_NAME, COUNT(*) AS CLINIC_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,8 , 1 ) <> 0 AND Processes_State = 2"

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE CARD_NO = '" & txt_card_no.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
            End If

            If ddl_clinics.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Cilinc = " & ddl_clinics.SelectedValue
            End If

            If ddl_services.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Services = " & ddl_services.SelectedValue
            End If

            If ddl_sub_service.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_SubServices = " & ddl_sub_service.SelectedValue
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
                Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                sql_str = sql_str & " And Processes_Date >= '" & start_dt & "' AND Processes_Date <= '" & end_dt & "'"
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text = "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_start_dt.Text & "'"
            End If

            If txt_start_dt.Text = "" And txt_end_dt.Text <> "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_end_dt.Text & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If ddl_motalba.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            ElseIf ddl_motalba.SelectedValue = 2 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID NOT IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1) AND Processes_Cilinc <> 43"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            'If CheckBox2.Checked = True Then
            '    sql_str = sql_str & " AND Processes_State = 4"
            'Else
            '    sql_str = sql_str & " AND Processes_State in (2,4)"
            'End If

            sql_str = sql_str & " GROUP BY Processes_Cilinc"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then

                With Me.Chart1
                    .Legends.Clear()
                    .Series.Clear()
                    .ChartAreas.Clear()
                End With

                Dim areas1 As ChartArea = Me.Chart1.ChartAreas.Add("Areas1")

                With areas1
                End With

                Dim series1 As Series = Me.Chart1.Series.Add("Series1")

                Dim x As String() = New String(dt_result.Rows.Count - 1) {}
                Dim y As Integer() = New Integer(dt_result.Rows.Count - 1) {}

                With series1
                    .ChartArea = areas1.Name
                    .ChartType = SeriesChartType.Pie
                    .CustomProperties = "PieLineColor=Black, PieLabelStyle=Outside"
                    For i = 0 To dt_result.Rows.Count - 1
                        x(i) = dt_result.Rows(i)(0).ToString()
                        y(i) = Convert.ToInt32(dt_result.Rows(i)(1))
                        Dim dr = dt_result.Rows(i)
                        .Points.AddXY(dr!CLINIC_NAME, dr!CLINIC_COUNT)
                        .ToolTip = "#VALX" & vbLf & "الخدمات المقدمة: #VALY" & vbLf & "النسبة: #PERCENT"
                    Next
                    Chart1.Series(0).Points.DataBindXY(x, y)
                    Chart1.Series(0).Label = "#PERCENT"
                    Chart1.Series(0).LegendText = "#AXISLABEL"
                End With


                Dim legends1 As Legend = Me.Chart1.Legends.Add("Legends1")
                legends1.IsTextAutoFit = True
                legends1.LegendStyle = LegendStyle.Table

            End If
        Catch ex As Exception
            MsgBox("3" & ex.Message)
        End Try
    End Sub

    Function getTotalValue() As Decimal

        Dim total_val As Decimal = 0

        Try
            Dim sql_str As String = "SELECT ISNULL(SUM(Processes_Residual), 0) AS Processes_Residual FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,8 , 1 ) <> 0 AND Processes_State = 2"

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE CARD_NO = '" & txt_card_no.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
            End If

            If ddl_clinics.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Cilinc = " & ddl_clinics.SelectedValue
            End If

            If ddl_services.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_Services = " & ddl_services.SelectedValue
            End If

            If ddl_sub_service.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_SubServices = " & ddl_sub_service.SelectedValue
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
                Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
                sql_str = sql_str & " And Processes_Date >= '" & start_dt & "' AND Processes_Date <= '" & end_dt & "'"
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text = "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_start_dt.Text & "'"
            End If

            If txt_start_dt.Text = "" And txt_end_dt.Text <> "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_end_dt.Text & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.C_ID IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If ddl_motalba.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            ElseIf ddl_motalba.SelectedValue = 2 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID NOT IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1) AND Processes_Cilinc <> 43"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Reservation_Code IN (SELECT INC_Patient_Code FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)

            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            total_val = sel_com.ExecuteScalar
            insurance_SQLcon.Close()

            Return total_val

        Catch ex As Exception
            MsgBox("4" & ex.Message)
        End Try
    End Function

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click
        Response.Redirect(Request.RawUrl)

    End Sub

    Private Sub ddl_search_field_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_search_field.SelectedIndexChanged
        If ddl_search_field.SelectedValue <> 0 Then
            ddl_operation.Enabled = True
            txt_search_val.Enabled = True
        Else
            ddl_operation.Enabled = False
            txt_search_val.Enabled = False
            ddl_operation.SelectedValue = 0
            txt_search_val.Text = ""
        End If
    End Sub

    Private Sub btn_export_excel_Click(sender As Object, e As EventArgs) Handles btn_export_excel.Click

        Dim dt As New DataTable("GridView_Data")
        For Each cell As TableCell In GridView1.HeaderRow.Cells
            dt.Columns.Add(cell.Text)
        Next
        For Each row As GridViewRow In GridView1.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 1
                dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text
            Next
        Next

        Using wb As New XLWorkbook()
            Dim ws = wb.Worksheets.Add(dt)
            ws.Rows().Height = 20
            ws.Tables.FirstOrDefault().ShowAutoFilter = False
            ws.Rows().Style.Border.TopBorder = XLBorderStyleValues.Medium
            ws.Rows().Style.Border.TopBorderColor = XLColor.Black
            ws.Rows().Style.Border.BottomBorder = XLBorderStyleValues.Medium
            ws.Rows().Style.Border.BottomBorderColor = XLColor.Black
            ws.Rows().Style.Border.RightBorder = XLBorderStyleValues.Medium
            ws.Rows().Style.Border.RightBorderColor = XLColor.Black
            ws.Rows().Style.Border.LeftBorder = XLBorderStyleValues.Medium
            ws.Rows().Style.Border.LeftBorderColor = XLColor.Black
            ws.Rows().Style.Fill.BackgroundColor = XLColor.White
            ws.Rows().Style.Font.FontColor = XLColor.Black
            ws.Rows(1).Style.Font.FontColor = XLColor.Black
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=StatisticsReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.[End]()
            End Using
        End Using
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        'If (e.CommandName = "pat_name") Then
        '    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        '    Dim row As GridViewRow = GridView1.Rows(index)

        '    Response.Redirect("Companies/patientInfo.aspx?pID=" & (row.Cells(0).Text))
        'End If
    End Sub

    Private Sub ddl_motalba_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_motalba.SelectedIndexChanged
        If ddl_motalba.SelectedValue = 1 Then
            txt_invoce_no.Enabled = True
            txt_invoce_no.Focus()
        Else
            txt_invoce_no.Enabled = False
        End If
    End Sub
End Class