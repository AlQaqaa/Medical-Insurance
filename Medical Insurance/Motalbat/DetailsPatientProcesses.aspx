<%@ Page Title="تفاصيل حركة المريض" Language="vb" AutoEventWireup="false" MasterPageFile="~/Motalbat/motalbat.Master" CodeBehind="DetailsPatientProcesses.aspx.vb" Inherits="Medical_Insurance.DetailsPatientProcesses" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row mt-2">
                <div class="col-sm-12">
                    <div class="card">
                        <div class="card-header">تفاصيل حركة المريض خلال فترة</div>
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-xs-12 col-md-5">
                                    <label for="ddl_companies">الشركة</label>
                                    <asp:TextBox ID="txt_company_name" runat="server" dir="rtl" CssClass="form-control" ReadOnly="True"></asp:TextBox>

                                </div>
                                <div class="form-group col-xs-12 col-md-3">
                                    <label for="ddl_companies">رقم الفاتورة</label>
                                    <asp:TextBox ID="txt_invoice_no" runat="server" dir="rtl" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-md-2">
                                    <label for="txt_start_dt">الفترة من</label>
                                    <asp:TextBox ID="txt_start_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_start_dt);" placeholder="سنه/شهر/يوم" TabIndex="6" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-md-2">
                                    <label for="txt_start_dt">إلى</label>
                                    <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6" ReadOnly="True"></asp:TextBox>
                                </div>

                            </div>
                            <!-- /form-row -->
                            <div class="form-row">
                                <div class="form-group col-xs-12 col-md-3">
                                    <label for="ddl_companies">رقم البطاقة</label>
                                    <asp:TextBox ID="txtCardNo" runat="server" dir="rtl" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-md-5">
                                    <label for="ddl_companies">اسم المنتفع</label>
                                    <asp:TextBox ID="txtName" runat="server" dir="rtl" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="form-group col-xs-12 col-md-3">
                                    <label for="ddl_companies">تصفية</label>
                                    <asp:DropDownList ID="ddlFilter" CssClass="drop-down-list form-control" runat="server" AutoPostBack="true">
                                        <asp:ListItem Value="0">الكل</asp:ListItem>
                                        <asp:ListItem Value="42">المعمل فقط</asp:ListItem>
                                        <asp:ListItem Value="18">العلاج الطبيعي فقط</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <!-- /form-row -->
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm" runat="server" Width="100%" GridLines="None" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ر.ت">
                                                <ItemTemplate>
                                                    <span>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="رقم المنتفع" DataField="Processes_ID">
                                                <ControlStyle CssClass="hide-colum" />
                                                <FooterStyle CssClass="hide-colum" />
                                                <HeaderStyle CssClass="hide-colum" />
                                                <ItemStyle CssClass="hide-colum" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubService_Code" HeaderText="كود الخدمة"></asp:BoundField>
                                            <asp:BoundField DataField="SubService_AR_Name" HeaderText="الخدمة"></asp:BoundField>
                                            <asp:BoundField DataField="Clinic_AR_Name" HeaderText="العيادة"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Date" HeaderText="التاريخ"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Price" HeaderText="السعر"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Residual" HeaderText="نسبة الشركة"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Paid" HeaderText="نسبة المستفيد"></asp:BoundField>
                                            <asp:BoundField DataField="PERSON_PER" HeaderText="النسبة">
                                                <ControlStyle CssClass="hide-colum" />
                                                <FooterStyle CssClass="hide-colum" />
                                                <HeaderStyle CssClass="hide-colum" />
                                                <ItemStyle CssClass="hide-colum" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Processes_Price" HeaderText="النسبة">
                                                <ControlStyle CssClass="hide-colum" />
                                                <FooterStyle CssClass="hide-colum" />
                                                <HeaderStyle CssClass="hide-colum" />
                                                <ItemStyle CssClass="hide-colum" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btn_print_one" runat="server"
                                                        CommandName="printProcess"
                                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        Text="طباعة الإيصال"
                                                        ControlStyle-CssClass="btn btn-primary btn-small" />

                                                    <asp:LinkButton ID="Edit" runat="server"
                                                        CommandName="editPrice"
                                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        Text="تعديل السعر"
                                                        ControlStyle-CssClass="btn btn-secondary btn-small" />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
                            <ajaxToolkit:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp"></ajaxToolkit:ModalPopupExtender>

                            <div id="divPopUp" class="modal-dialog shadow modal-dialog-scrollable" style="width: 450px">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">تعديل السعر</h5>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-12 form-group">
                                                <label>الخدمة</label>
                                                <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-12 form-group">
                                                <label>سعر الخدمة</label>
                                                <asp:TextBox ID="txtPrice" CssClass="form-control" runat="server" onKeyUp="return bar()"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="مطلوب" ControlToValidate="txtPrice" ForeColor="Red" ValidationGroup="editPrice" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 form-group">
                                                <label>قيمة الشركة</label>
                                                <asp:TextBox ID="txtCompanyPrice" CssClass="form-control" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="مطلوب" ControlToValidate="txtCompanyPrice" ForeColor="Red" ValidationGroup="editPrice" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 form-group">
                                                <label>قيمة المستفيد</label>
                                                <asp:TextBox ID="txtPatPrice" CssClass="form-control" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="مطلوب" ControlToValidate="txtPatPrice" ForeColor="Red" ValidationGroup="editPrice" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSave" CssClass="btn btn-outline-success" runat="server" Text="حفظ" ValidationGroup="editPrice" />
                                        <asp:Button ID="btnCancel" runat="server" class="btn btn-outline-secondary" Text="إغلاق" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Style/JS/jquery-3.4.1.min.js"></script>
    <script>
        function bar() {
            var patAmount = $('#<%=txtPrice.ClientID %>').val() * $('#<%=HiddenField1.ClientID %>').val() / 100
            var companyAmount = $('#<%=txtPrice.ClientID %>').val() * (100 - $('#<%=HiddenField1.ClientID %>').val()) / 100
            $('#<%=txtPatPrice.ClientID %>').val(patAmount)
            $('#<%=txtCompanyPrice.ClientID %>').val(companyAmount)
        }


    </script>
</asp:Content>
