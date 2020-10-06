$(document).ready(() => {
    $(".loader-container").hide();

    AddButtonToLastQuestion();
    SetQuestionCounter();
});
var TCCounter = 0;
function AddNewQuestion() {
    TCCounter = TCCounter + 1;
    let questionContainer = $(".allQuestions");
    let questionHtml = '<div class="questionContainer row">' +
        '<div class="questionLabel add-questions-all-question-for-counter not-hidden col-sm-2"></div>' +
        '<textarea class="question col-lg-6 col-sm-5" rows="2" cols="50"></textarea>' +
        '<input type="number" class="question-marks" />' +
        '<div class="addNew btn btn-primary add-ques-plus" style="height: 3.5rem;max-width:4rem;" onclick="AddNewQuestion()">+</div>' +
        '<div class="removeNew btn btn-danger add-ques-min " style="height: 3.5rem;max-width:4rem;" onclick="RemovewQuestion()">x</div>' +

        '<div class="panel-body allSampleTC' + TCCounter + ' container-fluid">' +
        '<div class="SampleTestCaseContainer row">' +
        '<div class="SampleTestCases col-sm-2">Sample Test Cases</div>' +
        '<textarea class="sampleTestCase SP' + TCCounter + '   col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<textarea class="sampleTestCase SPO' + TCCounter + ' col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<div class="addNewSampleTopic' + TCCounter + ' btn btn-primary col-sm-2 " style="height: 3.5rem;max-width:4rem;" onclick="addSampleTestCases(' + TCCounter + ')">+</div>' +
        '<div class="hidden removeNewSampleTopic' + TCCounter + ' btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewSampleTestcase(' + TCCounter + ')">x</div>' +
        '</div>' +
        '</div>' +
        '<div class="panel-body allSecretTC' + TCCounter + ' container-fluid">' +
        '<div class="SecretTestCaseContainer row">' +
        '<div class="SampleTestCases col-sm-2">Screte Test Cases</div>' +
        '<textarea class="sampleTestCase ST' + TCCounter + ' col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<textarea class="sampleTestCase STO' + TCCounter + ' col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<div class="addNewSecretTopic' + TCCounter + ' btn btn-primary col-sm-2 " style="height: 3.5rem;max-width:4rem;" onclick="addSecretTestCases(' + TCCounter + ')">+</div>' +
        '<div class="hidden removeNewSecretTopic' + TCCounter + ' btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewTestcase(' + TCCounter + ')">x</div>' +
        '</div>' +
        '</div>' +
        '</div>'
    ;
    questionContainer.append(questionHtml);
    AddButtonToLastQuestion();
    SetQuestionCounter();

}

function RemovewQuestion(curreEle) {
    let eleToRemove = $(".removeNew");
    if (eleToRemove.length === 1) {
        return;
    }
    eleToRemove.eq(eleToRemove.length - 1).parent().remove();
    AddButtonToLastQuestion();
    if (TCCounter != 0) {
        TCCounter = TCCounter - 1;
    }
}

function AddButtonToLastQuestion() {
    let allQuestionsChildrenAddNewButton = $(".questionContainer").children(".addNew");
    let allQuestionsChildrenremoveNewButton = $(".questionContainer").children(".removeNew");
    let lastElement = allQuestionsChildrenAddNewButton.length - 1;
    for (var i = 0; i < allQuestionsChildrenAddNewButton.length; i++) {
        if (i === lastElement) {
            allQuestionsChildrenAddNewButton.eq(lastElement).removeClass("hidden");
            allQuestionsChildrenremoveNewButton.eq(lastElement).removeClass("hidden");
        } else {
            allQuestionsChildrenAddNewButton.eq(i).addClass("hidden");
            allQuestionsChildrenremoveNewButton.eq(i).addClass("hidden");
        }
    }
}

//function AddAllQuestions() {
//    $(".loader-container").show();
//    let questionDescriptionEmpty = false;
//    let allQuestions = $('textarea.question');
//    let allQuestionsMarks = $('.question-marks');
//    let allQuestionToAddArrr = new Array();
//    let testID = $('.testID');
//    for (var i = 0; i < allQuestions.length; i++) {
//        // If question description is empty, we skip adding it
//        if (allQuestions[i].value === '') {
//            continue;
//        }
//        let questionToAdd = new Object();
//        questionToAdd.Test_ID = testID.text();
//        questionToAdd.Ques_Desc = allQuestions[i].value;
//        questionToAdd.Max_Marks = allQuestionsMarks[i].value;
//        questionToAdd.Ques_IsActive = true;
//        allQuestionToAddArrr.push(questionToAdd);
//    }
//    if (!questionDescriptionEmpty) {
//        AddAjaxFunc(allQuestionToAddArrr);
//    } else {

//    }
//}

//function AddAllQuestions() {
 
