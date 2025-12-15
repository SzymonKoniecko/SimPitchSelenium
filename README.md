This repository contains automated tests for the SimPitch project, utilizing Selenium WebDriver. These tests automate user interactions within the Vue 3 frontend to verify the integration with the .NET backend and ensure the deterministic nature of the simulation models.

## 📂 Project Ecosystem

The SimPitch solution is modularized into three distinct repositories to ensure a clean separation of concerns between the simulation engine, the user interface, and the data contracts.

### 🧠 [SimPitch.Backend](https://github.com/SzymonKoniecko/SimPitch)
**The Simulation Engine.**
*   **Tech:** C# .NET 8, MediatR (CQRS), EF Core.
*   **Role:** Executes the core mathematical logic (Poisson, Dixon-Coles, Momentum), manages the DDD domain state, and processes simulation strategies.

### 🖥️ [SimPitch.Web](https://github.com/SzymonKoniecko/SimPitchWeb)
**The Visualization Dashboard.**
*   **Tech:** Vue 3, Vite, TypeScript.
*   **Role:** Provides the UI for configuring `SimulationParams`, visualizing complex metrics (Posterior vs. Likelihood), and displaying league iterations.

### 🔗 [SimPitch.Shared](https://github.com/SzymonKoniecko/SimPitchPROTOS)
**The Contracts.**
*   **Tech:** Protocol Buffers (.proto).
*   **Role:** The **Single Source of Truth** for data structures. Defines the shared schemas for API communication to ensure type safety between the .NET backend and Vue frontend.


### 🔗 [SimPitch.Selenium](https://github.com/SzymonKoniecko/SimPitchSelenium)
**Automated tests.**
