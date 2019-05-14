$(document).ready(function () {
    $('.datepicker').datepicker(); //Initialise any date pickers
});

$(document).bind("ajaxStart", function () {
    $("#spinner-loader").removeClass("d-none");
}).bind("ajaxStop", function () {
    $("#spinner-loader").addClass("d-none")
})

function resetSearchInput() {
    $("#searchTasks").val("");
    $("#searchSubTasks").val("");
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
        var taskId = formData.get("TaskId");

        $.ajax({
            url: form.action,
            method: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                $("#viewSubTasksTab").html(response);
                getSubTasksListOfSpecificTask(taskId);
                redirectToTab("subTasksTabs", 0);
                getAddNewSubTaskTabContent(); // refresh the form
            },
            error: function () {
                alert("error");
            }
        });
    }

    return false;
}

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
    var subTaskId = $(subTask).attr("data-subtask-id");

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



function redirectToTab(tabsContainerId, tabNumber) {
    $("#" + tabsContainerId + " li a:eq(" + tabNumber + ")").tab("show");
}

function renameTabTitle(tabId, title) {
    $("#" + tabId).html(title);
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

function redirectToTasks() {
    $.ajax({
        url: "/Tasks/TasksPagePartial",
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
    getSubTasksListOfSpecificTask(taskId)
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
        },
        error: function () {
            alert("error");
        }
    })
}

function filterTasksByStatus(filter) {

    $.ajax({
        url: "/Tasks/ViewAllTasksTable/null/" + filter.value.toString(),
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#tasksTableContainer").html(response);
        },
        error: function () {
            alert("error");
        }
    })
}

function sortSubTasksAsec(e) {
    e.preventDefault();

    $.ajax({
        url: "/SubTasks/ViewAllSubTasksTable/asec",
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#subTasksTableContainer").html(response);
        },
        error: function () {
            alert("error");
        }
    })
}

function sortSubTasksDesc(e) {
    e.preventDefault();

    $.ajax({
        url: "/SubTasks/ViewAllSubTasksTable/desc",
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#subTasksTableContainer").html(response);
        },
        error: function () {
            alert("error");
        }
    })
}

function filterSubTasksByStatus(filter) {

    $.ajax({
        url: "/SubTasks/ViewAllSubTasksTable/null/" + filter.value.toString() + "/null",
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#subTasksTableContainer").html(response);
        },
        error: function () {
            alert("error");
        }
    })
}


function filterSubTasksByPriority(filter) {

    $.ajax({
        url: "/SubTasks/ViewAllSubTasksTable/null/null/" + filter.value.toString(),
        method: "GET",
        contentType: false,
        processData: false,
        success: function (response) {
            $("#subTasksTableContainer").html(response);
        },
        error: function () {
            alert("error");
        }
    })
}

function getSubTasksListOfSpecificTask(taskId) {

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