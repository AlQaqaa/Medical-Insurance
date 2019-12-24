﻿Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Public Class addNewCompany
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
        Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

        Try
            Dim insToCompany As New SqlCommand
            insToCompany.Connection = insurance_SQLcon
            insToCompany.CommandText = "INC_addNewCompany"
            insToCompany.CommandType = CommandType.StoredProcedure
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim fpath As String
            If FileUpload1.PostedFile.FileName = Nothing Then
                insToCompany.Parameters.AddWithValue("@bic_pic", "images/ImageCompany/UnknownUser.png")
            Else
                fpath = Server.MapPath("../images/ImageCompany/") & "\" & IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
                Dim FEx As String
                FEx = IO.Path.GetExtension(fpath)
                If FEx <> ".jpg" And FEx <> ".jpeg" And FEx <> ".png" Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('صيغة الملف غير صحيحة')", True)
                    Exit Sub
                End If
                Dim FileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)
                Dim Extension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
                Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")
                Dim FilePath As String = Server.MapPath("../images/ImageCompany/") & "\" & IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
                FileUpload1.SaveAs(FilePath)
                insToCompany.Parameters.AddWithValue("@bic_pic", "images/ImageCompany/ " & FileUpload1.PostedFile.FileName)
            End If

            '''''''''''''''''''''''''''''''''''''''''
            insToCompany.Parameters.AddWithValue("@cNameAr", txt_company_name_ar.Text)
            insToCompany.Parameters.AddWithValue("@cNameEn", txt_company_name_en.Text)
            insToCompany.Parameters.AddWithValue("@cState", False)
            insToCompany.Parameters.AddWithValue("@c_level", company_level)
            insToCompany.Parameters.AddWithValue("@startDt", start_dt)
            insToCompany.Parameters.AddWithValue("@endDt", end_dt)
            insToCompany.Parameters.AddWithValue("@maxVal", txt_max_company_value.Text)
            insToCompany.Parameters.AddWithValue("@maxCard", txt_max_card_value.Text)
            insToCompany.Parameters.AddWithValue("@paymentType", ddl_payment_type.SelectedValue)
            insToCompany.Parameters.AddWithValue("@contractType", 1) ' 1 = New Contract
            insToCompany.Parameters.AddWithValue("@patiaintPer", ddl_PATIAINT_PER.SelectedValue)
            insToCompany.Parameters.AddWithValue("@profile_price_id", ddl_profiles_prices.SelectedValue)
            insToCompany.Parameters.AddWithValue("@userId", 1)
            insToCompany.Parameters.AddWithValue("@userIp", GetIPAddress())
            insurance_SQLcon.Open()
            insToCompany.ExecuteNonQuery()
            insurance_SQLcon.Close()
            clrTxt()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

   
End Class