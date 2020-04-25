<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companyStatistics.aspx.vb" Inherits="Medical_Insurance.companyStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card mt-1">
        <div class="card-header bg-success text-light">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </div>
        <div class="card-body">
            <div class="form-row mb-3">
                <div class="form-group col-xs-12 col-sm-6">
                    <asp:Panel ID="main_company_panel" runat="server" Visible="False">
                        <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id" AutoPostBack="True"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    
                </div>
            </div>
        </div>
    </div>

</asp:Content>
