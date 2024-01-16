export default class OnlineStatus {
    initialize() {
        this._html = document.createElement('div');
        this._html.className = "online-status";

        this._inner = document.createElement('div');
        this._inner.className = "online-status-inner"
        this._html.appendChild(this._inner);
    }

    setStatus(status) {
        this._html.style.backgroundColor = status ? "#00ff7f" : "#f26065";
    }
}