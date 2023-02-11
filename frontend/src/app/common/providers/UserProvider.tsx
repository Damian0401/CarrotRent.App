import { createContext, Dispatch, PropsWithChildren, useReducer } from "react";
import { User } from "../../models/User";
import { UserAction, UserReducer } from "../reducers/UserReducer";

type InitialState = {
    state?: User;
    dispatch: Dispatch<UserAction>
}

export const UserContext = createContext<InitialState>({
    dispatch: () => null
});

export const UserProvider = ({children}: PropsWithChildren) => {
    const userAsString = window.localStorage.getItem('user');
    const user: User | undefined = !!userAsString ? JSON.parse(userAsString) : undefined;
    const [state, dispatch] = useReducer(UserReducer, user);

    return (
        <UserContext.Provider value={{state, dispatch}}>{children}</UserContext.Provider>
    )
}