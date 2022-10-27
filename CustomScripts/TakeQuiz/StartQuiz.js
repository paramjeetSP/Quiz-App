/**** Quiz ****/
var questions = [{
    question: "",
    choices: ["", "", "", ""],
    correctAnswer: 0
}];
var qId = 0;
var progressCount = 1;
function Test() {
    var item = $("#StartSubid").val();
    var i = 0;
    $.ajax
        ({
            url: '/Home/GetQuestions',
            type: 'get',
            data: { 'subId': 0 },
            success: function (result) {
                $.each(result, function (index, value) {
                    qid = value.Qid;
                    questions[i] = [{
                        question: [value.QuestionName],
                        choices: [value.OpOne, value.OpTwo, value.OpThree, value.OpFour],
                        correctAnswer: value.Correct
                    }];
                    i++;
                });
                displayCurrentQuestion();
            }
        });
}

var currentQuestion = 0;
var correctAnswers = 0;
var quizOver = false;
var count = 1;
var totalTime = '';
var today = 0;
var start = new Date().getTime();
var remaining = 0;

$(document).ready(function () {
    Test();
    $('#headerArea').hide();
    $(this).find(".quizMessage").hide();
    $(this).find(".nextButton").on("click", function () {
        if (!quizOver) {
            value = $("input[type='radio']:checked").val();
            if (value === undefined) {
                $(document).find(".quizMessage").text("Please select an answer");
                $(document).find(".quizMessage").show();
            } else {
                $(document).find(".quizMessage").hide();
                if (value === questions[currentQuestion][0].correctAnswer) {
                    correctAnswers++;
                }
                currentQuestion++;
                if (currentQuestion < (questions.length) - 1) {
                    displayCurrentQuestion();
                    progressBar();
                }
                else if (currentQuestion === (questions.length) - 1) {
                    displayCurrentQuestion();
                    progressBar();
                    $(document).find(".nextButton").text("Submit");
                }
                else {
                    displayScore(false);
                }
            }
        }
    });
});

function displayCurrentQuestion() {
    var question = questions[currentQuestion][0].question;
    var questionClass = $(document).find(".quizContainer > .question");
    var choiceList = $(document).find(".quizContainer > .choiceList");
    var numChoices = questions[currentQuestion][0].choices.length;
    $(questionClass).text(question);
    auto_grow(questionClass[0]);
    count++;
    $(choiceList).find("li").remove();
    var choice;
    var temp = 1;
    for (i = 0; i < numChoices; i++) {
        choice = questions[currentQuestion][0].choices[i];
        $('<li class="listyle"><input type="radio" id ="radio' + temp + '" value="' + choice + '" name="radio"  class="radio"/><label for="radio' + temp + '" class="labstyle">' + choice + '</label></li>').appendTo(choiceList);
        temp = temp + 1;
    }
}

function resetQuiz() {
    currentQuestion = 0;
    correctAnswers = 0;
    hideScore();
}

function displayScore(isautoSubmitted) {
    var newUrl = '@Url.Action("TakeQuiz","Home")';
    $("correctAns").val(correctAnswers);
    var nowt = new Date().getTime();
    var duration = (nowt - start) / 1000;
    var nowMinutes = Number(Math.floor(duration / 60));
    var nowSeconds = Number(Math.floor(duration % 60));
    totalTime = "" + format(nowMinutes) + ":" + format(nowSeconds) + "";
    let subId = $('#sub-topic-id').text();
    $.ajax({
        url: '/Home/GetScore',
        data: {
            'correctAnswers': correctAnswers,
            'totalTime': totalTime,
            'today': today,
            'currentQuestion': currentQuestion,
            'quesId': qid,
            'subId': subId,
            'isAutoSubmitted': isautoSubmitted
        },
        dataType: "html",
        success: function (result) {
            var data = JSON.parse(result);
            if (data.TotalTestSubmitted === 1) {
                let url = $('.url-Home-TakeQuiz').text();
                window.location.href = url;
            }
            else {
                let url = $('.url-Home-QuizSubmitted').text();
                window.location.href = url;
            }
        },
        error: (function (erro) {
            let url = $('.url-Home-QuizSubmitted').text();
            window.location.href = url;
        })
    })
}

function hideScore() {
    $(document).find(".result").hide();
}

countdown(3599);  // 60min

function countdown(seconds) {
    var now = new Date().getTime();
    var target = new Date(now + seconds * 1000);
    var update = 500;
    var int = setInterval(function () {
        var now = new Date();
        if (remaining >= 0) {
            remaining = (target - now) / 1000;
        }
        else {
            clearInterval(int);
            displayScore(false);
            return;
        }
        var nowdate = new Date();
        var hrs = ("0" + nowdate.getHours()).slice(-2);
        var min = ("0" + nowdate.getMinutes()).slice(-2);
        var sec = ("0" + nowdate.getSeconds()).slice(-2);
        var day = ("0" + nowdate.getDate()).slice(-2);
        var month = ("0" + (nowdate.getMonth() + 1)).slice(-2);
        today = now.getFullYear() + "/" + (month) + "/" + (day) + " " + (hrs) + ":" + (min) + ":" + (sec);
        var minutes = ~~(remaining / 60);
        var seconds = ~~(remaining % 60);
        document.getElementById("countdown").innerHTML
            = format(minutes) + ":" + format(seconds);
        if (remaining <= 1) {
            $(document).find(".nextButton").text("Submit").hide();
        }
    }, update);
}

function format(num) {
    return num < 10 ? "0" + num : num;
}

function auto_grow(element) {
    element.style.height = "5px";
    element.style.height = (element.scrollHeight) + "px";
}

function progressBar() {
    var x = document.getElementById("q" + progressCount + "").classList;
    var progressCount2 = progressCount + 1;
    var y = document.getElementById("q" + progressCount2 + "").classList;
    if (x.contains("active")) {
        x.remove("active");
        x.add("complete");
        if (y.contains("disabled")) {
            y.remove("disabled");
            y.add("active");
        }
    }
    progressCount = progressCount + 1;
}