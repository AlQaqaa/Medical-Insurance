﻿Imports System.Data.SqlClient

Public Class createProfilePrices
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            getActiveProfile()
            getStopProfile()

        End If

        Session("profile_no") = 0
    End Sub

    Private Sub btn_next_Click(sender As Object, e As EventArgs) Handles btn_next.Click

        Dim is_default As Boolean

        If cb_is_default.Checked = True Then
            is_default = True
        Else
            is_default = False
        End If
        Try
            Dim insNewProfile As New SqlCommand
            insNewProfile.Connection = insurance_SQLcon
            insNewProfile.CommandText = "INC_addNewProfilePrices"
            insNewProfile.CommandType = CommandType.StoredProcedure
            insNewProfile.Parameters.AddWithValue("@profile_name", txt_profile_name.Text)
            insNewProfile.Parameters.AddWithValue("@is_default", is_default)
            insNewProfile.Parameters.AddWithValue("@user_id", 1)
            insNewProfile.Parameters.AddWithValue("@user_ip", GetIPAddress())
            insNewProfile.Parameters.AddWithValue("@p_id", SqlDbType.Int).Direction = ParameterDirection.Output
            insurance_SQLcon.Open()
            insNewProfile.ExecuteNonQuery()
            Session.Item("profile_no") = insNewProfile.Parameters("@p_id").Value.ToString()
            insurance_SQLcon.Close()

            txt_profile_name.Text = ""

            Response.Redirect("SERVICES_PRICES.aspx", False)
            Exit Sub
        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub

    Private Sub getActiveProfile()
        Try
            Dim sel_com As New SqlCommand("SELECT PROFILE_ID, PROFILE_NAME, CONVERT(VARCHAR, PROFILE_DT, 23) AS PROFILE_DT, is_default FROM INC_PRICES_PROFILES WHERE PROFILE_STS = 0", insurance_SQLcon)
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
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub

    Private Sub getStopProfile()
        Try
            Dim sel_com As New SqlCommand("SELECT PROFILE_ID, PROFILE_NAME, CONVERT(VARCHAR, PROFILE_DT, 23) AS PROFILE_DT FROM INC_PRICES_PROFILES WHERE PROFILE_STS = 1", insurance_SQLcon)
            Dim dt_result As New DataTable
            dt_result.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_result.Rows.Count > 0 Then
                GridView2.DataSource = dt_result
                GridView2.DataBind()
            Else
                dt_result.Rows.Clear()
                GridView2.DataSource = dt_result
                GridView2.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand

        Try
            If (e.CommandName = "stop_profile") Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridView1.Rows(index)

                Dim result_a As Integer
                Dim sel_com As New SqlCommand("SELECT COUNT(C_ID) AS C_ID FROM INC_COMPANY_DETIAL WHERE PROFILE_PRICE_ID = " & (row.Cells(0).Text), insurance_SQLcon)
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                result_a = sel_com.ExecuteScalar()
                insurance_SQLcon.Close()
                If result_a <> 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('لا يمكن تعطيل ملف الأسعار هذا'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                    Exit Sub
                Else
                    If (row.Cells(1).Text) = True Then
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('خطأ! لا يمكن تعطيل ملف أسعار أساسي'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                        Exit Sub
                    End If
                    Dim edit_com As New SqlCommand("UPDATE INC_PRICES_PROFILES SET profile_sts = 1 WHERE is_default = 0 AND profile_Id = " & (row.Cells(0).Text), insurance_SQLcon)
                    insurance_SQLcon.Close()
                    insurance_SQLcon.Open()
                    edit_com.ExecuteNonQuery()
                    insurance_SQLcon.Close()
                    getStopProfile()
                    getActiveProfile()
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تمت عملية تعطيل ملف الأسعار بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('لا يمكن تعطيل ملف الأسعار هذا'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            Exit Sub
        End Try

        If (e.CommandName = "edit_price") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Session.Item("profile_no") = (row.Cells(0).Text)
            Response.Redirect("SERVICES_PRICES.aspx", False)
            Exit Sub
        End If

        If (e.CommandName = "set_default") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)

            Try
                Dim setDefaultProfilePrice As New SqlCommand
                setDefaultProfilePrice.Connection = insurance_SQLcon
                setDefaultProfilePrice.CommandText = "INC_setDefaultProfilePrice"
                setDefaultProfilePrice.CommandType = CommandType.StoredProcedure
                setDefaultProfilePrice.Parameters.AddWithValue("@profile_no", (row.Cells(0).Text))
                setDefaultProfilePrice.Parameters.AddWithValue("@user_id", 1)
                setDefaultProfilePrice.Parameters.AddWithValue("@user_ip", GetIPAddress())
                insurance_SQLcon.Open()
                setDefaultProfilePrice.ExecuteNonQuery()
                insurance_SQLcon.Close()
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.success('تم تحديد ملف الأسعار كأساسي بنجاح'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
                getActiveProfile()
                getStopProfile()

            Catch ex As Exception
                MsgBox(ex.Message)
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
            End Try

        End If

        If (e.CommandName = "show_profile") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Response.Redirect("servicesPricesShow.aspx?pID=" & (row.Cells(0).Text))
        End If
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim cell As TableCell = e.Row.Cells(1)
            Dim mov_state As String = cell.Text

            If mov_state = False Then
                e.Row.Cells(4).Visible = True
            Else
                e.Row.Cells(4).Visible = False
                e.Row.ForeColor = Drawing.Color.Green
            End If
        End If
    End Sub

    Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand

        Try
            If (e.CommandName = "stop_block") Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridView2.Rows(index)

                Dim edit_com As New SqlCommand("UPDATE INC_PRICES_PROFILES SET PROFILE_STS = 0 WHERE PROFILE_ID = " & (row.Cells(0).Text), insurance_SQLcon)
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                edit_com.ExecuteNonQuery()
                insurance_SQLcon.Close()
                getStopProfile()
                getActiveProfile()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class