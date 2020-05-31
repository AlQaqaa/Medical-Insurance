Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting
Imports System.IO

Public Class companyStatistics
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            If Session("company_id") IsNot Nothing Then

                getCompanyInfo()
                lbl_start_date.Text = ViewState("start_dt")

                getMostPatientVisit()
                getMoneyInfo()
                bindChartsSubServices()
                bindChartsDoctros()
                bindChartsClinic()
                getSummary()
                getPetientCount()
            Else
                Response.Redirect("../Default.aspx")

            End If

        End If

    End Sub

    Sub getCompanyInfo()
        Try
            Dim sel_com As New SqlCommand("SELECT (SELECT TOP 1 MAX_VAL FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS MAX_VAL, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_START, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_START, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_END, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_END, (SELECT TOP 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY CONTRACT_NO DESC) AS CONTRACT_NO, C_NAME_ARB, C_NAME_ENG, C_STATE, (CASE WHEN (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL ) IS NULL THEN  '-' ELSE (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL) END)AS MAIN_COMPANY, (SELECT TOP 1 PYMENT_TYPE FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS PYMENT_TYPE, (SELECT profile_name FROM INC_PRICES_PROFILES WHERE profile_Id = (SELECT TOP 1 PROFILE_PRICE_ID FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC)) AS PROFILE_PRICE_NAME FROM INC_COMPANY_DATA WHERE C_ID = " & Session("company_id"), insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Dim dr_com = dt_result.Rows(0)
                ViewState("start_dt") = dr_com!DATE_START
                ViewState("end_dt") = dr_com!DATE_END
                ViewState("MAX_VAL") = dr_com!MAX_VAL
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub getMostPatientVisit()
        Try
            Dim sel_com As New SqlCommand("SELECT TOP(10) COUNT(Processes_ID) AS P_COUNT, (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING([Processes_Reservation_Code],9 , 6 )) AS P_NAME FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING([Processes_Reservation_Code],7 , 2 ) = " & Session("company_id") & " AND Processes_Date > '" & ViewState("start_dt") & "' AND Processes_State IN (2,4) GROUP BY SUBSTRING([Processes_Reservation_Code],9 , 6 ) ORDER BY P_COUNT DESC", insurance_SQLcon)
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
                        x(i) = dt_result.Rows(i)(1).ToString()
                        y(i) = Convert.ToInt32(dt_result.Rows(i)(0))
                        Dim dr = dt_result.Rows(i)
                        .Points.AddXY(dr!P_NAME, dr!P_COUNT)
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

    Sub getMoneyInfo()
        Try
            Dim sel_com As New SqlCommand("SELECT SUM(Processes_Residual) AS C_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND Processes_State IN (2,4) AND SUBSTRING([Processes_Reservation_Code],7 , 2 ) = " & Session("company_id") & " AND Processes_Date >= '" & ViewState("start_dt") & "'", insurance_SQLcon)

            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                Dim dr = dt_result.Rows(0)
                With Me.Chart2
                    .Legends.Clear()
                    .Series.Clear()
                    .ChartAreas.Clear()
                End With

                Dim areas1 As ChartArea = Me.Chart2.ChartAreas.Add("Areas1")

                With areas1
                End With

                Dim series1 As Series = Me.Chart2.Series.Add("Series1")

                With series1
                    .ChartArea = areas1.Name
                    .ChartType = SeriesChartType.Doughnut
                    .Points.AddXY("المصروفات " & dr!C_COUNT, dr!C_COUNT)
                    .Points.AddXY("السقف " & ViewState("MAX_VAL"), ViewState("MAX_VAL"))
                    .ToolTip = "#VALY" & vbLf & "النسبة: #PERCENT"
                    Chart1.Series(0).Label = "#PERCENT"
                    Chart1.Series(0).LegendText = "#AXISLABEL"
                End With

                Dim legends1 As Legend = Me.Chart2.Legends.Add("Legends1")

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub bindChartsSubServices()
        Try
            Dim sql_str As String = "SELECT TOP (10) (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS SubService_AR_Name, COUNT(*) AS SubService_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING([Processes_Reservation_Code],7 , 2 ) = " & Session("company_id") & " AND Processes_Date >= '" & ViewState("start_dt") & "' AND Processes_State IN (2,4) GROUP BY Processes_SubServices ORDER BY COUNT(*) DESC"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then

                With Me.Chart3
                    .Legends.Clear()
                    .Series.Clear()
                    .ChartAreas.Clear()
                End With

                Dim areas1 As ChartArea = Me.Chart3.ChartAreas.Add("Areas1")

                With areas1
                End With

                Dim series1 As Series = Me.Chart3.Series.Add("Series1")

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
                        .Points.AddXY(dr!SubService_AR_Name, dr!SubService_COUNT)
                        .ToolTip = "#VALX" & vbLf & "الخدمات المقدمة: #VALY" & vbLf & "النسبة: #PERCENT"
                    Next
                    Chart3.Series(0).Points.DataBindXY(x, y)
                    Chart3.Series(0).Label = "#PERCENT"
                    Chart3.Series(0).LegendText = "#AXISLABEL"
                End With


                Dim legends1 As Legend = Me.Chart3.Legends.Add("Legends1")
                legends1.IsTextAutoFit = True
                legends1.LegendStyle = LegendStyle.Table

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub bindChartsClinic()
        Try
            Dim sql_str As String = "SELECT TOP (10) (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_CompanyProcesses.Processes_Cilinc) AS Clinic_AR_Name, COUNT(*) AS Clinic_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING([Processes_Reservation_Code],7 , 2 ) = " & Session("company_id") & " AND Processes_Date >= '" & ViewState("start_dt") & "' AND Processes_State IN (2,4) GROUP BY Processes_Cilinc ORDER BY COUNT(*) DESC"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then

                With Me.Chart5
                    .Legends.Clear()
                    .Series.Clear()
                    .ChartAreas.Clear()
                End With

                Dim areas1 As ChartArea = Me.Chart5.ChartAreas.Add("Areas1")

                With areas1
                End With

                Dim series1 As Series = Me.Chart5.Series.Add("Series1")

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
                        .Points.AddXY(dr!Clinic_AR_Name, dr!Clinic_COUNT)
                        .ToolTip = "#VALX" & vbLf & "العيادات: #VALY" & vbLf & "النسبة: #PERCENT"
                    Next
                    Chart5.Series(0).Points.DataBindXY(x, y)
                    Chart5.Series(0).Label = "#PERCENT"
                    Chart5.Series(0).LegendText = "#AXISLABEL"
                End With


                Dim legends1 As Legend = Me.Chart5.Legends.Add("Legends1")
                legends1.IsTextAutoFit = True
                legends1.LegendStyle = LegendStyle.Table

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub bindChartsDoctros()
        Try
            Dim sql_str As String = "SELECT TOP (10) (SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id) AS MedicalStaff_AR_Name, COUNT(*) AS doctors_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING([Processes_Reservation_Code],7 , 2 ) = " & Session("company_id") & " AND Processes_Date >= '" & ViewState("start_dt") & "' AND doctor_id <> 0 AND Processes_State IN (2,4) GROUP BY doctor_id ORDER BY COUNT(*) DESC"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then

                With Me.Chart4
                    .Legends.Clear()
                    .Series.Clear()
                    .ChartAreas.Clear()
                End With

                Dim areas1 As ChartArea = Me.Chart4.ChartAreas.Add("Areas1")

                With areas1

                End With

                Dim series1 As Series = Me.Chart4.Series.Add("Series1")

                Dim x As String() = New String(dt_result.Rows.Count - 1) {}
                Dim y As Integer() = New Integer(dt_result.Rows.Count - 1) {}

                With series1
                    .ChartArea = areas1.Name
                    '.ChartType = SeriesChartType.Pie
                    .CustomProperties = "PieLineColor=Black, PieLabelStyle=Outside"
                    For i = 0 To dt_result.Rows.Count - 1
                        x(i) = dt_result.Rows(i)(0).ToString()
                        y(i) = Convert.ToInt32(dt_result.Rows(i)(1))
                        Dim dr = dt_result.Rows(i)
                        .Points.AddXY(dr!MedicalStaff_AR_Name, dr!doctors_COUNT)
                        .ToolTip = "#VALX" & vbLf & "الخدمات المقدمة: #VALY" & vbLf & "النسبة: #PERCENT"
                        .LegendText = "عدد الزيارات"
                    Next
                    Chart4.Series(0).Points.DataBindXY(x, y)
                    Chart4.Series(0).Label = "#PERCENT"
                End With


                Dim legends1 As Legend = Me.Chart4.Legends.Add("Legends1")
                legends1.IsTextAutoFit = True
                legends1.LegendStyle = LegendStyle.Table

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub getSummary()
        Dim sql_str As String = "SELECT ISNULL(AVG(Processes_Residual),0) AS AVG_EXPENS, ISNULL(SUM(Processes_Residual),0) AS TOTAL_EXPENS, ISNULL(COUNT(*),0) AS TOTAL_SERVICES FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING([Processes_Reservation_Code],7 , 2 ) = " & Session("company_id") & " AND Processes_Date >= '" & ViewState("start_dt") & "' AND Processes_State IN (2,4)"

        Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            Dim dr = dt_result.Rows(0)
            lbl_expens_avg.Text = Format(dr!AVG_EXPENS, "0,0.000") & " د.ل"
            lbl_expens_total.Text = Format(dr!TOTAL_EXPENS, "0,0.000") & " د.ل"
            lbl_service_total.Text = dr!TOTAL_SERVICES
        Else
            lbl_expens_avg.Text = "0"
            lbl_expens_total.Text = "0"
            lbl_service_total.Text = "0"
        End If
    End Sub

    Sub getPetientCount()
        Dim sql_str As String = "SELECT COUNT(DISTINCT SUBSTRING([Processes_Reservation_Code],9 , 6 )) AS p_count FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING([Processes_Reservation_Code],7 , 2 ) = " & Session("company_id") & " AND Processes_Date >= '" & ViewState("start_dt") & "' AND Processes_State IN (2,4)"

        Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            Dim dr = dt_result.Rows(0)
            lbl_patient_count.Text = dr!p_count
        Else
            lbl_patient_count.Text = "0"
        End If
    End Sub

End Class