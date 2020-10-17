Imports System.Data.SqlClient
Imports System.Globalization

Public Class dorctorsForms
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.ClientScript.IsStartupScriptRegistered(Me.GetType, "bb1") = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "bb1", String.Format("<script type='text/javascript' src='{0}'></script>", Page.ResolveUrl("~/Js/MaskedEditFix.js")))
        End If


        If Session("INC_User_type") <> 0 And Session("INC_User_type") <> 1 Then
            If Session("User_per")("doctors_settled") = False Then
                Response.Redirect("Default.aspx", True)
                Exit Sub
            End If
        End If


        If IsPostBack Then

        Else


            'TxtDate_6.Text = CDate(Now.Date)
            'TxtDate_7.Text = CDate(Now.Date)

            DDMain_MedicalStaff.DataSource = getData(DDMain_MedicalStaff.SelectedIndex + 1)
            DDMain_MedicalStaff.DataValueField = "MedicalStaff_ID"
            DDMain_MedicalStaff.DataTextField = "MedicalStaff_AR_Name"
            DDMain_MedicalStaff.DataBind()
            clr()

            If IsNumeric(Request.QueryString("flg")) Then
                DDMain_MedicalStaff.SelectedValue = Request.QueryString("flg")
                search()
            End If

        End If
    End Sub

    Private Shared Function GetData(vindex As Integer) As DataTable
        Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

        Dim dt As New DataTable()
        If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
        Dim ssql As String = "SELECT '' as MedicalStaff_ID, '' as MedicalStaff_AR_Name Union "

        ssql += "   select   MedicalStaff_ID, MedicalStaff_AR_Name"
        ssql += " from  HAG_Processes_Doctor  inner join Main_MedicalStaff"
        ssql += " on Main_MedicalStaff.MedicalStaff_ID=Processes_Doctor_ID"

        Dim CD = New SqlCommand(ssql)
        Dim sda As New SqlDataAdapter()
        CD.CommandType = CommandType.Text
        CD.Connection = insurance_SQLcon
        sda.SelectCommand = CD

        'dt.Items.Insert(0, New ListItem("Please select"))
        sda.Fill(dt)


        Return dt
    End Function

    Protected Sub search()


        Dim vx_all As Integer = 0
        Dim SQL As String


        If DDMain_MedicalStaff.SelectedValue = 0 Then
            Button1.Visible = False
            GridView1.DataSource = ""
            GridView1.DataBind()

            Exit Sub
        End If

        '----------------------------------
        Dim CD1 As New SqlCommand

        CD1.Dispose()
        CD1.Connection = insurance_SQLcon

        CD1.CommandType = CommandType.StoredProcedure
        If DDMain_MedicalStaff.SelectedValue > 0 Then
            CD1.CommandText = "[acc_dr_money]"
            CD1.Parameters.AddWithValue("@ui", DDMain_MedicalStaff.SelectedValue)
            CD1.Parameters.AddWithValue("@user", Session("INC_User_Id"))
        Else

            GridView1.DataSource = ""
            GridView1.DataBind()
            Exit Sub
            'CD1.CommandText = "[acc_dr_money_all]"
            'vx_all = 1
        End If



        'CD.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
        'CD.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))

        If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()

        Dim dt_result As New DataTable
        dt_result.Rows.Clear()

        dt_result.Load(CD1.ExecuteReader)

        Dim sum1 As Double

        Dim sum2 As Double
        Dim sum22 As Double
        Dim sum1x As Double
        Dim vx As Integer = 0
        If dt_result IsNot Nothing AndAlso dt_result.Rows.Count > 0 Then
            For i As Integer = 0 To dt_result.Rows.Count - 1
                sum1 = dt_result.Rows(i)("sum1").ToString()
                sum2 = dt_result.Rows(i)("sum2").ToString()
                sum22 = dt_result.Rows(i)("sum22").ToString()
                sum1x = dt_result.Rows(i)("sum1x").ToString()
                vx_all = vx_all + 1

            Next
        End If



        'If sum1 > 0 Or sum22 > 0 Or vx_all = 2 Then
        '    GridView1.DataSource = dt_result
        '    GridView1.DataBind()
        'Else
        '    GridView1.DataSource = ""
        '    GridView1.DataBind()
        'End If

        GridView1.DataSource = ""
        GridView1.DataBind()


        insurance_SQLcon.Close()
        '----------------------------
        'If DDMain_MedicalStaff.SelectedValue <> 0 Then
        '    CD = New SqlCommand(" select * from acl_Ekfal where ek_from between @Date1 and @Date2 and ek_to between @Date1 and @Date2 and ek_user=@ek_user ", Cn)
        '    CD.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
        '    CD.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
        '    CD.Parameters.AddWithValue("@ek_user", DDMain_MedicalStaff.SelectedValue)
        '    If Cn.State=ConnectionState.Closed then Cn.open
        '    Dim rdr As SqlDataReader = CD.ExecuteReader
        '    If rdr.Read Then
        '        Button1.Visible = False
        '        ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('تم الاقفال للمستخدم في جزء من الفترة المحددة')", True)
        '    End If
        '    RD.Close()
        '    Cn.Close()
        '  End If

    End Sub
    Protected Sub search3()


        Dim vx_all As Integer = 0
        Dim SQL As String



        If DDMain_MedicalStaff.SelectedValue = 0 Then
            Button1.Visible = False
            GridView1.DataSource = ""
            GridView1.DataBind()

            Exit Sub
        End If

        '----------------------------------
        Dim CD1 As New SqlCommand

        CD1.Dispose()
        CD1.Connection = insurance_SQLcon

        CD1.CommandType = CommandType.StoredProcedure
        If DDMain_MedicalStaff.SelectedValue > 0 Then
            CD1.CommandText = "[acc_dr_money_esal]"
            CD1.Parameters.AddWithValue("@ui", DDMain_MedicalStaff.SelectedValue)
            CD1.Parameters.AddWithValue("@user", Session("INC_User_Id"))
            CD1.Parameters.AddWithValue("@esal", TextBox1.Text)

        Else

            GridView1.DataSource = ""
            GridView1.DataBind()
            Exit Sub
            'CD1.CommandText = "[acc_dr_money_all]"
            'vx_all = 1
        End If



        'CD.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
        'CD.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))

        If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()

        Dim dt_result As New DataTable
        dt_result.Rows.Clear()

        dt_result.Load(CD1.ExecuteReader)

        Dim sum1 As Double

        Dim sum2 As Double
        Dim sum22 As Double
        Dim sum1x As Double
        Dim vx As Integer = 0
        If dt_result IsNot Nothing AndAlso dt_result.Rows.Count > 0 Then




            GridView1.DataSource = dt_result
            GridView1.DataBind()
        Else
            GridView1.DataSource = ""
            GridView1.DataBind()
        End If

        'GridView1.DataSource = ""
        'GridView1.DataBind()


        insurance_SQLcon.Close()
        '----------------------------
        'If DDMain_MedicalStaff.SelectedValue <> 0 Then
        '    CD = New SqlCommand(" select * from acl_Ekfal where ek_from between @Date1 and @Date2 and ek_to between @Date1 and @Date2 and ek_user=@ek_user ", Cn)
        '    CD.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
        '    CD.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
        '    CD.Parameters.AddWithValue("@ek_user", DDMain_MedicalStaff.SelectedValue)
        '    If Cn.State=ConnectionState.Closed then Cn.open
        '    Dim rdr As SqlDataReader = CD.ExecuteReader
        '    If rdr.Read Then
        '        Button1.Visible = False
        '        ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('تم الاقفال للمستخدم في جزء من الفترة المحددة')", True)
        '    End If
        '    RD.Close()
        '    Cn.Close()
        '  End If

    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'CD = New SqlCommand("INSERT INTO[dbo].[acl_Ekfal] ([ek_from],[ek_to],[ek_user],[NakdiMosajal],[NakdiMostalam],[Tarjee],[Ohda],[Ajz],[EjmaliNakdi],[SakMosajal],[SakMostalam],[KobonMosajal],[KobonMostalam],[EServMosajal],[EServMostalam],[EjmaliSak],[ekfaluser]) VALUES (@ek_from,@ek_to,@ek_user,@NakdiMosajal,@NakdiMostalam,@Tarjee,@Ohda,@Ajz,@EjmaliNakdi,@SakMosajal,@SakMostalam,@KobonMosajal,@KobonMostalam,@EServMosajal,@EServMostalam,@EjmaliSak,@ekfaluser)", Cn)
        'CD.Parameters.AddWithValue("@ek_from", CDate(TxtDate_1.Text))
        'CD.Parameters.AddWithValue("@ek_to", CDate(TxtDate_2.Text))
        'CD.Parameters.AddWithValue("@ek_user", DDMain_MedicalStaff.SelectedValue)
        'CD.Parameters.AddWithValue("@NakdiMosajal", TxtNakdiMosajal.Text)
        'CD.Parameters.AddWithValue("@NakdiMostalam", TxtNakdiMostalam.Text)
        'CD.Parameters.AddWithValue("@Tarjee", TxtTarjee.Text)
        'CD.Parameters.AddWithValue("@Ohda", TxtOhda.Text)
        'CD.Parameters.AddWithValue("@Ajz", HiddenField1.Value)
        'CD.Parameters.AddWithValue("@EjmaliNakdi", TxtEjmaliNakdi.Text)
        'CD.Parameters.AddWithValue("@SakMosajal", TxtSakMosajal.Text)
        'CD.Parameters.AddWithValue("@SakMostalam", TxtSakMostalam.Text)
        'CD.Parameters.AddWithValue("@KobonMosajal", TxtKobonMosajal.Text)
        'CD.Parameters.AddWithValue("@KobonMostalam", TxtKobonMostalam.Text)
        'CD.Parameters.AddWithValue("@EServMosajal", TxtEServMosajal.Text)
        'CD.Parameters.AddWithValue("@EServMostalam", TxtEServMostalam.Text)
        'CD.Parameters.AddWithValue("@EjmaliSak", HiddenField6.Value)
        'CD.Parameters.AddWithValue("@ekfaluser", Val(Session("user_id")))
        'If Cn.State=ConnectionState.Closed then Cn.open
        'CD.ExecuteNonQuery()



        'CD = New SqlCommand("update View_Sdad_Esal set Receipt_State=1 where user_id=@user_id and Receipt_Date between @date1 and @date2", Cn)
        'CD.Parameters.AddWithValue("@user_id", DDMain_MedicalStaff.SelectedValue)
        'CD.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
        'CD.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
        'CD.ExecuteNonQuery()

        'CD = New SqlCommand("update View_Dofa_Hesab set payment_state=1 where payment_user=@user_id and payment_Date between @date1 and @date2", Cn)
        'CD.Parameters.AddWithValue("@user_id", DDMain_MedicalStaff.SelectedValue)
        'CD.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
        'CD.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
        'CD.ExecuteNonQuery()

        'CD = New SqlCommand("update View_Alrja_Esal set Return_State=1 where user_id=@user_id and return_date between @date1 and @date2", Cn)
        'CD.Parameters.AddWithValue("@user_id", DDMain_MedicalStaff.SelectedValue)
        'CD.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
        'CD.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
        'CD.ExecuteNonQuery()
        'Cn.Close()

        'ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('تم الحفظ')", True)
        'clr()
    End Sub

    Sub clr()
        TextBox1.Text = ""


        DDMain_MedicalStaff.SelectedValue = 0
    End Sub

    'Protected Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
    '    Dim warnings As Warning()
    '    Dim streamIds As String()
    '    Dim contentType As String
    '    Dim encoding As String
    '    Dim extension As String



    '    Dim mimeType As String
    '    Dim streams As String()



    '    Dim viewer As ReportViewer = New ReportViewer()

    '    '*****************************************

    '    viewer.ProcessingMode = ProcessingMode.Local
    '    viewer.LocalReport.ReportPath = Server.MapPath("Reports/EradKhazaynFaryaReport.rdlc")
    '    Dim dsCustomers As DS_EradKhazaynFarya = GetData2()
    '    Dim datasource As New ReportDataSource("DataSet1", dsCustomers.Tables(0))

    '    viewer.LocalReport.DataSources.Clear()



    '    Dim rp1 As ReportParameter = New ReportParameter("ReportParameter1", "تقرير القيم النقدية")


    '    viewer.LocalReport.SetParameters(New ReportParameter() {rp1})

    '    viewer.LocalReport.DataSources.Add(datasource)


    '    Dim FileName As String = "KashfHsab" & "e1" & ".pdf"
    '    File.Delete(Server.MapPath("~/files/" & FileName))

    '    'Export the RDLC Report to Byte Array.


    '    Dim mybytes As Byte() = viewer.LocalReport.Render("PDF", Nothing, extension, encoding, mimeType, streams, warnings)

    '    Using fs As FileStream = File.Create(Server.MapPath("~/files/" & FileName))
    '        fs.Write(mybytes, 0, mybytes.Length)
    '    End Using

    '    Response.ClearHeaders()
    '    Response.ClearContent()
    '    Response.Buffer = True
    '    Response.Clear()
    '    Response.Charset = ""
    '    '       
    '    'Response.Close()

    '    'Try
    '    Dim client As WebClient = New WebClient()

    '    Dim buffer As Byte() = client.DownloadData(Server.MapPath("~/files/" & FileName))

    '    If buffer IsNot Nothing Then

    '        Response.Clear()
    '        Response.ContentType = "application/pdf"
    '        Response.AddHeader("content-length", buffer.Length.ToString())
    '        Response.BinaryWrite(buffer)
    '        Response.Flush()
    '        Response.End()
    '    End If
    '    'Catch ex As Exception
    '    '    MsgBox(ex.ToString)
    '    'End Try

    'End Sub

    'Function GetData2() As DS_EradKhazaynFarya
    '    Dim dt As New DS_EradKhazaynFarya()
    '    Dim MovDate1, MovDate2 As String

    '    Dim sql As String

    '    If DDMain_MedicalStaff.SelectedValue = 0 Then
    '        sql = "SELECT  [Receipt_ID] as ID ,[Receipt_Date] as DDate ,[Orginal_UserName] as Username ,[Receipt_PaymantMethod_Price] as price,(select case when Receipt_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'سداد إيصال' as ttype FROM [IbnSina_DataBase].[dbo].[View_Sdad_Esal]where [PaymentMethods_ID]=1  union SELECT   [payment_No] as ID,[payment_Date] as DDate,[Orginal_UserName] as Username ,[payment_PaymantMethod_Price] as price,(select case when payment_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'دفعة تحت الحساب' as ttype FROM [IbnSina_DataBase].[dbo].[View_Dofa_Hesab] where [PaymentMethods_ID]=1 order by ttype,ID"
    '    Else
    '        sql = "SELECT  [Receipt_ID] as ID ,[Receipt_Date] as DDate ,[Orginal_UserName] as Username ,[Receipt_PaymantMethod_Price] as price ,(select case when Receipt_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'سداد إيصال' as ttype FROM [IbnSina_DataBase].[dbo].[View_Sdad_Esal]where [PaymentMethods_ID]=1 and Receipt_Date between @Date1 and @Date2 and user_id=@user_id "
    '        sql += " union SELECT   [payment_No] as ID,[payment_Date] as DDate,[Orginal_UserName] as Username ,[payment_PaymantMethod_Price] as price,(select case when payment_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'دفعة تحت الحساب' as ttype FROM [IbnSina_DataBase].[dbo].[View_Dofa_Hesab] where [PaymentMethods_ID]=1 and payment_Date between @Date1 and @Date2 and payment_User=@user_id order by ttype,ID"
    '    End If

    '    Dim cmd As New SqlCommand(sql, Cn)
    '    cmd.Parameters.AddWithValue("@user_id", DDMain_MedicalStaff.SelectedValue)
    '    'cmd.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
    '    'cmd.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
    '    'cmd.Parameters.AddWithValue("@Mov_Ac_no", Request.QueryString("AC_no"))
    '    'cmd.Parameters.AddWithValue("@Mov_AC_Feraa", Request.QueryString("AC_Feraa"))
    '    'cmd.Parameters.AddWithValue("@Mov_AC_Type", Request.QueryString("AC_Type"))
    '    If Cn.State = ConnectionState.Closed Then Cn.Open()
    '    'RD = cmd.ExecuteReader
    '    'If RD.Read Then
    '    '    Session("RaseedAwalModa") = CDbl(RD!RaseedAwalModa.ToString)
    '    '    Session("RaseedAkhrModa") = CDbl(RD!RaseedAkhrModa.ToString)
    '    'End If
    '    'RD.Close()
    '    'Cn.Close()
    '    If Cn.State = ConnectionState.Closed Then Cn.Open()


    '    '  cmd.Parameters.AddWithValue("@DDTE", CDate(TxtDate2.Text))
    '    'Using con As New SqlConnection(conString)
    '    Using sda As New SqlDataAdapter()
    '        cmd.Connection = Cn
    '        cmd.CommandType = CommandType.Text
    '        sda.SelectCommand = cmd
    '        Using dsCustomers As New DS_EradKhazaynFarya()
    '            sda.Fill(dsCustomers, "Nakdi")
    '            Return dsCustomers
    '        End Using
    '    End Using


    'End Function


    'Function GetData3() As DS_EradKhazaynFarya
    '    Dim dt As New DS_EradKhazaynFarya()
    '    Dim sql As String
    '    If DDMain_MedicalStaff.SelectedValue = 0 Then
    '        sql = "SELECT  [Receipt_ID] as ID ,[Receipt_Date] as DDate ,[Orginal_UserName] as Username ,[Receipt_PaymantMethod_Price] as price,(select case when Receipt_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'سداد إيصال' as ttype FROM [IbnSina_DataBase].[dbo].[View_Sdad_Esal] where [PaymentMethods_ID]=2  union SELECT   [payment_No] as ID,[payment_Date] as DDate,[Orginal_UserName] as Username ,[payment_PaymantMethod_Price] as price ,(select case when payment_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'دفعة تحت الحساب' as ttype FROM [IbnSina_DataBase].[dbo].[View_Dofa_Hesab] where [PaymentMethods_ID]=2  order by ttype,ID"
    '    Else
    '        sql = "SELECT  [Receipt_ID] as ID ,[Receipt_Date] as DDate ,[Orginal_UserName] as Username ,[Receipt_PaymantMethod_Price] as price,(select case when Receipt_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'سداد إيصال' as ttype FROM [IbnSina_DataBase].[dbo].[View_Sdad_Esal] where [PaymentMethods_ID]=2 and Receipt_Date between @Date1 and @Date2 and user_id=@user_id"
    '        sql += " union SELECT   [payment_No] as ID,[payment_Date] as DDate,[Orginal_UserName] as Username ,[payment_PaymantMethod_Price] as price ,(select case when payment_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus, N'دفعة تحت الحساب' as ttype FROM [IbnSina_DataBase].[dbo].[View_Dofa_Hesab] where [PaymentMethods_ID]=2 and  payment_Date between @Date1 and @Date2 and payment_user=@user_id order by ttype,ID"
    '    End If

    '    Dim cmd As New SqlCommand(sql, Cn)
    '    cmd.Parameters.AddWithValue("@user_id", DDMain_MedicalStaff.SelectedValue)
    '    'cmd.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
    '    'cmd.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
    '    'cmd.Parameters.AddWithValue("@Mov_Ac_no", Request.QueryString("AC_no"))
    '    'cmd.Parameters.AddWithValue("@Mov_AC_Feraa", Request.QueryString("AC_Feraa"))
    '    'cmd.Parameters.AddWithValue("@Mov_AC_Type", Request.QueryString("AC_Type"))
    '    If Cn.State = ConnectionState.Closed Then Cn.Open()
    '    'RD = cmd.ExecuteReader
    '    'If RD.Read Then
    '    '    Session("RaseedAwalModa") = CDbl(RD!RaseedAwalModa.ToString)
    '    '    Session("RaseedAkhrModa") = CDbl(RD!RaseedAkhrModa.ToString)
    '    'End If
    '    'RD.Close()
    '    'Cn.Close()
    '    If Cn.State = ConnectionState.Closed Then Cn.Open()
    '    '  cmd.Parameters.AddWithValue("@DDTE", CDate(TxtDate2.Text))
    '    'Using con As New SqlConnection(conString)
    '    Using sda As New SqlDataAdapter()
    '        cmd.Connection = Cn
    '        cmd.CommandType = CommandType.Text
    '        sda.SelectCommand = cmd
    '        Using dsCustomers As New DS_EradKhazaynFarya()
    '            sda.Fill(dsCustomers, "Sak")
    '            Return dsCustomers
    '        End Using
    '    End Using
    'End Function


    'Function GetData4() As DS_EradKhazaynFarya
    '    Dim dt As New DS_EradKhazaynFarya()
    '    Dim sql As String
    '    If DDMain_MedicalStaff.SelectedValue = 0 Then
    '        sql = "select Return_ID,Return_Date,Orginal_UserName,(select case when Return_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus,Processes_Paid,Patient_AR_Name from View_Alrja_Esal"
    '    Else
    '        sql = "select Return_ID,Return_Date,Orginal_UserName,(select case when Return_State=1 then N'تم الإقفال' else N'لم يتم الاقفال' end) as EkfatStatus,Processes_Paid,Patient_AR_Name from View_Alrja_Esal"
    '    End If
    '    Dim cmd As New SqlCommand(sql, Cn)
    '    'cmd.Parameters.AddWithValue("@user_id", DDMain_MedicalStaff.SelectedValue)
    '    'cmd.Parameters.AddWithValue("@Date1", CDate(TxtDate_1.Text))
    '    'cmd.Parameters.AddWithValue("@Date2", CDate(TxtDate_2.Text))
    '    If Cn.State = ConnectionState.Closed Then Cn.Open()
    '    If Cn.State = ConnectionState.Closed Then Cn.Open()

    '    Using sda As New SqlDataAdapter()
    '        cmd.Connection = Cn
    '        cmd.CommandType = CommandType.Text
    '        sda.SelectCommand = cmd
    '        Using dsCustomers As New DS_EradKhazaynFarya()
    '            sda.Fill(dsCustomers, "Erjaa")
    '            Return dsCustomers
    '        End Using
    '    End Using
    'End Function

    Protected Sub DDMain_MedicalStaff_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDMain_MedicalStaff.SelectedIndexChanged

    End Sub



    Private Function GetDatax() As DataTable
        Dim command As New SqlCommand
        Dim ds As New DataSet




        Dim dt As New DataTable()
        If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()

        command.Connection = insurance_SQLcon
        command.CommandType = CommandType.StoredProcedure
        command.CommandText = "acc_dr_money_all"

        ' command.Parameters.AddWithValue("@ui", DDMain_MedicalStaff.SelectedValue)
        command.Parameters.AddWithValue("@user", Session("INC_User_Id"))
        Dim sda As New SqlDataAdapter(command)
        sda.Fill(dt)


        Return dt
    End Function

    Sub search2()
        Dim code As String = TextBox1.Text
        Dim dt As DataTable = Me.GetDatax()
        Dim dataView As DataView = dt.DefaultView
        If Not String.IsNullOrEmpty(code) Then
            dataView.RowFilter = "req_code =" & code
        End If
        GridView1.DataSource = dataView
        GridView1.DataBind()
    End Sub
    Sub new_search()



        Dim msg As String
        Dim sql_str As String = "  select "
        sql_str += " (select Main_MedicalStaff.MedicalStaff_AR_Name  from Main_MedicalStaff where MedicalStaff_ID  =HAG_Processes_Doctor.Processes_Doctor_ID )  as name
