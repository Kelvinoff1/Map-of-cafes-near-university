let map;

function initializeMap() {
    if (map) {
        map.remove();
    }
    const cafesDataElement = document.getElementById('cafes-data');
    const cafesJson = cafesDataElement.textContent;
    const initialCoordinates = [50.4501, 30.5234];
    const initialZoom = 13;
    map = L.map('map').setView(initialCoordinates, initialZoom);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    const cafes = JSON.parse(cafesJson);

    cafes.forEach(cafe => {
        L.marker([cafe.latitude, cafe.longitude])
            .addTo(map)
            .bindPopup(cafe.popupContent);
    });
}