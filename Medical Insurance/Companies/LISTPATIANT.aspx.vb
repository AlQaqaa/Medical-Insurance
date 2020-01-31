Imports System.Data.SqlClient
Public Class LISTPATIANT
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then

            Dim company_no As Integer = Val(Request.QueryString("cID"))

            If company_no <> 0 Then
                'جلب منتفعين شركة معينة
                Dim sel_com As New SqlCommand("SELECT PINC_ID, CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, C_ID, PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, NAT_NUMBER, (CASE WHEN (P_STATE) = 0 THEN 'مفعل' ELSE 'موقوف' END) AS P_STATE, (SELECT CON_NAME FROM MAIN_CONST WHERE MAIN_CONST.CON_ID = INC_PATIANT.CONST_ID) AS CONST_ID FROM INC_PATIANT WHERE C_ID = " & company_no, insurance_SQLcon)
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
                ' جلب جميع المنتفعين
                Dim sel_com As New SqlCommand("SELECT PINC_ID, CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, C_ID, PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, NAT_NUMBER, (CASE WHEN (P_STATE) = 0 THEN 'مفعل' ELSE 'موقوف' END) AS P_STATE, (SELECT CON_NAME FROM MAIN_CONST WHERE MAIN_CONST.CON_ID = INC_PATIANT.CONST_ID) AS CONST_ID FROM INC_PATIANT", insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
                Page.Title = "جميع المنتفعين"
                Label1.Text = "جميع المنتفعين"

                Dim pnl As Panel = DirectCast(Master.FindControl("panel1"), Panel)
                pnl.Visible = False
                Panel1.Visible = False

            End If

        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Patiant Name ################
        If (e.CommandName = "pat_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Session.Item("patiant_id") = (row.Cells(0).Text)
            Session.Item("company_id") = (row.Cells(1).Text)
            Response.Redirect("patientInfo.aspx")
        End If
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cell As TableCell = e.Row.Cells(11)
            Dim pet_state As String = cell.Text

            If pet_state = "موقوف" Then
                cell.BackColor = Drawing.Color.Red
            End If
        End If
    End Sub
End Class