//    $(".loader-container").show();
//    let questionDescriptionEmpty = false;
//    let allQuestions = $('textarea.question');
//    let allQuestionsMarks = $('.question-marks');
//    let allSampleTC = $('.textarea.sampleTestCase');
//    let allQuestionToAddArrr = new Array();
//    let testID = $('.testID');
//    for (var i = 0; i < allQuestions.length; i++) {
//        // If question description is empty, we skip adding it
//        if (allQuestions[i].value === '') {
//            continue;
//        }
//        let questionToAdd = new Object();
//        questionToAdd.Test_ID = testID.text();
//        questionToAdd.Ques_Desc = allQuestions[i].value;
//        questionToAdd.Max_Marks = allQuestionsMarks[i].value;
//        questionToAdd.Ques_IsActive = true;
//        questionToAdd.SampleTestCase = allSampleTC;
//        let allSecretTestCase = $('textarea.ST' + i);
//        let allSecretToAddArrr = new Array();
//        for (var j = 0; j < allSecretTestCase.length; j++)
//        {
//            let secretTestCaseAdd = new Object();
//            secretTestCaseAdd.Test_Case_Description = allSecretTestCase[j].value;
//            secretTestCaseAdd.Type = true;
//            allSecretToAddArrr.push(secretTestCaseAdd);
//        }
//        questionToAdd.Tbl_Stud_ProgTest_TestCases = allSecretToAddArrr;
//        allQuestionToAddArrr.push(questionToAdd);
//    }
//    console.log(allQuestionToAddArrr);
//    if (!questionDescriptionEmpty) {
//           AddAjaxFunc(allQuestionToAddArrr);
//    } else {

//    }
//}

function AddAllQuestions() {
    debugger;
    $(".loader-container").show();
    let questionDescriptionEmpty = false;
    let allQuestions = $('textarea.question');
    let allQuestionsMarks = $('.question-marks');
    let allSampleTC = $('.textarea.sampleTestCase');
    let allQuestionToAddArrr = new Array();
    let testID = $('.testID');
    for (var i = 0; i < allQuestions.length; i++) {
        // If question description is empty, we skip adding it
        if (allQuestions[i].value === '') {
            continue;
        }
        let questionToAdd = new Object();
        questionToAdd.Test_ID = testID.text();
        questionToAdd.Ques_Desc = allQuestions[i].value;
        questionToAdd.Max_Marks = allQuestionsMarks[i].value;
        questionToAdd.Ques_IsActive = true;
        //questionToAdd.SampleTestCase = allSampleTC;
        let allSecretTestCase = $('textarea.ST' + i);
        let allSecretTestCaseO = $('textarea.STO' + i);
        let allSampleTestCase = $('textarea.SP' + i);
        let allSampleTestCaseO = $('textarea.SPO' + i);
        let allSecretToAddArrr = new Array();
        for (var j = 0; j < allSecretTestCase.length; j++) {
            let secretTestCaseAdd = new Object();
            secretTestCaseAdd.Test_Case_Input = allSecretTestCase[j].value;
            secretTestCaseAdd.Test_Case_Output = allSecretTestCaseO[j].value;
            secretTestCaseAdd.Type = true;
            allSecretToAddArrr.push(secretTestCaseAdd);
        }
        for (var k = 0; k < allSampleTestCase.length; k++) {
            let sampleTestCaseAdd = new Object();
            sampleTestCaseAdd.Test_Case_Input = allSampleTestCase[k].value;
            sampleTestCaseAdd.Test_Case_Output = allSampleTestCaseO[k].value;
            sampleTestCaseAdd.Type = false;
            allSecretToAddArrr.push(sampleTestCaseAdd);
        }
        questionToAdd.Tbl_Stud_ProgTest_TestCases = allSecretToAddArrr;
        allQuestionToAddArrr.push(questionToAdd);
    }
    console.log(allQuestionToAddArrr);
    if (!questionDescriptionEmpty) {
        AddAjaxFunc(allQuestionToAddArrr);
    } else {

    }
}


function AddAjaxFunc(allQuestionToAdd) {
    $.ajax({
        type: "POST",
        url: "/Home/AddQuestions",
        contentType: "application/json",
        data: JSON.stringify({ quesList: allQuestionToAdd }),
        dataType: "json",
        success: AddAjaxFuncSuccessFunc,
        error: ErrorFunc
    });
}

function AddAjaxFuncSuccessFunc(data) {
    //window.open("/Home/ProgrammingQuiz_list");
    //console.log(data);
    $(".loader-container").hide();

    window.location.href = "/Home/ProgrammingQuiz_list";
}

function ErrorFunc() {

}

function SetQuestionCounter() {
    let allQuestinoLabels = $('.all-question-for-counter').text();
    console.log(allQuestinoLabels);
    let counter = 1;
    $('.add-questions-all-question-for-counter.not-hidden').each(function () {
        //console.log($(this).text());
        $(this).text('Q ' + counter);
        //console.log($(this).text());
        counter += 1;
    })
}

