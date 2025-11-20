let map;
let markersLayer; // Група для маркерів, щоб ми могли їх видаляти при пошуку
let allCafes = []; // Тут зберігатимемо всі завантажені кафе

function initializeMap() {
    if (map) {
        map.remove();
    }

    // 1. Ініціалізація карти
    const initialCoordinates = [50.4291, 30.4815]; // ДУІКТ
    const initialZoom = 15;

    map = L.map('map').setView(initialCoordinates, initialZoom);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    // Створюємо шар для маркерів
    markersLayer = L.layerGroup().addTo(map);

    // 2. Отримання даних
    const cafesDataElement = document.getElementById('cafes-data');
    if (!cafesDataElement) return;

    const cafesJson = cafesDataElement.textContent;
    allCafes = JSON.parse(cafesJson); // Зберігаємо оригінальні дані глобально

    // 3. Перший рендер (показуємо всі, вони вже відсортовані бекендом)
    renderCafes(allCafes);

    // 4. Налаштування пошуку
    const searchInput = document.getElementById('search-input');
    if (searchInput) {
        searchInput.addEventListener('input', (e) => {
            const searchText = e.target.value.toLowerCase();

            // Фільтруємо масив
            const filteredCafes = allCafes.filter(cafe =>
                (cafe.name && cafe.name.toLowerCase().includes(searchText))
            );

            // Перемальовуємо карту та список
            renderCafes(filteredCafes);
        });
    }
}

// Функція для відображення списку та маркерів
function renderCafes(cafesToRender) {
    const resultsList = document.querySelector('.results-list');
    const resultsCountEl = document.getElementById('results-count');

    // Очищуємо список та маркери перед новим рендером
    resultsList.innerHTML = '';
    markersLayer.clearLayers();

    // Оновлюємо лічильник
    if (resultsCountEl) {
        resultsCountEl.textContent = `Знайдено ${cafesToRender.length} кафе`;
    }

    cafesToRender.forEach(cafe => {
        // А. Додаємо маркер у шар (LayerGroup)
        L.marker([cafe.latitude, cafe.longitude])
            .addTo(markersLayer)
            .bindPopup(cafe.popupContent);

        // Б. Створюємо картку
        const card = document.createElement('div');
        card.className = 'cafe-card';

        card.innerHTML = `
            <div class="card-content">
                <div class="card-title">${cafe.name || 'Назва невідома'}</div>
                <a href="https://www.google.com/maps/search/?api=1&query=${cafe.latitude},${cafe.longitude}" target="_blank" class="card-google-link">
                    Показати на Google карті
                </a>
            </div>
        `;

        // Додаємо клік по картці, щоб показати кафе на мапі (зум до нього)
        card.addEventListener('click', () => {
            map.setView([cafe.latitude, cafe.longitude], 18);
        });

        resultsList.appendChild(card);
    });
}