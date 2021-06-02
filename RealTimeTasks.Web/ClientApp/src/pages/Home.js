
import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import TaskRow from '../components/TaskRow';

export default function Home() {
    const [tasks, setTasks] = useState([]);
    const [taskDesc, setTaskDesc] = useState("");

    const connectionRef = useRef(null);


    useEffect(() => {

        const connectToHub = async () => {
            const connection = new HubConnectionBuilder().withUrl("/tasks").build();
            await connection.start();
            connection.invoke('getAllTasks');
            connectionRef.current = connection;

            connection.on('getAllTasks', obj => {
                setTasks(obj);
            });
        }
        connectToHub();

    }, []);

    const onAddTask = async () => {
        const connection = connectionRef.current;
        connection.invoke('addTask', { taskDesc });
        setTaskDesc('');
    }

    return (
        <div style={{ margintop: "70px" }}>
            <div className="row">
                <div className="col-md-10">
                    <input type="text" className="form-control" onChange={(e) => setTaskDesc(e.target.value)} placeholder="Task Title" value={taskDesc} />
                </div>
                <div className="col-md-2">
                    <button className="btn btn-primary btn-block" disabled={taskDesc === ""} onClick={onAddTask}>Add Task</button>
                </div>
            </div>
            <table className="table table-hover table-striped table-bordered mt-3">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {tasks.map(t => <TaskRow task={t} />)}
                </tbody>
            </table>
        </div>

    )
}