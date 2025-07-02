# Stock Market Project

## Dokumentacja projektu

### 1. Opis działania gry
Gra symuluje inwestowanie na rynku akcji. Gracz zarządza portfelem, kupuje i sprzedaje akcje różnych spółek, obserwuje zmiany cen oraz reaguje na wydarzenia globalne, sektorowe i rynkowe. Rozgrywka podzielona jest na tury, a każda tura może przynieść nowe wydarzenia wpływające na ceny akcji. Celem gracza jest osiągnięcie jak najwyższej wartości portfela do końca gry.

### 2. Główne elementy rozgrywki:

- Kupno i sprzedaż akcji: Gracz może kupować i sprzedawać akcje dostępnych spółek w każdej turze.
- Zmiany cen: Ceny akcji zmieniają się losowo w każdej turze, z możliwością dodatkowych zmian przez wydarzenia rynkowe. Kupno i sprzedaż akcji również ma niewielki wpływ na ceny.
- Wydarzenia: System eventów generuje wydarzenia globalne, sektorowe i rynkowe, które wpływają na ceny akcji.
- Historia: Wszystkie ważne akcje i wydarzenia są zapisywane w historii gry.
- Podsumowanie: Po zakończeniu gry wyświetlane jest podsumowanie wyników, w tym najlepsza i najgorsza inwestycja oraz historia rozgrywki.

### 3. Omówienie decyzji projektowych

Gra została zaprojektowana z podziałem na warstwy logiki:

- `GameManager` inicjalizuje główne systemy i zarządza turami.

- `MarketManager` obsługuje mechanikę rynku i aktualizację cen.

- `EventManager` odpowiada za system zdarzeń (eventy rynkowe).

- `PlayerPortfolio` śledzi inwestycje gracza i kontroluje przepływ gotówki.

- `UIManager` odpowiada za interfejs użytkownika i aktualizację elementów UI.

Dane spółek, eventów i sektorów przechowywane są w ScriptableObjectach, co ułatwia ich tworzenie, edycję i rozszerzanie bez zmiany kodu.


### 4. Mechanika rynku – popyt i podaż

W grze zaimplementowano uproszczony mechanizm popytu i podaży:

- Każdy zakup/sprzedaż wpływa na popyt/podaż).

- Na koniec tury wartość popytu/podaży wpływa na cenę akcji (np. większy popyt = wzrost).

- Popyt i podaż resetuje się co turę, co pozwala utrzymać czytelność i balans rozgrywki.

### 5. System zdarzeń rynkowych – event system

System zdarzeń oparty jest o Scriptable Objects i wzorzec strategii (Strategy Pattern):

- `MarketEventSO` reprezentuje konkretne zdarzenie (nazwa, opis, wpływ).

- `MarketEventTypeSO` to strategia (np. wpływ globalny, na jeden sektor, jedną spółkę).

Dzięki temu można łatwo dodać nowe typy wydarzeń bez modyfikowania kodu logiki.

Zdarzenia są uruchamiane domyślnie co 3 tury.

### 6. Game Event System
Zastosowano system eventów oparty na statycznej klasie `GameEvents`, która udostępnia publiczne eventy i metody do ich wywoływania. Pozwala to na luźne powiązanie między komponentami - np. UI, managerami i logiką gry - bez bezpośrednich referencji. Dzięki temu kod jest bardziej modularny i łatwiejszy w utrzymaniu.

Przykład użycia: 

`GameManager` wywołuje `GameEvents.RaiseOnGameEnded()`, a `SummaryUI` nasłuchuje na ten event, by wyświetlić panel podsumowania.

### 7. Historia gry
Klasa `GameHistory` nasłuchuje zdarzeń z systemu `GameEvents`, rejestrując wszystkie istotne akcje i wydarzenia, co pozwala na łatwe wyświetlanie historii w UI oraz analizę przebiegu rozgrywki.

### 8. Podsumowanie
Projekt został zaprojektowany w sposób modularny, z wyraźnym podziałem odpowiedzialności i zastosowaniem event-driven architecture. Ułatwia to rozwój, testowanie i utrzymanie kodu, a także pozwala na łatwe dodawanie nowych funkcji w przyszłości.

