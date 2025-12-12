# FleetManagerWeb

**FleetManagerWeb** to aplikacja webowa do zarządzania flotą pojazdów, stworzona w ASP.NET Core Razor Pages.  
Umożliwia podstawowe operacje CRUD na pojazdach, kierowcach, serwisach i urlopach oraz oferuje dashboard z analizą kosztów i generowanie raportów PDF.

## Główne funkcjonalności

### Dashboard
- Liczba pojazdów i kierowców
- Łączne koszty serwisów w bieżącym roku
- Średni wiek pojazdów
- Zaległe serwisy
- Analiza miesięczna kosztów

### Pojazdy
- CRUD: tworzenie, edycja, usuwanie
- Profil pojazdu z historią serwisów
- Raport PDF z wizualizacją danych

### Kierowcy
- CRUD kierowców
- Profil kierowcy
- Analiza statystyk serwisów per kierowca
- Raport PDF z historią serwisów

### Serwisy
- CRUD historii serwisowej
- Powiązanie z pojazdami i kierowcami
- Sortowanie i filtrowanie

### Urlopy
- CRUD urlopów kierowców
- Wybór zastępcy
- Walidacja kolizji terminów
- Analiza sumaryczna dni urlopowych

### Raporty PDF
- Generowanie PDF z analizą floty
- Wykresy kosztów miesięcznych
- Podsumowania i wizualizacje danych

## Wymagania

Aby uruchomić projekt **FleetManagerWeb**, wymagane są:

### **System**
- Windows / Linux / macOS

### **Środowisko .NET**
- **.NET 8 SDK** (lub nowszy)  
  Pobierz: https://dotnet.microsoft.com/download

### **Narzędzia developerskie** (opcjonalnie)
- **Visual Studio 2022** (zalecane)  
  - Workload: *ASP.NET and web development*
  - Workload: *Entity Framework Core tools* (instalowane automatycznie)
lub
- **Visual Studio Code**  
  - Extension: *C# Dev Kit*

### **Entity Framework Core CLI**  
Do migracji i obsługi bazy:
- ```bash
  dotnet tool install --global dotnet-ef

### **Baza danych**
Projekt domyślnie korzysta z **SQLite**
(plik .db tworzony automatycznie po migracji)
Można użyć SQL Server, edytując appsettings.json.

### Technologie

- **ASP.NET Core Razor Pages**
- **Entity Framework Core** (SQLite / SQL Server)
- **Chart.js** — wykresy w dashboardzie
- **QuestPDF** + **SkiaSharp** — generowanie raportów PDF
- **Bootstrap 5** — wygląd UI
Wszystkie zależności instalowane są automatycznie przez:
-   ```bash
    dotnet restore

## Uruchomienie projektu lokalnie

1. **Sklonuj repozytorium**
   ```bash
   git clone https://github.com/jwk99/FleetManagerWeb.git
2. **Przejdź do katalogu aplikacji**
   ```bash
   cd FleetManagerWeb
3. **Przywróć zależności i wykonaj migracje**
   ```bash
   dotnet restore
   dotnet ef database update
4. **Uruchom aplikację**
   ```bash
   dotnet run
5. **Otwórz w przeglądarce**
   https://localhost:7140/
   
## Struktura projektu
- Pages — strony Razor Pages
- Data — DbContext i modele danych
- Models — klasy encji
- wwwroot — zasoby statyczne (CSS, JS)
- appsettings.json — konfiguracja połączenia z bazą danych

## Uwagi
Projekt jest częścią praktyk – demonstruje umiejętność budowy aplikacji z pełnym CRUD, raportami i analizą danych.
Dostęp do wszystkich stron można zabezpieczyć logowaniem (opcjonalne rozszerzenie).

## Autor
Jan Kruk
