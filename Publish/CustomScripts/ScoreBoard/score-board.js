let dataTable;
let dataTableOptions = {
    //initComplete: FilterDataTable,
    //"bFilter": true,
    dom: 'Bfrtip',
    buttons: [{
        extend: 'excelHtml5',
        text: 'Download Result Excel',
        title: 'Score List'
    }, {
        extend: 'pdfHtml5',
        text: 'Download Result PDF',
        title: 'Score List'
    }, {
        extend: 'csvHtml5',
        text: 'Download Result CSV',
        title: 'Score List'
    }],
    destroy: true, paging: true, "aLengthMenu": [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100]],
    "iDisplayLength": 10,
}
$(document).ready(function () {
    var count = 0;
    $('.selectpicker').selectpicker(
      {
          liveSearchPlaceholder: 'Placeholder text'
      });
    $.ajax({
        url: '/Home/UserArea',
        datatype: 'json',
        Type: "Get",
        success: function (result) {
            $('#UserFieldArea').empty();

            $.each(result, function (index, value) {
                var notdate = value.today;
                var nowDate = new Date(parseInt(notdate.substr(6)));
                var da = nowDate.toLocaleDateString();
                var time = nowDate.toLocaleTimeString();

                var FullDate = da + " " + time;

                count = count + 1;
                $('#UserFieldArea').append('<tr><td style="text-align: -webkit-center;">' + count + '</td><td style="text-align: -webkit-center;">' + FullDate + '</td><td style="text-align: -webkit-center;">' + value.user + '</td><td style="text-align: -webkit-center;">' + value.RollNo + '</td><td style="text-align: -webkit-center;">' + value.Branch + '</td><td style="text-align: -webkit-center;">' + value.topicname + '</td><td style="text-align: -webkit-center;">' + value.subname + '</td><td style="text-align: -webkit-center;">' + value.score + '</td><td style="text-align: -webkit-center;">' + value.Attempted + '</td><td style="text-align: -webkit-center;">' + value.totalTime + '</td></tr>');
            })
            dataTable = $('#userList').DataTable(dataTableOptions);
        }
    });
    $('#select-score-board-picker').on('change', function (e) {
        console.log($(this).val());
        if ($(this).val() === '0') {
            GetAllTestsSubmitted();
            return;
        }
        GetUserScoreBoardTest($(this).val());
    });
    $('#select-score-board-picker-for-branch').on('change', function (e) {
        GetUserScoreBoardByBranch($(this).val());
    });
    $('#select-score-board-picker-for-sub-topic').on('change', function (e) {
        GetUserScoreBoardBySubTopic($(this).val());
    });
    $("#select-score-board-picker-for-date-from").on("change", function () {
        GetUserScoreBoardByDate();
    });
    $("#select-score-board-picker-for-date-to").on("change", function () {
        GetUserScoreBoardByDate();
    });
    //$.fn.dataTable.ext.search.push(
    //    FilterDataTable
    //);
    GetAllBranchesData();
    GetAllSubTopics();

});

function GetAllTestsSubmitted() {
    let url = $('.url-Home-GetAllTestsSubmitted').text();
    $.ajax({
        url: url,
        datatype: 'json',
        Type: "GET",
        success: function (result) {
            $('#userList').html(result);
            $('#userList').DataTable(dataTableOptions);
        }
    });
}


function GetUserScoreBoardTest(whichtest) {
    var count = 0;
    let url = $('.url-Home-UserArea' + whichtest + 'Test').text();
    $.ajax({
        url: url,
        datatype: 'json',
        Type: "Get",
        success: function (result) {
            $('#userList').html(result);
            $('#userList').DataTable(dataTableOptions);
        }
    });
}

function GetUserScoreBoardByBranch(whichBranch) {
    let baseUrl = $('.url-Home-GetScoreBoardDataByBranch').text();
    let url = baseUrl + '?' + 'branchName=' + whichBranch;
    $.ajax({
        url: url,
        datatype: 'json',
        Type: "GET",
        success: function (result) {
            $('#userList').html(result);
            $('#userList').DataTable(dataTableOptions);
        }
    });
}

