$(document).ready(function () {
    $('.card').on('click', '.card__remove-card-btn', deleteCard);

    function deleteCard(event) {
        var cardId = +$(event.target).data('cardId');

        $.ajax({
            method: "DELETE",
            url: "/Board/DeleteCard",
            success: onSuccess,
            error: onError,
            data: { cardId: cardId }
            
        });

        function onSuccess(data) {
            if (isNaN(data) || data === 0) {
                return;
            }

            $(event.target).closest(".card").remove();
        }

        function onError(data) {

        }
    }
});
