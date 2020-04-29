Imports System.Data.SqlClient
Public Class statistics
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            getData()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            txt_invoce_no.Enabled = True
            txt_invoce_no.Focus()
            ddl_invoice_type.Enabled = True
        Else
            txt_invoce_no.Enabled = False
            ddl_invoice_type.Enabled = False
        End If
    End Sub

    Private Sub getData()

        Try
            Dim sql_str As String = "SELECT Processes_Reservation_Code, SUBSTRING([Processes_Reservation_Code],9 , 6) AS PINC_ID, convert(varchar, Processes_Date, 23) AS Processes_Date, Processes_Time, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CompanyProcesses.Processes_Cilinc) AS Processes_Cilinc, (SELECT SubService_AR_Name FROM Main_SubServices WHERE Main_SubServices.SubService_ID = INC_CompanyProcesses.Processes_SubServices) AS Processes_SubServices, Processes_Price, Processes_Paid, Processes_Residual, ISNULL((SELECT MedicalStaff_AR_Name FROM Main_MedicalStaff WHERE Main_MedicalStaff.MedicalStaff_ID = INC_CompanyProcesses.doctor_id), '') AS MedicalStaff_AR_Name, ISNULL((SELECT NAME_ARB FROM INC_PATIANT WHERE INC_PATIANT.PINC_ID = SUBSTRING(INC_CompanyProcesses.Processes_Reservation_Code,9 , 6)), '') AS PATIENT_NAME FROM INC_CompanyProcesses WHERE Processes_State in (0,2)"

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
            Else
                dt_result.Rows.Clear()
                GridView1.DataSource = dt_result
                GridView1.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click

    End Sub
End Class