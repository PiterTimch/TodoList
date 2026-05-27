export interface ITaskItemResponse {
    id: number;
    name: string;
    description: string;
    dueDate: Date;
    isCompleted: boolean;
    dateCreated: Date;
}