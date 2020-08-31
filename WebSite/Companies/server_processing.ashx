<%@ WebHandler Language="C#" Class="server_processing" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class server_processing : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string reqtype = context.Request["reqtype"];
        string id = context.Request["reqtype"];

        List<string> columns = new List<string>();
        columns.Add("PINC_ID");
        columns.Add("NAME_ARB");
        columns.Add("NAME_ENG");
        columns.Add("CARD_NO");
        columns.Add("BIRTHDATE");
        columns.Add("BAGE_NO");
        columns.Add("PHONE_NO");
        columns.Add("EXP_DATE");
        columns.Add("NAT_NUMBER");
        columns.Add("P_STATE");
        columns.Add("CONST_ID");

        Int32 ajaxDraw = Convert.ToInt32(context.Request.Form["draw"]);
        Int32 ajaxRequestStart = Convert.ToInt32(context.Request.Form["start"]);
        ajaxRequestStart = ajaxRequestStart + 1;
        Int32 ajaxRequestLength = Convert.ToInt32(context.Request.Form["length"]);

        object searchby = context.Request.Form["search[value]"];
        string sortColumn = context.Request.Form["order[0][column]"];
        sortColumn = columns[Convert.ToInt32(sortColumn)];
        string sortDirection = context.Request.Form["order[0][dir]"];

        Int32 startRecordperPage = ajaxRequestStart;
        Int32 finRecordperPage = startRecordperPage + ajaxRequestLength - 1;

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["insurance_CS"].ToString());

        conn.Open();

        string sql;

        sql = " SELECT PINC_ID, CARD_NO, NAME_ARB, NAME_ENG, CONVERT(VARCHAR, BIRTHDATE, 23) AS BIRTHDATE,";
        sql += " BAGE_NO, C_ID, PHONE_NO, CONVERT(VARCHAR, EXP_DATE, 23) AS EXP_DATE, ";
        sql += " NAT_NUMBER, (CASE WHEN (P_STATE) = 0 THEN 'مفعل' ELSE 'موقوف' END) AS ";
        sql += " P_STATE, (CASE WHEN (CONST_ID) = 0 THEN 'المشترك'  WHEN (CONST_ID) = 1 ";
        sql += " THEN 'الأب'  WHEN (CONST_ID) = 2 THEN 'الأم'  WHEN (CONST_ID) = 3 THEN 'الزوجة'  WHEN";
        sql += " (CONST_ID) = 4 THEN 'الأبن'  WHEN (CONST_ID) = 5 THEN 'الابنة'  WHEN (CONST_ID) = 6 ";
        sql += " THEN 'الأخ'  WHEN (CONST_ID) = 7 THEN 'الأخت'  WHEN (CONST_ID) = 8 THEN 'الزوج'  WHEN ";
        sql += " (CONST_ID) = 9 THEN 'زوجة الأب' END) AS CONST_ID FROM INC_PATIANT WHERE 1=1 ";

        if (id == "null")
            sql += "  ";
        else
            sql += " and C_ID=@id";

        SqlDataAdapter da = new SqlDataAdapter(sql, conn);
        if (id != "null")
            da.SelectCommand.Parameters.AddWithValue("@id", id);

        DataTable dt = new DataTable();
        da.Fill(dt);
        da.Dispose();
        conn.Close();

        List<People> peoples = new List<People>();

        foreach (DataRow dr in dt.Rows)
        {

            People people = new People()
            {
                p_sts = dr["P_STATE"].ToString(),
                ar_name = "<a href='patientInfo.aspx?pID=" + dr["PINC_ID"].ToString() + "'>" + dr["NAME_ARB"].ToString() + "</a>",
                en_name = dr["NAME_ENG"].ToString(),
                c_no = dr["CARD_NO"].ToString(),
                b_dt = dr["BIRTHDATE"].ToString(),
                b_no = dr["BAGE_NO"].ToString(),
                phon_no = dr["PHONE_NO"].ToString(),
                exp_dt = dr["EXP_DATE"].ToString(),
                nat_num = dr["NAT_NUMBER"].ToString(),
                
                const_no = dr["CONST_ID"].ToString(),

            };
            peoples.Add(people);

        }
        Int32 recordTotal = RecordTotal(searchby, reqtype, id);
        Int32 recordFiltered = recordTotal;

        DataTableAjaxClass dtAC = new DataTableAjaxClass()
        {
            draw = ajaxDraw,
            recordsFiltered = recordTotal,
            recordsTotal = recordTotal,
            data = peoples
        };


        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        context.Response.Write(js.Serialize(dtAC));

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public Int32 RecordTotal(object searchby, string reqtype, string id)
    {
        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["insurance_CS"].ToString());
        conn.Open();
        string sql;

        if (reqtype != null)
        {
            sql = "select count(*) from INC_PATIANT ";
        }
        else { sql = " "; }

        SqlCommand comm = new SqlCommand(sql, conn);
        Int32 result = Convert.ToInt32(comm.ExecuteScalar());
        comm.Dispose();
        conn.Close();
        return result;
    }

}

public class DataTableAjaxClass
{
    public int draw;
    public int recordsTotal;
    public int recordsFiltered;
    public List<People> data;
}
public class People
{
    public string ar_name;
    public string en_name;
    public string c_no;
    public string b_dt;
    public string b_no;
    public string phon_no;
    public string exp_dt;
    public string nat_num;
    public string p_sts;
    public string const_no;

}