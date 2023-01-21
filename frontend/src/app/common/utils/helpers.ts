import { RentDetails } from "../../models/Rent";
import { User } from "../../models/User";
import { VehicleDetails } from "../../models/Vehicle";
import { ACTIVE, CLIENT, EMPLOYEE, MANAGER, RESERVED } from "./constants";


export const userCanDeleteVehicle = (user?: User, vehicle?: VehicleDetails) => 
    user?.role === MANAGER && vehicle && user.departmentIds.includes(vehicle.departmentId);

export const userCanEditVehicle = (user?: User, vehicle?: VehicleDetails) => 
    (user?.role === MANAGER || user?.role === EMPLOYEE) && vehicle 
    && user.departmentIds.includes(vehicle.departmentId);

export const userCanRentVehicle = (user?: User) =>  user?.role === CLIENT;

export const userCanCreateVehicle = (user?: User, departmentId?: string) =>
    user?.role === MANAGER && departmentId && user.departmentIds.includes(departmentId);
    
export const userCanManageRents = (user?: User, departmentId?: string) =>
    (user?.role === MANAGER || user?.role === EMPLOYEE) && departmentId 
    && user.departmentIds.includes(departmentId);
    
export const userCanManageEmployees = (user?: User, departmentId?: string) =>
    user?.role === MANAGER && departmentId && user.departmentIds.includes(departmentId);

export const userCanCancelRent = (user?: User, rent?: RentDetails) =>
    user?.role === CLIENT && rent?.status === RESERVED;

export const userCanIssueRent = (user?: User, rent?: RentDetails) =>
    (user?.role === MANAGER || user?.role === EMPLOYEE) && rent?.status === RESERVED;
    
export const userCanReceiveRent = (user?: User, rent?: RentDetails) =>
    (user?.role === MANAGER || user?.role === EMPLOYEE) && rent?.status === ACTIVE;