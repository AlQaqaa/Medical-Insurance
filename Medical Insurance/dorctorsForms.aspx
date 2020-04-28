<%@ Page Title="تسوية الأطباء" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="dorctorsForms.aspx.vb" Inherits="Medical_Insurance.dorctorsForms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Style/JS/jquery-3.4.1.min.js"></script>
    <script src="../Style/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../Style/plugins/dataTables/dataTables.bootstrap4.min.js"></script>

    <script>
        $(document).ready(function () {
            var table = $('table.com-tbl').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "fixedColumns": {
                    "leftColumns": 2
                },
                "fixedColumns": true,
                "columns": [
                    null,
                    null,
                    null,
                    { "searchable": false },
                    { "searchable": false },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null

                ],
                'columnDefs': [
                    {
                        "targets": "_all",
                        "className": "text-center"
                    }],
                "scrollX": true,
                "pageLength": 10,
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "language": {
                    "lengthMenu": "عرض _MENU_ سجل",
                    "zeroRecords": "لا توجد بيانات متاحة.",
                    "info": "الإجمالي: _TOTAL_ منتفع",
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
                            <div class="form-group col-xs-6 col-sm-3">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Visible="false" />
                            </div>
                        </div>
                        <!-- /form-row -->
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm com-tbl nowrap" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="تحديد">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="رقم العملية" DataField="Processes_ID">
                                            <ControlStyle CssClass="hide-colum" />
                                            <FooterStyle CssClass="hide-colum" />
                                            <HeaderStyle CssClass="hide-colum" />
                                            <ItemStyle CssClass="hide-colum" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="رقم المنتفع" DataField="PINC_ID">
                                            <ControlStyle CssClass="hide-colum" />
                                            <FooterStyle CssClass="hide-colum" />
                                            <HeaderStyle CssClass="hide-colum" />
                                            <ItemStyle CssClass="hide-colum" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Processes_Reservation_Code" HeaderText="كود الحركة"></asp:BoundField>
                                        <asp:BoundField DataField="PATIENT_NAME" HeaderText="اسم المنتفع"></asp:BoundField>
                                        <asp:BoundField DataField="Processes_Date" HeaderText="تاريخ الحركة"></asp:BoundField>
                                        <asp:BoundField DataField="Processes_Time" HeaderText="وقت الحركة"></asp:BoundField>
                                        <asp:BoundField DataField="Processes_Cilinc" HeaderText="العيادة"></asp:BoundField>
                                        <asp:BoundField DataField="Processes_SubServices" HeaderText="الخدمة"></asp:BoundField>
                                        <asp:BoundField DataField="MedicalStaff_AR_Name" HeaderText="اسم الطبيب"></asp:BoundField>
                                        <asp:BoundField DataField="Processes_Price" HeaderText="سعر الخدمة" DataFormatString="{0:C3}"></asp:BoundField>
                                        <asp:BoundField DataField="Processes_Paid" HeaderText="قيمة المنتفع" DataFormatString="{0:C3}"></asp:BoundField>
                                        <asp:BoundField DataField="Processes_Residual" HeaderText="قيمة الشركة" DataFormatString="{0:C3}"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <!-- /row -->
                        <div class="form-row justify-content-end mt-3">
                            <div class="form-group col-xs-6 col-sm-3">
                                <asp:Button ID="btn_confirm" runat="server" CssClass="btn btn-outline-dark btn-block" Text="تأكيد" />
                            </div>
                        </div>
                    </div>
                    <!-- /card-body -->
                </div>
                <!-- /card -->
            </div>
        </div>
        <!-- /row -->
    </div>

</asp:Content>
