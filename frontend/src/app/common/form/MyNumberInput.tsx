import { FormControl, FormErrorMessage, FormLabel, NumberDecrementStepper, NumberIncrementStepper, NumberInput, NumberInputField, NumberInputStepper } from "@chakra-ui/react";
import { Field, FieldProps } from "formik";




interface Props {
    name: string;
    label: string;
    isRequired: boolean;
    errors?: string,
    touched?: boolean,
}

export default function MyNumberInput({ name, label, isRequired, errors, touched }: Props) {


    return (
        <>
            <Field name={name}>
                {({ field, form }: FieldProps) => (
                    <FormControl isRequired={isRequired} isInvalid={!!errors && !!touched}>
                        <FormLabel>{label}</FormLabel>
                        <NumberInput
                            {...field}
                            onChange={(e) => form.setFieldValue(field.name, e)}
                        >
                            <NumberInputField />
                            <NumberInputStepper>
                                <NumberIncrementStepper />
                                <NumberDecrementStepper />
                            </NumberInputStepper>
                        </NumberInput>
                        <FormErrorMessage>{errors}</FormErrorMessage>
                    </FormControl>
                )}
            </Field>
        </>
    )
}