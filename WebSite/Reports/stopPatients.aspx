<%@ Page Title="تقرير عن المنتفعين" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="stopPatients.aspx.vb" Inherits="Medical_Insurance.stopPatients" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header">
            تقرير عن المنتفعين 
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label>البحث عن</label>
                        <asp:DropDownList ID="DropDownList1" CssClass="form-control drop-down-list" runat="server">
                        <asp:ListItem Value="0">الكل</asp:ListItem>
                        <asp:ListItem Value="1">الموقوفين</asp:ListItem>
                        <asp:ListItem Value="2">إنتهاء صلاحية البطاقة</asp:ListItem>
                        <asp:ListItem Value="3">المفعلين</asp:ListItem>
                    </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label>الشركة</label>
                        <asp:DropDownList ID="DropDownList2" CssClass="form-control drop-down-list chosen-select" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_Name_Arb" DataValueField="C_ID">
                        </asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [C_ID], 'جميع الشركات' AS [C_Name_Arb] FROM [INC_COMPANY_DATA] UNION SELECT [C_ID], [C_Name_Arb] FROM [INC_COMPANY_DATA] WHERE C_STATE = 0"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label>نوع البحث</label>
                        <asp:DropDownList ID="DropDownList3" CssClass="form-control drop-down-list" runat="server">
                        <asp:ListItem Value="0">الكل</asp:ListItem>
                        <asp:ListItem Value="1">المشتركين فقط</asp:ListItem>
                    </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2">
                    <label> </label>
                    <asp:Button ID="btn_search" CssClass="btn btn-outline-info btn-block mt-1" runat="server" Text="بحث" />
                </div>

            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12">
                    <div class="panel-scroll scrollable">
                        <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" waitmessagefont-names="Verdana" waitmessagefont-size="14pt" width="100%" style="text-align: center" sizetoreportcontent="True" showexportcontrols="True" showbackbutton="True" backcolor="" clientidmode="AutoID" highlightbackgroundcolor="" internalbordercolor="204, 204, 204" internalborderstyle="Solid" internalborderwidth="1px" linkactivecolor="" linkactivehovercolor="" linkdisabledcolor="" primarybuttonbackgroundcolor="" primarybuttonforegroundcolor="" primarybuttonhoverbackgroundcolor="" primarybuttonhoverforegroundcolor="" secondarybuttonbackgroundcolor="" secondarybuttonforegroundcolor="" secondarybuttonhoverbackgroundcolor="" secondarybuttonhoverforegroundcolor="" splitterbackcolor="" toolbardividercolor="" toolbarforegroundcolor="" toolbarforegrounddisabledcolor="" toolbarhoverbackgroundcolor="" toolbarhoverforegroundcolor="" toolbaritembordercolor="" toolbaritemborderstyle="Solid" toolbaritemborderwidth="1px" toolbaritemhoverbackcolor="" toolbaritempressedbordercolor="51, 102, 153" toolbaritempressedborderstyle="Solid" toolbaritempressedborderwidth="1px" toolbaritempressedhoverbackcolor="153, 187, 226">
                                </rsweb:reportviewer>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
