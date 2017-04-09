$(document).ready(function () {
    $('.card').on('click', '.card__remove-card-btn', deleteCard);
    $('button').on('click', showForm);
    $('.list').on('click', '.list__remove-list-btn', deleteList);
    $('.list__cards-container').sortable({
        connectWith: ".list__cards-container",
        update: updateCardPosition,
        start: function (event, ui) {
            var oldListId = ui.item.closest(".list").data("list-id");
            ui.item.attr('data-prevListId', oldListId);
        }
    });

    function updateCardPosition(event, ui) {
        if (this === ui.item.parent()[0]) {
        var listId = $(this).closest(".list").data("list-id");
        var cardId = ui.item.find(".card__remove-card-btn").data("card-id");
        var actualPositionCard = ui.item.index();
        var oldListId = ui.item.attr('data-prevListId');
        $(this).removeAttr('data-prevListId');

        $.ajax({
            method: "POST",
            url: "/Board/UpdateCardPosition",
            data: { listId: listId, cardId: cardId, positionCard: actualPositionCard, oldListId: oldListId }
        });
    }
    }

    function showForm(event) {
        event.stopPropagation();
        $(this).prev().removeClass("hidden-form");
    }

    function deleteList(event) {
        event.stopPropagation();
        var listId = +$(this).closest(".list").data("list-id");

        $.ajax({
            method: "DELETE",
            url: "/Board/DeleteList",
            data: { listId: listId },
            success: onSuccess,
            error: onError
        });


        function onSuccess(data) {
            if (isNaN(data) || data === 0) {
                return;
            }

            $(event.target).closest(".list").remove();
        }

        function onError(data) { }
    }

    function deleteCard(event) {
        var cardId = +$(event.target).data('cardId');
        var listId = +$(this).closest(".list").data("list-id");

        $.ajax({
            method: "DELETE",
            url: "/Board/DeleteCard",
            success: onSuccess,
            error: onError,
            data: { cardId: cardId, listId: listId }    
        });

        function onSuccess(data) {
            if (isNaN(data) || data === 0) {
                return;
            }

            $(event.target).closest(".card").remove();
        }

        function onError(data) { }
    }
});
