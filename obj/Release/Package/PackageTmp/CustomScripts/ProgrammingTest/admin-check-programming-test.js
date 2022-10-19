/// <reference path="admin-show-all-programming-test.js" />
let QuestionID;
let UserID;
let TestID;
let ScoreBoardID;
let lastTestOptionSelectedEvent;
$(document).ready(() => {
});

function GetTheTestSubmissions(event) {
    lastTestOptionSelectedEvent = event;
    if (event.value == 0) {
        $('#test-check-content-container').html('<h3>Select Right Option</h3>');
        return;
    }
    $('.loader-container').show();
    console.log(event.value);
    let url = $('.url-ProgrammingTest-GetAllSubmissions').text();
    $.ajax({
        url: url + '?testId=' + event.value,
        type: "GET",
        success: function (data) {
            $('#test-check-content-container').html(data);
            $('.loader-container').hide();

        }
    });
}

function GetTheSubmittionToMark(userId, testId, scoreBoardId) {
    $('.loader-container').show();

    let url = $('.url-ProgrammingTest-CheckTestSub').text() + '?userId=' + userId + '&testId=' + testId + '&scoreBoardId=' + scoreBoardId;
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            $('#programming-test-submission-test').html(data);
            $('.loader-container').hide();
            QuestionID = $('.programming-submission-marking-first-question-id').text();
            TestID = $('.programming-submission-marking-first-question-test-id').text();
            UserID = $('.programming-submission-marking-first-question-user-id').text();
            ScoreBoardID = $('.programming-submission-marking-first-question-score-board-id').text();
            LogQuestionSubMarkingVars();
        },
        error: function (err) {
            $('#programming-test-submission-test').html(err.statusText);

        }
    });
}

function QuestionForMarksChanges(userId, questionId, testId, scoreBoardId) {
    QuestionID = questionId;
    UserID = userId;
    TestID = testId;
    ScoreBoardID = scoreBoardId;
    LogQuestionSubMarkingVars();
}

function LogQuestionSubMarkingVars() {
}

function SubmitMarks() {
    let totalQuestions = parseInt($('.programming-test-total-number-of-questions').text());
    let allQuestionMarked = new Array();
    let singleQuestion = new Object();
    let markIsNullOrEmpty = false;
    let exceedsMaxMarks = false;
    for (var i = 1; i <= totalQuestions; i++) {
        singleQuestion = new Object();
        singleQuestion.QuestionId = $('.programming-test-answer-question-id-' + i).text();
        singleQuestion.ScoreBoardId = $('.programming-test-answer-score-board-id-' + i).text();
        singleQuestion.TestId = $('.programming-test-answer-test-id-' + i).text();
        singleQuestion.Marks = parseInt($('.programming-test-answer-marks-' + i).val());
        if (isNaN(singleQuestion.Marks) || singleQuestion.Marks === null) {
            $('.programming-test-answer-marks-' + i + '-alert').removeClass('hidden');
            $('.programming-test-answer-marks-' + i + '-alert').fadeIn();
            markIsNullOrEmpty = true;
        } else {
            $('.programming-test-answer-marks-' + i + '-alert').addClass('hidden');
        }
        let maxMarksForQuestion = parseInt($('.programming-test-answer-marks-' + i).attr('max'));
        if (singleQuestion.Marks > maxMarksForQuestion) {
            $('.programming-test-answer--max-marks-' + i + '-alert').fadeIn();
            $('.programming-test-answer--max-marks-' + i + '-alert').removeClass('hidden');
            exceedsMaxMarks = true;
        } else {
            $('.programming-test-answer--max-marks-' + i + '-alert').addClass('hidden');
        }
        allQuestionMarked.push(singleQuestion);
    }
    if (markIsNullOrEmpty || exceedsMaxMarks) {
        return;
    }
    SubmitMarksAjax(allQuestionMarked);
}

function SubmitMarksAjax(allQuestionMarked) {
    let url = $('.url-ProgrammingTest-SubmitProgrammingMarks').text();
    $.ajax({
        url: url,
        data: JSON.stringify(allQuestionMarked),
        type: "POST",
        contentType: "application/json;",
        success: function (data) {
            RefreshSubmissionsTable();
            $('.loader-container').hide();
            $(".modal.in").modal("hide");
        }
    });
}

function RefreshSubmissionsTable() {
    GetTheTestSubmissions(lastTestOptionSelectedEvent);
}

function OnInputMarksRemoveAlert() {
    let questionNo = $(this).data('question');
    $('.programming-test-answer-marks-' + questionNo + '-alert').fadeOut();
    $('.programming-test-answer--max-marks-' + questionNo + '-alert').fadeOut();
}