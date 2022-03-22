function ChangeUpdateBtnVisibility(id, isVisible) {
    const updateBtn = document.querySelector("button[data-itemId='" + id + "']");
    if (isVisible) {
        updateBtn.style.display = "inline-block";

    }
    else {
        updateBtn.style.display = "none";
    }
}