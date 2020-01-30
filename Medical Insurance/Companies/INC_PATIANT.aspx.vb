﻿Imports System.Data.SqlClient
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
                lbl_company_name.Text = "إضافة منتفعين " & dr_company!C_NAME_ARB
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

        'Try

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
        Dim fpath As String
        If FileUpload1.PostedFile.FileName = Nothing Then
            ins_PAT.Parameters.AddWithValue("@IMAGE_CARD", "images/ImagePatiant/card.png")
        Else
            fpath = Server.MapPath("../images/ImagePatiant/") & "\" & IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
            Dim FEx As String
            FEx = IO.Path.GetExtension(fpath)
            If FEx <> ".jpg" And FEx <> ".jpeg" And FEx <> ".png" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('صيغة الملف غير صحيحة')", True)
                Exit Sub
            End If
            Dim FileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")
            Dim FilePath As String = Server.MapPath("../images/ImagePatiant/") & "\" & IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
            FileUpload1.SaveAs(FilePath)
            ins_PAT.Parameters.AddWithValue("@IMAGE_CARD", "images/ImagePatiant/ " & FileUpload1.PostedFile.FileName)
        End If
        ins_PAT.Parameters.AddWithValue("@userId", 1)
        ins_PAT.Parameters.AddWithValue("@userIp", GetIPAddress())

        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        ins_PAT.ExecuteNonQuery()
        insurance_SQLcon.Close()

        clrTxt()
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        'Catch ex As Exception
        'MsgBox(ex.Message)
        ' End Try

    End Sub
End Class