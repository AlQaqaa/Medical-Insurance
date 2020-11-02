<%@ Page Title="النماذج المفقودة" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="missingForms.aspx.vb" Inherits="Medical_Insurance.missingForms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card mt-2">
        <div class="card-header">تسديد الاطباء</div>
        <div class="card-body">
            <div class="row">
                <div class="col">
                    <div class="panel-scroll scrollable">

                        <asp:GridView ID="GridView1" CssClass="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="رقم النموذج" DataField="Req_Code"></asp:BoundField>
                                <asp:BoundField DataField="Processes_Date" HeaderText="تاريخ الحركة"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btn_print_one" runat="server"
                                            CommandName="printProcess"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            Text="طباعة"
                                            ControlStyle-CssClass="btn btn-secondary btn-small" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
