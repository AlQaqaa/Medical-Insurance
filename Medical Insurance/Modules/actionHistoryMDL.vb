Imports System.Data
Imports System.Data.SqlClient

Module actionHistoryMDL

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Public dt_actions As New DataTable

    Public Sub add_action(ByVal system_no As Integer, ByVal pros_level As Integer, ByVal pros_type As Integer, ByVal pros_desc As String, ByVal user_id As Integer, ByVal user_ip As String)
        Dim ins_com As New SqlCommand("INSERT INTO Main_History_Log (SystemNo,ProcessesLevel,ProcessesType,ProcessesDesc,UserId,UserIp) VALUES (@SystemNo,@ProcessesLevel,@ProcessesType,@ProcessesDesc,@UserId,@UserIp)", insurance_SQLcon)
        ins_com.Parameters.Add("SystemNo", SqlDbType.Int).Value = system_no
        ins_com.Parameters.Add("ProcessesLevel", SqlDbType.Int).Value = pros_level
        ins_com.Parameters.Add("ProcessesType", SqlDbType.Int).Value = pros_type
        ins_com.Parameters.Add("ProcessesDesc", SqlDbType.NVarChar).Value = pros_desc
        ins_com.Parameters.Add("UserId", SqlDbType.Int).Value = user_id
        ins_com.Parameters.Add("UserIp", SqlDbType.NVarChar).Value = user_ip
        insurance_SQLcon.Close()
        insurance_SQLcon.Open()
        ins_com.ExecuteNonQuery()
        insurance_SQLcon.Close()
    End Sub

    Public Sub get_all_actions()
        dt_actions.Clear()
        Dim cmd As New SqlCommand("SELECT * FROM Main_History_Log", insurance_SQLcon)
        insurance_SQLcon.Open()
        dt_actions.Load(cmd.ExecuteReader)
        insurance_SQLcon.Close()
        cmd = Nothing
    End Sub
End Module
