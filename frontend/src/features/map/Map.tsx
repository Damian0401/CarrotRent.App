import { useEffect, useState } from "react";
import { MapContainer, TileLayer, ZoomControl } from "react-leaflet";
import agent from "../../app/api/agent";
import { DepartmentMarker } from "../../app/models/Department";
import "./leaf.css";
import MapMaker from "./MapMarker";

export default function Map() {

    useEffect(() => {
        agent.Department.getMarkers().then(data => setDepartments(data));
    }, []);

    const [departments, setDepartments] = useState<DepartmentMarker[]>([]);

    return (
        <>
            <MapContainer center={[51.1078852, 17.0385376]} zoom={14} scrollWheelZoom={true} zoomControl={false}>
                <TileLayer
                    attribution='<a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <ZoomControl position='bottomright' />
                {departments.map(department => (
                    <MapMaker department={department} key={department.id} />
                ))}
            </MapContainer>
        </>
    )
}