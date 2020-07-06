<%@ Page Title="تفاصيل طلب الموافقة" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="approvalDetails.aspx.vb" Inherits="Medical_Insurance.approvalDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header bg-info text-light">
            تفاصيل طلب الموافقة
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-2">
                    <label for="txt_confirm_no">رقم الطلب</label>
                    <asp:TextBox ID="txt_confirm_no" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-2">
                    <label for="txt_end_dt">تاريخ إنتهاء الطلب</label>
                    <asp:TextBox ID="txt_end_dt" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="txt_emp_name">اسم المشترك</label>
                    <asp:TextBox ID="txt_emp_name" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="txt_patient_name">اسم المريض</label>
                    <asp:TextBox ID="txt_patient_name" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                
            </div><!-- row -->
            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-2">
                    <label for="txt_relation">الصلة بالمشترك</label>
                    <asp:TextBox ID="txt_relation" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="txt_company_name">اسم الشركة</label>
                    <asp:TextBox ID="txt_company_name" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="txt_card_no">رقم البطاقة</label>
                    <asp:TextBox ID="txt_card_no" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="txt_emp_no">الرقم الوظيفي</label>
                    <asp:TextBox ID="txt_emp_no" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
            </div><!-- row -->

            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="txt_total_val">الإجمالي</label>
                    <asp:TextBox ID="txt_total_val" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="txt_approv_val">القيمة الموافق عليها</label>
                    <asp:TextBox ID="txt_approv_val" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="txt_pending_val">القيمة المتبقية</label>
                    <asp:TextBox ID="txt_pending_val" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="txt_requst">الجهة الطالبة</label>
                    <asp:TextBox ID="txt_requst" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                </div>
            </div><!-- row -->

            <div class="row">
                <div class="col-xs-12 col-sm-12">
                    <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None" HeaderStyle-CssClass="thead-dark">
                        <Columns>
                            <asp:TemplateField HeaderText="ر.ت">
                                <ItemTemplate>
                                    <span>
                                        <%#Container.DataItemIndex + 1%>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SubService_Code" HeaderText="كود العملية"></asp:BoundField>
                            <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم العملية"></asp:BoundField>
                            <asp:BoundField DataField="SERVICE_PRICE" HeaderText="سعر العملية"></asp:BoundField>
                            <asp:BoundField DataField="REQUEST_TYPE" HeaderText="النوع"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btn_delete" runat="server"
                                        CommandName="delete_ser"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        ToolTip="إزالة"
                                        ControlStyle-CssClass="btn btn-danger btn-small"
                                        OnClientClick="return confirm('هل أنت متأكد من إزالة هذه الخدمة')">إزالة</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!-- row -->

            <div class="form-row justify-content-end">
                    <div class="col-xs-12 col-sm-3">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btn_print" runat="server" CssClass="btn btn-outline-secondary btn-block mt-sm-4 mt-md-4" Text="طباعة" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_print" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
