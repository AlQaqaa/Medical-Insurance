<%@ Page Title="العيادات" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companyClinic.aspx.vb" Inherits="Medical_Insurance.companyClinic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
        <h1 class="display-4 d-none d-sm-block">
            <asp:Label ID="lbl_com_name" runat="server" Text=""></asp:Label></h1>
        <p class="lead d-none d-sm-block">
            <asp:Label ID="lbl_en_name" runat="server" Text=""></asp:Label>
        </p>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12">
                    <div class="card mt-1">
                        <div class="card-header bg-info text-white">العيادات</div>
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-xs-10 col-sm-3">
                                    <label>العيادات الغير مغطاة</label>
                                    <span data-toggle="tooltip" data-placement="top" title="لاختيار أكثر من عيادة اضغط على CTRL من لوحة المفاتيح وحدد العيادات التي تريد"><i class="fas fa-info-circle"></i></span>
                                    <asp:ListBox ID="source_list" runat="server" SelectionMode="Multiple" Width="100%" Height="350px"></asp:ListBox>

                                </div>
                                <div class="form-group col-sm-2 align-self-center text-center">
                                    <asp:Button ID="btnLeft" Text="<<" runat="server" CssClass="btn btn-danger btn-sm" OnClick="LeftClick" />
                                    <asp:Button ID="btnRight" Text=">>" runat="server" CssClass="btn btn-success btn-sm" OnClick="RightClick" />
                                </div>
                                <div class="form-group col-xs-10 col-sm-3">
                                    <label>العيادات المغطاة</label>
                                    <asp:ListBox ID="dist_list" runat="server" SelectionMode="Multiple" Width="100%" Height="350px"></asp:ListBox>
                                </div>
                                <div class="form-group col-xs-12 col-sm-4">
                                    <label>السقف العام</label>
                                    <asp:TextBox ID="txt_max_val" CssClass="form-control" runat="server" AutoCompleteType="Disabled"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال السقف" ControlToValidate="txt_max_val" ValidationGroup="save_data" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <%--<br />
                                    <br />
                                    <label>نسبة المشترك</label>
                                    <asp:TextBox ID="txt_person_per" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب إدخال نسبة المشترك" ControlToValidate="txt_person_per" ValidationGroup="save_data" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <br />
                                    <br />
                                    <label>عدد الجلسات</label><span data-toggle="tooltip" data-placement="top" title="عدد الجلسات يحدد لعيادة العلاج الطبيعي فقط، غير ذلك يمكنك تركه فارغ"> <i class="fas fa-info-circle"></i></span>
                                    <asp:TextBox ID="txt_session_count" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                                    <asp:Button ID="btn_save" runat="server" CssClass="btn btn-outline-success btn-block mt-4" Text="حفظ" ValidationGroup="save_data" />
                                </div>
                            </div>

                            <hr />
                            <div class="form-row">
                                <div class="form-group col-xs-12">
                                    <h3>العيادات المغطاة</h3>
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
