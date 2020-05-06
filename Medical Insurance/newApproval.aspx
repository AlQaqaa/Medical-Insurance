<%@ Page Title="إنشاء طلب موافقة جديد" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="newApproval.aspx.vb" Inherits="Medical_Insurance.newApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <div class="card mt-1">
        <div class="card-header bg-success text-light">
            طلب موافقة جديد
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-xs-12 col-sm-4">
                    <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                </div>
                <div class="form-group col-xs-12 col-sm-4">
                    <asp:TextBox ID="txt_search_box" CssClass="form-control" placeholder="رقم البطاقة / الرقم الوظيفي / الاسم" runat="server"></asp:TextBox>
                </div>
                <div class="form-group col-xs-6 col-sm-3">
                    <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-info btn-block" Text="بحث" />
                </div>
            </div>
            <!-- form-row -->
            <asp:Panel ID="Panel1" runat="server" Visible="False">
                <div class="form-row">
                    <div class="col-xs-12 col-sm-6">
                        <asp:ListBox ID="source_list" runat="server" SelectionMode="Multiple" Width="100%" Height="180px" AutoPostBack="True"></asp:ListBox>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6">
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6">
                                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                            </div>
                        </div><!-- form-row -->
                        <div class="row mt-4">
                            <div class="col-xs-12 col-sm-6">
                                <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6">
                                <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                            </div>
                        </div><!-- form-row -->
                        <div class="row mt-4">
                            <div class="col-xs-12 col-sm-6">
                                <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6">
                                <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                            </div>
                        </div><!-- form-row -->
                        <div class="row mt-4">
                            <div class="col-xs-12 col-sm-12">
                                <asp:Label ID="lbl_confirm_msg" runat="server" Text=""></asp:Label>
                            </div>
                        </div><!-- form-row -->
                    </div>
                </div>
                <!-- form-row -->
                <div class="form-row mt-2">
                    <div class="form-group col-xs-12 col-sm-3">
                        <div class="input-group">
                            <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="تاريخ انتهاء الموافقة" TabIndex="6" Enabled="False"></asp:TextBox>
                            <div class="input-group-prepend">
                                <div class="input-group-text">
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" Enabled="False" />
                                </div>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="* مطلوب" ControlToValidate="txt_end_dt" ForeColor="Red" ValidationGroup="chose"></asp:RequiredFieldValidator>
                        <ajaxtoolkit:calendarextender runat="server" targetcontrolid="txt_end_dt" id="CalendarExtender3" format="dd/MM/yyyy" popupbuttonid="ImageButton1" popupposition="TopLeft"></ajaxtoolkit:calendarextender>
                        <ajaxtoolkit:maskededitextender runat="server" culturedateplaceholder="" culturetimeplaceholder="" culturedecimalplaceholder="" culturethousandsplaceholder="" culturedateformat="" culturecurrencysymbolplaceholder="" cultureampmplaceholder="" century="2000" behaviorid="txt_end_dt_MaskedEditExtender" targetcontrolid="txt_end_dt" id="MaskedEditExtender3" mask="99/99/9999" masktype="Date"></ajaxtoolkit:maskededitextender>
                    </div>
                    <div class="col-xs-6 col-sm-2">
                        <asp:Button ID="btn_chose" runat="server" CssClass="btn btn-outline-primary btn-block" Text="يرجى اختيار منتفع" Enabled="false" ValidationGroup="chose" />
                    </div>
                </div>
                <!-- form-row -->
            </asp:Panel>
            <hr />
            <asp:Panel ID="Panel2" runat="server" Visible="false">
                <div class="form-row">
                    <div class="form-group col-xs-12 col-sm-2">
                        <asp:Label ID="Label9" runat="server" Text="النوع"></asp:Label>
                        <asp:DropDownList ID="ddl_type" CssClass="form-control" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="0">يرجى اختيار النوع</asp:ListItem>
                            <asp:ListItem Value="1">عملية/خدمة</asp:ListItem>
                            <asp:ListItem Value="2">إضافة على عملية</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-xs-6 col-sm-3">
                        <asp:Label ID="Label7" runat="server" Text="العيادة"></asp:Label>
                        <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID" Enabled="False">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT 0 AS clinic_id, 'يرجى اختيار عيادة' AS Clinic_AR_Name FROM [MAIN_CLINIC] UNION SELECT clinic_id, Clinic_AR_Name FROM [MAIN_CLINIC]"></asp:SqlDataSource>
                    </div>
                    <div class="form-group col-xs-6 col-sm-2">
                        <asp:Label ID="Label8" runat="server" Text="القسم"></asp:Label>
                        <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4" DataTextField="Service_AR_Name" DataValueField="Service_ID" Enabled="False">
                        </asp:DropDownList>

                        <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS Service_ID, 'الكل' AS Service_AR_Name FROM Main_Services UNION SELECT [Service_ID], [Service_AR_Name] FROM [Main_Services] WHERE ([Service_Clinic] = @CLINIC_ID)">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddl_clinics" PropertyName="SelectedValue" Name="CLINIC_ID" Type="Int32"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div class="form-group col-xs-6 col-sm-3">
                        <asp:Label ID="lbl_services_group" runat="server" Text="الخدمات" Enabled="false"></asp:Label>
                        <asp:DropDownList ID="ddl_sub_service" CssClass="chosen-select drop-down-list form-control" runat="server" Enabled="false"></asp:DropDownList>

                    </div>
                    <div class="col-xs-6 col-sm-2">
                        <asp:Button ID="btn_add" runat="server" CssClass="btn btn-outline-info btn-block mt-sm-4 mt-md-4" Text="أضف" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                    </div>
                </div>
                <!-- form-row -->
                <asp:Panel ID="Panel3" runat="server" Visible="False">
                    <div class="form-row mb-2">
                        <div class="col-xs-6 col-sm-6">
                            <asp:TextBox ID="txt_add_name" runat="server" AutoCompleteType="Disabled" CssClass="form-control" placeholder="اسم الإضافة"></asp:TextBox>
                        </div>
                        <div class="col-xs-6 col-sm-4">
                            <asp:TextBox ID="txt_add_price" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEUIN(event)" placeholder="سعر الإضافة"></asp:TextBox>
                        </div>
                    </div>
                    <!-- form-row -->
                </asp:Panel>
                <div class="form-row">
                    <div class="col-xs-12 col-sm-12">
                        <asp:GridView ID="GridView1" class="table table-striped table-bordered nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="ر.ت">
                                    <ItemTemplate>
                                        <span>
                                            <%#Container.DataItemIndex + 1%>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="تسلسلي" DataField="CD_ID">
                                    <ControlStyle CssClass="hide-colum" />
                                    <FooterStyle CssClass="hide-colum" />
                                    <HeaderStyle CssClass="hide-colum" />
                                    <ItemStyle CssClass="hide-colum" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="رقم الخدمة" DataField="SUB_SERVICE_ID">
                                    <ControlStyle CssClass="hide-colum" />
                                    <FooterStyle CssClass="hide-colum" />
                                    <HeaderStyle CssClass="hide-colum" />
                                    <ItemStyle CssClass="hide-colum" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SubService_Code" HeaderText="كود العملية"></asp:BoundField>
                                <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم العملية"></asp:BoundField>
                                <asp:BoundField DataField="SERVICE_PRICE" HeaderText="سعر العملية"></asp:BoundField>
                                <asp:BoundField DataField="REQUEST_TYPE" HeaderText="النوع"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_delete" runat="server"
                                            CommandName="delete_ser"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            ToolTip="إزالة"
                                            ControlStyle-CssClass="btn btn-danger btn-small"
                                            OnClientClick="return confirm('هل أنت متأكد من إزالة هذه الخدمة')">إزالة</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <!-- form-row -->
                <div class="form-row justify-content-end">
                    <div class="col-xs-12 col-sm-3">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btn_print" runat="server" CssClass="btn btn-outline-secondary btn-block mt-sm-4 mt-md-4" Text="طباعة" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_print" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>


</asp:Content>
