<%@ Page Title="أسعار الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="SERVICES_PRICES.aspx.vb" Inherits="Medical_Insurance.SERVICES_PRICES" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card mt-1 ">
                <div class="card-header bg-success text-light">إدخال أسعار الخدمات</div>
                <div class="card-body">

                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-4">
                            <label for="ddl_clinics">العيادة</label>
                            <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT * FROM [MAIN_CLINIC]"></asp:SqlDataSource>
                        </div> 
                        <div class="form-group col-xs-12 col-sm-4">
                            <label for="ddl_clinics">الخدمة</label>
                            <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4" DataTextField="Service_AR_Name" DataValueField="Service_ID">
                            </asp:DropDownList>

                            <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS Service_ID, '' AS Service_AR_Name FROM Main_Services UNION SELECT [Service_ID], [Service_AR_Name] FROM [Main_Services] WHERE ([Service_Clinic] = @CLINIC_ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddl_clinics" PropertyName="SelectedValue" Name="CLINIC_ID" Type="Int32"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div class="form-group col-xs-12 col-sm-4">
                            <label for="txt_start_dt">تاريخ اعتماد الاسعار</label>
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
                    </div>

                    <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="SubService_ID" HeaderText="ر.خ" SortExpression="SubService_ID" HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="SubService_Code" HeaderText="كود الخدمة" SortExpression="SubService_Code" />
                            <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم الخدمة بالعربي" SortExpression="SubService_AR_Name" />
                            <asp:BoundField DataField="SubService_EN_Name" HeaderText="اسم الخدمة بالانجليزي" SortExpression="SubService_EN_Name" />
                            <asp:TemplateField HeaderText="سعر الخاص" SortExpression="22">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_private_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="سعر التأمين" SortExpression="22">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_inc_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="سعر فوترة">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_invoice_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <hr />
                    <div class="form-row">
                        <div class="form-group col-sm-3">
                            <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
