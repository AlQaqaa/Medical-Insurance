Imports System.Data.SqlClient
Imports System.Globalization

Public Class invoiceContent
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            ViewState("invoice_no") = Val(Request.QueryString("invID"))

            If ViewState("invoice_no") <> 0 Then
                Page.Title = "محتويات الفاتورة رقم " & ViewState("invoice_no")
                txt_invoice_no.Text = ViewState("invoice_no")
                Dim sel_com As New SqlCommand("SELECT INVOICE_NO, C_ID, CONVERT(VARCHAR, DATE_FROM, 23) AS DATE_FROM, CONVERT(VARCHAR, DATE_TO, 23) AS DATE_TO FROM INC_INVOICES WHERE INVOICE_NO = " & ViewState("invoice_no"), insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()

                If dt_result.Rows.Count > 0 Then
                    Dim dr_inv = dt_result.Rows(0)
                    txt_company_name.Text = dr_inv!C_ID
                    ViewState("company_no") = dr_inv!C_ID
                    txt_start_dt.Text = dr_inv!DATE_FROM
                    txt_end_dt.Text = dr_inv!DATE_TO

                    getData()

                End If

            Else
                Response.Redirect("Default.aspx", False)
            End If

        End If

    End Sub

    Sub getData()
        Try
            Dim sel_com As New SqlCommand("SELECT P_ID,SUM(PROCESSES_RESIDUAL) AS PROCESSES_RESIDUAL,(SELECT CARD_NO FROM DBO.INC_PATIANT WHERE INC_PATIANT.PINC_ID = P_ID) AS CARD_NO,(SELECT BAGE_NO FROM DBO.INC_PATIANT WHERE INC_PATIANT.PINC_ID = P_ID) AS BAGE_NO, (SELECT NAME_ARB FROM DBO.INC_PATIANT WHERE INC_PATIANT.PINC_ID = P_ID) AS NAME_ARB FROM INC_IVOICESPROCESSES WHERE INVOICE_NO = " & ViewState("invoice_no") & " AND C_ID = " & ViewState("company_no") & " AND MOTALABA_STS = 1 GROUP BY P_ID", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
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
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class