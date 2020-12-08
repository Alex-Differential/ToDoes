//$.connection.hub.start()
//    .done(function () {
//        console.log("It works, guysqw!!")
//        $.connection.mainHub.server.announce("Connection!! yey");
//    })
//    .fail(function () { alert("ERROR!!!!") });

//$.connection.mainHub.client.announce = function (message) {

//    $("#welcome-message").append(message + "<br />");
//}


$.connection.hub.start()
    .done(function () {
        $(document).ready(function () {
            $("#sortable").sortable({
                update: function (event, ui) {
                    var itemIds = "";
                    var itemNames = "";
                    var itemDones = ""
                    $("#sortable").find(".taskSingleInline").each(function () {
                        var itemId = $(this).attr("data-taskid");
                        var itemDes = $(this).attr("data-taskname");
                        var itemDone = $(this).attr("data-taskdone");
                        itemIds = itemIds + itemId + ",";
                        itemNames = itemNames + itemDes + ",";
                        itemDones = itemDones + itemDone + ",";
                    });
                    console.log(itemIds);
                    console.log(itemNames + "<br/>");
                    console.log(itemDones);
                    $.connection.mainHub.server.announce(itemIds, itemNames, itemDones);
                }
            });
        });
        
    })
    .fail(function () { alert("ERROR!!!!") });

$.connection.mainHub.client.announce = function (itemIds, itemNames, itemDones) {
    console.log("It works, guysqw!!" + itemIds);
    $.ajax({
        url: '@Url.Action("UpdateItem","ToDoes")',
        data: {
            itemIds: itemIds,
            itemNames: itemNames,
            itemDones: itemDones
        },
        type: 'POST',
        success: function (data) {

        },
        error: function (xhr, status, error) {

        }
    });
}
