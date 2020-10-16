<%@ Page Title="تسوية الأطباء" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="dorctorsForms.aspx.vb" Inherits="Medical_Insurance.dorctorsForms" Culture="ar-LY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function openModal() {
            $('#myModal').modal('show');
        }
    </script>

    <style type="text/css">
        .HideAsl {
            display: none;
            visibility: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--   <div class="col-lg-2">
        </div>--%>

    <div class="card mt-2">
        <div class="card-header">تسديد الاطباء</div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-sm-12 col-md-4">
                    <label>الدكتور</label>
                    <asp:DropDownList ID="DDMain_MedicalStaff" runat="server" dir="rtl" Style="width: 100% !important; forecolor: #0033CC" CssClass="chosen-select drop-down-list form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-12 col-md-4">
                    <label>رقم الايصال</label>
                    <div class="input-group">
                        <asp:TextBox ID="TextBox1" runat="server" AutoCompleteType="Disabled" CssClass="form-control" Font-Size="12pt" onkeypress="return onlyNumbers(event);" AutoPostBack="True" MaxLength="18"></asp:TextBox>
                        <div class="input-group-prepend">
                            <div class="input-group-text g-text">
                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Style/images/searchicon.png" />
                            </div>
                        </div>
                        
                    </div>
                </div>
            </div>

            <div class="row">
                <asp:GridView ID="GridView1" CssClass="table table-striped table-hover" runat="server" AutoGenerateColumns="False" AllowSorting="True">
                    <Columns>
                        <%--<asp:BoundField DataField="ROW_NUMBER" HeaderText="#" SortExpression="ROW_NUMBER" >                            </asp:BoundField>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Patient_AR_Name" HeaderText="اسم المريض" SortExpression="Patient_AR_Name" />
                        <asp:BoundField DataField="req_code" HeaderText="رقم الايصال" SortExpression="req_code" />
                        <asp:BoundField DataField="Processes_Date" HeaderText="ت الايصال" DataFormatString="{0:d}" SortExpression="Processes_Date" />
                        <asp:BoundField DataField="Processes_Doctor_Price" HeaderText="القيمة" SortExpression="Processes_Doctor_Price" />
                        <asp:BoundField DataField="MedicalStaff_AR_Name" HeaderText="اسم الدكتور" SortExpression="MedicalStaff_AR_Name" />

                        <asp:BoundField DataField="esal_type" HeaderText="نوع الايصال" SortExpression="esal_type" />
                        <asp:BoundField DataField="DPID" HeaderText="DPID">
                            <ControlStyle CssClass="HideAsl" Height="1px" Width="1px" />
                            <HeaderStyle CssClass="HideAsl" />
                            <ItemStyle CssClass="HideAsl" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Processes_ID" HeaderText="Processes_ID">
                            <ControlStyle CssClass="HideAsl" Height="1px" Width="1px" />
                            <HeaderStyle CssClass="HideAsl" />
                            <ItemStyle CssClass="HideAsl" />
                        </asp:BoundField>

                        <asp:TemplateField>
                            <ItemStyle Width="20px" />
                            <ItemTemplate>

                                <asp:LinkButton ID="btn_user_edit" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditUser" ControlStyle-CssClass="btn btn-primary btn-xs" Text="تم  " />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <FooterStyle BackColor="#428BCA" ForeColor="Black" />
                </asp:GridView>
            </div>
            <div class="row">
            <div class="table-responsive">
                <asp:GridView ID="GridView2" CssClass="table table-striped table-hover" runat="server" AutoGenerateColumns="False" AllowSorting="True">
                    <Columns>

                        <asp:BoundField DataField="PayUser" HeaderText="اسم المستخدم" SortExpression="PayUser" />
                        <asp:BoundField DataField="prn_id" HeaderText="رقم امر الطباعه" SortExpression="prn_id" />
                        <asp:BoundField DataField="Paydate" HeaderText="ت الطباعه" DataFormatString="{0:d}" SortExpression="Paydate" />

                        <asp:TemplateField>

                            <ItemTemplate>

                                <asp:LinkButton ID="btn_user_edit" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditUser" ControlStyle-CssClass="btn btn-warning  " Text="طباعه  " />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <FooterStyle BackColor="#428BCA" ForeColor="Black" />
                </asp:GridView>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12 col-md-3">
                     <asp:Button ID="Button1" runat="server" UseSubmitBehavior="false" CssClass="btn btn-primary btn-block" Text="حفظ" ValidationGroup="3" Visible="False" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
