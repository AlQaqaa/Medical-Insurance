<%@ Page Title="قائمة فواتير تم إرسالها للشركات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Motalbat/motalbat.Master" CodeBehind="sentInvoices.aspx.vb" Inherits="Medical_Insurance.sentInvoices" Culture="ar-LY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="motalbat-pate">
                <div class="row mt-2">
                    <div class="col-sm-12">
                        <div class="card">
                            <div class="card-header">قائمة فواتير تم إرسالها للشركات</div>
                            <div class="card-body">
                                <div class="form-row">
                                    <div class="form-group col-sm-12 col-md-4 col-lg-3">
                                        <label for="ddl_companies">الشركة</label>
                                        <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                                    </div>
                                    <div class="form-group col-sm-12 col-md-3 col-lg-2">
                                        <label for="ddl_companies">نوع الفاتورة</label>
                                        <asp:DropDownList ID="ddl_invoice_type" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="-1">الكل</asp:ListItem>
                                            <asp:ListItem Value="0">الخدمات الطبية / الإيواء والعمليات</asp:ListItem>
                                            <asp:ListItem Value="1">الخدمات الطبية</asp:ListItem>
                                            <asp:ListItem Value="2">الإيواء والعمليات</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-3">
                                        <label for="txt_start_dt">الفترة من</label>
                                        <asp:TextBox ID="txt_start_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-3">
                                        <label for="txt_start_dt">إلى</label>
                                        <asp:TextBox ID="txt_end_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-sm-12 col-md-3 col-lg-2">
                                        <label for="ddl_companies"></label>
                                        <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-info btn-block mt-2" Text="بحث" />
                                    </div>
                                    <div class="form-group col-sm-12 col-md-3 col-lg-2">
                                        <label for="ddl_companies"></label>
                                        <asp:Button ID="btn_send" runat="server" CssClass="btn btn-outline-danger btn-block mt-2" Text="الإرجاع لقائمة الفواتير" Visible="False" OnClientClick="return confirm('هل أنت متأكد إجراء هذه الخطوة؟')" />
                                    </div>
                                </div>
                                <!-- /form-row -->

                                <div class="form-row">
                                    <div class="form-group col-xs-12 col-sm-1">
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="الكل" AutoPostBack="True" Visible="False" />
                                    </div>
                                    <div class="form-group col-xs-12 col-sm-6">
                                        <asp:Label ID="lbl_msg" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <!-- /form-row -->
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                        <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField HeaderText="رقم الفاتورة" DataField="INVOICE_NO">
                                                    <ControlStyle CssClass="hide-colum" />
                                                    <FooterStyle CssClass="hide-colum" />
                                                    <HeaderStyle CssClass="hide-colum" />
                                                    <ItemStyle CssClass="hide-colum" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="تحديد">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ر.ت">
                                                    <ItemTemplate>
                                                        <span>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:ButtonField DataTextField="INVOICE_NO" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على رقم الفاتورة لمعرفة تفاصيل أكثر عنها'>رقم الفاتورة <i class='fas fa-info-circle'></i></span>" CommandName="INVOICE_DETAILES"></asp:ButtonField>
                                                <asp:BoundField DataField="COMPANY_NAME" HeaderText="الشركة"></asp:BoundField>
                                                <asp:BoundField DataField="INCOICE_CREATE_DT" HeaderText="تاريخ إنشاء الفاتورة"></asp:BoundField>
                                                <asp:BoundField DataField="DATE_FROM" HeaderText="الفترة من"></asp:BoundField>
                                                <asp:BoundField DataField="DATE_TO" HeaderText="إلى"></asp:BoundField>
                                                <asp:BoundField DataField="total_val" HeaderText="إجمالي قيمة الفاتورة" DataFormatString="{0:C3}"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btn_print1" runat="server"
                                                            CommandName="printInvoice"
                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            Text="طباعة تفاصيل الفاتورة"
                                                            ToolTip="طباعة تفاصيل الفاتورة"
                                                            ControlStyle-CssClass="btn btn-primary btn-small" />

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
            $('.datepicker1').datepicker({
                format: "dd/mm/yyyy",
                todayBtn: "linked",
                language: "ar",
                autoclose: true,
                todayHighlight: true
            });
        });
    </script>
</asp:Content>
