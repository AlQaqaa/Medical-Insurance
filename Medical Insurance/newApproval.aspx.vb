Imports System.Data.SqlClient
Public Class newApproval
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        Try
            Dim sel_com As New SqlCommand("SELECT PINC_ID, NAME_ARB, NAME_ENG, CARD_NO, BAGE_NO, CONST_ID FROM INC_PATIANT WHERE C_ID =" & ddl_companies.SelectedValue & " AND NAME_ARB LIKE '%" & txt_search_box.Text & "%' OR CARD_NO LIKE '" & txt_search_box.Text & "' OR BAGE_NO LIKE '" & txt_search_box.Text & "'", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                Panel1.Visible = True
                Panel2.Visible = True
                source_list.DataSource = dt_result
                source_list.DataValueField = "PINC_ID"
                source_list.DataTextField = "NAME_ARB"
                source_list.DataBind()
                For i = 0 To dt_result.Rows.Count - 1
                    Dim dr_pet = dt_result.Rows(i)
                    If dr_pet!CONST_ID = 0 Then
                        Label1.Text = "اسم المشترك: " & dr_pet!NAME_ARB
                        Exit For
                    End If
                Next
            Else
                Panel1.Visible = False
                Panel2.Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub source_list_SelectedIndexChanged(sender As Object, e As EventArgs) Handles source_list.SelectedIndexChanged
        'Label1.Text = source_list.SelectedValue

        Dim sel_com As New SqlCommand("SELECT PINC_ID, NAME_ARB, NAME_ENG, CARD_NO, BAGE_NO, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID, P_STATE FROM INC_PATIANT WHERE PINC_ID =" & source_list.SelectedValue, insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_result.Rows.Count > 0 Then
            Dim dr_result = dt_result.Rows(0)

            If dr_result!P_STATE = 0 Then
                Label2.Text = "الحالة: مفعل"
                Label2.CssClass = "text-success"
            Else
                Label2.Text = "الحالة: موقوف"
                Label2.CssClass = "text-danger"
            End If
            Label3.Text = "المريض: " & source_list.SelectedItem.Text
            Label4.Text = "الصلة بالمشترك: " & dr_result!CONST_ID
            Label5.Text = "رقم البطاقة: " & dr_result!CARD_NO
            Label6.Text = "الرقم الوظيفي: " & dr_result!BAGE_NO
        End If
    End Sub

    Private Sub ddl_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_type.SelectedIndexChanged

        If ddl_type.SelectedValue = 1 Then
            ddl_clinics.Enabled = True
            ddl_services.Enabled = True
            ddl_sub_service.Enabled = True
            btn_chose.Enabled = True
        ElseIf ddl_type.SelectedValue = 2 Then
            btn_chose.Enabled = True
            ddl_clinics.Enabled = False
            ddl_services.Enabled = False
            ddl_sub_service.Enabled = True

            Dim sel_com As New SqlCommand("SELECT Ewa_Add_id, Ewa_Add_name FROM Ewa_Additionals WHERE Ewa_Add_Type = 1", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                ddl_sub_service.DataSource = dt_result
                ddl_sub_service.DataValueField = "Ewa_Add_id"
                ddl_sub_service.DataTextField = "Ewa_Add_name"
                ddl_sub_service.DataBind()
            End If
        Else
            ddl_clinics.Enabled = False
            ddl_services.Enabled = False
            ddl_sub_service.Enabled = False
            btn_chose.Enabled = False
        End If

    End Sub

    Private Sub ddl_services_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_services.SelectedIndexChanged
        If ddl_type.SelectedValue = 1 Then
            Dim sel_com As New SqlCommand("SELECT SubService_ID, SubService_AR_Name FROM Main_SubServices WHERE SubService_Service_ID = " & ddl_services.SelectedValue, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                ddl_sub_service.DataSource = dt_result
                ddl_sub_service.DataValueField = "SubService_ID"
                ddl_sub_service.DataTextField = "SubService_AR_Name"
                ddl_sub_service.DataBind()
            End If
        End If
    End Sub
End Class