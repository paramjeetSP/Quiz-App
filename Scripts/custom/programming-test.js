let urlToGetTestData = $('#url-edit-test-home').text();
let urlToUpdateTestData = $('#url-update-test-home').text();
let urlToShowProgrammingTest = $('#url-show-programming-test-home').text();
let urlToDeleteProgrammingTest = $('#url-delete-test-home').text();
let dataTableOptions = {
    "order": [
        [0, "desc"]
    ],
    "columnDefs": [
       { "orderable": false, "targets":4  }// So we dont have any sort on actions
    ]
};
$(document).ready(() => {
    $('#programmingListQuiz').DataTable(
        dataTableOptions
        );

    $(".loader-container").hide();
    OnModalCloseRefreshDT();
});

function DeleteTestFunc(testId) {
    let url = urlToDeleteProgrammingTest + '/' + testId;
    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        contentType: "application/json",
        success: function (data) {
            RefreshDataTable();
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function GetTest(testId) {
    $.ajax({
        type: "GET",
        url: urlToGetTestData + '/' + testId,
        cache: false,
        success: GetTestDataSuccess,
        dataType: "json",
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function GetTestDataSuccess(data) {
    $('#TestName').val(data.TestName);
    $('#TestDuration').val(data.TestDuration);
    $('#Testid').val(data.Testid);
}

function UpdateTestFunc() {
    //$('#UpdateTest').click(() => {
    //    $(".loader-container").show();
    //});
    $(".loader-container").show();

    let testName = $('#TestName').val();
    let testDuration = $('#TestDuration').val();
    let testId = $('#Testid').val();
    let ProgTestModal = new Object();
    ProgTestModal.TestName = testName;
    ProgTestModal.TestDuration = testDuration;
    let url = urlToUpdateTestData + '/' + testId;
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            //console.log(data);
            //$(".loader-container").hide();
            UpdateTheMainTestTable();
        },
        data: JSON.stringify(ProgTestModal),
        contentType: "application/json",
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function UpdateTheMainTestTable() {
    $.ajax({
        type: "GET",
        url: urlToShowProgrammingTest,
        cache: false,
        success: function (data) {
            //console.log(data);
            $('#quizform').empty()
            $('modalId').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            $('#quizform').html(data);
            $('.close').click();
            //$('.modal-backdrop').removeClass('modal-backdrop');
            $(".loader-container").hide();
            $('#programmingListQuiz').DataTable(dataTableOptions);
        },
        dataType: "html",
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}



function DeleteTest(testId) {

    if (confirm("Are you sure?")) {
        $(".loader-container").show();
        DeleteTestFunc(testId);
    }
    return false;
}

function RefreshDataTable() {
    $.ajax({
        type: "GET",
        url: urlToShowProgrammingTest,
        cache: false,
        success: function (data) {
            console.log(data);
            $('#quizform').empty()
            $('#quizform').html(data);
            $(".loader-container").hide();
            $('#programmingListQuiz').DataTable(dataTableOptions);
            FadeInAndOut('deleted-container');
        },
        dataType: "html",
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function FadeInAndOut(selector) {
    $('.' + selector).fadeIn();
    $('.' + selector).fadeOut(2000);
}

/**
function GetTestData() {
    let url = '/Home/ShowProgramingTestPartialJson';
    $.ajax({
        type: "GET",
        url: url,
        success: GetTestDataSuccess,
    });
}

function GetTestDataSuccess(data) {
    let theDefaultContentOfButtons = '<div>' +
    '<a class="edit-test btn btn-info">Edit Test</a>' +
    '<a class="delete-test btn btn-danger">Delete Test</a>' +
    '<a class="add-questions btn btn-info">Add Questions</a>' +
    '<a class="edit-questions btn btn-info">Edit Questions</a>' +
    '<a class="show-questions btn btn-info">Show Questions</a>' +
'</div>';
    let dataSet = data;
    $('#programmingListQuiz').DataTable({
        data: dataSet.ProgList,
        "columns": [
          { "data": "Testid", "className": "hidden"},
          { "data": "TestName" },
          { "data": "TestDuration" },
          { "data": "NoofQuestion" },
          { "defaultContent": theDefaultContentOfButtons }
        ]
    });
}
**/

function OnModalCloseRefreshDT() {
    $('#QuestionAdd').on('hidden.bs.modal', function () {
        UpdateTheMainTestTable();
        console.log('modal closes');
    })
}

function AddPrgarmming() {

}