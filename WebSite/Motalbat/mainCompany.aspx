<%@ Page Title="مطالبات الشركات الأم" Language="vb" AutoEventWireup="false" MasterPageFile="~/Motalbat/motalbat.Master" CodeBehind="mainCompany.aspx.vb" Inherits="Medical_Insurance.mainCompany" Culture="ar-LY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="motalbat-pate">
                <div class="row mt-2">
                    <div class="col-sm-12">
                        <div class="card">
                            <div class="card-header">قائمة الفواتير</div>
                            <div class="card-body">
                                <div class="form-row">
                                    <div class="form-group col-md-12 col-lg-4 col-xl-3">
                                        <label for="ddl_companies">الشركة</label>
                                        <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_Name_Arb" DataValueField="C_ID"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT C_ID, C_Name_Arb FROM INC_COMPANY_DATA AS TBL1 WHERE C_Level = 0 AND EXISTS (SELECT * FROM INC_COMPANY_DATA AS TBL2 WHERE TBL2.C_Level = TBL1.C_ID)"></asp:SqlDataSource>
                                    </div>
                                    <div class="form-group col-md-12 col-lg-4 col-xl-3">
                                        <label for="ddl_companies">نوع الفاتورة</label>
                                        <asp:DropDownList ID="ddl_invoice_type" CssClass="chosen-select drop-down-list form-control" runat="server">
                                            <asp:ListItem Value="0">الخدمات الطبية / الإيواء والعمليات</asp:ListItem>
                                            <asp:ListItem Value="1">الخدمات الطبية</asp:ListItem>
                                            <asp:ListItem Value="2">الإيواء والعمليات</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-12 col-lg-4 col-xl-3">
                                        <label for="txt_start_dt">الفترة من</label>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب اختيار التاريخ" ValidationGroup="print" ForeColor="Red" ControlToValidate="txt_start_dt"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12 col-lg-4 col-xl-3">
                                        <label for="txt_start_dt">إلى</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                                </div>
                                            </div>
                                        </div>
                                        <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_end_dt" ID="CalendarExtender2" Format="dd/MM/yyyy" PopupButtonID="ImageButton2" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_end_dt_MaskedEditExtender" TargetControlID="txt_end_dt" ID="MaskedEditExtender2" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="يجب اختيار التاريخ" ValidationGroup="print" ForeColor="Red" ControlToValidate="txt_end_dt"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12 col-lg-4 col-xl-3">
                                        <label for="txt_mang_name">الصفة</label>
                                        <asp:TextBox ID="txt_Adjective" runat="server" dir="rtl" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_Adjective" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12 col-lg-4 col-xl-3">
                                        <label for="txt_mang_name">الاسم</label>
                                        <asp:TextBox ID="txt_mang_name" runat="server" dir="rtl" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_mang_name" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <!-- /form-row -->
                                <div class="form-row">
                                    
                                    <div class="form-group col-md-6 col-lg-2">
                                        <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-info btn-block mt-2" Text="بحث" ValidationGroup="print" />
                                    </div>
                                    <div class="form-group col-md-6 col-lg-2">
                                        <asp:Button ID="btn_print" runat="server" CssClass="btn btn-outline-secondary btn-block mt-2" Text="طباعة" ValidationGroup="print" Visible="False" />
                                    </div>
                                </div><!-- /form-row -->
                                <!-- /form-row -->
                               <div class="form-row justify-content-center">
                                    <div class="form-group col-md-12 col-lg-6">
                                        <asp:Label ID="lbl_msg" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <!-- /form-row -->
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                        <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField HeaderText="رقم الشركة" DataField="C_ID">
                                                    <ControlStyle CssClass="hide-colum" />
                                                    <FooterStyle CssClass="hide-colum" />
                                                    <HeaderStyle CssClass="hide-colum" />
                                                    <ItemStyle CssClass="hide-colum" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="تحديد">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ر.ت">
                                                    <ItemTemplate>
                                                        <span>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:BoundField DataField="C_Name_Arb" HeaderText="الشركة"></asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_VAL" HeaderText="إجمالي القيمة المستحقة" DataFormatString="{0:c3}"></asp:BoundField>
                                                
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <!-- /row -->
                            </div>
                            <!-- /card-body -->
                        </div>
                        <!-- /card -->
                    </div>
                </div>
                <!-- /row -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
