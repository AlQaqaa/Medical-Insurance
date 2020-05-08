﻿Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting
Imports System.IO

Public Class statistics
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim dt_search_result As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            txt_invoce_no.Enabled = True
            txt_invoce_no.Focus()
        Else
            txt_invoce_no.Enabled = False
        End If
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click

        If ddl_search_field.SelectedValue <> 0 And txt_search_val.Text = "" Then
            Label1.Text = "مطلوب"
            Exit Sub

        End If
        Label1.Text = ""

        getData()
        bindChartsClinic()
        lbl_total.Text = Format(getTotalValue(), "0,0.000") & " د.ل"
        lbl_patient_count.Text = patientCount()


        If ddl_companies.SelectedValue <> 0 Then
            lbl_header_chart.Text = ddl_companies.SelectedItem.Text
        Else
            lbl_header_chart.Text = ""
        End If
    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT Processes_Reservation_Code, ISNULL((SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,7 , 2 )), '') AS COMPANY_NAME, SUBSTRING([Processes_Reservation_Code],9 , 6) AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id), '') AS MedicalStaff_AR_Name, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,9 , 6)), '') AS PATIENT_NAME FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) <> 0"

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CARD_NO = '" & txt_patient_name.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
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
                sql_str = sql_str & " AND CONVERT(VARCHAR, Processes_Date, 103) between '" & txt_start_dt.Text & "' AND '" & txt_end_dt.Text & "'"
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text = "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_start_dt.Text & "'"
            End If

            If txt_start_dt.Text = "" And txt_end_dt.Text <> "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_end_dt.Text & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If CheckBox1.Checked = True Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            If CheckBox2.Checked = True Then
                sql_str = sql_str & " AND Processes_State in (2,4)"
            Else
                sql_str = sql_str & " AND Processes_State = 2"
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            'Dim dt_result As New DataTable
            'dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_search_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_search_result.Rows.Count > 0 Then
                btn_export_excel.Enabled = True
                Panel1.Visible = True
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
            MsgBox(ex.Message)
        End Try

    End Sub

    Function patientCount() As Integer

        Dim count_val As Integer = 0

        Try
            Dim sql_str As String = "SELECT SUBSTRING(Processes_Reservation_Code,9 , 6 ) FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) <> 0"

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CARD_NO = '" & txt_patient_name.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
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
                sql_str = sql_str & " AND CONVERT(VARCHAR, Processes_Date, 103) between '" & txt_start_dt.Text & "' AND '" & txt_end_dt.Text & "'"
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text = "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_start_dt.Text & "'"
            End If

            If txt_start_dt.Text = "" And txt_end_dt.Text <> "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_end_dt.Text & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If CheckBox1.Checked = True Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            If CheckBox2.Checked = True Then
                sql_str = sql_str & " AND Processes_State in (2,4)"
            Else
                sql_str = sql_str & " AND Processes_State = 2"
            End If

            sql_str = sql_str & " GROUP BY SUBSTRING(Processes_Reservation_Code,9 , 6 )"

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
            MsgBox(ex.Message)
        End Try
    End Function

    Sub bindChartsClinic()
        Try
            Dim sql_str As String = "SELECT (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_CompanyProcesses.Processes_Cilinc) AS CLINIC_NAME, COUNT(*) AS CLINIC_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) <> 0"

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CARD_NO = '" & txt_patient_name.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
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
                sql_str = sql_str & " AND CONVERT(VARCHAR, Processes_Date, 103) between '" & txt_start_dt.Text & "' AND '" & txt_end_dt.Text & "'"
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text = "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_start_dt.Text & "'"
            End If

            If txt_start_dt.Text = "" And txt_end_dt.Text <> "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_end_dt.Text & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If CheckBox1.Checked = True Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            If CheckBox2.Checked = True Then
                sql_str = sql_str & " AND Processes_State in (2,4)"
            Else
                sql_str = sql_str & " AND Processes_State = 2"
            End If

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
            MsgBox(ex.Message)
        End Try
    End Sub

    Function getTotalValue() As Decimal

        Dim total_val As Decimal = 0

        Try
            Dim sql_str As String = "SELECT SUM(Processes_Paid) AS Processes_Paid FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) <> 0"

            If ddl_companies.SelectedItem.Value <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) = " & ddl_companies.SelectedValue
            End If

            If txt_patient_name.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAME_ARB LIKE '%" & txt_patient_name.Text & "%')"
            End If

            If txt_card_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CARD_NO = '" & txt_patient_name.Text & "')"
            End If

            If txt_emp_no.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE BAGE_NO = '" & txt_emp_no.Text & "')"
            End If

            If txt_patient_code.Text <> "" Then
                sql_str = sql_str & " AND Processes_Reservation_Code = " & txt_patient_code.Text
            End If

            If ddl_doctors.SelectedValue <> 0 Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_Doctor_ID FROM HAG_Processes_Doctor WHERE Doctor_Processes_ID = " & ddl_doctors.SelectedValue & ")"
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
                sql_str = sql_str & " AND CONVERT(VARCHAR, Processes_Date, 103) between '" & txt_start_dt.Text & "' AND '" & txt_end_dt.Text & "'"
            End If

            If txt_start_dt.Text <> "" And txt_end_dt.Text = "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_start_dt.Text & "'"
            End If

            If txt_start_dt.Text = "" And txt_end_dt.Text <> "" Then
                sql_str = sql_str & " AND Processes_Date = '" & txt_end_dt.Text & "'"
            End If

            If ddl_payment_type.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) IN (SELECT C_ID FROM INC_COMPANY_DETIAL WHERE PYMENT_TYPE = " & ddl_payment_type.SelectedValue & " AND DATE_START <= '" & Date.Now.Date & "' AND DATE_END >= '" & Date.Now.Date & "')"
            End If

            If ddl_invoice_type.SelectedValue = 1 Then
                sql_str = sql_str & " AND Processes_Cilinc NOT IN (SELECT ECID FROM EWA_Clinic)"
            ElseIf ddl_invoice_type.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Cilinc IN (SELECT ECID FROM EWA_Clinic)"
            End If

            If ddl_relation.SelectedValue <> -1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE CONST_ID = " & ddl_relation.SelectedValue & ")"
            End If

            If ddl_sex.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE GENDER = " & ddl_sex.SelectedValue & ")"
            End If

            If txt_phone_num.Text <> "" Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE PHONE_NO = '" & txt_phone_num.Text & "')"
            End If

            If ddl_NAL_ID.SelectedValue <> 0 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE NAL_ID = " & ddl_NAL_ID.SelectedValue & ")"
            End If

            If CheckBox1.Checked = True Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1)"
            End If

            If txt_invoce_no.Text <> "" Then
                sql_str = sql_str & " AND Processes_ID IN (SELECT Processes_ID FROM INC_MOTALBAT WHERE MOTALABA_STS = 1 AND INVOICE_NO = " & txt_invoce_no.Text & ")"
            End If

            If ddl_search_field.SelectedValue = 1 Then
                sql_str = sql_str & " AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) IN (SELECT PINC_ID FROM INC_PATIANT WHERE DATEDIFF(YEAR, BIRTHDATE, getdate()) " & ddl_operation.SelectedValue & " " & txt_search_val.Text & ")"
            ElseIf ddl_search_field.SelectedValue = 2 Then
                sql_str = sql_str & " AND Processes_Price " & ddl_operation.SelectedValue & " " & txt_search_val.Text
            End If

            If CheckBox2.Checked = True Then
                sql_str = sql_str & " AND Processes_State in (2,4)"
            Else
                sql_str = sql_str & " AND Processes_State = 2"
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)

            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            total_val = sel_com.ExecuteScalar
            insurance_SQLcon.Close()

            Return total_val
        Catch ex As Exception
            MsgBox(ex.Message)
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

    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.getData()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

    Private Sub btn_export_excel_Click(sender As Object, e As EventArgs) Handles btn_export_excel.Click
        If GridView1.Rows.Count > 0 Then
            Try
                GridView1.AllowPaging = False
                GridView1.GridLines = GridLines.Both
                GridView1.Font.Name = "Arial"
                GridView1.HeaderStyle.Font.Size = 14
                GridView1.HeaderStyle.VerticalAlign = VerticalAlign.Middle
                GridView1.HeaderStyle.Height = 30
                GridView1.RowStyle.Font.Size = 11
                GridView1.RowStyle.Font.Bold = True
                GridView1.RowStyle.HorizontalAlign = HorizontalAlign.Center
                getData()
                GridView1.DataSource = dt_search_result
                GridView1.DataBind()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.Buffer = True
                HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "Report.xls"))
                HttpContext.Current.Response.ContentType = "application/ms-excel"
                HttpContext.Current.Response.Charset = "UTF-8"
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
                HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
                Dim sw As New StringWriter()
                Dim htw As New HtmlTextWriter(sw)
                Panel2.RenderControl(htw)
                HttpContext.Current.Response.Write(sw.ToString())
                HttpContext.Current.Response.[End]()

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                GridView1.Columns(0).Visible = True

            End Try
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "pat_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Session.Item("patiant_id") = (row.Cells(0).Text) ' 13471

            Response.Redirect("Companies/patientInfo.aspx")
        End If
    End Sub
End Class