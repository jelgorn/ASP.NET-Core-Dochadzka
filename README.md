# EduApp – evidencia dochádzky a hodnotenia študentov

Webová aplikácia vytvorená v **ASP.NET Core MVC** na evidenciu dochádzky, správu predmetov a hodnotenie študentov. Projekt vznikol ako praktická časť bakalárskej práce **„ASP.NET Core aplikácia na evidenciu prítomnosti a hodnotenia študentov“**.

<p align="center">
  <img src="https://raw.githubusercontent.com/jelgorn/ASP.NET-Core-Dochadzka/main/docs/screenshots/home.webp" alt="Úvodná stránka EduApp" width="900">
</p>

## O projekte

EduApp je jednoduchý školský informačný systém s rozhraním prispôsobeným podľa používateľskej roly. Systém rozlišuje tri základné roly: **administrátor**, **učiteľ** a **žiak**. Každá rola má prístup len k funkciám a údajom, ktoré potrebuje.

### Administrátor

- prehľad základných štatistík systému,
- správa používateľských účtov,
- správa predmetov,
- priraďovanie predmetov učiteľom,
- priraďovanie predmetov žiakom,
- export používateľov do CSV,
- vytvorenie databázovej zálohy.

### Učiteľ

- osobný profil a základné štatistiky,
- prehľad vyučovaných predmetov,
- zadávanie a úprava známok,
- evidencia prítomnosti a neprítomnosti žiakov.

### Žiak

- osobný profil,
- prehľad predmetov a priemerov,
- zobrazenie známok podľa predmetov,
- prehľad vlastnej dochádzky.

## Použité technológie

| Oblasť | Technológie |
| --- | --- |
| Backend | C#, .NET 8, ASP.NET Core MVC |
| Dáta | Entity Framework Core, MySQL 8 |
| Frontend | Razor Views, Bootstrap 5, Bootstrap Icons, vlastné CSS |
| Autentifikácia | Cookie Authentication, role-based authorization |
| Architektúra | MVC + vrstvy Application, Domain a Infrastructure |
| Vývoj | Visual Studio, .NET CLI, EF Core migrations |

## Ukážky systému

Snímky nižšie pochádzajú priamo z obrazovej prílohy bakalárskej práce a zachytávajú reálne rozhranie aplikácie.

### Administrátorský panel

Administrátor vidí základné štatistiky systému a má prístup k správe používateľov, predmetov a ich priradení.

<p align="center">
  <img src="https://raw.githubusercontent.com/jelgorn/ASP.NET-Core-Dochadzka/main/docs/screenshots/admin-dashboard.webp" alt="Administrátorský panel EduApp" width="1000">
</p>

### Prehľad známok žiaka

Žiak má známky rozdelené podľa predmetov. Pri každom predmete sa zobrazuje vypočítaný priemer a jednotlivé hodnotenia.

<p align="center">
  <img src="https://raw.githubusercontent.com/jelgorn/ASP.NET-Core-Dochadzka/main/docs/screenshots/student-grades.webp" alt="Moje známky – žiacke rozhranie" width="1000">
</p>

## Štruktúra projektu

```text
ASP.NET-Core-Dochadzka/
├── Application/       # aplikačná logika, commands a queries
├── Controllers/       # MVC controllery
├── Domain/            # doménové konštanty a pravidlá
├── Infrastructure/    # databáza, autorizácia, migrácie a seed
├── Models/            # dátové modely
├── Shared/            # spoločné filtre a pomocné komponenty
├── ViewModels/        # modely pre používateľské rozhranie
├── Views/             # Razor Views
└── Program.cs         # konfigurácia aplikácie a dependency injection
```

## Lokálne spustenie

### Požiadavky

- **.NET 8 SDK**
- **MySQL 8.x**

### 1. Naklonovanie repozitára

```bash
git clone https://github.com/jelgorn/ASP.NET-Core-Dochadzka.git
cd ASP.NET-Core-Dochadzka
```

### 2. Nastavenie databázy

Vytvor MySQL databázu a nastav connection string `DefaultConnection` pre svoje lokálne prostredie.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=bakalarka;User=YOUR_USER;Password=YOUR_PASSWORD;"
  }
}
```

### 3. Spustenie aplikácie

```bash
dotnet restore
dotnet run
```

Pri štarte aplikácia aplikuje dostupné **EF Core migrations** a vytvorí demo používateľov, ak ešte v databáze neexistujú.

## Demo účty

| Rola | Email | Heslo |
| --- | --- | --- |
| Admin | `admin@demo.sk` | `Admin123!` |
| Žiak | `user@demo.sk` | `User123!` |

> Demo účty sú určené iba na lokálne a testovacie použitie.

## Kontext projektu

Projekt bol vytvorený v roku **2025** ako praktická časť bakalárskej práce na Pedagogickej fakulte Katolíckej univerzity v Ružomberku. Cieľom bolo navrhnúť a implementovať prehľadnú webovú aplikáciu na evidenciu prítomnosti a hodnotenia študentov s oddelenými rozhraniami pre administrátora, učiteľa a žiaka.

---

**Autor:** Sebastián Šatan  
**Technológie:** ASP.NET Core MVC · C# · Entity Framework Core · MySQL · Bootstrap
