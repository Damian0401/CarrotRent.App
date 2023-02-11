import { Box, Text } from "@chakra-ui/react";
import { Vehicle } from "../../../app/models/Vehicle";
import VehicleListItem from "./VehicleListItem";

interface Props {
    vehicles: Vehicle[]
}

export default function VehicleList({ vehicles }: Props) {

    return (
        <>
            <Text>
                List of vehicles:
            </Text>
            <Box overflow='auto'>
                {vehicles.map(vehicle => (
                    <VehicleListItem vehicle={vehicle} key={vehicle.id} />
                ))}
            </Box>
        </>
    )
}