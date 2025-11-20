let map;

function initializeMap() {
    if (map) {
        map.remove();
    }
    const cafesDataElement = document.getElementById('cafes-data');
    const cafesJson = cafesDataElement.textContent;
    const initialCoordinates = [50.4291, 30.4815]; // Координати ДУІКТ
    const initialZoom = 15;

    // Ініціалізуємо карту
    map = L.map('map').setView(initialCoordinates, initialZoom);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    const cafes = JSON.parse(cafesJson);

    // === НОВА ЧАСТИНА ===
    // Отримуємо контейнер для списку та очищуємо його
    const resultsList = document.querySelector('.results-list');
    resultsList.innerHTML = ''; // Очищуємо від заглушок

    // Оновлюємо лічильник
    const resultsCountEl = document.getElementById('results-count');
    if (resultsCountEl) {
        resultsCountEl.textContent = `Знайдено ${cafes.length} кафе`;
    }
    // === КІНЕЦЬ НОВОЇ ЧАСТИНИ ===


    // Проходимо по кожному кафе з JSON
    cafes.forEach(cafe => {

        // 1. Додаємо маркер на карту (як і раніше)
        L.marker([cafe.latitude, cafe.longitude])
            .addTo(map)
            .bindPopup(cafe.popupContent);

        // 2. Створюємо і додаємо HTML картку в сайдбар (НОВЕ)
        const card = document.createElement('div');
        card.className = 'cafe-card';

        // Формуємо HTML для картки
        // ВАЖЛИВО: Ваш `Cafe.cs` передає лише 'Name' та 'PopupContent'.
        // Щоб реалізувати макет (рейтинг, адреса), вам треба
        // додати ці поля в `Cafe.cs` та `LeafletService.cs`.
        // Поки що я використаю `cafe.name` та заглушки.

        card.innerHTML = `
            <div class="card-image-placeholder"></div>
            <div class="card-content">
                <div class="card-title">${cafe.name || 'Назва невідома'}</div>
                <div class="card-rating">Рейтинг: (нема даних)</div>
                <button class="card-menu-btn">Меню</button>
                <a href="https://www.google.com/maps?q=${cafe.latitude},${cafe.longitude}" target="_blank" class="card-google-link">
                    Показати на Google карті
                </a>
            </div>
        `;

        // Додаємо створену картку до списку
        resultsList.appendChild(card);
    });
}