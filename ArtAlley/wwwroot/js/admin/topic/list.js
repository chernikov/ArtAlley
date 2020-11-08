function List() {
    var _this = this;
    var idForDelete = 0;
    this.init = function () {
        $(".deleteItem").on("click", function () {
            var id = $(this).data("id");
            _this.openConfirmModal(id);
        });

        $("#ConfirmBtn").on("click", function () {
            window.location.href = "/admin/topic/delete/" + idForDelete;
        });
    }

    this.openConfirmModal = function (id) {
        idForDelete = id;
        console.log(idForDelete);
        $('#confirmModal').modal('show');

    }
}




var list = null;
$(_ => {
    list = new List();
    list.init();
});