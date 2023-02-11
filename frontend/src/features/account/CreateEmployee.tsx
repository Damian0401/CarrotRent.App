import { CardBody, CardHeader } from '@chakra-ui/card';
import { Button, ButtonGroup, Flex, Heading } from '@chakra-ui/react';
import { Formik } from 'formik';
import * as React from 'react';
import { Navigate, useNavigate, useParams } from 'react-router-dom';
import MyInput from '../../app/common/form/MyInput';
import ContentCard from '../../app/common/shared/ContentCard';
import { CreateEmployeeValues } from '../../app/models/User';
import * as Yup from 'yup';
import agent from '../../app/api/agent';
import { toast } from 'react-toastify';


function CreateEmployee() {
    const initialValues: CreateEmployeeValues = {
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

    const navigate = useNavigate();
    const { departmentId } = useParams<{ departmentId: string }>();

    if (!departmentId) return <Navigate to='NotFound' />

    const handleCreate = (registerValues: CreateEmployeeValues) => {
        agent.Account.createEmployee(registerValues, departmentId).then(() => {
            toast.success('Employee created');
            navigate(-1);
        }).catch(() => toast.error('Invalid employee data'));
    }


    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Create employee
                    </Heading>
                </CardHeader>
                <CardBody overflow='auto' p='2'>
                    <Formik
                        initialValues={initialValues}
                        onSubmit={handleCreate}
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
                                <Flex pt='2'>
                                    <ButtonGroup colorScheme='teal'>
                                        <Button type='submit'>
                                            Create
                                        </Button>
                                        <Button onClick={() => navigate(-1)}>
                                            Back
                                        </Button>
                                    </ButtonGroup>
                                </Flex>
                            </form>
                        )}
                    </Formik>
                </CardBody>
            </ContentCard>
        </>
    );
}

export default CreateEmployee;