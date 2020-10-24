Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.IO

Public Class invoiceContent
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ViewState("invoice_no") = Val(Request.QueryString("invID"))
        Page.Title = "محتويات الفاتورة رقم " & ViewState("invoice_no")

        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("print_motalba") = False Then
                    btn_print.Visible = False
                    btn_print_details.Visible = False
                End If
                If Session("User_per")("return_motalba") = False Then
                    btn_return.Visible = False
                End If
            End If
            If ViewState("invoice_no") <> 0 Then

                txt_invoice_no.Text = ViewState("invoice_no")
                Dim sel_com As New SqlCommand("SELECT INVOICE_NO, INC_INVOICES.C_ID, TBL1.C_Name_Arb AS COMPANY_NAME, ISNULL(TBL2.C_Name_Arb, '') AS MAIN_COMPANY,CONVERT(VARCHAR, DATE_FROM, 23) AS DATE_FROM, CONVERT(VARCHAR, DATE_TO, 23) AS DATE_TO FROM INC_INVOICES
                    LEFT JOIN INC_COMPANY_DATA AS TBL1 ON TBL1.C_ID = INC_INVOICES.C_ID
                    LEFT JOIN INC_COMPANY_DATA AS TBL2 ON TBL2.C_ID = TBL1.C_Level WHERE INVOICE_NO = " & ViewState("invoice_no"), insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()

                If dt_result.Rows.Count > 0 Then
                    Dim dr_inv = dt_result.Rows(0)
                    txt_company_name.Text = dr_inv!COMPANY_NAME
                    ViewState("company_no") = dr_inv!C_ID
                    ViewState("main_company") = dr_inv!MAIN_COMPANY
                    txt_start_dt.Text = dr_inv!DATE_FROM
                    txt_end_dt.Text = dr_inv!DATE_TO

                    getData()

                End If

            Else
                Response.Redirect("Default.aspx", False)
            End If

        End If

    End Sub

    Sub getPatientProcessesInvoice()
        Using main_ds
            Dim sel_com As New SqlCommand("SELECT Processes_ID, Processes_Date, Processes_Residual, Processes_Price, Processes_Paid, INVOICE_NO,CARD_NO,NAME_ARB,BAGE_NO,CONVERT(VARCHAR, BIRTHDATE, 111) AS BIRTHDATE, Clinic_AR_Name,SubService_AR_Name, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, INC_COMPANY_DATA.C_Name_Arb,SubService_Code FROM INC_IvoicesProcesses
                INNER JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_IvoicesProcesses.P_ID
                INNER JOIN Main_Clinic ON Main_Clinic.clinic_id = INC_IvoicesProcesses.Processes_Cilinc
                INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_IvoicesProcesses.Processes_SubServices
                LEFT JOIN HAG_Processes_Doctor ON HAG_Processes_Doctor.Doctor_Processes_ID = INC_IvoicesProcesses.Processes_ID AND ISNULL(HAG_Processes_Doctor.doc_type, 0) = 0
                LEFT JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = HAG_Processes_Doctor.Processes_Doctor_ID
                INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_IvoicesProcesses.C_ID WHERE INVOICE_NO = " & ViewState("invoice_no") & " ORDER BY INC_IvoicesProcesses.id")
            Using sda As New SqlDataAdapter()
                sel_com.Connection = insurance_SQLcon
                sda.SelectCommand = sel_com
                sda.Fill(main_ds, "patientProcessesInvoice")
                'sda.Fill(dt_result)
                'Return main_ds
            End Using
        End Using
    End Sub

    Sub getData()
        Try
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()

            Using main_ds
                Dim sel_com As New SqlCommand("SELECT P_ID,INC_PATIANT.INC_Patient_Code,SUM(PROCESSES_RESIDUAL) AS PROCESSES_RESIDUAL,CARD_NO,BAGE_NO,NAME_ARB, INC_COMPANY_DATA.C_Name_Arb
            FROM INC_IVOICESPROCESSES 
            INNER JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_IVOICESPROCESSES.P_ID
            INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_IVOICESPROCESSES.C_ID WHERE INVOICE_NO = " & ViewState("invoice_no") & " AND INC_IVOICESPROCESSES.C_ID = " & ViewState("company_no") & " AND MOTALABA_STS = 1 GROUP BY P_ID,CARD_NO, BAGE_NO,NAME_ARB,INC_COMPANY_DATA.C_Name_Arb,INC_PATIANT.INC_Patient_Code ORDER BY NAME_ARB")
                Using sda As New SqlDataAdapter()
                    sel_com.Connection = insurance_SQLcon
                    sda.SelectCommand = sel_com
                    sda.Fill(main_ds, "INC_IvoicesProcesses")
                    sda.Fill(dt_result)
                    'Return main_ds
                End Using
            End Using

            If dt_result.Rows.Count > 0 Then
                ViewState("invoice_count") = dt_result.Rows.Count
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('" & ex.Message & "')", True)
        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If (e.CommandName = "printProcess") Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridView1.Rows(index)
                Dim p_link As String = "printPatientProcesses.aspx?invID=" & Val(txt_invoice_no.Text) & "&pID=" & (row.Cells(2).Text)
                'Response.Write("<script type='text/javascript'>")
                'Response.Write("window.open('" & p_link & "','_blank');")
                'Response.Write("</script>")
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "window.open('" & p_link & "','_blank');", True)
                'Response.Redirect("printPatientProcesses.aspx?invID=" & Val(txt_invoice_no.Text) & "&pID=" & (row.Cells(1).Text), False)
            End If

            If (e.CommandName = "returnProcess") Then

                If ViewState("invoice_count") > 1 Then
                    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim row As GridViewRow = GridView1.Rows(index)

                    Dim returnInvoice As New SqlCommand
                    returnInvoice.Connection = insurance_SQLcon
                    returnInvoice.CommandText = "INC_deletePatientInvoice"
                    returnInvoice.CommandType = CommandType.StoredProcedure
                    returnInvoice.Parameters.AddWithValue("@inv_no", ViewState("invoice_no"))
                    returnInvoice.Parameters.AddWithValue("@patient_no", (row.Cells(3).Text))
                    insurance_SQLcon.Open()
                    returnInvoice.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                    getData()

                    add_action(1, 3, 3, "إرجاع حركة المنتفع: " & (row.Cells(4).Text) & " من المطالبة رقم: " & ViewState("invoice_no"), Session("INC_User_Id"), GetIPAddress())

                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية إرجاع الحركة بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
                Else
                    Dim returnInvoice As New SqlCommand
                    returnInvoice.Connection = insurance_SQLcon
                    returnInvoice.CommandText = "INC_deleteInvoice"
                    returnInvoice.CommandType = CommandType.StoredProcedure
                    returnInvoice.Parameters.AddWithValue("@inv_no", ViewState("invoice_no"))
                    insurance_SQLcon.Open()
                    returnInvoice.ExecuteNonQuery()
                    insurance_SQLcon.Close()

                    add_action(1, 3, 3, "إرجاع المطالبة رقم: " & ViewState("invoice_no"), Session("INC_User_Id"), GetIPAddress())

                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية إرجاع المطالبة بنجاح',
                showConfirmButton: false,
                timer: 1500
            });
            window.setTimeout(function () {
                window.location.href = 'invoicesList.aspx';
            }, 1500);", True)
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click

        getData()
        getPatientProcessesInvoice()

        Dim viewer As ReportViewer = New ReportViewer()

        Dim datasource As New ReportDataSource("invoiceContentDS", main_ds.Tables("INC_IvoicesProcesses"))
        Dim datasource1 As New ReportDataSource("patientProcesses", main_ds.Tables("patientProcessesInvoice"))
        viewer.LocalReport.DataSources.Clear()
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/invoiceContent.rdlc")
        viewer.LocalReport.DataSources.Add(datasource)
        viewer.LocalReport.DataSources.Add(datasource1)

        Dim rp1 As ReportParameter
        Dim rp2 As ReportParameter
        Dim rp3 As ReportParameter
        Dim rp4 As ReportParameter
        'Dim rp5 As ReportParameter
        'Dim rp6 As ReportParameter
        'Dim rp7 As ReportParameter
        'Dim rp8 As ReportParameter
        'Dim rp9 As ReportParameter
        'Dim rp10 As ReportParameter
        'Dim rp11 As ReportParameter

        rp1 = New ReportParameter("from_dt", txt_start_dt.Text.ToString)
        rp2 = New ReportParameter("to_dt", txt_end_dt.Text.ToString)
        rp3 = New ReportParameter("INVOICE_NO", ViewState("invoice_no").ToString)
        rp4 = New ReportParameter("main_company", ViewState("main_company").ToString)

        viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3, rp4})

        Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
        Dim r As String = "~/Reports/invoiceContent.rdlc"
        ' Page.Controls.Add(rv)

        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim FolderLocation As String
        FolderLocation = Server.MapPath("~/Reports")
        Dim filepath As String = FolderLocation & "/invoiceContent" & Session("INC_User_Id") & ".pdf"
        If Directory.Exists(filepath) Then
            File.Delete(filepath)
        End If
        bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
            encoding, extension, streamids, warnings)
        Dim fs As New FileStream(FolderLocation & "/invoiceContent" & Session("INC_User_Id") & ".pdf", FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        Response.Redirect("~/Reports/invoiceContent" & Session("INC_User_Id") & ".pdf")
    End Sub

    Private Sub btn_return_Click(sender As Object, e As EventArgs) Handles btn_return.Click
        Try
            Dim returnInvoice As New SqlCommand
            returnInvoice.Connection = insurance_SQLcon
            returnInvoice.CommandText = "INC_deleteInvoice"
            returnInvoice.CommandType = CommandType.StoredProcedure
            returnInvoice.Parameters.AddWithValue("@inv_no", ViewState("invoice_no"))
            insurance_SQLcon.Open()
            returnInvoice.ExecuteNonQuery()
            insurance_SQLcon.Close()

            add_action(1, 3, 3, "إرجاع المطالبة رقم: " & ViewState("invoice_no"), Session("INC_User_Id"), GetIPAddress())

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية إرجاع المطالبة بنجاح',
                showConfirmButton: false,
                timer: 1500
            });
            window.setTimeout(function () {
                window.location.href = 'invoicesList.aspx';
            }, 1500);", True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound

        If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim cell As TableCell = e.Row.Cells(8)

                Dim btn_print_one As LinkButton = cell.FindControl("btn_print_one")
                Dim btn_return_one As LinkButton = cell.FindControl("btn_return_one")
                btn_print_one.Visible = Session("User_per")("print_motalba")
                btn_return_one.Visible = Session("User_per")("return_motalba")
            End If
        End If

    End Sub

    Private Sub btn_print_details_Click(sender As Object, e As EventArgs) Handles btn_print_details.Click

        getPatientProcessesInvoice()

        Dim viewer As ReportViewer = New ReportViewer()
        Dim datasource As New ReportDataSource("patientProcesses", main_ds.Tables("patientProcessesInvoice"))
        viewer.LocalReport.DataSources.Clear()
        viewer.ProcessingMode = ProcessingMode.Local
        viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/motalbaDetailes.rdlc")
        viewer.LocalReport.DataSources.Add(datasource)

        Dim rv As New Microsoft.Reporting.WebForms.ReportViewer
        Dim r As String = "~/Reports/motalbaDetailes.rdlc"
        ' Page.Controls.Add(rv)

        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim FolderLocation As String
        FolderLocation = Server.MapPath("~/Reports")
        Dim filepath As String = FolderLocation & "/motalbaDetailes" & Session("INC_User_Id") & ".pdf"
        If Directory.Exists(filepath) Then
            File.Delete(filepath)
        End If
        bytes = viewer.LocalReport.Render("PDF", Nothing, mimeType,
            encoding, extension, streamids, warnings)
        Dim fs As New FileStream(FolderLocation & "/motalbaDetailes" & Session("INC_User_Id") & ".pdf", FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()
        Response.Redirect("~/Reports/motalbaDetailes" & Session("INC_User_Id") & ".pdf")
    End Sub
End Class