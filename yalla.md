# Задание: REST API для системы корпоративной доставки еды Yalla.tj

## 🎯 Цель проекта

Разработать REST API на C# (.NET 8+) с использованием Dapper для работы с базой данных PostgreSQL. API должно обеспечивать управление корпоративными заказами питания, работу с меню и подписками компаний.

## 📚 Технологический стек

- ASP.NET Core WebAPI (.NET 8+)
- Dapper
- PostgreSQL
- Swagger/OpenAPI для документации
- ApiResponse Pattern для стандартизации ответов
- Dependency Injection

## 🏗️ Структура решения

```
YallaDelivery/
├── YallaDelivery.API/
│   ├── Controllers/
│   └── Program.cs
├── YallaDelivery.Domain/
│   ├── Entities/
│   ├── Dtos/
│   └── Responses/
└── YallaDelivery.Infrastructure/
    ├── Interfaces/
    └── Services/
```

## 📂 Структура базы данных

База данных содержит шесть основных таблиц:

### 1. Таблица `companies` – Компании

| Поле         | Тип данных             | Описание                           |
|------------- |----------------------- |----------------------------------- |
| `id`         | SERIAL PRIMARY KEY     | Уникальный идентификатор компании  |
| `name`       | VARCHAR(255) NOT NULL  | Название компании                  |
| `address`    | TEXT NOT NULL          | Адрес доставки                     |
| `phone`      | VARCHAR(50) NOT NULL   | Контактный телефон                 |
| `email`      | VARCHAR(255) NOT NULL  | Электронная почта                  |
| `created_at` | TIMESTAMP              | Дата создания записи               |
| `updated_at` | TIMESTAMP              | Дата обновления записи             |

### 2. Таблица `menus` – Меню

| Поле         | Тип данных             | Описание                           |
|------------- |----------------------- |----------------------------------- |
| `id`         | SERIAL PRIMARY KEY     | Уникальный идентификатор меню      |
| `menu_date`  | DATE NOT NULL          | Дата меню                          |
| `is_active`  | BOOLEAN               | Статус активности меню             |
| `created_at` | TIMESTAMP              | Дата создания записи               |
| `updated_at` | TIMESTAMP              | Дата обновления записи             |

### 3. Таблица `menu_items` – Позиции меню

| Поле         | Тип данных             | Описание                           |
|------------- |----------------------- |----------------------------------- |
| `id`         | SERIAL PRIMARY KEY     | Уникальный идентификатор позиции   |
| `menu_id`    | INTEGER               | Внешний ключ к таблице menus       |
| `name`       | VARCHAR(255) NOT NULL  | Название блюда                     |
| `description`| TEXT                   | Описание блюда                     |
| `price`      | DECIMAL(10,2) NOT NULL | Цена блюда                         |
| `category`   | VARCHAR(50) NOT NULL   | Категория блюда                    |
| `created_at` | TIMESTAMP              | Дата создания записи               |
| `updated_at` | TIMESTAMP              | Дата обновления записи             |

### 4. Таблица `subscriptions` – Подписки

| Поле           | Тип данных             | Описание                           |
|--------------- |----------------------- |----------------------------------- |
| `id`           | SERIAL PRIMARY KEY     | Уникальный идентификатор подписки  |
| `company_id`   | INTEGER               | Внешний ключ к таблице companies   |
| `plan_type`    | VARCHAR(50) NOT NULL   | Тип плана подписки                 |
| `meals_per_day`| INTEGER NOT NULL       | Количество блюд в день             |
| `price`        | DECIMAL(10,2) NOT NULL | Стоимость подписки                 |
| `start_date`   | DATE NOT NULL          | Дата начала подписки               |
| `end_date`     | DATE NOT NULL          | Дата окончания подписки            |
| `is_active`    | BOOLEAN               | Статус активности подписки         |
| `created_at`   | TIMESTAMP              | Дата создания записи               |
| `updated_at`   | TIMESTAMP              | Дата обновления записи             |

### 5. Таблица `orders` – Заказы

| Поле           | Тип данных             | Описание                           |
|--------------- |----------------------- |----------------------------------- |
| `id`           | SERIAL PRIMARY KEY     | Уникальный идентификатор заказа    |
| `company_id`   | INTEGER               | Внешний ключ к таблице companies   |
| `order_date`   | DATE NOT NULL          | Дата заказа                        |
| `status`       | VARCHAR(50) NOT NULL   | Статус заказа                      |
| `total_amount` | DECIMAL(10,2) NOT NULL | Общая сумма заказа                 |
| `created_at`   | TIMESTAMP              | Дата создания записи               |
| `updated_at`   | TIMESTAMP              | Дата обновления записи             |

