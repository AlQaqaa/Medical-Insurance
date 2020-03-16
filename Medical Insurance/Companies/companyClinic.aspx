<%@ Page Title="العيادات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companyClinic.aspx.vb" Inherits="Medical_Insurance.companyClinic" Culture="ar-LY" %>

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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div class="row">
                <div class="col-sm-12">
                    <div class="card mt-1">
                        <div class="card-header bg-info text-white">العيادات</div>
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-xs-10 col-sm-3">
                                    <label>العيادات الغير مغطاة</label>
                                    <span data-toggle="tooltip" data-placement="top" title="لاختيار أكثر من عيادة اضغط على CTRL من لوحة المفاتيح وحدد العيادات التي تريد"><i class="fas fa-info-circle"></i></span>
                                    <asp:ListBox ID="source_list" runat="server" SelectionMode="Multiple" Width="100%" Height="350px"></asp:ListBox>

                                </div>
                                <div class="form-group col-sm-2 align-self-center text-center">
                                    <asp:Button ID="btnLeft" Text="<<" runat="server" CssClass="btn btn-danger btn-sm" OnClick="LeftClick" />
                                    <asp:Button ID="btnRight" Text=">>" runat="server" CssClass="btn btn-success btn-sm" OnClick="RightClick" />
                                </div>
                                <div class="form-group col-xs-10 col-sm-3">
                                    <label>العيادات المغطاة</label>
                                    <asp:ListBox ID="dist_list" runat="server" SelectionMode="Multiple" Width="100%" Height="350px"></asp:ListBox>
                                </div>
                                <div class="form-group col-xs-12 col-sm-4">
                                    <label>السقف العام</label>
                                    <asp:TextBox ID="txt_max_val" CssClass="form-control" runat="server" AutoCompleteType="Disabled"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال السقف" ControlToValidate="txt_max_val" ValidationGroup="save_data" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <%--<br />
                                    <br />
                                    <label>نسبة المشترك</label>
                                    <asp:TextBox ID="txt_person_per" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب إدخال نسبة المشترك" ControlToValidate="txt_person_per" ValidationGroup="save_data" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                    <br />
                                    <label>عدد الجلسات</label><span data-toggle="tooltip" data-placement="top" title="عدد الجلسات يحدد لعيادة العلاج الطبيعي فقط، غير ذلك يمكنك تركه فارغ"> <i class="fas fa-info-circle"></i></span>
                                    <asp:TextBox ID="txt_session_count" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block mt-4" Text="حفظ" ValidationGroup="save_data" />
                                </div>
                            </div>

                            <hr />
                            <div class="row">
                                <div class="col-sm-12">
                                    <h3>العيادات المغطاة</h3>
                                    <%--<asp:Literal ID="Literal1" runat="server"></asp:Literal>--%>
                                    <asp:GridView ID="GridView1" class="table table-striped table-bordered com-tbl nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None" AllowPaging="True">
                                        <Columns>
                                            <asp:BoundField HeaderText="رقم العيادة" DataField="Clinic_ID">
                                                <ControlStyle CssClass="hide-colum" />
                                                <FooterStyle CssClass="hide-colum" />
                                                <HeaderStyle CssClass="hide-colum" />
                                                <ItemStyle CssClass="hide-colum" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="اسم العيادة" DataField="Clinic_AR_Name"></asp:BoundField>
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
                    </div>
                </div>
            </div>
</asp:Content>
