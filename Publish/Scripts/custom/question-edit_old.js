let testId = $('.testID').text();
let urlToUpdateQuestion = $('#url-to-update-question').text();
$(document).ready(() => {
    //alert("Hello from question-edit.js");
    $(".loader-container").hide();
    AddButtonToLastQuestion();
    MarkOldQuestionToUpdate();
    $('.removeOld').click(function () {
        RemoveOldQuestion(this);
    });
    SetQuestionCounter();
});
var TCCounter = 0;

function AddButtonToLastQuestion() {
    let allQuestionsChildrenAddNewButton = $(".questionContainer").children(".addNewNotHidden");
    let allQuestionsChildrenremoveNewButton = $(".questionContainer").children(".removeNew");
    let lastElement = allQuestionsChildrenAddNewButton.length - 1;// TODO: Wokring here
    for (var i = 0; i < allQuestionsChildrenAddNewButton.length; i++) {
        if (i === lastElement) {
            allQuestionsChildrenAddNewButton.eq(lastElement).removeClass("hidden");
            //allQuestionsChildrenAddNewButton.eq(lastElement).removeClass("addNewNotHidden");
            allQuestionsChildrenAddNewButton.eq(i).addClass("addNewLastElement");
        } else {
            allQuestionsChildrenAddNewButton.eq(i).addClass("hidden");
            allQuestionsChildrenAddNewButton.eq(i).addClass("addNewNotHidden");
        }
    }
}

//function AddNewQuestion() {
//    let questionContainer = $(".allQuestions");
//    let questionHtml = '<div class="questionContainer questionNew row">' +
//        '<div class="questionLabel all-question-for-counter not-hidden col-lg-2 col-sm-2"></div>' +
//        '<textarea class="question newQuestionDesc col-lg-6 col-sm-5" rows="2" cols="50"></textarea>' +
//        '<input type="number" class="new-question-marks" />' +
//        '<div class="addNew btn btn-primary col-lg-2 edit-quis-move" style="height: 3.5rem;max-width:4rem;" onclick="AddNewQuestion()">+</div>' +
//        '<div class="removeNew btn btn-danger col-lg-2 edit-quis-move" style="height: 3.5rem;max-width:4rem;" onclick="RemoveQuestion()">x</div>' +
//    '</div>' +
//        '<div class="SampleTestCaseContainer sampleTestNew row">' +
//        '<textarea class="question newSampleTestCase col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
//        '<textarea class="question newSampleTestCase col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
//        '<div class="addNew btn btn-primary col-lg-2 edit-quis-move" style="height: 3.5rem;max-width:4rem;" onclick="addSampleTestCasesForNewQuestion()">+</div>' +
//        '<div class="removeNew btn btn-danger col-lg-2 edit-quis-move" style="height: 3.5rem;max-width:4rem;" onclick="removeSampleTestCasesForNewQuestion()">x</div>' +
//    '</div>' +
//        '<div class="SecretTestCaseContainer secertTestNew row">' +
//        '<textarea class="question newSecretTestCase col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
//        '<textarea class="question newSecretTestCase col-lg-2 col-sm-5" rows="2" cols="50"></textarea>' +
//        '<div class="addNew btn btn-primary col-lg-2 edit-quis-move" style="height: 3.5rem;max-width:4rem;" onclick="addSecretTestCasesForNewQuestion()">+</div>' +
//        '<div class="removeNew btn btn-danger col-lg-2 edit-quis-move" style="height: 3.5rem;max-width:4rem;" onclick="removeSecretTestCasesForNewQuestion()">x</div>' +
//    '</div>';
//    questionContainer.append(questionHtml);
//    AddButtonToLastQuestion();
//    SetQuestionCounter();
//}





/**
On remove click of new added question, perform all row delete activities
Perform these activities on the basis of removeNew class
**/
function RemoveQuestion(curreEle) {
    let eleToRemoveNew = $(".removeNew");
    let eleToRemoveOld = $(".removeOld");
    let totalQuestions = eleToRemoveNew.length + eleToRemoveOld.length;
    if (totalQuestions === 1) {
        return;
    }
    eleToRemoveNew.eq(eleToRemoveNew.length - 1).parent().remove();
    AddButtonToLastQuestion();
    SetQuestionCounter();
}

function SetQuestionCounter() {
    let allQuestinoLabels = $('.all-question-for-counter').text();
    console.log(allQuestinoLabels);
    let counter = 1;
    $('.all-question-for-counter.not-hidden').each(function () {
        //console.log($(this).text());
        $(this).text('Q ' + counter);
        //console.log($(this).text());
        counter += 1;
    })
}

/**
On remove click of already added question, perform all soft delete activities
Perform these activities on the basis of removeOld class
**/
function RemoveOldQuestion(theThis) {
    if (confirm("Are you sure?")) {
        let eleToRemoveNew = $(".removeNew");
        let eleToRemoveOld = $(".removeOld.not-removed-yet");
        let totalQuestions = eleToRemoveNew.length + eleToRemoveOld.length;
        if (totalQuestions === 1) {
            return;
        }
        //eleToRemoveOld.eq(eleToRemoveOld.length - 1).parent().remove();
        RemoveOldQuestionMetaDataForUpdation(theThis);
        AddButtonToLastQuestion();
        SetQuestionCounter();

    }
    return false;
}

