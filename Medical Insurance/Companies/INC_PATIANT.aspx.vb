Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Public Class INC_PATIANT
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim company_no As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("active_card") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If

            company_no = Val(Session("company_id"))

            If company_no = 0 Then
                Response.Redirect("Default.aspx")
            End If

            Dim sel_com As New SqlCommand("SELECT C_NAME_ARB, (SELECT TOP (1) DATE_END FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_END FROM INC_COMPANY_DATA WHERE C_id = " & company_no, insurance_SQLcon)
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
                CalendarExtender1.SelectedDate = dr_company!DATE_END
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

            Dim dbpath As String = "images/ImagePatiant/card.png"

            Dim ins_PAT As New SqlCommand
            ins_PAT.Connection = insurance_SQLcon
            ins_PAT.CommandText = "INC_addNewpatiant"
            ins_PAT.CommandType = CommandType.StoredProcedure
            ins_PAT.Parameters.AddWithValue("@CARD_NO", txt_CARD_NO.Text)
            ins_PAT.Parameters.AddWithValue("@NAME_ARB", txt_NAME_ARB.Text)
            ins_PAT.Parameters.AddWithValue("@NAME_ENG", txt_NAME_ENG.Text)
            ins_PAT.Parameters.AddWithValue("@BIRTHDATE", DateTime.Parse(txt_BIRTHDATE.Text))
            ins_PAT.Parameters.AddWithValue("@BAGE_NO", txt_BAGE_NO.Text)
            ins_PAT.Parameters.AddWithValue("@C_ID", Val(Session("company_id")))
            ins_PAT.Parameters.AddWithValue("@GENDER", ddl_GENDER.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@NAL_ID", ddl_NAL_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@PHONE_NO", txt_PHONE_NO.Text)
            ins_PAT.Parameters.AddWithValue("@CONST_ID", CONST_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@EXP_DATE", DateTime.Parse(txt_exp_date.Text))
            ins_PAT.Parameters.AddWithValue("@NOTES", txt_NOTES.Text)
            ins_PAT.Parameters.AddWithValue("@P_STATE", P_STATE.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@NAT_NUMBER", Val(txt_NAT_NUMBER.Text))
            ins_PAT.Parameters.AddWithValue("@KID_NO", Val(txt_KID_NO.Text))
            ins_PAT.Parameters.AddWithValue("@CITY_ID", ddl_CITY_ID.SelectedValue)
            ins_PAT.Parameters.AddWithValue("@IMAGE_CARD", dbpath)
            ins_PAT.Parameters.AddWithValue("@userId", Session("INC_User_Id"))
            ins_PAT.Parameters.AddWithValue("@userIp", GetIPAddress())
            ins_PAT.Parameters.AddWithValue("@pat_id", SqlDbType.Int).Direction = ParameterDirection.Output
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ins_PAT.ExecuteNonQuery()
            insurance_SQLcon.Close()

            Dim pat_id As Integer = ins_PAT.Parameters("@pat_id").Value.ToString()

            If FileUpload1.HasFile = True Then
                Dim newFilename As String = Val(pat_id) & "N"
                Dim fileExtension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
                Dim updatedFilename As String = newFilename + fileExtension
                Dim fpath As String = Server.MapPath("../images/ImagePatiant") & "/" & updatedFilename
                dbpath = "images/ImagePatiant" & "/" & updatedFilename
                Dim FEx As String
                FEx = IO.Path.GetExtension(fpath)
                If FEx <> ".jpg" And FEx <> ".JPG" And FEx <> ".png" And FEx <> ".PNG" And FEx <> ".bmp" And FEx <> ".jpeg" Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alter('خطأ! صيغة الصورة غير صحيحة ');", True)
                    Exit Sub
                End If
                If System.IO.File.Exists(fpath) Then
                    System.IO.File.Delete(Server.MapPath("../images/ImagePatiant" & "/" & updatedFilename))
                End If
                FileUpload1.PostedFile.SaveAs(fpath)

                Dim edit_img As New SqlCommand("UPDATE INC_PATIANT SET IMAGE_CARD = @IMAGE_CARD WHERE PINC_ID = " & Val(pat_id), insurance_SQLcon)
                edit_img.Parameters.AddWithValue("@IMAGE_CARD", dbpath)
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                edit_img.ExecuteNonQuery()
                insurance_SQLcon.Close()

            End If

            clrTxt()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('تمت عملية حفظ البيانات بنجاح');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub txt_CARD_NO_TextChanged(sender As Object, e As EventArgs) Handles txt_CARD_NO.TextChanged
        Try
            Dim sel_com As New SqlCommand("SELECT NAME_ARB FROM INC_PATIANT WHERE CARD_NO = '" & txt_CARD_NO.Text & "' AND C_ID = " & Val(Session("company_id")), insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                Dim strbody As New StringBuilder()
                strbody.Append("<div class='alert alert-info' role='alert'>")
                strbody.Append("<ul class='list-unstyled'>")

                For i = 0 To dt_result.Rows.Count - 1
                    Dim dr_result = dt_result.Rows(i)
                    strbody.Append("<li>" & dr_result!NAME_ARB & "</li>")
                Next
                strbody.Append("</ul>")
                strbody.Append("</div>")

                LtlTableBody.Text = strbody.ToString()
            End If

            txt_BAGE_NO.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txt_NAME_ARB_TextChanged(sender As Object, e As EventArgs) Handles txt_NAME_ARB.TextChanged
        txt_NAME_ENG.Text = TransA2E(txt_NAME_ARB.Text)
        txt_NAME_ENG.Focus()
    End Sub
End Class