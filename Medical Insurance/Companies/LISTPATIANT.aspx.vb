Imports System.Data.SqlClient
Public Class LISTPATIANT
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("active_card") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If

            Dim company_no As Integer = Val(Request.QueryString("cID"))

            If company_no <> 0 Then
                main_company_panel.Visible = False

                'جلب منتفعين شركة معينة
                Dim sel_com As New SqlCommand("SELECT PINC_ID, CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, C_ID, PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, NAT_NUMBER, (CASE WHEN (P_STATE) = 0 THEN 'مفعل' ELSE 'موقوف' END) AS P_STATE, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID FROM INC_PATIANT WHERE C_ID = " & company_no, insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()
                GridView1.DataSource = dt_result
                GridView1.DataBind()

                ' جلب بيانات الشركة
                Dim get_comp As New SqlCommand("SELECT C_NAME_ARB,C_NAME_ENG FROM INC_COMPANY_DATA WHERE C_id = " & company_no, insurance_SQLcon)
                Dim dt_comp As New DataTable
                dt_comp.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_comp.Load(get_comp.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_comp.Rows.Count > 0 Then
                    Dim dr_company = dt_comp.Rows(0)
                    Page.Title = "منتفعي شركة " & dr_company!C_NAME_ARB
                    Label1.Text = "منتفعي شركة " & dr_company!C_NAME_ARB
                End If
            Else
                main_company_panel.Visible = True

                Page.Title = "المنتفعين"
                Label1.Text = "المنتفعين"

                Dim pnl As Panel = DirectCast(Master.FindControl("panel1"), Panel)
                pnl.Visible = False

            End If

        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Patiant Name ################
        If (e.CommandName = "pat_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Session.Item("patiant_id") = (row.Cells(0).Text) ' 13471
            Session.Item("company_id") = (row.Cells(1).Text) '56 

            Response.Redirect("patientInfo.aspx")
        End If
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cell As TableCell = e.Row.Cells(2)
            Dim pet_state As String = cell.Text

            If pet_state = "موقوف" Then
                cell.BackColor = Drawing.Color.Red
            End If
        End If
    End Sub

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        Try
            'جلب منتفعين شركة معينة
            Dim sel_com As New SqlCommand("SELECT PINC_ID, CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, C_ID, PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, NAT_NUMBER, (CASE WHEN (P_STATE) = 0 THEN 'مفعل' ELSE 'موقوف' END) AS P_STATE, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID FROM INC_PATIANT WHERE C_ID = " & ddl_companies.SelectedValue, insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            GridView1.DataSource = dt_result
            GridView1.DataBind()
            sel_com = Nothing

            ' جلب بيانات الشركة
            Dim get_comp As New SqlCommand("SELECT C_NAME_ARB,C_NAME_ENG FROM INC_COMPANY_DATA WHERE C_id = " & ddl_companies.SelectedValue, insurance_SQLcon)
            Dim dt_comp As New DataTable
            dt_comp.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_comp.Load(get_comp.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_comp.Rows.Count > 0 Then
                Dim dr_company = dt_comp.Rows(0)
                Page.Title = "منتفعي شركة " & dr_company!C_NAME_ARB
                Label1.Text = "منتفعي شركة " & dr_company!C_NAME_ARB
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class