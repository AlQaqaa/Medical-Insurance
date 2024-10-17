Imports System.Data.SqlClient

Public Class ClinicException
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("company_services") = False Then
                    Response.Redirect("../Default.aspx", True)
                End If
            End If



            If Session("company_id") = Nothing Then
                Response.Redirect("Default.aspx")
            End If

            ViewState("company_no") = Val(Session("company_id"))

            If ViewState("company_no") = 0 Then
                Response.Redirect("Default.aspx")
            End If

            Dim sel_com As New SqlCommand("SELECT C_id, (SELECT top 1 (DATE_END) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS DATE_END, (SELECT top 1 (CONTRACT_NO) FROM INC_COMPANY_DETIAL WHERE INC_COMPANY_DETIAL.C_ID = INC_COMPANY_DATA.C_ID order by n desc) AS CONTRACT_NO, C_NAME_ARB, C_NAME_ENG FROM INC_COMPANY_DATA WHERE C_id = " & ViewState("company_no"), insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()
            If dt_result.Rows.Count > 0 Then
                Dim dr_company = dt_result.Rows(0)
                ViewState("contract_no") = dr_company!CONTRACT_NO
            End If
            getClinicAvailable()

        End If

    End Sub

    Sub getClinicAvailable()
        Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS Clinic_AR_Name, ISNULL(IsException, 0) AS IsException FROM INC_CLINICAL_RESTRICTIONS WHERE CLINIC_ID <> 0 AND C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView1.DataSource = dt_result
            GridView1.DataBind()

            For i = 0 To dt_result.Rows.Count - 1
                Dim dr = dt_result.Rows(i)
                Dim dd As GridViewRow = GridView1.Rows(i)

                Dim ch As CheckBox = dd.FindControl("CheckBox2")
                ch.Checked = If(dr!IsException = True, True, False)

            Next
        End If
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        Try
            For Each dd As GridViewRow In GridView1.Rows
                Dim ch As CheckBox = dd.FindControl("CheckBox2")
                Dim sqlComm As New SqlCommand("UPDATE INC_CLINICAL_RESTRICTIONS SET IsException=@IsException WHERE C_ID=@C_ID AND CONTRACT_NO=@CONTRACT_NO AND Clinic_ID=@Clinic_ID", insurance_SQLcon)
                sqlComm.Parameters.AddWithValue("IsException", ch.Checked)
                sqlComm.Parameters.AddWithValue("C_ID", ViewState("company_no"))
                sqlComm.Parameters.AddWithValue("CONTRACT_NO", ViewState("contract_no"))
                sqlComm.Parameters.AddWithValue("Clinic_ID", dd.Cells(0).Text)
                If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
                sqlComm.ExecuteNonQuery()
                insurance_SQLcon.Close()

            Next
            getClinicAvailable()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية حفظ البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)

        Catch ex As Exception

        End Try
    End Sub
End Class