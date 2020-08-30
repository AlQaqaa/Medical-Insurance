
Imports System.Data.SqlClient

Public Class list1
    Inherits System.Web.UI.Page
    Dim Cnx As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Public CDx As SqlCommand
    Public RDx As SqlDataReader
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            '  getWhileLoopDataN()
        End If

    End Sub




End Class