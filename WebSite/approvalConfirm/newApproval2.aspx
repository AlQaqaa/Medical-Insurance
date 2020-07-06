<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="newApproval2.aspx.vb" Inherits="Medical_Insurance.newApproval2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card mt-1">
                <div class="card-header bg-success text-light">
                    طلب موافقة جديد
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-4">
                            <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue="0" runat="server" ErrorMessage="* مطلوب" ControlToValidate="ddl_companies" ValidationGroup="chose" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-xs-12 col-sm-8">
                            <asp:TextBox ID="txt_name" CssClass="form-control" placeholder="اسم المنتفع" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_name" ValidationGroup="print" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <!-- form-row -->
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-4">
                            <asp:TextBox ID="txt_service_name" CssClass="form-control" placeholder="اسم العملية" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_service_name" ValidationGroup="print" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-xs-12 col-sm-4">
                            <asp:TextBox ID="txt_value" CssClass="form-control" placeholder="قيمة العملية" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_value" ValidationGroup="print" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <!-- form-row -->
                        <div class="form-row justify-content-end">
                            <div class="col-xs-12 col-sm-3">
                                <asp:Button ID="btn_print" runat="server" CssClass="btn btn-outline-secondary btn-block mt-sm-4 mt-md-4" Text="طباعة" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" ValidationGroup="print" />
                            </div>
                        </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_print" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
