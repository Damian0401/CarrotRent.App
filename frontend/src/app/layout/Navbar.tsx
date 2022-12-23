import { ArrowBackIcon, ArrowForwardIcon, AtSignIcon, CalendarIcon, EditIcon, HamburgerIcon, Search2Icon, ViewIcon } from "@chakra-ui/icons";
import { Center, Flex, HStack, Icon, IconButton, Menu, MenuButton, MenuItem, MenuList, Spacer, Text } from "@chakra-ui/react";
import { useContext } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { UserContext } from "../common/providers/UserProvider";
import { UserAction, UserActionType } from "../common/reducers/UserReducer";

export default function Navbar() {

    const { state: user, dispatch } = useContext(UserContext);

    const navigate = useNavigate();

    const handleLogout = () => {
        const action: UserAction = {
            type: UserActionType.CLEAR_USER
        };

        dispatch(action);
        navigate('/');
    }


    return (
        <>
            <Flex p='3' bgColor='blackAlpha.800' fontFamily='fantasy' width='100%' zIndex='1' height='7vh'>
                <HStack as={Link} to='/' >
                    <Icon as={CalendarIcon} w='8' h='8' color='whiteAlpha.800' />
                    <Text fontSize='3xl' noOfLines={1} variant='navbar-text'>
                        Carrot Rent
                    </Text>
                </HStack>
                <Spacer />
                <Center height='100%'>
                    {user && 
                    <Text variant='navbar-text'>
                        {user.login}
                    </Text>}
                    <Menu>
                        <MenuButton
                            as={IconButton}
                            aria-label='Options'
                            icon={<HamburgerIcon boxSize={6} color='whiteAlpha.800' />}
                            variant='navbar-button'
                        />
                        <MenuList fontFamily='sans-serif'>
                            <MenuItem icon={<Search2Icon />} as={Link} to='/map'>
                                Map
                            </MenuItem>
                            <MenuItem icon={<AtSignIcon />} as={Link} to='/departments'>
                                Departments
                            </MenuItem>
                            <MenuItem icon={<ViewIcon />} as={Link} to='/vehicles'>
                                Vehicles
                            </MenuItem>
                            {user ? <>
                                <MenuItem icon={<CalendarIcon />} as={Link} to='/rents'>
                                    Rents
                                </MenuItem>
                                <MenuItem icon={<ArrowBackIcon />} onClick={handleLogout}>
                                    Logout
                                </MenuItem>
                            </> : <>
                                <MenuItem icon={<ArrowForwardIcon />} as={Link} to='/login'>
                                    Login
                                </MenuItem>
                                <MenuItem icon={<EditIcon />} as={Link} to='/register'>
                                    Register
                                </MenuItem>
                            </>}
                        </MenuList>
                    </Menu>
                </Center>
            </Flex>
            <Outlet />
        </>
    )
}