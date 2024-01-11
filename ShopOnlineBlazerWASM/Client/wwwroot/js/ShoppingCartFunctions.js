window.MakeUpdateQtyButtonVisible = function (id, visible) {
    const updateQtyButton = document.querySelector("button[data-itemId='" + id + "']");
    if (updateQtyButton) {
        updateQtyButton.style.display = visible ? "inline-block" : "none";
    }
};
