export interface ITaskItemResponse {
    id: number;
    name: string;
    description: string;
    dueDate: string | null;
    isCompleted: boolean;
    dateCreated: string;
}