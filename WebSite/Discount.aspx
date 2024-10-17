<%@ Page Title="التخفيضات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="Discount.aspx.vb" Inherits="Medical_Insurance.Discount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="Style/plugins/dataTables/dataTables.bootstrap4.min.css" rel="stylesheet" />
<script src="Style/JS/jquery-3.4.1.min.js"></script>
<script src="Style/plugins/dataTables/jquery.dataTables.min.js"></script>
<script src="Style/plugins/dataTables/dataTables.bootstrap4.min.js"></script>

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

            <asp:HiddenField ID="hfewaId" runat="server" />
            <asp:HiddenField ID="hfpCodeId" runat="server" />
            <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>

            <ajaxToolkit:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp" CancelControlID="btnCancel"></ajaxToolkit:ModalPopupExtender>

            <div id="divPopUp" class="modal-dialog shadow modal-dialog-scrollable" style="width: 650px; min-height: 400px; max-height: 400px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">بيانات التخفيض</h5>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col form-group">
                                <label>قيمة التخفيض</label>
                                <asp:TextBox ID="txtAmount" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col form-group">
                                <label>سبب التخفيض</label>
                                <asp:TextBox ID="txtDesc" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-outline-success" Text="حفظ" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-secondary" Text="إغلاق" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-lg-12">
                    <div class="card">

                        <!-- /.panel-heading -->
                        <div class="card-body">

                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-sm com-tbl" AutoGenerateColumns="false" GridLines="None">
                                        <Columns>
                                            <asp:BoundField HeaderText="رقم الإيواء" DataField="EWA_Record_ID"></asp:BoundField>
                                            <asp:BoundField HeaderText="رقم المنتفع" DataField="INC_Patient_Code"></asp:BoundField>
                                            <asp:BoundField HeaderText="الاسم" DataField="NAME_ARB"></asp:BoundField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="imgbtndetails" runat="server" Text="اختيار" OnClick="imgbtndetails_Click"></asp:LinkButton>
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
