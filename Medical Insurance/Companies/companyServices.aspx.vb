Imports System.Data.SqlClient

Public Class companyServices
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
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
            getServicesAvailable()
        End If
    End Sub

    Sub getClinicAvailable()
        Dim sel_com As New SqlCommand("SELECT 0 AS CLINIC_ID, '' AS CLINIC_NAME FROM INC_CLINICAL_RESTRICTIONS UNION SELECT CLINIC_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.Clinic_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS CLINIC_NAME FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
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
    Sub getServicesAvailable()
        Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.Clinic_ID = INC_SERVICES_RESTRICTIONS.CLINIC_ID) AS CLINIC_NAME FROM INC_SERVICES_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no") & " GROUP BY Clinic_ID", insurance_SQLcon)
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

            Literal1.Text = resultString
        Else
            Literal1.Text = "لا يوجد خدمات مغطاة"
        End If
    End Sub


    Sub getServices()

        Dim MAX_VALUE As String = "ISNULL((SELECT MAX_VALUE FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.Service_ID = Main_Services.Service_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS MAX_VALUE,"
        Dim SER_STATE As String = "ISNULL((SELECT SERVICE_STS FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.Service_ID = Main_Services.Service_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS SER_STATE"
        

        Dim sel_data As New SqlCommand("select Service_ID, Service_AR_Name, " & MAX_VALUE & SER_STATE & " from Main_Services WHERE Service_Clinic = " & ddl_clinics.SelectedValue, insurance_SQLcon)
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
                Dim txt_max_val As TextBox = dd.FindControl("txt_max_val")

                If IsDBNull(dt_res.Rows(i)("SER_STATE")) Then
                    ch.Checked = True
                ElseIf dt_res.Rows(i)("SER_STATE") = 1 Then
                    ch.Checked = False
                Else
                    ch.Checked = True
                End If
                txt_max_val.Text = dt_res.Rows(i)("MAX_VALUE")
            Next
        Else
            Panel1.Visible = False
            dt_res.Rows.Clear()
            GridView1.DataSource = dt_res
            GridView1.DataBind()
        End If
    End Sub

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        getServices()
        txt_clinics_max.Text = getMaxClinicValue(ddl_clinics.SelectedValue, ViewState("company_no"), ViewState("contract_no"))
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")
            Dim txt_max_val As TextBox = dd.FindControl("txt_max_val")

            Dim ser_sts As Boolean = False

            If ch.Checked = True Then
                ser_sts = False
                'If Val(txt_person_per.Text) = 0 And Val(txt_family_per.Text) = 0 And Val(txt_parent_per.Text) = 0 And CDec(txt_person_max.Text) = 0 And CDec(txt_family_max.Text) = 0 Then
                '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('خطأ! تأكد من إدخال البيانات بشكل صحيح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                '    Exit Sub
                'End If

                Dim insClinic As New SqlCommand
                insClinic.Connection = insurance_SQLcon
                insClinic.CommandText = "INC_addCompanyServices"
                insClinic.CommandType = CommandType.StoredProcedure
                insClinic.Parameters.AddWithValue("@cID", ViewState("company_no"))
                insClinic.Parameters.AddWithValue("@clinicID", ddl_clinics.SelectedValue)
                insClinic.Parameters.AddWithValue("@serviceId", dd.Cells(0).Text)
                insClinic.Parameters.AddWithValue("@contractNo", ViewState("contract_no"))
                insClinic.Parameters.AddWithValue("@maxServiceValue", CDec(txt_max_val.Text))
                insClinic.Parameters.AddWithValue("@serviceSts", ser_sts)
                insClinic.Parameters.AddWithValue("@userId", 1)
                insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                insClinic.ExecuteNonQuery()
                insurance_SQLcon.Close()
                insClinic.CommandText = ""

            Else
                ser_sts = True
                Dim stop_ser As New SqlCommand("UPDATE INC_SERVICES_RESTRICTIONS SET SERVICE_STS = 1 WHERE Service_ID = " & dd.Cells(0).Text & " AND C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
                insurance_SQLcon.Open()
                stop_ser.ExecuteNonQuery()
                insurance_SQLcon.Close()
            End If
        Next
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

        getServicesAvailable()

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
            Dim txt_max_val As TextBox = dd.FindControl("txt_max_val")

            If ch.Checked = True Then
                txt_max_val.Text = CDec(txt_max_value_all.Text)

            End If

        Next
    End Sub

End Class