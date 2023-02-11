import { FormControl, FormErrorMessage, FormLabel, Input } from "@chakra-ui/react";
import { Field } from "formik";


interface Props {
    errors?: string,
    touched?: boolean,
    name: string;
    type: string;
    label: string;
    isRequired?: boolean;
}

export default function MyInput({ errors, touched, name, type, label, isRequired = false }: Props) {


    return (
        <FormControl isInvalid={!!errors && !!touched} isRequired={isRequired}>
            <FormLabel htmlFor={name}>{label}</FormLabel>
            <Field
                as={Input}
                id={name}
                name={name}
                type={type}
            />
            <FormErrorMessage>{errors}</FormErrorMessage>
        </FormControl>
    )
}