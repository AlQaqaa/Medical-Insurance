<%@ Page Title="الشركات الموقوفة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home/main.Master" CodeBehind="STOP_COMPANY.aspx.vb" Inherits="Medical_Insurance.STOP_COMPANY" %>
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
                "pageLength": 25,
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

    <div class="card mt-1 ">
        <div class="card-header bg-success text-light" style="font-size: x-large; font-weight: bold; color: #FF0000">الشركات الموقوفة</div>
        <div class="card-body">
            
            <asp:GridView ID="dt_GridView" class="table table-striped table-bordered com-tbl" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="C_id" HeaderText="ر.ش">
                        <ControlStyle CssClass="hide-colum" />
                        <FooterStyle CssClass="hide-colum" />
                        <HeaderStyle CssClass="hide-colum" />
                        <ItemStyle CssClass="hide-colum" />
                    </asp:BoundField>
                    <asp:ButtonField DataTextField="C_NAME_ARB" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم الشركة للوصول إلى الإعدادات الخاصة بها ومعرفة تفاصيل أكثر عنها'>الاسم بالعربي <i class='fas fa-info-circle'></i></span>" CommandName="com_name"></asp:ButtonField>
                    <asp:ButtonField DataTextField="C_NAME_ENG" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم الشركة للوصول إلى الإعدادات الخاصة بها ومعرفة تفاصيل أكثر عنها'>الاسم بالإنجليزي <i class='fas fa-info-circle'></i></span>" CommandName="com_name"></asp:ButtonField>

                    <asp:BoundField DataField="MAIN_COMPANY" HeaderText="الشركة الأم"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btn_com_active" runat="server"
                                CommandName="active_com"
                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                ToolTip="تفعيل الشركة"
                                ControlStyle-CssClass="btn btn-success btn-sm text-white"
                                OnClientClick="return confirm('هل أنت متأكد من إعادة تفعيل هذه الشركة')">تفعيل</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>

