let testId_Edit = $('.testID').text();
let urlToUpdateQuestion = $('#url-to-update-question').text();
let TCCounterEdit = 0;
$(document).ready(() => {
    $(".loader-container").hide();
    TCCounterEdit = $('.questionContainer').length;
    AddButtonToLastQuestion();
    SetQuestionCounter();
});


function AddNewQuestion(currentQuestion) {
    TCCounterEdit = TCCounterEdit + 1;
    let numOfSampleTestCase = 1;
    let numOfSecertTestCase = 1;
    let questionContainer = $(".allQuestions");
    let questionHtml = '<tr id ="Q_' + TCCounterEdit + '" class="questionContainer">' +
        '<td class ="QuestionNo" scope="row"><h4>' + TCCounterEdit + '.</h4></td>' +
        '<td>' +
        '<div class="text-wrapper">' +
        '<textarea class="question col" rows="2" cols="10" style="margin: 0px 0px 10px; width: 1022px; height: 40px;"></textarea>' +
        '</div>' +
        '<div class="marks">' +
        '<lable>Marks</lable>' +
        '<input type="number" class="question-marks">' +
        '</div>' +
        '<div class="sample-test">' +
        '<div class="sample-heading">' +
        '<p>Sample test case</p>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-sm-5">' +
        '<div class="sample-inner">' +
        '<div class="sample-inner-heading">' +
        '<p>Input</p>' +
        '</div>' +
        '<div class="textarea-wrapper">' +
        '<div class="SampleTestCaseInput_' + TCCounterEdit + '">' +
        '<textarea id="SPI_' + numOfSampleTestCase + '" class="SP_' + TCCounterEdit + '"></textarea>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="col-sm-6">' +
        '<div class="sample-inner">' +
        '<div class="sample-inner-heading">' +
        '<p>Output</p>' +
        '</div>' +
        '<div class="sample-inner-content">' +
        '<div>' +
        '<div class="textarea-wrapper">' +
        '<div class="row">' +
        '<div class="SampleTestCaseInputAndButtons_' + TCCounterEdit + '">' +
        '<div id="SPOB_' + numOfSampleTestCase + '">' +
        '<div class="col-sm-9"><textarea class="SPO_' + TCCounterEdit + '"></textarea></div>' +
        '<div class="col-sm-3">' +
        '<div class="addNewSampleTopic_' + TCCounterEdit + ' btn btn-primary col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="addSampleTestCases(' + TCCounterEdit + ',' + numOfSampleTestCase + ')">+</div>' +
        '<div class="removeNewSampleTopic_@numOfQuestions btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewSampleTestcase(' + TCCounterEdit + ',' + numOfSampleTestCase + ')">x</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div><div class="sample-test">' +
        '<div class="sample-heading">' +
        '<p>Secret Test Cases</p>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-sm-5">' +
        '<div class="sample-inner">' +
        '<div class="sample-inner-heading">' +
        '<p>Input</p>' +
        '</div>' +
        '<div class="textarea-wrapper">' +
        '<div class="SecretTestCaseInput_' + TCCounterEdit + '">' +
        '<textarea id="STI_' + numOfSecertTestCase + '" class="ST_' + TCCounterEdit + '"></textarea>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="col-sm-6">' +
        '<div class="sample-inner">' +
        '<div class="sample-inner-heading">' +
        '<p>Output</p>' +
        '</div>' +
        '<div class="">' +
        '<div>' +
        '<div class="textarea-wrapper">' +
        '<div class="SecretTestCaseInputAndButtons_' + TCCounterEdit + '">' +
        '<div id="STOB_' + numOfSecertTestCase + '" class="row">' +
        '<div class="col-sm-9"><textarea class="STO_' + TCCounterEdit + '"></textarea></div>' +
        '<div class="col-sm-3">' +
        '<div class="addNewSecretTopic_' + TCCounterEdit + ' btn btn-primary col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="addSecretTestCases(' + TCCounterEdit + ',' + numOfSecertTestCase + ')">+</div>' +
        '<div class="removeNewSecretTopic_' + TCCounterEdit + ' btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewSecretTestcase(' + TCCounterEdit + ',' + numOfSecertTestCase + ')">x</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</td>' +
        '<td>' +
        '<div class="questionButton">' +
        '<div class="addNew btn btn-primary col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="AddNewQuestion(' + TCCounterEdit + ')">+</div>' +
        '<div class="removeNew btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewQuestion(' + TCCounterEdit + ')">x</div>' +
        '</div>' +
        '</td>' +
        '</tr>';

    questionContainer.append(questionHtml);
    AddButtonToLastQuestion();
    SetQuestionCounter();



}

