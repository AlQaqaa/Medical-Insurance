<%@ Page Title="إنشاء فاتورة جديدة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Motalbat/motalbat.Master" CodeBehind="newInvoice.aspx.vb" Inherits="Medical_Insurance.newInvoice" Culture="ar-LY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
                    { "searchable": false },
                    { "searchable": false },
                    { "searchable": false },
                    null,
                    null,
                    { "searchable": false },
                    { "searchable": false },
                    null,
                    null,
                    null,
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
                "paging": false,
                "ordering": false,
                "info": false,
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
                    <div class="card-header">إنشاء فاتورة جديدة</div>
                    <div class="card-body">
                        <div class="form-row">
                            <div class="form-group col-xs-12 col-sm-4">
                                <label for="ddl_companies">الشركة</label>
                                <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                            </div>
                            <div class="form-group col-xs-12 col-sm-2">
                                <label for="ddl_companies">نوع الفاتورة</label>
                                <asp:DropDownList ID="ddl_invoice_type" CssClass="chosen-select drop-down-list form-control" runat="server">
                                    <asp:ListItem Value="0">الكل</asp:ListItem>
                                    <asp:ListItem Value="1">الخدمات الطبية</asp:ListItem>
                                    <asp:ListItem Value="2">الإيواء والعمليات</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-xs-12 col-sm-3">
                                <label for="txt_start_dt">الفترة من</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txt_start_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_start_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                        </div>
                                    </div>
                                </div>
                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_start_dt" ID="CalendarExtender3" Format="dd/MM/yyyy" PopupButtonID="ImageButton1" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_start_dt_MaskedEditExtender" TargetControlID="txt_start_dt" ID="MaskedEditExtender3" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                            </div>
                            <div class="form-group col-xs-12 col-sm-3">
                                <label for="txt_start_dt">إلى</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                        </div>
                                    </div>
                                </div>
                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_end_dt" ID="CalendarExtender2" Format="dd/MM/yyyy" PopupButtonID="ImageButton2" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_end_dt_MaskedEditExtender" TargetControlID="txt_end_dt" ID="MaskedEditExtender2" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                            </div>
                        </div>
                        <!-- /form-row -->

                        <div class="form-row justify-content-end">
                            <div class="form-group col-xs-6 col-sm-3">
                                <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-info btn-block" Text="بحث" />
                            </div>

                            <div class="form-group col-xs-6 col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btn_create" runat="server" CssClass="btn btn-outline-dark btn-block" Text="إنشاء" Enabled="false" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btn_create" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                        <!-- /form-row -->

                        <hr />
                        <div class="form-row">
                            <div class="form-group col-xs-6 col-sm-3">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Visible="false" />
                            </div>
                        </div>
                        <!-- /form-row -->
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="panel scrollable">
                                    <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm com-tbl" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
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
                                            <asp:ButtonField DataTextField="PATIENT_NAME" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم المنتفع للوصول إلى الإعدادت والمعلومات الخاصة به'> اسم المنتفع <i class='fas fa-info-circle'></i></span>" CommandName="pat_name"></asp:ButtonField>
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
