Imports System.IO
Imports System.Security.Cryptography
Imports System.Data.SqlClient

Public Class index
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim sel_com As New SqlCommand("SELECT * FROM User_Table WHERE user_id =1 ", insurance_SQLcon)
        'Dim dt_user As New DataTable
        'dt_user.Rows.Clear()
        'insurance_SQLcon.Close()
        'insurance_SQLcon.Open()
        'dt_user.Load(sel_com.ExecuteReader)
        'insurance_SQLcon.Close()

        'If dt_user.Rows.Count > 0 Then
        '    Dim dr_user = dt_user.Rows(0)
        '    Session.Item("INC_User_Id") = dr_user!user_id
        '    Session.Item("INC_User_name") = dr_user!user_name
        '    Session.Item("INC_user_full_name") = dr_user!Orginal_UserName
        '    Session.Item("INC_User_type") = dr_user!user_type
        '    Session.Item("INC_User_ip") = dr_user!user_ip
        '    If dr_user!user_type <> 1 And dr_user!user_type <> 0 Then
        '        If getUserPermissions().Rows.Count = Nothing Then
        '            MsgBox("No Permissions")
        '        Else
        '            Session("User_per") = getUserPermissions().Rows(0)

        '        End If
        '    End If


        'End If

        'Session("INC_hublogin") = 1
        'Session("systemlogin") = "401"
        'Response.Redirect("default.aspx", True)

        Try
            If Session("INC_hublogin") = 1 Then
                Session("INC_hublogin") = Nothing
                Session("systemlogin") = Nothing
                Response.Redirect("http://10.10.1.10/Default.aspx?flag:1", True)
            Else

                If Session("systemlogin") = "401" Then
                    Session("INC_hublogin") = Nothing
                    Session("systemlogin") = Nothing
                    Response.Redirect("http://10.10.1.10/Default.aspx?flag:2", True)
                Else
                    If (Request.QueryString("u")) = "" Then
                        Session("INC_hublogin") = Nothing
                        Session("systemlogin") = Nothing
                        Response.Redirect("http://10.10.1.10/Default.aspx?flag:3", True)

                    End If

                    Dim u As String = Decrypt(Request.QueryString("u").Replace(" ", "+"))
                    Dim p As String = Decrypt(Request.QueryString("p").Replace(" ", "+"))
                    Dim d As String = Decrypt(Request.QueryString("d").Replace(" ", "+"))
                    Dim t As String = Decrypt(Request.QueryString("t").Replace(" ", "+"))
                    Label2.Text = u
                    Dim sel_com As New SqlCommand("SELECT * FROM User_Table WHERE user_id = " & u, insurance_SQLcon)
                    Dim dt_user As New DataTable
                    dt_user.Rows.Clear()
                    insurance_SQLcon.Close()
                    insurance_SQLcon.Open()
                    dt_user.Load(sel_com.ExecuteReader)
                    insurance_SQLcon.Close()

                    If dt_user.Rows.Count > 0 Then
                        Dim dr_user = dt_user.Rows(0)
                        Session.Item("INC_User_Id") = dr_user!user_id
                        Session.Item("INC_User_name") = dr_user!user_name
                        Session.Item("INC_user_full_name") = dr_user!Orginal_UserName
                        Session.Item("INC_User_type") = dr_user!user_type
                        Session.Item("INC_User_ip") = dr_user!user_ip

                        If dr_user!user_type <> 1 And dr_user!user_type <> 0 Then
                            If getUserPermissions().Rows.Count = Nothing Then
                                Session("INC_hublogin") = Nothing
                                Session("systemlogin") = Nothing
                                Response.Redirect("http://10.10.1.10/Default.aspx?flag:NoPermissions", True)
                                Exit Sub
                            Else
                                Session("User_per") = getUserPermissions().Rows(0)
                            End If
                            'If getUserPermissions().Rows.Count = 0 Then
                            '    Session("INC_hublogin") = Nothing
                            '    Session("systemlogin") = Nothing
                            '    Response.Redirect("http://10.10.1.10/Default.aspx?flag:4", True)
                            '    Exit Sub
                            'End If

                        End If

                    End If

                    Dim tim1 As Date = DateTime.FromOADate(t)

                    'If DateDiff(DateInterval.Second, CDate(tim1.ToString("HH:mm:ss")), CDate(DateTime.Now.ToString("HH:mm:ss"))) > 5 Then
                    '    Response.Redirect("http://10.10.1.10/Default.aspx", True)
                    'Else
                    Session("INC_hublogin") = 1
                    Session("systemlogin") = "401"
                    Response.Redirect("default.aspx", False)
                    'End If

                End If

            End If
        Catch ex As Exception
            'Label1.Text = ex.Message
            Response.Redirect("http://10.10.1.10/Default.aspx?flag:5", True)
        End Try

    End Sub

    Private Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function

    Public Function getUserPermissions() As DataTable
        Dim dt_result As New DataTable

        Dim sel_com As New SqlCommand("SELECT * FROM SYS_INC_Permissions WHERE user_id = " & Session("INC_User_Id"), insurance_SQLcon)
        insurance_SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        insurance_SQLcon.Close()

        Return dt_result
    End Function

End Class