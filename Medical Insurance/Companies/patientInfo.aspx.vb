Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting
Imports System.IO
Imports System.Globalization

Public Class patientInfo
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then

            Dim pat_no As Integer = Val(Session("patiant_id"))

            If pat_no = 0 Then
                Response.Redirect("LISTPATIANT.aspx")
            End If

            Dim company_name_panel As Panel = DirectCast(Master.FindControl("Panel_company_info"), Panel)

            getPatInfo()
            getBlockServices()
            getConstList()
            getProcessesData()
            bindChartsSubServices()
            bindChartsDoctros()


            Dim contract_count As Integer = 0
            Dim sel_com As New SqlCommand("SELECT COUNT(*) AS CONTRACT_COUNT FROM INC_COMPANY_DETIAL WHERE C_ID = " & Session("company_id"), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            contract_count = sel_com.ExecuteScalar
            insurance_SQLcon.Close()

            Dim total_expenses As Decimal = 0
            If contract_count = 1 Then
                total_expenses = getTotalPatientExpenses() + getOldExpenses()
            Else
                total_expenses = getTotalPatientExpenses()
            End If

            lbl_total_expensess.Text = "إجمالي المصروفات " & Format(total_expenses, "0,0.000") & " د.ل"
            If getMaxValue() <> 0 Then
                With Me.Chart3
                    .Legends.Clear()
                    .Series.Clear()
                    .ChartAreas.Clear()
                End With

                Dim areas1 As ChartArea = Me.Chart3.ChartAreas.Add("Areas1")

                With areas1
                End With

                Dim series1 As Series = Me.Chart3.Series.Add("Series1")

                With series1
                    .ChartArea = areas1.Name
                    .ChartType = SeriesChartType.Doughnut
                    .Points.AddXY("إجمالي المصروفات", total_expenses)
                    .Points.AddXY("السقف العام", getMaxValue())
                    .ToolTip = "#VALX" & vbLf & "الإجمالي: #VALY" & vbLf & "النسبة: #PERCENT"
                    .Label = "#PERCENT"
                    .LegendText = "#AXISLABEL"
                End With

                Dim legends1 As Legend = Me.Chart3.Legends.Add("Legends1")
            End If


        End If


    End Sub

    Sub getPatInfo()
        Dim get_pet As New SqlCommand("SELECT CARD_NO, NAME_ARB, NAME_ENG, C_ID, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, isnull(PHONE_NO, 0) AS PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, P_STATE, isnull(NAT_NUMBER, 0) AS NAT_NUMBER, IMAGE_CARD, (SELECT C_NAME_ARB FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID) AS COMPANY_NAME, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID, (SELECT Nationality_AR_Name FROM Main_Nationality WHERE MAIN_NATIONALITY.Nationality_ID = INC_PATIANT.NAL_ID) AS NAT_NAME, (SELECT City_AR_Name FROM Main_City WHERE Main_City.City_ID = INC_PATIANT.CITY_ID) AS CITY_NAME, OLD_ID FROM INC_PATIANT WHERE PINC_ID = " & Val(Session("patiant_id")), insurance_SQLcon)
        Dim dt_pat As New DataTable
        dt_pat.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_pat.Load(get_pet.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_pat.Rows.Count > 0 Then
            Dim dr_pat = dt_pat.Rows(0)
            lbl_name_eng.Text = dr_pat!NAME_ENG
            lbl_pat_name.Text = dr_pat!NAME_ARB
            lbl_birthdate.Text = dr_pat!BIRTHDATE
            lbl_nat_num.Text = dr_pat!NAT_NUMBER
            lbl_phone.Text = dr_pat!PHONE_NO
            lbl_company_name.Text = dr_pat!COMPANY_NAME
            lbl_card_no.Text = dr_pat!CARD_NO
            lbl_exp_dt.Text = dr_pat!EXP_DATE
            lbl_bage_no.Text = dr_pat!BAGE_NO
            ViewState("bage_no") = dr_pat!BAGE_NO
            Session("company_id") = dr_pat!C_ID
            ViewState("old_id") = dr_pat!OLD_ID
            lbl_const.Text = dr_pat!CONST_ID
            img_pat_img.ImageUrl = "../" & dr_pat!IMAGE_CARD
            ViewState("pat_state") = dr_pat!P_STATE
            If dr_pat!EXP_DATE > Date.Now.Date Then
                Panel1.Visible = False
            Else
                Panel1.Visible = True
            End If
            If dr_pat!P_STATE = 0 Then
                lbl_icon_sts.CssClass = "text-success"
                lbl_sts.Text = "مفعل"
                lbl_sts.CssClass = "text-success"
                btn_change_sts.Text = "إيقاف"
                btn_change_sts.CssClass = "btn btn-outline-danger btn-block"
            Else
                lbl_icon_sts.CssClass = "text-danger"
                lbl_sts.Text = "موقوف"
                lbl_sts.CssClass = "text-danger"
                btn_change_sts.Text = "تفعيل"
                btn_change_sts.CssClass = "btn btn-outline-success btn-block"
            End If
        End If
    End Sub

    Private Sub btn_change_sts_Click(sender As Object, e As EventArgs) Handles btn_change_sts.Click

        Dim new_sts As Integer = 0

        If ViewState("pat_state") = 0 Then
            new_sts = 1
        Else
            new_sts = 0
        End If

        Try
            Dim edit_sts As New SqlCommand("UPDATE INC_PATIANT SET P_STATE = " & new_sts & " WHERE PINC_ID = " & Val(Session("patiant_id")), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            edit_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            If new_sts = 1 Then
                add_action(1, 2, 2, "إيقاف المنتفع رقم: " & Val(Session("patiant_id")), Session("User_Id"), GetIPAddress())
            Else
                add_action(1, 2, 2, "تفعيل المنتفع رقم: " & Val(Session("patiant_id")), Session("User_Id"), GetIPAddress())
            End If

            getPatInfo()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_ban_service_Click(sender As Object, e As EventArgs) Handles btn_ban_service.Click
        Try
            Dim ins_com As New SqlCommand("INSERT INTO INC_BLOCK_SERVICES (OBJECT_ID, SER_ID, BLOCK_TP, NOTES, USER_ID, USER_IP) VALUES (@OBJECT_ID, @SER_ID, @BLOCK_TP, @NOTES, @USER_ID, @USER_IP)", insurance_SQLcon)
            ins_com.Parameters.Add("@OBJECT_ID", SqlDbType.Int).Value = Val(Session("patiant_id"))
            ins_com.Parameters.Add("@SER_ID", SqlDbType.Int).Value = ddl_services.SelectedValue
            ins_com.Parameters.Add("@BLOCK_TP", SqlDbType.Int).Value = 1 ' Block Service
            ins_com.Parameters.Add("@NOTES", SqlDbType.NVarChar).Value = txt_notes.Text
            ins_com.Parameters.Add("@USER_ID", SqlDbType.Int).Value = Session("User_Id")
            ins_com.Parameters.Add("@USER_IP", SqlDbType.NVarChar).Value = GetIPAddress()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ins_com.ExecuteNonQuery()
            insurance_SQLcon.Close()

            add_action(1, 2, 2, "حظر الخدمة رقم: " & ddl_services.SelectedValue & " عن المنتفع رقم: " & Val(Session("patiant_id")), 1, GetIPAddress())

            getBlockServices()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub getBlockServices()
        Dim get_com As New SqlCommand("SELECT SER_ID, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_BLOCK_SERVICES.SER_ID) AS SERVICE_NAME, (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_BLOCK_SERVICES.SER_ID) AS SERV_CODE, NOTES FROM INC_BLOCK_SERVICES WHERE OBJECT_ID = " & Val(Session("patiant_id")) & " AND BLOCK_TP = 1", insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(get_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView1.DataSource = dt_result
            GridView1.DataBind()
            Label1.Text = ""
        Else
            dt_result.Rows.Clear()
            GridView1.DataBind()
            Label1.Text = "لا توجد خدمات محظورة عن هذا المشترك"
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Stop Block ################
        If (e.CommandName = "stop_block") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim del_com As New SqlCommand("DELETE FROM INC_BLOCK_SERVICES WHERE OBJECT_ID = " & Val(Session("patiant_id")) & " AND SER_ID = " & (row.Cells(0).Text) & " AND BLOCK_TP = 1", insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            del_com.ExecuteNonQuery()
            insurance_SQLcon.Close()

            add_action(1, 2, 2, "إيقف حظر الخدمة رقم: " & (row.Cells(0).Text) & " عن المنتفع رقم: " & Val(Session("patiant_id")), Session("User_Id"), GetIPAddress())

            getBlockServices()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت العملية بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

        End If
    End Sub

    Private Sub getConstList()

        Try
            If ViewState("bage_no") <> 0 Then
                Dim sel_com As New SqlCommand("SELECT PINC_ID, NAME_ARB, C_ID, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_NAME FROM INC_PATIANT WHERE BAGE_NO = '" & ViewState("bage_no") & "' AND C_ID = " & Session("company_id") & " ORDER BY CONST_ID ASC", insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_result.Rows.Count > 0 Then
                    GridView2.DataSource = dt_result
                    GridView2.DataBind()
                Else
                    dt_result.Rows.Clear()
                    GridView2.DataSource = dt_result
                    GridView2.DataBind()
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand
        '################ When User Press On Patiant Name ################
        If (e.CommandName = "pat_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView2.Rows(index)
            Session.Clear()
            Session.Item("patiant_id") = (row.Cells(0).Text)
            Session.Item("company_id") = (row.Cells(1).Text)
            Response.Redirect("patientInfo.aspx")
        End If
    End Sub

    Private Sub getProcessesData()

        Try
            Dim sql_str As String = "SELECT TOP(10) Processes_ID, Processes_Reservation_Code, SUBSTRING([Processes_Reservation_Code],9 , 6) AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id), '') AS MedicalStaff_AR_Name, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,9 , 6)), '') AS PATIENT_NAME FROM INC_CompanyProcesses WHERE SUBSTRING([Processes_Reservation_Code],9 , 6) = " & Session("patiant_id") & ""

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                Dim dr = dt_result.Rows(0)
                lbl_code.Text = dr!Processes_Reservation_Code
                GridView3.DataSource = dt_result
                GridView3.DataBind()
            Else
                lbl_code.Text = "لم يحصل على كود بعد"
                dt_result.Rows.Clear()
                GridView3.DataSource = dt_result
                GridView3.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btn_renew_card_Click(sender As Object, e As EventArgs) Handles btn_renew_card.Click
        Try
            Dim exp As String = DateTime.ParseExact(txt_exp_date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

            Dim renew_card As New SqlCommand("UPDATE INC_PATIANT SET EXP_DATE=@EXP_DATE WHERE PINC_ID=" & Session("patiant_id"), insurance_SQLcon)
            renew_card.Parameters.AddWithValue("@EXP_DATE", exp)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            renew_card.ExecuteNonQuery()
            insurance_SQLcon.Close()

            add_action(1, 2, 2, "تجديد بطاقة المنتفع رقم: " & Val(Session("patiant_id")), Session("User_Id"), GetIPAddress())

            getPatInfo()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_upload_card_Click(sender As Object, e As EventArgs) Handles btn_upload_card.Click
        Try
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If FileUpload1.PostedFile.FileName <> Nothing Then
                Dim newFilename As String = Val(Session("patiant_id")) & "N"
                Dim fileExtension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
                Dim updatedFilename As String = newFilename + fileExtension
                Dim fpath As String = Server.MapPath("../images/ImagePatiant") & "/" & updatedFilename
                Dim dbpath As String = "images/ImagePatiant" & "/" & updatedFilename
                Dim FEx As String
                FEx = IO.Path.GetExtension(fpath)
                If FEx <> ".jpg" And FEx <> ".JPG" And FEx <> ".png" And FEx <> ".PNG" And FEx <> ".bmp" And FEx <> ".jpeg" Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alertify.error('خطأ! صيغة الصورة غير صحيحة '); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                    Exit Sub
                End If
                If System.IO.File.Exists(fpath) Then
                    System.IO.File.Delete(Server.MapPath("../images/ImagePatiant" & "/" & updatedFilename))
                End If
                FileUpload1.PostedFile.SaveAs(fpath)

                Dim edit_img As New SqlCommand("UPDATE INC_PATIANT SET IMAGE_CARD = @IMAGE_CARD WHERE PINC_ID = " & Val(Session("patiant_id")), insurance_SQLcon)
                edit_img.Parameters.AddWithValue("@IMAGE_CARD", dbpath)
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                edit_img.ExecuteNonQuery()
                insurance_SQLcon.Close()
                Page_Load(sender, e)
            End If

            '''''''''''''''''''''''''''''''''''''''''
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub bindChartsSubServices()
        Try
            Dim sql_str As String = "SELECT TOP (10) (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS SubService_AR_Name, COUNT(*) AS SubService_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) <> 0 AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) = " & Val(Session("patiant_id")) & " GROUP BY Processes_SubServices ORDER BY COUNT(*) DESC"

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
                        .Points.AddXY(dr!SubService_AR_Name, dr!SubService_COUNT)
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

    Sub bindChartsDoctros()
        Try
            Dim sql_str As String = "SELECT TOP (10) (SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id) AS MedicalStaff_AR_Name, COUNT(*) AS doctors_COUNT FROM INC_CompanyProcesses WHERE Processes_Residual <> 0 AND SUBSTRING(Processes_Reservation_Code,7 , 2 ) <> 0 AND SUBSTRING(Processes_Reservation_Code,9 , 6 ) = " & Val(Session("patiant_id")) & " AND doctor_id <> 0 GROUP BY doctor_id ORDER BY COUNT(*) DESC"

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then

                With Me.Chart2
                    .Legends.Clear()
                    .Series.Clear()
                    .ChartAreas.Clear()
                End With

                Dim areas1 As ChartArea = Me.Chart2.ChartAreas.Add("Areas1")

                With areas1

                End With

                Dim series1 As Series = Me.Chart2.Series.Add("Series1")

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
                    Chart2.Series(0).Points.DataBindXY(x, y)
                    Chart2.Series(0).Label = "#PERCENT"
                End With


                Dim legends1 As Legend = Me.Chart2.Legends.Add("Legends1")
                legends1.IsTextAutoFit = True
                legends1.LegendStyle = LegendStyle.Table

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Function getTotalPatientExpenses() As Decimal
        Dim total_val As Decimal = 0

        Dim sel_com As New SqlCommand("select TOTAL_EXPENSES from INC_totalPatientExpenses(" & Session("patiant_id") & ",(select top(1) DATE_START from [dbo].[INC_COMPANY_DETIAL] where C_ID = " & Session("company_id") & " order by n desc),(select top(1) DATE_END from [dbo].[INC_COMPANY_DETIAL] where C_ID = " & Session("company_id") & " order by n desc))", insurance_SQLcon)
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        total_val = sel_com.ExecuteScalar
        insurance_SQLcon.Close()

        Return total_val
    End Function

    Function getOldExpenses() As Decimal
        Dim total_val As Decimal = 0

        Dim sel_com As New SqlCommand("SELECT ISNULL(BLNC_VALUE, 0) AS BLNC_VALUE FROM INC_OPEN_BALANCE WHERE OLD_ID =" & ViewState("old_id"), insurance_SQLcon)
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        total_val = sel_com.ExecuteScalar
        insurance_SQLcon.Close()

        Return total_val
    End Function

    Function getMaxValue() As Decimal
        Dim max_val As Decimal = 0

        Dim sel_com As New SqlCommand("select top(1) MAX_PERSON from [dbo].[INC_COMPANY_DETIAL] where C_ID = " & Session("company_id") & " order by n desc", insurance_SQLcon)
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        max_val = sel_com.ExecuteScalar
        insurance_SQLcon.Close()

        Return max_val

    End Function
End Class