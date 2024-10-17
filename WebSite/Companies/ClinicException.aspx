<%@ Page Title="استثناء عيادات من السقف العام" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="ClinicException.aspx.vb" Inherits="Medical_Insurance.ClinicException" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="row">
        <div class="col-sm-12">
            <div class="card mt-1">
                <div class="card-header bg-info text-white">استثناء عيادات من السقف العام </div>
                <div class="card-body">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col">
                                    <asp:GridView ID="GridView1" runat="server" class="table table-striped table-bordered" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="Clinic_ID" HeaderText="رقم الخدمة">
                                                <ControlStyle CssClass="hide-colum" />
                                                <FooterStyle CssClass="hide-colum" />
                                                <HeaderStyle CssClass="hide-colum" />
                                                <ItemStyle CssClass="hide-colum" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="مستثنى؟">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="العيادة" DataField="Clinic_AR_Name"></asp:BoundField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="form-row justify-content-end">

                                <div class="form-group col-xs-6 col-sm-3 text-left">
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block" Text="حفظ" ValidationGroup="save_data" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                                </div>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_save" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
