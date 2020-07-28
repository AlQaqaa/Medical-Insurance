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
                "fixedColumns": true,
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
                    null,
                    { "searchable": false }
                    
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
   
    <div class="card mt-1 ">
        <div class="card-header bg-success text-light">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </div>
        <div class="card-body">
            <div class="form-row mb-3">
                <div class="form-group col-xs-12 col-sm-6">
                    <asp:Panel ID="main_company_panel" runat="server" Visible="False">
                        <label for="ddl_companies">الشركة</label>
                        <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id" AutoPostBack="True"></asp:DropDownList>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb + (case when (C_Level <> 0) then (select ' - ' + C_Name_Arb from [INC_COMPANY_DATA] as tbl1 where tbl1.C_ID = tbl2.C_Level) else '' end) as C_Name_Arb FROM [INC_COMPANY_DATA] as tbl2 WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <asp:GridView ID="GridView1" class="table table-striped table-bordered com-tbl nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                        <Columns>
                            <asp:BoundField HeaderText="رقم المنتفع" DataField="PINC_ID">
                                <ControlStyle CssClass="hide-colum" />
                                <FooterStyle CssClass="hide-colum" />
                                <HeaderStyle CssClass="hide-colum" />
                                <ItemStyle CssClass="hide-colum" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="رقم الشركة" DataField="C_ID">
                                <ControlStyle CssClass="hide-colum" />
                                <FooterStyle CssClass="hide-colum" />
                                <HeaderStyle CssClass="hide-colum" />
                                <ItemStyle CssClass="hide-colum" />
                            </asp:BoundField>
                            <asp:BoundField DataField="P_STATE" HeaderText="حالة المنتفع">
                                <ItemStyle BackColor="#33CC33" ForeColor="White"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="رقم البطاقة" DataField="CARD_NO"></asp:BoundField>
                            <asp:ButtonField DataTextField="NAME_ARB" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم المنتفع للوصول إلى الإعدادت والمعلومات الخاصة به'> الاسم بالعربي <i class='fas fa-info-circle'></i></span>" CommandName="pat_name"></asp:ButtonField>
                            <asp:ButtonField DataTextField="NAME_ENG" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم المنتفع للوصول إلى الإعدادت والمعلومات الخاصة به'> الاسم بالإنجليزي <i class='fas fa-info-circle'></i></span>" CommandName="pat_name"></asp:ButtonField>
                            <asp:BoundField HeaderText="تاريخ الميلاد" DataField="BIRTHDATE"></asp:BoundField>
                            <asp:BoundField HeaderText="الرقم الوظيفي" DataField="BAGE_NO"></asp:BoundField>
                            <asp:BoundField HeaderText="الرقم الوطني" DataField="NAT_NUMBER"></asp:BoundField>
                            <asp:BoundField HeaderText="رقم الهاتف" DataField="PHONE_NO"></asp:BoundField>
                            <asp:BoundField HeaderText="تاريخ صلاحية البطاقة" DataField="EXP_DATE"></asp:BoundField>
                            <asp:BoundField HeaderText="صلة القرابة" DataField="CONST_ID"></asp:BoundField>
                            
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
