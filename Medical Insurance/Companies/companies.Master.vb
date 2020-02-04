Imports System.Data.SqlClient
Public Class companies
    Inherits System.Web.UI.MasterPage

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        
        If IsPostBack = False Then
            hl_company_users_list.NavigateUrl = "~/Companies/LISTPATIANT.aspx?cID=" & Session("company_id")

            If Session("company_id") IsNot Nothing Then
                Dim sel_com As New SqlCommand("SELECT (SELECT TOP 1 (CONVERT(VARCHAR, DATE_START, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_START, (SELECT TOP 1 (CONVERT(VARCHAR, DATE_END, 111)) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY N DESC) AS DATE_END, (SELECT TOP 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID ORDER BY CONTRACT_NO DESC) AS CONTRACT_NO, C_NAME_ARB, C_NAME_ENG, C_STATE, (CASE WHEN (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL ) IS NULL THEN  '-' ELSE (SELECT C_NAME_ARB FROM [INC_COMPANY_DATA] AS X WHERE X.C_ID=[INC_COMPANY_DATA].C_LEVEL) END)AS MAIN_COMPANY FROM INC_COMPANY_DATA WHERE C_ID = " & Session("company_id"), insurance_SQLcon)
                Dim dt_result As New DataTable
                dt_result.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_result.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()
                If dt_result.Rows.Count > 0 Then
                    Dim dr_company = dt_result.Rows(0)
                    lbl_company_name.Text = dr_company!C_NAME_ARB
                    lbl_en_name.Text = dr_company!C_NAME_ENG
                End If

            Else
                Session.RemoveAll()
                Session.Clear()
                Panel_company_info.Visible = False
            End If

        End If

    End Sub

End Class