import { Button, Flex, Heading, Spacer, Stack, Text } from "@chakra-ui/react";
import { useContext } from "react";
import { Link } from "react-router-dom";
import { UserContext } from "../../../app/common/providers/UserProvider";
import { Department } from "../../../app/models/Department";


interface Props {
    department: Department;
}

export default function DepartmentListItem({ department }: Props) {
    const { state: user } = useContext(UserContext);

    return (
        <>
            <Stack
                _hover={{ bgColor: 'green.400' }}
                p='3' borderRadius='0.5rem'
                >
                <Heading size='sm'>
                    {department.name}
                    {user?.departmentIds.includes(department.id) && ' *'}
                </Heading>
                <Flex>
                    <Text>
                        {department.address}
                    </Text>
                    <Spacer />
                    <Button
                        size='sm'
                        colorScheme='teal'
                        as={Link}
                        to={`/departments/${department.id}`}
                    >
                        View
                    </Button>
                </Flex>
            </Stack>
        </>
    )
}