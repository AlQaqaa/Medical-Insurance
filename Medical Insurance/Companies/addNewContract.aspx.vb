Imports System.Data.SqlClient
Imports System.Globalization

Public Class addNewContract
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
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

        Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
        Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

        Try
            Dim sel_data As New SqlCommand("SELECT * FROM INC_COMPANY_DETIAL WHERE C_ID = " & Session("company_id") & " AND DATE_END >= '" & start_dt & "'", insurance_SQLcon)
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
                insToCompany.Parameters.AddWithValue("@startDt", start_dt)
                insToCompany.Parameters.AddWithValue("@endDt", end_dt)
                insToCompany.Parameters.AddWithValue("@maxVal", txt_max_company_value.Text)
                insToCompany.Parameters.AddWithValue("@maxCard", txt_max_card_value.Text)
                insToCompany.Parameters.AddWithValue("@paymentType", ddl_payment_type.SelectedValue)
                insToCompany.Parameters.AddWithValue("@contractType", ddl_contractType.SelectedValue) ' 3تمدبد2.. تجدبد
                insToCompany.Parameters.AddWithValue("@patiaintPer", ddl_PATIAINT_PER.SelectedValue)
                insToCompany.Parameters.AddWithValue("@userId", 1)
                insToCompany.Parameters.AddWithValue("@userIp", GetIPAddress())
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                insToCompany.ExecuteNonQuery()
                insurance_SQLcon.Close()

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                clrTxt()
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('خطأ هناك عقد ساري لهذه الشركة بنفس الفترة التي اخترتها'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class