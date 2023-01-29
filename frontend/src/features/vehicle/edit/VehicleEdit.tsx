import { CardBody, CardHeader } from "@chakra-ui/card";
import { Button, ButtonGroup, Flex, Heading } from "@chakra-ui/react";
import { Formik } from "formik";
import { useContext, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import agent from "../../../app/api/agent";
import ContentCard from "../../../app/common/shared/ContentCard";
import LoadingSpinner from "../../../app/layout/LoadingSpinner";
import { VehicleDetails, VehicleEditValues } from "../../../app/models/Vehicle";
import * as Yup from 'yup';
import MyInput from "../../../app/common/form/MyInput";
import { UserContext } from "../../../app/common/providers/UserProvider";
import { userCanEditVehicle } from "../../../app/common/utils/helpers";
import MyNumberInput from "../../../app/common/form/MyNumberInput";



export default function VehicleEdit() {
    const { id } = useParams<{ id: string }>();
    const [vehicle, setVehicle] = useState<VehicleDetails>();
    const { state: user } = useContext(UserContext);
    const navigate = useNavigate();

    useEffect(() => {
        agent.Vehicle.getById(id!)
            .then(data => setVehicle(data));
        }, [id]);
        
    useEffect(() => {
        if (vehicle && !userCanEditVehicle(user, vehicle)) navigate('/');
    }, [vehicle, user, navigate])

    const handleSave = (vehicle: VehicleDetails) => {
        if (!id) return;

        
        const editValues: VehicleEditValues = {
            description: vehicle.description,
            price: vehicle.price,
            seats: vehicle.seats,
            yearOfProduction: vehicle.yearOfProduction,
            imageUrl: vehicle.imageUrl
        };
        agent.Vehicle.update(id, editValues).then(() => navigate(-1));
    }

    if (!vehicle) return <LoadingSpinner />

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Edit {vehicle.model}
                    </Heading>
                    {vehicle.brand}
                </CardHeader>
                <CardBody>
                    <Formik
                        initialValues={vehicle}
                        onSubmit={handleSave}
                        validationSchema={Yup.object({
                            description: Yup.string()
                                .required()
                                .max(255),
                            yearOfProduction: Yup.number()
                                .required(),
                            seats: Yup.number()
                                .required()
                                .max(10)
                                .min(1),
                            price: Yup.number()
                                .required(),
                            imageUrl: Yup.string(),
                        })}
                    >
                        {({ handleSubmit, errors, touched }) => (
                            <form onSubmit={handleSubmit}>
                                <MyInput
                                    label="Description"
                                    name="description"
                                    errors={errors.description}
                                    touched={touched.description}
                                    type="text"
                                    isRequired={true}
                                    />
                                <MyInput
                                    label="YearOfProduction"
                                    name="yearOfProduction"
                                    errors={errors.yearOfProduction}
                                    touched={touched.yearOfProduction}
                                    type="number"
                                    isRequired={true}
                                    />
                                <MyNumberInput 
                                    isRequired={true}
                                    label='Seats'
                                    name='seats'
                                    errors={errors.seats}
                                    touched={touched.seats}
                                />
                                <MyInput
                                    label="PricePerDay"
                                    name="price"
                                    errors={errors.price}
                                    touched={touched.price}
                                    type="number"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="ImageUrl"
                                    name="imageUrl"
                                    errors={errors.imageUrl}
                                    touched={touched.imageUrl}
                                    type="text"
                                />
                                <Flex pt='2'>
                                    <ButtonGroup>
                                        <Button
                                            type='submit'
                                            colorScheme='blue'
                                        >
                                            Save
                                        </Button>
                                        <Button
                                            onClick={() => navigate(-1)}
                                            colorScheme='teal'
                                        >
                                            Cancel
                                        </Button>
                                    </ButtonGroup>
                                </Flex>
                            </form>
                        )}
                    </Formik>
                </CardBody>
            </ContentCard>
        </>
    )
}