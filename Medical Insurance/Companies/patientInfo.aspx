<%@ Page Title="معلومات المشترك" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="patientInfo.aspx.vb" Inherits="Medical_Insurance.patientInfo" Culture="ar-LY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="info-page">

                <div class="row mt-1">
                    <div class="col-xs-12 col-sm-6">
                        <div class="card text-dark bg-light  h-100">
                            <div class="card-body bg-light pb-2">
                                <div class="rotate">
                                    <i class="fa fa-id-card fa-4x"></i>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="text-center">
                                            <asp:Image ID="img_pat_img" CssClass="img-fluid img-thumbnail rounded " runat="server" Width="100%" Height="140px" />
                                            <asp:Panel ID="Panel1" runat="server" Style="margin-top: 4px">
                                                <button type="button" class="btn btn-primary btn-small" data-toggle="modal" data-target="#renew_card">
                                                    تجديد صلاحية البطاقة
                                                </button>
                                            </asp:Panel>
                                            <button type="button" class="btn btn-primary btn-small mt-2" data-toggle="modal" data-target="#upload_card">
                                                رفع بطاقة
                                            </button>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <ul class="list-unstyled mb-0">
                                            <li class="text-center">
                                                <asp:Label ID="lbl_name_eng" runat="server" Text="AlQaqaa Benghuzi"></asp:Label></li>
                                            <li class="mt-1"><b>الاسم: </b>
                                                <asp:Label ID="lbl_pat_name" runat="server" Text="القعقاع بن غزي"></asp:Label></li>
                                            <li class="mt-1"><b>تاريخ الميلاد: </b>
                                                <asp:Label ID="lbl_birthdate" runat="server" Text="1990-04-17"></asp:Label></li>
                                            <li class="mt-1"><b>الرقم الوطني: </b>
                                                <asp:Label ID="lbl_nat_num" runat="server" Text="123459638574"></asp:Label></li>
                                            <li class="mt-1"><b>رقم الهاتف: </b>
                                                <asp:Label ID="lbl_phone" runat="server" Text="0926444115"></asp:Label></li>
                                        </ul>
                                    </div>
                                </div>
                                <!-- row -->
                            </div>
                        </div>
                    </div>

                    <div class="col-xs-12 col-sm-4">
                        <div class="card text-dark bg-light  h-100">
                            <div class="card-body bg-light ">
                                <div class="rotate">
                                    <i class="fa fa-info-circle fa-4x"></i>
                                </div>
                                <ul class="list-unstyled mb-0">
                                    <li><b>الشركة: </b>
                                        <asp:Label ID="lbl_company_name" runat="server" Text="شركة البريقة"></asp:Label></li>
                                    <li class="mt-1"><b>رقم البطاقة: </b>
                                        <asp:Label ID="lbl_card_no" runat="server" Text="652398"></asp:Label></li>
                                    <li class="mt-1"><b>تاريخ صلاحية البطاقة: </b>
                                        <asp:Label ID="lbl_exp_dt" runat="server" Text="2020-04-17"></asp:Label></li>
                                    <li class="mt-1"><b>الرقم الوظيفي: </b>
                                        <asp:Label ID="lbl_bage_no" runat="server" Text="35484"></asp:Label></li>
                                    <li class="mt-1"><b>صلة القرابة: </b>
                                        <asp:Label ID="lbl_const" runat="server" Text="الابن"></asp:Label></li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-2 text-center align-self-center">

                        <asp:Label ID="lbl_icon_sts" runat="server" Text="<i class='fa fa-user fa-3x'></i>"></asp:Label><br />
                        <asp:Label ID="lbl_sts" runat="server" Text=""></asp:Label><br />
                        <br />
                        <asp:Button ID="btn_change_sts" runat="server" CssClass="btn btn-outline-danger btn-block" Text="إيقاف" />
                        <asp:HyperLink ID="hl_edit" CssClass="btn btn-outline-primary btn-block" role="button" runat="server" NavigateUrl="~/Companies/EDITPATIANT.aspx">تعديل البيانات</asp:HyperLink>
                    </div>
                </div>
                <!-- row -->
                <div class="row mt-3">
                    <div class="col-xs-12 col-sm-6">
                        <div class="card bg-light mb-3">
                            <div class="card-header bg-danger text-white">
                                الخدمات المحظورة عن هذا المشترك
                                <button type="button" class="btn btn-dark btn-sm float-right" data-toggle="modal" data-target="#ban_service">حظر خدمة</button>
                            </div>
                            <div class="card-body">
                                <asp:GridView ID="GridView1" class="table table-striped" runat="server" GridLines="None" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="SER_ID" HeaderText="ر.خ">
                                            <ControlStyle CssClass="hide-colum" />
                                            <FooterStyle CssClass="hide-colum" />
                                            <HeaderStyle CssClass="hide-colum" />
                                            <ItemStyle CssClass="hide-colum" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SERV_CODE" HeaderText="رمز الخدمة"></asp:BoundField>
                                        <asp:BoundField DataField="SERVICE_NAME" HeaderText="اسم الخدمة"></asp:BoundField>
                                        <asp:BoundField DataField="NOTES" HeaderText="ملاحظات"></asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btn_block_stop" runat="server"
                                                    CommandName="stop_block"
                                                    CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                    ToolTip="إيقاف الحظر"
                                                    ControlStyle-CssClass="btn btn-link text-success btn-new"
                                                    OnClientClick="return confirm('هل أنت متأكد من إيقاف الحظر عن هذه الخدمة؟')"><i class='fas fa-lock-open'></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="Label1" runat="server" CssClass="text-center" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="col-xs-12 col-sm-6">
                        <div class="card bg-light mb-3">
                            <div class="card-header">أفراد العائلة</div>
                            <div class="card-body">
                                <asp:GridView ID="GridView2" class="table table-striped" runat="server" GridLines="None" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="PINC_ID" HeaderText="ر.م">
                                            <ControlStyle CssClass="hide-colum" />
                                            <FooterStyle CssClass="hide-colum" />
                                            <HeaderStyle CssClass="hide-colum" />
                                            <ItemStyle CssClass="hide-colum" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="C_ID" HeaderText="ر.ش">
                                            <ControlStyle CssClass="hide-colum" />
                                            <FooterStyle CssClass="hide-colum" />
                                            <HeaderStyle CssClass="hide-colum" />
                                            <ItemStyle CssClass="hide-colum" />
                                        </asp:BoundField>
                                        <asp:ButtonField DataTextField="NAME_ARB" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم المشترك للوصول إلى الإعدادت والمعلومات الخاصة به'> الاسم بالعربي <i class='fas fa-info-circle'></i></span>" CommandName="pat_name">
                                            <ControlStyle ForeColor="Black"></ControlStyle>

                                            <ItemStyle ForeColor="Black"></ItemStyle>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CONST_NAME" HeaderText="صلة القرابة"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /row -->
                <div class="row mt-2">
                    <div class="col-sm-12">
                        <div class="card bg-light mb-3">
                            <div class="card-header bg-success text-white">آخر الخدمات المقدمة لهذا المنتفع</div>
                            <div class="card-body">
                                <div class="panel-scroll scrollable">
                                    <asp:GridView ID="GridView3" class="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="Processes_Reservation_Code" HeaderText="كود الحركة"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Date" HeaderText="التاريخ"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Cilinc" HeaderText="العيادة"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_SubServices" HeaderText="الخدمة"></asp:BoundField>
                                            <asp:BoundField DataField="MedicalStaff_AR_Name" HeaderText="اسم الطبيب"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Price" HeaderText="سعر الخدمة" DataFormatString="{0:C3}"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Paid" HeaderText="قيمة المنتفع" DataFormatString="{0:C3}"></asp:BoundField>
                                            <asp:BoundField DataField="Processes_Residual" HeaderText="قيمة الشركة" DataFormatString="{0:C3}"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>
                    </div>
                    <!-- /col -->
                </div>
                <!-- /row -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Ban Services Modal -->
    <div class="modal fade" id="ban_service" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">حظر خدمة عن مستخدم</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <label for="service" class="col-sm-2 col-form-label">الخدمة</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="SubService_AR_Name" DataValueField="SubService_ID" Width="100%"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT [SubService_ID], [SubService_AR_Name] FROM [Main_SubServices] WHERE SubService_State = 0"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="service" class="col-sm-2 col-form-label">ملاحظات</label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txt_notes" class="form-control" runat="server" Rows="3" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btn_ban_service" class="btn btn-outline-success" runat="server" Text="حفظ" />
                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">إغلاق</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal renew card -->
    <div class="modal fade" id="renew_card" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="renew_cardLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="renew_cardLabel">تجديد بطاقة المنتفع</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <label for="txt_EXP_DATE">تاريخ صلاحية البطاقة</label>
                    <div class="input-group">
                        <asp:TextBox ID="txt_exp_date" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_exp_date);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                        <div class="input-group-prepend">
                            <div class="input-group-text">
                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                            </div>
                        </div>
                    </div>
                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_exp_date" ID="CalendarExtender1" Format="dd/MM/yyyy" PopupButtonID="ImageButton2" PopupPosition="BottomLeft"></ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_exp_date_MaskedEditExtender" TargetControlID="txt_exp_date" ID="MaskedEditExtender1" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btn_renew_card" CssClass="btn btn-outline-success" runat="server" Text="حفظ" />
                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">إغلاق</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Upload card Image -->
    <div class="modal fade" id="upload_card" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="renew_cardLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="upload_cardLabel">رفع صورة بطاقة المنتفع</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btn_upload_card" CssClass="btn btn-outline-success" runat="server" Text="حفظ" />
                    <button type="button" class="btn btn-outline-secondary" data-dismiss="modal">إغلاق</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
