Imports System.Data.SqlClient
Public Class patientInfo
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim pat_no As Integer = Val(Session("patiant_id"))

            If pat_no = 0 Then
                Response.Redirect("LISTPATIANT.aspx")
            End If

            getPatInfo()
            getBlockServices()

        End If

    End Sub

    Sub getPatInfo()
        Dim get_pet As New SqlCommand("SELECT CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE, BAGE_NO, PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, P_STATE, NAT_NUMBER, IMAGE_CARD, (SELECT C_NAME_ARB FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_PATIANT.C_ID) AS COMPANY_NAME, (SELECT CON_NAME FROM MAIN_CONST WHERE MAIN_CONST.CON_ID = INC_PATIANT.CONST_ID) AS CONST_ID, (SELECT NAT_NAME FROM MAIN_NATIONALITY WHERE MAIN_NATIONALITY.NAT_ID = INC_PATIANT.NAL_ID) AS NAT_NAME, (SELECT CT_NAME FROM MAIN_CITY WHERE MAIN_CITY.CT_ID = INC_PATIANT.CITY_ID) AS CITY_NAME FROM INC_PATIANT WHERE PINC_ID = " & Val(Session("patiant_id")), insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(get_pet.ExecuteReader)
        insurance_SQLcon.Close()
        If dt_result.Rows.Count > 0 Then
            Dim dr_pat = dt_result.Rows(0)
            lbl_name_eng.Text = dr_pat!NAME_ENG
            lbl_pat_name.Text = dr_pat!NAME_ARB
            lbl_name_page.Text = dr_pat!NAME_ARB
            lbl_birthdate.Text = dr_pat!BIRTHDATE
            lbl_nat_num.Text = dr_pat!NAT_NUMBER
            lbl_phone.Text = dr_pat!PHONE_NO
            lbl_company_name.Text = dr_pat!COMPANY_NAME
            lbl_com_name.Text = dr_pat!COMPANY_NAME
            lbl_card_no.Text = dr_pat!CARD_NO
            lbl_exp_dt.Text = dr_pat!EXP_DATE
            lbl_bage_no.Text = dr_pat!BAGE_NO
            lbl_const.Text = dr_pat!CONST_ID
            ViewState("pat_state") = dr_pat!P_STATE
            If dr_pat!P_STATE = 0 Then
                lbl_icon_sts.CssClass = "text-success"
                lbl_sts.Text = "مفعل"
                lbl_sts.CssClass = "text-success"
                btn_change_sts.Text = "إيقاف"
                btn_change_sts.CssClass = "btn btn-outline-danger btn-block"
            Else
                lbl_icon_sts.CssClass = "text-danger"
                lbl_sts.Text = "موقوف"
                lbl_sts.CssClass = "text-danger"
                btn_change_sts.Text = "تفعيل"
                btn_change_sts.CssClass = "btn btn-outline-success btn-block"
            End If
        End If
    End Sub

    Private Sub btn_change_sts_Click(sender As Object, e As EventArgs) Handles btn_change_sts.Click

        Dim new_sts As Integer = 0

        If ViewState("pat_state") = 0 Then
            new_sts = 1
        Else
            new_sts = 0
        End If

        Try
            Dim edit_sts As New SqlCommand("UPDATE INC_PATIANT SET P_STATE = " & new_sts & " WHERE PINC_ID = " & Val(Session("patiant_id")), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            edit_sts.ExecuteNonQuery()
            insurance_SQLcon.Close()

            getPatInfo()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_ban_service_Click(sender As Object, e As EventArgs) Handles btn_ban_service.Click
        Try
            Dim ins_com As New SqlCommand("INSERT INTO INC_BLOCK_SERVICES (OBJECT_ID, SER_ID, BLOCK_TP, NOTES, USER_ID, USER_IP) VALUES (@OBJECT_ID, @SER_ID, @BLOCK_TP, @NOTES, @USER_ID, @USER_IP)", insurance_SQLcon)
            ins_com.Parameters.Add("@OBJECT_ID", SqlDbType.Int).Value = Val(Session("patiant_id"))
            ins_com.Parameters.Add("@SER_ID", SqlDbType.Int).Value = ddl_services.SelectedValue
            ins_com.Parameters.Add("@BLOCK_TP", SqlDbType.Int).Value = 1 ' Block Service
            ins_com.Parameters.Add("@NOTES", SqlDbType.NVarChar).Value = txt_notes.Text
            ins_com.Parameters.Add("@USER_ID", SqlDbType.Int).Value = 1
            ins_com.Parameters.Add("@USER_IP", SqlDbType.NVarChar).Value = GetIPAddress()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            ins_com.ExecuteNonQuery()
            insurance_SQLcon.Close()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub getBlockServices()
        Dim get_com As New SqlCommand("SELECT SER_ID, (SELECT SERV_NAMEARB FROM MAIN_SERVIES WHERE MAIN_SERVIES.SERV_ID = INC_BLOCK_SERVICES.SER_ID) AS SERVICE_NAME, (SELECT SERV_CODE FROM MAIN_SERVIES WHERE MAIN_SERVIES.SERV_ID = INC_BLOCK_SERVICES.SER_ID) AS SERV_CODE, NOTES FROM INC_BLOCK_SERVICES WHERE OBJECT_ID = " & Val(Session("patiant_id")) & " AND BLOCK_TP = 1", insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(get_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView1.DataSource = dt_result
            GridView1.DataBind()
        Else
            dt_result.Rows.Clear()
            GridView1.DataBind()
            Label1.Text = "لا توجد خدمات محظورة عن هذا المشترك"
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Stop Block ################
        If (e.CommandName = "stop_block") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim del_com As New SqlCommand("DELETE FROM INC_BLOCK_SERVICES WHERE PINC_ID = " & Val(Session("patiant_id")) & " AND SER_ID = " & (row.Cells(0).Text) & " AND BLOCK_TP = 1", insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            del_com.ExecuteNonQuery()
            insurance_SQLcon.Close()
            getBlockServices()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت العملية بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)

        End If
    End Sub
End Class