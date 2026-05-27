export interface ITasksSearchRequest {
    name?: string;
    isCompleted?: boolean;
    description?: string;
    dueDate?: Date;
}