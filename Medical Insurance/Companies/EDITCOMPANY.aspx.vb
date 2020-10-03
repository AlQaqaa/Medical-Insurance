Imports System.Data.SqlClient
Public Class EDITCOMPANY
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

            getData()
        End If

    End Sub

    Private Sub txt_company_name_ar_TextChanged(sender As Object, e As EventArgs) Handles txt_company_name_ar.TextChanged
        txt_company_name_en.Text = TransA2E(txt_company_name_ar.Text)
        txt_company_name_en.Focus()
    End Sub


    Sub clrTxt()
        txt_company_name_ar.Text = ""
        txt_company_name_en.Text = ""
        txt_end_dt.Text = ""
        txt_max_card_value.Text = ""
        txt_max_company_value.Text = ""
        txt_start_dt.Text = ""

    End Sub

    Protected Sub getData()
        Dim s1 As String = " (SELECT top 1 CONVERT(VARCHAR, DATE_START, 111) AS DATE_START FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS DATE_STARt"
        Dim s2 As String = " (SELECT top 1 CONVERT(VARCHAR, DATE_END, 111) AS DATE_END FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS DATE_END"
        Dim s3 As String = " (SELECT top 1 (MAX_VAL) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS MAX_VAL"
        Dim s4 As String = " (SELECT top 1 (MAX_CARD) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS MAX_CARD"
        Dim s5 As String = " (SELECT top 1 (PATIAINT_PER) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS PATIAINT_PER"
        Dim s6 As String = " (SELECT top 1 (PYMENT_TYPE) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS PYMENT_TYPE"
        Dim s7 As String = " (SELECT top 1 (PROFILE_PRICE_ID) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS PROFILE_PRICE_ID"
        Dim s8 As String = " (SELECT top 1 (MAX_PERSON) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS MAX_PERSON"
        Dim s9 As String = " (SELECT top 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS CONTRACT_NO"


        Dim ins_com1 As New SqlCommand("SELECT C_NAME_ARB, C_NAME_ENG, " & s1 & ", " & s2 & ", " & s3 & ", " & s4 & ", " & s5 & ", " & s6 & ", " & s7 & ", " & s8 & ", " & s9 & "  FROM INC_COMPANY_DATA WHERE C_ID = @C_id", insurance_SQLcon)


        ins_com1.Parameters.AddWithValue("@C_id", Val(Session("company_id")))
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()

        Dim rd1 As SqlDataReader = ins_com1.ExecuteReader

        If rd1.Read Then
            If IsDBNull(rd1!DATE_START) Then

                txt_start_dt.Text = ""
            Else
                txt_start_dt.Text = rd1!DATE_START
            End If
            If IsDBNull(rd1!DATE_END) Then

                txt_end_dt.Text = ""
            Else
                txt_end_dt.Text = rd1!DATE_END
            End If
            txt_company_name_ar.Text = rd1!C_NAME_ARB
            txt_company_name_en.Text = rd1!C_NAME_ENG
            If IsDBNull(rd1!MAX_VAL) Then
                txt_max_company_value.Text = ""
            Else
                txt_max_company_value.Text = rd1!MAX_VAL
            End If

            txt_max_card_value.Text = rd1!MAX_CARD
            txt_max_person.Text = rd1!MAX_PERSON
            ddl_PATIAINT_PER.SelectedValue = rd1!PATIAINT_PER
            ddl_payment_type.SelectedValue = rd1!PYMENT_TYPE
            ViewState("contract_no") = rd1!CONTRACT_NO

        End If
        insurance_SQLcon.Close()
    End Sub

    Protected Sub btn_edit_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        Try
            Dim insToCompany As New SqlCommand
            insToCompany.Connection = insurance_SQLcon
            insToCompany.CommandText = "INC_editCompany"
            insToCompany.CommandType = CommandType.StoredProcedure
            insToCompany.Parameters.AddWithValue("@cId", Val(Session("company_id")))
            insToCompany.Parameters.AddWithValue("@contractNo", Val(ViewState("contract_no")))
            insToCompany.Parameters.AddWithValue("@cNameAr", txt_company_name_ar.Text)
            insToCompany.Parameters.AddWithValue("@cNameEn", txt_company_name_en.Text)
            insToCompany.Parameters.AddWithValue("@startDt", DateTime.Parse(txt_start_dt.Text))
            insToCompany.Parameters.AddWithValue("@endDt", DateTime.Parse(txt_end_dt.Text))
            insToCompany.Parameters.AddWithValue("@maxVal", CDec(txt_max_company_value.Text))
            insToCompany.Parameters.AddWithValue("@maxCard", CDec(txt_max_card_value.Text))
            insToCompany.Parameters.AddWithValue("@maxPerson", CDec(txt_max_person.Text))
            insToCompany.Parameters.AddWithValue("@paymentType", ddl_payment_type.SelectedValue)
            insToCompany.Parameters.AddWithValue("@patiaintPer", ddl_PATIAINT_PER.SelectedValue)
            insToCompany.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
            insToCompany.Parameters.AddWithValue("@userIp", GetIPAddress())
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()

            insToCompany.ExecuteNonQuery()
            insurance_SQLcon.Close()
            clrTxt()
            getData()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية تعديل البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class