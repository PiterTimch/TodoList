# Todo List

Test task project. A simple app to create and manage tasks.

Live demo: https://counterwatch.itstep.click/

## Tech stack

Backend:
- ASP.NET Core 10
- Entity Framework Core
- PostgreSQL
- AutoMapper

Frontend:
- React
- TypeScript
- Vite
- Redux Toolkit (RTK Query)
- Tailwind CSS
- React Router

Deployment:
- Docker
- Docker Compose
- Nginx

## Project structure

```
Api/
  TodoListApi/   - REST API, controllers
  BLL/           - business logic, services
  DAL/           - database, entities, migrations
  TodoListApi.Tests/ - unit and integration tests

web/             - React frontend

```

## Local run

You need secrets to run the project locally (connection string, API URL, and other settings). I will send them in email.

General steps:
1. Start PostgreSQL and set connection string for API
2. Run API from `Api/TodoListApi`
3. Set frontend env and run `web` with `npm install` and `npm run dev`

## Features

- View task list
- Search tasks
- Create new task
- Mark task as completed
- Delete task
