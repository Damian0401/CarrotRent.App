

export interface Vehicle {
    id: string;
    model: string;
    brand: string;
    yearOfProduction: number;
    price: number;
    imageUrl: string;
}

export interface VehicleDetails {
    id: string;
    model: string;
    brand: string;
    yearOfProduction: number;
    imageUrl?: string;
    fuel: string;
    seats: number;
    description: string;
    status: string;
    department: string;
    departmentId: string;
    price: number;
}

export interface VehicleEditValues {
    yearOfProduction: number;
    seats: number;
    price: number;
    description: string;
    imageUrl?: string;
}

export interface VehicleCreateValues {
    description: string;
    imageUrl?: string;
    registration: string;
    vin: string;
    seats: number;
    yearOfProduction: number;
    price: number;
    modelId: string;
    departmentId: string;
    fuelId: string;
}

export interface VehicleForRent {
    id: string;
    model: string;
    brand: string;
    yearOfProduction: number;
}

export interface VehicleFilters {
    fuels: Fuel[];
    brands: Brand[];
    departments: Department[];
}

interface Fuel {
    id: string;
    type: string;
}

interface Brand {
    id: string;
    name: string;
    models: Model[]
}

interface Model {
    id: string;
    name: string;
}

interface Department {
    id: string;
    name: string;
}

export interface SelectedFilters {
    departmentId?: string;
    fuelId?: string;
    brandId?: string;
    modelId?: string;
    seats: number
    minPrice: number;
    maxPrice: number;
}