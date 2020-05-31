Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Public Class INC_PATIANT
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim company_no As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            company_no = Val(Session("company_id"))

            If company_no = 0 Then
                Response.Redirect("Default.aspx")
            End If

            Dim sel_com As New SqlCommand("SELECT C_NAME_ARB FROM INC_COMPANY_DATA WHERE C_id = " & company_no, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Dim dr_company = dt_result.Rows(0)
                Session("lb") = dr_company!C_NAME_ARB
                Session("lb1") = company_no
            End If
        End If
    End Sub

    Sub clrTxt()
        txt_BAGE_NO.Text = ""
        txt_BIRTHDATE.Text = ""
        txt_CARD_NO.Text = ""
        txt_exp_date.Text = ""
        txt_KID_NO.Text = ""
        txt_NAME_ARB.Text = ""
        txt_NAME_ENG.Text = ""
        txt_NAT_NUMBER.Text = ""
        txt_NOTES.Text = ""
        txt_PHONE_NO.Text = ""
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        Try

            Dim sel_com As New SqlCommand("SELECT * FROM INC_PATIANT WHERE CARD_NO = @CARD_NO AND BAGE_NO = @BAGE_NO AND C_ID = @C_ID AND CONST_ID = 0", insurance_SQLcon)
            sel_com.Parameters.AddWithValue("@CARD_NO", txt_CARD_NO.Text)
            sel_com.Parameters.AddWithValue("@BAGE_NO", txt_BAGE_NO.Text)
            sel_com.Parameters.AddWithValue("@C_ID", Val(Session("company_id")))
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'خطا! هذا المشترك تمت إضافته مسبقاً',
                showConfirmButton: false,
                timer: 1500
            });", True)
                Exit Sub
            End If

            Dim dob As String = DateTime.ParseExact(txt_BIRTHDATE.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
            Dim exp As String = DateTime.ParseExact(txt_exp_date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

            Dim ins_PAT As New SqlCommand
            ins_PAT.Connection = insurance_SQLcon
            ins_PAT.CommandText = "INC_addNewpatiant"
            ins_PAT.CommandType = CommandType.StoredProcedure
            ins_PAT.Parameters.AddWithValue("@CARD_NO", txt_CARD_NO.Text)
            ins_PAT.Parameters.AddWithValue("@NAME_ARB", txt_NAME_ARB.Text)
            ins_PAT.Parameters.AddWithValue("@NAME_ENG", txt_NAME_ENG.Text)
            ins_PAT.Parameters.AddWithValue("@BIRTHDATE", dob)
            ins_PAT.Parameters.AddWithValue("@BAGE_NO", txt_BAGE_NO.Text)
            ins_PAT.Parameters.AddWithValue("@C_ID", Val(Session("company_id")))
            ins_PAT.Parameters.AddWithValue("@GENDER", ddl_GENDER.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@NAL_ID", ddl_NAL_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@PHONE_NO", Val(txt_PHONE_NO.Text))
            ins_PAT.Parameters.AddWithValue("@CONST_ID", CONST_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@EXP_DATE", exp)
            ins_PAT.Parameters.AddWithValue("@NOTES", txt_NOTES.Text)
            ins_PAT.Parameters.AddWithValue("@P_STATE", P_STATE.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@NAT_NUMBER", Val(txt_NAT_NUMBER.Text))
            ins_PAT.Parameters.AddWithValue("@KID_NO", Val(txt_KID_NO.Text))
            ins_PAT.Parameters.AddWithValue("@CITY_ID", ddl_CITY_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@IMAGE_CARD", "images/ImagePatiant/card.png")
            ins_PAT.Parameters.AddWithValue("@userId", Session("User_Id"))
            ins_PAT.Parameters.AddWithValue("@userIp", GetIPAddress())

            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ins_PAT.ExecuteNonQuery()
            insurance_SQLcon.Close()

            clrTxt()
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
End Class