var editor;
var currentQuestion;
var remaining = 0;
var ProgramingTestCounter = 0;
var item1 = new Array();
var TotalMarks = 0;
let setTestCaseState = SetTestCaseState();
let getUserQuestionAnswerStatus = GetUserQuestionAnswerStatus();
var SecretOptionLength = 0;
$(document).ready(function () {
    $('.led-yellow').hide();
   

    editor = CodeMirror.fromTextArea($('.textarea-code-editor')[0], {
        lineNumbers: true,
        mode: 'application/json',
    });
    $(".runCode").click(function () {
        GetTheTestResults(editor.getValue());
    });

    $(".questionPerSubmit").click(function () {
        var confirmSubmit = confirm("Warning! Answer once submitted cannot be changed! Are you sure want to submit your answer?");
        if (confirmSubmit === true) {
            setTestCaseState.resetTestCaseState();
            var Question_ID = document.getElementsByClassName("questionPerSubmit")[0].id;
            var marks = $('#lable-desc-' + Question_ID + '').text();
            evalutateProgram(editor.getValue(), Question_ID);
        }
    });

    $('#finishTest').click(() => {
        Swal.fire({
            title: 'Are you sure?',
            //position: 'top-end',
            type: 'comfirm',
            showCancelButton: true,
            //confirmButtonColor: '#DD6B55',
            confirmButtonText: 'Yes!',
            cancelButtonText: 'No.'
        }).then((result) => {
            if (result.value) {
                //debugger;
                let url = $('.url-ProgrammingTest-TestSubmitted').text();
                SubmitTestDuration();
                PerformTasksBeforeChangingTheQuestion();
                CalculateSumOFMarks();
                //window.location.href = url;
                TestSubmitted();
            } else {
                // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
            }
        });
    });

    LanguageChanges($('#testLanguage')[0]);

    SetTheQuestionVarOnLoad();
    let testDuration = parseInt($('.programming-test-duration').text());
    //debugger;
    countdown(testDuration * 60);

    UserRefreshingInTest();
    //$(window).on("unload", function (e) {

    //});
    //window.onbeforeunload = function () {
    //    let url = $('.url-Home-TakeQuiz').text();

    //    window.location.href = url;
    //    return "Dude, are you sure you want to refresh? Think of the kittens!";
    //}
    var itemST = new Array();
    $('.SampleTC_1 td:first-child').each(function () {
        console.log($(this).text());
        itemST.push($(this).text());
        var SmapletestCase = itemST[0];
        $('.test-input').text(SmapletestCase);
    });


    $('.disabledAnswer').bind('keydown', function (e) {
        //debugger;
        e.preventDefault();

    });
});

function TestSubmitted() {
    Swal.fire({
        text: 'Test Submitted',
        icon: "success"
    }).then((result) => {
        if (result.value) {
            let url = $('.url-Home-TakeQuiz').text();
            window.location.href = url;
        }
    });
}

function GetTheTestResults(value) {
    $('.led-yellow').show();
    let theProgram = $(".code-1").val();
    let theLanguageNum = $('#testLanguage').val();
    let testInput = $('.test-input').val();
    let compilerArgs = "";
    if (parseInt(theLanguageNum) === 27) {
        var plus = encodeURIComponent('+');
        compilerArgs = "-Wall -std=c++14 -stdlib=libc++ -O2 -o a.out source_file.cpp";
        let urlForCPlu = "https://rextester.com/rundotnet/api?LanguageChoice=" + theLanguageNum + "&Program=" + value + "&Input=" + testInput + "&CompilerArgs=" + compilerArgs;

        RexTestForCPlus(urlForCPlu);
        return;
    }
    let filteredValue = CleanTheProgram(value);
    // Example Param Object
    //var to_compile = {
    //    "LanguageChoice": "1",
    //    "Program": $(".code-1").val(),
    //    "Input": "",
    //    "CompilerArgs": ""
    //};
    let url = "https://rextester.com/rundotnet/api?LanguageChoice=" + theLanguageNum + "&Program=" + filteredValue + "&Input=" + testInput + "&CompilerArgs=" + compilerArgs;
    console.log(url);
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        //dataType: 'jsonp',
        success: function (data) {
            $('.led-yellow').hide();
            if (data.Result) {
                $('.test-output').html(data.Result);
            } else {
                $('.test-output').html(data.Errors);
            }
        }
    });
}

