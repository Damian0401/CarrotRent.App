import { Reducer } from "react";
import { User } from "../../models/User";



export enum UserActionType {
    SET_USER,
    CLEAR_USER
};

export type UserAction = {
    type: UserActionType;
    payload?: any;
};

export const UserReducer: Reducer<User | undefined, UserAction> = (state, action) => {
    switch (action.type) {
        case UserActionType.SET_USER:
            const userAsString = JSON.stringify(action.payload);
            window.localStorage.setItem('user', userAsString);
            return action.payload;
        case UserActionType.CLEAR_USER:
            window.localStorage.removeItem('user');
            return null;
        default:
            return state;
    }
}

export {}