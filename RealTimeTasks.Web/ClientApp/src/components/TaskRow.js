import React,{useState,useRef,useEffect} from 'react';
import axios from 'axios';
import { useAuthContext } from '../AuthContext';
import { HubConnectionBuilder } from '@microsoft/signalr';

export default function TaskRow({ task }) {
    const {id,taskDescription,userId,status}=task;
    const [taskUserName,setTaskUserName]=useState("");
    const { user } = useAuthContext();
    const connectionRef = useRef(null);

    useEffect(() => {

        const connectToHub = async () => {
            const connection = new HubConnectionBuilder().withUrl("/tasks").build();
            await connection.start();
            connectionRef.current = connection;
        }

        connectToHub();

    }, []);
    
    const onTookTask = async () => {
        const connection = connectionRef.current;
        connection.invoke('TookTask', { taskId: id });
    }
    const onCompletedTask = async () => {
        const connection = connectionRef.current;
        connection.invoke('completedTask', { taskId: id });
    }

    const displayButtonWithUserName =  () => {      
        const getName=async()=>{
            const {data}=await axios.get(`/api/account/getNameOfUser?id=${userId}`);
            setTaskUserName(data.name);                      
        }  
        getName(); 
        return <button className="btn btn-warning" disabled="true">{taskUserName} is doing this one!</button>;           
    }

    return (
        <tr>
            <td>{taskDescription}</td>
            <td>
                {status === 1 && <button className="btn btn-primary" onClick={onTookTask}>I'm doing this one!</button>}
                {(status === 2 && userId === user.id) && <button className="btn btn-success" onClick={onCompletedTask}>I'm done!</button>}
                {(status === 2 && userId !== user.id) && displayButtonWithUserName()}
            </td>
        </tr>
    )
}