function RexTestForCPlus(url) {
    let cleanUrl = CleanUrlForCPlus(url);
    console.log(cleanUrl);
    $.ajax({
        url: cleanUrl,
        type: "POST",
        dataType: "json",
        success: function (data) {
            $('.led-yellow').hide();
            if (data.Result) {
                $('.test-output').html(data.Result);
            } else {
                $('.test-output').html(data.Errors);
            }
        }
    });
}


function CleanUrlForCPlus(url) {
    let regex = new RegExp("{[\r\n] +", "gm");
    let regex2 = new RegExp("[\r\n] +}", "gm");
    let hashRegex = new RegExp("#", "gm");
    let openCurlyDone = url.replace(regex, "{\n");
    let closingCurlyDone = openCurlyDone.replace(regex2, "\n}");
    let hashDone = closingCurlyDone.replace(hashRegex, '%23');
    let lessThanReplaced = hashDone.replace(/</gm, '%3C');
    let greaterThanReplaced = lessThanReplaced.replace(/>/gm, '%3E');
    let plus = greaterThanReplaced.replace(/\+/gm, '%2B');
    let lineFeedWithInt = plus.replace(/\n/gm, '%0A');
    return lineFeedWithInt;
}

function CleanTheProgram(val) {
    //debugger;
    let regex = new RegExp("{[\r\n] +", "gm");
    let regex2 = new RegExp("[\r\n] +}", "gm");
    let hashRegex = new RegExp("#", "gm");
    let semiColonRegex = new RegExp(';', 'gm');
    let andOperator = new RegExp('&', 'gm');
    //let exclamation
    let openCurlyDone = val.replace(regex, "{\n");
    let closingCurlyDone = openCurlyDone.replace(regex2, "\n}");
    let hashDone = closingCurlyDone.replace(hashRegex, '%23');
    let andOperatorDone = hashDone.replace(andOperator, '%26');
    let lessThanReplaced = andOperatorDone.replace(/</gm, '%3C');
    let greaterThanReplaced = lessThanReplaced.replace(/>/gm, '%3E');
    let plus = greaterThanReplaced.replace(/\+/gm, '%2B');
    let lineFeedWithInt = plus.replace(/\n/gm, '%0A');
    //console.log(val.match(/;/gm));
    //let semiColon = hashDone.replace(semiColonRegex, '%3B');
    return lineFeedWithInt;
}

function GetAllLanguagesAndRexTesterCodes() {
    let theList = new Array();
    let theObject = new Object();
    theObject.slug = "c-sharp";
    theObject.rexTesterLanguageNumber = 1;
    theList.push(theObject);
    theObject = new Object();
    theObject.slug = "javascript";
    theObject.rexTesterLanguageNumber = 17;
    theList.push(theObject);
    theObject = new Object();
    theObject.slug = "node-js";
    theObject.rexTesterLanguageNumber = 23;
    theList.push(theObject);
    theObject = new Object();
    theObject.slug = "php";
    theObject.rexTesterLanguageNumber = 8;
    theList.push(theObject);
    theObject = new Object();
    theObject.slug = "swift";
    theObject.rexTesterLanguageNumber = 37;
    theList.push(theObject);
    theObject = new Object();
    theObject.slug = "java";
    theObject.rexTesterLanguageNumber = 4;
    theList.push(theObject);
    theObject = new Object();
    theObject.slug = "cpp";
    theObject.rexTesterLanguageNumber = 27;
    theList.push(theObject);
    return theList;
}

