Imports System.Data.SqlClient
Imports CrystalDecisions.Shared

Public Class printPrices
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim dt_result As New DataTable
    Dim main_ds As New DataSet1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub ddl_clinics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_clinics.SelectedIndexChanged
        Try
            Dim sel_com As New SqlCommand("SELECT 0 AS [Service_ID], N'يرجى اختيار قسم' AS [Service_AR_Name] FROM [Main_Services] UNION SELECT [Service_ID], [Service_AR_Name] FROM [Main_Services] where Service_State = 0 and Service_Clinic = " & ddl_clinics.SelectedValue, insurance_SQLcon)
            Dim dt_result As New DataTable
            insurance_SQLcon.Close()
            insurance_SQLcon.Open()
            dt_result.Load(sel_com.ExecuteReader)
            insurance_SQLcon.Close()

            ddl_services.DataSource = dt_result
            ddl_services.DataValueField = "Service_ID"
            ddl_services.DataTextField = "Service_AR_Name"
            ddl_services.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ddl_show_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_show_type.SelectedIndexChanged
        'If ddl_show_type.SelectedValue = 3 Then
        '    Panel1.Visible = True
        'Else
        '    Panel1.Visible = False
        'End If
    End Sub

    Private Function getData() As DataSet1

        Dim query_str As String

        If ddl_show_type.SelectedValue = 2 Then
            query_str = "SELECT SubService_ID, SubService_Code, SubService_AR_Name, SubService_EN_Name, SubService_Service_ID, (SELECT Service_EN_Name FROM Main_Services WHERE Main_Services.Service_ID=SubService_Service_ID) AS Service_EN_Name,SubService_Clinic, SUBGROUP_ID, SubGroup_ARname, isnull(SubGroup_ENname, '') as SubGroup_ENname, MainGroup_ID, Group_ARname, INS_PRS, DOCTOR_ID, PROFILE_PRICE_ID FROM MAIN_SUBGROUP 
          inner join MAIN_SUBSERVICES on MAIN_SUBSERVICES.SubService_Group = MAIN_SUBGROUP.SubGroup_ID
          inner join INC_SERVICES_PRICES on INC_SERVICES_PRICES.SER_ID = MAIN_SUBSERVICES.SubService_ID
          inner join Main_GroupSubService on Main_GroupSubService.Group_ID = Main_SubGroup.MainGroup_ID
          where Group_flag <> 1 AND PROFILE_PRICE_ID = " & ddl_prices_profile.SelectedValue & " AND DOCTOR_ID = " & dll_doctors.SelectedValue
        Else
            query_str = " SELECT SubService_ID, SubService_Code, SubService_AR_Name, SubService_EN_Name, SubService_Service_ID, (SELECT Service_EN_Name FROM Main_Services WHERE Main_Services.Service_ID=SubService_Service_ID) AS Service_EN_Name,SubService_Clinic, SUBGROUP_ID, SubGroup_ARname, isnull(SubGroup_ENname, '') as SubGroup_ENname, MainGroup_ID, Group_ARname, INS_PRS, DOCTOR_ID, PROFILE_PRICE_ID FROM MAIN_SUBGROUP 
          inner join MAIN_SUBSERVICES on MAIN_SUBSERVICES.SubService_ID = (SELECT TOP (1) SUBSERVICE_ID FROM MAIN_SUBSERVICES WHERE MAIN_SUBSERVICES.SUBSERVICE_GROUP = MAIN_SUBGROUP.SUBGROUP_ID)
          inner join INC_SERVICES_PRICES on INC_SERVICES_PRICES.SER_ID = (SELECT TOP (1) SUBSERVICE_ID FROM MAIN_SUBSERVICES WHERE MAIN_SUBSERVICES.SUBSERVICE_GROUP = MAIN_SUBGROUP.SUBGROUP_ID)
          inner join Main_GroupSubService on Main_GroupSubService.Group_ID = Main_SubGroup.MainGroup_ID
          where Group_flag <> 1 AND PROFILE_PRICE_ID = " & ddl_prices_profile.SelectedValue & " AND DOCTOR_ID = " & dll_doctors.SelectedValue

        End If

        If ddl_show_type.SelectedValue <> 0 Then
            query_str = query_str & " and MainGroup_ID = " & ddl_show_type.SelectedValue
        End If

        If ddl_sub_gourp.SelectedValue <> 0 Then
            query_str = query_str & " and SubGroup_ID = " & ddl_sub_gourp.SelectedValue
        End If

        If Val(ddl_services.SelectedValue) <> 0 Then
            query_str = query_str & " and SubService_Service_ID = " & ddl_services.SelectedValue
        End If


        Using main_ds
            Dim cmd As New SqlCommand(query_str)
            Using sda As New SqlDataAdapter()
                cmd.Connection = insurance_SQLcon
                sda.SelectCommand = cmd
                sda.Fill(main_ds, "printKashef")
                sda.Fill(dt_result)

                If dt_result.Rows.Count > 0 Then
                    GridView1.DataSource = dt_result
                    GridView1.DataBind()
                    btn_print.Enabled = True
                Else
                    btn_print.Enabled = False
                End If
                Return main_ds
            End Using
        End Using

    End Function

    Private Sub btn_show_Click(sender As Object, e As EventArgs) Handles btn_show.Click

        getData()

    End Sub

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Try

            If ddl_show_type.SelectedValue <> 2 Then
                Dim CrReport As New servicesPricesKashf()
                Dim CrExportOptions As ExportOptions
                Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
                CrReport.SetDataSource(getData())

                Dim FolderLocation As String
                FolderLocation = Server.MapPath("~/Reports")
                Dim filepath As String = FolderLocation & "/servicesPricesKashf.pdf"
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
                Response.Redirect("~/Reports/servicesPricesKashf.pdf", False)

            Else
                Dim CrReport As New servicesPricesOprations()
                Dim CrExportOptions As ExportOptions
                Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
                CrReport.SetDataSource(getData())

                Dim FolderLocation As String
                FolderLocation = Server.MapPath("~/Reports")
                Dim filepath As String = FolderLocation & "/servicesPricesOprations.pdf"
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
                Response.Redirect("~/Reports/servicesPricesOprations.pdf", False)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alertify.error('" & ex.Message & "'); alertify.set('notifier','delay', 3); alertify.set('notifier','position', 'top-right');", True)
        End Try
    End Sub
End Class