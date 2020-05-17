<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Motalbat/motalbat.Master" CodeBehind="printPatientProcesses.aspx.vb" Inherits="Medical_Insurance.printPatientProcesses" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row mt-2">
                <div class="col-sm-12">
                    <div class="card">
                        <div class="card-header">مرفق حركة المريض خلال فترة</div>
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-xs-12 col-sm-5">
                                    <label for="ddl_companies">الشركة</label>
                                    <asp:TextBox ID="txt_company_name" runat="server" dir="rtl" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                    
                                </div>
                                <div class="form-group col-xs-12 col-sm-3">
                                    <label for="ddl_companies">رقم الفاتورة</label>
                                    <asp:TextBox ID="txt_invoice_no" runat="server" dir="rtl" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-sm-2">
                                    <label for="txt_start_dt">الفترة من</label>
                                    <asp:TextBox ID="txt_start_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_start_dt);" placeholder="سنه/شهر/يوم" TabIndex="6" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-sm-2">
                                    <label for="txt_start_dt">إلى</label>
                                    <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <!-- /form-row -->
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Literal ID="ltEmbed" runat="server" />
                                    
                                </div>
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