function LanguageChanges(event) {
    let langVal = event.value;
    let allLanguages = GetAllLanguagesAndRexTesterCodes();
    for (var i = 0; i < allLanguages.length; i++) {
        let langNum = allLanguages[i].rexTesterLanguageNumber;
        let langSelected = parseInt(langVal);
        let theLangSlug = '.sample-program-for-' + allLanguages[i].slug;
        if (langNum === langSelected) {
            editor.setValue($(theLangSlug).text());
            return;
        }
    }
}

function CurrentQuestionChanges(event, counter, currentquestionID) {
    $('.loader-container').show();
    var itemSTS = new Array();
    if (currentQuestion !== null && currentQuestion !== undefined) {
        $('.SampleTC_' + counter + ' td:first-child').each(function () {
           
            console.log($(this).text());
            itemSTS.push($(this).text());
            var SmapletestCase = itemSTS[0];
            $('.test-input').text(SmapletestCase);
        });
        PerformTasksBeforeChangingTheQuestion(currentquestionID);
    }
    currentQuestion = $(event).data('ques-id');
    GetTheComingQuestionData(currentQuestion, currentquestionID);
    $('.questionPerSubmit').attr('id', currentQuestion);
    $('.test-output').text('Output');
}

function GetTheComingQuestionData(currentQuestion, currentquestionID) {
    //debugger;
    let url = $('.url-ProgrammingTest-QuestionAnswerSubmitted').text();
    $('.loader-container').show();
    //
    let ScoreBoardID = $('#model-score-board-id').text();
    let theQuestionToQueryFor = new Object();
    theQuestionToQueryFor.QuestionID = currentQuestion;
    theQuestionToQueryFor.ScoreID = ScoreBoardID;
    $.ajax({
        url: url,
        type: "GET",
        data: theQuestionToQueryFor,
        success: function (data) {
            console.log(data);
            $('.loader-container').show();
            $('.programming-test-language').val(data.ProgLanguageId);
            editor.setValue(MakeTheAnswerUIFriendly(data.Answer));
            console.log('After Loader');
            setTimeout(function () { $('.loader-container').hide(); }, 1500);

            
        },
        error: function (err) {
            $('.loader-container').hide();
            console.log(err);
        }
    });
}

function MakeTheAnswerUIFriendly(answerText) {
    let lessThanReplaced = answerText.replace(/&lt;/g, '<');
    let greaterThanReplaced = lessThanReplaced.replace(/&gt;/g, '>');
    return greaterThanReplaced;
}

function SetTheQuestionVarOnLoad() {
    let theEle = $('.onload-the-first-question');
    CurrentQuestionChanges(theEle);
}

function PerformTasksBeforeChangingTheQuestion(currentquestionID) {
    let StudentID = $('#model-student-id').text();
    let TestID = $('#model-test-id').text();
    let TestName = $('#model-test-name').text();
    let TestDuration = $('#model-test-duration').text();
    let ScoreBoardID = $('#model-score-board-id').text();
    let TimeTaken = $('#model-test-id').text();
    let TestLanguageId = $('.programming-test-language').val();
    let QuestionsList = new Array();
    let Question = new Object();
    Question.QuestionID = currentQuestion;
    Question.QuestionDescription = $('.question-desc-' + currentQuestion).text();
    Question.Answer = MakeTheAnswerSubmittable(editor.getValue());
    Question.ProgLanguageId = TestLanguageId;
    QuestionsList.push(Question);
    let theSubmittionObject = {
        "StudentID": StudentID,
        "TestID": TestID,
        "TestName": TestName,
        "TestDuration": TestDuration,
        "ScoreBoardID": ScoreBoardID,
        "QuestionsList": QuestionsList,
    };
    SubmittingTheAnswerAjaxFunc(theSubmittionObject, currentquestionID);
}

function MakeTheAnswerSubmittable(answerText) {
    let lessThanReplaced = answerText.replace(/</g, '&lt;');
    let greaterThanReplaced = lessThanReplaced.replace(/>/g, '&gt;');
    return greaterThanReplaced;
}

