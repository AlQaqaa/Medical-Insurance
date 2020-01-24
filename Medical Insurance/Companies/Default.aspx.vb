Imports System.Data.SqlClient
Public Class _Default3
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            ViewState("company_no") = Val(Session("company_id"))

            If ViewState("company_no") = 0 Then
                Response.Redirect("../Default.aspx")
            End If

            Dim sel_com As New SqlCommand("SELECT (SELECT TOP 1 (CONVERT(VARCHAR, DATE_START, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_START, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_END, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_END, (SELECT TOP 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY CONTRACT_NO DESC) AS CONTRACT_NO, C_NAME_ARB, C_NAME_ENG, C_STATE, (CASE WHEN (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL ) IS NULL THEN  '-' ELSE (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL) END)AS MAIN_COMPANY FROM INC_COMPANY_DATA WHERE C_ID = " & ViewState("company_no"), insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Dim dr_company = dt_result.Rows(0)
                lbl_company_name.Text = dr_company!C_NAME_ARB
                lbl_en_name.Text = dr_company!C_NAME_ENG
                lbl_start_dt.Text = dr_company!DATE_START
                lbl_end_dt.Text = dr_company!DATE_END
                If dr_company!C_STATE = 0 Then
                    lbl_company_sts.Text = "نشط"
                    lbl_company_sts.CssClass = "badge badge-success"
                Else
                    lbl_company_sts.Text = "موقوف"
                    lbl_company_sts.CssClass = "badge badge-danger"
                End If
                lbl_main_company.Text = dr_company!MAIN_COMPANY
                ViewState("contract_no") = dr_company!CONTRACT_NO
            End If

            ' جلب عدد المستخدمين
            Dim pats_count As New SqlCommand("SELECT COUNT(*) FROM INC_PATIANT WHERE C_ID = " & ViewState("company_no"), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            lbl_pats_count.Text = pats_count.ExecuteScalar
            insurance_SQLcon.Close()

            getClinicAvailable()
            getBlockDoctors()

        End If
    End Sub

    Sub getClinicAvailable()
        Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS Clinic_AR_Name, (CASE WHEN (GROUP_NO) <> 0 THEN (SELECT MAX_VALUE FROM INC_CLINIC_GROUP WHERE INC_CLINIC_GROUP.GROUP_NO = INC_CLINICAL_RESTRICTIONS.GROUP_NO) ELSE (SELECT MAX_VALUE FROM INC_CLINICAL_RESTRICTIONS AS M_X WHERE M_X.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID AND M_X.C_ID = INC_CLINICAL_RESTRICTIONS.C_ID) END) AS MAX_VALUE, (CASE WHEN (GROUP_NO) <> 0 THEN (SELECT PER_T FROM INC_CLINIC_GROUP WHERE INC_CLINIC_GROUP.GROUP_NO = INC_CLINICAL_RESTRICTIONS.GROUP_NO) ELSE (SELECT PER_T FROM INC_CLINICAL_RESTRICTIONS AS M_X WHERE M_X.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID AND M_X.C_ID = INC_CLINICAL_RESTRICTIONS.C_ID) END) AS PER_T, (CASE WHEN GROUP_NO = 0 THEN '-' ELSE 'مشتركة' END) AS GROUP_CLINIC FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView1.DataSource = dt_result
            GridView1.DataBind()
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Stop Clinic ################
        If (e.CommandName = "stop_block") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim del_com As New SqlCommand("DELETE FROM INC_CLINICAL_RESTRICTIONS WHERE CLINIC_ID = " & (row.Cells(0).Text) & " AND C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            del_com.ExecuteNonQuery()
            insurance_SQLcon.Close()

            Dim edit_sts As New SqlCommand("UPDATE INC_SUB_SERVICES_RESTRICTIONS SET SER_STATE=1 WHERE CLINIC_ID = " & (row.Cells(0).Text) & " AND C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            edit_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت العملية بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

        End If
    End Sub

    Private Sub btn_ban_doctor_Click(sender As Object, e As EventArgs) Handles btn_ban_doctor.Click
        Try
            Dim ins_com As New SqlCommand("INSERT INTO INC_BLOCK_SERVICES (OBJECT_ID, SER_ID, BLOCK_TP, NOTES, USER_ID, USER_IP) VALUES (@OBJECT_ID, @SER_ID, @BLOCK_TP, @NOTES, @USER_ID, @USER_IP)", insurance_SQLcon)
            ins_com.Parameters.Add("@OBJECT_ID", SqlDbType.Int).Value = Val(Session("company_id"))
            ins_com.Parameters.Add("@SER_ID", SqlDbType.Int).Value = ddl_services.SelectedValue
            ins_com.Parameters.Add("@BLOCK_TP", SqlDbType.Int).Value = 2 ' Block Doctor
            ins_com.Parameters.Add("@NOTES", SqlDbType.NVarChar).Value = txt_notes.Text
            ins_com.Parameters.Add("@USER_ID", SqlDbType.Int).Value = 1
            ins_com.Parameters.Add("@USER_IP", SqlDbType.NVarChar).Value = GetIPAddress()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ins_com.ExecuteNonQuery()
            insurance_SQLcon.Close()
            getBlockDoctors()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حظر الطبيب عن هذه الشركة بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub getBlockDoctors()
        Dim get_com As New SqlCommand("SELECT SER_ID, (SELECT MedicalStaff_AR_Name  FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_BLOCK_SERVICES.SER_ID) AS MedicalStaff_AR_Name, NOTES FROM INC_BLOCK_SERVICES WHERE OBJECT_ID = " & Val(Session("company_id")) & " AND BLOCK_TP = 2", insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(get_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView2.DataSource = dt_result
            GridView2.DataBind()
        Else
            dt_result.Rows.Clear()
            GridView2.DataBind()
            Label1.Text = "لا يوجد أطباء محظورين عن هذه الشركة"
        End If
    End Sub

    Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand
        '################ When User Press On Stop Block ################
        If (e.CommandName = "stop_block_doctor") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView2.Rows(index)

            Dim del_com As New SqlCommand("DELETE FROM INC_BLOCK_SERVICES WHERE OBJECT_ID = " & Val(Session("company_id")) & " AND SER_ID = " & (row.Cells(0).Text) & " AND BLOCK_TP = 2", insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            del_com.ExecuteNonQuery()
            insurance_SQLcon.Close()
            getBlockDoctors()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية فك حظر الطبيب عن هذه الشركة بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

        End If
    End Sub
End Class