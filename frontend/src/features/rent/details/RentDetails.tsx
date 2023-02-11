import { CardBody, CardHeader } from "@chakra-ui/card";
import { Box, Button, ButtonGroup, Heading, Stack, StackDivider, Text } from "@chakra-ui/react";
import { format } from "date-fns";
import { useContext, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import agent from "../../../app/api/agent";
import { UserContext } from "../../../app/common/providers/UserProvider";
import ContentCard from "../../../app/common/shared/ContentCard";
import { userCanCancelRent, userCanIssueRent, userCanReceiveRent } from "../../../app/common/utils/helpers";
import LoadingSpinner from "../../../app/layout/LoadingSpinner";
import { RentDetails as Rent } from "../../../app/models/Rent"


export default function RentDetails() {

    const { rentId } = useParams<{ rentId: string }>();
    const { state: user } = useContext(UserContext);
    const [rent, setRent] = useState<Rent>();
    const navigate = useNavigate();

    useEffect(() => {
        if (rentId)
            agent.Rent.getById(rentId).then(data => setRent(data));
    }, [rentId]);

    const handleCancel = () => {
        if (rent)
            agent.Rent.cancel(rent.id).then(() => navigate(-1));
    }

    const handleIssue = () => {
        if (rent)
            agent.Rent.issue(rent.id).then(() => navigate(-1));
    }

    const handleReceive = () => {
        if (rent)
            agent.Rent.receive(rent.id).then(() => navigate(-1));
    }

    if (!rent) return <LoadingSpinner />

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
                        {rent.renter && <Text>
                            <b>Renter:</b> {rent.renter}
                        </Text>}
                        {rent.receiver && <Text>
                            <b>Receiver:</b> {rent.receiver}
                        </Text>}
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
                    <ButtonGroup position='absolute' bottom='0' right='0' colorScheme='teal'>
                        {userCanCancelRent(user, rent) && <Button onClick={handleCancel}>
                            Cancel
                        </Button>}
                        {userCanIssueRent(user, rent) && <Button onClick={handleIssue}>
                            Issue
                        </Button>}
                        {userCanReceiveRent(user, rent) && <Button onClick={handleReceive}>
                            Receive
                        </Button>}
                    </ButtonGroup>
                </CardBody>
            </ContentCard>
        </>
    )
}