function RemovewQuestion(curreQuest) {
    debugger;
    let eleToRemove = $("#Q_" + curreQuest);
    var tablelength = $('tr').length;
    if (tablelength === 1) {
        var result = confirm('Do you want to delete this Question!');
        if (result == false) {
            return;
        }
        $('tr').find("textarea").val("");
        $('tr').find("input").val("");
        return;
    }
    var result = confirm('Do you want to delete this Question!');

    if (result == false) {
        return;
    }
    eleToRemove.eq(eleToRemove.length - 1).remove();
    AddButtonToLastQuestion();
    if (TCCounter != 0) {
        TCCounter = TCCounter - 1;
    }
    SetQuestionCounter();
}

function addSampleTestCases(TCCount, curreEleTestCase) {
    debugger;
    curreEleTestCase = curreEleTestCase + 1;
    var SampleTestCaseCount = $('.SP_' + TCCount).length;
    //if (TCCount != 0) {
    //    TCCount = TCCounterEdit;
    //}
    if (SampleTestCaseCount > 2) {
        return;
    }
    let sampleInputContainer = $(".SampleTestCaseInput_" + TCCount);
    let sampleInputHtml = ' <textarea id= "SPI_' + curreEleTestCase + '" class= "SP_' + TCCount + '"></textarea>';
    sampleInputContainer.append(sampleInputHtml);

    let sampleOutputContainer = $(".SampleTestCaseInputAndButtons_" + TCCount);
    let sampleOutputHtml = '<div id="SPOB_' + curreEleTestCase + '"><div class="col-sm-9"><textarea class="SPO_' + TCCount + '"></textarea></div>' +
        '<div class="col-sm-3">' +
        '<div class="addNewSampleTopic_' + TCCount + ' btn btn-primary col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="addSampleTestCases(' + TCCount + ',' + curreEleTestCase + ')">+</div>' +
        '<div class="removeNewSampleTopic_' + TCCount + ' btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewSampleTestcase(' + TCCount + ',' + curreEleTestCase + ')">x</div>' +
        '</div></div>';

    sampleOutputContainer.append(sampleOutputHtml);
    AddButtonToLastSampleTestCase(TCCount);

}

function RemovewSampleTestcase(curreEle, curreEleTestCase) {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}
    var SampletTestCaseCount = $('.SP_' + curreEle).length;
    let eleToRemoveTextAreaI = $('#SPI_' + curreEleTestCase);
    //let eleToRemoveButtons = $('.addNewSecretTopic_' + curreEle);

    let eleToRemoveTextAreaO = $('#SPOB_' + curreEleTestCase);
    if (SampletTestCaseCount === 1) {
        var result = confirm('Do you want to delete this Test Case!');
        if (result == false) {
            return;
        }
        $('#SPI_' + curreEleTestCase).val("");
        $('#SPOB_' + curreEleTestCase).find('textarea').val("");
        return;
    }
    var result = confirm('Do you want to delete this Test Case!');
    if (result == false) {
        return;
    }
    eleToRemoveTextAreaO.eq(eleToRemoveTextAreaO.length - 1).remove();
    eleToRemoveTextAreaI.eq(eleToRemoveTextAreaI.length - 1).remove();
    AddButtonToLastSampleTestCase(curreEle);

}

