<template>
  <div class="container">
    <div class="row">
      <div class="col-xxl-3 col-xl-2 col-lg-2 col-md-1 d-none d-sm-none d-md-block"></div>
      <div class="col-12 col-xxl-6 col-xl-8 col-lg-8 col-md-10 col-sm-12">
        <div class="d-flex flex-column m-2">
          <div class="d-flex flex-sm-row flex-column m-2 justify-content-between align-items-center">
            <div class="p-2"><img alt="logo" src="../assets/logo.png"></div>
            <div class="p-2">
              <h3>Todo Microservice App</h3>
            </div>
            <div class="btn-group" role="group" aria-label="Task Status">
              <input v-model="searchCompleted" @change="search" value="" type="radio" class="btn-check"
                name="btn-radio-task-status" id="btn-radio-task-status-all" autocomplete="off" checked>
              <label class="btn btn-outline-primary" for="btn-radio-task-status-all">All</label>

              <input v-model="searchCompleted" @change="search" value="true" type="radio" class="btn-check"
                name="btn-radio-task-status" id="btn-radio-task-status-done" autocomplete="off">
              <label class="btn btn-outline-primary" for="btn-radio-task-status-done">Done</label>

              <input v-model="searchCompleted" @change="search" value="false" type="radio" class="btn-check"
                name="btn-radio-task-status" id="btn-radio-task-status-undone" autocomplete="off">
              <label class="btn btn-outline-primary" for="btn-radio-task-status-undone">Undone</label>
            </div>
          </div>

          <div class="m-2">
            <div class="input-group">
              <input v-model="searchText" type="text" class="form-control" placeholder="Search" aria-label="Search"
                aria-describedby="button-search">
              <button @click="search" class="btn btn-outline-primary" type="button" id="button-search">Search</button>
            </div>
          </div>

          <div class="m-2">
            <div class="input-group">
              <input v-model="task.title" type="text" class="form-control" placeholder="Add a new task" aria-label="Add a new task"
                aria-describedby="button-add button-cancel">
              <button v-show="task.id" @click="cancelEdit" class="btn btn-danger" type="button"
                id="button-cancel">Cancel</button>
              <button @click="task.id ? updateTask() : addTask()" class="btn btn-primary" type="button"
                id="button-add">{{ task.id ? "Edit" : "Add" }}</button>
            </div>
          </div>

          <div class="m-2">
            <ul id="task-list" class="list-group list-group-striped user-select-none">
              <li v-for="task in tasks" :key="task.id"
                class="list-group-item list-group-item-action d-flex justify-content-between align-items-start"
                :class="{ 'checked': task.completed }">
                <div @click="changeCompleted(task)" class="ms-4 w-100" role="button">{{ task.title }}</div>
                <span @click="editTask(task)" class="edit text-success ms-2" role="button">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                    <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                  </svg>
                </span>
                <span @click="deleteTask(task.id)" class="text-danger ms-2" role="button">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3" viewBox="0 0 16 16">
                    <path d="M6.5 1h3a.5.5 0 0 1 .5.5v1H6v-1a.5.5 0 0 1 .5-.5ZM11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3A1.5 1.5 0 0 0 5 1.5v1H2.506a.58.58 0 0 0-.01 0H1.5a.5.5 0 0 0 0 1h.538l.853 10.66A2 2 0 0 0 4.885 16h6.23a2 2 0 0 0 1.994-1.84l.853-10.66h.538a.5.5 0 0 0 0-1h-.995a.59.59 0 0 0-.01 0H11Zm1.958 1-.846 10.58a1 1 0 0 1-.997.92h-6.23a1 1 0 0 1-.997-.92L3.042 3.5h9.916Zm-7.487 1a.5.5 0 0 1 .528.47l.5 8.5a.5.5 0 0 1-.998.06L5 5.03a.5.5 0 0 1 .47-.53Zm5.058 0a.5.5 0 0 1 .47.53l-.5 8.5a.5.5 0 1 1-.998-.06l.5-8.5a.5.5 0 0 1 .528-.47ZM8 4.5a.5.5 0 0 1 .5.5v8.5a.5.5 0 0 1-1 0V5a.5.5 0 0 1 .5-.5Z" />
                  </svg>
                </span>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="col-xxl-3 col-xl-2 col-lg-2 col-md-1 d-none d-sm-none d-md-block"></div>
    </div>
  </div>

</template>

<style>
#task-list li:nth-child(odd) {
  background: #f3f3f3;
}

#task-list li.checked {
  background: #999;
  color: #fff;
  text-decoration: line-through;
}

#task-list li.checked::before {
  content: '';
  position: absolute;
  border-color: #fff;
  border-style: solid;
  border-width: 0 2px 2px 0;
  top: 10px;
  left: 16px;
  transform: rotate(45deg);
  height: 15px;
  width: 7px;
}
</style>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import { createTaskManagementApi, createTaskSearchApi } from '@/api'
import { TaskApi, TaskListItemViewModel } from '@/metadata/task-management-api'
import { SearchApi } from '@/metadata/task-search-api'
import * as signalR from "@microsoft/signalr"
import { useToast } from "vue-toastification"

@Options({})
export default class HomeView extends Vue {
  taskApi: TaskApi = createTaskManagementApi(TaskApi)
  taskSearchApi: SearchApi = createTaskSearchApi(SearchApi)
  tasks: TaskListItemViewModel[] = []
  searchText: string | null = null
  searchCompleted = ""
  task: TaskListItemViewModel = {}
  toast = useToast()

  mounted(): void {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:8081/generalNotificationHub")
        .build()

    connection.on("GeneralNotification", (message: string) => this.toast.success(message, {
        timeout: 5_000
      }))
    connection.start()

    this.listTasks()
  }

  listTasks(): void {
    this.taskApi.getTasks().then(x => this.tasks = x.data)
  }

  changeCompleted(task: TaskListItemViewModel): void {
    const completed = !task.completed;
    this.taskApi.changeCompleted(task.id as number, completed).then(() => task.completed = completed)
  }

  search(): void {
    this.taskSearchApi.search({ text: this.searchText, completed: this.getSearchCompleted() }).then(x => {
        this.tasks = []
        x.data.forEach(task => {
          this.tasks.push(task)
        });
      })
  }

  addTask(): void {
    this.taskApi.addTask({ title: this.task.title as string }).then(() => {
      this.cancelEdit()
      this.listTasks()
    })
  }

  updateTask(): void {
    this.taskApi.updateTask({ taskId: this.task.id, title: this.task.title as string }).then(() => {
      this.cancelEdit()
      this.listTasks()
    })
  }

  deleteTask(id?: number): void {
    this.cancelEdit()

    this.taskApi.deleteTask(id as number).then(() => {
      this.listTasks()
    })
  }

  editTask(task: TaskListItemViewModel): void {
    this.task = { ...task }
  }

  cancelEdit(): void {
    this.task.id = undefined
    this.task.title = undefined
  }

  getSearchCompleted(): boolean | null {
    switch (this.searchCompleted) {
      case "true":
        return true
      case "false":
        return false
      default:
        return null
    }
  }
}
</script>
