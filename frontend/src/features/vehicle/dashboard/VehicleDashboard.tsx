import { CardBody, CardHeader } from "@chakra-ui/card";
import { Heading, Stack } from "@chakra-ui/react";
import { useState } from "react";
import agent from "../../../app/api/agent";
import ContentCard from "../../../app/common/shared/ContentCard";
import { SelectedFilters, Vehicle } from "../../../app/models/Vehicle";
import VehicleList from "../list/VehicleList";
import VehicleFilters from "./VehicleFilters";


export default function VehicleDashboard() {

    const [vehicles, setVehicles] = useState<Vehicle[]>([]);
    
    const handleSearch = (filters: SelectedFilters) => {
        agent.Vehicle.getAll(filters).then(data => setVehicles(data));
    }

    return (
        <ContentCard>
            <CardHeader>
                <Heading>
                    Vehicles
                </Heading>
            </CardHeader>
            <CardBody>
                <Stack maxHeight='80vh'>
                    <VehicleFilters handleSearch={handleSearch} />
                    <VehicleList vehicles={vehicles} />
                </Stack>
            </CardBody>
        </ContentCard>
    )
}