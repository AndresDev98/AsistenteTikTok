

# Que necesitamos para arrancar esta app.

- Desde un terminal normal en windows --> ngrok http 5000
- BlueStacks5 (Emulador)
- adb devices (Para ver el listado)
- adb connect 127.0.0.1:5555 (Para conectarlo)


| Elemento         | ¿Activo? | Detalles                                         |
| ---------------- | -------- | ---------------------------------------------    |
| Emulador Android | ✅ / ❌    | Abierto, TikTok instalado, ADB conectado      |
| ADB instalado    | ✅ / ❌    | `adb devices` funciona                        |
| API ASP.NET Core | ✅ / ❌    | `dotnet run` levanta Swagger sin errores      |
| Internet         | ✅ / ❌    | Accede a `https://www.1secmail.com` desde app |
| Ngrok            | ❌ / 🔁   | Solo si necesitas exponer tu API fuera         |


# Siguientes pasos:

┌────────────────────────────┐
│     1. Email Manager       │
│                            │
│ • Crear correo persistente │
│ • Guardar en DB (IMAP/Sender)│
│ • Leer códigos (MailKit)   │
└────────────┬───────────────┘
             │
             ▼
┌──────────────────────────────┐
│  2. BotCreator (ADB + TikTok)│
│                              │
│ • Iniciar TikTok vía ADB     │
│ • Simular taps para registrar│
│ • Insertar email real        │
│ • Esperar código por correo  │
│ • Insertar código y crear cuenta │
└────────────┬────────────────┘
             │
             ▼
┌──────────────────────────────┐
│  3. Bot Repository / Database│
│                              │
│ • Guardar:                   │
│   - Email + Password         │
│   - Username TikTok          │
│   - Estado del bot           │
│   - Emulador asignado        │
└────────────┬────────────────┘
             │
             ▼
┌──────────────────────────────┐
│  4. Emulator Manager         │
│                              │
│ • Detectar emuladores activos│
│ • Asignar emulador libre     │
│ • Lanzar/monitor instancias  │
└────────────┬────────────────┘
             │
             ▼
┌──────────────────────────────┐
│ 5. Bot Action Executor       │
│                              │
│ • Login si no persistido     │
│ • ADB inputs:                │
│   - Tap like, guardar, follow│
│   - Navegar, comentar, etc.  │
└────────────┬────────────────┘
             │
             ▼
┌──────────────────────────────┐
│ 6. Backend API (ASP.NET Core)│
│                              │
│ • POST /create               │
│ • POST /batch                │
│ • GET /list                  │
│ • POST /action               │
│ • Asigna bots + emuladores   │
└──────────────────────────────┘


# 🔍 DESGLOSE DETALLADO DE CADA BLOQUE

## 1. 📧 Email Manager
IEmailService
Implementación: ImapEmailService (usando MailKit)
Acciones:
Crear nuevo correo (Gmail, Yandex, dominio propio)
Leer buzón hasta recibir código de TikTok
Enviar desde ese correo si hiciera falta

## 2. 🤖 BotCreatorService
IBotCreatorService
Usa ADBService para simular:
Tap “Registrarse”
Introducir correo real
Esperar verificación (usa EmailService)
Crear cuenta
Guarda el resultado en base de datos

## 3. 🗄️ Base de datos / Bot Repository
Modelo BotAccount
Id, Email, Password, TikTokUsername, Estado, EmuladorId
Repositorio: IBotAccountRepository
Guardar y obtener bots
Almacenamiento:
Inicial: JSON
Futuro: SQLite o PostgreSQL

## 4. 🖥️ Emulator Manager
Detecta emuladores activos por adb devices
Reparte bots entre instancias:
Ej: 127.0.0.1:5555, 5556, 5557...
Administra:
Lanzamiento
Reinicio si se cae
Asignación de recursos

## 5. 🎮 BotActionExecutor
Recibe acción del tipo:
Like
Follow
Guardar en favoritos
Comentar

Ejecuta con comandos ADB:
csharp
Copiar
Editar
input_tap(x, y);
input_text("🔥 buen vídeo");
input_swipe(x1, y1, x2, y2);
Se puede parametrizar y automatizar en cola

## 6. 🌐 Backend ASP.NET Core (Orquestador)
Endpoints actuales:
POST /api/bot/create
POST /api/bot/batch
GET /api/bot/list

Endpoints futuros:
POST /api/bot/action
GET /api/emulators/available

Inyecta servicios: Email, ADB, BotRepo, etc.

Coordina toda la ejecución

📦 FUTURO ESCALABLE
Escenario futuro	Qué bloque se modifica
Pasar de JSON a SQL	Solo BotRepository
Añadir SMS con Twilio/5sim	IPhoneVerificationService nuevo
Añadir UI React	Cliente REST con endpoint list, action, etc.
Multi-PC con emuladores	EmulatorManager distribuido