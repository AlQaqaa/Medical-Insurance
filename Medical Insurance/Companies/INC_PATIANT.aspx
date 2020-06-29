<%@ Page Title="إضافة منتفع" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="INC_PATIANT.aspx.vb" Inherits="Medical_Insurance.INC_PATIANT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="card mt-1">
        <div class="card-header bg-info text-white">بيانات المنتفع</div>
        <div class="card-body">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-4">
                            <label for="txt_CARD_NO">رقم البطاقة</label>
                            <asp:TextBox ID="txt_CARD_NO" CssClass="form-control" runat="server" AutoPostBack="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_CARD_NO" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-sm-4">
                            <label for="BAGE_NO">الرقم الوظيفي</label>
                            <asp:TextBox ID="txt_BAGE_NO" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_BAGE_NO" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-sm-4">
                            <asp:Literal ID="LtlTableBody" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <!-- row -->

                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="form-row">
                <div class="form-group col-sm-4">
                    <label for="txt_NAME_ARB">اسم المشترك بالعربي</label>
                    <asp:TextBox ID="txt_NAME_ARB" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyAR(event)"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_NAME_ARB" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-sm-4">
                    <label for="txt_NAME_ENG">اسم المشترك بالإنجليزية</label>
                    <asp:TextBox ID="txt_NAME_ENG" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEN(event)"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_NAME_ENG" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>

                </div>

                <div class="form-group col-xs-12 col-sm-4">
                    <label for="txt_start_dt">تاريخ الميلاد</label>
                    <div class="input-group">
                        <asp:TextBox ID="txt_BIRTHDATE" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_BIRTHDATE);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>

                        <div class="input-group-prepend">
                            <div class="input-group-text">
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                            </div>
                        </div>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_BIRTHDATE" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_BIRTHDATE" ID="CalendarExtender3" Format="dd/MM/yyyy" PopupButtonID="ImageButton1" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_BIRTHDATE_MaskedEditExtender" TargetControlID="txt_BIRTHDATE" ID="MaskedEditExtender3" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                </div>

            </div>
            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="NAL_ID">الجنسية</label>
                    <asp:DropDownList ID="ddl_NAL_ID" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="Nationality_AR_Name" DataValueField="Nationality_ID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT [Nationality_ID], [Nationality_AR_Name] FROM [Main_Nationality] WHERE Nationality_State=0"></asp:SqlDataSource>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="GENDER">الجنس</label>
                    <asp:DropDownList ID="ddl_GENDER" CssClass="form-control" runat="server">
                        <asp:ListItem Value="1" Text="ذكر">ذكر</asp:ListItem>
                        <asp:ListItem Value="2" Text="أنثى">أنثى</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="ddl_CONST_ID">صلة القربة</label>
                    <asp:DropDownList ID="CONST_ID" CssClass="chosen-select drop-down-list form-control" runat="server">
                        <asp:ListItem Value="0">المشترك</asp:ListItem>
                        <asp:ListItem Value="1">الأب</asp:ListItem>
                        <asp:ListItem Value="2">الأم</asp:ListItem>
                        <asp:ListItem Value="3">الزوجة</asp:ListItem>
                        <asp:ListItem Value="4">الابن</asp:ListItem>
                        <asp:ListItem Value="5">الابنة</asp:ListItem>
                        <asp:ListItem Value="6">الأخ</asp:ListItem>
                        <asp:ListItem Value="7">الأخت</asp:ListItem>
                        <asp:ListItem Value="8">الزوج</asp:ListItem>
                        <asp:ListItem Value="9">زوجة الأب</asp:ListItem>
                    </asp:DropDownList>

                </div>
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="CITY_ID">المدينة</label>
                    <asp:DropDownList ID="ddl_CITY_ID" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource3" DataTextField="City_AR_Name" DataValueField="City_ID"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT * FROM [Main_City] WHERE ([City_State] = 0)"></asp:SqlDataSource>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-3">
                    <label for="P_STATE">حالة المريض</label>
                    <asp:DropDownList ID="P_STATE" CssClass="chosen-select drop-down-list form-control" runat="server">
                        <asp:ListItem Value="0">مفعل</asp:ListItem>
                        <asp:ListItem Value="1">موقوف</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-3">
                    <label for="txt_exp_date">تاريخ صلاحية البطاقة</label>
                    <div class="input-group">
                        <asp:TextBox ID="txt_exp_date" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_exp_date);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>

                        <div class="input-group-prepend">
                            <div class="input-group-text">
                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                            </div>
                        </div>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_exp_date" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_exp_date" ID="CalendarExtender1" Format="dd/MM/yyyy" PopupButtonID="ImageButton2" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_exp_date_MaskedEditExtender" TargetControlID="txt_exp_date" ID="MaskedEditExtender1" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                </div>
                <div class="form-group col-sm-3">
                    <label for="txt_NAT_NUMBER">الرقم الوطني</label>
                    <asp:TextBox ID="txt_NAT_NUMBER" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEUIN(event)"></asp:TextBox>

                </div>
                <div class="form-group col-sm-3">
                    <label for="txt_KID_NO">رقم القيد</label>
                    <asp:TextBox ID="txt_KID_NO" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEUIN(event)"></asp:TextBox>

                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-sm-3">
                    <label for="PHONE_NO">رقم الهاتف</label>
                    <asp:TextBox ID="txt_PHONE_NO" CssClass="form-control" runat="server" onkeypress="return isAlphabetKeyEUIN(event)"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_PHONE_NO" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-sm-6">
                    <label for="NOTES">ملاحظات</label>
                    <asp:TextBox ID="txt_NOTES" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="form-group col-sm-3">
                    <label for="FileUpload1">صورة البطاقة</label>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </div>
            </div>

            <hr />
            <div class="form-row">
                <div class="form-group col-sm-3">
                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
