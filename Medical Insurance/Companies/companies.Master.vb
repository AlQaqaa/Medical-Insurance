Imports System.Data.SqlClient
Imports System.IO

Public Class companies
    Inherits System.Web.UI.MasterPage

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then
            Session("User_Id") = 1

            If Session("User_Id") Is Nothing Or Session("User_Id") = 0 Then
                Response.Redirect("10.10.1.10", False)
            End If

        End If

        hl_company_users_list.NavigateUrl = "~/Companies/LISTPATIANT.aspx?cID=" & Session("company_id")

        If Session("company_id") IsNot Nothing Then
            Dim com_info As New SqlCommand("SELECT Bic_Link, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_START, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_START, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_END, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_END, (SELECT TOP 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY CONTRACT_NO DESC) AS CONTRACT_NO, (SELECT TOP 1 (PROFILE_PRICE_ID) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY n DESC) AS PROFILE_PRICE_ID, C_NAME_ARB, C_NAME_ENG, C_STATE, (CASE WHEN (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL ) IS NULL THEN  '-' ELSE (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL) END)AS MAIN_COMPANY FROM INC_COMPANY_DATA WHERE C_ID = " & Session("company_id"), insurance_SQLcon)
            Dim dt_com_info As New DataTable
            dt_com_info.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_com_info.Load(com_info.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_com_info.Rows.Count > 0 Then
                Dim dr_company = dt_com_info.Rows(0)
                lbl_company_name.Text = dr_company!C_NAME_ARB
                lbl_en_name.Text = dr_company!C_NAME_ENG
                img_company_logo.ImageUrl = "../" & dr_company!Bic_Link
                ViewState("contract_no") = dr_company!CONTRACT_NO
                ViewState("profile_id") = dr_company!PROFILE_PRICE_ID
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

    Private Sub btn_change_profile_Click(sender As Object, e As EventArgs) Handles btn_change_profile.Click
        Try
            Dim changePricesProfile As New SqlCommand
            changePricesProfile.Connection = insurance_SQLcon
            changePricesProfile.CommandText = "INC_editCompanyPricesProfile"
            changePricesProfile.CommandType = CommandType.StoredProcedure
            changePricesProfile.Parameters.AddWithValue("@c_id", Val(Session("company_id")))
            changePricesProfile.Parameters.AddWithValue("@contract_no", ViewState("contract_no"))
            changePricesProfile.Parameters.AddWithValue("@profile_id", ddl_profiles_prices.SelectedValue)
            changePricesProfile.Parameters.AddWithValue("@user_id", 1)
            changePricesProfile.Parameters.AddWithValue("@user_ip", GetIPAddress())
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()

            changePricesProfile.ExecuteNonQuery()
            insurance_SQLcon.Close()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية تعديل البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class