,isnull( (select Orginal_UserName   from user_table where user_id=PayUser  ) ,0) as payuser
,(Processes_State ),
 paydate   ,prn_id ,
isnull((select Orginal_UserName  from  HAG_Return as x inner join  user_table as y on x.Return_User =y.user_id  
and Return_ID in (select  Process_Return_ID   from HAG_Return_Process where Return_Process_ID  =Req_PID )),0) as return_users
  ,  isnull( (select Return_User_Date    from  HAG_Return as x inner join  user_table as y on x.Return_User =y.user_id    and Return_ID in 
    (select  Process_Return_ID   from HAG_Return_Process where Return_Process_ID  =Req_PID )),0) as Return_User_Date 

from HAG_Request   
inner join HAG_Processes_Doctor on  HAG_Request.Req_PID =HAG_Processes_Doctor.Doctor_Processes_ID 
inner join HAG_Processes on Processes_ID =Req_PID"
        sql_str += " where   HAG_Processes.Processes_Residual >0 and Req_Code =@DPID"


        msg = ""
        Dim vfound As Integer = 0
        If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()

        Dim sel_com As New SqlCommand(sql_str, insurance_SQLcon)
        sel_com.Parameters.AddWithValue("@DPID", TextBox1.Text)


        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        dt_result.Load(sel_com.ExecuteReader)

        If dt_result.Rows.Count > 0 Then
            If dt_result.Rows(0)!Processes_State = 3 Then
                msg = "تم ترجيع القيمة لهذه المعاملة عن طريق مستخدم  " & dt_result.Rows(0)!return_users & " بتاريخ : " & dt_result.Rows(0)!Return_User_Date & " لذا لايمكن تاكيد عملية الكشف "
            End If

            If (dt_result.Rows(0)!payuser) = "0" Then
            Else
                msg = "تم دفع القيمة لهذه المعاملة عن طريق  " & dt_result.Rows(0)!payuser
                msg = msg & " بتاريخ " & dt_result.Rows(0)!paydate
                msg = msg & " امر طباعه رقم " & dt_result.Rows(0)!prn_id
            End If

        End If

        If msg <> "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('" & msg & " ')", True)
            Exit Sub
        End If
        GridView2.Visible = False
        GridView1.Visible = True
        search3()
        'search2()
    End Sub
    Protected Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click

        new_search()
    End Sub

    Protected Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        'search2()
        new_search()
    End Sub



    Private Sub gv_users_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand




        If (e.CommandName = "EditUser") Then
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            Dim row As GridViewRow = GridView1.Rows(index)


            '--------------------------
            'Dim sql_str As String = "    Select DPID   From HAG_Processes where DPID=@DPID "

            'Dim vfound As Integer = 0

            'Dim sel_com As New SqlCommand(sql_str, Cn)
            'sel_com.Parameters.AddWithValue("@DPID", row.Cells(5).Text)


            'Dim dt_result As New DataTable
            'dt_result.Rows.Clear()
            'dt_result.Load(sel_com.ExecuteReader)

            'If dt_result.Rows.Count > 0 Then
            '    vfound = dt_result.Rows(0)!Processes_ID
            'Else
            '    vfound = 0
            'End If
            'If vfound = 0 Then
            '    msg = "تم ترجيع القيمة لهذه المعاملة لذا لايمكن تاكيد عملية الكشف "
            '    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('" & msg & " ')", True)
            '    Exit Sub
            'End If
            If IsNumeric(row.Cells(7).Text) = True Then
                Dim se As Integer = row.Cells(7).Text
                '--------------------------
                Dim CD As SqlCommand
                If row.Cells(6).Text = "تامين" Then
                    If Val(row.Cells(4).Text) > 0 Then  ' معناه استثناء دكتور وياخذ كاش على تامين

                        CD = New SqlCommand("update  HAG_Processes_Doctor  set   HAG_Processes_Doctor.DrPrs_State=1 ,Payuser=@Payuser, paydate=@paydate  where DPID=@DPID  ", insurance_SQLcon)
                    Else
                        CD = New SqlCommand("update  HAG_Processes_Doctor  set   Payuser=@Payuser, paydate=@paydate  where DPID=@DPID  ", insurance_SQLcon)
                    End If

                Else
                    CD = New SqlCommand("update  HAG_Processes_Doctor  set HAG_Processes_Doctor.DrPrs_State=1 ,Payuser=@Payuser , paydate=@paydate where DPID=@DPID  ", insurance_SQLcon)
                End If

                CD.Parameters.AddWithValue("@DPID", row.Cells(7).Text)
                CD.Parameters.AddWithValue("@Payuser", Session("INC_User_Id"))
                CD.Parameters.AddWithValue("@Paydate", Today.Date)
                CD.ExecuteNonQuery()


                CD = New SqlCommand("update HAG_Processes set Processes_State =2 where Processes_ID =@Processes_ID    ", insurance_SQLcon)
                CD.Parameters.AddWithValue("@Processes_ID", row.Cells(8).Text)
                CD.ExecuteNonQuery()

                insurance_SQLcon.Close()
                search()
                TextBox1.Text = ""
                TextBox1.Focus()


                '   ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "alertMessage", "alert('   تم التخزين  ')", True)

            End If

        End If
        '  Page.Response.Redirect("~/main_c.aspx")
    End Sub


    Private Sub gv_users_RowCommand2(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand



    End Sub



    'Function GetData4(prm1 As Integer, prn_id As Integer) As Ds_dataset


    '    If Session("acc_user_pay") Is Nothing Or Session("INC_User_Id") = 0 Then
    '        Response.Redirect("~/index.aspx", False)
    '    End If

    '    Dim dt As New Ds_dataset()

    '    Dim cmd As SqlCommand = New SqlCommand("acc_rep_dr", insurance_SQLcon)

    '    cmd.CommandType = CommandType.StoredProcedure


    '    cmd.Parameters.AddWithValue("@user", Session("INC_User_Id"))
    '    cmd.Parameters.AddWithValue("@dr", DDMain_MedicalStaff.SelectedValue)
    '    cmd.Parameters.AddWithValue("@prm1", prm1)
    '    cmd.Parameters.AddWithValue("@prn", prn_id)

    '    'cmd.Parameters.AddWithValue("@Date1", CDate(TxtDate_6.Text))
    '    'cmd.Parameters.AddWithValue("@Date2", CDate(TxtDate_7.Text))

    '    If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open()



    '    'Using con As New SqlConnection(conString)
    '    Using sda As New SqlDataAdapter()
    '        cmd.Connection = insurance_SQLcon

    '        sda.SelectCommand = cmd
    '        Using dsCustomers As New Ds_dataset()
    '            sda.Fill(dsCustomers, "acc_rep_userpaycash1")
    '            Return dsCustomers
    '        End Using

    '    End Using

    '    insurance_SQLcon.Close()


    'End Function
End Class