function RemoveOldQuestionMetaDataForUpdation(theThis) {
    let theEle = $(theThis);
    //console.log($(this).parent());
    $(theThis).siblings('.all-question-for-counter.not-hidden').removeClass('not-hidden');
    $(theThis).removeClass('not-removed-yet');
    theEle.parent().addClass('hidden');
    $(theThis).siblings('.addNewNotHidden').removeClass('addNewNotHidden');
    theEle.attr('data-isActive', false);
    theEle.siblings('textarea').attr('data-toUpdate', true);
    theEle.siblings('textarea').attr('data-isActive', false);
}

function UpdateQuestions() {
    $(".loader-container").show();

    let oldQuestions = GetAllOldQuestionsWithContainer();
    //console.log('oldQuestions');
    //console.log(oldQuestions);
    let newQuestions = GetAllNewQuestions();
    //console.log('newQuestions');
    //console.log(newQuestions);
    let oldQuestionsToUpdate = FilterOldQuestionsToUpdate();

    let allQuestions = {
        "QuestionsToUpdate": oldQuestionsToUpdate,
        "AddQuestions": newQuestions
    };
    console.log(allQuestions);
    //return;
    let url = urlToUpdateQuestion + '?id=' + testId;
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(allQuestions),
        success: GetTestDataSuccess,
        processData: false,
        contentType: "application/json;"
    });
}

function GetTestDataSuccess(data) {
    let url = $('#url-to-all-tests').text();
    $(".loader-container").hide();

    window.location.href = url;
}

function FilterOldQuestionsToUpdate() {
    let allQuetionsTextArea = GetAllOldQuestionsTextArea();
    //console.log('allQuetionsTextArea');
    //console.log(allQuetionsTextArea);

    $('.questionContainer.questionOld textarea[data-toupdate="true"]').each((ele) => {
        console.log($(this));
    });
    let allQuestionReadyForUpdation = new Array();
    $('textarea[data-toupdate="true"').each(function (index) {
        let Ques_Desc = $(this).val();
        if (Ques_Desc !== '') {
            let UpdateThisQuestion = new Object();
            let Ques_ID = $(this).data('ques-id');
            let IsActive = $(this).data('isactive');
            UpdateThisQuestion.Ques_ID = Ques_ID;
            UpdateThisQuestion.Test_ID = testId;
            UpdateThisQuestion.Ques_Desc = Ques_Desc;
            UpdateQuestions.IsActive = IsActive;
            // Not able to get the data value when it is true, so improvising here, as when the data-isActive is false then we get that value and when it is undefined which means it is active and data-IsActive is not false
            if ($(this).attr('data-isActive') === 'undefined' || $(this).attr('data-isActive') === undefined) {
                UpdateThisQuestion.Ques_IsActive = 'true';
            } else {
                UpdateThisQuestion.Ques_IsActive = $(this).attr('data-isActive');
            }
            allQuestionReadyForUpdation.push(UpdateThisQuestion);
        }
    });
    return allQuestionReadyForUpdation;
}


function GetAllOldQuestionsTextArea() {
    return $('.questionContainer.questionOld textarea[data-toupdate="true"]');
}

function GetAllOldQuestionsWithContainer() {
    return $(".questionContainer.questionOld");
}
function GetAllNewQuestions() {
    let allQuestionReadyForAddition = new Array();
    let allQuestionsMarks = $('.new-question-marks');// Getting all new questions to add
    $(".questionContainer.questionNew textarea").each(function (index) {
        let AddThisQuestion = new Object();
        let Ques_Desc = $(this).val();
        AddThisQuestion.Ques_Desc = Ques_Desc;
        AddThisQuestion.Test_ID = testId;
        AddThisQuestion.Ques_IsActive = 'true';
        AddThisQuestion.Max_Marks = allQuestionsMarks[index].value;
        allQuestionReadyForAddition.push(AddThisQuestion);
    });
    return allQuestionReadyForAddition;
}

function MarkOldQuestionToUpdate() {
    $("textarea").on("change keyup paste", function () {
        var currentVal = $(this).attr('data-toUpdate', true);
    });
}
////// NEW LOGIC FOR UPDATE ////////







///////////LOGIC FOR ADDING NEW QUESTION NEW SAMPLE AND SECRET TESTCASES//////////////

function AddNewQuestion() {
    TCCounter = TCCounter + 1;
    let questionContainer = $(".allQuestions");
    let questionHtml = '<div class="questionContainer questionNew row">' +
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




function addSampleTestCasesForNewQuestion(TCCount) {
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


function addSecretTestCasesForNewQuestion(TCCount) {
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

function removeSampleTestCasesForNewQuestion() {

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
    AddButtonToNewLastSampleTestCase(TCCount);
}

function removeSecretTestCasesForNewQuestion() {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}
    let eleToRemove = $(".removeNewSecretTopic" + curreEle);
    if (eleToRemove.length === 1) {
        return;
    }
    eleToRemove.eq(eleToRemove.length - 1).parent().remove();
    AddButtonToNewLastTestCase(curreEle);

}

function removeSampleTestCasesForNewQuestion() {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}
    let eleToRemove = $(".removeNewSampleTopic" + curreEle);
    if (eleToRemove.length === 1) {
        return;
    }
    eleToRemove.eq(eleToRemove.length - 1).parent().remove();
    AddButtonToLastNewSampleTestCase(curreEle);

}

function AddButtonToNewLastTestCase(curreEle) {
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

function AddButtonToNewLastSampleTestCase(curreEle) {
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

////////END///////////////
