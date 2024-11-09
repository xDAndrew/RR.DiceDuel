console.log("hello")

window.playSound = function (url) {
    const audio = new Audio(url);
    audio.play();
};