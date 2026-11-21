### Папка проекта

Получить именно нужную папку из [официального репозитория](https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/tutorials/min-web-api/samples/10.x/TodoApi_VSC_Scalar). Откройте PowerShell в месте, где хотите хранить проект, и выполните команды **по одной строке**:

```powershell
git clone --depth 1 --filter=blob:none --sparse https://github.com/dotnet/AspNetCore.Docs.git
cd AspNetCore.Docs
git sparse-checkout set aspnetcore/tutorials/min-web-api/samples/10.x/TodoApi_VSC_Scalar
cd aspnetcore/tutorials/min-web-api/samples/10.x/TodoApi_VSC_Scalar
```

После этого вы окажетесь в нужной папке проекта. 
## Запуск в Visual Studio Code

1. Введите команды ниже **по одной строке** в этом же терминале:

   ```powershell
   $env:DOTNET_ENVIRONMENT = 'Development'
   $env:ASPNETCORE_ENVIRONMENT = 'Development'
   $env:ASPNETCORE_URLS = 'http://localhost:5000'
   dotnet run
   ```

2. Дождитесь сообщения примерно такого вида:

   ```text
   Now listening on: http://localhost:5000
   ```

   **Оставьте терминал работающим.** Пока в нём выполняется `dotnet run`, сервис принимает запросы. Если в строке `Now listening on:` указан другой порт, дальше используйте именно этот адрес и порт.
## Откройте Scalar

1. В браузере введите **`http://localhost:5000/scalar/v1`**. Если сервер написал другой адрес в `Now listening on:`, подставьте его и добавьте `/scalar/v1`.
2. Должна появиться страница со списком операций: `GET /todoitems`, `POST /todoitems`, `GET /todoitems/{id}` и другие.
3. Для первой проверки выберите **`GET /todoitems`**, нажмите **Test Request** или откройте API Client, затем **Send**. Справа ожидается статус `200 OK` и массив JSON. На только что запущенном сервисе массив может быть пустым: `[]`.

Если это получилось, сервис и Scalar готовы для КТ6.
## Как пользоваться Scalar

| Элемент страницы | Что означает и куда смотреть |
|---|---|
| Название операции, например `GET /todoitems` | Метод `GET` и путь `/todoitems` определяют действие. Выбирайте операцию из списка, а не меняйте наугад адрес. |
| **Variables → id** | Для пути `/todoitems/{id}` сюда вводится число, полученное из ответа на собственный `POST`. Фигурные скобки в адресе вручную заменять не требуется. |
| **Body → JSON** | Тело для `POST`, `PUT` или `PATCH`. Вводите **один** корректный JSON-объект, например `{"name":"Моя задача","isComplete":false}`. Два объекта подряд вызовут ошибку чтения JSON. |
| **Send** | Отправляет настоящий запрос работающему API. Только просмотр страницы Scalar ничего не создаёт и не меняет. |
| Правая часть: статус и **Body** | Фактический ответ сервиса. Сравнивайте его с ожиданием из черновика. У ответа `204 No Content` тела нет; чтобы узнать итоговые поля задачи, отправьте отдельный `GET`. |

Записывайте `id` **из ответа на свой `POST`**. Не полагайтесь на пример `id = 1`: после новых запросов или перезапуска идентификаторы могут отличаться. В этом проекте база хранится в памяти процесса, поэтому после остановки и нового запуска созданные задачи исчезают. [Описание учебного примера Microsoft.](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0)
