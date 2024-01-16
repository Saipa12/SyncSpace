function _isBottomSheetChild(elem) {
    while (elem.parentElement != null) {
        elem = elem.parentElement;
        if (elem.className == 'bottom-sheet ')
            return true;
    }
    return false;
}

class BottomSheetView {
    initialize(id) {
        this._isOpen = false;

        this._html = document.getElementById(id);

        if (this._html == null)
            return;
        this._hover = this._html.querySelector("#hover");

        this._hover.addEventListener('touchstart', this.touchStart.bind(this));
        this._hover.addEventListener('mousedown', this.touchStart.bind(this));
        document.addEventListener('mousedown', this.clicked.bind(this));
        document.addEventListener('touchstart', this.clicked.bind(this));
    }

    dispose() {
        delete this._html;
    }

    openBottomSheet(height) {
        this._html.style.height = height + '%';
        this._isOpen = true;

        this._hover.addEventListener('touchstart', this.touchStart.bind(this));
        this._hover.addEventListener('mousedown', this.touchStart.bind(this));
        document.addEventListener('mousedown', this.clicked.bind(this));
        document.addEventListener('touchstart', this.clicked.bind(this));
    }

    closeBottomSheet() {
        this._html.style.height = '0%';
        this._isOpen = false;

        this._hover.removeEventListener('touchstart', this.touchStart.bind(this));
        this._hover.removeEventListener('mousedown', this.touchStart.bind(this));
        document.removeEventListener('mousedown', this.clicked.bind(this));
        document.removeEventListener('touchstart', this.clicked.bind(this));
    }

    touchStart(e) {
        this._isHovered = true;
        this._startY = e.clientY;

        window.addEventListener('mousemove', this.mouseMove.bind(this));
        window.addEventListener('mouseup', this.touchEnd.bind(this));
        window.addEventListener('touchmove', this.touchMove.bind(this));
        window.addEventListener('touchend', this.touchEnd.bind(this));
    }

    mouseMove(e) {
        if (e.buttons == 1 && this._isHovered) {
            let curY = e.clientY;
            this._height = window.innerHeight - curY;
            this._html.style.height = this._height + 15 + 'px';
        }
        return true;
    }

    touchMove(e) {
        if (e.touches.length == 1) {
            let curY = e.touches[0].clientY;

            this._height = window.innerHeight - curY;
            this._html.style.height = this._height + 15 + 'px';
        }
        return true;
    }

    touchEnd(e) {
        if (this._height <= 150) {
            this.closeBottomSheet();
        }

        this._isHovered = false;
        window.removeEventListener('mousemove', this.mouseMove.bind(this));
        window.removeEventListener('mouseup', this.touchEnd.bind(this));
        window.removeEventListener('touchmove', this.touchMove.bind(this));
        window.removeEventListener('touchend', this.touchEnd.bind(this));
        return true;
    }

    clicked(e) {
        let temp = _isBottomSheetChild(e.target);
        if (this._isOpen && !this._isHovered && !temp) {
            this.closeBottomSheet();
        }
    }
};

window.bottomSheetView = function () {
    return new BottomSheetView();
};        
