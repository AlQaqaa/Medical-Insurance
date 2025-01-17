﻿<%@ Page Title="إضافة شركة جديدة" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="addNewCompany.aspx.vb" Inherits="Medical_Insurance.addNewCompany" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .UserPic {
            border-radius: 50%;
            border-style: solid;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="card mt-1">
        <div class="card-header bg-info text-white">بيانات الشركة</div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="ddl_company_level">مستوى الشركة</label>
                    <asp:DropDownList ID="ddl_company_level" CssClass="drop-down-list form-control" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="1">مستوى أول</asp:ListItem>
                        <asp:ListItem Value="2">مستوى ثاني</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-xs-12 col-sm-6">
                    <asp:Panel ID="main_company_panel" runat="server" Visible="False">
                        <label for="ddl_companies">الشركة الأم</label>
                        <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT * FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                    </asp:Panel>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-6">
                            <label for="txt_company_name_ar">اسم الشركة بالعربية</label>
                            <asp:TextBox ID="txt_company_name_ar" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyAR(event)" AutoPostBack="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال اسم الشركة" ControlToValidate="txt_company_name_ar" ForeColor="Red" ValidationGroup="save_data"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-xs-12 col-sm-6">
                            <label for="txt_company_name_en">اسم الشركة بالإنجليزية</label>
                            <asp:TextBox ID="txt_company_name_en" CssClass="form-control" dir="ltr" runat="server" onkeypress="return isAlphabetKeyEN(event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب إدخال اسم الشركة" ControlToValidate="txt_company_name_en" ForeColor="Red" ValidationGroup="save_data"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_start_dt">تاريخ بداية التعاقد</label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_start_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_start_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                    </div>
                                </div>
                            </div>
                            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_start_dt" ID="CalendarExtender3" Format="yyyy/MM/dd" PopupButtonID="ImageButton1" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                            <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_start_dt_MaskedEditExtender" TargetControlID="txt_start_dt" ID="MaskedEditExtender3" Mask="9999/99/99" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_start_dt">تاريخ نهاية التعاقد</label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                    </div>
                                </div>
                            </div>
                            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_end_dt" ID="CalendarExtender1" Format="yyyy/MM/dd" PopupButtonID="ImageButton2" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                            <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="ttxt_end_dt_MaskedEditExtender" TargetControlID="txt_end_dt" ID="MaskedEditExtender1" Mask="9999/99/99" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="ddl_payment_type">طريقة الدفع</label>
                            <asp:DropDownList ID="ddl_payment_type" CssClass="drop-down-list form-control" runat="server">
                                <asp:ListItem Value="1">خاص</asp:ListItem>
                                <asp:ListItem Value="2">تأمين</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="ddl_payment_type">طريقة دفع نسبة المريض</label>
                            <asp:DropDownList ID="ddl_PATIAINT_PER" CssClass="drop-down-list form-control" runat="server">
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
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="ddl_profiles_prices">ملف الأسعار</label>
                            <asp:DropDownList ID="ddl_profiles_prices" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource2" DataTextField="profile_name" DataValueField="profile_Id"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT [profile_Id], [profile_name] FROM [INC_PRICES_PROFILES] WHERE PROFILE_STS = 0"></asp:SqlDataSource>
                        </div>
                    </div>
                    <!-- /row -->
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_max_company_value">سقف المعاينة الواحدة</label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_max_one_processes" CssClass="form-control" Text="0" runat="server" onkeypress="return isAlphabetKeyEU(event)"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">د.ل</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /row -->
                    <hr />
                    <div class="form-row justify-content-end">
                        <div class="form-group col-xs-6 col-sm-3">
                            <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>
                </ContentTemplate>
                
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
