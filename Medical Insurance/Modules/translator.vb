Imports System.Data
Imports System.Data.SqlClient

Module translator
    Dim con_str As New SqlConnection(ConfigurationManager.ConnectionStrings("con_string").ToString)

    Public Function TransA2E(W As String) As String
        Dim str As String = W
        Dim strName As String = ""
        Dim strArr() As String
        Dim count As Integer
        strArr = str.Split(" ")
        If con_str.State = ConnectionState.Closed Then con_str.Open()
        For count = 0 To strArr.Length - 1
            Dim Sql As String
            Sql = "Select * From TRNS_Names Where AR_Name = @PrmARName"
            Dim Com As New SqlCommand(Sql, con_str)
            Com.Parameters.Add(New SqlClient.SqlParameter("@PrmARName", SqlDbType.NVarChar)).Value = strArr(count)
            Dim red As SqlDataReader = Com.ExecuteReader
            If red.Read Then
                strName = strName + red!EN_Name + " "
            Else
                strName = strName + TransLA2E(strArr(count)) + " "
            End If
            red.Close()
        Next
        Return strName

    End Function
    Function TransLA2E(W As String) As String
        Try
            Dim p As Integer
            Dim AL(), EL()
            AL = (New String() {" ال", "َا", "ا", "أ", "آ", "ى", "إ", "ؤ", "ئ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "ة", "ُوْ", "و", "ِيْ", "ي", "َ", "ً", "ُ", "ٌ", "ِ", "ٍ", "ء"})
            EL = (New String() {" Al-", "a", "a", "a", "a", "a", "i", "u", "i", "b", "t", "th", "j", "ha", "kh", "d", "th", "r", "z", "s", "sh", "s", "sh", "t", "th", "a", "g", "f", "q", "k", "l", "m", "n", "h", "h", "u", "ou", "i", "y", "a", "tn", "u", "un", "i", "in", "'a"})
            Do
                p = InStr(p + 1, W, "ّ")
                If p > 0 Then W = Left(W, p - 1) & Mid(W, p - 1, 1) & Mid(W, p + 1)
            Loop While p > 0
            For R = LBound(AL) To UBound(AL)
                W = Replace(W, AL(R), EL(R))
            Next
        Catch ex As Exception
        End Try
        Return W
    End Function
End Module
