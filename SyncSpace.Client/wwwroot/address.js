function getAddress(lat, lon, zoom) {
    fetch("https://nominatim.openstreetmap.org/search.php?q=" + lat + "," + lon + "&format=json&accept-language=ru-RU&addressdetails=1")
        .then(response => response.json())
        .then(json => {
            let items = getZoomAddressArgs(zoom);

            let addressString = "";
            let city = "";

            items.forEach(item => {
                if (item == "city")
                    city = json[0]["address"][item];
                if (item == "state" && city == json[0]["address"][item])
                    return;

                if (json[0]["address"][item] != null)
                    addressString += json[0]["address"][item] + "</br>";
            });

            let bar = document.querySelector(".address-bar");
            if (bar != null)
                bar.innerHTML = addressString;
        })
}

function getZoomAddressArgs(zoom) {
    switch (true) {
        case (zoom < 9):
            return ["country"];
        case (zoom < 13):
            return ["city", "town", "village"];
        case (zoom < 17):
            return ["state", "suburb"];
        default:
            return ["road"];
    }
}