function SubmittingTheAnswerAjaxFunc(theSubmittionObject, currentquestionID) {
   // $('.loader-container').show();

    let url = $('.url-ProgrammingTest-SubmitOrUpdateQuestionAnswer').text();
    url = url + '?currentID=' + currentquestionID;
    $.ajax({
        url: url,
        type: "POST",
        data: theSubmittionObject,
        success: function (result) {
            console.log('SubmittingTheAnswerAjaxFunc');
            console.log(result.RecordId);
            console.log(result.IsFinalSubmit);
            if (result.IsFinalSubmit) {
                $('.submitButton').prop('disabled', true);
                $('#testLanguage').prop('disabled', true);
               // $('.CodeMirror').attr('onkeydown', 'return disableEditor(event)');
                editor.setOption("readOnly", true);
                $('.CodeMirror-cursors').css("display", "none");
                $('.CodeMirror').css("background-color", "rgb(235, 235, 228)");
                $("[data-ques-id='" + currentQuestion + "']").css("background-color", "#76de76");
            }
            else {
                $('.submitButton ').prop('disabled', false);
                $('#testLanguage').prop('disabled', false);
                //$('.CodeMirror').attr('onkeydown', '');
                editor.setOption("readOnly", false);
                $('.CodeMirror-cursors').css("display", "");
                $('.CodeMirror').css("background-color", "rgba(0,0,0,0)");
                $("[data-ques-id='" + currentQuestion + "']").css("background-color", "#f75050");
            }
            $('.loader-container').hide();

        },
        error: function (err) {
            console.log(err);
        }
    });
}

function SubmitTheQuestionAnswer(questionId) {

}
function countdown(seconds) {

    // current timestamp.
    var now = new Date().getTime();
    var target = new Date(now + seconds * 1000);
    // update frequency; note, this is flexible, and when the tab is
    // inactive, there are no guarantees that the countdown will update
    // at this frequency.
    var update = 500;

    var int = setInterval(function () {

        // current timestamp
        var now = new Date();
        // remaining time, in seconds

        if (remaining >= 0) {
            remaining = (target - now) / 1000;
        }
        else {

            clearInterval(int);
            displayScore();
            return;
        }

        //today = new Date();
        var nowdate = new Date();

        var hrs = ("0" + nowdate.getHours()).slice(-2);
        var min = ("0" + nowdate.getMinutes()).slice(-2);
        var sec = ("0" + nowdate.getSeconds()).slice(-2);
        var day = ("0" + nowdate.getDate()).slice(-2);
        var month = ("0" + (nowdate.getMonth() + 1)).slice(-2);
        today = now.getFullYear() + "/" + (month) + "/" + (day) + " " + (hrs) + ":" + (min) + ":" + (sec);


        // format
        var minutes = ~~(remaining / 60);
        var seconds = ~~(remaining % 60);

        //var nowMinutes = ~~(tnow / 60);
        //var nowSeconds = ~~(tnow % 60);
        //totalTime = ""+nowMinutes + ":" + nowSeconds;

        // display
        document.getElementById("countdown").innerHTML = format(minutes) + ":" + format(seconds);
        //console.log(remaining);
        if (minutes === 10) {
            $('.blink-two').removeClass('hidden');
        }
        if (remaining <= 1) {
            let url = $('.url-ProgrammingTest-TestSubmitted').text();
            PerformTasksBeforeChangingTheQuestion();
            CalculateSumOFMarks();
            window.location.href = url;
        }
    }, update);
}

function format(num) {
    return num < 10 ? "0" + num : num;
}

