Imports System.Data.SqlClient

Public Class _Default
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub getData()
        Try
            Dim sel_com As New SqlCommand("SELECT INVOICE_NO,P_ID,SUM(PROCESSES_RESIDUAL) AS PROCESSES_RESIDUAL,(SELECT CARD_NO FROM DBO.INC_PATIANT WHERE INC_PATIANT.PINC_ID = P_ID) AS CARD_NO,(SELECT BAGE_NO FROM DBO.INC_PATIANT WHERE INC_PATIANT.PINC_ID = P_ID) AS BAGE_NO, (SELECT NAME_ARB FROM DBO.INC_PATIANT WHERE INC_PATIANT.PINC_ID = P_ID) AS NAME_ARB FROM IBNSINA_DATABASE.DBO.INC_IVOICESPROCESSES WHERE C_ID = " & ddl_companies.SelectedValue & " AND MOTALABA_STS = 1 GROUP BY INVOICE_NO, P_ID", insurance_SQLcon)
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

    Private Sub ddl_companies_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_companies.SelectedIndexChanged
        If ddl_companies.SelectedValue <> 0 Then
            getData()
        End If
    End Sub


    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        If ddl_companies.SelectedValue <> 0 Then
            getData()
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Company Name ################
        If (e.CommandName = "printKasima") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Response.Write("<script type='text/javascript'>")
            Response.Write("window.open('../Default.aspx','_blank');")
            Response.Write("</script>")
        End If
    End Sub
End Class