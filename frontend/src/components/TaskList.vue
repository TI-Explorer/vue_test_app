<script setup>
import {ref, onMounted} from "vue";
import api from "../services/api";
import '../assets/homepage.css'
const tasks = ref([]);
const newTask = ref("");

async function loadTasks() {
    const res = await api.get("/tasks");
    tasks.value = res.data;
}

async function addTask() {
    if(!newTask.value) return;
    await api.post("/tasks", {title: newTask.value, status: "ToDo"});
    console.log("attempting to add " + newTask.title);
    newTask.value = "";
    loadTasks();
}

async function deleteTask(id) {
    await api.delete(`/tasks/${id}`);
    loadTasks();
    
}

onMounted(loadTasks);
</script>

<template>
    <div class="containers">
        <h2 class="mainHeaders">Task Master</h2>

        <form @submit.prevent="addTask" id="main_form">
            <input v-model="newTask" placeholder="New Task"/>
            <button>Add</button>
        </form>

        <ul>
            <li v-for="task in tasks" :key="task.id">
                {{ task.title }} - {{ task.status }}
                <button @click="deleteTask(task.id)">Delete</button>
            </li>
        </ul>
    </div>
</template>