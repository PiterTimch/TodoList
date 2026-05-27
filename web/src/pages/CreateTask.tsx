import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useCreateTaskMutation } from "../services/api/apiTask.ts";
import type { ICreateTaskRequest } from "../types/task/ICreateTaskRequest.ts";
import { CreateTaskForm } from "../components/forms/CreateTaskForm.tsx";
import { getApiErrorMessage } from "../utils/getApiErrorMessage.ts";

const CreateTask: React.FC = () => {
    const navigate = useNavigate();
    const [createTask, { isLoading }] = useCreateTaskMutation();
    const [errorMessage, setErrorMessage] = useState<string | undefined>();

    const handleCreate = async (task: ICreateTaskRequest) => {
        setErrorMessage(undefined);

        try {
            await createTask(task).unwrap();
            navigate("/");
        } catch (error) {
            setErrorMessage(getApiErrorMessage(error));
        }
    };

    return (
        <div className="min-h-screen bg-amber-50 px-4 py-10 text-stone-800">
            <div className="mx-auto w-full max-w-3xl">
                <div className="mb-6 rounded-2xl bg-white p-5 shadow-sm sm:p-6">
                    <div className="flex items-center justify-between gap-3">
                        <h1 className="text-3xl font-semibold tracking-tight text-stone-900">
                            New Task
                        </h1>

                        <Link
                            to="/"
                            className="inline-flex items-center justify-center rounded-xl bg-stone-700 px-4 py-2 text-sm font-medium text-white shadow-sm transition hover:bg-stone-800"
                        >
                            Back
                        </Link>
                    </div>
                </div>

                <div className="rounded-2xl bg-white p-5 shadow-sm sm:p-6">
                    <CreateTaskForm
                        onSubmit={handleCreate}
                        isSubmitting={isLoading}
                        errorMessage={errorMessage}
                    />
                </div>
            </div>
        </div>
    );
};

export default CreateTask;
