<%@ Page Title="أرصدة المنتفعين" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="PatientBalances.aspx.vb" Inherits="Medical_Insurance.PatientBalances" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Style/plugins/dataTables/dataTables.bootstrap4.min.css" rel="stylesheet" />
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row mt-3">
                <div class="col-lg-12">
                    <div class="card">

                        <!-- /.panel-heading -->
                        <div class="card-body">

                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm com-tbl" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField HeaderText="رقم الإيواء" DataField="EWA_Record_ID"></asp:BoundField>
                                            <asp:BoundField HeaderText="رقم المنتفع" DataField="INC_Patient_Code"></asp:BoundField>
                                            <asp:BoundField HeaderText="الاسم" DataField="NAME_ARB"></asp:BoundField>
                                            <asp:TemplateField HeaderText="الرصيد المسموح به">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBalance" runat="server" CssClass="form-control"></asp:TextBox>
                                                </ItemTemplate>
                                               
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btn_save" runat="server"
                                                        CommandName="save"
                                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        ToolTip="حفظ"
                                                        ControlStyle-CssClass="btn btn-link text-primary btn-new">حفظ</asp:LinkButton>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
