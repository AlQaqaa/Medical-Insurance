<%@ Page Title="تسوية الأطباء" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="dorctorsForms.aspx.vb" Inherits="Medical_Insurance.dorctorsForms" Culture="ar-LY" %>

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
                "paging": false,
                "ordering": false,
                "info": false,
                "columns": [
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    null,
                    null,
                    null,
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false }

                ],
                'columnDefs': [
                    {
                        "targets": "_all",
                        "className": "text-center"
                    }],
                "scrollX": true,
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
    <style>
        .panel {
            max-height: 530px;
        }

        .scrollable {
            overflow-y: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <div class="motalbat-pate">

        <div class="row mt-2">
            <div class="col-sm-12">
                <div class="card">
                    <div class="card-header">تسوية الأطباء</div>
                    <div class="card-body">
                        <div class="form-row mt-2">
                            <div class="form-group col-sm-12 col-md-3">
                                <asp:Label ID="Label1" runat="server" Text="العيادة"></asp:Label>
                                <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT 0 AS clinic_id, '- جميع العيادات -' AS Clinic_AR_Name FROM [MAIN_CLINIC] UNION SELECT clinic_id, Clinic_AR_Name FROM [MAIN_CLINIC] WHERE Clinic_State = 0"></asp:SqlDataSource>
                            </div>
                            <div class="form-group col-sm-12 col-md-3">
                                <asp:Label ID="Label3" runat="server" Text="الطبيب"></asp:Label>
                                <asp:DropDownList ID="dll_doctors" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource3" DataTextField="MedicalStaff_AR_Name" DataValueField="MedicalStaff_ID" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS [MedicalStaff_ID], '- جميع الأطباء -' AS [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] UNION SELECT [MedicalStaff_ID], [MedicalStaff_AR_Name] FROM [Main_MedicalStaff]"></asp:SqlDataSource>

                            </div>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="form-row mt-2">
                                    <div class="form-group col-xs-6 col-sm-3">
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Visible="false" />
                                    </div>
                                </div>
                                <!-- /form-row -->
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm nowrap com-tbl" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
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
                                                <asp:BoundField DataField="pros_code" HeaderText="كود الحركة"></asp:BoundField>
                                                <asp:BoundField DataField="Processes_Reservation_Code" HeaderText="كود المنتفع"></asp:BoundField>
                                                <asp:BoundField DataField="PATIENT_NAME" HeaderText="اسم المنتفع"></asp:BoundField>
                                                <asp:BoundField DataField="Processes_Date" HeaderText="تاريخ الحركة"></asp:BoundField>
                                                <asp:BoundField DataField="Processes_Time" HeaderText="وقت الحركة"></asp:BoundField>
                                                <asp:BoundField DataField="Clinic_AR_Name" HeaderText="العيادة"></asp:BoundField>
                                                <asp:BoundField DataField="SubService_AR_Name" HeaderText="الخدمة"></asp:BoundField>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /card-body -->
                </div>
                <!-- /card -->
            </div>
        </div>
        <!-- /row -->
    </div>


</asp:Content>
