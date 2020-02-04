<%@ Page Title="حركة الشركة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companyProcesses.aspx.vb" Inherits="Medical_Insurance.companyProcesses" Culture="ar-LY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Style/JS/jquery-3.4.1.min.js"></script>
    <script src="../Style/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../Style/plugins/dataTables/dataTables.bootstrap4.min.js"></script>

    <script>
        $(document).ready(function () {
            $('table.com-tbl').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                'columnDefs': [
                  {
                      "targets": "_all",
                      "className": "text-center"
                  }],
                "scrollX": true,
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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="card mt-2">
        <div class="card-header">حركة الشركة</div>
        <div class="card-body">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-row">
                        <div class="form-group col-xs-12 col-sm-4">
                            <label for="ddl_clinics">الفترة</label>
                            <asp:DropDownList ID="ddl_time" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="1">آخر 30 يوم</asp:ListItem>
                                <asp:ListItem Value="2">آخر 7 أيام</asp:ListItem>
                                <asp:ListItem Value="3">هذا الشهر</asp:ListItem>
                                <asp:ListItem Value="4">هذه السنة</asp:ListItem>
                                <asp:ListItem Value="5">مخصص</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_start_dt">من</label>
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
                            <label for="txt_end_dt">إلى</label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                    </div>
                                </div>
                            </div>
                            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_end_dt" ID="CalendarExtender1" Format="dd/MM/yyyy" PopupButtonID="ImageButton2" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                            <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_end_dt_MaskedEditExtender" TargetControlID="txt_end_dt" ID="MaskedEditExtender1" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                        </div>
                        <div class="form-group col-xs-12 col-sm-2">
                            <label for="txt_start_dt"></label>
                            <div class="input-group">
                                <asp:Button ID="btn_apply" CssClass="btn btn-success btn-block" runat="server" Text="تطبيق" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="row mt-2">
                <div class="col-sm-12">
                    <asp:GridView ID="GridView1" class="table table-striped table-bordered com-tbl nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                        <Columns>
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
                <!-- /col -->
            </div>
            <!-- /row -->
        </div>
    </div>
    <!-- /card -->

</asp:Content>
