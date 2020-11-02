<%@ Page Title="الرئيسية" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="Default.aspx.vb" Inherits="Medical_Insurance._Default1" %>

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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12 col-sm-12">
                    <div class="card mt-1 ">
                        <div class="card-header bg-info text-light">بحث سريع</div>
                        <div class="card-body">
                            <div class="form-row align-items-center">
                                <div class="col-sm-4 my-1">
                                    <div class="input-group">
                                        <asp:TextBox ID="txt_pat_search" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                        <div class="input-group-prepend">
                                            <asp:Button ID="btn_search" runat="server" CssClass="btn btn-info" Text="بحث" />
                                            <asp:Button ID="btn_clear" runat="server" CssClass="btn btn-dark" Text="إلغاء" Visible="False" />
                                        </div>
                                    </div>
                                    <small id="passwordHelptxt_pat_searchBlock" class="form-text text-muted" data-toggle="tooltip" data-placement="bottom" title="1- رقم البطاقة
2- الرقم الوظيفي
الاسم"><i class='fas fa-info'></i>&nbsp;دليل البحث</small>
                                </div>
                            </div>

                            <asp:Panel ID="Panel1" runat="server" Visible="False">
                                <div class="form-row">
                                    <div class="col-sm-12">
                                        <div class="panel-scroll scrollable">
                                            <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
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
                                                    <asp:BoundField HeaderText="كود المنتفع" DataField="INC_Patient_Code"></asp:BoundField>
                                                    <asp:BoundField HeaderText="اسم الشركة" DataField="C_NAME"></asp:BoundField>
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
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-xs-12 col-sm-12">
            <div class="card mt-2">
                <div class="card-header bg-success text-light">الشركات</div>
                <div class="card-body">

                    <%--<table id="mytable" class="table table-striped table-bordered" style="width: 100%">
                <thead>
                    <tr>
                        <th class="form-label">ر.ش</th>
                        <th class="form-label">الاسم بالعربي</th>
                        <th class="form-label">الاسم بالانجليزي</th>
                        <th class="form-label">الشركة الأم</th>
                        <th class="form-label" style='width: 120px; text-align: center !important'>العمليات</th>
                    </tr>
                </thead>

                <asp:Literal ID="LtlTableBody" runat="server"></asp:Literal>
            </table>
            <br />--%>
                    <asp:GridView ID="dt_GridView" class="table table-striped table-bordered table-sm com-tbl w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="C_id" HeaderText="ر.ش">
                                <ControlStyle CssClass="hide-colum" />
                                <FooterStyle CssClass="hide-colum" />
                                <HeaderStyle CssClass="hide-colum" />
                                <ItemStyle CssClass="hide-colum" />
                            </asp:BoundField>
                            <asp:BoundField DataField="contract_no" HeaderText="رقم العقد">
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
                                    <asp:LinkButton ID="btn_edit_com" runat="server"
                                        CommandName="edit_com"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        ToolTip="تعديل بيانات الشركة"
                                        ControlStyle-CssClass="btn btn-link text-primary btn-new"><i class='fas fa-edit'></i></asp:LinkButton>

                                    <asp:LinkButton ID="btn_add_pat" runat="server"
                                        CommandName="add_pat"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        ToolTip="إضافة مشتركين"
                                        ControlStyle-CssClass="btn btn-link text-success btn-new"><i class='fas fa-user-plus'></i></asp:LinkButton>

                                    <asp:LinkButton ID="btn_contract" runat="server"
                                        CommandName="new_contract"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        ToolTip="تمديد/تجديد العقد"
                                        ControlStyle-CssClass="btn btn-link text-warning btn-new"><i class='fas fa-file-signature'></i></asp:LinkButton>

                                    <asp:LinkButton ID="btn_com_stop" runat="server"
                                        CommandName="stop_com"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        ToolTip="إيقاف الشركة"
                                        ControlStyle-CssClass="btn btn-link text-danger btn-new"
                                        OnClientClick="return confirm('هل أنت متأكد من إيقاف هذه الشركة')"><i class='fas fa-ban'></i></asp:LinkButton>
                                    <asp:LinkButton ID="btn_end_contract" runat="server"
                                        CommandName="end_contract"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        ToolTip="إنهاء عقد الشركة"
                                        ControlStyle-CssClass="btn btn-link text-secondary btn-new"
                                        OnClientClick="return confirm('هل أنت متأكد من إنهاء عقد هذه الشركة')"><i class='fas fa-stop-circle'></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
