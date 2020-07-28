Imports System.Data.SqlClient
Public Class companySubServices
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("company_services") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If

            Dim company_no As Integer = Val(Session("company_id"))

            If company_no = 0 Then
                Response.Redirect("Default.aspx", False)
                Exit Sub
            End If

            Panel1.Visible = False

            ViewState("company_no") = company_no

            Dim sel_com As New SqlCommand("SELECT (SELECT top 1 (DATE_END) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS DATE_END, (SELECT top 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS CONTRACT_NO, C_NAME_ARB, C_NAME_ENG FROM INC_COMPANY_DATA WHERE C_id = " & company_no, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Dim dr_company = dt_result.Rows(0)
                ViewState("contract_no") = dr_company!CONTRACT_NO
            End If

            getClinicAvailable()
            ' getSubServicesAvailable()
            getSubServices(3)

        End If
    End Sub

    Sub getClinicAvailable()
        Dim sel_com As New SqlCommand("SELECT 0 AS CLINIC_ID, 'جميع العيادات' AS CLINIC_NAME FROM INC_CLINICAL_RESTRICTIONS UNION SELECT CLINIC_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.Clinic_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS CLINIC_NAME FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            ddl_clinics.DataSource = dt_result
            ddl_clinics.DataValueField = "CLINIC_ID"
            ddl_clinics.DataTextField = "CLINIC_NAME"
            ddl_clinics.DataBind()
        End If
    End Sub

    Sub getSubServices(ByVal para As Integer)

        Dim search_by As String

        If para = 1 Then
            search_by = " AND SubService_Clinic = " & ddl_clinics.SelectedValue
        ElseIf para = 2 Then
            search_by = " AND SubService_Clinic = " & ddl_clinics.SelectedValue & " and SubService_Service_ID = " & ddl_services.SelectedValue
        Else
            search_by = " "
        End If

        Dim sel_data As New SqlCommand("SELECT MAIN_SUBSERVICES.SUBSERVICE_ID, SUBSERVICE_CODE, SUBSERVICE_AR_NAME, SERVICE_STS, CLINIC_AR_NAME AS CLINIC_NAME, ISNULL(PERSON_PER, 1) AS PERSON_PER, ISNULL(FAMILY_PER, 1) AS FAMILY_PER, ISNULL(PARENT_PER, 1) AS PARENT_PER, ISNULL(MAX_PERSON_VAL, 1) AS MAX_PERSON_VAL, ISNULL(MAX_FAMILY_VAL,1) AS MAX_FAMILY_VAL, ISNULL(SER_STATE, 1) AS SER_STATE, ISNULL(INC_SUB_SERVICES_RESTRICTIONS.IS_APPROVAL, 1) AS IS_APPROVAL FROM MAIN_SUBSERVICES
            LEFT JOIN MAIN_CLINIC ON MAIN_CLINIC.CLINIC_ID = MAIN_SUBSERVICES.SUBSERVICE_CLINIC
            LEFT JOIN INC_SERVICES_RESTRICTIONS ON INC_SERVICES_RESTRICTIONS.SERVICE_ID = MAIN_SUBSERVICES.SUBSERVICE_SERVICE_ID AND CONTRACT_NO = " & ViewState("contract_no") & "
            LEFT JOIN INC_SUB_SERVICES_RESTRICTIONS ON INC_SUB_SERVICES_RESTRICTIONS.SUBSERVICE_ID = MAIN_SUBSERVICES.SUBSERVICE_ID AND INC_SUB_SERVICES_RESTRICTIONS.C_ID = " & Session("company_id") & " AND INC_SUB_SERVICES_RESTRICTIONS.CONTRACT_NO = " & ViewState("contract_no") & "
            WHERE INC_SERVICES_RESTRICTIONS.SERVICE_STS = 0 " & search_by, insurance_SQLcon)
        Dim dt_res As New DataTable
        dt_res.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_res.Load(sel_data.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_res.Rows.Count > 0 Then
            Panel1.Visible = True
            GridView1.DataSource = dt_res
            GridView1.DataBind()
            For i = 0 To dt_res.Rows.Count - 1
                Dim dd As GridViewRow = GridView1.Rows(i)

                Dim ch As CheckBox = dd.FindControl("CheckBox2")
                Dim ch_Is_Approval As CheckBox = dd.FindControl("CheckBox3")
                Dim txt_person_per As TextBox = dd.FindControl("txt_person_per")
                Dim txt_family_per As TextBox = dd.FindControl("txt_family_per")
                Dim txt_parent_per As TextBox = dd.FindControl("txt_parent_per")
                Dim txt_person_max As TextBox = dd.FindControl("txt_person_max")
                Dim txt_family_max As TextBox = dd.FindControl("txt_family_max")

                ch.Checked = If(dt_res.Rows(i)("SER_STATE") = 1, False, True)
                ch_Is_Approval.Checked = If(dt_res.Rows(i)("IS_APPROVAL") = True, False, True)

                txt_person_per.Text = dt_res.Rows(i)("PERSON_PER")
                txt_family_per.Text = dt_res.Rows(i)("FAMILY_PER")
                txt_parent_per.Text = dt_res.Rows(i)("PARENT_PER")
                txt_person_max.Text = dt_res.Rows(i)("MAX_PERSON_VAL")
                txt_family_max.Text = dt_res.Rows(i)("MAX_FAMILY_VAL")
            Next
        Else
            Panel1.Visible = False
            dt_res.Rows.Clear()
            GridView1.DataSource = dt_res
            GridView1.DataBind()
        End If
    End Sub

    Private Sub getServicesAvailable()
        Dim MAX_VALUE As String = "ISNULL((SELECT MAX_VALUE FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.Service_ID = Main_Services.Service_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS MAX_VALUE,"
        Dim SER_STATE As String = "ISNULL((SELECT SERVICE_STS FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.Service_ID = Main_Services.Service_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS SER_STATE"


        Dim sel_data As New SqlCommand("select 0 as Service_ID, 'الكل' as Service_AR_Name union select Service_ID, (select Service_AR_Name from Main_Services WHERE Main_Services.Service_ID = INC_SERVICES_RESTRICTIONS.Service_ID) as Service_AR_Name from INC_SERVICES_RESTRICTIONS where SERVICE_STS = 0 AND CONTRACT_NO = " & ViewState("contract_no") & " AND CLINIC_ID = " & ddl_clinics.SelectedValue, insurance_SQLcon)
        Dim dt_res As New DataTable
        dt_res.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_res.Load(sel_data.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_res.Rows.Count > 0 Then
            ddl_services.DataSource = dt_res
            ddl_services.DataValueField = "Service_ID"
            ddl_services.DataTextField = "Service_AR_Name"
            ddl_services.DataBind()
        End If
    End Sub

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        getServicesAvailable()
        getSubServices(1)
        txt_clinics_max.Text = getMaxClinicValue(ddl_clinics.SelectedValue, ViewState("company_no"), ViewState("contract_no"))
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click


        If ddl_clinics.SelectedValue = 0 Then
            Dim sel_com As New SqlCommand("select SubService_ID, SubService_Clinic FROM Main_SubServices WHERE SubService_Clinic IN (SELECT CLINIC_ID FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no") & ")", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                For i = 0 To dt_result.Rows.Count - 1
                    Dim dr_res = dt_result.Rows(i)
                    Dim insClinic As New SqlCommand
                    insClinic.Connection = insurance_SQLcon
                    insClinic.CommandText = "INC_addCompanySubServices"
                    insClinic.CommandType = CommandType.StoredProcedure
                    insClinic.Parameters.AddWithValue("@cID", ViewState("company_no"))
                    insClinic.Parameters.AddWithValue("@clinicID", dr_res!SubService_Clinic)
                    insClinic.Parameters.AddWithValue("@contractNo", ViewState("contract_no"))
                    insClinic.Parameters.AddWithValue("@subServiceId", dr_res!SubService_ID)
                    insClinic.Parameters.AddWithValue("@serPersonPer", 0)
                    insClinic.Parameters.AddWithValue("@serFamilyPer", 0)
                    insClinic.Parameters.AddWithValue("@serParentPer", 0)
                    insClinic.Parameters.AddWithValue("@serPersonMax", 0)
                    insClinic.Parameters.AddWithValue("@serFamilyMax", 0)
                    insClinic.Parameters.AddWithValue("@serState", 0)
                    insClinic.Parameters.AddWithValue("@serPaymentType", 0)
                    insClinic.Parameters.AddWithValue("@Is_Approval", True)
                    insClinic.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
                    insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
                    insurance_SQLcon.Open()
                    insClinic.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                    insClinic.CommandText = ""
                Next
            End If
        Else
            For Each dd As GridViewRow In GridView1.Rows
                Dim ch As CheckBox = dd.FindControl("CheckBox2")
                Dim ch_Is_Approval As CheckBox = dd.FindControl("CheckBox3")
                Dim txt_person_per As TextBox = dd.FindControl("txt_person_per")
                Dim txt_family_per As TextBox = dd.FindControl("txt_family_per")
                Dim txt_parent_per As TextBox = dd.FindControl("txt_parent_per")
                Dim txt_person_max As TextBox = dd.FindControl("txt_person_max")
                Dim txt_family_max As TextBox = dd.FindControl("txt_family_max")

                Dim ser_sts As Boolean = False
                Dim Is_Approval As Boolean = False

                If ch.Checked = True Then
                    ser_sts = False
                    'If Val(txt_person_per.Text) = 0 And Val(txt_family_per.Text) = 0 And Val(txt_parent_per.Text) = 0 And CDec(txt_person_max.Text) = 0 And CDec(txt_family_max.Text) = 0 Then
                    '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('خطأ! تأكد من إدخال البيانات بشكل صحيح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                    '    Exit Sub
                    'End If

                    If CDec(txt_person_max.Text) > CDec(txt_family_max.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'خطأ! سقف الفرد يجب أن يكون أقل من سقف العائلة',
                showConfirmButton: false,
                timer: 1500
            });", True)
                        Exit Sub
                    End If

                    Dim clinic_no As Integer = 0
                    If ddl_show_type.SelectedValue = 1 Then
                        clinic_no = ddl_clinics.SelectedValue
                    Else
                        Dim sel_com As New SqlCommand("SELECT SubService_Clinic FROM Main_SubServices WHERE SubService_ID = " & dd.Cells(0).Text, insurance_SQLcon)
                        insurance_SQLcon.Close()
                        insurance_SQLcon.Open()
                        clinic_no = sel_com.ExecuteScalar
                        insurance_SQLcon.Close()
                    End If

                    If ch_Is_Approval.Checked = True Then
                        Is_Approval = False
                    Else
                        Is_Approval = True
                    End If

                    Dim insClinic As New SqlCommand
                    insClinic.Connection = insurance_SQLcon
                    insClinic.CommandText = "INC_addCompanySubServices"
                    insClinic.CommandType = CommandType.StoredProcedure
                    insClinic.Parameters.AddWithValue("@cID", ViewState("company_no"))
                    insClinic.Parameters.AddWithValue("@clinicID", clinic_no)
                    insClinic.Parameters.AddWithValue("@contractNo", ViewState("contract_no"))
                    insClinic.Parameters.AddWithValue("@subServiceId", dd.Cells(0).Text)
                    insClinic.Parameters.AddWithValue("@serPersonPer", Val(txt_person_per.Text))
                    insClinic.Parameters.AddWithValue("@serFamilyPer", Val(txt_family_per.Text))
                    insClinic.Parameters.AddWithValue("@serParentPer", Val(txt_parent_per.Text))
                    insClinic.Parameters.AddWithValue("@serPersonMax", CDec(txt_person_max.Text))
                    insClinic.Parameters.AddWithValue("@serFamilyMax", CDec(txt_family_max.Text))
                    insClinic.Parameters.AddWithValue("@serState", ser_sts)
                    insClinic.Parameters.AddWithValue("@serPaymentType", 0)
                    insClinic.Parameters.AddWithValue("@Is_Approval", Is_Approval)
                    insClinic.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
                    insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
                    insurance_SQLcon.Open()
                    insClinic.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                    insClinic.CommandText = ""

                Else
                    ser_sts = True
                    Dim stop_ser As New SqlCommand("UPDATE INC_SUB_SERVICES_RESTRICTIONS SET SER_STATE = 1 WHERE SubService_ID = " & dd.Cells(0).Text & " AND C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
                    insurance_SQLcon.Open()
                    stop_ser.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                End If
            Next
        End If

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية حفظ البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)

        getSubServicesAvailable()

    End Sub

    Sub getSubServicesAvailable()
        Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.Clinic_ID = INC_SUB_SERVICES_RESTRICTIONS.CLINIC_ID) AS CLINIC_NAME FROM INC_SUB_SERVICES_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no") & " GROUP BY Clinic_ID", insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            Dim resultString As String = ""
            Dim isFirstResult = True

            For i = 0 To dt_result.Rows.Count - 1
                Dim dr = dt_result.Rows(i)

                If Not isFirstResult Then
                    resultString &= String.Format(" <a href='#'><span class='badge badge-pill badge-info p-2 mt-2'>{0}</span></a>", dr!CLINIC_NAME)
                Else
                    isFirstResult = False
                    resultString &= String.Format("<a href='#'><span class='badge badge-pill badge-info p-2 mt-2'>{0}</span></a>", dr!CLINIC_NAME)
                End If
            Next

            'Literal1.Text = resultString
        Else
            ' Literal1.Text = "لا يوجد خدمات مغطاة"
        End If
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
            Dim txt_person_per As TextBox = dd.FindControl("txt_person_per")
            Dim txt_family_per As TextBox = dd.FindControl("txt_family_per")
            Dim txt_parent_per As TextBox = dd.FindControl("txt_parent_per")
            Dim txt_person_max As TextBox = dd.FindControl("txt_person_max")
            Dim txt_family_max As TextBox = dd.FindControl("txt_family_max")

            If ch.Checked = True Then
                If txt_person_per_all.Text <> "" Then
                    txt_person_per.Text = Val(txt_person_per_all.Text)
                End If
                If txt_family_per_all.Text <> "" Then
                    txt_family_per.Text = Val(txt_family_per_all.Text)
                End If
                If txt_parent_per_all.Text <> "" Then
                    txt_parent_per.Text = Val(txt_parent_per_all.Text)
                End If
                If txt_person_max_all.Text <> "" Then
                    txt_person_max.Text = CDec(txt_person_max_all.Text)
                End If
                If txt_family_max_all.Text <> "" Then
                    txt_family_max.Text = CDec(txt_family_max_all.Text)
                End If


            End If

        Next
    End Sub

    Private Sub ddl_services_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services.SelectedIndexChanged
        If ddl_services.SelectedValue = 0 Then
            getSubServices(1)
        Else
            getSubServices(2)
        End If
    End Sub

    Private Sub ddl_show_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_show_type.SelectedIndexChanged
        If ddl_show_type.SelectedValue = 1 Then
            clinic_Panel.Visible = True
            groups_Panel.Visible = False
        Else
            clinic_Panel.Visible = False
            groups_Panel.Visible = True

        End If
    End Sub

    Private Sub ddl_gourp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_gourp.SelectedIndexChanged
        Try
            Dim sel_com As New SqlCommand("SELECT 0 AS SubGroup_ID, 'الكل' AS SubGroup_ARname FROM Main_SubGroup UNION SELECT SubGroup_ID, SubGroup_ARname FROM Main_SubGroup WHERE SubGroup_State = 0 AND MainGroup_ID = " & ddl_gourp.SelectedValue, insurance_SQLcon)
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

        Dim search_by As String

        If ddl_services_group.SelectedValue = 0 Then
            search_by = "SubService_Group in (SELECT SubGroup_ID FROM Main_SubGroup WHERE MainGroup_ID = " & ddl_gourp.SelectedValue & ")"
        Else
            search_by = "SubService_Group = " & ddl_services_group.SelectedValue
        End If

        Dim sel_data As New SqlCommand("SELECT MAIN_SUBSERVICES.SUBSERVICE_ID, SUBSERVICE_CODE, SUBSERVICE_AR_NAME, SERVICE_STS, CLINIC_AR_NAME AS CLINIC_NAME, ISNULL(PERSON_PER, 1) AS PERSON_PER, ISNULL(FAMILY_PER, 1) AS FAMILY_PER, ISNULL(PARENT_PER, 1) AS PARENT_PER, ISNULL(MAX_PERSON_VAL, 1) AS MAX_PERSON_VAL, ISNULL(MAX_FAMILY_VAL,1) AS MAX_FAMILY_VAL, ISNULL(SER_STATE, 1) AS SER_STATE, ISNULL(IS_APPROVAL, 1) AS IS_APPROVAL FROM MAIN_SUBSERVICES
            LEFT JOIN MAIN_CLINIC ON MAIN_CLINIC.CLINIC_ID = MAIN_SUBSERVICES.SUBSERVICE_CLINIC
            LEFT JOIN INC_SERVICES_RESTRICTIONS ON INC_SERVICES_RESTRICTIONS.SERVICE_ID = MAIN_SUBSERVICES.SUBSERVICE_SERVICE_ID AND CONTRACT_NO = " & ViewState("contract_no") & "
            LEFT JOIN INC_SUB_SERVICES_RESTRICTIONS ON INC_SUB_SERVICES_RESTRICTIONS.SUBSERVICE_ID = MAIN_SUBSERVICES.SUBSERVICE_ID AND INC_SUB_SERVICES_RESTRICTIONS.C_ID = " & Session("company_id") & " AND INC_SUB_SERVICES_RESTRICTIONS.CONTRACT_NO = " & ViewState("contract_no") & "
            WHERE " & search_by, insurance_SQLcon)
        Dim dt_res As New DataTable
        dt_res.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_res.Load(sel_data.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_res.Rows.Count > 0 Then
            Panel1.Visible = True
            GridView1.DataSource = dt_res
            GridView1.DataBind()
            For i = 0 To dt_res.Rows.Count - 1
                Dim dd As GridViewRow = GridView1.Rows(i)

                Dim ch As CheckBox = dd.FindControl("CheckBox2")
                Dim ch_Is_Approval As CheckBox = dd.FindControl("CheckBox3")
                Dim txt_person_per As TextBox = dd.FindControl("txt_person_per")
                Dim txt_family_per As TextBox = dd.FindControl("txt_family_per")
                Dim txt_parent_per As TextBox = dd.FindControl("txt_parent_per")
                Dim txt_person_max As TextBox = dd.FindControl("txt_person_max")
                Dim txt_family_max As TextBox = dd.FindControl("txt_family_max")

                ch.Checked = If(dt_res.Rows(i)("SER_STATE") = 1, False, True)
                ch_Is_Approval.Checked = If(dt_res.Rows(i)("IS_APPROVAL") = True, False, True)

                txt_person_per.Text = dt_res.Rows(i)("PERSON_PER")
                txt_family_per.Text = dt_res.Rows(i)("FAMILY_PER")
                txt_parent_per.Text = dt_res.Rows(i)("PARENT_PER")
                txt_person_max.Text = dt_res.Rows(i)("MAX_PERSON_VAL")
                txt_family_max.Text = dt_res.Rows(i)("MAX_FAMILY_VAL")
            Next
        Else
            Panel1.Visible = False
            dt_res.Rows.Clear()
            GridView1.DataSource = dt_res
            GridView1.DataBind()
        End If
    End Sub
End Class