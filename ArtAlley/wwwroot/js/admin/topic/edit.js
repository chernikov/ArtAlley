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
            fileuploaddone : function (e, data) {
                alert('Done');
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
    }
}




var edit = null;
$(_ => {
    edit = new Edit();
    edit.init();
});