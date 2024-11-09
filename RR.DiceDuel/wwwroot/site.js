window.playSound = function (url) {
    const audio = new Audio(url);
    audio.play();
};

window.onBeforeUnload = {
    subscribeBeforeUnload: function (dotNetHelper) {
        window.addEventListener("beforeunload", function (event) {
            dotNetHelper.invokeMethodAsync("OnBeforeUnload");
        });
    }
};