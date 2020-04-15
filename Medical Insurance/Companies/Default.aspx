<%@ Page Title="معلومات عن الشركة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="Default.aspx.vb" Inherits="Medical_Insurance._Default3" Culture="ar-LY" %>

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
                "pageLength": 10,
                "processing": true,
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

    <style>
        .form-control {
            width: 80% !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="info-page">

        <div class="row mb-3">
            <div class="col-xl-3 col-sm-6 py-2">
                <div class="card bg-success text-white h-100">
                    <div class="card-body bg-success">
                        <div class="rotate">
                            <i class="fa fa-users fa-4x"></i>
                        </div>
                        <h6 class="text-uppercase">عدد المنتفعين</h6>
                        <h1 class="display-4">
                            <asp:Label ID="lbl_pats_count" runat="server" Text="Label"></asp:Label></h1>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-sm-6 py-2">
                <div class="card text-white bg-info h-100">
                    <div class="card-body bg-info">
                        <div class="rotate">
                            <i class="fa fa-info-circle fa-4x"></i>
                        </div>
                        <ul class="list-unstyled mb-0">
                            <li>حالة الشركة: 
                                <asp:Label ID="lbl_company_sts" runat="server" Text=""></asp:Label></li>
                            
                            <li class="mt-1">بداية العقد: 
                                <asp:Label ID="lbl_start_dt" runat="server" Text="2018/10/10"></asp:Label></li>
                            <li class="mt-1">نهاية العقد: 
                                <asp:Label ID="lbl_end_dt" runat="server" Text="2020/10/10"></asp:Label></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-sm-6 py-2">
                <div class="card text-white bg-secondary h-100">
                    <div class="card-body">
                        <div class="rotate">
                            <i class="fa fa-money-check-alt fa-4x"></i>
                        </div>
                        <ul class="list-unstyled mb-0">
                            <li>طريقة الدفع: 
                                <asp:Label ID="lbl_payment_type" runat="server" Text=""></asp:Label></li>
                            <li class="mt-1">ملف الأسعار: 
                                <asp:Label ID="lbl_prices_profile" runat="server" Text="شركة الثقة"></asp:Label></li>
                            <li class="mt-1">الشركة الأم: 
                                <asp:Label ID="lbl_main_company" runat="server" Text="شركة الثقة"></asp:Label></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!--/row-->
        <hr />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row mt-3">
                    <div class="col-xs-12 col-sm-6">
                        <div class="card bg-light mb-3">
                            <div class="card-header bg-success text-white">العيادات المغطاة</div>
                            <div class="card-body">
                                <asp:GridView ID="GridView1" class="table table-striped table-bordered com-tbl nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                    <Columns>
                                        <asp:BoundField HeaderText="رقم العيادة" DataField="Clinic_ID">
                                            <ControlStyle CssClass="hide-colum" />
                                            <FooterStyle CssClass="hide-colum" />
                                            <HeaderStyle CssClass="hide-colum" />
                                            <ItemStyle CssClass="hide-colum" />
                                        </asp:BoundField>
                                        <asp:ButtonField DataTextField="Clinic_AR_Name" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم العيادة للوصول إلى الإعدادات الخاصة بها ومعرفة تفاصيل أكثر عنها'>اسم العيادة <i class='fas fa-info-circle'></i></span>" CommandName="clinic_name"></asp:ButtonField>
                                        <asp:BoundField HeaderText="سقف العيادة" DataField="MAX_VALUE" DataFormatString="{0:C3}"></asp:BoundField>
                                        <asp:BoundField HeaderText="" DataField="GROUP_CLINIC"></asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btn_clinic_stop" runat="server"
                                                    CommandName="stop_clinic"
                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    ToolTip="إيقاف العيادة"
                                                    ControlStyle-CssClass="btn btn-link text-danger btn-new"
                                                    OnClientClick="return confirm('هل أنت متأكد من إيقاف هذه العيادة')"><i class='fas fa-ban'></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="card bg-light mb-3">
                            <div class="card-header bg-danger text-white">
                                الأطباء المحظورين عن هذه الشركة
                                <button type="button" class="btn btn-dark btn-sm float-right" data-toggle="modal" data-target="#ban_service">حظر طبيب</button>
                            </div>
                            <div class="card-body">
                                <asp:GridView ID="GridView2" class="table table-striped" runat="server" GridLines="None" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="SER_ID" HeaderText="ر.خ">
                                            <ControlStyle CssClass="hide-colum" />
                                            <FooterStyle CssClass="hide-colum" />
                                            <HeaderStyle CssClass="hide-colum" />
                                            <ItemStyle CssClass="hide-colum" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MedicalStaff_AR_Name" HeaderText="اسم الطبيب"></asp:BoundField>
                                        <asp:BoundField DataField="NOTES" HeaderText="ملاحظات"></asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btn_block_stop" runat="server"
                                                    CommandName="stop_block_doctor"
                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    ToolTip="إيقاف الحظر"
                                                    ControlStyle-CssClass="btn btn-link text-success btn-new"
                                                    OnClientClick="return confirm('هل أنت متأكد من إيقاف الحظر عن هذا الطبيب؟')"><i class='fas fa-lock-open'></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="Label1" runat="server" CssClass="text-center" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>

                </div>
                <!--/row-->
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Ban Doctor Modal -->
    <div class="modal fade" id="ban_service" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">حظر طبيب عن شركة</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <label for="service" class="col-sm-2 col-form-label">الطبيب</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="MedicalStaff_AR_Name" DataValueField="MedicalStaff_ID" Width="100%"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT [MedicalStaff_ID], [MedicalStaff_AR_Name] FROM [Main_MedicalStaff] WHERE MedicalStaff_State = 0"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="service" class="col-sm-2 col-form-label">ملاحظات</label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txt_notes" class="form-control" runat="server" Rows="3" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btn_ban_doctor" class="btn btn-outline-success" runat="server" Text="حفظ" />
                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">إغلاق</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
