<script setup>
import {ref, onMounted, compile, computed} from "vue";
import api from "../services/api";
import '../assets/homepage.css'
const tasks = ref([]);
const newTask = ref("");
const isEditing = ref(false);
const editingTask = ref({ id: null, title: "", status: "ToDo" });
const errorMessage = ref("");
const sortOption = ref("newest");
const newTaskPriority = ref(0); 

async function loadTasks() {
    
    try{
        const res = await api.get("/tasks");
        tasks.value = res.data;
        errorMessage.value = "";
    } catch (err){
        errorMessage.value = "There was an error loading the task list";
        console.log("Error loading tasks..." + err);
    }
}

async function addTask() {
    
    try{
        if(!newTask.value) return;
        await api.post("/tasks", {title: newTask.value, status: "ToDo", priority: newTaskPriority.value});
        console.log("attempting to add " + newTask.title);
        newTask.value = "";
        newTaskPriority.value = 0;
        loadTasks();
        errorMessage.value = "";
    } catch (err){
        errorMessage.value = "There was an issue adding the task";
        console.log("Error adding new task: " + err);
    }
}

async function deleteTask(id) {
    try{
        await api.delete(`/tasks/${id}`);
        loadTasks();
        errorMessage.value = "";
    } catch (err){
        errorMessage.value = "Failed to delete task. please try again later";
        console.log("There was an error deleting the task: " + id + "\n" + err);

    }
}

const activeTaskCount = computed(() => {
   return tasks.value.filter(task => task && task.status !== "Done").length;
});

const sortedTasks = computed(() => {
    
    return [...tasks.value].sort((a,b) => {
        if(sortOption.value === "newest"){
            return new Date(b.createdDate) - new Date(a.createdDate);
        }
        else{
            return new Date(a.createdDate) - new Date(b.createdDate);
        }
    });
});

function showUpdatePopup(task) {
  editingTask.value = { ...task }; 
  isEditing.value = true;
}

function priorityLabel(priority){
    console.log(priority);
    switch(priority){
        case 2: return "High";
        case 1: return "Meduim";
        default: return "Low";
    }
}


async function updateTaskSubmit() {
  
    try{
        await api.put(`/tasks/${editingTask.value.id}`, editingTask.value);
        isEditing.value = false;
        loadTasks();
  } catch (err) {
        errorMessage.value = "There was an error updaing the task";
        console.log("There was an error updaing the task: " + editingTask.id + "\n" + err);
  }

}


onMounted(loadTasks);
</script>

<template>
          
    <h2 class="mainHeaders">Task Master</h2>   
    

    <!---main content-->
    <div class="wrapper">
        <div class="containers">
            <p class="headerText">
                Hello! Enter tasks by clicking the <span onclick="goToElement('#submit_button')">Add Item</span> button. You can remove/update any item by clicking on it!
            </p>
        </div>
        
        <div class="containers">
            <form @submit.prevent="addTask" id="main_form">
                <input v-model="newTask" placeholder="New Task"/>
                  <div class="priority-options">
                    <label>
                    <input type="radio" value="0" v-model.number="newTaskPriority" />
                    Low
                    </label>
                    <label>
                    <input type="radio" value="1" v-model.number="newTaskPriority" />
                    Medium
                    </label>
                    <label>
                    <input type="radio" value="2" v-model.number="newTaskPriority" />
                    High
                    </label>
                </div>
                <button id="submit_button">Add Item</button>
            </form>
        </div>


        <div class="containers">
            <h3 class="taskCounter">Tasks remaning: {{ activeTaskCount }}</h3>
            
            <div>
                <select v-model="sortOption" id="sort_tasks_dropdown">
                    <option value="oldest">Oldest</option>
                    <option value="newest">Newest</option>
                </select>
            </div>

            <ul id="task_list">
                <li v-for="task in sortedTasks" :key="task.id" :class="{completed: task.status === 'Done'}">
                    {{ task.title }} - {{ task.status }} - <span class="priority_label">{{priorityLabel(task.priority) + " Priority"}}</span>
                    
                    <div class="task-buttons">
                         <button @click="deleteTask(task.id)">Delete</button>
                        <button @click="showUpdatePopup(task)">Update</button>
                    </div>
                </li>
            </ul>
        </div>






        <!---Pop up for editing tasks-->
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


        <!---Popup for error message-->
        <div v-if="errorMessage" class="error-popup">
            {{  errorMessage }}
            <button @click="errorMessage = ''">Close</button>
        </div>
    </div>

</template>