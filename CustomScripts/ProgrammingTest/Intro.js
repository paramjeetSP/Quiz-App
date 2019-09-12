let UrlProgrammingTestAllProgrammingTests = $('.url-programming-test-all-programming-test').text();
let UrlProgrammingTestGetTestData = $('.url-programming-test-get-test-data-test').text();

$(document).ready(() => {
    //alert("Some message");
    $(".loader-container").show();

    GetAllQuestions();
});

function GetAllQuestions() {
    $.ajax({
        type: "GET",
        url: UrlProgrammingTestAllProgrammingTests,
        success: GetAllProgrammingTestSuccess,
        processData: false,
        dataType: "html"
    });
}

function GetAllProgrammingTestSuccess(data) {
    $('#ProgrammingTests').empty();
    $('#ProgrammingTests').html(data);
    $(".loader-container").hide();
}

function GetInstructions(testId) {
    let timer = setTimeout(function () {
        $(".loader-container").show();
    }, 500);
    let url  = UrlProgrammingTestGetTestData + '/' + testId;
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            clearTimeout(timer);
            $('#instructions').empty();
            $('#instructions').html(data);
            $(".loader-container").hide()
        },
        processData: false,
        dataType: "html"
    });
}
function GetInstructionsSuccess(data, timer) {
;
}
