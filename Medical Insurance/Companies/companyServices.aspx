<%@ Page Title="الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companyServices.aspx.vb" Inherits="Medical_Insurance.companyServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <h1 class="display-4 d-none d-sm-block">
        <asp:Label ID="lbl_com_name" runat="server" Text=""></asp:Label></h1>
    <p class="lead d-none d-sm-block">
        <asp:Label ID="lbl_en_name" runat="server" Text=""></asp:Label>
    </p>

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
                    <hr />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
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
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="رمز الخدمة" DataField="SubService_Code"></asp:BoundField>
                                    <asp:BoundField HeaderText="اسم الخدمة" DataField="SubService_AR_Name"></asp:BoundField>
                                    <asp:TemplateField HeaderText="نسبة الفرد">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_person_per" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="نسبة العائلة">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_family_per" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="نسبة الوالدين">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_parent_per" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="سقف الفرد">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_person_max" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="سقف العائلة">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_family_max" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="9pt" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <hr />
                            <div class="form-row justify-content-end">
                                <div class="form-group col-xs-6 col-sm-3">
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
