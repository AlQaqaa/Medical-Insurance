<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home/main.Master" CodeBehind="SERVICES_PRICES.aspx.vb" Inherits="Medical_Insurance.SERVICES_PRICES" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="card mt-1 ">
        <div class="card-header bg-success text-light">إدخال أسعار الخدمات</div>
        <div class="card-body">

            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="ddl_clinics">العيادة</label>
                    <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="CLINICNAME_EN" DataValueField="CLINIC_ID">
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT * FROM [MAIN_CLINIC]"></asp:SqlDataSource>




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

            <asp:GridView ID="GridView1" class="table table-striped table-bordered" runat="server" Width="98%" GridLines="None" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="رقم الخدمة" SortExpression="ID" />
                    <asp:BoundField DataField="SUBSERV_CODE" HeaderText="كود الخدمة" SortExpression="SUBSERV_CODE" />
                    <asp:BoundField DataField="SUBSERV_NAMEARB" HeaderText="اسم الخدمة بالعربي" SortExpression="SUBSERV_NAMEARB" />
                    <asp:BoundField DataField="SUBSERV_NAMEENG" HeaderText="اسم الخدمة بالانجليزي" SortExpression="SUBSERV_NAMEENG" />



                    <asp:TemplateField HeaderText="سعر الخاص" SortExpression="22">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" Width="73px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="سعر التأمين" SortExpression="22">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" Width="73px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="سعر فوترة">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" Width="73px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT * FROM [MAIN_SUBSERVIES] WHERE ([CLINIC_ID] = @CLINIC_ID)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddl_clinics" Name="CLINIC_ID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>


        </div>
    </div>

    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT * FROM [MAIN_SUBSERVIES]"></asp:SqlDataSource>

    <div class="form-row">
        <div class="form-group col-sm-3">
            <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" />
        </div>
    </div>
    <div class="form-group col-sm-3">
    </div>

</asp:Content>
