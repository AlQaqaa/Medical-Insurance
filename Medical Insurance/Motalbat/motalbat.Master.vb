﻿Public Class motalbat
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then
            'If Session("User_Id") Is Nothing Or Session("User_Id") = 0 Then
            '    Response.Redirect("http://10.10.1.10", True)
            'End If

            'If Session("systemlogin") <> "401" Then
            '    Response.Redirect("http://10.10.1.10", True)
            'End If
        End If

    End Sub

End Class