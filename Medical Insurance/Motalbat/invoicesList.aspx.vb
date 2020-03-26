Imports System.Data.SqlClient

Public Class invoicesList
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

        End If
    End Sub

    Sub getData()
        Dim sel_com As New SqlCommand("SELECT INVOICE_NO, CONVERT(VARCHAR, INCOICE_CREATE_DT, 23) AS INCOICE_CREATE_DT,(SELECT C_Name_Arb FROM INC_COMPANY_DATA WHERE INC_COMPANY_DATA.C_ID = INC_INVOICES.C_ID) AS COMPANY_NAME, CONVERT(VARCHAR, DATE_FROM, 23) AS DATE_FROM, CONVERT(VARCHAR, DATE_TO, 23) AS DATE_TO,(select SUM(Processes_Residual) from INC_IvoicesProcesses where INC_IvoicesProcesses.INVOICE_NO in (INC_INVOICES.INVOICE_NO)) as total_val FROM INC_INVOICES WHERE C_ID = " & ddl_companies.SelectedValue, insurance_SQLcon)
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
        Else
            dt_result.Rows.Clear()
            GridView1.DataSource = dt_result
            GridView1.DataBind()
            Label1.Text = "<div class='alert alert-danger' role='alert'>لا يوجد بيانات لعرضها</div>"
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
    End Sub
End Class