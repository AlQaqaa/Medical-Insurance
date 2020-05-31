Imports System.Data.SqlClient
Public Class companyClinic
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

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

            fillListClinic()
            getClinicAvailable()

        End If
    End Sub

    Sub fillListClinic()
        source_list.Items.Clear()
        Dim sel_com As New SqlCommand("SELECT Clinic_ID, Clinic_AR_Name FROM Main_Clinic", insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        source_list.DataSource = dt_result
        source_list.DataValueField = "Clinic_ID"
        source_list.DataTextField = "Clinic_AR_Name"
        source_list.DataBind()
    End Sub

    'Sub getClinicAvailable()
    '    Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.Clinic_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS CLINIC_NAME FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
    '    Dim dt_result As New DataTable
    '    dt_result.Rows.Clear()
    '    insurance_SQLcon.Close()
    '    insurance_SQLcon.Open()
    '    dt_result.Load(sel_com.ExecuteReader)
    '    insurance_SQLcon.Close()

    '    If dt_result.Rows.Count > 0 Then
    '        Dim resultString As String = ""
    '        Dim isFirstResult = True

    '        For i = 0 To dt_result.Rows.Count - 1
    '            Dim dr = dt_result.Rows(i)

    '            ' Delete Available Clinics From Clinics ListBox
    '            Dim liItem As ListItem = source_list.Items.FindByValue(dr!Clinic_ID)
    '            If (liItem IsNot Nothing) Then
    '                source_list.Items.Remove(liItem)
    '            End If

    '            If Not isFirstResult Then
    '                resultString &= String.Format(" <span class='badge badge-pill badge-info p-2'>{0}</span>", dr!CLINIC_NAME)
    '            Else
    '                isFirstResult = False
    '                resultString &= String.Format("<span class='badge badge-pill badge-info p-2'>{0}</span>", dr!CLINIC_NAME)
    '            End If
    '        Next

    '        Literal1.Text = resultString
    '    Else
    '        Literal1.Text = "لا يوجد عيادة مغطاة"
    '    End If
    'End Sub

    Sub getClinicAvailable()
        Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS Clinic_AR_Name, (CASE WHEN (GROUP_NO) <> 0 THEN (SELECT MAX_VALUE FROM INC_CLINIC_GROUP WHERE INC_CLINIC_GROUP.GROUP_NO = INC_CLINICAL_RESTRICTIONS.GROUP_NO) ELSE (SELECT MAX_VALUE FROM INC_CLINICAL_RESTRICTIONS AS M_X WHERE M_X.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID AND M_X.C_ID = INC_CLINICAL_RESTRICTIONS.C_ID) END) AS MAX_VALUE, (CASE WHEN (GROUP_NO) <> 0 THEN (SELECT PER_T FROM INC_CLINIC_GROUP WHERE INC_CLINIC_GROUP.GROUP_NO = INC_CLINICAL_RESTRICTIONS.GROUP_NO) ELSE (SELECT PER_T FROM INC_CLINICAL_RESTRICTIONS AS M_X WHERE M_X.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID AND M_X.C_ID = INC_CLINICAL_RESTRICTIONS.C_ID) END) AS PER_T, (CASE WHEN GROUP_NO = 0 THEN '-' ELSE 'مشتركة' END) AS GROUP_CLINIC FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
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

                ' Delete Available Clinics From Clinics ListBox
                Dim liItem As ListItem = source_list.Items.FindByValue(dr!Clinic_ID)
                If (liItem IsNot Nothing) Then
                    source_list.Items.Remove(liItem)
                End If

            Next
        End If
    End Sub

    Protected Sub LeftClick(ByVal sender As Object, ByVal e As EventArgs)
        'List will hold items to be removed.
        Dim removedItems As List(Of ListItem) = New List(Of ListItem)

        'Loop and transfer the Items to Destination ListBox.
        For Each item As ListItem In dist_list.Items
            If item.Selected Then
                item.Selected = False
                source_list.Items.Add(item)
                removedItems.Add(item)
            End If
        Next

        'Loop and remove the Items from the Source ListBox.
        For Each item As ListItem In removedItems
            dist_list.Items.Remove(item)
        Next
    End Sub

    Protected Sub RightClick(ByVal sender As Object, ByVal e As EventArgs)
        'List will hold items to be removed.
        Dim removedItems As List(Of ListItem) = New List(Of ListItem)

        'Loop and transfer the Items to Destination ListBox.
        For Each item As ListItem In source_list.Items
            If item.Selected Then
                item.Selected = False
                dist_list.Items.Add(item)
                removedItems.Add(item)
            End If
        Next

        'Loop and remove the Items from the Source ListBox.
        For Each item As ListItem In removedItems
            source_list.Items.Remove(item)
        Next
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        If Val(txt_session_count.Text) <> 0 And CDec(txt_max_val.Text) <> 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'خطأ! لا يمكن إدخال السقف وعدد الجلسات لنفس العيادة',
                showConfirmButton: false,
                timer: 1500
            });", True)

            Exit Sub
        End If

        If Val(txt_session_count.Text) <> 0 And dist_list.Items.Count > 1 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'خطأ! لا يمكن مشاركة عدد الجلسات لأكثر من عيادة',
                showConfirmButton: false,
                timer: 1500
            });", True)

            Exit Sub
        End If

        If dist_list.Items.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'خطأ! يجب اختيار عيادة واحدة على الأقل',
                showConfirmButton: false,
                timer: 1500
            });", True)

            Exit Sub
        End If

        Try
            If dist_list.Items.Count = 1 Then
                Dim insClinic As New SqlCommand
                insClinic.Connection = insurance_SQLcon
                insClinic.CommandText = "INC_addCompanyClinic"
                insClinic.CommandType = CommandType.StoredProcedure
                insClinic.Parameters.AddWithValue("@cID", ViewState("company_no"))
                insClinic.Parameters.AddWithValue("@clinicID", dist_list.Items.Item(0).Value)
                insClinic.Parameters.AddWithValue("@contractNo", ViewState("contract_no"))
                insClinic.Parameters.AddWithValue("@maxClinicValue", CDec(txt_max_val.Text))
                insClinic.Parameters.AddWithValue("@clinicPersonPer", 0)
                insClinic.Parameters.AddWithValue("@sessionCount", Val(txt_session_count.Text))
                insClinic.Parameters.AddWithValue("@group_no", 0)
                insClinic.Parameters.AddWithValue("@userId", Session("User_Id"))
                insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
                insurance_SQLcon.Open()
                insClinic.ExecuteNonQuery()
                insurance_SQLcon.Close()
                insClinic.CommandText = ""
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية حفظ البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
                fillListClinic()
                getClinicAvailable()
                clrTxt()
            Else
                Dim group_no As Integer
                Dim insGroup As New SqlCommand
                insGroup.Connection = insurance_SQLcon
                insGroup.CommandText = "INC_newClinicGroup"
                insGroup.CommandType = CommandType.StoredProcedure
                insGroup.Parameters.AddWithValue("@maxVal", CDec(txt_max_val.Text))
                insGroup.Parameters.AddWithValue("@personPer", 0)
                insGroup.Parameters.AddWithValue("@groupNo", SqlDbType.Int).Direction = ParameterDirection.Output
                insurance_SQLcon.Open()
                insGroup.ExecuteNonQuery()
                group_no = insGroup.Parameters("@groupNo").Value.ToString()
                insurance_SQLcon.Close()
                insGroup.CommandText = ""

                For i = 0 To dist_list.Items.Count - 1
                    Dim it_val = dist_list.Items.Item(i).Value
                    Dim insClinic As New SqlCommand
                    insClinic.Connection = insurance_SQLcon
                    insClinic.CommandText = "INC_addCompanyClinic"
                    insClinic.CommandType = CommandType.StoredProcedure
                    insClinic.Parameters.AddWithValue("@cID", ViewState("company_no"))
                    insClinic.Parameters.AddWithValue("@clinicID", it_val)
                    insClinic.Parameters.AddWithValue("@contractNo", ViewState("contract_no"))
                    insClinic.Parameters.AddWithValue("@maxClinicValue", CDec(txt_max_val.Text))
                    insClinic.Parameters.AddWithValue("@clinicPersonPer", 0)
                    insClinic.Parameters.AddWithValue("@sessionCount", Val(txt_session_count.Text))
                    insClinic.Parameters.AddWithValue("@group_no", group_no)
                    insClinic.Parameters.AddWithValue("@userId", Session("User_Id"))
                    insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
                    insurance_SQLcon.Open()
                    insClinic.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                    insClinic.CommandText = ""
                Next
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت عملية حفظ البيانات بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)
                fillListClinic()
                getClinicAvailable()
                clrTxt()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Sub clrTxt()
        txt_max_val.Text = ""
        txt_session_count.Text = ""
        dist_list.Items.Clear()

    End Sub

    Private Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Private Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        getClinicAvailable()
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        '################ When User Press On Stop Clinic ################
        If (e.CommandName = "stop_clinic") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Dim stopClinic As New SqlCommand
            stopClinic.Connection = insurance_SQLcon
            stopClinic.CommandText = "INC_stopCompanyClinic"
            stopClinic.CommandType = CommandType.StoredProcedure
            stopClinic.Parameters.AddWithValue("@clinic_no", (row.Cells(0).Text))
            stopClinic.Parameters.AddWithValue("@company_no", ViewState("company_no"))
            stopClinic.Parameters.AddWithValue("@contract_no", ViewState("contract_no"))
            insurance_SQLcon.Open()
            stopClinic.ExecuteNonQuery()
            insurance_SQLcon.Close()
            stopClinic.CommandText = ""

            add_action(1, 2, 2, "إيقاف العيادة رقم: " & (row.Cells(0).Text) & " عن الشركة رقم " & ViewState("company_no") & " عقد رقم: " & ViewState("contract_no"), Session("User_Id"), GetIPAddress())

            fillListClinic()
            getClinicAvailable()


            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'تمت العملية بنجاح',
                showConfirmButton: false,
                timer: 1500
            });", True)

        End If

        If (e.CommandName = "clinic_name") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Response.Redirect("editClinic.aspx?clinicId=" & (row.Cells(0).Text), True)
        End If
    End Sub
End Class