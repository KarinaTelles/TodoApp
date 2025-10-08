<template>
  <div class="container">
    <h1>To do app (Vue + C#)</h1>

    <form @submit.prevent="add">
      <input v-model="newTitle" placeholder="Nova tarefa" />
      <button type="submit">Adicionar</button>
    </form>

    <ul v-if="todos.length">
      <li v-for="t in todos" :key="t.id" class="todo">
        <label>
          <input type="checkbox" :checked="t.done" @change="toggle(t)" />
          <span :class="{ done: t.done }">{{ t.title }}</span>
        </label>
        <button @click="remove(t)">Excluir</button>
      </li>
    </ul>

    <p v-else>Sem tarefas ainda — adicione a primeira!</p>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import api from "./api";

export default {
  setup() {
    const todos = ref([]);
    const newTitle = ref("");

    const load = async () => {
      try {
        const res = await api.get("/todos");
        todos.value = res.data;
      } catch (err) {
        console.error("Erro ao carregar todos:", err);
        alert("Erro ao carregar tarefas. Veja o console (F12).");
      }
    };

    const add = async () => {
      if (!newTitle.value.trim()) return;
      try {
        const res = await api.post("/todos", { title: newTitle.value });
        todos.value.push(res.data);
        newTitle.value = "";
      } catch (err) {
        console.error("Erro ao adicionar:", err);
        alert("Erro ao adicionar tarefa. Veja o console (F12).");
      }
    };

    const toggle = async (t) => {
      try {
        const res = await api.put(`/todos/${t.id}`, {
          title: t.title,
          done: !t.done,
        });
        const idx = todos.value.findIndex((x) => x.id === t.id);
        if (idx !== -1) todos.value.splice(idx, 1, res.data);
      } catch (err) {
        console.error("Erro ao atualizar:", err);
        alert("Erro ao atualizar tarefa. Veja o console (F12).");
      }
    };

    const remove = async (t) => {
      if (!confirm("Excluir essa tarefa?")) return;
      try {
        await api.delete(`/todos/${t.id}`);
        todos.value = todos.value.filter((x) => x.id !== t.id);
      } catch (err) {
        console.error("Erro ao deletar:", err);
        alert("Erro ao deletar tarefa. Veja o console (F12).");
      }
    };

    onMounted(load);

    return { todos, newTitle, add, toggle, remove, load };
  },
};
</script>

<style>
.container {
  max-width: 700px;
  margin: 40px auto;
  font-family: Arial, sans-serif;
  padding: 0 16px;
}
h1 { margin-bottom: 12px; }
form { display:flex; gap:8px; margin-bottom:16px; }
input[type="text"], input { flex:1; padding:8px; font-size:14px; }
button { padding:8px 12px; cursor:pointer; }
ul { list-style:none; padding:0; }
.todo { display:flex; justify-content:space-between; align-items:center; padding:8px 0; border-bottom:1px solid #eee; }
.todo label { display:flex; gap:8px; align-items:center; }
.done { text-decoration: line-through; color: #777; }
</style>
