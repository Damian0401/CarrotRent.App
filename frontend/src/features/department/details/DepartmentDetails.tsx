import { ButtonGroup, Flex, Heading, IconButton, Stack, StackDivider, Text, Tooltip } from "@chakra-ui/react";
import { useParams } from "react-router-dom";
import VehicleList from "../../vehicle/list/VehicleList";
import { DepartmentDetails as Department } from "../../../app/models/Department";
import { CardBody, CardHeader } from "@chakra-ui/card";
import ContentCard from "../../../app/common/shared/ContentCard";
import { useContext, useEffect, useState } from "react";
import LoadingSpinner from "../../../app/layout/LoadingSpinner";
import agent from "../../../app/api/agent";
import { AddIcon, CalendarIcon, EditIcon, RepeatClockIcon } from "@chakra-ui/icons";
import { userCanCreateVehicle, userCanManageEmployees, userCanManageRents } from "../../../app/common/utils/helpers";
import { UserContext } from "../../../app/common/providers/UserProvider";
import { Link } from "react-router-dom";

export default function DepartmentDetails() {

    const { departmentId } = useParams<{ departmentId: string }>();
    const [department, setDepartment] = useState<Department>();
    const { state: user } = useContext(UserContext);

    useEffect(() => {
        if (departmentId)
            agent.Department.getById(departmentId).then(data => setDepartment(data));
    }, [departmentId]);

    if (!department) return <LoadingSpinner />;

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        {department.name}
                    </Heading>
                </CardHeader>
                <CardBody>
                    <Stack divider={<StackDivider />} spacing='4' maxHeight='80vh'>
                        <StackDivider />
                        <Text>
                            Address: {department.address}
                        </Text>
                        <Text>
                            Manager: {department.manager}
                        </Text>
                        <Flex>
                            <ButtonGroup>
                                {userCanCreateVehicle(user, department?.id) && <Tooltip label='Add a new vehicle' >
                                    <IconButton
                                        aria-label="add-button"
                                        icon={<AddIcon />}
                                        as={Link} to={`/vehicles/create/${department.id}`}
                                    />
                                </Tooltip>}
                                {userCanManageEmployees(user, department?.id) && <Tooltip label='Add an employee' >
                                    <IconButton aria-label="manage-button" icon={<EditIcon />} />
                                </Tooltip>}
                                {userCanManageRents(user, department?.id) && <>
                                    <Tooltip label='Manage rents' >
                                        <IconButton
                                            aria-label="rents-button"
                                            icon={<CalendarIcon />}
                                            as={Link}
                                            to={`/departments/${departmentId}/rents`}
                                        />
                                    </Tooltip>
                                    <Tooltip label='Archived rents' >
                                        <IconButton
                                            aria-label="rents-archive-button"
                                            icon={<RepeatClockIcon />}
                                            as={Link}
                                            to={`/departments/${departmentId}/rents/archive`}
                                        />
                                    </Tooltip>
                                </>}
                            </ButtonGroup>
                        </Flex>
                        <VehicleList vehicles={department.vehicles} />
                    </Stack>
                </CardBody>
            </ContentCard>
        </>
    )
}