function goHome(){
    window.location = "/Home/Index";
}

function redirectTo(url) {
    window.location = "/Home/RedirectTo?Path=" + encodeURIComponent(url);
}

function goLogin() {
    window.location = "/Account/Login";
}

function ajaxFormSubmit(e, successFunc) {
    var url = $(e).closest("form").attr("action");
    var formData = $(e).closest("form").serialize();
    $.ajax({
        type: "POST",
        url: url,
        data: formData,
        success: successFunc
    });
}

function saveUserJob(jobId) {
    $.ajax({
        url: "/Job/SaveUserJob",
        type: "POST",
        async: true,
        data: { jobId: jobId},
        traditional: true,
        error: function (xhr, textStatus, error) {
            //ReuestError(xhr, textStatus, error);
        },
        success: function (data) {
            console.log(data);
        }
    });
}

function reloadGridonSearch(grid) {
    var pg = $("[data-role='pager']").data("kendoPager")
    if (pg != null) {
        var pgNum = pg.page();
        if (pgNum > 1) {
            $("[data-role='pager']").data("kendoPager").page(1);
        } else {
            grid.dataSource.read();
        }
    } else {
        grid.dataSource.read();
    }

}