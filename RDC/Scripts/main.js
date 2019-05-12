$(document).ready(function () {
    $('.datepicker').datepicker(); //Initialise any date pickers
    //colorizeTasksTableStatusText();
});

$(document).bind("ajaxStart", function () {
    $("#spinner-loader").removeClass("d-none");
}).bind("ajaxStop", function () {
    $("#spinner-loader").addClass("d-none")
})


//function colorizeTasksTableStatusText() {

//    var tasksTable = document.getElementById("tasksTable"); // used in many places in this file .. optimize

//    if (!tasksTable) {
//        return;
//    }

//    for (var i = 0; i < tasksTable.rows.length; i++) {
//        var cell = tasksTable.rows[i].cells[3];
//        var cellText = cell.textContent.trim();

//        if (cellText == "backlog") {
//            cell.className = "text-warning";
//        }
//        else if (cellText == "In Progress") {
//            cell.className = "text-primary";

//        } else if (cellText == "Done") {
//            cell.className = "text-success";
//        }
//    }
//}

function deleteTask(task) {
    var row = $(task).closest("tr");
    var taskId = $(task).attr("data-task-id");

    $.confirm({
        title: 'Delete Task',
        buttons: {
            confirm: function () {
                $.ajax({
                    url: "/Tasks/Delete/" + taskId,
                    method: "POST",
                    contentType: false,
                    processData: false,
                    success: function () {
                        row.remove();
                    },
                    error: function () {
                        alert("error");
                    }
                })
                $.alert('Task Deleted Successfully!');
            },
            cancel: function () {
                $.alert('Task Deletion Canceled!');
            }
        }
    });
}

function deleteSubTask(subTask) {
    var row = $(subTask).closest("tr");
    console.log(row);
    var subTaskId = $(subTask).attr("data-subtask-id");
    console.log(subTaskId);

    $.confirm({
        title: 'Delete subTask',
        buttons: {
            confirm: function () {
                $.ajax({
                    url: "/SubTasks/Delete/" + subTaskId,
                    method: "POST",
                    contentType: false,
                    processData: false,
                    success: function () {
                        row.remove();
                    },
                    error: function () {
                        alert("error");
                    }
                })
                $.alert('subTask Deleted Successfully!');
            },
            cancel: function () {
                $.alert('subTask Deletion Canceled!');
            }
        }
    });
}

function postTask(form) {

    $.validator.unobtrusive.parse(form);

    if ($(form).valid()) {

        var formData = new FormData(form);

        $.ajax({
            url: form.action,
            method: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) { 
                $("#viewTasksTab").html(response);
                redirectToTab("tasksTabs", 0);
                //colorizeTasksTableStatusText();
                getAddNewTaskTabContent(); // refresh the form
            },
            error: function () {
                alert("error");
            }
        });
    }

    return false;
}

function postSubTask(form) {

    $.validator.unobtrusive.parse(form);

    if ($(form).valid()) {

        var formData = new FormData(form);
        var taskId = formData.get("TaskId");  // magic string

        $.ajax({
            url: form.action,
            method: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                $("#viewSubTasksTab").html(response);
                getTaskSubTasksList(taskId);
                redirectToTab("subTasksTabs", 0);
                //colorizeTasksTableStatusText();
                getAddNewSubTaskTabContent(); // refresh the form
            },
            error: function () {
                alert("error");
            }
        });
    }

    return false;
}

function redirectToTab(tabsContainerId, tabNumber) {
    $("#" + tabsContainerId + " li a:eq(" + tabNumber + ")").tab("show");
}


function getAddNewTaskTabContent() {

    $.ajax({
        url: "/Tasks/Create",
        method: "GET",
        success: function (response) {
            $("#taskTab").html(response);
            $('.datepicker').datepicker(); //Initialise date pickers
        },
        error: function () {
            alert("error");
        }
    });
}

function getAddNewSubTaskTabContent() {

    $.ajax({
        url: "/SubTasks/Create",
        method: "GET",
        success: function (response) {
            $("#subTaskTab").html(response);
            $('.datepicker').datepicker(); //Initialise date pickers
        },
        error: function () {
            alert("error");
        }
    });
}

function renameTabTitle(tabId, title) {
    $("#" + tabId).html(title);
}

