Imports System.Data.SqlClient
Imports System.Globalization

Public Class WebForm1
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            ViewState("patiant_id") = Val(Request.QueryString("pID"))

            If ViewState("patiant_id") = 0 Then
                Response.Redirect("patientList.aspx")
            End If

            Dim sel_com As New SqlCommand("SELECT PINC_ID, C_ID,CARD_NO,NAME_ARB,NAME_ENG,CONVERT(VARCHAR, BIRTHDATE, 103) AS BIRTHDATE,BAGE_NO,C_ID,GENDER,NAL_ID,PHONE_NO,CONST_ID,CONVERT(VARCHAR, EXP_DATE, 103) AS EXP_DATE,ISNULL(NOTES, '') AS NOTES,P_STATE,ISNULL(NAT_NUMBER, 0) AS NAT_NUMBER,ISNULL(KID_NO, 0) AS KID_NO,CITY_ID FROM INC_PATIANT WHERE PINC_ID = " & ViewState("patiant_id"), insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                Dim dr = dt_result.Rows(0)
                txt_CARD_NO.Text = dr!card_no
                txt_NAME_ARB.Text = dr!name_arb
                txt_NAME_ENG.Text = dr!name_eng
                txt_BIRTHDATE.Text = dr!BIRTHDATE
                txt_BAGE_NO.Text = dr!bage_no
                ddl_GENDER.SelectedValue = dr!gender
                ddl_NAL_ID.SelectedValue = dr!NAL_ID
                txt_PHONE_NO.Text = dr!phone_no
                ddl_CONST_ID.SelectedValue = dr!CONST_ID
                txt_exp_date.Text = dr!exp_date
                txt_NOTES.Text = dr!notes
                txt_NAT_NUMBER.Text = dr!NAT_NUMBER
                txt_KID_NO.Text = dr!KID_NO
                ddl_CITY_ID.SelectedValue = dr!CITY_ID
                ddl_company.SelectedValue = dr!C_ID
            End If

        End If

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim ins_PAT As New SqlCommand
            ins_PAT.Connection = insurance_SQLcon
            ins_PAT.CommandText = "INC_editPatiant"
            ins_PAT.CommandType = CommandType.StoredProcedure
            ins_PAT.Parameters.AddWithValue("@CARD_NO", txt_CARD_NO.Text)
            ins_PAT.Parameters.AddWithValue("@NAME_ARB", txt_NAME_ARB.Text)
            ins_PAT.Parameters.AddWithValue("@NAME_ENG", txt_NAME_ENG.Text)
            ins_PAT.Parameters.AddWithValue("@BIRTHDATE", DateTime.ParseExact(txt_BIRTHDATE.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture))
            ins_PAT.Parameters.AddWithValue("@BAGE_NO", txt_BAGE_NO.Text)
            ins_PAT.Parameters.AddWithValue("@C_ID", ddl_company.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@GENDER", ddl_GENDER.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@NAL_ID", ddl_NAL_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@PHONE_NO", txt_PHONE_NO.Text)
            ins_PAT.Parameters.AddWithValue("@CONST_ID", ddl_CONST_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@EXP_DATE", DateTime.ParseExact(txt_exp_date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture))
            ins_PAT.Parameters.AddWithValue("@NOTES", txt_NOTES.Text)
            ins_PAT.Parameters.AddWithValue("@NAT_NUMBER", txt_NAT_NUMBER.Text)
            ins_PAT.Parameters.AddWithValue("@KID_NO", txt_KID_NO.Text)
            ins_PAT.Parameters.AddWithValue("@CITY_ID", ddl_CITY_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@PINC_ID", ViewState("patiant_id"))
            ins_PAT.Parameters.AddWithValue("@user_id", Session("INC_User_Id"))
            ins_PAT.Parameters.AddWithValue("@user_ip", GetIPAddress())

            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ins_PAT.ExecuteNonQuery()
            insurance_SQLcon.Close()

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
