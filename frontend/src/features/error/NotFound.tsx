import { Alert, AlertDescription, AlertIcon, AlertTitle, Box } from "@chakra-ui/react";

interface Props {
    content?: JSX.Element;
}

export default function NotFound({content}: Props) {
    return (
        <>
            <Box w='100%' p={4}>
                <Alert status='error' p='2' borderRadius='0.5rem'>
                    <AlertIcon />
                    <AlertTitle>Not found!</AlertTitle>
                    <AlertDescription>We are so sorry, but we can't find what you are looking for.</AlertDescription>
                </Alert>
            </Box>
            {content}
        </>
    )
}