function addSecretTestCases(TCCount) {
    debugger;
    var TestCaseCount = $('.ST' + TCCount).length;
    //if (TCCount != 0) {
    //    TCCount = TCCounter;
    //}
    if (TestCaseCount > 4) {
        return;
    }
    let questionContainer = $(".allSecretTC" + TCCount);
    let questionHtml = '<div class="SecretTestCaseContainer row">' +
        '<div class="SampleTestCases col-sm-2">Screte Test Cases</div>' +
        '<textarea class="sampleTestCase ST' + TCCount + '  col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<textarea class="sampleTestCase STO' + TCCount + ' col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<div class="addNewSecretTopic' + TCCount + ' btn btn-primary col-sm-2 " style="height: 3.5rem;max-width:4rem;" onclick="addSecretTestCases(' + TCCount + ')">+</div>' +
        '<div class="removeNewSecretTopic' + TCCount + ' btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewTestcase(' + TCCount + ')">x</div>' +
    '</div>';
    questionContainer.append(questionHtml);
    AddButtonToLastTestCase(TCCount);
}

function AddButtonToLastTestCase(curreEle) {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}
  
    let TestCaseChildrenAddNewButton = $(".SecretTestCaseContainer").children(".addNewSecretTopic" + curreEle);
    let TestCaseChildrenremoveNewButton = $(".SecretTestCaseContainer").children(".removeNewSecretTopic" + curreEle);
    let lastElement = TestCaseChildrenAddNewButton.length - 1;
    for (var i = 0; i < TestCaseChildrenAddNewButton.length; i++) {
        if (i === lastElement) {
            TestCaseChildrenAddNewButton.eq(lastElement).removeClass("hidden");
            TestCaseChildrenremoveNewButton.eq(lastElement).removeClass("hidden");
        } else {
            TestCaseChildrenAddNewButton.eq(i).addClass("hidden");
            TestCaseChildrenremoveNewButton.eq(i).addClass("hidden");
        }
    }
}


function RemovewTestcase(curreEle) {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}
    let eleToRemove = $(".removeNewSecretTopic" + curreEle);
    if (eleToRemove.length === 1) {
        return;
    }
    eleToRemove.eq(eleToRemove.length - 1).parent().remove();
    AddButtonToLastTestCase(curreEle);
}

//Sample Test Cases

function addSampleTestCases(TCCount) {
    debugger;
    var SampleTestCaseCount = $('.SP' + TCCount).length;
    //if (TCCount != 0) {
    //    TCCount = TCCounter;
    //}
    if (SampleTestCaseCount > 2) {
        return;
    }
    let questionContainer = $(".allSampleTC" + TCCount);
    let questionHtml = ' <div class="SampleTestCaseContainer row">' +
        '<div class="SampleTestCases col-sm-2">Sample Test Cases</div>' +
        '<textarea class="sampleTestCase SP' + TCCount + '  col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<textarea class="sampleTestCase SPO' + TCCount + ' col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
        '<div class="addNewSampleTopic' + TCCount + ' btn btn-primary col-sm-2 " style="height: 3.5rem;max-width:4rem;" onclick="addSampleTestCases(' + TCCount + ')">+</div>' +
        '<div class="removeNewSampleTopic' + TCCount + ' btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewSampleTestcase(' + TCCount + ')">x</div>' +
    '</div>';
    questionContainer.append(questionHtml);
    AddButtonToLastSampleTestCase(TCCount);
}

function AddButtonToLastSampleTestCase(curreEle) {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}

    let TestCaseChildrenAddNewButton = $(".SampleTestCaseContainer").children(".addNewSampleTopic" + curreEle);
    let TestCaseChildrenremoveNewButton = $(".SampleTestCaseContainer").children(".removeNewSampleTopic" + curreEle);
    let lastElement = TestCaseChildrenAddNewButton.length - 1;
    for (var i = 0; i < TestCaseChildrenAddNewButton.length; i++) {
        if (i === lastElement) {
            TestCaseChildrenAddNewButton.eq(lastElement).removeClass("hidden");
            TestCaseChildrenremoveNewButton.eq(lastElement).removeClass("hidden");
        } else {
            TestCaseChildrenAddNewButton.eq(i).addClass("hidden");
            TestCaseChildrenremoveNewButton.eq(i).addClass("hidden");
        }
    }
}


function RemovewSampleTestcase(curreEle) {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}
    let eleToRemove = $(".removeNewSampleTopic" + curreEle);
    if (eleToRemove.length === 1) {
        return;
    }
    eleToRemove.eq(eleToRemove.length - 1).parent().remove();
    AddButtonToLastSampleTestCase(curreEle);
}
