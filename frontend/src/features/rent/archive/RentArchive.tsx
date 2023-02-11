import { CardBody, CardHeader } from "@chakra-ui/card";
import { Heading } from "@chakra-ui/react";
import { useContext, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import agent from "../../../app/api/agent";
import { UserContext } from "../../../app/common/providers/UserProvider";
import ContentCard from "../../../app/common/shared/ContentCard";
import { CLIENT } from "../../../app/common/utils/constants";
import { userCanManageRents } from "../../../app/common/utils/helpers";
import { Rent } from "../../../app/models/Rent";
import RentList from "../list/RentList";



export default function RentArchive() {
    
    const { departmentId } = useParams<{ departmentId: string }>();
    const { state: user } = useContext(UserContext);
    const [rents, setRents] = useState<Rent[]>([]);
    
    useEffect(() => {
        if (user?.role === CLIENT)
            agent.Rent.getMyArchived().then(rents => setRents(rents));
            
        if (departmentId && userCanManageRents(user, departmentId))
            agent.Department.getArchivedRents(departmentId).then(rents => setRents(rents));
    }, [user, departmentId]);
    

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading size='lg' p='2'>
                        Archived rents:
                    </Heading>
                </CardHeader>
                <CardBody>
                    <RentList rents={rents} />
                </CardBody>
            </ContentCard>
        </>
    )
}