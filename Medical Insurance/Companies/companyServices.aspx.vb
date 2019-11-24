Imports System.Data.SqlClient
Public Class companyServices
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim company_no As Integer = Val(Session("company_id"))

            If company_no = 0 Then
                Response.Redirect("Default.aspx", False)
            End If

            ViewState("company_no") = company_no

            Dim sel_com As New SqlCommand("SELECT (SELECT top 1 (DATE_END) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS DATE_END, (SELECT top 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID) AS CONTRACT_NO, C_NAME_ARB FROM INC_COMPANY_DATA WHERE C_id = " & company_no, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Dim dr_company = dt_result.Rows(0)
                lbl_company_name.Text = dr_company!C_NAME_ARB
                ViewState("contract_no") = dr_company!CONTRACT_NO
            End If

            getClinicAvailable()

        End If
    End Sub

    Sub getClinicAvailable()
        Dim sel_com As New SqlCommand("SELECT CLINIC_ID, (SELECT CLINICNAME_AR FROM MAIN_CLINIC WHERE MAIN_CLINIC.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS CLINIC_NAME FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
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

    Sub getServices()

        Dim PERSON_PER As String = "ISNULL((SELECT PERSON_PER FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.SER_ID = MAIN_SERVIES.SERV_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS PERSON_PER,"
        Dim FAMILY_PER As String = "ISNULL((SELECT FAMILY_PER FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.SER_ID = MAIN_SERVIES.SERV_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS FAMILY_PER,"
        Dim PARENT_PER As String = "ISNULL((SELECT PARENT_PER FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.SER_ID = MAIN_SERVIES.SERV_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS PARENT_PER,"
        Dim MAX_PERSON_VAL As String = "ISNULL((SELECT MAX_PERSON_VAL FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.SER_ID = MAIN_SERVIES.SERV_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS MAX_PERSON_VAL,"
        Dim MAX_FAMILY_VAL As String = "ISNULL((SELECT MAX_FAMILY_VAL FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.SER_ID = MAIN_SERVIES.SERV_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS MAX_FAMILY_VAL,"
        Dim SER_STATE As String = "ISNULL((SELECT SER_STATE FROM INC_SERVICES_RESTRICTIONS WHERE INC_SERVICES_RESTRICTIONS.SER_ID = MAIN_SERVIES.SERV_ID AND CONTRACT_NO = " & ViewState("contract_no") & "), 0) AS SER_STATE"

        Dim sel_data As New SqlCommand("select SERV_ID, SERV_CODE, SERV_NAMEARB, " & PERSON_PER & FAMILY_PER & PARENT_PER & MAX_PERSON_VAL & MAX_FAMILY_VAL & SER_STATE & " from MAIN_SERVIES WHERE CLINIC_ID = " & ddl_clinics.SelectedValue, insurance_SQLcon)
        Dim dt_res As New DataTable
        dt_res.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_res.Load(sel_data.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_res.Rows.Count > 0 Then
            GridView1.DataSource = dt_res
            GridView1.DataBind()
            For i = 0 To dt_res.Rows.Count - 1
                Dim dd As GridViewRow = GridView1.Rows(i)

                Dim ch As CheckBox = dd.FindControl("CheckBox1")
                Dim txt_person_per As TextBox = dd.FindControl("txt_person_per")
                Dim txt_family_per As TextBox = dd.FindControl("txt_family_per")
                Dim txt_parent_per As TextBox = dd.FindControl("txt_parent_per")
                Dim txt_person_max As TextBox = dd.FindControl("txt_person_max")
                Dim txt_family_max As TextBox = dd.FindControl("txt_family_max")

                If dt_res.Rows(i)("SER_STATE") <> 0 Then
                    ch.Checked = True
                Else
                    ch.Checked = False
                End If
                txt_person_per.Text = dt_res.Rows(i)("PERSON_PER")
                txt_family_per.Text = dt_res.Rows(i)("FAMILY_PER")
                txt_parent_per.Text = dt_res.Rows(i)("PARENT_PER")
                txt_person_max.Text = dt_res.Rows(i)("MAX_PERSON_VAL")
                txt_family_max.Text = dt_res.Rows(i)("MAX_FAMILY_VAL")
            Next
        Else
            dt_res.Rows.Clear()
            GridView1.DataSource = dt_res
            GridView1.DataBind()
        End If
    End Sub

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        getServices()
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox1")
            Dim txt_person_per As TextBox = dd.FindControl("txt_person_per")
            Dim txt_family_per As TextBox = dd.FindControl("txt_family_per")
            Dim txt_parent_per As TextBox = dd.FindControl("txt_parent_per")
            Dim txt_person_max As TextBox = dd.FindControl("txt_person_max")
            Dim txt_family_max As TextBox = dd.FindControl("txt_family_max")

            'If ch.Checked = True Then
            Dim insClinic As New SqlCommand
            insClinic.Connection = insurance_SQLcon
            insClinic.CommandText = "INC_addCompanyServices"
            insClinic.CommandType = CommandType.StoredProcedure
            insClinic.Parameters.AddWithValue("@cID", ViewState("company_no"))
            insClinic.Parameters.AddWithValue("@clinicID", ddl_clinics.SelectedValue)
            insClinic.Parameters.AddWithValue("@contractNo", ViewState("contract_no"))
            insClinic.Parameters.AddWithValue("@serviceId", dd.Cells(0).Text)
            insClinic.Parameters.AddWithValue("@serPersonPer", Val(txt_person_per.Text))
            insClinic.Parameters.AddWithValue("@serFamilyPer", Val(txt_family_per.Text))
            insClinic.Parameters.AddWithValue("@serParentPer", Val(txt_parent_per.Text))
            insClinic.Parameters.AddWithValue("@serPersonMax", CDec(txt_person_max.Text))
            insClinic.Parameters.AddWithValue("@serFamilyMax", CDec(txt_family_max.Text))
            insClinic.Parameters.AddWithValue("@serState", ch.Checked)
            insClinic.Parameters.AddWithValue("@serPaymentType", 1)
            insClinic.Parameters.AddWithValue("@userId", 1)
            insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
            insurance_SQLcon.Open()
            insClinic.ExecuteNonQuery()
            insurance_SQLcon.Close()
            insClinic.CommandText = ""
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

            '  End If
        Next
    End Sub
End Class