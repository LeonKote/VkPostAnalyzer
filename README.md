# VkPostAnalyzer

![logo](https://i.imgur.com/YDyDETr.png)

![VkPostAnalyzer](https://img.shields.io/badge/ASP.NET_Core-8.0-blue.svg) ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-blue.svg) ![Swagger](https://img.shields.io/badge/Swagger-UI-green.svg)

VkPostAnalyzer — это веб-приложение, которое анализирует последние 5 постов пользователя ВКонтакте, подсчитывает частоту букв в текстах постов и сохраняет результаты в базу данных PostgreSQL. Взаимодействие с backend осуществляется через Swagger UI.

## 📌 Функциональность
- Авторизация через VK OAuth 2.0
- Получение 5 последних постов пользователя VK
- Подсчет вхождения каждой буквы (регистронезависимо)
- Сортировка результата по алфавиту
- Сохранение результатов анализа в базу данных PostgreSQL
- Логирование процесса в локальный файл
- Swagger UI для взаимодействия с API

## 🛠 Технологии
- **Backend:** ASP.NET Core 8.0, Entity Framework Core
- **Database:** PostgreSQL
- **Logging:** Serilog
- **API Documentation:** Swagger
- **VK API:** Официальный OAuth 2.0 и метод wall.get

## 🚀 Запуск проекта

### 1. Клонирование репозитория
```bash
git clone https://github.com/yourusername/VkPostAnalyzer.git
cd VkPostAnalyzer
```

### 2. Настройка конфигурации
Откройте файл appsettings.json в корневой директории проекта и укажите параметры VK API и базы данных. Настройки VK API можно оставить по умолчанию, так как они предназначены для тестового приложения на localhost:

```json
{
  "ConnectionStrings": {
    "VkPostAnalyzerDb": "Host=localhost;Port=5432;Database=vkpostanalyzer;Username=your_user;Password=your_password"
  },
  "Vk": {
    "ClientId": "53190985",
    "RedirectUri": "https://localhost/api/vk/auth/response",
    "Version": "5.131"
  }
}
```

### 3. Разворачивание базы данных PostgreSQL
Запустите PostgreSQL и создайте базу данных `vkpostanalyzer`.

### 4. Запуск приложения
```bash
dotnet run
```

После запуска API будет доступен по адресу: `https://localhost/` (на 443 порту)

**⚠️ Авторизация через OAuth 2.0 от VK для сайтов на данный момент может происходить только с переадресацией на порты 80 и 443.**

### 5. Открытие Swagger UI
Swagger UI доступен по адресу:
```
https://localhost/swagger/index.html
```

## 📌 API Эндпоинты

### Авторизация
- `GET /api/vk/auth/url` — Получение OAuth URL для VK
- `GET /api/vk/auth/response` — Обработка ответа VK

  **⚠️ Переадресация на этот эндпоинт произойдёт автоматически со страницы авторизации VK и вернёт токен доступа VK в отдельном окне.**

### Анализ постов
- `POST /api/vk/posts/analyze` — Анализ 5 последних постов
  ```json
  {
    "accessToken": "your_access_token",
    "ownerId": 12345678
  }
  ```
  Без указания ```ownerId``` получение постов будет происходить со страницы текущего пользователя.

## 📜 Логирование
Логи записываются в `logs/app.log` и содержат информацию о запуске и завершении анализа постов.

## 📄 Лицензия
Этот проект распространяется под лицензией MIT.

---
_Автор: [Папушев Роман](https://github.com/LeonKote)_
