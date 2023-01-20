import { Box, Button, Text } from "@chakra-ui/react"
import { Rent } from "../../../app/models/Rent"
import { format } from 'date-fns'
import { Link } from "react-router-dom"

interface Props {
    rent: Rent
}

export default function RentListItem({ rent }: Props) {

    return (
        <>
            <Box
                _hover={{ bgColor: 'green.400' }}
                p='3' borderRadius='0.5rem'
                position='relative'
                boxSizing='border-box'
            >
                <Text as='b'>
                    {rent.vehicle}
                </Text>
                <Text>
                    {'start date: ' + format(rent.startDate, 'dd.MM.yyyy, HH:mm')}
                </Text>
                <Text>
                    {'end date: ' + format(rent.endDate, 'dd.MM.yyyy, HH:mm')}
                </Text>
                <Text>
                    {'status: ' + rent.status}
                </Text>
                <Button 
                    size='sm' m='2'
                    colorScheme='teal' 
                    position='absolute' 
                    bottom='0' right='0'
                    as={Link} to={`/rents/${rent.id}`}>
                    View
                </Button>
            </Box>
        </>
    )
}