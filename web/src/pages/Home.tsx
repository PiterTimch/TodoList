import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useGetTasksQuery } from "../services/api/apiTask.ts";
import type { ITasksSearchRequest } from "../types/task/ITasksSearchRequest.ts";
import { SearchForm } from "../components/forms/SearchForm.tsx";
import { TaskItem } from "../components/tasks/TaskItem.tsx";

const Home: React.FC = () => {
    const [search, setSearch] = useState<ITasksSearchRequest>({});
    const { data } = useGetTasksQuery(search);

    const handleSearch = (next: ITasksSearchRequest) => {
        setSearch(next);
    };

    return (
        <div className="min-h-screen bg-amber-50 px-4 py-10 text-stone-800">
            <div className="mx-auto w-full max-w-3xl">
                <div className="mb-6 rounded-2xl bg-white p-5 shadow-sm sm:p-6">
                    <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
                        <h1 className="text-3xl font-semibold tracking-tight text-stone-900">
                            Todo List
                        </h1>

                        <Link
                            to="/create"
                            className="inline-flex items-center justify-center rounded-xl bg-stone-700 px-4 py-2 text-sm font-medium text-white shadow-sm transition hover:bg-stone-800"
                        >
                            +
                        </Link>
                    </div>
                </div>

                <div className="mb-6 rounded-2xl bg-white p-5 shadow-sm sm:p-6">
                    <SearchForm onSearch={handleSearch} />
                </div>

                <div className="space-y-4">
                    {(data?.length ?? 0) === 0 ? (
                        <div className="rounded-2xl bg-white p-5 text-center text-stone-500 shadow-sm">
                            No tasks found.
                        </div>
                    ) : null}

                    {data?.map((task) => <TaskItem key={task.id} task={task} />)}
                </div>
            </div>
        </div>
    );
};

export default Home;