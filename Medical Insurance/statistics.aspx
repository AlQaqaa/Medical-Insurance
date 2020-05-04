<%@ Page Title="الاستفسارات و الإحصائيات" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="statistics.aspx.vb" Inherits="Medical_Insurance.statistics" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .panel {
            max-height: 530px;
            max-width: 100%;
        }

        .scrollable {
            overflow-y: auto;
            overflow-x: auto;
        }

        .nowrap {
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <button class="btn btn-outline-secondary mt-1" type="button" data-toggle="collapse" data-target="#advancedSearch" aria-expanded="false" aria-controls="collapseExample">
        <i class="fas fa-filter"></i>بحث متقدم
    </button>

    <div class="collapse show mt-1" id="advancedSearch">
        <div class="card card-body">
            <div class="tab-pane fade show active" id="search1" role="tabpanel" aria-labelledby="home-tab">
                    <div class="row mb-2">
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList ID="ddl_companies" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="C_NAME_ARB" DataValueField="C_id" Width="100%"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS C_ID, 'جميع الشركات' AS C_Name_Arb FROM INC_COMPANY_DATA UNION SELECT C_ID, C_Name_Arb FROM [INC_COMPANY_DATA] WHERE ([C_STATE] =0)"></asp:SqlDataSource>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox ID="txt_patient_name" CssClass="form-control" runat="server" AutoCompleteType="Disabled" placeholder="اسم المنتفع"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox ID="txt_card_no" CssClass="form-control" runat="server" AutoCompleteType="Disabled" placeholder="رقم البطاقة"></asp:TextBox>
                        </div>
                    </div>
                    <!-- row -->
                    <div class="row mb-2">
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox ID="txt_emp_no" CssClass="form-control" runat="server" AutoCompleteType="Disabled" placeholder="الرقم الوظيفي"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox ID="txt_patient_code" CssClass="form-control" runat="server" AutoCompleteType="Disabled" placeholder="كود المنتفع"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList ID="ddl_doctors" CssClass="chosen-select drop-down-list form-control" runat="server" Width="100%" DataSourceID="SqlDataSource5" DataTextField="MedicalStaff_AR_Name" DataValueField="MedicalStaff_ID"></asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource5" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS MedicalStaff_ID, 'جميع الأطباء' AS MedicalStaff_AR_Name FROM Main_MedicalStaff UNION SELECT [MedicalStaff_ID], [MedicalStaff_AR_Name] FROM [Main_MedicalStaff]"></asp:SqlDataSource>
                        </div>
                    </div>
                    <!-- row -->
                    <div class="row mb-2">
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID" Width="100%">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT 0 AS clinic_id, 'جميع العيادات' AS Clinic_AR_Name FROM [MAIN_CLINIC] UNION SELECT clinic_id, Clinic_AR_Name FROM [MAIN_CLINIC]"></asp:SqlDataSource>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList ID="ddl_services" CssClass="chosen-select drop-down-list form-control" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4" DataTextField="Service_AR_Name" DataValueField="Service_ID" Width="100%">
                            </asp:DropDownList>

                            <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS Service_ID, 'جميع الأقسام' AS Service_AR_Name FROM Main_Services UNION SELECT [Service_ID], [Service_AR_Name] FROM [Main_Services] WHERE ([Service_Clinic] = @CLINIC_ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddl_clinics" PropertyName="SelectedValue" Name="CLINIC_ID" Type="Int32"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList ID="ddl_sub_service" CssClass="chosen-select drop-down-list form-control" runat="server" Width="100%" DataSourceID="SqlDataSource3" DataTextField="SubService_AR_Name" DataValueField="SubService_ID"></asp:DropDownList>

                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS SubService_ID, 'جميع الخدمات' AS SubService_AR_Name FROM Main_SubServices UNOIN SELECT [SubService_ID], [SubService_AR_Name] FROM [Main_SubServices] WHERE ([SubService_Service_ID] = @SubService_Service_ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddl_services" PropertyName="SelectedValue" Name="SubService_Service_ID" Type="Int16"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                    <!-- row -->
                    <div class="row mb-2">
                        <div class="form-group col-xs-12 col-sm-3">
                            <div class="input-group">
                                <asp:TextBox ID="txt_start_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_start_dt);" placeholder="سنه/شهر/يوم" TabIndex="6" ToolTip="تاريخ تقديم الخدمة"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                    </div>
                                </div>
                            </div>
                            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_start_dt" ID="CalendarExtender3" Format="dd/MM/yyyy" PopupButtonID="ImageButton1" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                            <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_start_dt_MaskedEditExtender" TargetControlID="txt_start_dt" ID="MaskedEditExtender3" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <div class="input-group">
                                <asp:TextBox ID="txt_end_dt" runat="server" dir="rtl" CssClass="form-control" onkeyup="KeyDownHandler(txt_end_dt);" placeholder="سنه/شهر/يوم" TabIndex="6"></asp:TextBox>
                                <div class="input-group-prepend">
                                    <div class="input-group-text">
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/Style/images/Calendar.png" Width="20px" TabIndex="100" />
                                    </div>
                                </div>
                            </div>
                            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txt_end_dt" ID="CalendarExtender2" Format="dd/MM/yyyy" PopupButtonID="ImageButton2" PopupPosition="TopLeft"></ajaxToolkit:CalendarExtender>
                            <ajaxToolkit:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" BehaviorID="txt_end_dt_MaskedEditExtender" TargetControlID="txt_end_dt" ID="MaskedEditExtender2" Mask="99/99/9999" MaskType="Date"></ajaxToolkit:MaskedEditExtender>
                        </div>
                        <div class="col-xs-12 col-sm-2">
                            <asp:DropDownList ID="ddl_payment_type" CssClass="drop-down-list form-control" runat="server">
                                <asp:ListItem Value="0">جميع طرق الدفع</asp:ListItem>
                                <asp:ListItem Value="1">خاص</asp:ListItem>
                                <asp:ListItem Value="2">تأمين</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList ID="ddl_invoice_type" CssClass="drop-down-list form-control" runat="server">
                                <asp:ListItem Value="0">الخدمات الطبية / الإيواء والعمليات</asp:ListItem>
                                <asp:ListItem Value="1">الخدمات الطبية</asp:ListItem>
                                <asp:ListItem Value="2">الإيواء والعمليات</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <!-- row -->
                    <div class="row mb-2">
                        <div class="col-xs-12 col-sm-2">
                            <asp:DropDownList ID="ddl_relation" CssClass="drop-down-list form-control" runat="server" Width="100%">
                                <asp:ListItem Value="10">صلة القرابة</asp:ListItem>
                                <asp:ListItem Value="0">المشترك</asp:ListItem>
                                <asp:ListItem Value="1">الأب</asp:ListItem>
                                <asp:ListItem Value="2">الأم</asp:ListItem>
                                <asp:ListItem Value="3">الزوجة</asp:ListItem>
                                <asp:ListItem Value="4">الابن</asp:ListItem>
                                <asp:ListItem Value="5">الابنة</asp:ListItem>
                                <asp:ListItem Value="6">الأخ</asp:ListItem>
                                <asp:ListItem Value="7">الأخت</asp:ListItem>
                                <asp:ListItem Value="8">الزوج</asp:ListItem>
                                <asp:ListItem Value="9">زوجة الأب</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-sm-2">
                            <asp:DropDownList ID="ddl_sex" CssClass="drop-down-list form-control" runat="server" Width="100%">
                                <asp:ListItem Value="0">الجنس</asp:ListItem>
                                <asp:ListItem Value="1">ذكر</asp:ListItem>
                                <asp:ListItem Value="2">أنثى</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-sm-3">
                            <asp:TextBox ID="txt_phone_num" CssClass="form-control" runat="server" AutoCompleteType="Disabled" placeholder="رقم الهاتف"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-3">
                            <asp:DropDownList ID="ddl_NAL_ID" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource6" DataTextField="Nationality_AR_Name" DataValueField="Nationality_ID"></asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString='<%$ ConnectionStrings:insurance_CS %>' SelectCommand="SELECT 0 AS Nationality_ID, 'جميع الجنسيات' AS Nationality_AR_Name FROM Main_Nationality UNION SELECT [Nationality_ID], [Nationality_AR_Name] FROM [Main_Nationality] WHERE Nationality_State=0"></asp:SqlDataSource>
                        </div>
                    </div>
                    <!-- row -->
                <hr />
                    <div class="row mb-2">
                        <div class="col-xs-12 col-sm-2">
                            <div class="form-check">
                                <asp:CheckBox ID="CheckBox1" CssClass="form-check-input form-check-label" runat="server" Text="تمت المطالبة" AutoPostBack="True" />
                            </div>

                        </div>
                        <div class="col-xs-12 col-sm-3">
                            <asp:TextBox ID="txt_invoce_no" CssClass="form-control" runat="server" AutoCompleteType="Disabled" placeholder="رقم الفاتورة" Enabled="False"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-3">
                            <asp:DropDownList ID="ddl_search_field" CssClass="drop-down-list form-control" runat="server" Width="100%" AutoPostBack="True">
                                <asp:ListItem Value="0">بحث</asp:ListItem>
                                <asp:ListItem Value="1">العمر</asp:ListItem>
                                <asp:ListItem Value="2">سعر الخدمة</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-sm-1">
                            <asp:DropDownList ID="ddl_operation" CssClass="drop-down-list form-control" runat="server" Enabled="false" Width="100%">
                                <asp:ListItem Value="<"><</asp:ListItem>
                                <asp:ListItem Value=">">></asp:ListItem>
                                <asp:ListItem Value="=">=</asp:ListItem>
                                <asp:ListItem Value="<>"><></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-sm-3">
                            <asp:TextBox ID="txt_search_val" CssClass="form-control" runat="server" AutoCompleteType="Disabled" placeholder="" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <!-- row -->                  
                </div>

            <div class="row mb-2 justify-content-end">
                <div class="col-xs-12 col-sm-3 ">
                    <asp:Button ID="btn_search" runat="server" CssClass="btn btn-outline-success btn-block mt-2" Text="بحث" />
                </div>
            </div>
            <!-- row -->
        </div>
    </div>

    <asp:Panel ID="Panel1" runat="server">
        <div class="card card-body mt-2 pb-1 border-secondary">
            <div class="row">
                <div class="col-sm-3 col-xs-6 mb-3">
                    <div class="card border-right-primary shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col ml-2">
                                    <div class="small font-weight-bold text-primary text-uppercase mb-1">عدد المنتفعين</div>
                                    <div class="h5 mb-0 font-weight-bold text-secondary">75</div>
                                </div>
                                <div class="col-auto">
                                    <i class="fas fa-user-friends fa-2x text-secondary"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3 col-xs-6 mb-3">
                    <div class="card border-right-success shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col ml-2">
                                    <div class="small font-weight-bold text-success text-uppercase mb-1">عدد الخدمات</div>
                                    <div class="h5 mb-0 font-weight-bold text-secondary">154</div>
                                </div>
                                <div class="col-auto">
                                    <i class="fas fa-stethoscope fa-2x text-secondary"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3 col-xs-6 mb-3">
                    <div class="card border-right-info shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col ml-2">
                                    <div class="small font-weight-bold text-info text-uppercase mb-1">إجمالي الخدمات</div>
                                    <div class="h5 mb-0 font-weight-bold text-secondary">765.000</div>
                                </div>
                                <div class="col-auto">
                                    <i class="fas fa-coins fa-2x text-secondary"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- row -->
        </div>
    </asp:Panel>

    <div class="row mt-2">
        <div class="col-sm-12">
            <div class="card card-body mt-2 pb-1 border-info">
                <div class="panel scrollable">
                    <asp:GridView ID="GridView1" class="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                        <Columns>
                            <asp:BoundField HeaderText="رقم المنتفع" DataField="PINC_ID">
                                <ControlStyle CssClass="hide-colum" />
                                <FooterStyle CssClass="hide-colum" />
                                <HeaderStyle CssClass="hide-colum" />
                                <ItemStyle CssClass="hide-colum" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Processes_Reservation_Code" HeaderText="كود الحركة"></asp:BoundField>
                            <asp:ButtonField DataTextField="PATIENT_NAME" HeaderText="<span data-toggle='tooltip' data-placement='top' title='يمكنك النقر على اسم المنتفع للوصول إلى الإعدادت والمعلومات الخاصة به'> اسم المنتفع <i class='fas fa-info-circle'></i></span>" CommandName="pat_name"></asp:ButtonField>
                            <asp:BoundField DataField="Processes_Date" HeaderText="تاريخ الحركة"></asp:BoundField>
                            <asp:BoundField DataField="Processes_Time" HeaderText="وقت الحركة"></asp:BoundField>
                            <asp:BoundField DataField="Processes_Cilinc" HeaderText="العيادة"></asp:BoundField>
                            <asp:BoundField DataField="Processes_SubServices" HeaderText="الخدمة"></asp:BoundField>
                            <asp:BoundField DataField="MedicalStaff_AR_Name" HeaderText="اسم الطبيب"></asp:BoundField>
                            <asp:BoundField DataField="Processes_Price" HeaderText="سعر الخدمة" DataFormatString="{0:C3}"></asp:BoundField>
                            <asp:BoundField DataField="Processes_Paid" HeaderText="قيمة المنتفع" DataFormatString="{0:C3}"></asp:BoundField>
                            <asp:BoundField DataField="Processes_Residual" HeaderText="قيمة الشركة" DataFormatString="{0:C3}"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <!-- panel -->
            </div>

        </div>
        <!-- /col -->
    </div>
    <!-- /row -->

</asp:Content>
