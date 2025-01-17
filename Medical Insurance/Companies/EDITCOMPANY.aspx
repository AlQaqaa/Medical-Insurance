﻿<%@ Page Title="تعديل بيانات شركة" Language="vb" AutoEventWireup="false" MasterPageFile="companies.Master" CodeBehind="EDITCOMPANY.aspx.vb" Inherits="Medical_Insurance.EDITCOMPANY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card mt-1">
                <div class="card-header bg-info text-white">تعديل بيانات شركة</div>
                <div class="card-body">
                    <div class="form-row">

                        <div class="form-group col-xs-12 col-sm-6">
                            <asp:Panel ID="main_company_panel" runat="server" Visible="False">
                                <label for="ddl_companies">الشركة الأم</label>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-6">
                            <label for="txt_company_name_ar">اسم الشركة بالعربية</label>
                            <asp:TextBox ID="txt_company_name_ar" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyAR(event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال اسم الشركة" ControlToValidate="txt_company_name_ar" ForeColor="Red" ValidationGroup="save_data"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-xs-12 col-sm-6">
                            <label for="txt_company_name_en">اسم الشركة بالإنجليزية</label>
                            <asp:TextBox ID="txt_company_name_en" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEN(event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب إدخال اسم الشركة" ControlToValidate="txt_company_name_en" ForeColor="Red" ValidationGroup="save_data"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_start_dt">تاريخ بداية التعاقد</label>
                            <asp:TextBox ID="txt_start_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_end_dt">تاريخ نهاية التعاقد</label>
                            <asp:TextBox ID="txt_end_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="ddl_payment_type">طريقة الدفع</label>
                            <asp:DropDownList ID="ddl_payment_type" CssClass="chosen-select drop-down-list form-control" runat="server">
                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                                <asp:ListItem Value="1">خاص</asp:ListItem>
                                <asp:ListItem Value="2">تأمين</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                       <div class="form-group col-xs-12 col-sm-3">
                            <label for="ddl_payment_type">طريقة دفع نسبة المريض</label>
                            <asp:DropDownList ID="ddl_PATIAINT_PER" CssClass="chosen-select drop-down-list form-control" runat="server">
                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                                <asp:ListItem Value="1">خاص</asp:ListItem>
                                <asp:ListItem Value="2">تأمين</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_max_company_value">السقف العام للشركة</label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_max_company_value" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">د.ل</div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_max_card_value">السقف العام للعائلة</label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_max_card_value" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">د.ل</div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_max_person">السقف العام للفرد</label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_max_person" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">د.ل</div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_max_person">السقف العام للمشترك</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtEmpMax" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">د.ل</div>
                                </div>
                            </div>
                        </div>
                        
                    </div>
                    <hr />
                    <div class="form-row justify-content-end">
                        <div class="form-group col-xs-6 col-sm-3">
                            <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="تعديل" ValidationGroup="save_data" Style="margin-bottom: 0" />
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

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
