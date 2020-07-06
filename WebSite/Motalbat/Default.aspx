<%@ Page Title="المطالبات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Motalbat/motalbat.Master" CodeBehind="Default.aspx.vb" Inherits="Medical_Insurance._Default" %>

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
                "pageLength": 25,
                "processing": true,
                "columns": [
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    null,
                    { "searchable": false },
                   null,
                    null

                ],
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "language": {
                    "lengthMenu": "عرض _MENU_ سجل",
                    "zeroRecords": "لا توجد بيانات متاحة.",
                    "info": "الإجمالي: _TOTAL_ شركة",
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="motalbat-pate">
        <div class="row mt-2">
            <div class="col-sm-12">
                <div class="card">
                    <div class="card-header">إنشاء فاتورة جديدة</div>
                    <div class="card-body">
                        <div class="form-row">
                            <div class="form-group col-xs-12 col-sm-6">
                                <label for="ddl_companies">الشركة</label>
                                <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                            </div>

                        </div>
                        <!-- /form-row -->
                        <div class="form-row justify-content-start">
                            <div class="form-group col-xs-6 col-sm-3">
                                <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-info btn-block" Text="بحث" />
                            </div>
                            <div class="form-group col-xs-6 col-sm-3">
                                <asp:Button ID="btn_print" runat="server" CssClass="btn btn-outline-secondary btn-block" Text="طباعة" />
                            </div>
                        </div>
                        <!-- /form-row -->
                        <hr />
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:GridView ID="GridView1" class="table table-striped table-bordered com-tbl nowrap w-100" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                    <Columns>
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
                                        <asp:BoundField DataField="CARD_NO" HeaderText="رقم البطاقة"></asp:BoundField>
                                        <asp:BoundField DataField="BAGE_NO" HeaderText="الرقم الوظيفي"></asp:BoundField>
                                        <asp:BoundField DataField="NAME_ARB" HeaderText="اسم المنتفع"></asp:BoundField>
                                        <asp:BoundField DataField="PROCESSES_RESIDUAL" HeaderText="القيمة المستحقة"></asp:BoundField>
                                        <asp:BoundField DataField="INVOICE_NO" HeaderText="رقم الفاتورة"></asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>

                                                <asp:Button ID="btn_print" runat="server"
                                                    CommandName="printKasima"
                                                    CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                    Text="طباعة"
                                                    ToolTip="طباعة حركة المنتفع"
                                                    ControlStyle-CssClass="btn btn-primary btn-small" />

                                            </ItemTemplate>
                                        </asp:TemplateField>
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
</asp:Content>