### 6. Таблица `order_items` – Позиции заказа

| Поле           | Тип данных             | Описание                           |
|--------------- |----------------------- |----------------------------------- |
| `id`           | SERIAL PRIMARY KEY     | Уникальный идентификатор позиции   |
| `order_id`     | INTEGER               | Внешний ключ к таблице orders      |
| `menu_item_id` | INTEGER               | Внешний ключ к таблице menu_items  |
| `quantity`     | INTEGER NOT NULL       | Количество                         |
| `price`        | DECIMAL(10,2) NOT NULL | Цена на момент заказа              |
| `created_at`   | TIMESTAMP              | Дата создания записи               |
| `updated_at`   | TIMESTAMP              | Дата обновления записи             |

## 🔍 API Endpoints и Сервисы

### CompanyController

1. **GET /api/companies**
   * **Метод:** Task<ApiResponse<List<Company>>> GetAllAsync()
   * **Описание:** Получение списка всех компаний

2. **GET /api/companies/{id}**
   * **Метод:** Task<ApiResponse<Company>> GetByIdAsync(int id)
   * **Описание:** Получение компании по идентификатору

3. **POST /api/companies**
   * **Метод:** Task<ApiResponse<string>> CreateAsync(Company request)
   * **Описание:** Создание новой компании

4. **PUT /api/companies/{id}**
   * **Метод:** Task<ApiResponse<string>> UpdateAsync(int id, Company request)
   * **Описание:** Обновление данных компании

5. **DELETE /api/companies/{id}**
   * **Метод:** Task<ApiResponse<string>> DeleteAsync(int id)
   * **Описание:** Удаление компании

### MenuController

1. **GET /api/menus/active**
   * **Метод:** Task<ApiResponse<Menu>> GetActiveMenuAsync()
   * **Описание:** Получение активного меню

2. **GET /api/menus/{date}**
   * **Метод:** Task<ApiResponse<Menu>> GetMenuByDateAsync(DateTime date)
   * **Описание:** Получение меню по дате

3. **POST /api/menus**
   * **Метод:** Task<ApiResponse<string>> CreateMenuAsync(Menu request)
   * **Описание:** Создание нового меню

4. **POST /api/menus/{menuId}/items**
   * **Метод:** Task<ApiResponse<string>> AddMenuItemAsync(int menuId, MenuItem item)
   * **Описание:** Добавление позиции в меню

5. **GET /api/menus/categories**
   * **Метод:** Task<ApiResponse<List<string>>> GetMenuCategoriesAsync()
   * **Описание:** Получение списка категорий блюд

### SubscriptionController

1. **GET /api/subscriptions/company/{companyId}**
   * **Метод:** Task<ApiResponse<List<Subscription>>> GetCompanySubscriptionsAsync(int companyId)
   * **Описание:** Получение подписок компании

2. **POST /api/subscriptions**
   * **Метод:** Task<ApiResponse<string>> CreateSubscriptionAsync(Subscription request)
   * **Описание:** Создание новой подписки

3. **PUT /api/subscriptions/{id}/status**
   * **Метод:** Task<ApiResponse<string>> UpdateSubscriptionStatusAsync(int id, bool isActive)
   * **Описание:** Обновление статуса подписки

4. **GET /api/subscriptions/active**
   * **Метод:** Task<ApiResponse<List<Subscription>>> GetActiveSubscriptionsAsync()
   * **Описание:** Получение всех активных подписок

### OrderController

1. **GET /api/orders/company/{companyId}**
   * **Метод:** Task<ApiResponse<List<Order>>> GetCompanyOrdersAsync(int companyId)
   * **Описание:** Получение заказов компании

2. **POST /api/orders**
   * **Метод:** Task<ApiResponse<string>> CreateOrderAsync(OrderCreateRequest request)
   * **Описание:** Создание нового заказа

3. **PUT /api/orders/{id}/status**
   * **Метод:** Task<ApiResponse<string>> UpdateOrderStatusAsync(int id, string status)
   * **Описание:** Обновление статуса заказа

4. **GET /api/orders/daily-summary**
   * **Метод:** Task<ApiResponse<DailySummaryResponse>> GetDailySummaryAsync(DateTime date)
   * **Описание:** Получение сводки заказов за день

5. **GET /api/orders/company-statistics**
   * **Метод:** Task<ApiResponse<List<CompanyStatisticsResponse>>> GetCompanyOrderStatisticsAsync()
   * **Описание:** Получение статистики заказов по компаниям