function SubmitTestDuration() {
    var minutes = ~~(remaining / 60);
    var seconds = ~~(remaining % 60);

    //var nowMinutes = ~~(tnow / 60);
    //var nowSeconds = ~~(tnow % 60);
    //totalTime = ""+nowMinutes + ":" + nowSeconds;

    // display

    let url = $('.url-ProgrammingTest-SubmitTestDuration').text();
    let dataObj = new Object();
    dataObj.duration = format(minutes) + ":" + format(seconds);
    dataObj.scoreBoardId = $('#model-score-board-id').text();
    $.ajax({
        url: encodeURI(url),
        type: "POST",
        data: dataObj,
        success: function (data) {
            $('.led-yellow').hide();
            if (data.Result) {
                $('.test-output').html(data.Result);
            } else {
                $('.test-output').html(data.Errors);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function UserRefreshingInTest() {
    if (window.performance) {
        console.info("window.performance works fine on this browser");
    }
    if (performance.navigation.type == 1) {
        let url = $('.url-Home-TakeQuiz').text();

        window.location.href = url;
    } else {
        console.info("This page is not reloaded");
    }
}

function evalutateProgram(value,questionID) {
    /** NOT APPLICABLE
     * 
    anyTestCaseFailed has 3 states
    0 :- means there is no result from the current request
    1 :- means the test case has passed
    2 :- means the test case has failed
    **/
    //debugger;
    $('.loader-container').show();
    SecretOptionLength = $('.QI_' + questionID + '').length;
    let totalMarksOfAllQuestion = new Array();
    let questionObject = new Object();
    questionObject.QuestionID = questionID;
    questionObject.Marks = 0;

    let requestTimeFunc = CalculateAndReturnRequestTime();

    let QuestionIterator = 1;
    let currentDateSeconds = new Date().getSeconds();
    let prevQuesIterator = 0;
    //while (QuestionIterator <= +SecretOptionLength) {

    //    let anyTestCaseStatus = 0;
    //    if (prevQuesIterator === QuestionIterator) {
    //        continue;
    //    }
    //    prevQuesIterator = QuestionIterator;
    //    let scheduleRequestTime = requestTime();
    //    let requestTimeEnd = currentDateSeconds + scheduleRequestTime;
    //    let theCurrentTimeToCheckAgainstTheRequestEndTime = new Date().getSeconds();
    //    setTimeout(() => {
    //        var input = $('.SCInputID_' + questionID + '_' + QuestionIterator + '').text();
    //        var output = $('.SCOutputID_' + questionID + '_' + QuestionIterator + '').text();
    //        var marks = $('.lable-desc-' + questionID + '').val();
    //        anyTestCaseStatus = CalculateMarks(value, marks, input, output, questionID);
    //    }, scheduleRequestTime);
    //    if (theCurrentTimeToCheckAgainstTheRequestEndTime >= requestTimeEnd) {// Means scheduled request has executed
    //        if (anyTestCaseStatus === 1) {
    //            QuestionIterator += 1;
    //        } else if (anyTestCaseStatus === 2) {

    //        } else {

    //        }
    //    }
    for (var i = 1; i <= SecretOptionLength; i++) {
        let requestTime = requestTimeFunc();
        setTimeout((i) => {
            ToGatherInfoThenCalculateMarks(i,questionID,value);
        }, requestTime,i);
    }
    
    if (!setTestCaseState.getTestCaseState()) {
        SwalForSuccessFullyPassing();
    }
}
$(document).ajaxComplete(function () {
    console.log('All ajax request have completed');
});
function ToGatherInfoThenCalculateMarks(j, questionID,value) {
    var input = $('.SCInputID_' + questionID + '_' + j + '').text();
    var output = $('.SCOutputID_' + questionID + '_' + j + '').text();
    var marks = $('#lable-desc-' + questionID + '').text();
    var TestCaseID = $(".SCTestID_" + questionID + "_" + j).text();
    CalculateMarks(j, value, marks, input, output, questionID, TestCaseID);
}

function SetTestCaseState() {
    let testCaseFailed = false;
    return {
        setTestCaseAsFailed: function () {
            testCaseFailed = true;
        },
        getTestCaseState: function () {
            return testCaseFailed;
        },
        resetTestCaseState: function () {
            testCaseFailed = false;
        }
    };
}

function PerformSecretInputChecking(i) {

}
function SwalForSuccessFullyPassing() {
    swal({
        text: 'Test Submitted',
        icon: "success"
    }).then((result) => {
        if (result.value) {
            let url = $('.url-Home-TakeQuiz').text();
            window.location.href = url;
        }
    });
}

function GetUserQuestionAnswerStatus() {
    let arrOfQuestionMarks = new Array();
    let arrOfSecretTestCases = new Array();
    let FinalQuestionObject = new Object();
    let QuestionDetailsObject = new Object();
    //let TestCaseDetailsObject = new Object();
    return {
        //SetQuestionData: function (QuestionId, TestCaseFailure,answerSubmitted,Marks) {
        //    QuestionObject = new Object();
        //    QuestionObject.QuestionId = QuestionId;
        //    QuestionObject.TestCases = ListOfTestCases;
        //    QuestionObject.TestCases = ListOfTestCases;
        //    QuestionObject.AnswerSubmitted = answerSubmitted;
        //    QuestionObject.Marks = Marks;
        //    arrOfQuestionMarks.push(QuestionObject);
        //},

        //

        SetQuestionData: function (questionID, testCaseStatus, answer, QuestionMarksAsPerStatus, TestCaseID) {
            //QuestionObject = new Object();
            let TestCaseDetailsObject = new Object();
            //QuestionObject.QuestionId = QuestionId;
            //QuestionObject.TestCases = ListOfTestCases;
            //QuestionObject.TestCases = ListOfTestCases;
            //QuestionObject.AnswerSubmitted = answerSubmitted;
            //QuestionObject.Marks = Marks;
            //if (arrOfQuestionMarks)
            //arrOfQuestionMarks.push(QuestionObject);
            let TestLanguageId = $('.programming-test-language').val();
            let StudentID = $('#model-student-id').text();
            let ScoreBoardID = $('#model-score-board-id').text();
            let TestID = $('#model-test-id').text();
            if (QuestionDetailsObject.questionId !== questionID) {
                QuestionDetailsObject.questionId = questionID;
                //QuestionDetailsObject.answer = MakeTheAnswerSubmittable(answer);
                QuestionDetailsObject.answer = MakeTheAnswerSubmittable(editor.getValue())
                QuestionDetailsObject.QuestionMarksAsPerStatus = QuestionMarksAsPerStatus;
                QuestionDetailsObject.TestLanguageId = TestLanguageId;
                QuestionDetailsObject.StudentID = StudentID;
                QuestionDetailsObject.ScoreBoardID = ScoreBoardID;
                QuestionDetailsObject.testID = TestID;
            }
            if (QuestionDetailsObject.questionId === questionID) {
                if (QuestionDetailsObject.QuestionMarksAsPerStatus !== 0 && QuestionMarksAsPerStatus === 0) {
                    QuestionDetailsObject.QuestionMarksAsPerStatus = QuestionMarksAsPerStatus;
                }
            }
            TestCaseDetailsObject.TestCaseID = TestCaseID;
            TestCaseDetailsObject.testCaseStatus = testCaseStatus;
            TestCaseDetailsObject.questionId = questionID;
            arrOfSecretTestCases.push(TestCaseDetailsObject);
        },

        GetQuesData: function (QuestionId) {
            for (var i = 0; i < arrOfQuestionMarks.length; i++) {
                if( arrOfQuestionMarks[i].QuestionId === QuestionId){
                    return arrOfQuestionMarks[i];
                }
            }
        },
        GetAllQuestionArray: function () {
            return arrOfQuestionMarks;
        },
        SubmitEvaluatedQuestions: function () {
            //let url = urlToUpdateQuestion + '?id=' + testId_Edit;
            FinalQuestionObject.QuestionDetailsObject = QuestionDetailsObject;
            FinalQuestionObject.arrOfSecretTestCases = arrOfSecretTestCases;
            console.log(FinalQuestionObject);
            $('.loader-container').hide();
            getUserQuestionAnswerStatus = GetUserQuestionAnswerStatus();
            let url = $('.url-ProgrammingTest-SubmitAfterEvaluation').text();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(FinalQuestionObject),
                success: function (data) {
                    if (data === true) {
                        $('.submitButton ').prop('disabled', true);
                        $('#testLanguage').prop('disabled', true);
                       // $('.CodeMirror').addClass('disabledAnswer');
                        editor.setOption("readOnly", true);
                        $('.CodeMirror-cursors').css("display", "none");
                        $('.CodeMirror').css("background-color","rgb(235, 235, 228)");
                        $("[data-ques-id='" + currentQuestion + "']").css("background-color", "#76de76");
                        ChecksubmitFinalQuetsions();
                    }
                    else {
                        $('.submitButton ').prop('disabled', false);
                        $('#testLanguage').prop('disabled', false);
                        //$('.CodeMirror').removeClass('disabledAnswer');
                        editor.setOption("readOnly", false);
                        $('.CodeMirror-cursors').css("display", "");
                        $('.CodeMirror').css("background-color", "rgba(0,0,0,0)");
                        $("[data-ques-id='" + currentQuestion + "']").css("background-color", "#f75050");
                    }
                },
                processData: false,
                contentType: "application/json;"
            });
        }
    };
}

// CalculateMarks(j, value, marks, input, output, questionID, TestCaseID);
function CalculateMarks(currentTestCase, answer, perQuestionMarks, secretTestInput, secretTestCaseOutput, questionID, TestCaseID) {
    //$.ajaxSetup({ async: false });
    let theProgram = $(".code-1").val();
    let theLanguageNum = $('#testLanguage').val();
    currentTestCase1 = currentTestCase + 1;
    //let testInput = $('.test-input').val();
    let compilerArgs = "";
    let testCaseStatus = false; //BY default set as testCase is Wrong.
    let QuestionMarksAsPerStatus = 0;
    if (parseInt(theLanguageNum) === 27) {
        var plus = encodeURIComponent('+');
        compilerArgs = "-Wall -std=c++14 -stdlib=libc++ -O2 -o a.out source_file.cpp";
        let urlForCPlu = "https://rextester.com/rundotnet/api?LanguageChoice=" + theLanguageNum + "&Program=" + answer + "&Input=" + secretTestInput + "&CompilerArgs=" + compilerArgs;

        RexTestForCPlusTestCases(urlForCPlu);
        getUserQuestionAnswerStatus.SetQuestionData(questionID, testCaseStatus, filteredValue, QuestionMarksAsPerStatus, TestCaseID);
        if (currentTestCase === SecretOptionLength) {
            getUserQuestionAnswerStatus.SubmitEvaluatedQuestions();
        }
        return;
    }
    let filteredValue = CleanTheProgram(answer);
    let url = "https://rextester.com/rundotnet/api?LanguageChoice=" + theLanguageNum + "&Program=" + filteredValue + "&Input=" + secretTestInput + "&CompilerArgs=" + compilerArgs;
    console.log(url);

    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        //dataType: 'jsonp',
        success: function (data) {
            console.log(data.Result);
            if (data.Result === null) {
                testCaseStatus = false;
                QuestionMarksAsPerStatus = 0;
            }
            if (data.Result !== null) {
                if (data.Result.trimEnd() !== secretTestCaseOutput) {
                    //setTestCaseState.setTestCaseAsFailed();
                    testCaseStatus = false;
                    QuestionMarksAsPerStatus = 0;

                }
                else if (data.Result.trimEnd() === secretTestCaseOutput) {
                    testCaseStatus = true;
                    QuestionMarksAsPerStatus = perQuestionMarks;
                }
            }
            getUserQuestionAnswerStatus.SetQuestionData(questionID, testCaseStatus, filteredValue, QuestionMarksAsPerStatus, TestCaseID);

            if (currentTestCase === SecretOptionLength ) {
                getUserQuestionAnswerStatus.SubmitEvaluatedQuestions();
            }
            //if (setTestCaseState.getTestCaseState()) {// if the state is true then a test case has failed
            //    //getUserQuestionAnswerStatus.SetQuestionData(questionID, setTestCaseState.getTestCaseState(), answer, 0, TestCaseID);
               
            //}


        }
    });
}

function CalculateAndReturnRequestTime() {
    let requestNumber = 1;

    return function () {
        let timeOfCurrentRequest = requestNumber * 2000;
        requestNumber += 1;
        return timeOfCurrentRequest;
    };
}


function RexTestForCPlusTestCases(url) {
    let cleanUrl = CleanUrlForCPlus(url);
    console.log(cleanUrl);
    $.ajax({
        url: cleanUrl,
        type: "POST",
        dataType: "json",
        success: function (data) {
            $('.loader-container').hide();
            if (data.Result) {
                $('.test-output').html(data.Result);
            } else {
                $('.test-output').html(data.Errors);
            }
        }
    });
}

function CalculateSumOFMarks() {
  var Score_ID =  $('#model-score-board-id').text();
    let url = $('.url-ProgrammingTest-SumUpTestSubmitted').text();
    url = url + '?Score_ID=' + Score_ID;
    $.ajax({
        url: url,
        type: "POST",
        success: function (Is_SumpCalculated) {
            if (Is_SumpCalculated) {
                console.log(Is_SumpCalculated);
            } else {
                console.log(Is_SumpCalculated);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function ChecksubmitFinalQuetsions() {
    let url = $('.url-ProgrammingTest-CheckSubmitFinalQuestions').text();
    var totalNoofQuestion = $('.totalNoOfQuestions').text();
    let StudentID = $('#model-student-id').text();
    let Test_ID = $('#model-test-id').text();
    //url = url + '?Test_ID=' + Test_ID + '?StudentID=' + StudentID;
    let dataObj = new Object();
    dataObj.Test_ID = Test_ID;
    dataObj.StudentID = StudentID;
    $.ajax({
        url: url,
        type: "POST",
        data: dataObj,
        success: function (total_SubmitQuestion) {
            if (total_SubmitQuestion == totalNoofQuestion.trimLeft().trimRight().trimEnd()) {
                console.log(total_SubmitQuestion);
                //$('.submitButton')
                finalSubmission();
            } else {
                console.log(total_SubmitQuestion);
                alert("Answer is submitted successfully!");
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function finalSubmission() {
    Swal.fire({
        title: 'Your test is completed Successfully!',
        //position: 'top-end',
        type: 'alert',
        //showCancelButton: true,
        //confirmButtonColor: '#DD6B55',
        confirmButtonText: 'OK'
        //cancelButtonText: 'No.'
    }).then((result) => {
        if (result.value) {
            let url = $('.url-ProgrammingTest-TestSubmitted').text();
            SubmitTestDuration();
            PerformTasksBeforeChangingTheQuestion();
            CalculateSumOFMarks();
            //window.location.href = url;
            TestSubmitted();
        } else {
            // result.dismiss can be 'cancel', 'overlay', 'esc' or 'timer'
        }
    });
}

$(document).keydown(function (event) {
    if (event.keyCode == 123) { // Prevent F12
        $('.f12Disable').removeClass('hidden');
        $(".f12Disable").fadeTo(1000, 500).slideUp(500, function () {
            //$(".f12Disable").alert('close');
            $('.f12Disable').addClass('hidden');
        });
        return false;
    } else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I  
        $('.f12Disable').removeClass('.hidden');
        $(".f12Disable").fadeTo(1000, 500).slideUp(500, function () {
            $('.f12Disable').addClass('hidden');
           //$(".f12Disable").alert('close');
        });
        return false;
    }
});

$(document).ready(function () {
    $(document).bind("contextmenu", function (e) {
        $('.rightClick').removeClass('hidden');
        $(".rightClick").fadeTo(1000, 500).slideUp(500, function () {
            //$(".rightClick").alert('close');
            $('.rightClick').addClass('hidden');
        });
        return false;
    });
});

