class QrCode {
    initialize(id, args) {
        let options = {
            width: args[0],
            height: args[1],
            type: "svg",
            data: args[2],
           /* image: "https://upload.wikimedia.org/wikipedia/commons/5/51/Facebook_f_logo_%282019%29.svg",*/
            dotsOptions: {
                type: "rounded",
                gradient: {
                    type: "radial",
                    colorStops: [{ offset: 0, color: '#940cf1' }, { offset: 1, color: '#0e1fe9' }]
                }
            },
            backgroundOptions: {
                color: "#00000000",
            },
            imageOptions: {
                crossOrigin: "anonymous",
                margin: 20
            },
            cornersSquareOptions: {
                type: "extra-rounded",
                gradient: {
                    type: "radial",
                    colorStops: [{ offset: 0, color: '#940cf1' }, { offset: 1, color: '#0e1fe9' }]
                },
            },
            cornersDotSquareOptions: {
                type: "None",
                gradient: {
                    type: "radial",
                    colorStops: [{ offset: 0, color: '#940cf1' }, { offset: 1, color: '#0e1fe9' }]
                }
            }
        };

        this._qrCode = new QRCodeStyling(options);

        this._qrCode.append(document.getElementById(id));
        /* this._qrCode.download({ name: "qr", extension: "svg" });*/
    }
};

window.qrCode = function () {
    return new QrCode();
};

// ������������