var modal = document.getElementById('OrgDocuModal');
var img = document.getElementById('orgProofImg');
var zoomImg = document.getElementById('docOrg');
var rotateButton = document.getElementById('rotateButton');
var zoomInButton = document.getElementById('zoomInButton');
var zoomOutButton = document.getElementById('zoomOutButton');

img.onclick = function () {
    modal.style.display = "flex";
    zoomImg.src = this.src;
}

var span = document.getElementsByClassName("org-close")[0];
span.onclick = function () {
    modal.style.display = "none";
    zoomImg.style.transform = "rotate(0deg) scale(1)";
    zoomImg.dataset.rotation = 0;
    zoomImg.dataset.scale = 1;
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
        zoomImg.style.transform = "rotate(0deg) scale(1)";
        zoomImg.dataset.rotation = 0;
        zoomImg.dataset.scale = 1;
    }
}

rotateButton.onclick = function () {
    var currentRotation = parseInt(zoomImg.dataset.rotation) || 0;
    currentRotation += 90;
    zoomImg.style.transform = "rotate(" + currentRotation + "deg) scale(" + (zoomImg.dataset.scale || 1) + ")";
    zoomImg.dataset.rotation = currentRotation;
}

zoomInButton.onclick = function () {
    var currentScale = parseFloat(zoomImg.dataset.scale) || 1;
    currentScale += 0.1;
    zoomImg.style.transform = "scale(" + currentScale + ") rotate(" + (zoomImg.dataset.rotation || 0) + "deg)";
    zoomImg.dataset.scale = currentScale;
}

zoomOutButton.onclick = function () {
    var currentScale = parseFloat(zoomImg.dataset.scale) || 1;
    currentScale = Math.max(1, currentScale - 0.1);
    zoomImg.style.transform = "scale(" + currentScale + ") rotate(" + (zoomImg.dataset.rotation || 0) + "deg)";
    zoomImg.dataset.scale = currentScale;
}
