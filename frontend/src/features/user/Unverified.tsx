import { CardBody, CardHeader } from "@chakra-ui/card";
import { Heading } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import agent from "../../app/api/agent";
import ContentCard from "../../app/common/shared/ContentCard";
import { UserDetails } from "../../app/models/User";
import UserList from "./list/UserList";


export function Unverified() {

    const [users, setUsers] = useState<UserDetails[]>([
        {
            id: '1',
            email: 'email',
            firstName: "firstName",
            lastName: "lastName",
            pesel: "pesel",
            phoneNumber: "phoneNumber",
        },
        {
            id: '2',
            email: 'email',
            firstName: "firstName",
            lastName: "lastName",
            pesel: "pesel",
            phoneNumber: "phoneNumber",
        },
        {
            id: '3',
            email: 'email',
            firstName: "firstName",
            lastName: "lastName",
            pesel: "pesel",
            phoneNumber: "phoneNumber",
        },
    ]);

    useEffect(() => {
        agent.Account.unverified().then(x => setUsers(x))
    },[]);



    const handleVefiry = (id: string) => {
        console.log(id);
        agent.Account
            .verify(id)
            .then(() => setUsers(users.filter(x => x.id !== id)))
            .catch(() => toast.error('Unable to verify user'));
    }


    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Unverified users
                    </Heading>
                </CardHeader>
                <CardBody overflow='auto' maxHeight='80vh'>
                    <UserList users={users} buttonName={'Verify'} onClick={handleVefiry} />
                </CardBody>
            </ContentCard>
        </>
    )
}