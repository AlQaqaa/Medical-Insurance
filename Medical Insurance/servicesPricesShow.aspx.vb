Imports System.Data.SqlClient
Imports CrystalDecisions.Shared

Public Class servicesPricesShow
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim dt_result As New DataTable
    Dim main_ds As New DataSet1


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Session("User_Id") Is Nothing Or Session("User_Id") = 0 And Session("systemlogin") <> "401" Then
                Response.Redirect("http://10.10.1.10", True)
            End If

            ViewState("profile_no") = Val(Request.QueryString("pID"))

            If ViewState("profile_no") = 0 Then
                Response.Redirect("createProfilePrices.aspx", False)
            End If

            Dim sel_com As New SqlCommand("SELECT profile_name FROM INC_PRICES_PROFILES WHERE profile_Id = " & ViewState("profile_no"), insurance_SQLcon)
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            Page.Title = sel_com.ExecuteScalar
            insurance_SQLcon.Close()

        End If
    End Sub

    Private Sub ddl_show_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_show_type.SelectedIndexChanged
        GridView1.DataSource = Nothing
        GridView1.DataBind()

        'If ddl_show_type.SelectedValue = 1 Then
        '    clinic_Panel.Visible = True
        '    groups_Panel.Visible = False
        'Else
        '    clinic_Panel.Visible = False
        '    groups_Panel.Visible = True
        'End If
    End Sub

    Private Sub btn_show_Click(sender As Object, e As EventArgs) Handles btn_show.Click
        Try

            Dim ser_price As String

            If ddl_price.SelectedValue = 1 Then
                ser_price = "INS_PRS"
            Else
                ser_price = "CASH_PRS"
            End If

            Dim sql_str As String = ""

            If ddl_show_type.SelectedValue = 1 Then
                sql_str = "SELECT SER_ID, ISNULL(" & ser_price & ", 0) AS SERVICE_PRICE, SubService_Code, SubService_AR_Name, SubService_EN_Name, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_servicesPrices.SubService_Clinic) AS CLINIC_NAME FROM INC_servicesPrices WHERE PROFILE_PRICE_ID = " & ViewState("profile_no")
            Else
                sql_str = "SELECT distinct SUBGROUP_ID as SER_ID, (select distinct [Group_ARname] from [dbo].[Main_GroupSubService] where Main_GroupSubService.Group_ID = MAIN_SUBGROUP.MainGroup_ID) as CLINIC_NAME, SUBGROUP_ARNAME as SubService_AR_Name, isnull(SubGroup_ENname, '') as SubService_EN_Name, '-' as SubService_Code, ISNULL((SELECT " & ser_price & " FROM [DBO].[INC_SERVICES_PRICES] WHERE INC_SERVICES_PRICES.PROFILE_PRICE_ID = " & ViewState("profile_no") & " and INC_SERVICES_PRICES.[SER_ID] = (SELECT TOP (1) [SUBSERVICE_ID] FROM [DBO].[MAIN_SUBSERVICES] WHERE MAIN_SUBSERVICES.SUBSERVICE_GROUP = MAIN_SUBGROUP.SUBGROUP_ID)), 0) AS SERVICE_PRICE FROM [DBO].[MAIN_SUBGROUP]"
            End If

            Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
            Dim dt_res As New DataTable
            dt_res.Rows.Clear()
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_res.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            If dt_res.Rows.Count > 0 Then
                btn_print.Enabled = True
                GridView1.DataSource = dt_res
                GridView1.DataBind()
            Else
                btn_print.Enabled = False
                dt_res.Rows.Clear()
                GridView1.DataSource = dt_res
                GridView1.DataBind()
            End If
        Catch ex As Exception
            Label1.Text = ex.Message
            
        End Try
    End Sub

    Private Function getDataReport() As DataSet1
        Dim ser_price As String

        If ddl_price.SelectedValue = 1 Then
            ser_price = "INS_PRS"
        Else
            ser_price = "CASH_PRS"
        End If

        Dim sql_str As String = ""

        If ddl_show_type.SelectedValue = 1 Then
            sql_str = "SELECT SER_ID, ISNULL(" & ser_price & ", 0) AS SERVICE_PRICE, SubService_Code, SubService_AR_Name, SubService_EN_Name, (SELECT Clinic_AR_Name FROM Main_Clinic WHERE Main_Clinic.clinic_id = INC_servicesPrices.SubService_Clinic) AS CLINIC_NAME FROM INC_servicesPrices WHERE PROFILE_PRICE_ID = " & ViewState("profile_no")
        Else
            sql_str = "SELECT distinct SUBGROUP_ID as SER_ID, (select distinct [Group_ARname] from [dbo].[Main_GroupSubService] where Main_GroupSubService.Group_ID = MAIN_SUBGROUP.MainGroup_ID) as CLINIC_NAME, SUBGROUP_ARNAME as SubService_AR_Name, isnull(SubGroup_ENname, '') as SubService_EN_Name, '-' as SubService_Code, ISNULL((SELECT " & ser_price & " FROM [DBO].[INC_SERVICES_PRICES] WHERE INC_SERVICES_PRICES.PROFILE_PRICE_ID = " & ViewState("profile_no") & " and INC_SERVICES_PRICES.[SER_ID] = (SELECT TOP (1) [SUBSERVICE_ID] FROM [DBO].[MAIN_SUBSERVICES] WHERE MAIN_SUBSERVICES.SUBSERVICE_GROUP = MAIN_SUBGROUP.SUBGROUP_ID)), 0) AS SERVICE_PRICE FROM [DBO].[MAIN_SUBGROUP]"
        End If

        Using main_ds
            Dim cmd As New SqlCommand(sql_str)
            Using sda As New SqlDataAdapter()
                cmd.Connection = insurance_SQLcon
                sda.SelectCommand = cmd
                sda.Fill(main_ds, "INC_servicesPrices")
                sda.Fill(dt_result)
                Return main_ds
            End Using
        End Using
    End Function

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click

        Try

            Dim CrReport As New servicesPrices()
            Dim CrExportOptions As ExportOptions
            Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
            CrReport.SetDataSource(getDataReport())

            Dim FolderLocation As String
            FolderLocation = Server.MapPath("~/Reports")
            Dim filepath As String = FolderLocation & "/servicesPrices.pdf"
            CrDiskFileDestinationOptions.DiskFileName = filepath

            CrExportOptions = CrReport.ExportOptions

            With CrExportOptions

                'Set the destination to a disk file
                .ExportDestinationType = ExportDestinationType.DiskFile

                'Set the format to PDF
                .ExportFormatType = ExportFormatType.PortableDocFormat

                'Set the destination options to DiskFileDestinationOptions object
                .DestinationOptions = CrDiskFileDestinationOptions
                .FormatOptions = CrFormatTypeOptions
            End With

            CrReport.Export()
            Response.Redirect("~/Reports/servicesPrices.pdf", False)

        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub
End Class