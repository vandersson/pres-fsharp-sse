
function containerElement (tagName, args, subelems) {
    let div = document.createElement(tagName);
    Object.entries(args).forEach(([k,v]) => div.setAttribute(k, v));
    subelems.forEach(e => div.appendChild(e));
    return div;
}

function div (args, subelems) {
    return containerElement('div', args, subelems);
}

function text (s) {
    let span = document.createElement("span");
    span.innerText = s;
    return span;
}

function article (args, subelems) {
    return containerElement('article', args, subelems);
}

function img(src, alttext) {
    let img = document.createElement('img');
    img.setAttribute('src', src);
    img.setAttribute('alt', alttext);
    return img;
}

function h2 (s) {
    let h2 = document.createElement("h2");
    h2.innerText = s;
    return h2;
}

function b (args, subelem) {
    return containerElement('b', args, subelem);
}
function i (args, subelem) {
    return containerElement('i', args, subelem);
}
function p (args, subelem) {
    return containerElement('p', args, subelem);
}