function addSecretTestCases(TCCount, curreEleTestCase) {
    var SecretTestCaseCount = $('.ST_' + TCCount).length;
    //if (TCCount != 0) {
    //    TCCount = TCCounterEdit;
    //}
    curreEleTestCase = curreEleTestCase + 1;
    if (SecretTestCaseCount > 4) {
        return;
    }
    let secretInputContainer = $(".SecretTestCaseInput_" + TCCount);
    let secretInputHtml = '<textarea id= "STI_' + curreEleTestCase + '" class="ST_' + TCCount + '"></textarea>';
    secretInputContainer.append(secretInputHtml);

    let secretOutputContainer = $(".SecretTestCaseInputAndButtons_" + TCCount);
    let secretOutputHtml = '<div id="STOB_' + curreEleTestCase + '" class="row">' +
        '<div class="col-sm-9"><textarea class="STO_' + TCCount + '"></textarea></div>' +
        '<div class="col-sm-3">' +
        '<div class="addNewSecretTopic_' + TCCount + ' btn btn-primary col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="addSecretTestCases(' + TCCount + ',' + curreEleTestCase + ')">+</div>' +
        '<div class="removeNewSecretTopic_' + TCCount + ' btn btn-danger col-sm-2" style="height: 3.5rem;max-width:4rem;" onclick="RemovewSecretTestcase(' + TCCount + ',' + curreEleTestCase + ')">x</div>' +
        '</div>';

    secretOutputContainer.append(secretOutputHtml);
    AddButtonToLastSecretTestCase(TCCount);
}

function RemovewSecretTestcase(curreEle, curreEleTestCase) {
    debugger;
    //if (curreEle != 0) {
    //    curreEle = TCCounter;
    //}


    var SecretTestCaseCount = $('.ST_' + curreEle).length;
    let eleToRemoveTextAreaI = $('#STI_' + curreEleTestCase);
    //let eleToRemoveButtons = $('.addNewSecretTopic_' + curreEle);

    let eleToRemoveTextAreaO = $('#STOB_' + curreEleTestCase);
    if (SecretTestCaseCount === 1) {
        var result = confirm('Do you want to delete this Test Case!');
        if (result == false) {
            return;
        }
        $('#STI_' + curreEleTestCase).val("");
        $('#STOB_' + curreEleTestCase).find('textarea').val("");
        return;
    }
    var result = confirm('Do you want to delete this Test Case!');
    if (result == false) {

        return;
    }
    eleToRemoveTextAreaO.eq(eleToRemoveTextAreaO.length - 1).remove();
    eleToRemoveTextAreaI.eq(eleToRemoveTextAreaI.length - 1).remove();
    AddButtonToLastSampleTestCase(curreEle);
}

function SetQuestionCounter() {
    debugger;
    let allQuestinoLabels = $('.QuestionNo').text();
    console.log(allQuestinoLabels);
    let counter = 1;
    $('.QuestionNo').each(function () {
        //console.log($(this).text());
        $(this).text('' + counter);
        //console.log($(this).text());
        counter += 1;
    })
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
        }
    }

}

function AddButtonToLastSampleTestCase(curreEle) {
    debugger;
    let TestCaseChildrenAddNewButton = $(".SampleTestCaseInputAndButtons_" + curreEle).children(".addNewSampleTopic_" + curreEle);
    let TestCaseChildrenremoveNewButton = $(".SampleTestCaseInputAndButtons_" + curreEle).children(".removeNewSampleTopic_" + curreEle);
    //let lastElement = TestCaseChildrenAddNewButton.length - 1;
    for (var i = 0; i < TestCaseChildrenAddNewButton.length; i++) {
        if (i === lastElement) {
            TestCaseChildrenAddNewButton.eq(lastElement).removeClass("hidden");
            TestCaseChildrenremoveNewButton.eq(lastElement).removeClass("hidden");
        } else {
            //TestCaseChildrenAddNewButton.eq(i).addClass("hidden");
            TestCaseChildrenremoveNewButton.eq(i).addClass("hidden");
        }
    }

}