function redirectToEditTaskTab(url) {
    $.ajax({
        url: url,
        method: "GET",
        success: function (response) {
            $("#taskTab").html(response);
            $('.datepicker').datepicker(); //Initialise date pickers
            redirectToTab("tasksTabs", 1);
        },
        error: function () {
            alert("error");
        }
    })
}

function redirectToEditSubTaskTab(url) {
    $.ajax({
        url: url,
        method: "GET",
        success: function (response) {
            $("#subTaskTab").html(response);
            $('.datepicker').datepicker(); //Initialise date pickers
            redirectToTab("subTasksTabs", 1);
        },
        error: function () {
            alert("error");
        }
    })
}


function redirectToTaskDetailsTab(url) {

    $.ajax({
        url: url,
        method: "GET",
        success: function (response) {
            $("#taskTab").html(response);
            redirectToTab("tasksTabs", 1);
        },
        error: function () {
            alert("error");
        }
    })
}

function redirectToSubTaskDetailsTab(url) {
    $.ajax({
        url: url,
        method: "GET",
        success: function (response) {
            $("#subTaskTab").html(response);
            redirectToTab("subTasksTabs", 1);
        },
        error: function () {
            alert("error");
        }
    })
}

function searchTasks(e) {

    var inputText = e.target.value.toLowerCase();
    var tasksRowsList = document.querySelector(".tbody");
    var tasksTable = document.getElementById("tasksTable");

    var numberOfTableColumns = tasksTable.rows[0].cells.length;

    Array.from(tasksRowsList.children).forEach(function (tr) {
        var rowContent = "";

        Array.from(tr.children).forEach(function (td, index) {
            // skip controls column
            if (isControlsColumn(index, numberOfTableColumns)) {
                return;
            }
            rowContent += td.textContent.toLowerCase();
        });

        if (rowContent.toLowerCase().indexOf(inputText) == -1) {
            tr.style.display = "none";
        } else {
            tr.style.display = "table-row";
        }
    })
}

function searchSubTasks(e) {

    var inputText = e.target.value.toLowerCase();
    var subTasksRowsList = document.querySelector(".tbody");
    var subTasksTable = document.getElementById("subTasksTable");

    var numberOfTableColumns = subTasksTable.rows[0].cells.length;

    Array.from(subTasksRowsList.children).forEach(function (tr) {
        var rowContent = "";

        Array.from(tr.children).forEach(function (td, index) {
            // skip controls column
            if (isControlsColumn(index, numberOfTableColumns)) {
                return;
            }
            rowContent += td.textContent.toLowerCase();
        });

        if (rowContent.toLowerCase().indexOf(inputText) == -1) {
            tr.style.display = "none";
        } else {
            tr.style.display = "table-row";
        }
    })
}

function isControlsColumn(index, numberOfTableColumns) {
    return index == numberOfTableColumns - 1;
}

function sortTasksAsec(e) {
    e.preventDefault();

    $.ajax({
        url: "/Tasks/ViewAllTasksTable/asec",
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#tasksTableContainer").html(response);
            //colorizeTasksTableStatusText();
        },
        error: function () {
            alert("error");
        }
    })
}

function sortTasksDesc(e) {
    e.preventDefault();

    $.ajax({
        url: "/Tasks/ViewAllTasksTable/desc",
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#tasksTableContainer").html(response);
            //colorizeTasksTableStatusText();
        },
        error: function () {
            alert("error");
        }
    })
}

function filterTasksByStatus(filter) {

    $.ajax({
        url: "/Tasks/ViewAllTasksTable/ /" + filter.value.toString(),
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#tasksTableContainer").html(response);
            //colorizeTasksTableStatusText();
        },
        error: function () {
            alert("error");
        }
    })
}

function redirectToTasks() {
    $.ajax({
        url: "/Tasks/TasksPartial",
        method: "GET",
        success: function (response) {
            $("#page").html(response);
        },
        error: function () {
            alert("error");
        }
    });
}

function redirectToSubTasks(task) {

    var taskId = $(task).attr("data-task-id");
    getTaskSubTasksList(taskId)
}

function getTaskSubTasksList(taskId) {

    $.ajax({
        url: "/SubTasks/Index/" + taskId,
        method: "GET",
        success: function (response) {
            $("#page").html(response);
            $('.datepicker').datepicker(); //Initialise date pickers
        },
        error: function () {
            alert("error");
        }
    });
}