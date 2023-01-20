import { CardBody, CardHeader } from "@chakra-ui/card";
import { Box, Button, Flex, FormLabel, Heading, Input } from "@chakra-ui/react";
import { forwardRef, useState } from "react";
import ContentCard from "../../../app/common/shared/ContentCard";
import DatePicker from "react-datepicker";
import { RentCreate as Rent } from "../../../app/models/Rent";
import agent from "../../../app/api/agent";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

interface Props {
    vehicleId: string;
}

export default function RentCreate({ vehicleId }: Props) {
    const navigate = useNavigate();

    const CustomInput = forwardRef((props: any, ref) => {
        return <Input {...props} ref={ref} />;
    });

    const [rent, setRent] = useState<Rent>({
        vehicleId: vehicleId,
        startDate: new Date(),
        endDate: new Date(),
    });

    const { startDate, endDate } = rent;

    const isDisabled = startDate.getDate() >= endDate.getDate() || startDate.getDate() < new Date().getDate();

    const handleSubmit = () => {
        agent.Rent
            .create(rent)
            .then(() => navigate('/rents'))
            .catch(() => toast.error('Unable to create rent, please select another date.'))
    }

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Create rent
                    </Heading>
                </CardHeader>
                <CardBody>
                    <Flex gap='4' p='2'>
                        <Box>
                            <FormLabel>Start date</FormLabel>
                            <DatePicker
                                selected={startDate}
                                onChange={(date: Date) => setRent({ ...rent, startDate: date })}
                                customInput={<CustomInput />}
                            />
                        </Box>
                        <Box>
                            <FormLabel>End date</FormLabel>
                            <DatePicker
                                selected={endDate}
                                onChange={(date: Date) => setRent({ ...rent, endDate: date })}
                                customInput={<CustomInput />}
                            />
                        </Box>
                    </Flex>
                </CardBody>
                <Flex justifyContent='flex-end'>
                    <Button colorScheme='teal' onClick={handleSubmit} isDisabled={isDisabled}>
                        Submit
                    </Button>
                </Flex>
            </ContentCard>
        </>
    )
}