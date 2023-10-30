Imports System.Data.SqlClient
Imports System.Globalization
Imports Microsoft.ReportingServices.Interfaces

Public Class PrintCards
    Inherits System.Web.UI.Page

    Dim insurance_SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        Dim dtResult As New DataTable
        Dim stringBuilder As New StringBuilder
        Dim ss As String = "select  
y.Orginal_UserName  ,  dbo.GetDateFromOADateNumber (DateAdded )  AS DD,
PatientID  
,(select NAME_ARB    from  INC_PATIANT  as x  where x.PINC_ID =PatientID) as  namep,
(select C_Name_Arb from INC_COMPANY_DATA where C_ID = (select C_ID    from  INC_PATIANT  as x  where x.PINC_ID =PatientID)) as c_name
, 'http://10.10.1.10:3630/'+LogFile   as  filename
  from Insurance_Patients_Forms_Validation as x 
inner join  User_Table as  y  on x.AddedBy  =y.user_id 
WHERE CONVERT(date,dateadd(day, DateAdded, '1899/12/30'),103) = @DateAdded"

        If txtNo.Text <> "" Then 
            ss += " AND PatientCode = '" + txtNo.Text + "'"
        End If
        ss += " order by DateAdded  desc"

        Try
            Dim sqlComm As New SqlCommand(ss, insurance_SQLcon)
            sqlComm.Parameters.AddWithValue("DateAdded", DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture))
            If insurance_SQLcon.State = ConnectionState.Closed Then insurance_SQLcon.Open
            dtResult.Load(sqlComm.ExecuteReader)
            insurance_SQLcon.Close
            If dtResult.Rows.Count > 0 Then
                For i = 0 To dtResult.Rows.Count - 1
                    stringBuilder.Append("<tr>")
                    stringBuilder.Append("<td>" & dtResult.Rows(i)("Orginal_UserName") & "</td>")
                    stringBuilder.Append("<td>" & dtResult.Rows(i)("DD") & "</td>")
                    stringBuilder.Append("<td>" & dtResult.Rows(i)("namep") & "</td>")
                    stringBuilder.Append("<td>" & dtResult.Rows(i)("c_name") & "</td>")
                    stringBuilder.Append("<td><a href='" & dtResult.Rows(i)("filename") & "' target='_blank'>اضغط هنا لعرض البطاقة</a></td>")
                    stringBuilder.Append("</tr>")
                Next
                Label1.Text = ""
                Literal1.Text = stringBuilder.ToString
            Else
                Literal1.Text = ""
                Label1.Text = "لا يوجد بيانات لعرضها"
            End If
        Catch ex As Exception

        End Try

    End Sub
End Class