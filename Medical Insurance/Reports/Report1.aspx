<%@ Page Title="تقرير" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="Report1.aspx.vb" Inherits="Medical_Insurance.Report1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="card mt-1">
        <div class="card-header">
            تقرير 
        </div>
        <div class="card-body">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="form-group col-xs-12 col-sm-3">
                            <label>العيادة</label>
                            <asp:DropDownList ID="ddl_clinics" CssClass="chosen-select drop-down-list form-control" runat="server" DataSourceID="SqlDataSource2" DataTextField="Clinic_AR_Name" DataValueField="CLINIC_ID" Width="100%">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:insurance_CS %>" SelectCommand="SELECT 0 AS clinic_id, 'جميع العيادات' AS Clinic_AR_Name FROM [MAIN_CLINIC] UNION SELECT clinic_id, Clinic_AR_Name FROM [MAIN_CLINIC]"></asp:SqlDataSource>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_start_dt">من</label>
                            <asp:TextBox ID="txt_start_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="مطلوب *" ControlToValidate="txt_start_dt" ForeColor="Red" ValidationGroup="search"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-xs-12 col-sm-3">
                            <label for="txt_start_dt">إلى</label>
                            <asp:TextBox ID="txt_end_dt" class="form-control datepicker1" runat="server" placeholder="yyyy/mm/dd" AutoCompleteType="Disabled"></asp:TextBox>
                        </div>
                        <div class="form-group col-xs-12 col-sm-2">
                            <label> </label>
                            <asp:Button ID="btn_search" CssClass="btn btn-outline-info btn-block mt-2" runat="server" Text="بحث" ValidationGroup="search" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                        <div class="form-group col-xs-12 col-sm-1">
                            <label> </label>
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="2">
                                <ProgressTemplate>
                                    <div class="spinner-border text-info mt-2" role="status">
                                        <span class="sr-only">Loading...</span>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                    <!-- /row -->

                    <div class="row">
                        <div class="col-xs-12 col-sm-12">
                            <div class="panel-scroll scrollable">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" Style="text-align: center" SizeToReportContent="True" ShowExportControls="True" ShowBackButton="True" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226">
                                </rsweb:ReportViewer>

                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_search" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

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
