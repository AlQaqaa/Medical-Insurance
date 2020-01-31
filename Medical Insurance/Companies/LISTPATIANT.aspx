<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="LISTPATIANT.aspx.vb" Inherits="Medical_Insurance.LISTPATIANT" %>

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
                "fixedColumns":   true,
                "columns": [
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    { "searchable": false },
                    null
                ],
                'columnDefs': [
                  {
                      "targets": "_all", 
                      "className": "text-center"
                  }],
                "scrollX": true,
                "pageLength": 25,
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
    <asp:Panel ID="Panel1" runat="server">

    </asp:Panel>
    <div class="card mt-1 ">
        <div class="card-header bg-success text-light">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></div>
        <div class="card-body">

            
            <asp:GridView ID="GridView1" class="table table-striped table-bordered com-tbl nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                <Columns>
                    <asp:BoundField HeaderText="رقم المنتفع" DataField="PINC_ID">
                        <ControlStyle CssClass="hide-colum" />
                        <FooterStyle CssClass="hide-colum" />
                        <HeaderStyle CssClass="hide-colum" />
                        <ItemStyle CssClass="hide-colum" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ر.ش" DataField="C_ID"></asp:BoundField>
                    <asp:BoundField HeaderText="رقم البطاقة" DataField="CARD_NO"></asp:BoundField>
                    <asp:ButtonField DataTextField="NAME_ARB" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم المنتفع للوصول إلى الإعدادت والمعلومات الخاصة به'> الاسم بالعربي <i class='fas fa-info-circle'></i></span>" CommandName="pat_name"></asp:ButtonField>
                    <asp:ButtonField DataTextField="NAME_ENG" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم المنتفع للوصول إلى الإعدادت والمعلومات الخاصة به'> الاسم بالإنجليزي <i class='fas fa-info-circle'></i></span>" CommandName="pat_name"></asp:ButtonField>
                    <asp:BoundField HeaderText="تاريخ الميلاد" DataField="BIRTHDATE"></asp:BoundField>
                    <asp:BoundField HeaderText="الرقم الوظيفي" DataField="BAGE_NO"></asp:BoundField>
                    <asp:BoundField HeaderText="الرقم الوطني" DataField="NAT_NUMBER"></asp:BoundField>
                    <asp:BoundField HeaderText="رقم الهاتف" DataField="PHONE_NO"></asp:BoundField>
                    <asp:BoundField HeaderText="تاريخ صلاحية البطاقة" DataField="EXP_DATE"></asp:BoundField>
                    <asp:BoundField HeaderText="صلة القرابة" DataField="CONST_ID"></asp:BoundField>
                    <asp:BoundField DataField="P_STATE" HeaderText="حالة المنتفع">
                        <ItemStyle BackColor="#33CC33" ForeColor="White"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
