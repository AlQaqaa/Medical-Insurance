Imports System.Data.SqlClient

Module checkModule
    Dim SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    ' جلب قيمة سقف العيادة
    Public Function getMaxClinicValue(ByVal clinic_id As Integer, ByVal company_id As Integer, ByVal contract_no As Integer) As Decimal
        Dim sel_com As New SqlCommand("SELECT (CASE WHEN (GROUP_NO) <> 0 THEN (SELECT MAX_VALUE FROM INC_CLINIC_GROUP WHERE INC_CLINIC_GROUP.GROUP_NO = INC_CLINICAL_RESTRICTIONS.GROUP_NO) ELSE (SELECT MAX_VALUE FROM INC_CLINICAL_RESTRICTIONS AS M_X WHERE M_X.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID AND M_X.C_ID = INC_CLINICAL_RESTRICTIONS.C_ID) END) AS MAX_VALUE FROM INC_CLINICAL_RESTRICTIONS WHERE CLINIC_ID = " & clinic_id & " AND C_ID = " & company_id & " AND CONTRACT_NO = " & contract_no, SQLcon)
        SQLcon.Close()
        SQLcon.Open()
        Return sel_com.ExecuteScalar
        SQLcon.Close()
    End Function

    ' جلب إجمالي مصروفات الشركة بحسب تاريخ معين
    Public Function getCompanyProcesses(ByVal company_id As Integer, ByVal start_dt As Date, ByVal end_dt As Date) As Decimal
        Dim sel_com As New SqlCommand("SELECT SUM(Processes_Paid) AS SUM_PAID FROM HAG_Processes WHERE Processes_State = 3 AND Processes_Date > '" & start_dt & "' AND Processes_Date < '" & end_dt & "'", SQLcon)
        SQLcon.Close()
        SQLcon.Open()
        Return sel_com.ExecuteScalar
        SQLcon.Close()
    End Function

    ' جلب إجمالي مصروفات المنتفع بحسب تاريخ معين
    Public Function getPatientProcesses(ByVal company_id As Integer, ByVal start_dt As Date, ByVal end_dt As Date) As Decimal
        Dim sel_com As New SqlCommand("SELECT SUM(Processes_Paid) AS SUM_PAID FROM HAG_Processes WHERE Processes_State = 3 AND Processes_Date > '" & start_dt & "' AND Processes_Date < '" & end_dt & "'", SQLcon)
        SQLcon.Close()
        SQLcon.Open()
        Return sel_com.ExecuteScalar
        SQLcon.Close()
    End Function

    ' جلب سعر الخدمة 
    Public Function getSubServicePrice(ByVal subServiceID As Integer, ByVal profile_price As Integer, ByVal payment_tp As Integer) As Decimal
        Dim sql_str As String = ""

        Select Case payment_tp
            Case 1
                sql_str = "SELECT CASH_PRS FROM INC_SERVICES_PRICES WHERE SER_ID = " & subServiceID & " AND PROFILE_PRICE_ID = " & profile_price
            Case 2
                sql_str = "SELECT INS_PRS FROM INC_SERVICES_PRICES WHERE SER_ID = " & subServiceID & " AND PROFILE_PRICE_ID = " & profile_price
            Case 3
                sql_str = "SELECT INVO_PRS FROM INC_SERVICES_PRICES WHERE SER_ID = " & subServiceID & " AND PROFILE_PRICE_ID = " & profile_price
        End Select
        Dim sel_com As New SqlCommand(sql_str, SQLcon)
        SQLcon.Close()
        SQLcon.Open()
        Return sel_com.ExecuteScalar
        SQLcon.Close()

    End Function

    ' إحضار شروط الخدمة
    Public Function getServicesRestrictions(ByVal ser_id As Integer, ByVal com_id As Integer, ByVal contract_num As Integer) As DataTable
        Dim sel_com As New SqlCommand("SELECT PERSON_PER, FAMILY_PER, PARENT_PER, MAX_PERSON_VAL, MAX_FAMILY_VAL, SER_STATE, PAYMENT_TYPE FROM INC_SUB_SERVICES_RESTRICTIONS WHERE C_ID = " & com_id & " AND SubService_ID = " & ser_id & " AND CONTRACT_NO = " & contract_num, SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        SQLcon.Close()
        SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        SQLcon.Close()

        Return dt_result

    End Function

End Module
