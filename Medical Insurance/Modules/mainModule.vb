﻿Module mainModule

    Dim currencies As New List(Of CurrencyInfo)()

    ' Get Client IP Address 
    Public Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

    Public Function GetNumberToWord(Value As Double) As String
        currencies.Add(New CurrencyInfo(CurrencyInfo.Currencies.Libya))
        Dim toWord As New ToWord(Convert.ToDouble(Value), currencies(0))
        Return toWord.ConvertToArabic()
    End Function
End Module
