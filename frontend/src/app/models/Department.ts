import { Vehicle } from "./Vehicle";



export interface Department {
    id: string;
    name: string;
    address: string;
}

export interface DepartmentMarker {
    id: string;
    name: string;
    address: string;
    xPosition: number;
    yPosition: number;
}

export interface DepartmentDetails {
    id: string;
    name: string;
    vehicles: Vehicle[],
    manager: string | undefined;
    address: string;
}