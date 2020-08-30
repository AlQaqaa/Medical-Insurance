<%@ Page Title="قائمة المنتفعين" Language="vb" AutoEventWireup="false" MasterPageFile="~/Companies/companies.Master" CodeBehind="list1.aspx.vb" Inherits="Medical_Insurance.list1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Style/JS/jquery-3.4.1.min.js"></script>
    <style type="text/css">
        .HideAsl {
            display: none;
            visibility: hidden;
        }
    </style>
    <style type="text/css">
        .Kteba {
            text-align: left;
            direction: rtl;
        }

        .Adawat {
            text-align: right;
            direction: rtl;
            padding-right: 5px;
        }

        .Kamsa {
            position: absolute;
            left: 35px;
            top: 30px;
            font-family: Tahoma;
            font-size: 22pt;
        }
    </style>
    <%--<h3>Session Idle:&nbsp;
        <asp:Label ID="secondsIdle" runat="server" ></asp:Label>     
  </h3> --%>

    <script type="text/javascript">
        var getParams = function (url) {
	var params = {};
	var parser = document.createElement('a');
	parser.href = url;
	var query = parser.search.substring(1);
	var vars = query.split('&');
	for (var i = 0; i < vars.length; i++) {
		var pair = vars[i].split('=');
		params[pair[0]] = decodeURIComponent(pair[1]);
	}
	return params;
};

        function loadtable() {
            

            var url = window.location.href;
            if (url.indexOf('?cID=') != -1) {
                var comNumber = getParams(window.location.href);
            } else {
                var comNumber = 0;
            }

            var typeid = 0;
            if (comNumber == 0) {
                typeid = $('#dplbankslist').val();
            } else {
                typeid = 3;
                console.log(typeid);
                $('#dplbankslist').hide();
            }      

            var mytable = $("#mytable").DataTable({
                destroy: true,
                "ajax": { url: "server_processing.ashx?reqtype=" + typeid },
                "columns": [
                    { "data": "p_sts" },
                    { "data": "ar_name" },
                    { "data": "en_name" },
                    { "data": "c_no" },
                    { "data": "b_dt" },
                    { "data": "b_no" },
                    { "data": "phon_no" },
                    { "data": "exp_dt" },
                    { "data": "nat_num" },
                    { "data": "const_no" },
                ],
                columnDefs: [
                    {
                        targets: 0,
                    }
                ],
                select: {
                    style: 'multi'
                },
                //"aoColumnDefs": [  { "bVisible": false, "aTargets": 4 }],
                order: [[1, 'asc']],
                "scrollX": true,
                "pageLength": 10,
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "language": {
                    "lengthMenu": "عرض _MENU_ سجل",
                    "zeroRecords": "لا توجد بيانات متاحة.",
                    "info": "الإجمالي: _TOTAL_ منتفع",
                    "infoEmpty": "لا توجد بيانات متاحة.",
                    "infoFiltered": "(تمت تصفيتها من اصل _MAX_ سجل)",
                    "emptyTable": "لا توجد بيانات متاحة.",
                    "loadingRecords": "جاري التحميل...",
                    "processing": "جاري المعالجة...",
                    "search": "البحث: ",
                    "zeroRecords": "لا توجد بيانات مطابقة!",
                    "paginate": {
                        "first": "البداية",
                        "last": "النهاية",
                        "next": "التالي",
                        "previous": "السابق"
                    },
                    "aria": {
                        "sortAscending": ": تفعيل ليتم الترتيب التصاعدي",
                        "sortDescending": ": تفعيل ليتم الترتيب التنازلي"
                    }
                }
            });
        }
        $(document).ready(function () {
            GetList();
            loadtable();

            $("#dplbankslist").on("change", function () {

                loadtable();

            });
        });
        function GetList() {

            $.ajax({
                url: 'getcategories.asmx/getbankslist',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (response) {
                    $('#dplbankslist').append('<option value="0">---يرجى اختيار شركة---</option>');
                    for (var i = 0; i < response.d.length; i++) {
                        $('#dplbankslist').append('<option value="' + response.d[i].Shortcut_id + '">' + response.d[i].Shortcut_name + '</option>');
                    }
                },
                error: function (err) {
                    alert(err.responseText);
                }
            });
        };

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-lg-12">
            <div class="card">
                
                <!-- /.panel-heading -->
                <div class="card-body">

                    <div class="row">
                        <div class="col-sm-12 col-md-4">
                            <select
                        id="dplbankslist"
                        name="bankslist"
                        class="form-control"
                        required="required">
                    </select>
                        </div>
                    </div>
                   <br />
                    <div class="row">
                        <div class="col-sm-12">

                            <%--<table style="width:100%" class="table table-striped table-bordered table-hover"  data-order="[[ 0, &quot;desc&quot; ]]"  id="dataTablesLZ">--%>
                            <table style="width: 100%" class="table table-striped table-bordered nowrap w-100" id="mytable">
                                <%--<table id="dataTablesLZ" class="table table-bordered" cellspacing="0" width="90%">--%>
                                <thead>
                                    <tr>
                                        <th class="auto-style2">حالة المنتفع</th>
                                        <th class="auto-style2">الاسم بالعربي</th>
                                        <th class="auto-style2">الاسم بالإنجليزي</th>
                                        <th class="auto-style2">رقم البطاقة</th>
                                        <th class="auto-style2">تاريخ الميلاد</th>
                                        <th class="auto-style2">الرقم الوظيفي</th>
                                        <th class="auto-style2">رقم الهاتف</th>
                                        <th class="auto-style2">صلاحية البطاقة</th>
                                        <th class="auto-style2">الرقم الوطني</th>
                                        <th class="auto-style2">صلة القرابة</th>
                                    </tr>
                                </thead>
                            </table>

                        </div>
                    </div>
                  
                </div>

            </div>
        </div>

    </div>

</asp:Content>
