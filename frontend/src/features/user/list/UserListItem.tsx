import { Text, Flex, Heading, Stack, Spacer, Button } from "@chakra-ui/react";
import { UserDetails } from "../../../app/models/User";

interface Props {
    user: UserDetails,
    buttonName?: string,
    onClick?: (id: string) => void
}

export default function UserListItem({ user, buttonName, onClick }: Props) {


    return (
        <>
            <Stack
                _hover={{ bgColor: 'green.400' }}
                p='3' borderRadius='0.5rem'
            >
                <Heading size='sm'>
                    {user.firstName} {user.lastName}
                </Heading>
                <Flex>
                    <Stack>
                        <Text>
                            <strong>Email:</strong> {user.email}
                        </Text>
                        <Text>
                            <strong>PhoneNumber:</strong> {user.phoneNumber}
                        </Text>
                        <Text>
                            <strong>Pesel:</strong> {user.pesel}
                        </Text>
                    </Stack>
                    <Spacer />
                    {buttonName && <Button
                        onClick={onClick ? () => onClick(user.id) : undefined}
                        colorScheme='teal'
                    >
                        {buttonName}
                    </Button>}
                </Flex>
            </Stack>
        </>
    )
}