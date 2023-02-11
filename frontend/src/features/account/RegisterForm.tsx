import { CardBody, CardHeader } from "@chakra-ui/card";
import { Button, ButtonGroup, Flex, Heading, Spacer, Link as ChakraLink, Checkbox, Tooltip } from "@chakra-ui/react";
import { Formik } from "formik";
import { Link, useNavigate } from "react-router-dom";
import MyInput from "../../app/common/form/MyInput";
import ContentCard from "../../app/common/shared/ContentCard";
import * as Yup from 'yup';
import { UserRegisterValues } from "../../app/models/User";
import { useContext, useState } from "react";
import { UserContext } from "../../app/common/providers/UserProvider";
import agent from "../../app/api/agent";
import { UserAction, UserActionType } from "../../app/common/reducers/UserReducer";
import { toast } from "react-toastify";


export default function RegisterForm() {

    const [isPrivacyAccepted, setIsPrivacyAccepted] = useState(false);

    const initialValues: UserRegisterValues = {
        email: "",
        login: "",
        firstName: "", 
        lastName: "",
        password: "",
        apartmentNumber: "",
        city: "",
        houseNumber: "",
        pesel: "",
        phoneNumber: "",
        postCode: "",
        street: "",
    }

    const { dispatch } = useContext(UserContext);

    const navigate = useNavigate();

    const handleRegster = (registerValues: UserRegisterValues) => {
        agent.Account.register(registerValues).then(user => {
            if (!user) return;
            const action: UserAction = {
                type: UserActionType.SET_USER,
                payload: user
            };
            dispatch(action);
            navigate('/');
        }).catch(() => toast.error('Invalid user data'));
    }


    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Register
                    </Heading>
                </CardHeader>
                <CardBody overflow='auto' p='2'>
                    <Formik
                        initialValues={initialValues}
                        onSubmit={handleRegster}
                        validationSchema={Yup.object({
                            login: Yup.string().required(),
                            password: Yup.string().required(),
                            email: Yup.string().required().email(),
                            apartmentNumber: Yup.string().required(),
                            city: Yup.string().required(),
                            houseNumber: Yup.string().required(),
                            postCode: Yup.string().required(),
                            street: Yup.string().required(),
                            pesel: Yup.number().required(),
                            phoneNumber: Yup.string().required(),
                            firstName: Yup.string().required(),
                            lastName: Yup.string().required(),
                        })}
                    >
                        {({ handleSubmit, errors, touched }) => (
                            <form onSubmit={handleSubmit}>
                                <MyInput
                                    label="Email"
                                    name="email"
                                    errors={errors.email}
                                    touched={touched.email}
                                    type="email"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="FirstName"
                                    name="firstName"
                                    errors={errors.firstName}
                                    touched={touched.firstName}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="LastName"
                                    name="lastName"
                                    errors={errors.lastName}
                                    touched={touched.lastName}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="Login"
                                    name="login"
                                    errors={errors.login}
                                    touched={touched.login}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="Password"
                                    name="password"
                                    errors={errors.password}
                                    touched={touched.password}
                                    type="password"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="Pesel"
                                    name="pesel"
                                    errors={errors.pesel}
                                    touched={touched.pesel}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="PhoneNumber"
                                    name="phoneNumber"
                                    errors={errors.phoneNumber}
                                    touched={touched.phoneNumber}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="ApartmentNumber"
                                    name="apartmentNumber"
                                    errors={errors.apartmentNumber}
                                    touched={touched.apartmentNumber}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="HouseNumber"
                                    name="houseNumber"
                                    errors={errors.houseNumber}
                                    touched={touched.houseNumber}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="PostCode"
                                    name="postCode"
                                    errors={errors.postCode}
                                    touched={touched.postCode}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="City"
                                    name="city"
                                    errors={errors.city}
                                    touched={touched.city}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="Street"
                                    name="street"
                                    errors={errors.street}
                                    touched={touched.street}
                                    type="text"
                                    isRequired={true}
                                />
                                <Tooltip label="We can sell your data" aria-label='A tooltip'>
                                    <Checkbox
                                        p='2' pl='1'
                                        onChange={(e) => setIsPrivacyAccepted(e.target.checked)}
                                    >
                                        Accept privacy policy
                                    </Checkbox>
                                </Tooltip>
                                <Flex pt='2'>
                                    <ButtonGroup colorScheme='teal'>
                                        <Button type='submit' disabled={!isPrivacyAccepted}>
                                            Register
                                        </Button>
                                        <Button onClick={() => navigate(-1)}>
                                            Back
                                        </Button>
                                    </ButtonGroup>
                                    <Spacer />
                                    <ChakraLink color='blue.500' as={Link} to='/login'>
                                        Already have an account?
                                    </ChakraLink>
                                </Flex>
                            </form>
                        )}
                    </Formik>
                </CardBody>
            </ContentCard>
        </>
    )
}