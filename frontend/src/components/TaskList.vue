<script setup>
import {ref, onMounted, compile, computed} from "vue";
import api from "../services/api";
import '../assets/homepage.css'
const tasks = ref([]);
const newTask = ref("");
const isEditing = ref(false);
const editingTask = ref({ id: null, title: "", status: "ToDo" });


async function loadTasks() {
    
    try{
        const res = await api.get("/tasks");
        tasks.value = res.data;
    } catch (err){
        console.log("Error loading tasks..." + err);
    }
    
    
 
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

function goToElement(elementID) {
    const target = document.getElementById(elementID);
    if (target){
        target.scrollIntoView({behavior: 'smooth'});
    }
}


const activeTaskCount = computed(() => {
   return tasks.value.filter(task => task && task.status !== "Completed").length;
});


function showUpdatePopup(task) {
  editingTask.value = { ...task }; 
  isEditing.value = true;
}


async function updateTaskSubmit() {
  await api.put(`/tasks/${editingTask.value.id}`, editingTask.value);
  isEditing.value = false;
  loadTasks();
}


onMounted(loadTasks);
</script>

<template>
          
          
          
    <h2 class="mainHeaders">Task Master</h2>   
    
    <div class="wrapper">
        <div class="containers">
            <p class="headerText">
                Hello! Enter tasks by clicking the <span onclick="goToElement('#submit_button')">Add Item</span> button. You can remove/update any item by clicking on it!
            </p>
        </div>
        
        <div class="containers">
            <form @submit.prevent="addTask" id="main_form">
                <input v-model="newTask" placeholder="New Task"/>
                <button id="submit_button">Add Item</button>
            </form>
        </div>

        <div class="containers">
            <h3 class="taskCounter">Tasks remaning: {{ activeTaskCount }}</h3>
            
            <ul>
                <li v-for="task in tasks" :key="task.id" :class="{completed: task.status === 'Done'}">
                    {{ task.title }} - {{ task.status }}
                    
                    <div class="task-buttons">
                         <button @click="deleteTask(task.id)">Delete</button>
                        <button @click="showUpdatePopup(task)">Update</button>
                    </div>
                </li>
            </ul>
        </div>

            <div v-if="isEditing" class="modal-overlay">
        <div class="modal-content">
            <h3>Edit Task</h3>
            <input id="update_item_popup" v-model="editingTask.title" placeholder="Task Title" />
            <select v-model="editingTask.status">
            <option>ToDo</option>
            <option>InProgress</option>
            <option>Done</option>
            </select>
                    <div class="modal-buttons">
                        <button @click="updateTaskSubmit">Save</button>
                        <button @click="isEditing = false">Cancel</button>
                    </div>
                </div>
        </div>
    </div>

</template>