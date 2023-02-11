import { Center, Text, VStack } from "@chakra-ui/react";
import { CSSProperties } from "react";


export default function HomePage() {

    const hoverStyles: CSSProperties = {
        color: '#fc810f',
        transform: 'scale(1.1)'
    }

    return (
        <>
            <Center h='100%'>
                <VStack
                    _hover={hoverStyles}
                    transition='1s ease-out 100ms'
                >
                    <Text
                        fontSize='5rem'
                        as='i'
                        align='center'
                    >
                        "Life is a journey, not a destination."
                    </Text>
                    <Text fontSize='2rem' w='100%' align='right' p='4'>
                        ~Ralph Valdo Emerson
                    </Text>
                </VStack>
            </Center>
        </>
    )
}