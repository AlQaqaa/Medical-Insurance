<%@ Page Title="أرصدة ومصروفات المنتفعين" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="patientsExpenses.aspx.vb" Inherits="Medical_Insurance.patientsExpenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header">
            أرصدة ومصروفات المنتفعين
        </div>
        <div class="card-body">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="form-group col-sm-12 col-md-4">
                            <asp:DropDownList ID="ddl_companies" runat="server" CssClass="form-control drop-down-list chosen-select" DataSourceID="SqlDataSource1" DataTextField="C_Name_Arb" DataValueField="C_ID" AutoPostBack="True"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [C_ID], N'- يرجى اختيار شركة -' AS [C_Name_Arb] FROM [INC_COMPANY_DATA] UNION SELECT [C_ID], [C_Name_Arb] FROM [INC_COMPANY_DATA] WHERE C_State = 0"></asp:SqlDataSource>
                        </div>

                        <div class="form-group col-sm-12 col-md-2">
                            <asp:Button ID="btn_print" CssClass="btn btn-outline-secondary btn-block" runat="server" Text="طباعة" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" Enabled="False" />
                        </div>
                        <div class="form-group col-sm-12 col-md-2">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="2">
                                <ProgressTemplate>
                                    <div class="spinner-border text-info mt-2" role="status">
                                        <span class="sr-only">Loading...</span>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                    <!-- /row -->

                    <div class="row">
                        <div class="col-sm-12">
                            <div class="panel-scroll scrollable">

                                <asp:GridView ID="GridView1" CssClass="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                    <Columns>
                                        <asp:BoundField HeaderText="كود المنتفع" DataField="INC_Patient_Code"></asp:BoundField>
                                        <asp:BoundField DataField="CARD_NO" HeaderText="رقم البطاقة"></asp:BoundField>
                                        <asp:BoundField DataField="NAME_ARB" HeaderText="اسم المنتفع"></asp:BoundField>
                                        <asp:BoundField DataField="TOTAL_WITHDRAW" HeaderText="إجمالي المصروفات"></asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btn_print_one" runat="server"
                                                    CommandName="printProcess"
                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    Text="التفاصيل"
                                                    ControlStyle-CssClass="btn btn-primary btn-small" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_print" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
