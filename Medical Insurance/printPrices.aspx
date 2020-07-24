<%@ Page Title="طباعة ملف أسعار الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="printPrices.aspx.vb" Inherits="Medical_Insurance.printPrices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header bg-success text-light">طباعة ملف أسعار الخدمات</div>
        <div class="card-body">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row justify-content-center">
                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_prices_profile">ملف الأسعار</label>
                            <asp:DropDownList ID="ddl_prices_profile" CssClass="form-control drop-down-list chosen-select" runat="server" DataSourceID="SqlDataSource3" DataTextField="profile_name" DataValueField="profile_id"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="select 3025 as profile_id, 'ملف أسعار عروض الشركات' as profile_name from INC_PRICES_PROFILES union select profile_id, profile_name from INC_PRICES_PROFILES where is_default = 0 and profile_sts = 0 and profile_id <> 3025"></asp:SqlDataSource>
                        </div>
                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_show_type">التصنيف</label>
                            <asp:DropDownList ID="ddl_show_type" CssClass="form-control drop-down-list" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource5" DataTextField="Group_ARname" DataValueField="Group_ID">
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource5" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT [Group_ID], [Group_ARname] FROM [Main_GroupSubService] WHERE ([Group_State] = 0)"></asp:SqlDataSource>
                        </div>
                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_sub_gourp">المجموعة</label>
                            <asp:DropDownList ID="ddl_sub_gourp" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource2" DataTextField="SubGroup_ARname" DataValueField="SubGroup_ID"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [SubGroup_ID], N'غير مصنف' AS [SubGroup_ARname] FROM Main_SubGroup UNION SELECT [SubGroup_ID], [SubGroup_ARname] FROM [Main_SubGroup] WHERE [SubGroup_State] = 0 and MainGroup_ID = @MainGroup_ID">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddl_show_type" PropertyName="SelectedValue" Name="MainGroup_ID"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>

                    </div>
                    <!-- form-row -->
                    <div class="row justify-content-center">
                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_clincs">العيادة</label>
                            <asp:DropDownList ID="ddl_clinics" runat="server" AutoPostBack="True" CssClass="chosen-select drop-down-list form-control" DataSourceID="SqlDataSource1" DataTextField="Clinic_AR_Name" DataValueField="clinic_id"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT -1 AS [clinic_id], N'يرجى اختيار عيادة' AS [Clinic_AR_Name] FROM [Main_Clinic] UNION SELECT [clinic_id], [Clinic_AR_Name] FROM [Main_Clinic] where Clinic_State = 0"></asp:SqlDataSource>
                        </div>
                        <div class="form-group col-sm-12 col-md-3">
                            <label for="ddl_services">القسم</label>
                            <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-sm-12 col-md-3">
                            <label for="dll_doctors">الطبيب</label>
                            <asp:DropDownList ID="dll_doctors" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource4" DataTextField="MedicalStaff_AR_Name" DataValueField="MedicalStaff_ID">
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [MedicalStaff_ID], 'غير مصنف' AS [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] UNION SELECT [MedicalStaff_ID], [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] where MedicalStaff_State = 0"></asp:SqlDataSource>

                        </div>
                    </div>

                    <div class="row justify-content-center mt-3">
                        <div class="col-sm-12 col-md-3">
                            <asp:Button ID="btn_show" runat="server" Text="عرض" CssClass="btn btn-info btn-block" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                        <div class="col-sm-12 col-md-3">
                            <asp:Button ID="btn_print" runat="server" Text="طباعة" CssClass="btn btn-success btn-block" Enabled="False" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                        </div>

                    </div>

                    <div class="row mt-2">
                        <div class="col-sm-12">
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="SubService_ID" HeaderText="رقم الخدمة">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SubService_Code" HeaderText="كود الخدمة" SortExpression="SubService_Code" />
                                    <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم الخدمة" SortExpression="SubService_AR_Name" />
                                    <asp:BoundField DataField="Service_EN_Name" HeaderText="القسم" SortExpression="Service_AR_Name" />
                                    <asp:BoundField DataField="SubGroup_ARname" HeaderText="المجموعة" SortExpression="SubGroup_ARname"></asp:BoundField>
                                    <asp:BoundField DataField="Group_ARname" HeaderText="التصنيف" SortExpression="Group_ARname"></asp:BoundField>
                                    <asp:BoundField DataField="INS_PRS" HeaderText="السعر" SortExpression="INS_PRS" DataFormatString="{0:c3}"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btn_print" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>
