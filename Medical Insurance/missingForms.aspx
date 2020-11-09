<%@ Page Title="النماذج المفقودة" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="missingForms.aspx.vb" Inherits="Medical_Insurance.missingForms" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .HideAsl {
            display: none;
            visibility: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <div class="card mt-2">
        <div class="card-header">تسديد الاطباء</div>
        <div class="card-body">
            <div class="row mb-2 justify-content-end">
                <div class="col-xs-12 col-md-3">
                    <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id" Width="100%"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'جميع الشركات' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                </div>

                <div class="col-xs-12 col-md-3">
                    <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID" Width="100%">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT 0 AS clinic_id, 'جميع العيادات' AS Clinic_AR_Name FROM [MAIN_CLINIC] UNION SELECT clinic_id, Clinic_AR_Name FROM [MAIN_CLINIC]"></asp:SqlDataSource>
                </div>
                <div class="form-group col-xs-12 col-md-3">
                    <div class="input-group">
                        <asp:TextBox ID="txt_start_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_start_dt);" placeholder="سنه/شهر/يوم" TabIndex="6" ToolTip="تاريخ تقديم الخدمة"></asp:TextBox>
                        <div class="input-group-prepend">
                            <div class="input-group-text">
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                            </div>
                        </div>
                    </div>
                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_start_dt" ID="CalendarExtender3" Format="dd/MM/yyyy" PopupButtonID="ImageButton1" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_start_dt_MaskedEditExtender" TargetControlID="txt_start_dt" ID="MaskedEditExtender3" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                </div>
                <div class="form-group col-xs-12 col-md-3">
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
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-md-3">
                    <asp:DropDownList ID="ddl_doctors" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource3" DataTextField="MedicalStaff_AR_Name" DataValueField="MedicalStaff_ID" Width="100%"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [MedicalStaff_ID], N'جميع الأطباء' AS [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] UNION SELECT [MedicalStaff_ID], [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] WHERE MedicalStaff_State = 0"></asp:SqlDataSource>
                </div>
                <div class="col-xs-12 col-md-2">

                    <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-success btn-block" Text="بحث" />
                </div>

                <div class="col-xs-12 col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col">
                    <div class="panel-scroll scrollable">

                        <asp:GridView ID="GridView1" CssClass="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="رقم النموذج" DataField="Req_Code"></asp:BoundField>
                                <asp:BoundField DataField="Processes_Date" HeaderText="تاريخ الحركة"></asp:BoundField>
                                <asp:BoundField DataField="NAME_ARB" HeaderText="اسم المنتفع"></asp:BoundField>
                                <asp:BoundField DataField="CARD_NO" HeaderText="رقم البطاقة"></asp:BoundField>
                                <asp:BoundField DataField="Clinic_AR_Name" HeaderText="العيادة"></asp:BoundField>
                                <asp:BoundField DataField="SubService_AR_Name" HeaderText="الخدمة"></asp:BoundField>

                                <asp:BoundField DataField="Req_No" HeaderText="pay" SortExpression="Req_No">
                                    <ControlStyle CssClass="HideAsl" Height="1px" Width="1px" />
                                    <HeaderStyle CssClass="HideAsl" />
                                    <ItemStyle CssClass="HideAsl" />
                                </asp:BoundField>


                                <asp:BoundField DataField="c_id" HeaderText="pay" SortExpression="c_id">
                                    <ControlStyle CssClass="HideAsl" Height="1px" Width="1px" />
                                    <HeaderStyle CssClass="HideAsl" />
                                    <ItemStyle CssClass="HideAsl" />
                                </asp:BoundField>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btn_print_one" runat="server"
                                            CommandName="printProcess"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            Text="طباعة"
                                            ControlStyle-CssClass="btn btn-secondary btn-small" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
