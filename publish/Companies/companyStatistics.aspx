<%@ Page Title="إحصائيات الشركة" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="companyStatistics.aspx.vb" Inherits="Medical_Insurance.companyStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row mt-2">
        <div class="col-xs-12 col-sm-12">
            <div class="alert alert-info" role="alert">
                <i class="fas fa-info-circle"></i>هذه البيانات من تاريخ بداية عقد الشركة
                <asp:Label ID="lbl_start_date" runat="server" Text="01-01-2020" Font-Bold="True"></asp:Label>
                إلى تاريخ اليوم
            </div>
        </div>
    </div>
    <!-- /row -->

    <div class="row mt-2">
        <div class="col-sm-3 col-xs-6 mb-3">
            <div class="card border-right-primary shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col ml-2">
                            <div class="small font-weight-bold text-primary text-uppercase mb-1">عدد المنتفعين</div>
                            <div class="h5 mb-0 font-weight-bold text-secondary">
                                <asp:Label ID="lbl_patient_count" runat="server" Text="0"></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-user-friends fa-2x text-secondary"></i>
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
                            <div class="h5 mb-0 font-weight-bold text-secondary">
                                <asp:Label ID="lbl_service_total" runat="server" Text="0"></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-coins fa-2x text-secondary"></i>
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
                            <div class="small font-weight-bold text-success text-uppercase mb-1">متوسط المصروفات</div>
                            <div class="h5 mb-0 font-weight-bold text-secondary">
                                <asp:Label ID="lbl_expens_avg" runat="server" Text="0"></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-stethoscope fa-2x text-secondary"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-3 col-xs-6 mb-3">
            <div class="card border-right-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col ml-2">
                            <div class="small font-weight-bold text-warning text-uppercase mb-1">إجمالي المصروفت</div>
                            <div class="h5 mb-0 font-weight-bold text-secondary">
                                <asp:Label ID="lbl_expens_total" runat="server" Text="0"></asp:Label>
                            </div>
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

    <div class="row mt-2">
        <div class="col-xs-12 col-sm-6">
            <div class="card border-info shadow">
                <div class="card-body">
                    <div class="card-title text-primary"><strong>أكثر 10 منتفعين زيارة</strong></div>
                    <asp:Chart ID="Chart1" runat="server" CssClass="chart" Width="600px" Palette="BrightPastel" RightToLeft="Yes" BackImageWrapMode="Tile" IsMapEnabled="True">
                        <Series>
                            <asp:Series Name="Series1"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6">
            <div class="card border-danger shadow">
                <div class="card-body">
                    <div class="card-title text-danger"><strong>البيانات المالية</strong></div>
                    <asp:Chart ID="Chart2" runat="server" CssClass="chart" Width="600px" Palette="BrightPastel" RightToLeft="Yes" BackImageWrapMode="Tile" IsMapEnabled="True" BackGradientStyle="VerticalCenter">
                        <Series>
                            <asp:Series Name="Series1" Color="#009933" Font="Cairo-Regular" Label="عدد الزيارات" XAxisType="Secondary"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" AlignmentStyle="AxesView"></asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>
        </div>
    </div>
    <!-- /row -->

    <div class="row mt-3">
        <div class="col-xs-12 col-sm-12">
            <div class="card mb-3 border-success shadow">
                <div class="card-body">
                    <div class="card-title text-success"><strong>أكثر 10 أطباء زيارة</strong></div>
                    <asp:Chart ID="Chart4" runat="server" CssClass="chart" Width="1600px" Palette="BrightPastel" RightToLeft="Yes" BackImageWrapMode="Tile" IsMapEnabled="True" BackGradientStyle="VerticalCenter" Height="450px">
                        <Series>
                            <asp:Series Name="Series1" Color="#009933" Font="Cairo-Regular" Label="عدد الزيارات" XAxisType="Secondary"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" AlignmentStyle="AxesView"></asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>
        </div>
    </div>

    <div class="row mt-3">
        <div class="col-xs-12 col-sm-6">
            <div class="card mb-3 border-danger shadow">
                <div class="card-body">
                    <div class="card-title text-danger"><strong>أكثر 10 خدمات استخداماً</strong></div>
                    <asp:Chart ID="Chart3" runat="server" CssClass="chart" Width="600px" Palette="EarthTones" RightToLeft="Yes" BackImageWrapMode="Tile" IsMapEnabled="True">
                        <Series>
                            <asp:Series Name="Series1"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>
        </div>

        <div class="col-xs-12 col-sm-6">
            <div class="card mb-3 border-warning shadow">
                <div class="card-body">
                    <div class="card-title text-warning"><strong>أكثر 10 عيادات زيارة</strong></div>
                    <asp:Chart ID="Chart5" runat="server" CssClass="chart" Width="600px" Palette="Chocolate" RightToLeft="Yes" BackImageWrapMode="Tile" IsMapEnabled="True">
                        <Series>
                            <asp:Series Name="Series1"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>
        </div>

    </div>
    <!-- /row -->
</asp:Content>
