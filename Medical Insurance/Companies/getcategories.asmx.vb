Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _


<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
<System.Web.Script.Services.ScriptService>
Public Class getcategories
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function
    <WebMethod>
    Public Function getbankslist() As List(Of Employee)
        Dim Countries As List(Of Employee) = New List(Of Employee)()
        Dim c As Employee = New Employee()

        Dim cs As String = ConfigurationManager.ConnectionStrings("insurance_CS").ConnectionString

        Using con As SqlConnection = New SqlConnection(cs)
            Dim ss As String = " Select C_ID, C_Name_Arb + (case when (C_Level <> 0) then (select ' - ' "
            ss += "  + C_Name_Arb from [INC_COMPANY_DATA] as tbl1 where tbl1.C_ID = tbl2.C_Level) "
            ss += "  else  '' end) "
            ss += "  as C_Name_Arb FROM [INC_COMPANY_DATA] as tbl2 WHERE ([C_STATE] =0)"


            Dim cmd As SqlCommand = New SqlCommand(ss, con)
            cmd.CommandType = CommandType.Text
            'cmd.Parameters.Add(New SqlParameter("@Id", employeeId))
            con.Open()
            Dim rdr As SqlDataReader = cmd.ExecuteReader()

            While rdr.Read()
                c = New Employee()
                c.Shortcut_id = rdr("C_ID").ToString()
                c.Shortcut_name = rdr("C_Name_Arb").ToString()

                Countries.Add(c)
            End While
        End Using

        Return Countries
    End Function

    Public Class Employee
        Public Property Shortcut_id As String
        Public Property Shortcut_name As String

    End Class

End Class