<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="servicesPricesShow.aspx.vb" Inherits="Medical_Insurance.servicesPricesShow" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header bg-success text-light">طباعة ملف أسعار الخدمات</div>
        <div class="card-body">
            <div class="form-row justify-content-center">
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="ddl_clinics">طريقة العرض</label>
                    <asp:DropDownList ID="ddl_show_type" CssClass="form-control" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="1">العيادات</asp:ListItem>
                        <asp:ListItem Value="2">الخدمات المجمعة</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="ddl_clinics">السعر</label>
                    <asp:DropDownList ID="ddl_price" CssClass="form-control" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="1">سعر التأمين</asp:ListItem>
                        <asp:ListItem Value="2">سعر النقدي</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <!-- form-row -->
            
<%--            <asp:Panel ID="groups_Panel" runat="server" Visible="False">
                <div class="form-row justify-content-center mb-2">
                    <div class="form-group col-xs-12 col-sm-4">
                        <label for="ddl_clinics">المجموعة</label>
                        <asp:DropDownList ID="ddl_gourp" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="GROUP_ARNAME" DataValueField="GROUP_ID"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [Group_ID], 'يرجى اختيار مجموعة' AS [Group_ARname] FROM Main_GroupSubService UNION SELECT [Group_ID], [Group_ARname] FROM [Main_GroupSubService] WHERE [Group_State] = 0"></asp:SqlDataSource>
                    </div>
                    <div class="form-group col-xs-12 col-sm-4">
                        <label for="txt_clinics_max">الخدمات</label>
                        <asp:DropDownList ID="ddl_services_group" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </div>
                </div>
            </asp:Panel>--%>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    <div class="row justify-content-center mt-3">
                        <div class="col-sm-3">
                            <asp:Button ID="btn_show" runat="server" Text="عرض" CssClass="btn btn-info btn-block" />
                        </div>
                        <div class="col-sm-3">
                            <asp:Button ID="btn_print" runat="server" Text="طباعة" CssClass="btn btn-success btn-block" Enabled="False" />
                        </div>
                    </div>

                    <div class="row mt-2">
                        <div class="col-sm-12">
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="SER_ID" HeaderText="رقم الخدمة">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SubService_Code" HeaderText="كود الخدمة" SortExpression="SubService_Code" />
                                    <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم الخدمة بالعربي" SortExpression="SubService_AR_Name" />
                                    <asp:BoundField DataField="SubService_EN_Name" HeaderText="اسم الخدمة بالانجليزي" SortExpression="SubService_EN_Name" />
                                    <asp:BoundField DataField="SERVICE_PRICE" HeaderText="سعر الخدمة" SortExpression="SERVICE_PRICE"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" Visible="False" />
        </div>
    </div>


</asp:Content>
