Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Globalization

Public Class approvalDetails
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)
    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            ViewState("confirm_no") = Val(Request.QueryString("confID"))

            If Val(ViewState("confirm_no")) = 0 Then
                Response.Redirect("listApproval.aspx", False)
            End If
            getConfirmData()
            getConfirmDetails()
        End If

    End Sub

    Sub getConfirmData()

        'Try
        Dim sel_com As New SqlCommand("SELECT (SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS P_NAME, (SELECT CARD_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS CARD_NO, (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS BAGE_NO, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = ((SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID))) AS C_NAME, (SELECT SUM(SERVICE_PRICE) FROM INC_CONFIRM_DETAILS WHERE INC_CONFIRM_DETAILS.CONFIRM_ID = INC_CONFIRM.CONFIRM_ID GROUP BY CONFIRM_ID) AS TOTAL_VALUE, convert(varchar, CONFRIM_END_DATE, 23) AS CONFRIM_END_DATE, PENDING_VALUE, (CASE WHEN (REQUEST_UNIT = 1) THEN 'قسم التأمين الصحي' ELSE 'الإيواء والعمليات' END) AS REQUEST_TYPE, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.BAGE_NO = (SELECT BAGE_NO FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AND INC_PATIANT.CONST_ID = 0), '') AS EMP_NAME, (SELECT (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN (CONST_ID) = 9 THEN 'زوجة الأب' END) FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID) AS CONST_ID, (SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = (SELECT C_ID FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = INC_CONFIRM.PINC_ID)) AS COMPANY_NAME, APPROVED_VALUE FROM INC_CONFIRM WHERE REQUEST_STS = 0", insurance_SQLcon)
            Dim dt_result As New DataTable
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Dim dr_confirm = dt_result.Rows(0)
                txt_patient_name.Text = dr_confirm!P_NAME
                txt_emp_name.Text = dr_confirm!EMP_NAME
                txt_confirm_no.Text = ViewState("confirm_no")
                txt_card_no.Text = dr_confirm!CARD_NO
                txt_emp_no.Text = dr_confirm!BAGE_NO
                txt_relation.Text = dr_confirm!CONST_ID
                txt_emp_name.Text = dr_confirm!EMP_NAME
                txt_end_dt.Text = dr_confirm!CONFRIM_END_DATE
                txt_approv_val.Text = Format(dr_confirm!APPROVED_VALUE, "0,00.000")
                txt_total_val.Text = Format(dr_confirm!TOTAL_VALUE, "0,00.000")
                txt_pending_val.Text = Format(dr_confirm!PENDING_VALUE, "0,00.000")
                txt_requst.Text = dr_confirm!REQUEST_TYPE
                txt_company_name.Text = dr_confirm!COMPANY_NAME
            End If
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub

    Sub getConfirmDetails()
        ' Try

        Dim sel_com As New SqlCommand("SELECT CD_ID, SUB_SERVICE_ID, SERVICE_PRICE, (CASE WHEN (REQUEST_TYPE = 1) THEN 'خدمة/عملية' ELSE 'إضافة على عملية' END) AS REQUEST_TYPE, (CASE WHEN (REQUEST_TYPE = 1) THEN (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = TBL1.SUB_SERVICE_ID) ELSE '/' END) AS SubService_Code, (CASE WHEN (REQUEST_TYPE = 1) THEN (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = TBL1.SUB_SERVICE_ID) ELSE (SELECT ADD_NAME FROM INC_CONFIRM_DETAILS AS TBL2 WHERE TBL2.CD_ID = TBL1.CD_ID) END) AS SubService_AR_Name FROM INC_CONFIRM_DETAILS AS TBL1 WHERE CONFIRM_ID = " & ViewState("confirm_no"), insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            End If

        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "delete_ser") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim del_com As New SqlCommand("DELETE FROM INC_CONFIRM_DETAILS WHERE EWA_NO = 0 AND CD_ID = " & (row.Cells(1).Text), insurance_SQLcon)
            insurance_SQLcon.Open()
            del_com.ExecuteNonQuery()
            insurance_SQLcon.Close()
            getConfirmDetails()
        End If
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Try

            If GridView1.Rows.Count = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('يرجى اختيار خدمة واحدة على الأقل'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                Exit Sub
            End If

            Using main_ds
                Dim sel_com As New SqlCommand("SELECT CD_ID, SUB_SERVICE_ID, SERVICE_PRICE, (CASE WHEN (REQUEST_TYPE = 1) THEN 'خدمة/عملية' ELSE 'إضافة على عملية' END) AS REQUEST_TYPE, (CASE WHEN (REQUEST_TYPE = 1) THEN (SELECT SubService_Code FROM Main_SubServices WHERE Main_SubServices.SubService_ID = TBL1.SUB_SERVICE_ID) ELSE '/' END) AS SubService_Code, (CASE WHEN (REQUEST_TYPE = 1) THEN (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = TBL1.SUB_SERVICE_ID) ELSE (SELECT ADD_NAME FROM INC_CONFIRM_DETAILS AS TBL2 WHERE TBL2.CD_ID = TBL1.CD_ID) END) AS SubService_AR_Name FROM INC_CONFIRM_DETAILS AS TBL1 WHERE CONFIRM_ID = " & ViewState("confirm_no"))
                Using sda As New SqlDataAdapter()
                    sel_com.Connection = insurance_SQLcon
                    sda.SelectCommand = sel_com
                    sda.Fill(main_ds, "INC_EWA_Confirm")
                End Using
            End Using

            Dim viewer As ReportViewer = New ReportViewer()

            Dim datasource As New ReportDataSource("confirmDS", main_ds.Tables("INC_EWA_Confirm"))
            viewer.LocalReport.DataSources.Clear()
            viewer.ProcessingMode = ProcessingMode.Local
            viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/confirmApproval.rdlc")
            viewer.LocalReport.DataSources.Add(datasource)

            Dim rp1 As ReportParameter
            Dim rp2 As ReportParameter
            Dim rp3 As ReportParameter
            Dim rp4 As ReportParameter
            Dim rp5 As ReportParameter
            Dim rp6 As ReportParameter
            Dim rp7 As ReportParameter

            rp1 = New ReportParameter("company_name", txt_company_name.Text)
            rp2 = New ReportParameter("pat_name", "للمريض: " & txt_patient_name.Text)
            rp3 = New ReportParameter("card_no", "رقم البطاقة: " & txt_card_no.Text)
            rp4 = New ReportParameter("emp_no", "الرقم الوظيفي: " & txt_emp_no.Text)
            rp5 = New ReportParameter("moshtark_name", "اسم المشترك: " & txt_emp_name.Text)
            rp6 = New ReportParameter("relation_m", "الصلة بالمشترك: " & txt_relation.Text)
            rp7 = New ReportParameter("approv_no", ViewState("confirm_no").ToString)

            viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4, rp5, rp6, rp7})

            Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
            Dim r As String = "~/Reports/confirmApproval.rdlc"
            ' Page.Controls.Add(rv)

            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim bytes As Byte()
            Dim FolderLocation As String
            FolderLocation = Server.MapPath("~/Reports")
            Dim filepath As String = FolderLocation & "/confirmApproval" & Session("User_Id") & ".pdf"
            If Directory.Exists(filepath) Then
                File.Delete(filepath)
            End If
            bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
                encoding, extension, streamids, warnings)
            Dim fs As New FileStream(FolderLocation & "/confirmApproval" & Session("User_Id") & ".pdf", FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
            fs.Close()
            Response.Redirect("~/Reports/confirmApproval" & Session("User_Id") & ".pdf", False)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class