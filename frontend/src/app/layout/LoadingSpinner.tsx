import { Center, Spinner } from "@chakra-ui/react";

export default function LoadingSpinner() {

    return (
        <Center height='93vh'>
            <Spinner size='xl' />
        </Center>
    );
}