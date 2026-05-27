import {configureStore} from "@reduxjs/toolkit";
import {setupListeners} from "@reduxjs/toolkit/query";
import {apiTasks} from "../services/api/apiTask.ts";

export const store = configureStore({
    reducer:{
        [apiTasks.reducerPath]: apiTasks.reducer
    },

    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(apiTasks.middleware)
})

setupListeners(store.dispatch)
