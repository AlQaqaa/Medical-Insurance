﻿Imports System.Data.SqlClient
Imports System.Globalization

Public Class addNewContract
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("active_company") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If

            Dim company_no As Integer = Val(Session("company_id"))

            If company_no = 0 Then
                Response.Redirect("Default.aspx", False)
            End If

        End If
    End Sub

    Sub clrTxt()

        txt_end_dt.Text = ""
        txt_max_card_value.Text = ""
        txt_max_company_value.Text = ""
        txt_start_dt.Text = ""

    End Sub

    Protected Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        Try
            Dim sel_data As New SqlCommand("SELECT * FROM INC_COMPANY_DETIAL WHERE C_ID = " & Session("company_id") & " AND DATE_END >= '" & DateTime.Parse(txt_start_dt.Text) & "'", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_data.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count = 0 Then
                Dim insToCompany As New SqlCommand
                insToCompany.Connection = insurance_SQLcon
                insToCompany.CommandText = "INC_addNewContract"
                insToCompany.CommandType = CommandType.StoredProcedure
                insToCompany.Parameters.AddWithValue("@cid", Val(Session("company_id")))
                insToCompany.Parameters.AddWithValue("@startDt", DateTime.Parse(txt_start_dt.Text))
                insToCompany.Parameters.AddWithValue("@endDt", DateTime.Parse(txt_end_dt.Text))
                insToCompany.Parameters.AddWithValue("@maxVal", CDec(txt_max_company_value.Text))
                insToCompany.Parameters.AddWithValue("@maxCard", CDec(txt_max_card_value.Text))
                insToCompany.Parameters.AddWithValue("@maxPerson", CDec(txt_max_person.Text))
                insToCompany.Parameters.AddWithValue("@paymentType", ddl_payment_type.SelectedValue)
                insToCompany.Parameters.AddWithValue("@contractType", ddl_contractType.SelectedValue) ' 3تمدبد2.. تجدبد
                insToCompany.Parameters.AddWithValue("@patiaintPer", ddl_PATIAINT_PER.SelectedValue)
                insToCompany.Parameters.AddWithValue("@profile_price_id", ddl_profiles_prices.SelectedValue)
                insToCompany.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
                insToCompany.Parameters.AddWithValue("@userIp", GetIPAddress())
                insToCompany.Parameters.AddWithValue("@max_one_processes", CDec(txt_max_one_processes.Text))
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                insToCompany.ExecuteNonQuery()
                insurance_SQLcon.Close()

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية حفظ البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
                clrTxt()
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'خطأ هناك عقد ساري لهذه الشركة بنفس الفترة التي اخترتها',
                showConfirmButton: false,
                timer: 1500
            });", True)

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class