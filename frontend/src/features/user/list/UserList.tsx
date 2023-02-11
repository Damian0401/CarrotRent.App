import { Box, Text } from "@chakra-ui/react";
import { UserDetails } from "../../../app/models/User";
import UserListItem from "./UserListItem";

interface Props {
    users: UserDetails[],
    buttonName?: string,
    onClick?: (id: string) => void
}

export default function UserList(props: Props) {
    const {users} = props;

    return (
        <>
            <Text>
                List of users:
            </Text>
            <Box>
                {users.map(user => (
                    <UserListItem user={user} key={user.id} {...props} />
                ))}
            </Box>
        </>
    )
}