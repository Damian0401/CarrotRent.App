import { Box, Button, Flex, Spacer, Text } from "@chakra-ui/react";
import { CSSProperties } from "react";
import { Link } from "react-router-dom";
import { Vehicle } from "../../../app/models/Vehicle";


interface Props {
    vehicle: Vehicle
}

export default function VehicleListItem({ vehicle }: Props) {

    const imageStyles: CSSProperties = {
        height: '3rem',
        width: '3rem',
        borderRadius: '0.5rem'
    }

    return (
        <>
            <Flex p='2' _hover={{ bgColor: 'green.400' }} borderRadius='0.5rem'>
                <img src={vehicle.imageUrl} style={imageStyles} alt='img' />
                <Box>
                    <Text p='1'>
                        <b>Model:</b> {vehicle.model}
                    </Text>
                    <Text p='1' pt='0'>
                        <b>Brand:</b> {vehicle.brand}
                    </Text>
                    <Text p='1' pt='0'>
                        <b>Year of production:</b> {vehicle.yearOfProduction}
                    </Text>
                    <Text p='1' pt='0'>
                        <b>Price per day:</b> {vehicle.price}$
                    </Text>
                </Box>
                <Spacer />
                <Flex direction='column'>
                    <Spacer />
                    <Button
                        size='xs'
                        colorScheme='teal'
                        as={Link} to={`/vehicles/${vehicle.id}`}
                    >
                        View
                    </Button>
                </Flex>
            </Flex>
        </>
    )
}