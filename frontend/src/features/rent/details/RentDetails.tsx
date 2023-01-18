import { CardBody, CardHeader } from "@chakra-ui/card";
import { Box, Button, Heading, Stack, StackDivider, Text } from "@chakra-ui/react";
import { format } from "date-fns";
import { Link, useParams } from "react-router-dom";
import ContentCard from "../../../app/common/shared/ContentCard";
import { RentDetails as Rent } from "../../../app/models/Rent"


export default function RentDetails() {

    const { id } = useParams<{ id: string }>();

    const rent: Rent = {
        id: id || 'id',
        status: 'active',
        client: 'client',
        clientId: 'clientId',
        renter: 'renter',
        renterId: 'renterId',
        receiver: 'receiver',
        receiverId: 'receiverId',
        startDate: new Date(),
        endDate: new Date(),
        vehicle: {
            id: '1',
            model: 'model',
            brand: 'brand',
            yearOfProduction: 2000
        }
    }

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Rent details
                    </Heading>
                </CardHeader>
                <CardBody position='relative'>
                    <Stack divider={<StackDivider />} spacing='4' maxHeight='80vh' position='relative'>
                        <StackDivider />
                        <Text>
                            <b>Status:</b> {rent.status}
                        </Text>
                        <Text>
                            <b>Client:</b> {rent.client}
                        </Text>
                        <Text>
                            <b>Renter:</b> {rent.renter}
                        </Text>
                        <Text>
                            <b>Receiver:</b> {rent.receiver}
                        </Text>
                        <Text>
                            <b>Start date:</b> {rent.startDate && format(rent.startDate, 'dd.MM.yyyy, HH:mm')}
                        </Text>
                        <Text>
                            <b>End date:</b> {rent.endDate && format(rent.endDate, 'dd.MM.yyyy, HH:mm')}
                        </Text>
                        <Box
                            as={Link} to={`/vehicles/${rent.vehicle.id}`}
                            boxShadow='lg' p='2' _hover={{ bgColor: 'green.400' }}
                            width='fit-content' borderRadius='0.5rem'
                        >
                            <Text as='b'>
                                Vehicle details:
                            </Text>
                            <Text>
                                Model: {rent.vehicle.model}
                            </Text>
                            <Text>
                                Brand: {rent.vehicle.brand}
                            </Text>
                            <Text>
                                Year of production: {rent.vehicle.yearOfProduction}
                            </Text>
                        </Box>
                    </Stack>
                        <Button position='absolute' bottom='0' right='0' colorScheme='teal'>
                            Do something
                        </Button>
                </CardBody>
            </ContentCard>
        </>
    )
}