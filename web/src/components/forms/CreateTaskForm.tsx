import React, { useState } from "react";
import type { ICreateTaskRequest } from "../../types/task/ICreateTaskRequest.ts";
import { TextInput } from "./TextInput.tsx";
import { TextArea } from "./TextArea.tsx";
import { CalendarInput } from "./CalendarInput.tsx";
import { FormErrorMessage } from "./FormErrorMessage.tsx";

type CreateTaskFormProps = {
    onSubmit: (task: ICreateTaskRequest) => void;
    isSubmitting?: boolean;
    errorMessage?: string;
};

export const CreateTaskForm: React.FC<CreateTaskFormProps> = ({
    onSubmit,
    isSubmitting = false,
    errorMessage,
}) => {
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [dueDate, setDueDate] = useState<string | undefined>(undefined);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        const trimmedName = name.trim();
        const trimmedDescription = description.trim();
        if (!trimmedName || !trimmedDescription) return;

        onSubmit({
            name: trimmedName,
            description: trimmedDescription,
            ...(dueDate ? { dueDate } : {}),
        });
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <TextInput
                label="Name"
                value={name}
                onChange={setName}
                placeholder="Eat"
            />
            <TextArea
                label="Description"
                value={description}
                onChange={setDescription}
                placeholder="Task details"
            />
            <CalendarInput label="Due date" value={dueDate} onChange={setDueDate} />

            <button
                type="submit"
                disabled={isSubmitting}
                className="w-full rounded-xl bg-stone-700 px-3 py-2.5 text-sm font-medium text-white shadow-sm transition hover:bg-stone-800 disabled:opacity-50"
            >
                Create
            </button>

            <FormErrorMessage message={errorMessage} />
        </form>
    );
};
