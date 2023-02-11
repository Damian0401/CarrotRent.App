import { useContext } from "react";
import { Navigate } from "react-router-dom";
import { UserContext } from "../providers/UserProvider";



interface Props {
    roles?: string[];
    Component: React.ComponentType;
}

export default function PrivateRoute({ roles, Component }: Props) {
    const { state: user } = useContext(UserContext);
    const userHasRequiredRole = user && roles && roles.includes(user.role) ? true : false;

    if (!user) return <Navigate to='/' />;

    if (roles && !userHasRequiredRole) return <Navigate to='/' />;

    return <Component />;
}