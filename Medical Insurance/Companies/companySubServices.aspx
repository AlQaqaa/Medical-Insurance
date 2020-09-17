<%@ Page Title="الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companySubServices.aspx.vb" Inherits="Medical_Insurance.companySubServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .panel {
            height: 530px;
            max-height: 530px;
        }

        .scrollable {
            overflow-y: auto;
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
                        <div class="card-header bg-info text-white">العيادات والخدمات</div>
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-xs-12 col-sm-4">
                                    <label for="ddl_clinics">طريقة العرض</label>
                                    <asp:DropDownList ID="ddl_show_type" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True">
                                        <asp:ListItem Value="1">العيادات</asp:ListItem>
                                        <asp:ListItem Value="2">الخدمات المجمعة</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <!-- form-row -->
                            <asp:Panel ID="clinic_Panel" runat="server">
                                <div class="form-row">
                                    <div class="form-group col-xs-12 col-sm-4">
                                        <label for="ddl_clinics">العيادة</label>
                                        <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-4">
                                        <label for="txt_clinics_max">القسم</label>
                                        <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-4">
                                        <label for="txt_clinics_max">سقف العيادة</label>
                                        <asp:TextBox ID="txt_clinics_max" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="form-group col-xs-6 col-sm-12">
                                        <asp:Panel ID="Panel2" runat="server">
                                            <div class="alert alert-warning" role="alert">
                                                عند اختيار جميع العيادات سيتم تغطية كافة الأقسام للعيادات المغطاة بأسقف مفتوحة
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="groups_Panel" runat="server" Visible="False">
                                <div class="form-row">
                                    <div class="form-group col-xs-12 col-sm-4">
                                        <label for="ddl_clinics">المجموعة</label>
                                        <asp:DropDownList ID="ddl_gourp" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="GROUP_ARNAME" DataValueField="GROUP_ID"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS GROUP_ID, 'الكل' AS GROUP_ARNAME FROM Main_GroupSubService UNION SELECT GROUP_ID, GROUP_ARNAME FROM [Main_GroupSubService] WHERE GROUP_STATE = 0"></asp:SqlDataSource>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-4">
                                        <label for="txt_clinics_max">الخدمات</label>
                                        <asp:DropDownList ID="ddl_services_group" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                </div>
                            </asp:Panel>
                            <%--<div class="form-row">
                        <div class="form-group col-xs-12">
                            <h6>العيادات المغطاة</h6>
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <hr />--%>


                            <asp:Panel ID="Panel1" runat="server">
                                <div class="form-row">
                                    <div class="form-group col-xs-12 col-sm-1">
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Checked="True" />
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-2">
                                        <asp:TextBox ID="txt_person_per_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEU(event)" placeholder="نسبة الفرد"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-2">
                                        <asp:TextBox ID="txt_family_per_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEU(event)" placeholder="نسبة العائلة"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-2">
                                        <asp:TextBox ID="txt_parent_per_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEU(event)" placeholder="نسبة الوالدين"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-2">
                                        <asp:TextBox ID="txt_person_max_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEU(event)" placeholder="سقف الفرد"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-2">
                                        <asp:TextBox ID="txt_family_max_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEU(event)" placeholder="سقف العائلة"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-1">

                                        <asp:Button ID="btn_apply" CssClass="btn btn-success btn-block" runat="server" Text="تطبيق" />
                                    </div>
                                </div>
                                <!-- row -->
                            </asp:Panel>
                            <hr />
                            <div class="form-row justify-content-end">
                                <div class="col-sm-2">
                                    <div class="form-group">
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="2">
                                            <ProgressTemplate>
                                                <asp:Image ID="Image1" runat="server" Width="50px" ImageUrl="~/Style/images/loading.gif" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                </div>
                                <div class="form-group col-xs-6 col-sm-3">
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" OnClientClick="return confirm('هل أنت متأكد من هذا الإجراء؟')" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="panel scrollable">
                                        <asp:GridView ID="GridView1" runat="server" class="table table-striped table-bordered" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                            <Columns>
                                                <asp:BoundField DataField="SubService_ID" HeaderText="رقم الخدمة">
                                                    <ControlStyle CssClass="hide-colum" />
                                                    <FooterStyle CssClass="hide-colum" />
                                                    <HeaderStyle CssClass="hide-colum" />
                                                    <ItemStyle CssClass="hide-colum" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="مغطاة؟">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="موافقة؟">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox3" runat="server" Checked="False" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="رمز الخدمة" DataField="SubService_Code"></asp:BoundField>
                                                <asp:BoundField HeaderText="اسم الخدمة" DataField="SubService_AR_Name"></asp:BoundField>
                                                <asp:BoundField HeaderText="اسم العيادة" DataField="CLINIC_NAME"></asp:BoundField>
                                                <asp:TemplateField HeaderText="نسبة الفرد">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_person_per" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="نسبة العائلة">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_family_per" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="نسبة الوالدين">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_parent_per" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="سقف الفرد">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_person_max" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="سقف العائلة">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_family_max" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
