<%@ Page Title="أسعار الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="cashPrices.aspx.vb" Inherits="Medical_Insurance.cashPrices" Culture="ar-LY" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        hr {
            margin-top: 1rem;
            margin-bottom: 1rem;
            border: 0;
            border-top: 1px solid rgba(0, 0, 0, 0.1);
        }

        hrx {
            display: block;
            margin-top: 0.5em;
            margin-bottom: 0.5em;
            margin-left: auto;
            margin-right: auto;
            border-style: inset;
            border-width: 1px;
        }
    </style>

    <%--<script src="../Style/JS/jquery-3.4.1.min.js"></script>--%>
    <script src="../Style/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../Style/plugins/dataTables/dataTables.bootstrap4.min.js"></script>
    <script src="../Style/JS/sweetalert2.all.min.js"></script>
    <link href="../Style/CSS/sweetalert2.min.css" rel="stylesheet" />




    <script>
        $(document).ready(function () {
            $('table.com-tbl').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "scrollX": true,
                "responsive": true,
                "paging": false,
                "ordering": false,
                "info": false,
                "processing": true,
                "language": {
                    "lengthMenu": "عرض _MENU_ سجل",
                    "zeroRecords": "لا توجد بيانات متاحة.",
                    "info": "الإجمالي: _TOTAL_ خدمة",
                    "infoEmpty": "لا توجد بيانات متاحة.",
                    "infoFiltered": "(تمت تصفيتها من اصل _MAX_ سجل)",
                    "emptyTable": "لا توجد بيانات متاحة.",
                    "loadingRecords": "جاري التحميل...",
                    "processing": "جاري المعالجة...",
                    "search": "البحث: ",
                    "zeroRecords": "لا توجد بيانات مطابقة!",
                    "paginate": {
                        "first": "البداية",
                        "last": "النهاية",
                        "next": "التالي",
                        "previous": "السابق"
                    },
                    "aria": {
                        "sortAscending": ": تفعيل ليتم الترتيب التصاعدي",
                        "sortDescending": ": تفعيل ليتم الترتيب التنازلي"
                    }
                }
            });
        });
    </script>

    <style>
        .panel {
            height: 1530px;
            max-height: 1530px;
        }

        .scrollable {
            overflow-y: auto;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card mt-2">
                <div class="card-header">أسعار العرض</div>
                <div class="card-body">

                    <asp:Label ID="lbl_profile_name" runat="server" Text=""></asp:Label>

                    <div class="row">
                        <div class=" col-xs-12 col-sm-3">
                            <%--<label for="ddl_clinics">طريقة العرض</label>--%>
                            <asp:Label ID="Label4" runat="server" Text="طريقة العرض"></asp:Label>

                            <asp:DropDownList ID="ddl_show_type" CssClass="form-control" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="1">العيادات</asp:ListItem>
                                <asp:ListItem Value="2">الخدمات المجمعة</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-lg-3">
                            <asp:Label ID="Label1" runat="server" Text="العيادة"></asp:Label>
                            <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT 0 AS clinic_id, '- جميع العيادات -' AS Clinic_AR_Name FROM [MAIN_CLINIC] UNION SELECT clinic_id, Clinic_AR_Name FROM [MAIN_CLINIC] WHERE Clinic_State = 0"></asp:SqlDataSource>
                        </div>

                        <div class="col-lg-3">
                            <asp:Label ID="Label2" runat="server" Text="القسم"></asp:Label>
                            <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4" DataTextField="Service_AR_Name" DataValueField="Service_ID">
                            </asp:DropDownList>

                            <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS Service_ID, 'الكل' AS Service_AR_Name FROM Main_Services UNION SELECT [Service_ID], [Service_AR_Name] FROM [Main_Services] WHERE ([Service_Clinic] = @CLINIC_ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddl_clinics" PropertyName="SelectedValue" Name="CLINIC_ID" Type="Int32"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>


                        <div class="col-lg-3">
                            <asp:Label ID="lbl_groub" runat="server" Text="المجموعة" Enabled="false"></asp:Label>
                            <asp:DropDownList ID="ddl_group" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="GROUP_ARNAME" DataValueField="GROUP_ID"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [Group_ID], 'يرجى اختيار مجموعة' AS [Group_ARname] FROM Main_GroupSubService UNION SELECT [Group_ID], [Group_ARname] FROM [Main_GroupSubService] WHERE [Group_State] = 0"></asp:SqlDataSource>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-lg-3">
                            <asp:Label ID="lbl_services_group" runat="server" Text="الخدمات" Enabled="false"></asp:Label>
                            <asp:DropDownList ID="ddl_services_group" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>

                        </div>

                        <div class="col-lg-3">
                            <asp:Label ID="Label3" runat="server" Text="الطبيب"></asp:Label>
                            <asp:DropDownList ID="dll_doctors" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource3" DataTextField="MedicalStaff_AR_Name" DataValueField="MedicalStaff_ID">
                                <asp:ListItem Value="Choose a Division" Text="Choose a Division">Choose a Division</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [MedicalStaff_ID], '' AS [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] UNION SELECT [MedicalStaff_ID], [MedicalStaff_AR_Name] FROM [Main_MedicalStaff]"></asp:SqlDataSource>

                        </div>

                        <div class="col-lg-1">
                            <br />
                            <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Checked="True" />

                        </div>

                        <div class="col-lg-3">
                            <br />
                            <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-primary btn-block" Text="بحث" ValidationGroup="save_data" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />

                        </div>
                        <%--<div class="col-lg-4">
                    <br />
                    <asp:Button ID="btn_print" CssClass="btn btn-outline-secondary" runat="server" Text="طباعة الأسعار" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                    <asp:Button ID="btn_print1" CssClass="btn btn-outline-secondary" runat="server" Text="طباعة خدمات بدون سعر" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                </div>
                        --%>
                    </div>

                    <hr />

                    <asp:Panel ID="Panel1" runat="server" Visible="false">

                        <div class="col-lg-12">
                            <div class="row">
                                <div class="col-lg-3">
                                    <br />
                                    <asp:TextBox ID="txt_private_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEUIN(event)" placeholder="سعر الخدمة"></asp:TextBox>
                                </div>
                                <div class="col-lg-3">
                                    <br />
                                    <asp:TextBox ID="txt_per_add" runat="server" AutoCompleteType="Disabled" CssClass="form-control" placeholder="نسبة الزيادة/النقصان" TextMode="Number"></asp:TextBox>
                                </div>

                                <div class="col-lg-3">
                                    <br />
                                    <asp:Button ID="btn_apply" CssClass="btn btn-outline-info btn-block" runat="server" Text="تطبيق" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                                </div>

                                <div class="col-lg-3">
                                    <br />
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" Visible="false" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                                </div>
                            </div>
                        </div>

                        <!-- row -->
                    </asp:Panel>

                    <br />

                    <div class="row">
                        <div class="col-sm-12">

                            <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm com-tbl" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="SubService_ID" HeaderText="رقم الخدمة">
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
                                    <asp:BoundField DataField="SubService_Code" HeaderText="كود الخدمة" SortExpression="SubService_Code" />
                                    <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم الخدمة بالعربي" SortExpression="SubService_AR_Name" />
                                    <asp:BoundField DataField="SubService_EN_Name" HeaderText="اسم الخدمة بالانجليزي" SortExpression="SubService_EN_Name" />
                                    <asp:BoundField HeaderText="اسم العيادة" DataField="CLINIC_NAME"></asp:BoundField>
                                    <asp:BoundField HeaderText="سعر النقدي" DataField="CASH_PRS"></asp:BoundField>
                                    <asp:TemplateField HeaderText="السعر" SortExpression="22">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_service_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
         <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_save" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btn_apply" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btn_search" EventName="Click" />
                </Triggers>
    </asp:UpdatePanel>

</asp:Content>


