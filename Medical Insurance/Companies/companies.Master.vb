Imports System.Data.SqlClient
Imports System.IO

Public Class companies
    Inherits System.Web.UI.MasterPage

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then
           

        End If

        hl_company_users_list.NavigateUrl = "~/Companies/LISTPATIANT.aspx?cID=" & Session("company_id")

        If Session("company_id") IsNot Nothing Then
            Dim sel_com As New SqlCommand("SELECT Bic_Link, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_START, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_START, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_END, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_END, (SELECT TOP 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY CONTRACT_NO DESC) AS CONTRACT_NO, C_NAME_ARB, C_NAME_ENG, C_STATE, (CASE WHEN (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL ) IS NULL THEN  '-' ELSE (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL) END)AS MAIN_COMPANY FROM INC_COMPANY_DATA WHERE C_ID = " & Session("company_id"), insurance_SQLcon)
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
                img_company_logo.ImageUrl = "../" & dr_company!Bic_Link
            End If

        Else
            Session.RemoveAll()
            Session.Clear()
            Panel_company_info.Visible = False
        End If

    End Sub

    Private Sub btn_change_img_Click(sender As Object, e As EventArgs) Handles btn_change_img.Click
        Try
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim fpath As String
            If FileUpload1.PostedFile.FileName <> Nothing Then
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
                Dim edit_img As New SqlCommand("UPDATE INC_COMPANY_DATA SET Bic_Link = @bic_pic WHERE C_ID = " & Session("company_id"), insurance_SQLcon)
                edit_img.Parameters.AddWithValue("@bic_pic", "images/ImageCompany/" & FileUpload1.PostedFile.FileName)
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                edit_img.ExecuteNonQuery()
                insurance_SQLcon.Close()
                Page_Load(sender, e)
            End If

            '''''''''''''''''''''''''''''''''''''''''
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class