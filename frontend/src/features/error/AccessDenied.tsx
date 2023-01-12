import { Alert, AlertDescription, AlertIcon, AlertTitle, Box } from "@chakra-ui/react";

interface Props {
    content?: JSX.Element;
}

export default function AccessDenied({content}: Props) {
    return (
        <>
            <Box w='100%' p={4}>
                <Alert status='warning' p='2' borderRadius='0.5rem'>
                    <AlertIcon />
                    <AlertTitle>Access denied!</AlertTitle>
                    <AlertDescription>You do not have permission to access this page.</AlertDescription>
                </Alert>
            </Box>
            {content}
        </>
    )
}