function GetAllBranchesData() {
    let url = $('.url-Home-GetAllBranchesData').text();
    $.ajax({
        url: url,
        datatype: 'json',
        Type: "GET",
        success: function (result) {
            MakeTheSelectOptions(result, '#select-score-board-picker-for-branch');
        },
        error: function (err) {

        }
    });
}

function MakeTheSelectOptions(data,selector) {
    var options = '';
    data.forEach(function (item) {
        options += '<option value="' + item.BranchName + '">' + item.BranchName + '</option>';
    });
    $(selector).html(options);
    $(selector).selectpicker('refresh');
}

function MakeTheSelectOptionsForDifferentData(data, selector) {
    var options = '';
    data.listOfProgrammintTestTopics.forEach(function (item) {
        options += '<option value="' + item.TestName + '">' + item.TestName + '</option>';
    });
    data.listOfQuizTestSubTopics.forEach(function (item) {
        options += '<option value="' + item.Name + '">' + item.Name + '</option>';
    });
    $(selector).html(options);
    $(selector).selectpicker('refresh');
}
//function FilterDataTableDraw(table) {
//    table.draw();
//}

//function FilterDataTable(settings, data, dataIndex) {
//    var min = parseInt($('#min').val(), 10);
//    var test = $('#select-score-board-picker').val();
//    var tblTest = data[5] || ''; // use data for the age column
//    //console.log(test === 0);
//    if (test === tblTest || parseInt(test) === 0) {
//        return true;
//    }
//    return false;
//}

function GetAllSubTopics() {
    let url = $('.url-Home-GetAllSubTopicsData').text();
    $.ajax({
        url: url,
        datatype: 'JSON',
        Type: "GET",
        success: function (result) {
            MakeTheSelectOptionsForDifferentData(result, '#select-score-board-picker-for-sub-topic');
        },
        error: function (err) {

        }
    });
}

function GetUserScoreBoardBySubTopic(whichSubTopic) {
    let baseUrl = $('.url-Home-GetScoreBoardDataBySubTopic').text();
    let url = baseUrl + '?' + 'subTopic=' + whichSubTopic;
    $.ajax({
        url: url,
        datatype: 'json',
        Type: "GET",
        success: function (result) {
            $('#userList').html(result);
            $('#userList').DataTable(dataTableOptions);
        }
    });
}

function GetUserScoreBoardByDate() {
    let fromDate = $('#select-score-board-picker-for-date-from').val();
    let toDate = $('#select-score-board-picker-for-date-to').val();
    if (fromDate != '' && toDate != '') {
        GetUserScoreBoardByDateAjax(fromDate, toDate);
        $('#sb-date-errors').html('');
    } else {
        if (fromDate == '') {
            $('#sb-date-errors').html('Kindly Select From Date');
        } else if (toDate == '') {
            $('#sb-date-errors').html('Kindly Select To Date');
        }
    }
}

function GetUserScoreBoardByDateAjax(from, to) {
    let baseUrl = $('.url-Home-GetScoreBoardDataByDate').text();
    let url = baseUrl + '?' + 'from=' + from + '&to=' +to;
    $.ajax({
        url: url,
        datatype: 'json',
        Type: "GET",
        success: function (result) {
            $('#userList').html(result);
            $('#userList').DataTable(dataTableOptions);
        }
    });
}

function ResetAllScoreBoardFilters() {
    $("#select-score-board-picker").val('default');
    $("#select-score-board-picker").selectpicker("refresh");
    $("#select-score-board-picker-for-sub-topic").val('default');
    $("#select-score-board-picker-for-sub-topic").selectpicker("refresh");
    $("#select-score-board-picker-for-branch").val('default');
    $("#select-score-board-picker-for-branch").selectpicker("refresh");
    $('#select-score-board-picker-for-date-to').val("");
    $('#select-score-board-picker-for-date-from').val("");
    GetAllTestsSubmitted();
}