Imports System.IO
Imports System.Security.Cryptography
Imports System.Data.SqlClient

Public Class index
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("hublogin") = 1 Then
            Response.Redirect("http://10.10.1.10:888/Default.aspx", True)
        Else

            If Session("systemlogin") = "401" Then
                Response.Redirect("http://10.10.1.10:888/Default.aspx", True)
            Else
                If (Request.QueryString("u")) = "" Then
                    Response.Redirect("http://10.10.1.10:888/Default.aspx", True)

                End If

                Dim u As String = Decrypt(Request.QueryString("u").Replace(" ", "+"))
                Dim p As String = Decrypt(Request.QueryString("p").Replace(" ", "+"))
                Dim d As String = Decrypt(Request.QueryString("d").Replace(" ", "+"))
                Dim t As String = Decrypt(Request.QueryString("t").Replace(" ", "+"))

                Dim sel_com As New SqlCommand("SELECT * FROM User_Table WHERE user_id = " & u, insurance_SQLcon)
                Dim dt_user As New DataTable
                dt_user.Rows.Clear()
                insurance_SQLcon.Close()
                insurance_SQLcon.Open()
                dt_user.Load(sel_com.ExecuteReader)
                insurance_SQLcon.Close()

                If dt_user.Rows.Count > 0 Then
                    Dim dr_user = dt_user.Rows(0)
                    Session.Item("User_Id") = dr_user!user_id
                    Session.Item("User_name") = dr_user!user_name
                    Session.Item("user_full_name") = dr_user!Orginal_UserName
                    Session.Item("User_per") = dr_user!user_type
                    Session.Item("User_ip") = dr_user!user_ip
                End If

                Dim tim1 As Date = DateTime.FromOADate(t)

                'If DateDiff(DateInterval.Second, CDate(tim1.ToString("HH:mm:ss")), CDate(DateTime.Now.ToString("HH:mm:ss"))) > 5 Then
                '    Response.Redirect("http://10.10.1.10:888/Default.aspx", True)
                'Else
                Session("hublogin") = 1
                    Session("systemlogin") = "401"
                    Response.Redirect("default.aspx", False)
                'End If

            End If

        End If

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

End Class