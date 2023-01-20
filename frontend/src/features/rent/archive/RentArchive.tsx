import { CardBody, CardHeader } from "@chakra-ui/card";
import { Heading } from "@chakra-ui/react";
import { useContext, useEffect, useState } from "react";
import agent from "../../../app/api/agent";
import { UserContext } from "../../../app/common/providers/UserProvider";
import ContentCard from "../../../app/common/shared/ContentCard";
import { CLIENT } from "../../../app/common/utils/constants";
import { Rent } from "../../../app/models/Rent";
import RentList from "../list/RentList";



export default function RentArchive() {
    
    const { state: user } = useContext(UserContext);
    const [rents, setRents] = useState<Rent[]>([]);
    
    useEffect(() => {
            if (user?.role === CLIENT)
                agent.Rent.getMyArchived().then(rents => setRents(rents));
    }, []);
    

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