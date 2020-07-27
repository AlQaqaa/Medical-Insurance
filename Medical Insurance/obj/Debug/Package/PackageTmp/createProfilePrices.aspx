<%@ Page Title="إنشاء ملف أسعار" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="createProfilePrices.aspx.vb" Inherits="Medical_Insurance.createProfilePrices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_cb_is_default {
            margin-left: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12">
                    <div class="card mt-1">
                        <div class="card-header bg-primary text-light">إنشاء ملف أسعار جديد</div>
                        <div class="card-body">
                            <div class="row justify-content-center">
                                <div class="col-sm-12 col-md-6">
                                    <div class="form-group row">
                                        <label for="inputName" class="col-sm-3 col-form-label">اسم الملف</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txt_profile_name" CssClass="form-control" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال اسم الملف" ValidationGroup="next_btn" ControlToValidate="txt_profile_name" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- row -->
                            <%--<div class="row justify-content-center mb-5">
                                <div class="col-sm-12 col-md-6">
                                    <div class="form-check">
                                        <asp:CheckBox ID="cb_is_default" CssClass="form-check-input" runat="server" Text=" إعتماد ملف الأسعار هذا كملف أساسي؟" />
                                    </div>

                                </div>
                            </div>--%>
                            <!-- row -->
                            <hr />
                            <div class="row justify-content-end">
                                <div class="col-sm-12 col-md-3">
                                    <asp:Button ID="btn_next" CssClass="btn btn-outline-success btn-block" runat="server" Text="التالي" ValidationGroup="next_btn" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                                </div>
                            </div>
                            <!-- row -->
                            <hr />
                        </div>
                    </div>
                </div>
            </div>
            <!-- /row -->
            <div class="row mt-1">
                <div class="col-sm-12 col-md-6">
                    <div class="card">
                        <div class="card-header bg-success text-light">ملفات الأسعار المتاحة <span data-toggle='tooltip' data-placement='top' title='لا يمكن تعطيل ملف أسعار مرتبط بشركة ما أو كان الملف هو ملف الأسعار الأساسي'><i class='fas fa-info-circle'></i></span></div>
                        <div class="card-body">

                            <asp:GridView ID="GridView1" class="table table-striped table-sm" runat="server" GridLines="None" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="PROFILE_ID" HeaderText="ر.م">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="is_default" HeaderText="ر.خ">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:ButtonField DataTextField="PROFILE_NAME" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم الملف لمشاهدة الأسعار وطباعتها'> اسم الملف <i class='fas fa-info-circle'></i></span>" CommandName="show_profile"></asp:ButtonField>
                                    <asp:BoundField DataField="PROFILE_DT" HeaderText="تاريخ إنشاء الملف"></asp:BoundField>
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_set_default" runat="server"
                                                CommandName="set_default"
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                ToolTip="تحديد ملف الأسعار كملف أساسي"
                                                ControlStyle-CssClass="btn btn-secondary btn-sm btn-small">أساسي</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_edit_price" runat="server"
                                                CommandName="edit_price"
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                ToolTip="تعديل قائمة الأسعار"
                                                ControlStyle-CssClass="btn btn-primary btn-sm btn-small">تعديل</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_stop_profile" runat="server"
                                                CommandName="stop_profile"
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                ToolTip="تعطيل"
                                                ControlStyle-CssClass="btn btn-danger btn-sm btn-small"
                                                OnClientClick="return confirm('هل أنت متأكد من تعطيل ملف الأسعار هذا؟')">تعطيل</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12 col-md-6">
                    <div class="card">
                        <div class="card-header bg-danger text-light">ملفات الأسعار الموقوفة</div>
                        <div class="card-body">

                            <asp:GridView ID="GridView2" class="table table-striped table-sm" runat="server" GridLines="None" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="PROFILE_ID" HeaderText="ر.خ">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROFILE_NAME" HeaderText="اسم الملف"></asp:BoundField>
                                    <asp:BoundField DataField="PROFILE_DT" HeaderText="تاريخ إنشاء الملف"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_block_stop" runat="server"
                                                CommandName="stop_block"
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                ToolTip="إيقاف الحظر"
                                                ControlStyle-CssClass="btn btn-success btn-sm btn-small"
                                                OnClientClick="return confirm('هل أنت متأكد من تفعيل هذا الملف؟')">تفعيل</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <!-- row -->
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_next" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
