import './App.css'
import {useGetTasksQuery} from "./services/api/apiTask.ts";
import type {ITasksSearchRequest} from "./types/task/ITasksSearchRequest.ts";

function App() {
    const search : ITasksSearchRequest = {}
    const {data} = useGetTasksQuery(search);

    console.log(data);

    return (
        <>
            <p className={"text-6xl text-center p-20 text-blue-500"}>Test</p>
        </>
    )
}

export default App
