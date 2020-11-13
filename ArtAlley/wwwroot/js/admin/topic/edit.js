function Edit() {
    var _this = this;
    this.init = function ()
    {
      
        $("#uploadImageBtn").on('click',
            function () {
                $('#imageUploader').click();
            }
        );

        $('#imageUploader').fileupload({
            url: "/image",
            dataType: 'json',
            add: function (e, data) {
                var uploadErrors = [];
                if (data.originalFiles[0]['size'].length && data.originalFiles[0]['size'] > 15000000) {
                    uploadErrors.push('Filesize is too big');
                }
                if (uploadErrors.length > 0) {
                    alert(uploadErrors.join("\n"));
                } else {
                    data.submit();
                }
            },
            done: function (e, data) {
                var path = data.result.path;
                $("#ImagePath").val(path);
                $("#PreviewImage").attr("src", path);

            },
            fail: function (e, data) {
                console.log(e, data);
            }
        });

        $(".deleteItem").on("click", removeFile);

        $("#addFile").on("click",
            function () {
                $('#fileUploader').click();
            });

        $('#fileUploader').fileupload({
            url: "/file",
            dataType: 'json',
            add: function (e, data) {
                var uploadErrors = [];
                if (data.originalFiles[0]['size'].length && data.originalFiles[0]['size'] > 50000000) {
                    uploadErrors.push('Filesize is too big');
                }
                if (uploadErrors.length > 0) {
                    alert(uploadErrors.join("\n"));
                } else {
                    data.submit();
                }
            },
            done: function (e, data) {
                var path = data.result.path;
                var name = data.result.name;
                addTopicFile(name, path);
            },
            fail: function (e, data) {
                console.log(e, data);
            }
        });
    }

    function addTopicFile(name, url) {
        var total = $("#TopicFilesList").children().length;
        var row = $("<tr/>").attr("id", "TopicFilesWrapper" + total).appendTo("#TopicFilesList");
        var field = $("<td/>").appendTo(row);
        $("<input/>").attr("type", "hidden").attr("name", "TopicFiles[" + total + "].Id").attr("id", "TopicFiles[" + total + "]_Id").appendTo(field);
        $("<input/>").attr("type", "hidden").attr("name", "TopicFiles[" + total + "].Url").attr("id", "TopicFiles[" + total + "]_Url").val(url).appendTo(field);
        $("<input/>").attr("type", "text").attr("name", "TopicFiles[" + total + "].Name").attr("id", "TopicFiles[" + total + "]_Name").val(name).appendTo(field);
        $("<td>0</td>").appendTo(row);
        var field3 = $("<td/>").appendTo(row);
        var button = $(`<div class="btn btn-primary deleteItem">Видалити</div>`).appendTo(field3);
        button.click(removeFile);
    }

    function removeFile() {
        var total = $("#TopicFilesList").children().length;
        var container = $(this).parents("tr");
        var startNum = parseInt(container.attr("id").substring("TopicFilesWrapper".length));
        //удаляем этот div
        container.remove();
        for (var i = startNum + 1; i < total; i++) {
            //функция пересчета аттрибутов name и id
            RecalculateNamesAndIds(i);
        }
    }

    function RecalculateNamesAndIds(number) {
        var prevNumber = number - 1;
        $("#TopicFilesWrapper" + number).attr("id", "TopicFilesWrapper" + prevNumber);
        //скобки "[" и "]" которые присутствуют в id DOM-объекта в jquery селекторе необходим экранировать двойным обратным слэшем \\
        $("#TopicFiles\\[" + number + "\\]_Id").attr("id", "TopicFiles[" + prevNumber + "]_Id").attr("name", "TopicFiles[" + prevNumber + "].Id");
        $("#TopicFiles\\[" + number + "\\]_Url").attr("id", "TopicFiles[" + prevNumber + "]_Url").attr("name", "TopicFiles[" + prevNumber + "].Url");
        $("#TopicFiles\\[" + number + "\\]_Name").attr("id", "TopicFiles[" + prevNumber + "]_Name").attr("name", "TopicFiles[" + prevNumber + "].Name");
    }

}




var edit = null;
$(_ => {
    edit = new Edit();
    edit.init();
});