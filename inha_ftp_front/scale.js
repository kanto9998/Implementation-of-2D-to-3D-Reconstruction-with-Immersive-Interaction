
function setScale() {
    var target = document.querySelectorAll( 'body > div' )[ 0 ];
    if( target == null ) return;

    var windowWidth = window.innerWidth;
    var windowHeight = window.innerHeight;

    var targetWidth = target.clientWidth;
    var targetHeight = target.clientHeight;
    var bRatio = windowWidth / targetWidth;
    var yRatio = windowHeight / targetHeight;
    var ratio = bRatio > yRatio ? yRatio : bRatio;

    ( target.style.left = ( windowWidth - targetWidth * ratio ) / 2 + "px" ),
    ( target.style.top = ( windowHeight - targetHeight * ratio ) / 2 + "px" ),
    ( target.style.transformOrigin = "0% 0%" ),
    ( target.style.MsTransformOrigin = "0% 0%" ),
    ( target.style.MozTransformOrigin = "0% 0%" ),
    ( target.style.WebkitTransformOrigin = "0% 0%" ),
    ( target.style.transform = "scale(".concat(ratio, ")" )),
    ( target.style.MsTransform = "scale(".concat(ratio, ")" )),
    ( target.style.MozTransform = "scale(".concat(ratio, ")" )),
    ( target.style.WebkitTransform = "scale(".concat(ratio, ")" ));
}

setScale();
window.addEventListener( 'resize', setScale, true );

window.onload = function() {
    setScale();
};