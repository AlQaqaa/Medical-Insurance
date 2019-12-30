Imports System.Data.SqlClient

Module checkModule
    Dim SQLcon As New SqlConnection(ConfigurationManager.ConnectionStrings("insurance_CS").ToString)

    Public Function checkPatient(ByVal p_id As Integer, ByVal c_id As Integer, ByVal doc_id As Integer, ByVal ser_id As Integer, ByVal ser_price As Decimal) As Array

        Dim patient_sts As Boolean = False ' حالة المنتفع
        Dim company_sts As Boolean = False ' حالة الشركة
        Dim max_company_val As Decimal = 0 ' سقف الشركة
        Dim max_company_card As Decimal = 0 ' سقف البطاقة العام
        Dim per_company_pat As Integer = 0 ' النسبة العامة التي يدفعها المشترك
        Dim payment_type As Integer = 0 ' طريقة الدفع
        Dim contract_no As Integer = 0 ' رقم العقد

        If p_id <> 0 Then
            '############# التحقق من حالة المنتفع وصلاحية البطاقة ##############
            Dim sel_pat As New SqlCommand("SELECT P_STATE, EXP_DATE FROM INC_PATIANT WHERE PINC_ID = " & p_id, SQLcon)
            Dim dt_res As New DataTable
            dt_res.Rows.Clear()
            SQLcon.Close()
            SQLcon.Open()
            dt_res.Load(sel_pat.ExecuteReader)
            SQLcon.Close()
            If dt_res.Rows.Count > 0 Then
                Dim dr = dt_res.Rows(0)

                If dr!P_STATE = 0 And dr!EXP_DATE > Date.Now.Date Then
                    patient_sts = True
                Else
                    patient_sts = False
                End If

            End If
            '############# التحقق من الشركة وإحضار سقف الشركة والبطاقة ونسبة المنتفع العامة ##############
            Dim sel_com As New SqlCommand("SELECT  TOP (1) dbo.INC_COMPANY_DATA.C_STATE, dbo.INC_COMPANY_DETIAL.MAX_VAL, dbo.INC_COMPANY_DETIAL.MAX_CARD, dbo.INC_COMPANY_DETIAL.CONTRCT_TYPE, dbo.INC_COMPANY_DETIAL.PATIAINT_PER, dbo.INC_COMPANY_DETIAL.PYMENT_TYPE, dbo.INC_COMPANY_DETIAL.CONTRACT_NO FROM dbo.INC_COMPANY_DETIAL INNER JOIN dbo.INC_COMPANY_DATA ON dbo.INC_COMPANY_DETIAL.C_ID = dbo.INC_COMPANY_DATA.C_id WHERE (dbo.INC_COMPANY_DATA.C_STATE = 0) AND(dbo.INC_COMPANY_DETIAL.DATE_END > GETDATE()) ORDER BY dbo.INC_COMPANY_DETIAL.CONTRACT_NO DESC", SQLcon)
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
            Else
                company_sts = False
            End If
        End If

    End Function

    ' جلب قيمة سقف العيادة
    Public Function getMaxClinicValue(ByVal clinic_id As Integer, ByVal company_no As Integer, ByVal contract_no As Integer) As Decimal
        Dim sel_com As New SqlCommand("SELECT (CASE WHEN (GROUP_NO) <> 0 THEN (SELECT MAX_VALUE FROM INC_CLINIC_GROUP WHERE INC_CLINIC_GROUP.GROUP_NO = INC_CLINICAL_RESTRICTIONS.GROUP_NO) ELSE (SELECT MAX_VALUE FROM INC_CLINICAL_RESTRICTIONS AS M_X WHERE M_X.CLINIC_ID = INC_CLINICAL_RESTRICTIONS.CLINIC_ID AND M_X.C_ID = INC_CLINICAL_RESTRICTIONS.C_ID) END) AS MAX_VALUE FROM INC_CLINICAL_RESTRICTIONS WHERE CLINIC_ID = " & clinic_id & " AND C_ID = " & company_no & " AND CONTRACT_NO = " & contract_no, SQLcon)
        SQLcon.Close()
        SQLcon.Open()
        Return sel_com.ExecuteScalar
        SQLcon.Close()
    End Function

End Module
