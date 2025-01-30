import { BaseAPI as TaskManagementApi } from "./metadata/task-management-api/base"
import { BaseAPI as TaskSearchApi } from "./metadata/task-search-api/base"

export function createTaskManagementApi<T extends TaskManagementApi>(ctor: { new (): T }) : T {
    const api = new ctor()
    api["basePath"] = "http://localhost:10000/task-management"
    return api
}

export function createTaskSearchApi<T extends TaskSearchApi>(ctor: { new (): T }) : T {
    const api = new ctor()
    api["basePath"] = "http://localhost:10000/task-search"
    return api
}