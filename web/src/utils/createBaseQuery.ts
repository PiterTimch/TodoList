import {fetchBaseQuery} from "@reduxjs/toolkit/query";
import APP_ENV from "../env";

export  const  createBaseQuery = (endpoint: string) =>
{
    const baseUrl = `${APP_ENV.API_BASE_URL ?? "/api"}`.replace(/\/+$/, "");
    return fetchBaseQuery({
        baseUrl: `${baseUrl}/${endpoint}`
    });
}