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
                                    <asp:BoundField DataField="IEC_ID" HeaderText="ر.ت">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SubService_ID" HeaderText="ر.خ">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IEC_PID" HeaderText="رقم المنتفع">
                                        <ControlStyle CssClass="hide-colum" />
                                        <FooterStyle CssClass="hide-colum" />
                                        <HeaderStyle CssClass="hide-colum" />
                                        <ItemStyle CssClass="hide-colum" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="P_NAME_ARB" HeaderText="اسم المنتفع"></asp:BoundField>
                                    <asp:BoundField DataField="CARD_NO" HeaderText="رقم البطاقة"></asp:BoundField>
                                    <asp:BoundField DataField="BAGE_NO" HeaderText="الرقم الوظيفي"></asp:BoundField>
                                    <asp:BoundField DataField="SubService_Code" HeaderText="كود الخدمة"></asp:BoundField>
                                    <asp:BoundField DataField="SubService_AR_Name" HeaderText="اسم الخدمة"></asp:BoundField>
                                    <asp:BoundField DataField="IEC_Type_char" HeaderText="النوع"></asp:BoundField>
                                    <asp:BoundField DataField="SubService_Price" HeaderText="السعر"></asp:BoundField>
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
</asp:Content>
