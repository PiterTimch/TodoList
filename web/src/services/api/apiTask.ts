import { createApi } from "@reduxjs/toolkit/query/react";
import { createBaseQuery } from "../../utils/createBaseQuery.ts";
import type {ITaskItemResponse} from "../../types/task/ITaskItemResponse.ts";
import type {ITasksSearchRequest} from "../../types/task/ITasksSearchRequest.ts";
import type {ISetTaskCompletedRequest} from "../../types/task/ISetTaskCompletedRequest.ts";
import type {ICreateTaskRequest} from "../../types/task/ICreateTaskRequest.ts";

export const apiTasks = createApi({
    reducerPath: "tasks",
    baseQuery: createBaseQuery("Tasks"),
    tagTypes: ["Tasks"],
    endpoints: (builder) => ({

        getTasks: builder.query<ITaskItemResponse[], ITasksSearchRequest>({
            query: (params) => ({
                url: "",
                method: "GET",
                params,
            }),
            providesTags: ["Tasks"],
        }),

        deleteTask: builder.mutation<void, number>({
            query: (id) => ({
                url: `${id}`,
                method: "DELETE",
            }),
            invalidatesTags: ["Tasks"],
        }),

        setTaskCompleted: builder.mutation<void, ISetTaskCompletedRequest>({
            query: (body) => ({
                url: "complete",
                method: "PATCH",
                body,
            }),
            invalidatesTags: ["Tasks"],
        }),

        createTask: builder.mutation<ITaskItemResponse, ICreateTaskRequest>({
            query: (body) => ({
                url: "",
                method: "POST",
                body,
            }),
            invalidatesTags: ["Tasks"],
        }),
    }),
});

export const {
    useGetTasksQuery,
    useDeleteTaskMutation,
    useSetTaskCompletedMutation,
    useCreateTaskMutation,
} = apiTasks;
