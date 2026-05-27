import React, { useState } from "react";
import type { ITasksSearchRequest } from "../../types/task/ITasksSearchRequest.ts";
import { TextInput } from "./TextInput.tsx";
import { CalendarInput } from "./CalendarInput.tsx";

type SearchFormProps = {
    onSearch: (search: ITasksSearchRequest) => void;
};

export const SearchForm: React.FC<SearchFormProps> = ({ onSearch }) => {
    const [name, setName] = useState<string>("");
    const [dueDate, setDueDate] = useState<string | undefined>(undefined);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        onSearch({
            ...(name.trim() ? { name: name.trim() } : {}),
            ...(dueDate ? { dueDate } : {}),
        });
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <TextInput
                label="Search by name"
                value={name}
                onChange={setName}
                placeholder="Eat"
            />
            <CalendarInput label="Due date" value={dueDate} onChange={setDueDate} />

            <button
                type="submit"
                className="w-full rounded-xl bg-stone-700 px-3 py-2.5 text-sm font-medium text-white shadow-sm transition hover:bg-stone-800"
            >
                Find
            </button>
        </form>
    );
};

