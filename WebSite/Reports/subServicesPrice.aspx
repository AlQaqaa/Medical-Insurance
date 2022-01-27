<%@ Page Title="طباعة أسعار الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="subServicesPrice.aspx.vb" Inherits="Medical_Insurance.subServicesPrice" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header bg-success text-light">طباعة أسعار الخدمات</div>
        <div class="card-body">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row justify-content-center">
                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_prices_profile">ملف الأسعار</label>
                            <asp:DropDownList ID="ddl_prices_profile" CssClass="form-control drop-down-list chosen-select" runat="server" DataSourceID="SqlDataSource3" DataTextField="profile_name" DataValueField="profile_id"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="select 3025 as profile_id, 'ملف أسعار عروض الشركات' as profile_name from INC_PRICES_PROFILES union select profile_id, profile_name from INC_PRICES_PROFILES where is_default = 0 and profile_sts = 0 and profile_id <> 3025"></asp:SqlDataSource>
                        </div>

                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_clincs">العيادة</label>
                            <asp:DropDownList ID="ddl_clinics" runat="server" AutoPostBack="True" CssClass="chosen-select drop-down-list form-control" DataSourceID="SqlDataSource1" DataTextField="Clinic_AR_Name" DataValueField="clinic_id"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT [clinic_id], [Clinic_AR_Name] FROM [Main_Clinic] where Clinic_State = 0 order by clinic_id "></asp:SqlDataSource>
                        </div>
                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_services">القسم</label>
                            <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-sm-12 col-md-3">
                            <label for="dll_doctors">الطبيب</label>
                            <asp:DropDownList ID="dll_doctors" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource4" DataTextField="MedicalStaff_AR_Name" DataValueField="MedicalStaff_ID">
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [MedicalStaff_ID], 'غير مصنف' AS [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] UNION SELECT [MedicalStaff_ID], [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] where MedicalStaff_State = 0"></asp:SqlDataSource>

                        </div>
                    </div>

                    <div class="row justify-content-center mt-3">
                        
                        <div class="col-sm-12 col-md-3">
                            <asp:Button ID="btn_print" runat="server" Text="عرض" CssClass="btn btn-success btn-block" />
                        </div>

                    </div>

                    <div class="row mt-2">
                        <div class="col">
                             <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" waitmessagefont-names="Verdana" waitmessagefont-size="14pt" width="100%" style="text-align: center" sizetoreportcontent="True" showexportcontrols="True" showbackbutton="True" backcolor="" clientidmode="AutoID" highlightbackgroundcolor="" internalbordercolor="204, 204, 204" internalborderstyle="Solid" internalborderwidth="1px" linkactivecolor="" linkactivehovercolor="" linkdisabledcolor="" primarybuttonbackgroundcolor="" primarybuttonforegroundcolor="" primarybuttonhoverbackgroundcolor="" primarybuttonhoverforegroundcolor="" secondarybuttonbackgroundcolor="" secondarybuttonforegroundcolor="" secondarybuttonhoverbackgroundcolor="" secondarybuttonhoverforegroundcolor="" splitterbackcolor="" toolbardividercolor="" toolbarforegroundcolor="" toolbarforegrounddisabledcolor="" toolbarhoverbackgroundcolor="" toolbarhoverforegroundcolor="" toolbaritembordercolor="" toolbaritemborderstyle="Solid" toolbaritemborderwidth="1px" toolbaritemhoverbackcolor="" toolbaritempressedbordercolor="51, 102, 153" toolbaritempressedborderstyle="Solid" toolbaritempressedborderwidth="1px" toolbaritempressedhoverbackcolor="153, 187, 226">
                                </rsweb:reportviewer>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }
    </script>
</asp:Content>
