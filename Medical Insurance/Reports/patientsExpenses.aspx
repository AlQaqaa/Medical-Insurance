<%@ Page Title="أرصدة ومصروفات المنتفعين" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="patientsExpenses.aspx.vb" Inherits="Medical_Insurance.patientsExpenses" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


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
                        <div class="col-md-12 col-lg-4 col-xl-3">
                            <label for="txt_start_dt">الشركة</label>
                            <asp:DropDownList ID="ddl_companies" runat="server" CssClass="form-control drop-down-list chosen-select" DataSourceID="SqlDataSource1" DataTextField="C_Name_Arb" DataValueField="C_ID"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [C_ID], N'- يرجى اختيار شركة -' AS [C_Name_Arb] FROM [INC_COMPANY_DATA] UNION SELECT [C_ID], [C_Name_Arb] FROM [INC_COMPANY_DATA] WHERE C_State = 0"></asp:SqlDataSource>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب اختيار شركة" ControlToValidate="ddl_companies" InitialValue="0" ValidationGroup="search"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-12 col-lg-3 col-xl-2">
                            <label for="txt_start_dt">قيمة المصروف</label>
                            <asp:TextBox ID="txt_value" runat="server" CssClass="form-control" placeholder="قيمة المصروفات" onblur="appendDollar(this.id);" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                        </div>
                        <div class="col-md-12 col-lg-3 col-xl-2">
                            <label for="txt_start_dt">الفترة من</label>
                            <asp:TextBox ID="txt_start_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب إدخال التاريخ" ControlToValidate="txt_start_dt" ForeColor="Red" ValidationGroup="search"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-12 col-lg-3 col-xl-2">
                            <label for="txt_start_dt">إلى</label>
                           <asp:TextBox ID="txt_end_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="يجب إدخال التاريخ" ControlToValidate="txt_end_dt" ForeColor="Red" ValidationGroup="search"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-12 col-lg-2 col-xl-1">
                            <asp:Button ID="btn_search" CssClass="btn btn-outline-info btn-block mt-4" runat="server" Text="بحث" ValidationGroup="search"/>
                        </div>
                        <div class="col-md-12 col-lg-2 col-xl-1">
                            <asp:Button ID="btn_print" CssClass="btn btn-outline-secondary btn-block mt-4" runat="server" Text="طباعة" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" Enabled="False" />
                        </div>
                        <div class="form-group col-sm-12 col-md-1">
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
                        <div class="col-sm-12 col-md-6">
                            <asp:Label ID="Label1" runat="server" Text="" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <!-- /row -->
                    <div class="row mt-3">
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

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
            $('.datepicker1').datepicker({
                format: "dd/mm/yyyy",
                todayBtn: "linked",
                language: "ar",
                autoclose: true,
                todayHighlight: true
            });
        });
    </script>
</asp:Content>
