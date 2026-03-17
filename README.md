# CodeReviewAssistant

A small Blazor Interactive Server application (Target: .NET 8) that uses a hosted OpenAI-compatible endpoint (GROQ) to provide concise automated code reviews. Paste code into the editor and get improvements, positive notes, and optional refactored code.

---

## Key Features

- Blazor interactive server components (InteractiveServer render mode).
- Uses a simple `OpenAIService` to send chat completions to a GROQ-compatible OpenAI endpoint.
- Configurable AI settings via UI (model, temperature, tokens, counts, include refactored code).
- Displays structured results: Improvements, Positive notes, Refactored code.
- Simple JS interop for toasts (`wwwroot/js/toastInterop.js`).
- Dockerfile for containerized deployment (targets .NET 8 runtime).

---

## Important — GROQ API Key (Required)

Before using the app's Code Review feature, you must enter your GROQ API key in the Settings page. The app cannot call the GROQ endpoint without it.

### Where to get a GROQ API key:

- Sign up at the GROQ website and create an API key in your dashboard.
- Consult the GROQ docs or developer console for instructions on creating and managing keys.

**Suggested official starting points:**
- [GROQ Website](https://groq.com)
- [GROQ Documentation](https://groq.com/docs)

**Security reminder:** Do not commit your API key to source control. Use secure storage for production keys (environment variables, secrets manager, Key Vault).

---

## Architecture & Important Files

- `Program.cs` — App startup and DI registration. Adds Blazor interactive server components and registers services.
- `Services/OpenAIService.cs` — Responsible for building prompts and calling the GROQ API (`https://api.groq.com/openai/v1/chat/completions`).
- `Services/SettingsService.cs` — In-memory singleton to hold `UserAISettings` for the session.
- `Models/UserAISettings.cs` — AI configuration model (API key, model, temperature, tokens, counts, include refactor flag).
- `Models/ReviewResult.cs` — Stores parsed review results.
- `Models/OpenAIError.cs` — Response parsing for errors and chat responses.
- `Components/Pages/CodeReview.razor` — Primary UI for pasting code and showing results.
- `Components/Pages/Settings.razor` — UI to configure AI settings (API key required for API calls).
- `Components/App.razor`, `Components/Routes.razor`, `Components/Layout/MainLayout.razor`, `_Imports.razor` — Blazor shell and routing.
- `wwwroot/js/toastInterop.js` — JS interop to show Bootstrap toasts.
- `Dockerfile` — Multi-stage build; publishes and packages for ASP.NET Core 8.0 runtime.

---

## Prerequisites

- .NET 8 SDK (matching project target)
- Visual Studio 2022 (or later) with ASP.NET and web development workload
- Docker (optional, for containerized runs)

---

## Run Locally

### Using Visual Studio 2022

1. Open the solution in Visual Studio.
2. Restore NuGet packages (Visual Studio will usually do this automatically).
3. Build and run (F5) — the app uses Interactive Server Render Mode for Blazor.

### Using dotnet CLI


# Restore and run
dotnet restore
dotnet run --project .

**Notes:**
- The app listens on Kestrel with HTTPS by default in development.

---

## Docker

Build and run the container:

# From repository root
docker build -t codereviewassistant:latest .
# Run and map port 8080
docker run -it --rm -p 8080:8080 codereviewassistant:latest

The container sets `ASPNETCORE_URLS=http://+:8080` and exposes port `8080`.

---

## Configuration and API Key

- Open the app and navigate to Settings. Enter your GROQ API key in the `GROQ API Key` field and select model and other AI controls.
- Make sure you enter the GROQ API key first before running any code review requests.
- The app currently stores settings in-memory (`SettingsService`) for the running session and does not persist to disk.
- **Important:** Do NOT commit secrets to source control. Consider adding secure storage (User Secrets, environment variables, Key Vault) for production.


---

## How It Works

1. User pastes code in the `Code Review` page and clicks `Review Code`.
2. `CodeReview.razor` calls `OpenAIService.ReviewCodeAsync` with the pasted code and user settings.
3. `OpenAIService` builds a system + user prompt and posts to the GROQ chat completions endpoint.
4. Response is parsed; `CodeReview.razor` parses sections (Improvements, Positive, Refactored Code) and renders them.
5. Toast notifications are shown via `toastInterop.js` for success/failure and progress.

---

## Error Handling & Troubleshooting

- When the API returns an error, the service returns an `ERROR::code::message` string. The UI interprets common codes:
  - `invalid_api_key` → check API Key
  - `insufficient_quota` → out of quota
  - `rate_limit_exceeded` → too many requests
- If you see `Something went wrong with API call`, inspect network calls or the GROQ API key settings.
- Check `appsettings.Development.json` for development flags and enable logging.

---

## Development Notes

- The project targets .NET 8 and uses Blazor Interactive Server components (`AddRazorComponents().AddInteractiveServerComponents()`).
- The current settings model and settings service are intentionally simple; consider persisting settings (User Secrets, local storage, or server-side storage) if required.
- `OpenAIService` is implemented using `HttpClient`. It expects responses shaped like the GROQ/OpenAI chat schema; error parsing is implemented in `Models/OpenAIError.cs`.

---
