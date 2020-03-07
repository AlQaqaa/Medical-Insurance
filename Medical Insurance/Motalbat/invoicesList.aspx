﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Motalbat/motalbat.Master" CodeBehind="invoicesList.aspx.vb" Inherits="Medical_Insurance.invoicesList" %>

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
                            <div class="card-header">محتويات الفاتورة</div>
                            <div class="card-body">
                                <div class="form-row">
                                    <div class="form-group col-xs-12 col-sm-6">
                                        <label for="ddl_companies">الشركة</label>
                                        <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id" Enabled="False"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'يرجى اختيار شركة' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                                    </div>
                                    <div class="form-group col-xs-6 col-sm-3">
                                        <asp:Button ID="btn_print" runat="server" CssClass="btn btn-outline-secondary btn-block" Text="طباعة" />
                                    </div>
                                </div>
                                <!-- /form-row -->
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="تحديد">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ر.ت">
                                                    <ItemTemplate>
                                                        <span>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="رقم الفاتورة" DataField="INVOICE_NO"></asp:BoundField>
                                                <asp:BoundField DataField="COMPANY_NAME" HeaderText="الشركة"></asp:BoundField>
                                                <asp:BoundField DataField="INCOICE_CREATE_DT" HeaderText="تاريخ إنشاء الفاتورة"></asp:BoundField>
                                                <asp:BoundField DataField="PROCESSES_RESIDUAL" HeaderText="إجمالي قيمة الفاتورة" DataFormatString="{0:C3}"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>

                                                        <asp:Button ID="btn_print" runat="server"
                                                            CommandName="printInvoice"
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            Text="طباعة"
                                                            ToolTip="طباعة الفاتورة"
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
</asp:Content>
