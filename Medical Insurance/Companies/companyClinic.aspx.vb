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
                lbl_en_name.Text = dr_company!C_NAME_ENG
                lbl_com_name.Text = dr_company!C_NAME_ARB
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

    Sub getClinicAvailable()
        Dim sel_com As New SqlCommand("SELECT Clinic_ID, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.Clinic_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID) AS CLINIC_NAME FROM INC_CLINICAL_RESTRICTIONS WHERE C_ID = " & ViewState("company_no") & " AND CONTRACT_NO = " & ViewState("contract_no"), insurance_SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        If dt_result.Rows.Count > 0 Then
            Dim resultString As String = ""
            Dim isFirstResult = True
            
            For i = 0 To dt_result.Rows.Count - 1
                Dim dr = dt_result.Rows(i)

                ' Delete Available Clinics From Clinics ListBox
                Dim liItem As ListItem = source_list.Items.FindByValue(dr!Clinic_ID)
                If (liItem IsNot Nothing) Then
                    source_list.Items.Remove(liItem)
                End If

                If Not isFirstResult Then
                    resultString &= String.Format(" <span class='badge badge-pill badge-info p-2'>{0}</span>", dr!CLINIC_NAME)
                Else
                    isFirstResult = False
                    resultString &= String.Format("<span class='badge badge-pill badge-info p-2'>{0}</span>", dr!CLINIC_NAME)
                End If
            Next

            Literal1.Text = resultString
        Else
            Literal1.Text = "لا يوجد عيادة مغطاة"
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

        If dist_list.Items.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('خطأ! يجب اختيار عيادة واحدة على الأقل'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
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
                insClinic.Parameters.AddWithValue("@clinicPersonPer", Val(txt_person_per.Text))
                insClinic.Parameters.AddWithValue("@sessionCount", Val(txt_session_count.Text))
                insClinic.Parameters.AddWithValue("@group_no", 0)
                insClinic.Parameters.AddWithValue("@userId", 1)
                insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
                insurance_SQLcon.Open()
                insClinic.ExecuteNonQuery()
                insurance_SQLcon.Close()
                insClinic.CommandText = ""
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
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
                insGroup.Parameters.AddWithValue("@personPer", Val(txt_person_per.Text))
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
                    insClinic.Parameters.AddWithValue("@clinicPersonPer", Val(txt_person_per.Text))
                    insClinic.Parameters.AddWithValue("@sessionCount", Val(txt_session_count.Text))
                    insClinic.Parameters.AddWithValue("@group_no", group_no)
                    insClinic.Parameters.AddWithValue("@userId", 1)
                    insClinic.Parameters.AddWithValue("@userIp", GetIPAddress())
                    insurance_SQLcon.Open()
                    insClinic.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                    insClinic.CommandText = ""
                Next
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية حفظ البيانات بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
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
        txt_person_per.Text = ""
        dist_list.Items.Clear()

    End Sub
End Class