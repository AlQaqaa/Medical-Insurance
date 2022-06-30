<%@ Page Title="إرجاع فاتورة إيواء" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="EwaReturn.aspx.vb" Inherits="Medical_Insurance.EwaReturn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <div class="card mt-2">
        <div class="card-header">إرجاع فاتورة إيواء</div>
        <div class="card-body">
            <div class="form-row mt-3">
                
                <div class="form-group col-sm-12 col-md-4">
                    <label>رقم الفاتورة</label>
                    <div class="input-group">
                        <asp:TextBox ID="TextBox1" runat="server" AutoCompleteType="Disabled" CssClass="form-control" onkeypress="return onlyNumbers(event);" MaxLength="18"></asp:TextBox>
                        <div class="input-group-prepend">
                            <div class="input-group-text g-text">
                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Style/images/searchicon.png" />
                            </div>
                        </div>
                        
                    </div>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col">
                    <div class="panel-scroll scrollable">

                        <asp:GridView ID="GridView1" CssClass="table table-striped table-bordered table-sm nowrap w-100" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None">
                            <Columns>
                                
                                <asp:BoundField DataField="inv_id" HeaderText="رقم الفاتورة"></asp:BoundField>
                                <asp:BoundField DataField="p_name" HeaderText="اسم المنتفع"></asp:BoundField>
                                <asp:BoundField DataField="EWA_Racode_Date" HeaderText="التاريخ"></asp:BoundField>
                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btn_return" runat="server"
                                            CommandName="return"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            Text="إرجاع"
                                            ControlStyle-CssClass="btn btn-secondary btn-small"
                                            OnClientClick="return confirm('هل أنت متأكد من إرجاع هذه الفاتورة؟')" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
