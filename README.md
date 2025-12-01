# ☕ Map of Cafes Near University

A high-performance ASP.NET Core MVC web application designed to help students and staff of the **State University of Information and Communication Technologies** find the best coffee spots nearby in Kyiv.

The project demonstrates a **clean architecture** approach using completely free open-source technologies, avoiding paid map services.

🚀 **Live Demo:** [http://91.218.215.251/](http://91.218.215.251/)

---

## 📖 Table of Contents
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture & Data Flow](#-architecture--data-flow)
- [Configuration](#-configuration)
- [Installation & Run](#-installation--run)

---

## 🌟 Features

* **🗺️ Interactive Map:** Built with **Leaflet.js**, offering a smooth and responsive experience.
* **🆓 100% Free:** Uses **OpenStreetMap** data via the Overpass API. No Google Maps API keys or billing required.
* **⚡ High Performance:** Implements server-side **In-Memory Caching**. Data is fetched from the external API only once every few hours; subsequent requests are served instantly from RAM.
* **📍 Localized Search:** The logic is optimized to search within a specific **Bounding Box** around the university coordinates, ensuring relevance and speed.
* **🏗️ Solid Backend:** Data processing, parsing, and error handling happen on the server (C#), keeping the client-side lightweight.

---

## 🛠 Tech Stack

### Backend
* **Framework:** ASP.NET Core 9.0 (MVC Pattern)
* **Language:** C#
* **Networking:** `IHttpClientFactory` for efficient API requests.
* **Caching:** `IMemoryCache` for temporary data storage.
* **JSON:** `System.Text.Json` for serialization.
* **DI:** Native Dependency Injection container.

### Frontend
* **Map Engine:** [Leaflet.js](https://leafletjs.com/) v1.9.4
* **Map Tiles:** OpenStreetMap Standard
* **Logic:** Vanilla JavaScript (ES6+)
* **Styling:** CSS3 (Flexbox for the sidebar layout)

### Data Source
* **Provider:** [Overpass API](https://overpass-api.de/) (Querying raw OSM nodes with `amenity=cafe`).

---

## 🏗 Architecture & Data Flow

The project follows the **Service Layer Pattern** to ensure separation of concerns.

### Project Structure
```text
MapOfCafesNearUniversity/
├── Controllers/          # Handles HTTP requests (HomeController)
├── Services/             # Business logic (LeafletService, OverpassApiClient)
├── Models/               # Internal application models (Cafe)
├── DTOs/                 # Data Transfer Objects for external API (OverpassResponse)
├── Settings/             # Strongly typed configuration classes
├── Views/                # Razor Views (HTML generation)
└── wwwroot/              # Static assets (JS, CSS)
```

---

## Data Flow Execution

1. **User Request:** The user opens the website.
2. **Controller:** `HomeController` requests cafe data from `ILeafletService`.
3. **Cache Check:** `LeafletService` checks if data exists in `IMemoryCache`.
* **Hit:** Returns data immediately.
* **Miss:** Calls `OverpassApiClient`.
4. **External API Call:** `OverpassApiClient` constructs a query using the Bounding Box from settings and fetches data from the Overpass API.
5. **Mapping:** The raw JSON response is deserialized into DTOs and mapped to the internal `Cafe` model (extracting name, address, hours, website).
6. **Response:** The controller passes the structured data to the View, which renders the map.

---

## ⚙️ Configuration

The application is highly configurable via `appsettings.json`. The geographical search area is not hardcoded but defined in the configuration.
```text
"OverpassApi": {
  "BaseUrl": "[https://overpass-api.de/](https://overpass-api.de/)",
  // Coordinates (South, West, North, East) focused on Solomianka district
  "BoundingBox": "50.41,30.44,50.45,30.50",
  "QueryTemplate": "[out:json];node[\"amenity\"=\"cafe\"]({0});out;"
}
```

---

## 🚀 Installation & Run

1. Clone the repository:
```text
git clone [https://github.com/Kelvinoff1/Map-of-cafes-near-university.git](https://github.com/Kelvinoff1/Map-of-cafes-near-university.git)
```

2. Navigate to the project folder:
```text
cd Map-of-cafes-near-university
```

3. Restore dependencies:
```text
dotnet restore
```

4. Run the application:
```text
dotnet run
```

5. Open your browser at `http://localhost:5000`.
