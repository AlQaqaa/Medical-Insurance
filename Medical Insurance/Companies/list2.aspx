<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="list2.aspx.vb" Inherits="Medical_Insurance.list2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>
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


    <script type="text/javascript">


        function loadtable() {
            alert('das');
            
             var typeid = $('#dplbankslist').val();
             
            //if (isNaN(parseFloat(typeid))) { typeid = 2; }

            var mytable = $("#mytable").DataTable({
                //"bFilter": true,
                //"bSort": true,
                //"bAutoWidth": true,

                destroy: true,

                
                "ajax": { url: "../checkboxselrow/server_processing.ashx?reqtype=" + typeid },

                "columns": [
                   { "data": "s" },
                   { "data": "FullName" },
                   { "data": "PhoneNumber1" },
                   { "data": "PhoneNumber2" },
                   { "data": "PhoneNumber3" },

                 {
                     "render": function (data, type, row) {
                       
                         //var h = '<a href="hospitalrequestinfo.aspx?RequestID=' + row["RequestID"] + '">details</a>';
                         //var result = '<a href="hospitalrequestinfo.aspx?RequestID=' + row["s"] + '&RecordID=' + row["FullName"] + '">' + "تفاصيل" + '</a>';
                         var result = '<a href="Movement.aspx?AC_no=' + row["PhoneNumber1"]  + '&AC_Feraa=' + row["PhoneNumber2"] + "&AC_Type=" + row["PhoneNumber3"] + '">' + "تفاصيل" + '</a>';

                         //window.location.replace("Movement.aspx" + '?AC_no=' + data[2] + "&AC_Feraa=" + data[3] + "&AC_Type=" + data[4]);

                         return result;
                     },
                     "targets": 5,
                 }


                ], columnDefs: [
                    {
                        targets: 0,
                    }
                ],
                select: {
                    style: 'multi'
                },
                "aoColumnDefs": [{ "bVisible": false, "aTargets": 2 }, { "bVisible": false, "aTargets": 3 }, { "bVisible": false, "aTargets": 4 }],
                order: [[1, 'asc']]



            });
            //$('#test_filter input').unbind();
            //$('#test_filter input').bind('keyup', function (e) {
            //    if (e.keyCode == 13) {
            //        alert("يبسش")
            //        oTable.fnFilter(this.value);
            //    }
            //});

           

         




        }

        $(document).ready(function () {
           
            //document.getElementsByName("secondsIdle").innerHTML = 2;

            alert('dasdas');
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
                    $('#dplbankslist').append('<option value="0">---يرجى الأختيار---</option>');
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


    <script src="../Style/JS/jquery-3.4.1.min.js"></script>

    <script src="../checkboxselrow/js/jquery.min.js"></script>

    <script src="../checkboxselrow/js/jquery.dataTables.min.js"></script>
    <script src="../checkboxselrow/js/dataTables.bootstrap.min.js"></script>
    <script src="../checkboxselrow/js/dataTables.checkboxes.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
          



        
    
    <%--<h3>Session Idle:&nbsp;
        <asp:Label ID="secondsIdle" runat="server" ></asp:Label>     
  </h3> --%>

    


     

    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    قوائم 
                </div>
                <!-- /.panel-heading -->
                <div class="panel-body">
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs">

                        <li class="active"><a href="#profile" data-toggle="tab">قائمة الزبائن</a>
                        </li>
                        <li><a href="#home" data-toggle="tab">حركات اليوم</a>
                        </li>
                        <%--  <li><a href="#messages" data-toggle="tab">Messages</a>
                                </li>
                                <li><a href="#settings" data-toggle="tab">Settings</a>
                                </li>--%>
                    </ul>

                    <!-- Tab panes -->

                    
                                    <center>
             نوع الحساب
       
                                    <br />


                                   
                                    <select

                                        id="dplbankslist"
                                        name="bankslist"
                                        required="required" >
                                    </select>

                                   
                                        </center>

                    <div class="tab-content">
                        <div class="tab-pane fade in active" id="profile">


                            <h4>قائمة الزبائن</h4>


                            <div class="row">
            <div class="col-sm-12">

                <%--<table style="width:100%" class="table table-striped table-bordered table-hover"  data-order="[[ 0, &quot;desc&quot; ]]"  id="dataTablesLZ">--%>
                <table style="width: 100%" class="table table-striped table-bordered table-hover" id="mytable">
                    <%--<table id="dataTablesLZ" class="table table-bordered" cellspacing="0" width="90%">--%>
                    <thead>
                        <tr>
                            <th class="auto-style2">رقم المنفع</th>
                            <th class="auto-style2">اسم المنتغع  </th>
                            <th class="auto-style2">رقم البطاقة</th>
                            <th class="auto-style2">رقم البطاقة</th>

                            <%--<th style="display: none" class="auto-style2">idx</th>--%>
                            
                            <th class="auto-style2"></th>
                        </tr>
                    </thead>






                </table>

            </div>
        </div>
                            <div class="row">
                                <div class="col-md-4">
                                </div>
                                <div class="col-lg-4">





 
                                    <br />
                                </div>
                            </div>
                      
                    </div>
                </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>
    </div>

</div> 
    
     

    </form>
</body>
</html>
