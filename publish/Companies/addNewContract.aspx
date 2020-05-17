<%@ Page Title="تمديد/تجديد عقد" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="addNewContract.aspx.vb" Inherits="Medical_Insurance.addNewContract" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="card mt-1">
        <div class="card-header bg-info text-white">تمديد / تجديد عقد شركة</div>
        <div class="card-body">
            <div class="form-row">

                <div class="form-group col-xs-12 col-sm-6">
                    <asp:Panel ID="main_company_panel" runat="server" Visible="False">
                        <label for="ddl_companies">الشركة الأم</label>
                    </asp:Panel>
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
                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_start_dt" ID="CalendarExtender3" Format="dd/MM/yyyy" PopupButtonID="ImageButton1" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_start_dt_MaskedEditExtender" TargetControlID="txt_start_dt" ID="MaskedEditExtender3" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="txt_end_dt">تاريخ نهاية التعاقد</label>
                    <div class="input-group">
                        <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                        <div class="input-group-prepend">
                            <div class="input-group-text">
                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                            </div>
                        </div>
                    </div>
                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_end_dt" ID="CalendarExtender1" Format="dd/MM/yyyy" PopupButtonID="ImageButton2" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_end_dt_MaskedEditExtender" TargetControlID="txt_end_dt" ID="MaskedEditExtender1" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="ddl_payment_type">طريقة الدفع</label>
                    <asp:DropDownList ID="ddl_payment_type" CssClass="chosen-select drop-down-list form-control" runat="server">
                        <asp:ListItem Value="1">خاص</asp:ListItem>
                        <asp:ListItem Value="2">تأمين</asp:ListItem>
                        <asp:ListItem Value="3">فوترة</asp:ListItem>

                    </asp:DropDownList>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="ddl_payment_type">طريقة التعقد</label>
                    <asp:DropDownList ID="ddl_contractType" CssClass="form-control " runat="server">
                        <asp:ListItem Value="2">تمديد</asp:ListItem>
                        <asp:ListItem Value="3">تجديد</asp:ListItem>
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
                    <label for="ddl_payment_type">طريقة دفع نسبة المريض</label>
                    <asp:DropDownList ID="ddl_PATIAINT_PER" CssClass="chosen-select drop-down-list form-control" runat="server">
                        <asp:ListItem Value="1">خاص</asp:ListItem>
                        <asp:ListItem Value="0">تأمين</asp:ListItem>
                        <asp:ListItem Value="3">فوترة</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="ddl_profiles_prices">ملف الأسعار</label>
                    <asp:DropDownList ID="ddl_profiles_prices" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource2" DataTextField="profile_name" DataValueField="profile_Id"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT [profile_Id], [profile_name] FROM [INC_PRICES_PROFILES] WHERE PROFILE_STS = 0"></asp:SqlDataSource>
                </div>
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
            <hr />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row justify-content-end">
                        <div class="form-group col-xs-6 col-sm-3">
                            <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" Style="margin-bottom: 0" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
