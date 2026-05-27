import React from "react";
import type { ITaskItemResponse } from "../../types/task/ITaskItemResponse.ts";
import {
    useDeleteTaskMutation,
    useSetTaskCompletedMutation,
} from "../../services/api/apiTask.ts";

type TaskItemProps = {
    task: ITaskItemResponse;
};

const formatDate = (value: string | null) => {
    if (!value) return "";
    const d = new Date(value);
    return Number.isNaN(d.getTime()) ? value : d.toLocaleDateString();
};

export const TaskItem: React.FC<TaskItemProps> = ({ task }) => {
    const [deleteTask, { isLoading: isDeleting }] = useDeleteTaskMutation();
    const [setTaskCompleted, { isLoading: isCompleting }] = useSetTaskCompletedMutation();

    const cardClasses = task.isCompleted
        ? "bg-stone-200 text-stone-600"
        : "bg-amber-100 text-stone-800";

    const handleDelete = async () => {
        if (isDeleting) return;
        await deleteTask(task.id);
    };

    const handleToggleCompleted = async () => {
        if (isCompleting) return;
        await setTaskCompleted({
            id: task.id,
            isCompleted: !task.isCompleted,
        });
    };

    return (
        <div className={`rounded-2xl p-5 shadow-sm ${cardClasses}`}>
            <div className="flex items-center justify-between gap-3">
                <button
                    type="button"
                    aria-label={task.isCompleted ? "Mark as incomplete" : "Mark as complete"}
                    onClick={handleToggleCompleted}
                    disabled={isCompleting}
                    className="flex h-8 w-8 items-center justify-center rounded-lg border border-stone-500 bg-white text-stone-700 shadow-sm disabled:opacity-50"
                >
                    {task.isCompleted ? "✓" : ""}
                </button>

                <button
                    type="button"
                    onClick={handleDelete}
                    disabled={isDeleting}
                    className="rounded-lg bg-red-700 px-3 py-1.5 text-sm font-medium text-white shadow-sm transition hover:bg-red-800 disabled:opacity-50"
                >
                    Delete
                </button>
            </div>

            <div className="mt-3 space-y-1">
                <div className="text-lg font-semibold">
                    {task.name}
                </div>
                <div className="text-sm leading-snug">
                    {task.description}
                </div>
                {task.dueDate ? (
                    <div className="text-sm">
                        Due: {formatDate(task.dueDate)}
                    </div>
                ) : null}
            </div>
        </div>
    );
};

