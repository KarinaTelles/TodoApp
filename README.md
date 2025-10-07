# Todo App (C# + Vue.js)

Aplicação full-stack que utiliza backend ASP.NET Core e frontend Vue.js (Vite). 
Seu objetivo é criar uma lista de "tarefas a fazer"(TODO list), marcar elas como feitas e excluí-la. 

## Tecnologias
- Backend: C# (.NET 8, Web API)
- Frontend: Vue.js (Vite)
- HTTP Client: Axios

## Como rodar

### Backend e Frontend

cd TodoApi
dotnet run

cd ../
cd todo-frontend
npm run dev

#### No CMD, usando o curl. GET, POST, PUT e DELETE
curl http://localhost:5000/todos
curl -X POST http://localhost:5000/todos -H "Content-Type: application/json" -d "{\"title\":\"Comprar pão\"}"
curl -X PUT http://localhost:5000/todos/1 -H "Content-Type: application/json" -d "{\"title\":\"Comprar pão\",\"done\":true}"
curl -X DELETE http://localhost:5000/todos/1 (usar o ID que pretende deletar).


