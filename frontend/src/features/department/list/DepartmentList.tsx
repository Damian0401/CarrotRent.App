import { Flex, Stack, StackDivider } from "@chakra-ui/react"
import { Department } from "../../../app/models/Department"
import DepartmentListItem from "./DepartmentListItem"

interface Props {
    departments: Department[]
}

export default function DepartmentList({departments}: Props) {

    return (
        <>
            <Flex direction='column' maxHeight='80vh'>
                <Stack divider={<StackDivider />} spacing='4' overflow='auto'>
                    {departments.map(department => (
                        <DepartmentListItem department={department} key={department.id} />
                    ))}
                </Stack>
            </Flex>
        </>
    )
}