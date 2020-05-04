<%@ Page Title="طلبات الموافقة" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="listApproval.aspx.vb" Inherits="Medical_Insurance.listApproval" %>

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
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "language": {
                    "lengthMenu": "عرض _MENU_ سجل",
                    "zeroRecords": "لا توجد بيانات متاحة.",
                    "info": "الإجمالي: _TOTAL_ سجل",
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
    <div class="row mb-2">
        <div class="col-xs-12 col-sm-12">
            <div class="card mt-1 ">
                <div class="card-header bg-secondary text-light">طلبات معلقة</div>
                <div class="card-body">
                    <div class="row mb-2">
                        <div class="col-xs-12 col-sm-6">
                            <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id" AutoPostBack="True"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'جميع الشركات' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12">
                            <asp:GridView ID="GridView1" class="table table-striped table-bordered com-tbl" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="CONFIRM_ID" HeaderText="رقم الطلب"></asp:BoundField>
                                    <asp:BoundField DataField="P_NAME" HeaderText="اسم المنتفع"></asp:BoundField>
                                    <asp:BoundField DataField="C_NAME" HeaderText="اسم الشركة"></asp:BoundField>
                                    <asp:BoundField DataField="CARD_NO" HeaderText="رقم البطاقة"></asp:BoundField>
                                    <asp:BoundField DataField="BAGE_NO" HeaderText="الرقم الوظيفي"></asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_VALUE" HeaderText="الإجمالي"></asp:BoundField>
                                    <asp:TemplateField HeaderText="قيمة الموافقة" SortExpression="22">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_confirm_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_approval" runat="server"
                                                CommandName="approval_req"
                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                ControlStyle-CssClass="btn btn-small btn-success text-white"
                                                OnClientClick="return confirm('هل أنت متأكد من هذا الإجراء؟')">موافقة</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_reject" runat="server"
                                                CommandName="reject_req"
                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                ControlStyle-CssClass="btn btn-small btn-danger text-white"
                                                OnClientClick="return confirm('هل أنت متأكد من هذا الإجراء؟')">رفض</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-2">
        <div class="col-xs-12 col-sm-12">
            <div class="card mt-1 ">
                <div class="card-header bg-warning text-dark">خدمات قدمت وفي انتظار الموافقة</div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12">
                            <asp:GridView ID="GridView2" class="table table-striped table-bordered" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="CONFIRM_ID" HeaderText="رقم الطلب"></asp:BoundField>
                                    <asp:BoundField DataField="P_NAME" HeaderText="اسم المنتفع"></asp:BoundField>
                                    <asp:BoundField DataField="C_NAME" HeaderText="اسم الشركة"></asp:BoundField>
                                    <asp:BoundField DataField="CARD_NO" HeaderText="رقم البطاقة"></asp:BoundField>
                                    <asp:BoundField DataField="BAGE_NO" HeaderText="الرقم الوظيفي"></asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_VALUE" HeaderText="الإجمالي"></asp:BoundField>
                                    <asp:BoundField DataField="APPROVED_VALUE" HeaderText="القيمة المدفوعة"></asp:BoundField>
                                    <asp:BoundField DataField="PENDING_VALUE" HeaderText="القيمةالمتبقية"></asp:BoundField>
                                    <asp:TemplateField HeaderText="القيمة المتبقية" SortExpression="22">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_confirm_price" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_approval" runat="server"
                                                CommandName="approval_req"
                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                ControlStyle-CssClass="btn btn-small btn-success text-white"
                                                OnClientClick="return confirm('هل أنت متأكد من هذا الإجراء؟')">موافقة</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_reject" runat="server"
                                                CommandName="reject_req"
                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                ControlStyle-CssClass="btn btn-small btn-danger text-white"
                                                OnClientClick="return confirm('هل أنت متأكد من هذا الإجراء؟')">رفض</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function Confirm() {
            return confirm("قيمة الموافقة أقل من قيمة العملية، هل تود الإستمرار؟");
        }
    </script>
</asp:Content>
