<%@ Page Title="أسعار الخدمات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="SERVICES_PRICES.aspx.vb" Inherits="Medical_Insurance.SERVICES_PRICES" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Style/JS/jquery-3.4.1.min.js"></script>
    <script src="../Style/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../Style/plugins/dataTables/dataTables.bootstrap4.min.js"></script>

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
            height: 530px;
            max-height: 530px;
        }

        .scrollable {
            overflow-y: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header bg-success text-light">
            <asp:Label ID="lbl_profile_name" runat="server" Text=""></asp:Label>
        </div>
        <div class="card-body">

            <div class="form-row justify-content-center">
                <div class="form-group col-xs-12 col-sm-4">
                    <label for="ddl_clinics">طريقة العرض</label>
                    <asp:DropDownList ID="ddl_show_type" CssClass="form-control" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="1">العيادات</asp:ListItem>
                        <asp:ListItem Value="2">الخدمات المجمعة</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <!-- form-row -->
            <div class="form-row justify-content-center">
                <div class="form-group col-xs-6 col-sm-3">
                    <asp:Label ID="Label1" runat="server" Text="العيادة"></asp:Label>
                    <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT 0 AS clinic_id, 'يرجى اختيار عيادة' AS Clinic_AR_Name FROM [MAIN_CLINIC] UNION SELECT clinic_id, Clinic_AR_Name FROM [MAIN_CLINIC]"></asp:SqlDataSource>
                </div>
                <div class="form-group col-xs-6 col-sm-3">
                    <asp:Label ID="Label2" runat="server" Text="القسم"></asp:Label>
                    <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4" DataTextField="Service_AR_Name" DataValueField="Service_ID">
                    </asp:DropDownList>

                    <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS Service_ID, 'الكل' AS Service_AR_Name FROM Main_Services UNION SELECT [Service_ID], [Service_AR_Name] FROM [Main_Services] WHERE ([Service_Clinic] = @CLINIC_ID)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddl_clinics" PropertyName="SelectedValue" Name="CLINIC_ID" Type="Int32"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="form-group col-xs-6 col-sm-3">
                    <asp:Label ID="lbl_groub" runat="server" Text="المجموعة" Enabled="false"></asp:Label>
                    <asp:DropDownList ID="ddl_group" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="GROUP_ARNAME" DataValueField="GROUP_ID"></asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [Group_ID], 'يرجى اختيار مجموعة' AS [Group_ARname] FROM Main_GroupSubService UNION SELECT [Group_ID], [Group_ARname] FROM [Main_GroupSubService] WHERE [Group_State] = 0"></asp:SqlDataSource>
                </div>
                <div class="form-group col-xs-6 col-sm-3">
                    <asp:Label ID="lbl_services_group" runat="server" Text="الخدمات" Enabled="false"></asp:Label>
                    <asp:DropDownList ID="ddl_services_group" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                </div>
            </div>
            <!-- form-row -->

            <div class="form-row justify-content-end">

                <div class="form-group col-xs-6 col-sm-2">
                    <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-info btn-block" Text="بحث" ValidationGroup="save_data" />
                </div>
            </div>

            <asp:Panel ID="Panel1" runat="server" Visible="false">
                <div class="form-row justify-content-center">
                    <div class="form-group col-xs-12 col-sm-1">
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Checked="True" />
                    </div>
                    <%--<div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_private_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isAlphabetKeyEUIN(event)" placeholder="سعر الخاص"></asp:TextBox>
                    </div>--%>
                    <div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_inc_price_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isNumberKey(event,this)" placeholder="سعر التأمين"></asp:TextBox>
                    </div>
                   <%-- <div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_invoice_price_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isNumberKey(event,this)" placeholder="سعر المستأجر"></asp:TextBox>
                    </div>--%>
                    <div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_cost_price_all" runat="server" onblur="appendDollar(this.id);" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isNumberKey(event,this)" placeholder="سعر التكلفة"></asp:TextBox>
                    </div>
                    <%--<div class="form-group col-xs-12 col-sm-2">
                        <asp:TextBox ID="txt_add_per" runat="server" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return isNumberKey(event,this)" placeholder="نسبة زيادة سعر التأمين"></asp:TextBox>
                    </div>--%>
                    <div class="form-group col-xs-12 col-sm-2">
                        <asp:Button ID="btn_apply" CssClass="btn btn-success btn-block" runat="server" Text="تطبيق" />
                    </div>

                </div>
                <!-- row -->
            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-row justify-content-end">
                        <div class="col-sm-2">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="2">
                                    <ProgressTemplate>
                                        <img src="Style/images/loading.gif" width="50px" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                        <div class="form-group col-xs-6 col-sm-2">
                            <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" Visible="false" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>
                    </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_save" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
                    <hr />

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
                                        <%--<asp:TemplateField HeaderText="سعر الخاص" SortExpression="22">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_private_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="سعر التأمين" SortExpression="22">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_inc_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="سعر المستأجر">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_invoice_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="سعر التكلفة">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_cost_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                       
                        </div>
                    </div>
                

        </div>
    </div>


</asp:Content>
