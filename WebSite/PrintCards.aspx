<%@ Page Title="طباعة البطاقات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="PrintCards.aspx.vb" Inherits="Medical_Insurance.PrintCards" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .HideAsl {
            display: none;
            visibility: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card mt-2">
                <div class="card-header">طباعة البطاقات</div>
                <div class="card-body">
                    <div class="row mb-2">

                        <div class="form-group col-md-3">
                            <label>التاريخ</label>
                            <asp:TextBox ID="txtDate" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" autocomplate="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال التاريخ" ControlToValidate="txtDate" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-3">
                            <label>رقم المتنفع</label>
                            <asp:TextBox ID="txtNo" class="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
<label>  </label>
    <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-success btn-block mt-2" Text="بحث" />
</div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col">
                            <div class="panel-scroll scrollable">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>اسم المستخدم</th>
                                            <th>التاريخ</th>
                                            <th>المنتفع</th>
                                            <th>الشركة</th>
                                            <th>البطاقة</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
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
