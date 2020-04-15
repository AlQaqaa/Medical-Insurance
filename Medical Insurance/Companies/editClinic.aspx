<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="editClinic.aspx.vb" Inherits="Medical_Insurance.editClinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12">
                    <div class="card mt-1">
                        <div class="card-header bg-info text-white">العيادات</div>
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-xs-12 col-sm-1">
                                    <label>ر.ع</label>
                                    <asp:TextBox ID="txt_clinic_id" CssClass="form-control" runat="server" AutoCompleteType="Disabled" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-sm-4">
                                    <label>اسم العيادة</label>
                                    <asp:TextBox ID="txt_clini_name" CssClass="form-control" runat="server" AutoCompleteType="Disabled" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-sm-4">
                                    <label>السقف العام</label>
                                    <asp:TextBox ID="txt_max_val" CssClass="form-control" runat="server" AutoCompleteType="Disabled" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال السقف" ControlToValidate="txt_max_val" ValidationGroup="save_data" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-xs-12 col-sm-2">
                                    <label></label>
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block mt-2" Text="حفظ" ValidationGroup="save_data" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-xs-12 col-sm-8">
                                    <asp:Label ID="lbl_info" runat="server" Text="" ForeColor="#CC0000" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="form-group col-xs-12 col-sm-4">
                                    <asp:Button ID="btn_separat" runat="server" Text="فصل العيادة" CssClass="btn btn-outline-primary btn-block" Visible="False" ValidationGroup="save_data" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-xs-12">
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
