import { CardBody, CardHeader } from "@chakra-ui/card";
import { Button, ButtonGroup, Flex, Heading, Spacer, Link as ChakraLink } from "@chakra-ui/react";
import { Formik } from "formik";
import { Link, useNavigate } from "react-router-dom";
import MyInput from "../../app/common/form/MyInput";
import ContentCard from "../../app/common/shared/ContentCard";
import * as Yup from 'yup';
import { UserLoginValues } from "../../app/models/User";
import { useContext } from "react";
import { UserContext } from "../../app/common/providers/UserProvider";
import { UserAction, UserActionType } from "../../app/common/reducers/UserReducer";
import agent from "../../app/api/agent";
import { toast } from "react-toastify";


export default function LoginForm() {

    const initialValues: UserLoginValues = {
        login: "",
        password: "",
    }

    const { dispatch } = useContext(UserContext);

    const navigate = useNavigate();

    const handleLogin = (loginValues: UserLoginValues) => {
        agent.Account.login(loginValues).then(user => {
            if (!user) return;
            const action: UserAction = {
                type: UserActionType.SET_USER,
                payload: user
            };
            dispatch(action);
            navigate('/');
        }).catch(() => toast.error('Invalid login or password'));
    }

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Login
                    </Heading>
                </CardHeader>
                <CardBody>
                    <Formik
                        initialValues={initialValues}
                        onSubmit={handleLogin}
                        validationSchema={Yup.object({
                            login: Yup.string().required(),
                            password: Yup.string().required()
                        })}
                    >
                        {({ handleSubmit, errors, touched }) => (
                            <form onSubmit={handleSubmit}>
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
                                <Flex pt='2'>
                                    <ButtonGroup colorScheme='teal'>
                                        <Button type='submit'>
                                            Login
                                        </Button>
                                        <Button onClick={() => navigate(-1)}>
                                            Back
                                        </Button>
                                    </ButtonGroup>
                                    <Spacer />
                                    <ChakraLink color='blue.500' as={Link} to='/register'>
                                        Do not have an account yet?
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