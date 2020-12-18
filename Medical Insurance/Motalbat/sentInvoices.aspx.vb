Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports System.Globalization

Public Class sentInvoices
    Inherits System.Web.UI.Page
    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("print_motalba") = False Then
                    btn_send.Visible = False
                End If

            End If
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")

            If CheckBox1.Checked = True Then
                ch.Checked = True
            Else
                ch.Checked = False

            End If
        Next
    End Sub

    Sub getData()
        Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
        Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)

        Dim sql_str As String = "SELECT INVOICE_NO, CONVERT(VARCHAR, INCOICE_CREATE_DT, 23) AS INCOICE_CREATE_DT,(SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_INVOICES.C_ID) AS COMPANY_NAME, CONVERT(VARCHAR, DATE_FROM, 23) AS DATE_FROM, CONVERT(VARCHAR, DATE_TO, 23) AS DATE_TO,isnull((select SUM(Processes_Residual) from INC_IvoicesProcesses where INC_IvoicesProcesses.INVOICE_NO in (INC_INVOICES.INVOICE_NO)), 0) as total_val FROM INC_INVOICES WHERE INCOICE_STS = 1 AND C_ID = " & ddl_companies.SelectedValue

        If ddl_invoice_type.SelectedValue <> -1 Then
            sql_str = sql_str & " AND INVOICE_TYPE = " & ddl_invoice_type.SelectedValue
        End If

        If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
            sql_str = sql_str & " AND INCOICE_CREATE_DT BETWEEN '" & start_dt & "' AND '" & end_dt & "'"
        End If

        sql_str = sql_str & " ORDER BY INVOICE_NO DESC"

        Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView1.DataSource = dt_result
            GridView1.DataBind()
            Label1.Text = ""
            btn_send.Visible = True
            CheckBox1.Visible = True
        Else
            dt_result.Rows.Clear()
            GridView1.DataSource = dt_result
            GridView1.DataBind()
            Label1.Text = "<div class='alert alert-danger' role='alert'>لا يوجد بيانات لعرضها</div>"
            btn_send.Visible = False
            CheckBox1.Visible = False
        End If
    End Sub

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        getData()
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "INVOICE_DETAILES") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Response.Redirect("invoiceContent.aspx?invID=" & Val(row.Cells(0).Text), False)
        End If

        If (e.CommandName = "printInvoice") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Response.Redirect("invoiceContent.aspx?invID=" & Val(row.Cells(0).Text), False)

            Try
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()

                Using main_ds
                    Dim sel_com As New SqlCommand("SELECT P_ID,SUM(PROCESSES_RESIDUAL) AS PROCESSES_RESIDUAL,CARD_NO,BAGE_NO,NAME_ARB, INC_COMPANY_DATA.C_Name_Arb
FROM INC_IVOICESPROCESSES 
INNER JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_IVOICESPROCESSES.P_ID
INNER JOIN INC_COMPANY_DATA ON INC_COMPANY_DATA.C_ID = INC_IVOICESPROCESSES.C_ID WHERE INVOICE_NO = " & Val(row.Cells(0).Text) & " AND MOTALABA_STS = 1 GROUP BY P_ID,CARD_NO, BAGE_NO,NAME_ARB,INC_COMPANY_DATA.C_Name_Arb,INC_PATIANT.INC_Patient_Code ORDER BY NAME_ARB")
                    Using sda As New SqlDataAdapter()
                        sel_com.Connection = insurance_SQLcon
                        sda.SelectCommand = sel_com
                        sda.Fill(main_ds, "INC_IvoicesProcesses")
                    End Using
                End Using

                Dim viewer As ReportViewer = New ReportViewer()

                Dim datasource As New ReportDataSource("invoiceContentDS", main_ds.Tables("INC_IvoicesProcesses"))
                viewer.LocalReport.DataSources.Clear()
                viewer.ProcessingMode = ProcessingMode.Local
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/invoiceContent.rdlc")
                viewer.LocalReport.DataSources.Add(datasource)

                Dim rp1 As ReportParameter
                Dim rp2 As ReportParameter
                Dim rp3 As ReportParameter
                'Dim rp4 As ReportParameter
                'Dim rp5 As ReportParameter
                'Dim rp6 As ReportParameter
                'Dim rp7 As ReportParameter
                'Dim rp8 As ReportParameter
                'Dim rp9 As ReportParameter
                'Dim rp10 As ReportParameter
                'Dim rp11 As ReportParameter

                rp1 = New ReportParameter("from_dt", (row.Cells(6).Text).ToString)
                rp2 = New ReportParameter("to_dt", (row.Cells(7).Text).ToString)
                rp3 = New ReportParameter("INVOICE_NO", (row.Cells(0).Text).ToString)

                viewer.LocalReport.SetParameters(New ReportParameter() {rp1, rp2, rp3})

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
                Response.Redirect("~/Reports/invoiceContent" & Session("INC_User_Id") & ".pdf", False)

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub ddl_invoice_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_invoice_type.SelectedIndexChanged
        If ddl_companies.SelectedValue <> 0 Then
            getData()
        End If
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim cell As TableCell = e.Row.Cells(9)
                Dim btn_print1 As Button = cell.FindControl("btn_print1")
                btn_print1.Visible = Session("User_per")("print_motalba")
            End If
        End If

    End Sub

    Private Sub btn_send_Click(sender As Object, e As EventArgs) Handles btn_send.Click
        Try
            Dim ch_counter As Integer = 0

            For Each dd As GridViewRow In GridView1.Rows
                Dim ch As CheckBox = dd.FindControl("CheckBox2")

                If ch.Checked = True Then
                    ch_counter = ch_counter + 1

                    Dim update_com As New SqlCommand("UPDATE INC_INVOICES SET INCOICE_STS=0 WHERE INVOICE_NO = " & dd.Cells(0).Text, insurance_SQLcon)
                    If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
                    insurance_SQLcon.Open()
                    update_com.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                End If
            Next

            If ch_counter = 0 Then
                lbl_msg.Visible = True
                lbl_msg.Text = "خطأ! لم يتم اختيار فاتورة، يرجى تحديد فاتورة أو أكثر"
                lbl_msg.ForeColor = Drawing.Color.Red
                Exit Sub
            Else
                lbl_msg.Visible = False
            End If

            getData()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت العملية بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()
    End Sub
End Class