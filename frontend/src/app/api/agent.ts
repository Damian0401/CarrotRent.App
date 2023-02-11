import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { BASE_API_URL } from "../common/utils/constants";
import { Department as DepartmentDto, DepartmentDetails, DepartmentMarker } from "../models/Department";
import { CreateEmployeeValues, User, UserDetails, UserLoginValues, UserRegisterValues } from "../models/User";
import { SelectedFilters, Vehicle as VehicleDto, VehicleCreateValues, VehicleDetails, VehicleEditValues, VehicleFilters } from "../models/Vehicle";
import { history } from "../..";
import { Rent as RentDto, RentCost, RentCreate, RentDetails } from "../models/Rent";

axios.defaults.baseURL = BASE_API_URL;

axios.interceptors.request.use(request => {
    const userAsString = window.localStorage.getItem('user');
    const user: User | undefined = !!userAsString ? JSON.parse(userAsString) : undefined;
    if (user) request.headers!.Authorization = `Bearer ${user.token}`;
    return request;
});

axios.interceptors.response.use(response => response, (error: AxiosError) => {
    if (!error.response) {
        toast.error('Network error');
        return Promise.reject(error);
    }

    const { status } = error.response;
    switch (status) {
        case 403:
            history.push('/accessDenied');
            break;
        case 404:
            history.push('/notFound');
            break;
    }

    return Promise.reject(error);
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Department = {
    getAll: () => requests.get<{ departments: DepartmentDto[] }>('/department').then(x => x.departments),
    getMarkers: () => requests.get<{ departments: DepartmentMarker[] }>('/department').then(x => x.departments),
    getById: (id: string) => requests.get<DepartmentDetails>(`/department/${id}`),
    getRents: (id: string) => requests.get<{ rents: RentDto[] }>(`/department/${id}/rents`).then(x => {
        x.rents.forEach(rent => rent.startDate = new Date(rent.startDate));
        x.rents.forEach(rent => rent.endDate = new Date(rent.endDate));
        return x.rents
    }),
    getArchivedRents: (id: string) => requests.get<{ rents: RentDto[] }>(`/department/${id}/rents/archived`).then(x => {
        x.rents.forEach(rent => rent.startDate = new Date(rent.startDate));
        x.rents.forEach(rent => rent.endDate = new Date(rent.endDate));
        return x.rents
    }),
};

const Vehicle = {
    getAll: ({ maxPrice, minPrice, seats, brandId, departmentId, fuelId, modelId }: SelectedFilters) => {
        let query = '';
        if (maxPrice) query += `maxPrice=${maxPrice}&`;
        if (minPrice) query += `minPrice=${minPrice}&`;
        if (seats) query += `seats=${seats}&`;
        if (brandId) query += `brandId=${brandId}&`;
        if (departmentId) query += `departmentId=${departmentId}&`;
        if (fuelId) query += `fuelId=${fuelId}&`;
        if (modelId) query += `modelId=${modelId}&`;
        return requests.get<{ vehicles: VehicleDto[] }>(`/vehicle?${query}`).then(x => x.vehicles)
    },
    getById: (id: string) => requests.get<VehicleDetails>(`/vehicle/${id}`),
    getFilters: () => requests.get<VehicleFilters>('/vehicle/filters'),
    delete: (id: string) => requests.delete(`/vehicle/${id}`),
    update: (id: string, vehicle: VehicleEditValues) => requests.put(`/vehicle/${id}`, vehicle),
    create: (vehicle: VehicleCreateValues) => requests.post('/vehicle', vehicle),
};

const Account = {
    login: (loginValues: UserLoginValues) => requests.post<User>('account/login', loginValues),
    register: (registerValues: UserRegisterValues) => requests.post<User>('/account/register', registerValues),
    createEmployee: (createEmployeeValues: CreateEmployeeValues, deparmentId: string) =>
        requests.post<User>(`/account/register/${deparmentId}/employee`, createEmployeeValues),
    verify: (id: string) => requests.post(`/account/verify/${id}`, {}),
    unverified: () => requests.get<{ users: UserDetails[] }>('/account/unverified').then(x => x.users),
}

const Rent = {
    getById: (id: string) => requests.get<RentDetails>(`/rent/${id}`).then(rent => {
        rent.startDate = new Date(rent.startDate);
        rent.endDate = new Date(rent.endDate);
        return rent;
    }),
    cancel: (id: string) => requests.post(`/rent/${id}/cancel`, {}),
    issue: (id: string) => requests.post(`/rent/${id}/issue`, {}),
    receive: (id: string) => requests.post<RentCost>(`/rent/${id}/receive`, {}),
    create: (rent: RentCreate) => requests.post('/rent', rent),
    getMy: () => requests.get<{ rents: RentDto[] }>('rent/my').then(x => {
        x.rents.forEach(rent => rent.startDate = new Date(rent.startDate));
        x.rents.forEach(rent => rent.endDate = new Date(rent.endDate));
        return x.rents
    }),
    getMyArchived: () => requests.get<{ rents: RentDto[] }>('rent/my/archived').then(x => {
        x.rents.forEach(rent => rent.startDate = new Date(rent.startDate));
        x.rents.forEach(rent => rent.endDate = new Date(rent.endDate));
        return x.rents
    }),
}

const agent = {
    Department,
    Vehicle,
    Account,
    Rent
};

export default agent;