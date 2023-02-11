import { CardBody, CardHeader } from "@chakra-ui/card";
import { Button, ButtonGroup, Flex, FormControl, FormErrorMessage, Heading, Select } from "@chakra-ui/react";
import { FieldProps, Formik, Field } from "formik";
import { useContext, useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import agent from "../../../app/api/agent";
import { UserContext } from "../../../app/common/providers/UserProvider";
import ContentCard from "../../../app/common/shared/ContentCard";
import { userCanCreateVehicle } from "../../../app/common/utils/helpers";
import LoadingSpinner from "../../../app/layout/LoadingSpinner";
import { VehicleCreateValues, VehicleFilters } from "../../../app/models/Vehicle";
import * as Yup from 'yup';
import MyInput from "../../../app/common/form/MyInput";
import MyNumberInput from "../../../app/common/form/MyNumberInput";
import { toast } from "react-toastify";


export default function VehicleCreate() {
    const { departmentId } = useParams<{ departmentId: string }>();
    const { state: user } = useContext(UserContext);
    const [filters, setFilters] = useState<VehicleFilters>();
    const [brandId, setBrandId] = useState<string>();
    const navigate = useNavigate();
    const initialValues: VehicleCreateValues = {
        departmentId: '',
        description: '',
        fuelId: '',
        modelId: '',
        price: 0,
        registration: '',
        seats: 5,
        vin: '',
        yearOfProduction: 1990
    };

    useEffect(() => {
        agent.Vehicle.getFilters().then(data => setFilters(data));
    }, []);

    const handleSubmit = (values: VehicleCreateValues) => {
        agent.Vehicle.create(values)
            .then(() => navigate(-1))
            .catch(() => toast.error('Invalid vehicle data'));
    }

    if (!userCanCreateVehicle(user, departmentId)) return <Navigate to='/' />

    if (!filters) return <LoadingSpinner />;

    return (
        <>
            <ContentCard>
                <CardHeader>
                    <Heading>
                        Create a vehicle
                    </Heading>
                </CardHeader>
                <CardBody overflow='auto'>
                    <Formik
                        initialValues={initialValues}
                        onSubmit={handleSubmit}
                        validationSchema={Yup.object({
                            description: Yup.string()
                                .required()
                                .max(255),
                            registration: Yup.string()
                                .required()
                                .max(255),
                            vin: Yup.string()
                                .required()
                                .max(255),
                            yearOfProduction: Yup.number()
                                .required()
                                .max(new Date().getFullYear())
                                .min(1990),
                            seats: Yup.number()
                                .required()
                                .max(10)
                                .min(1),
                            price: Yup.number()
                                .required()
                                .moreThan(0),
                            modelId: Yup.string()
                                .required(),
                            imageUrl: Yup.string()
                                .max(255),
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
                                    label="Registration"
                                    name="registration"
                                    errors={errors.registration}
                                    touched={touched.registration}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="Vin"
                                    name="vin"
                                    errors={errors.vin}
                                    touched={touched.vin}
                                    type="text"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="ImageUrl"
                                    name="imageUrl"
                                    errors={errors.imageUrl}
                                    touched={touched.imageUrl}
                                    type="text"
                                />
                                <MyInput
                                    label="YearOfProduction"
                                    name="yearOfProduction"
                                    errors={errors.yearOfProduction}
                                    touched={touched.yearOfProduction}
                                    type="number"
                                    isRequired={true}
                                />
                                <MyInput
                                    label="PricePerDay"
                                    name="price"
                                    errors={errors.price}
                                    touched={touched.price}
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
                                <Flex direction='column' pt='2' gap='2'>
                                    <FormControl isRequired isInvalid={!!errors.departmentId && !!touched.departmentId}>
                                        <Field name='departmentId'>
                                            {({ field }: FieldProps) => (
                                                <Select
                                                    placeholder='Department'
                                                    {...field}
                                                >
                                                    {filters.departments.filter(x => user?.departmentIds.includes(x.id)).map(d => (
                                                        <option key={d.id} value={d.id}>{d.name}</option>
                                                    ))}
                                                </Select>
                                            )}
                                        </Field>
                                        <FormErrorMessage>{errors.departmentId}</FormErrorMessage>
                                    </FormControl>
                                    <FormControl isRequired isInvalid={!!errors.fuelId && !!touched.fuelId}>
                                        <Field name='fuelId'>
                                            {({ field }: FieldProps) => (
                                                <Select
                                                    placeholder='Fuel'
                                                    {...field}
                                                >
                                                    {filters.fuels.map(f => (
                                                        <option key={f.id} value={f.id}>{f.type}</option>
                                                    ))}
                                                </Select>
                                            )}
                                        </Field>
                                        <FormErrorMessage>{errors.fuelId}</FormErrorMessage>
                                    </FormControl>
                                    <Select
                                        placeholder='Brand'
                                        name='brandId'
                                        id='brandId'
                                        onChange={(e) => setBrandId(e.target.value)}
                                    >
                                        {filters.brands.map(b => (
                                            <option key={b.id} value={b.id}>{b.name}</option>
                                        ))}
                                    </Select>
                                    <FormControl isRequired isInvalid={!!errors.modelId && !!touched.modelId}>
                                        <Field name='modelId'>
                                            {({ field }: FieldProps) => (
                                                <Select
                                                    placeholder='Model'
                                                    isDisabled={!brandId}
                                                    {...field}
                                                >
                                                    {brandId && <>
                                                        {filters.brands
                                                            .find(x => x.id === brandId)?.models.map(m => (
                                                                <option key={m.id} value={m.id} id='modelId'>{m.name}</option>
                                                            ))}
                                                    </>}
                                                </Select>
                                            )}
                                        </Field>
                                        <FormErrorMessage>{errors.modelId}</FormErrorMessage>
                                    </FormControl>
                                </Flex>
                                <Flex pt='2'>
                                    <ButtonGroup>
                                        <Button
                                            type='submit'
                                            colorScheme='blue'
                                        >
                                            Submit
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