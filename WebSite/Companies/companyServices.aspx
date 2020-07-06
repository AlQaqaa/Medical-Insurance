<%@ Page Title="الأقسام" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companyServices.aspx.vb" Inherits="Medical_Insurance.companyServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="row">
        <div class="col-sm-12">
            <div class="card mt-1">
                <div class="card-header bg-info text-white">العيادات والخدمات</div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-4">
                            <label for="ddl_clinics">العيادة</label>
                            <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                        </div>
                        <div class="form-group col-xs-12 col-sm-4">
                            <label for="txt_clinics_max">سقف العيادة</label>
                            <asp:TextBox ID="txt_clinics_max" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                        </div>

                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <%--<div class="form-row">
                                <div class="form-group col-xs-12">
                                    <h6>العيادات المغطاة</h6>
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <hr />--%>
                            <div class="form-row">

                                <div class="form-group col-xs-6 col-sm-9">
                                    <asp:Panel ID="Panel2" runat="server">
                                        <div class="alert alert-warning" role="alert">
                                            عند اختيار جميع العيادات سيتم تغطية كافة الأقسام للعيادات المغطاة بأسقف مفتوحة
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="form-group col-xs-6 col-sm-3 align-content-end">
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                                </div>
                            </div>
                            <hr />
                            <asp:Panel ID="Panel1" runat="server">
                                <div class="form-row">
                                    <div class="form-group col-xs-12 col-sm-1">
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" />
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-3">
                                        <asp:TextBox ID="txt_max_value_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEU(event)" placeholder="سقف الخدمة"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-1">

                                        <asp:Button ID="btn_apply" CssClass="btn btn-success btn-block" runat="server" Text="تطبيق" />
                                    </div>
                                </div>
                                <!-- row -->
                            </asp:Panel>

                            <asp:GridView ID="GridView1" runat="server" class="table table-striped table-bordered" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="Service_ID" HeaderText="رقم الخدمة">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="مغطاة؟">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox2" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="اسم الخدمة" DataField="Service_AR_Name"></asp:BoundField>
                                    <asp:TemplateField HeaderText="السقف">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_max_val" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_save" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
