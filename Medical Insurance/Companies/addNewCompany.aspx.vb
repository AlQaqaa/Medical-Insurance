﻿Imports System.Data.SqlClient
Imports System.Globalization

Public Class addNewCompany
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("active_company") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If
        End If

    End Sub

    Sub clrTxt()
        txt_company_name_ar.Text = ""
        txt_company_name_en.Text = ""
        txt_end_dt.Text = ""
        txt_max_card_value.Text = ""
        txt_max_company_value.Text = ""
        txt_start_dt.Text = ""

    End Sub

    Private Sub ddl_company_level_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_company_level.SelectedIndexChanged
        If ddl_company_level.SelectedValue = 2 Then
            main_company_panel.Visible = True
        Else
            main_company_panel.Visible = False
        End If
    End Sub

    Protected Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        ' Check Company Level
        Dim company_level As Integer
        If ddl_company_level.SelectedValue = 1 Then ' If Company is Level one then C_level field = 0 else C_level field = C_ID
            company_level = 0
        Else
            company_level = ddl_companies.SelectedValue
        End If

        Try
            Dim insToCompany As New SqlCommand
            insToCompany.Connection = insurance_SQLcon
            insToCompany.CommandText = "INC_addNewCompany"
            insToCompany.CommandType = CommandType.StoredProcedure
            insToCompany.Parameters.AddWithValue("@cNameAr", txt_company_name_ar.Text)
            insToCompany.Parameters.AddWithValue("@cNameEn", txt_company_name_en.Text)
            insToCompany.Parameters.AddWithValue("@cState", False)
            insToCompany.Parameters.AddWithValue("@c_level", company_level)
            insToCompany.Parameters.AddWithValue("@startDt", DateTime.Parse(txt_start_dt.Text))
            insToCompany.Parameters.AddWithValue("@endDt", DateTime.Parse(txt_end_dt.Text))
            insToCompany.Parameters.AddWithValue("@maxVal", CDec(Val(txt_max_company_value.Text)))
            insToCompany.Parameters.AddWithValue("@maxCard", CDec(Val(txt_max_card_value.Text)))
            insToCompany.Parameters.AddWithValue("@maxPerson", CDec(Val(txt_max_person.Text)))
            insToCompany.Parameters.AddWithValue("@paymentType", ddl_payment_type.SelectedValue)
            insToCompany.Parameters.AddWithValue("@contractType", 1) ' 1 = New Contract
            insToCompany.Parameters.AddWithValue("@patiaintPer", ddl_PATIAINT_PER.SelectedValue)
            insToCompany.Parameters.AddWithValue("@bic_pic", "images/ImageCompany/UnknownUser.png")
            insToCompany.Parameters.AddWithValue("@profile_price_id", ddl_profiles_prices.SelectedValue)
            insToCompany.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
            insToCompany.Parameters.AddWithValue("@userIp", GetIPAddress())
            insToCompany.Parameters.AddWithValue("@max_one_processes", CDec(txt_max_one_processes.Text))
            insToCompany.Parameters.AddWithValue("@empMax", CDec(txtEmpMax.Text))
            insurance_SQLcon.Open()
            insToCompany.ExecuteNonQuery()
            insurance_SQLcon.Close()
            clrTxt()

            add_action(1, 1, 1, "إضافة شركة جديدة: " & txt_company_name_ar.Text, Session("INC_User_Id"), GetIPAddress())

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية حفظ البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txt_company_name_ar_TextChanged(sender As Object, e As EventArgs) Handles txt_company_name_ar.TextChanged
        txt_company_name_en.Text = TransA2E(txt_company_name_ar.Text)
        txt_company_name_en.Focus()
    End Sub
End Class