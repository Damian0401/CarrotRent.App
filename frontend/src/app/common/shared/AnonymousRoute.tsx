import { useContext } from "react";
import { Navigate } from "react-router-dom";
import { UserContext } from "../providers/UserProvider";


interface Props {
    Component: React.ComponentType;
}

export default function AnonymousRoute({ Component }: Props) {
    const { state: user } = useContext(UserContext);

    if (user) return <Navigate to='/' />;

    return <Component />
}