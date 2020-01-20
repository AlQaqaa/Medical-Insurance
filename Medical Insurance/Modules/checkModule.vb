Imports System.Data.SqlClient

Module checkModule
    Dim SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Dim patient_sts As Boolean = False ' حالة المنتفع
    Dim company_sts As Boolean = False ' حالة الشركة
    Dim max_company_val As Decimal = 0 ' سقف الشركة
    Dim max_company_card As Decimal = 0 ' سقف البطاقة العام
    Dim max_company_person As Decimal = 0 ' سقف المنتفع العام
    Dim per_company_pat As Integer = 0 ' النسبة العامة التي يدفعها المشترك
    Dim payment_type As Integer = 0 ' طريقة الدفع
    Dim contract_no As Integer = 0 ' رقم العقد
    Dim contract_dt_start As String = Date.Now.Date ' تاريخ بداية العقد
    Dim contract_dt_end As String = Date.Now.Date ' تاريخ نهاية العقد
    Dim company_no As Integer = 0 ' رقم الشركة
    Dim profile_no As Integer = 0 ' رقم ملف الأسعار

    Public Function checkPatient(ByVal p_id As Integer, ByVal c_id As Integer, ByVal doc_id As Integer, ByVal ser_id As Integer) As Array
        ' p_id رقم المريض
        ' c_id رقم الشركة
        ' doc_id رقم الطبيب
        ' ser_id رقم الخدمة

        If p_id <> 0 Then
            '############# التحقق من حالة المنتفع وصلاحية البطاقة ##############
            Dim sel_pat As New SqlCommand("SELECT P_STATE, EXP_DATE, C_ID FROM INC_PATIANT WHERE PINC_ID = " & p_id, SQLcon)
            Dim dt_res As New DataTable
            dt_res.Rows.Clear()
            SQLcon.Close()
            SQLcon.Open()
            dt_res.Load(sel_pat.ExecuteReader)
            SQLcon.Close()
            If dt_res.Rows.Count > 0 Then
                Dim dr_pat = dt_res.Rows(0)
                company_no = dr_pat!C_ID
                If dr_pat!P_STATE = 0 And dr_pat!EXP_DATE > Date.Now.Date Then
                    patient_sts = True
                Else
                    patient_sts = False
                End If

            End If


            '############# التحقق من الشركة وإحضار سقف الشركة والبطاقة ونسبة المنتفع العامة ##############
            Dim sel_com As New SqlCommand("SELECT  TOP (1) dbo.INC_COMPANY_DATA.C_STATE, dbo.INC_COMPANY_DETIAL.MAX_VAL, dbo.INC_COMPANY_DETIAL.MAX_CARD, dbo.INC_COMPANY_DETIAL.DATE_START, dbo.INC_COMPANY_DETIAL.DATE_END, dbo.INC_COMPANY_DETIAL.PROFILE_PRICE_ID, dbo.INC_COMPANY_DETIAL.CONTRCT_TYPE, dbo.INC_COMPANY_DETIAL.PATIAINT_PER, dbo.INC_COMPANY_DETIAL.PYMENT_TYPE, dbo.INC_COMPANY_DETIAL.CONTRACT_NO FROM dbo.INC_COMPANY_DETIAL INNER JOIN dbo.INC_COMPANY_DATA ON dbo.INC_COMPANY_DETIAL.C_ID = dbo.INC_COMPANY_DATA.C_id WHERE (dbo.INC_COMPANY_DATA.C_ID = " & company_no & ") AND (dbo.INC_COMPANY_DATA.C_STATE = 0) AND (dbo.INC_COMPANY_DETIAL.DATE_END > GETDATE()) ORDER BY dbo.INC_COMPANY_DETIAL.CONTRACT_NO DESC", SQLcon)
            Dim com_result As New DataTable
            com_result.Rows.Clear()
            SQLcon.Close()
            SQLcon.Open()
            com_result.Load(sel_com.ExecuteReader)
            SQLcon.Close()
            If com_result.Rows.Count > 0 Then
                Dim dr_res = com_result.Rows(0)
                company_sts = True
                max_company_val = dr_res!MAX_VAL
                max_company_card = dr_res!MAX_CARD
                per_company_pat = dr_res!PATIAINT_PER
                payment_type = dr_res!PYMENT_TYPE
                contract_no = dr_res!CONTRACT_NO
                max_company_person = dr_res!MAX_PERSON
                contract_dt_start = dr_res!DATE_START
                contract_dt_end = dr_res!DATE_START
                profile_no = dr_res!PROFILE_PRICE_ID
            Else
                company_sts = False
            End If

            ' التأكد من رصيد الشركة هل يكفي لسعر الخدمة أم لا
            If (max_company_val - getCompanyProcesses(company_no, contract_dt_start, Date.Now.Date) - getSubServicePrice(ser_id, profile_no, payment_type)) < 0 Then
                company_sts = False
            End If

            If getPatientProcesses(company_no, contract_dt_end, Date.Now.Date) > getSubServicePrice(ser_id, profile_no, payment_type) Then
                patient_sts = False
            End If

        Else
            ' جلب سعر الخدمة في حال كان المريض لا يتبع للتأمين
            Dim get_porf As New SqlCommand("SELECT profile_Id FROM INC_PRICES_PROFILES WHERE is_default = 1", SQLcon)
            SQLcon.Close()
            SQLcon.Open()
            Dim prof_id As Integer = get_porf.ExecuteScalar
            SQLcon.Close()
            Dim service_price As Decimal = getSubServicePrice(ser_id, prof_id, 1)
            '/ جلب سعر الخدمة في حال كان المريض لا يتبع للتأمين

        End If

    End Function

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
        Dim sel_com As New SqlCommand("SELECT PERSON_PER, FAMILY_PER, PARENT_PER, MAX_PERSON_VAL, MAX_FAMILY_VAL, SER_STATE, PAYMENT_TYPE FROM INC_SERVICES_RESTRICTIONS WHERE C_ID = " & com_id & " AND SER_ID = " & ser_id & " AND CONTRACT_NO = " & contract_num, SQLcon)
        Dim dt_result As New DataTable
        dt_result.Rows.Clear()
        SQLcon.Close()
        SQLcon.Open()
        dt_result.Load(sel_com.ExecuteReader)
        SQLcon.Close()

        Return dt_result

    End Function

End Module
