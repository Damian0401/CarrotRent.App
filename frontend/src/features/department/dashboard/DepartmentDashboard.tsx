import { CardBody, CardHeader } from "@chakra-ui/card";
import { Heading } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import agent from "../../../app/api/agent";
import ContentCard from "../../../app/common/shared/ContentCard";
import { Department } from "../../../app/models/Department";
import DepartmentList from "../list/DepartmentList";

export default function DepartmentDashboard() {

    useEffect(() => {
        agent.Department.getAll().then(data => setDepartments(data));
    }, []);

    const [departments, setDepartments] = useState<Department[]>([]);

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading size='lg' p='2'>
                        Available departments:
                    </Heading>
                </CardHeader>
                <CardBody>
                    <DepartmentList departments={departments} />
                </CardBody>
            </ContentCard>
        </>
    )
}