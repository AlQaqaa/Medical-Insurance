<%@ Page Title="أسعار الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="SERVICES_PRICES.aspx.vb" Inherits="Medical_Insurance.SERVICES_PRICES" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header bg-success text-light">إدخال أسعار الخدمات</div>
        <div class="card-body">
            <div class="row justify-content-center mb-2">
                <div class="col-sm-12 col-md-6">
                    <div class="form-group row">
                        <label for="inputName" class="col-sm-3 col-form-label">اسم الملف</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txt_profile_name" CssClass="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-row justify-content-center">
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="ddl_clinics">طريقة العرض</label>
                    <asp:DropDownList ID="ddl_show_type" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="1">العيادات</asp:ListItem>
                        <asp:ListItem Value="2">الخدمات المجمعة</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <!-- form-row -->
            <asp:Panel ID="clinic_Panel" runat="server">
                <div class="form-row justify-content-center">
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

                </div>
            </asp:Panel>
            <asp:Panel ID="groups_Panel" runat="server" Visible="False">
                <div class="form-row justify-content-center mb-2">
                    <div class="form-group col-xs-12 col-sm-4">
                        <label for="ddl_clinics">المجموعة</label>
                        <asp:DropDownList ID="ddl_gourp" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="GROUP_ARNAME" DataValueField="GROUP_ID"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [Group_ID], 'يرجى اختيار مجموعة' AS [Group_ARname] FROM Main_GroupSubService UNION SELECT [Group_ID], [Group_ARname] FROM [Main_GroupSubService] WHERE [Group_State] = 0"></asp:SqlDataSource>
                    </div>
                    <div class="form-group col-xs-12 col-sm-4">
                        <label for="txt_clinics_max">الخدمات</label>
                        <asp:DropDownList ID="ddl_services_group" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-row justify-content-end">
                    <div class="form-group col-xs-12 col-sm-1">
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Checked="True" />
                    </div>
                    <div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_private_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isNumberKey(event,this)" placeholder="سعر الخاص"></asp:TextBox>
                    </div>
                    <div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_inc_price_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isNumberKey(event,this)" placeholder="سعر التأمين"></asp:TextBox>
                    </div>
                    <div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_invoice_price_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isNumberKey(event,this)" placeholder="سعر المستأجر"></asp:TextBox>
                    </div>
                    <div class="form-group col-xs-12 col-sm-1">
                        <asp:Button ID="btn_apply" CssClass="btn btn-success btn-block" runat="server" Text="تطبيق" />
                    </div>
                    <div class="form-group col-xs-12 col-sm-2">
                    </div>
                </div>
                <!-- row -->

            </asp:Panel>


            <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="SubService_ID" HeaderText="رقم الخدمة">
                        <ControlStyle CssClass="hide-colum" />
                        <FooterStyle CssClass="hide-colum" />
                        <HeaderStyle CssClass="hide-colum" />
                        <ItemStyle CssClass="hide-colum" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="مغطاة؟">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SubService_Code" HeaderText="كود الخدمة" SortExpression="SubService_Code" />
                    <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم الخدمة بالعربي" SortExpression="SubService_AR_Name" />
                    <asp:BoundField DataField="SubService_EN_Name" HeaderText="اسم الخدمة بالانجليزي" SortExpression="SubService_EN_Name" />
                    <asp:BoundField HeaderText="اسم العيادة" DataField="CLINIC_NAME"></asp:BoundField>
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row justify-content-end">
                        <div class="col-sm-2">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="2">
                                    <ProgressTemplate>
                                        <img src="Style/images/loading.gif" width="70px" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                        <div class="form-group col-sm-3">
                            <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>
