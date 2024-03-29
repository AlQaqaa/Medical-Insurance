﻿Imports System.Data.SqlClient
Public Class editClinc
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("company_services") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If

            If Session("company_id") = Nothing Then
                Response.Redirect("Default.aspx")
            End If

            ViewState("company_no") = Val(Session("company_id"))

            If ViewState("company_no") = 0 Then
                Response.Redirect("Default.aspx")
            End If

            Dim sel_com As New SqlCommand("SELECT C_id, (SELECT top 1 (DATE_END) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS DATE_END, (SELECT top 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS CONTRACT_NO, C_NAME_ARB, C_NAME_ENG FROM INC_COMPANY_DATA WHERE C_id = " & ViewState("company_no"), insurance_SQLcon)
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

            Dim clinic_no As Integer = Val(Request.QueryString("clinicId"))

            getClinicData()

        End If
    End Sub

    Sub getClinicData()

        Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS Clinic_AR_Name, (CASE WHEN (GROUP_NO) <> 0 THEN (SELECT MAX_VALUE FROM INC_CLINIC_GROUP WHERE INC_CLINIC_GROUP.GROUP_NO = INC_CLINICAL_RESTRICTIONS.GROUP_NO) ELSE (SELECT MAX_VALUE FROM INC_CLINICAL_RESTRICTIONS AS M_X WHERE M_X.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID AND M_X.C_ID = INC_CLINICAL_RESTRICTIONS.C_ID) END) AS MAX_VALUE, GROUP_NO, SESSION_COUNT FROM INC_CLINICAL_RESTRICTIONS WHERE Clinic_ID = " & Val(Request.QueryString("clinicId")) & " AND C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
        Dim dt_clinic As New DataTable
        dt_clinic.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_clinic.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_clinic.Rows.Count > 0 Then
            Dim dr_clinic = dt_clinic.Rows(0)
            txt_clinic_id.Text = dr_clinic!Clinic_ID
            txt_clini_name.Text = dr_clinic!Clinic_AR_Name
            Page.Title = dr_clinic!Clinic_AR_Name
            If dr_clinic!Clinic_ID <> 18 Then
                txt_max_val.Text = CDec(dr_clinic!MAX_VALUE)
                Label1.Text = "السقف العام"
            Else
                txt_max_val.Text = Val(dr_clinic!SESSION_COUNT)
                Label1.Text = "عدد الجلسات"
            End If
            ViewState("Clinic_ID") = dr_clinic!Clinic_ID
            ViewState("SESSION_COUNT") = dr_clinic!SESSION_COUNT
            ViewState("group_no") = dr_clinic!GROUP_NO
            If dr_clinic!GROUP_NO <> 0 Then
                lbl_info.Text = "هذه العيادة مشتركة في السقف العام مع العيادات التالية"
                btn_separat.Visible = True

                Dim sel_clinic_group As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS Clinic_AR_Name FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no") & " AND GROUP_NO = " & dr_clinic!GROUP_NO, insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_result.Load(sel_clinic_group.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_result.Rows.Count > 0 Then
                    ViewState("group_count") = dt_result.Rows.Count
                    Dim resultString As String = ""
                    Dim isFirstResult = True
                    For i = 0 To dt_result.Rows.Count - 1
                        Dim dr = dt_result.Rows(i)
                        If Not isFirstResult Then
                            resultString &= String.Format(" <a href='editClinic.aspx?clinicId=" & dr!Clinic_ID & "'><span class='badge badge-pill badge-info p-2 mb-2'>{0}</span></a>", dr!Clinic_AR_Name)
                        Else
                            isFirstResult = False
                            resultString &= String.Format("<a href='editClinic.aspx?clinicId=" & dr!Clinic_ID & "'><span class='badge badge-pill badge-info p-2 mb-2'>{0}</span></a>", dr!Clinic_AR_Name)
                        End If
                    Next
                    Literal1.Text = resultString
                Else
                    Literal1.Text = ""
                End If
            Else
                Literal1.Text = ""
                lbl_info.Text = ""
            End If
        End If
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        If ViewState("group_no") = 0 Then
            If ViewState("Clinic_ID") <> 18 Then
                Dim update_clinic As New SqlCommand("UPDATE INC_CLINICAL_RESTRICTIONS SET MAX_VALUE = @MAX_VALUE WHERE C_ID=@C_ID AND CLINIC_ID=@CLINIC_ID AND CONTRACT_NO=@CONTRACT_NO", insurance_SQLcon)
                update_clinic.Parameters.AddWithValue("@MAX_VALUE", CDec(txt_max_val.Text))
                update_clinic.Parameters.AddWithValue("@C_ID", ViewState("company_no"))
                update_clinic.Parameters.AddWithValue("@CLINIC_ID", Val(txt_clinic_id.Text))
                update_clinic.Parameters.AddWithValue("@CONTRACT_NO", ViewState("contract_no"))
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                update_clinic.ExecuteNonQuery()
                insurance_SQLcon.Close()

                add_action(1, 3, 2, "تعديل السقف العام للعيادة رقم: " & Val(txt_clinic_id.Text) & " للشركة رقم " & ViewState("company_no") & " عقد رقم: " & ViewState("contract_no"), Session("INC_User_Id"), GetIPAddress())
            Else
                ' في حالة كانت العيادة علاج طبيعي يتم تعديل عدد الجلسات وليس السقف
                Dim update_clinic As New SqlCommand("UPDATE INC_CLINICAL_RESTRICTIONS SET SESSION_COUNT = @SESSION_COUNT WHERE C_ID=@C_ID AND CLINIC_ID=@CLINIC_ID AND CONTRACT_NO=@CONTRACT_NO", insurance_SQLcon)
                update_clinic.Parameters.AddWithValue("@SESSION_COUNT", Val(txt_max_val.Text))
                update_clinic.Parameters.AddWithValue("@C_ID", ViewState("company_no"))
                update_clinic.Parameters.AddWithValue("@CLINIC_ID", Val(txt_clinic_id.Text))
                update_clinic.Parameters.AddWithValue("@CONTRACT_NO", ViewState("contract_no"))
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                update_clinic.ExecuteNonQuery()
                insurance_SQLcon.Close()

                add_action(1, 3, 2, "تعديل عدد الجلسات للعيادة رقم: " & Val(txt_clinic_id.Text) & " للشركة رقم " & ViewState("company_no") & " عقد رقم: " & ViewState("contract_no"), Session("INC_User_Id"), GetIPAddress())
            End If


            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية تعديل البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

            getClinicData()

        Else
            Dim update_clinic As New SqlCommand("UPDATE INC_CLINIC_GROUP SET max_value=@max_value WHERE group_no=@group_no", insurance_SQLcon)
            update_clinic.Parameters.AddWithValue("@max_value", CDec(txt_max_val.Text))
            update_clinic.Parameters.AddWithValue("@group_no", ViewState("group_no"))
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            update_clinic.ExecuteNonQuery()
            insurance_SQLcon.Close()

            add_action(1, 3, 2, "تعديل السقف العام لمجموعة عيادات رقم المجموعة: " & ViewState("group_no") & " للشركة رقم " & ViewState("company_no") & " عقد رقم: " & ViewState("contract_no"), Session("INC_User_Id"), GetIPAddress())

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية تعديل البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)

            getClinicData()

        End If
    End Sub

    Private Sub btn_separat_Click(sender As Object, e As EventArgs) Handles btn_separat.Click
        Try
            Dim sql_str As String = ""
            If ViewState("group_count") > 2 Then
                Dim update_clinic As New SqlCommand("UPDATE INC_CLINICAL_RESTRICTIONS SET MAX_VALUE=@MAX_VALUE, GROUP_NO=0 WHERE C_ID=@C_ID AND CLINIC_ID=@CLINIC_ID AND CONTRACT_NO=@CONTRACT_NO", insurance_SQLcon)
                update_clinic.Parameters.AddWithValue("@MAX_VALUE", CDec(txt_max_val.Text))
                update_clinic.Parameters.AddWithValue("@C_ID", ViewState("company_no"))
                update_clinic.Parameters.AddWithValue("@CLINIC_ID", Val(txt_clinic_id.Text))
                update_clinic.Parameters.AddWithValue("@CONTRACT_NO", ViewState("contract_no"))
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                update_clinic.ExecuteNonQuery()
                insurance_SQLcon.Close()
            ElseIf ViewState("group_count") = 2 Then
                Dim update_clinic As New SqlCommand("UPDATE INC_CLINICAL_RESTRICTIONS SET MAX_VALUE=@MAX_VALUE, GROUP_NO=0 WHERE C_ID=@C_ID AND GROUP_NO=@GROUP_NO AND CONTRACT_NO=@CONTRACT_NO", insurance_SQLcon)
                update_clinic.Parameters.AddWithValue("@MAX_VALUE", CDec(txt_max_val.Text))
                update_clinic.Parameters.AddWithValue("@C_ID", ViewState("company_no"))
                update_clinic.Parameters.AddWithValue("@GROUP_NO", Val(ViewState("group_no")))
                update_clinic.Parameters.AddWithValue("@CONTRACT_NO", ViewState("contract_no"))
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                update_clinic.ExecuteNonQuery()
                insurance_SQLcon.Close()
            End If

            add_action(1, 3, 2, "تعديل السقف العام للعيادة رقم: " & Val(txt_clinic_id.Text) & " وفصلها عن المجموعة رقم: " & ViewState("group_no") & " للشركة رقم " & ViewState("company_no") & " عقد رقم: " & ViewState("contract_no"), Session("INC_User_Id"), GetIPAddress())

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية تعديل البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)

            getClinicData()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class