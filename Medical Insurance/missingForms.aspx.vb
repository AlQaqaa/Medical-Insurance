Imports System.Data.SqlClient
Imports System.Globalization

Public Class missingForms
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
                If Session("User_per")("missing") = False Then
                    Response.Redirect("Default.aspx", True)
                    Exit Sub
                End If
            End If
            'txt_start_dt.Text = Today.Date
            'txt_end_dt.Text = Today.Date



        End If
    End Sub

    Private Sub getData()
        Dim s As String
        s = "select c_id,req_id_code_print,req_code , Req_No ,convert(varchar, Processes_Date, 111) as Processes_Date, (case when HAG_Processes.Processes_State = 0 then 'لم تتم التسوية' when HAG_Processes.Processes_State = 2 then 'تمت التسوية' else '' end) as Processes_State, INC_PATIANT.NAME_ARB, INC_PATIANT.CARD_NO, Main_Clinic.Clinic_AR_Name, Main_SubServices.SubService_AR_Name from HAG_Processes 
inner join HAG_Request on HAG_Processes.Processes_ID = Req_PID 
inner join INC_PATIANT on INC_PATIANT.INC_Patient_Code = HAG_Processes.Processes_Reservation_Code
inner join Main_Clinic on Main_Clinic.clinic_id = HAG_Processes.Processes_Cilinc
inner join Main_SubServices on Main_SubServices.SubService_ID = HAG_Processes.Processes_SubServices
inner join HAG_Processes_Doctor on HAG_Processes_Doctor.Doctor_Processes_ID = HAG_Processes.Processes_ID
where Processes_State < 3 and Processes_ID not in (select INC_MOTALBAT.Processes_ID from INC_MOTALBAT)"

        If txt_start_dt.Text <> "" And txt_end_dt.Text <> "" Then
            Dim start_dt As String = DateTime.ParseExact(txt_start_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
            Dim end_dt As String = DateTime.ParseExact(txt_end_dt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)
            s = s & " And Processes_Date >= '" & start_dt & "' AND Processes_Date <= '" & end_dt & "'"
        End If


        If Val(ddl_clinics.SelectedValue) > 0 Then s += " and Processes_Cilinc  =" & ddl_clinics.SelectedValue
        If Val(ddl_companies.SelectedValue) > 0 Then s += " and c_id  =" & ddl_companies.SelectedValue
        If Val(ddl_doctors.SelectedValue) > 0 Then s += " and HAG_Processes_Doctor.Processes_Doctor_ID  =" & ddl_doctors.SelectedValue


        s += " order by  Processes_Date"

        Dim sel_com As New SqlCommand(s, insurance_SQLcon)

        Dim dt_result As New DataTable
        If insurance_SQLcon.State = ConnectionState.Open Then insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            GridView1.DataSource = dt_result
            GridView1.DataBind()
            Label1.Text = dt_result.Rows.Count
        Else
            GridView1.DataSource = ""
            GridView1.DataBind()
            Label1.Text = 0
        End If
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If (e.CommandName = "printProcess") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Dim a As Integer
            Dim id_code As String

            id_code = 333

            Select Case row.Cells(3).Text
                Case 2 : id_code = 444 'Allianz
                Case 9, 43 : id_code = 555 'Libya Tamen
                Case 56 : id_code = 666 'Naher

            End Select

            '  If row.Cells(2).Text = 10 Or row.Cells(2).Text = 11 Or row.Cells(2).Text = 12 Or row.Cells(2).Text = 13 Then a = 1
            '    If row.Cells(2).Text = 333 Or row.Cells(2).Text = 444 Or row.Cells(2).Text = 555 Or row.Cells(2).Text = 666 Then a = 2
            ' If row.Cells(2).Text <> 10 And row.Cells(2).Text <> 11 And row.Cells(2).Text <> 12 And row.Cells(2).Text <> 13 Then a = 3



            '       If a = 1 Then Response.Redirect("http://10.10.1.10:438/PrintIncMultiServiceRec.aspx?Prm=" + row.Cells(2).Text + "&id_code=" + row.Cells(2).Text, True)
            ' If a = 2 Then Response.Redirect("http://10.10.1.10:438/PrintIncMultiServiceRec.aspx?Prm=" + row.Cells(2).Text + "&id_code=" + row.Cells(2).Text, True)
            '    If a = 3 Then Response.Redirect("http://10.10.1.10:438/PrintMultiServiceIncForm.aspx?Prm=" + row.Cells(2).Text + "&id_code=" + row.Cells(2).Text, True)
            Response.Redirect("http://10.10.1.10:438/PrintMultiServiceIncForm.aspx?Prm=" + row.Cells(2).Text + "&id_code=" + id_code, True)

        End If
    End Sub

    Protected Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        getData()
    End Sub
End Class