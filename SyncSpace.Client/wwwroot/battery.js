export default class Battery {
    initialize() {
        this._html = document.createElement("div");
        this._html.className = "battery";

        this._charge = document.createElement("div");
        this._charge.className = "battery-charge";

        this._text = document.createElement("div");
        this._text.className = "battery-text";

        this._html.appendChild(this._charge);
        this._html.appendChild(this._text);
    }

    setBatteryLevel(width) {
        this._charge.style.width = width + '%';
        if (width > 95) {
            console.log((width - 50) / 50);
            console.log(convert("00ff00", "ffff00", (width - 50) / 50));
        }
        this._charge.style.backgroundColor = width > 50 ? convert("00ff00", "ffff00", (width - 50) / 50) : convert("ffff00", "ff0000", width / 50);
        this._text.innerHTML = width.toString();
    }
}

function componentToHex(c) {
    let hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

function convert(col1, col2, p) {
    const rgb1 = parseInt(col1, 16);
    const rgb2 = parseInt(col2, 16);

    const [r1, g1, b1] = toArray(rgb1);
    const [r2, g2, b2] = toArray(rgb2);

    const q = 1 - p;
    const rr = Math.round(r1 * p + r2 * q);
    const rg = Math.round(g1 * p + g2 * q);
    const rb = Math.round(b1 * p + b2 * q);

    console.log(rr + ';' + rg + ';' + rb);
    return "#" + componentToHex(rr) + componentToHex(rg) + componentToHex(rb);
    //return (Number(rr << 16).toString(16) + Number(rg << 8).toString(16) + Number(rb).toString(16)).toString(16);
}

function toArray(rgb) {
    const r = rgb >> 16;
    const g = (rgb >> 8) % 256;
    const b = rgb % 256;

    return [r, g, b];
}