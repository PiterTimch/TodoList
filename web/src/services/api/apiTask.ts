import { createApi } from "@reduxjs/toolkit/query/react";
import { createBaseQuery } from "../../utils/createBaseQuery.ts";
import type {ITaskItemResponse} from "../../types/task/ITaskItemResponse.ts";
import type {ITasksSearchRequest} from "../../types/task/ITasksSearchRequest.ts";

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
    }),
});

export const {
    useGetTasksQuery,
} = apiTasks;
