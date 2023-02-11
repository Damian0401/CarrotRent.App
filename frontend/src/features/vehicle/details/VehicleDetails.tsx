import { Button, ButtonGroup, Grid, GridItem, Heading, Text } from "@chakra-ui/react";
import { CSSProperties, useContext, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import agent from "../../../app/api/agent";
import { UserContext } from "../../../app/common/providers/UserProvider";
import ContentCard from "../../../app/common/shared/ContentCard";
import { userCanDeleteVehicle, userCanEditVehicle, userCanRentVehicle } from "../../../app/common/utils/helpers";
import LoadingSpinner from "../../../app/layout/LoadingSpinner";
import { VehicleDetails as Vehicle } from "../../../app/models/Vehicle";
import RentCreate from "../../rent/create/RentCreate";


export default function VehicleDetails() {

    const { id } = useParams<{ id: string }>();
    const { state: user } = useContext(UserContext);
    const navigate = useNavigate();

    const [vehicle, setVehicle] = useState<Vehicle>();
    const [rentMode, setRentMode] = useState<boolean>(false);

    useEffect(() => {
        agent.Vehicle.getById(id!).then(data => setVehicle(data));
    }, [id]);

    const imageStyles: CSSProperties = {
        borderRadius: '0.5rem',
    }

    const handleDelete = () => {
        if (!vehicle) return;

        agent.Vehicle.delete(vehicle?.id).then(() => navigate(-1));
    }

    if (!vehicle) return <LoadingSpinner />

    return (
        <>
            <ContentCard>
                <Grid
                    templateRows='repeat(5, 1fr)'
                    templateColumns='repeat(5, 1fr)'
                    gap={3}
                >
                    <GridItem rowSpan={4} colSpan={2}>
                        <img
                            src={vehicle.imageUrl}
                            style={imageStyles}
                            alt='vehicleImage'
                        />
                    </GridItem>
                    <GridItem rowSpan={4} colSpan={3}>
                        <Heading>
                            {vehicle.model}
                        </Heading>
                        <Text noOfLines={2}>
                            {vehicle.description}
                        </Text>
                        <Text>
                            <b>Brand:</b> {vehicle.brand}
                        </Text>
                        <Text>
                            <b>Fuel:</b> {vehicle.fuel}
                        </Text>
                        <Text>
                            <b>Year of production:</b> {vehicle.yearOfProduction}
                        </Text>
                        <Text>
                            <b>Seats:</b> {vehicle.seats}
                        </Text>
                        <Text>
                            <b>Status:</b> {vehicle.status}
                        </Text>
                        <Text>
                            <b>Price per day:</b> {vehicle.price}$
                        </Text>
                        <Text>
                            <b>Department:</b> {vehicle.department}
                        </Text>
                    </GridItem>
                    <GridItem colSpan={5} position='relative'>
                        {user && <ButtonGroup position='absolute' bottom='0' right='0'>
                            {userCanDeleteVehicle(user, vehicle) && <Button colorScheme='red' onClick={handleDelete}>
                                Delete
                            </Button>}
                            {userCanEditVehicle(user, vehicle) && <Button
                                colorScheme='blue'
                                as={Link}
                                to={`/vehicles/edit/${vehicle.id}`}
                            >
                                Edit
                            </Button>}
                            {userCanRentVehicle(user) && <>
                                <Button
                                    colorScheme='teal'
                                    onClick={() => setRentMode(!rentMode)}
                                >
                                    Rent
                                </Button>
                            </>}
                        </ButtonGroup>}
                    </GridItem>
                </Grid>
            </ContentCard>
            {rentMode && <RentCreate vehicleId={vehicle.id} />}
        </>
    )
}