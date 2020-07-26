Imports System.Data.SqlClient
Imports System.Globalization

Public Class dorctorsForms
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("doctors_settled") = False Then
                    Response.Redirect("Default.aspx", True)
                    Exit Sub
                End If
            End If

            getData()
        End If
    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT pros_code, Processes_ID, Processes_Reservation_Code, INC_PATIANT.PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, Clinic_AR_Name, SubService_AR_Name, Processes_Price, Processes_Paid, Processes_Residual, ISNULL(MedicalStaff_AR_Name, '') AS MedicalStaff_AR_Name, ISNULL(NAME_ARB, '') AS PATIENT_NAME FROM INC_CompanyProcesses 
                INNER JOIN Main_Clinic ON Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc
                INNER JOIN Main_MedicalStaff ON Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id
                INNER JOIN INC_PATIANT ON INC_PATIANT.PINC_ID = INC_CompanyProcesses.PINC_ID
                INNER JOIN Main_SubServices ON Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices
                WHERE Processes_State = 2 AND INC_CompanyProcesses.C_ID <> 0 AND Processes_Residual <> 0 AND Processes_ID NOT IN (SELECT ewa_process_id FROM EWA_Processes WHERE EWA_Processes.ewa_process_id = INC_CompanyProcesses.Processes_ID)"

            If Val(ddl_clinics.SelectedValue) <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.Processes_Cilinc = " & ddl_clinics.SelectedValue
            End If

            If Val(dll_doctors.SelectedValue) <> 0 Then
                sql_str = sql_str & " AND INC_CompanyProcesses.doctor_id = " & dll_doctors.SelectedValue
            End If

            'If ddl_invoice_type.SelectedValue = 1 Then
            '    sql_str = sql_str & " AND Processes_ID NOT IN (SELECT ewa_process_id FROM EWA_Processes WHERE EWA_Processes.ewa_process_id = INC_CompanyProcesses.Processes_ID)"
            'ElseIf ddl_invoice_type.SelectedValue = 2 Then
            '    sql_str = sql_str & " AND Processes_ID IN (SELECT Ewa_Exit_ID FROM Ewa_Exit WHERE Ewa_Exit.Ewa_Exit_ID = INC_CompanyProcesses.Processes_ID)"
            'End If

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
                CheckBox1.Visible = True

            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
                CheckBox1.Visible = False

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('" & ex.Message & "')", True)
        End Try

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

    Private Sub btn_confirm_Click(sender As Object, e As EventArgs) Handles btn_confirm.Click
        Dim ch_counter As Integer = 0

        For Each dd As GridViewRow In GridView1.Rows
            Dim ch As CheckBox = dd.FindControl("CheckBox2")

            If ch.Checked = True Then
                ch_counter = ch_counter + 1
                Dim update_sts As New SqlCommand("UPDATE HAG_Processes SET Processes_State = 4 WHERE Processes_ID = " & dd.Cells(1).Text, insurance_SQLcon)
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                update_sts.ExecuteNonQuery()
                insurance_SQLcon.Close()
                getData()

            End If

        Next

        If ch_counter = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('خطأ! لم تقم باختيار أي حركة');", True)
            Exit Sub
        End If

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت التسوية مع الطبيب بنجاح',
                showConfirmButton: false,
                timer: 1000
            });
            window.setTimeout(function () {
                window.location.href = 'dorctorsForms.aspx';
            }, 1000);", True)

        'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('تمت التسوية مع الطبيب بنجاح');", True)
    End Sub

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        getData()
    End Sub

    Private Sub dll_doctors_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dll_doctors.SelectedIndexChanged
        getData()
    End Sub
End Class