function AddButtonToLastSecretTestCase(curreEle) {
    debugger;
    let TestCaseChildrenAddNewButton = $(".SampleTestCaseInputAndButtons_" + curreEle).children(".addNewSampleTopic_" + curreEle);
    let TestCaseChildrenremoveNewButton = $(".SampleTestCaseInputAndButtons_" + curreEle).children(".removeNewSampleTopic_" + curreEle);
    //let lastElement = TestCaseChildrenAddNewButton.length - 1;
    for (var i = 0; i < TestCaseChildrenAddNewButton.length; i++) {
        if (i === lastElement) {
            TestCaseChildrenAddNewButton.eq(lastElement).removeClass("hidden");
            TestCaseChildrenremoveNewButton.eq(lastElement).removeClass("hidden");
        } else {
            //TestCaseChildrenAddNewButton.eq(i).addClass("hidden");
            TestCaseChildrenremoveNewButton.eq(i).addClass("hidden");
        }
    }

}

function UpdateQuestions() {
    debugger;
    $(".loader-container").show();
    let questionDescriptionEmpty = false;
    let allQuestions = $('textarea.question');
    let allQuestionsMarks = $('.question-marks');
    let allSampleTC = $('.textarea.sampleTestCase');
    let allQuestionToAddArrr = new Array();
    let testId_Edit = $('.testID');

    for (var i = 0; i < allQuestions.length; i++) {
        let ques = i + 1;
        // If question description is empty, we skip adding it
        if (allQuestions[i].value === '') {

            alert('Question description of Question no. ' + ques + ' is empty!');
            $(".loader-container").hide();
            return;
        }
        if (allQuestionsMarks[i].value === '') {
            alert('Marks of Question no. ' + ques + ' is empty!');
            $(".loader-container").hide();
            return;
        }
        let questionToAdd = new Object();
        questionToAdd.Test_ID = testId_Edit.text();
        questionToAdd.Ques_Desc = allQuestions[i].value;
        questionToAdd.Max_Marks = allQuestionsMarks[i].value;
        questionToAdd.Ques_IsActive = true;
        let count = i + 1;
        //questionToAdd.SampleTestCase = allSampleTC;
        let allSecretTestCase = $('textarea.ST_' + count);
        let allSecretTestCaseO = $('textarea.STO_' + count);
        let allSampleTestCase = $('textarea.SP_' + count);
        let allSampleTestCaseO = $('textarea.SPO_' + count);
        let allSecretToAddArrr = new Array();
        for (var j = 0; j < allSecretTestCase.length; j++) {
            let secretTestCaseAdd = new Object();
            if (allSecretTestCase[j].value === '' || allSecretTestCaseO[j].value === '') {
                alert('One of thr Secret test case of Question no. ' + ques + ' is empty!');
                $(".loader-container").hide();
                return;
            }
            secretTestCaseAdd.Test_Case_Input = allSecretTestCase[j].value;
            secretTestCaseAdd.Test_Case_Output = allSecretTestCaseO[j].value;
            secretTestCaseAdd.Type = true;
            allSecretToAddArrr.push(secretTestCaseAdd);
        }
        for (var k = 0; k < allSampleTestCase.length; k++) {
            let sampleTestCaseAdd = new Object();
            if (allSampleTestCase[k].value === '' || allSampleTestCaseO[k].value === '') {
                alert('One of thr Sample test case of Question no. ' + ques + ' is empty!');
                $(".loader-container").hide();
                return;
            }
            sampleTestCaseAdd.Test_Case_Input = allSampleTestCase[k].value;
            sampleTestCaseAdd.Test_Case_Output = allSampleTestCaseO[k].value;
            sampleTestCaseAdd.Type = false;
            allSecretToAddArrr.push(sampleTestCaseAdd);
        }
        questionToAdd.Tbl_Stud_ProgTest_TestCases = allSecretToAddArrr;
        allQuestionToAddArrr.push(questionToAdd);
    }
    console.log(allQuestionToAddArrr);
    console.log(testId_Edit);
    if (!questionDescriptionEmpty) {
        AddAjaxFunc(allQuestionToAddArrr);
    } else {

    }
}

function AddAjaxFunc(allQuestions) {
    let url = urlToUpdateQuestion + '?id=' + testId_Edit;
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


