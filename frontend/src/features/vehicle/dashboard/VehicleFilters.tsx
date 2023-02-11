import { SearchIcon } from "@chakra-ui/icons";
import {
    Flex, IconButton, NumberDecrementStepper,
    NumberIncrementStepper, NumberInput,
    NumberInputField, NumberInputStepper,
    RangeSlider, RangeSliderFilledTrack,
    RangeSliderThumb, RangeSliderTrack,
    Select, Text
} from "@chakra-ui/react";
import { ChangeEvent, useEffect, useState } from "react";
import agent from "../../../app/api/agent";
import LoadingSpinner from "../../../app/layout/LoadingSpinner";
import { SelectedFilters, VehicleFilters as Filters } from "../../../app/models/Vehicle";

interface Props {
    handleSearch: (selectedFilters: SelectedFilters) => void
}

export default function VehicleFilters({ handleSearch }: Props) {

    const [filters, setFilters] = useState<Filters>();
    const [selectedFilters, setSelectedFilters] = useState<SelectedFilters>(
        {
            minPrice: 0,
            maxPrice: 300,
            seats: 5
        }
    )

    useEffect(() => {
        agent.Vehicle.getFilters().then(data => setFilters(data));
    }, []);

    function handleBrandChange(e: ChangeEvent<HTMLSelectElement>): void {
        if (!e.target.value)
            setSelectedFilters({ ...selectedFilters, brandId: undefined, modelId: undefined });
        else
            setSelectedFilters({ ...selectedFilters, brandId: e.target.value });
    }

    function handleSeatsChange(e: number): void {
        setSelectedFilters({ ...selectedFilters, seats: e });
    }

    function handlePriceChange(e: number[]): void {
        setSelectedFilters({ ...selectedFilters, minPrice: e[0], maxPrice: e[1] });
    }

    function handleSelectChange(e: ChangeEvent<HTMLSelectElement>) {
        setSelectedFilters({ ...selectedFilters, [e.target.name]: e.target.value });
    }

    if (!filters) return <LoadingSpinner />;

    return (
        <>
            <Flex p='1' gap='2' flexWrap='wrap'>
                <Select
                    placeholder='Department'
                    onChange={handleSelectChange}
                    minWidth='10rem'
                    maxWidth='15%'
                    name='departmentId'
                >
                    {filters.departments.map(d => (
                        <option key={d.id} value={d.id}>{d.name}</option>
                    ))}
                </Select>
                <Flex direction='row' gap='1' width='min-content'>
                    <Text display='flex' alignItems='center'>
                        Seats:
                    </Text>
                    <NumberInput
                        defaultValue={5}
                        min={1} max={9}
                        minWidth='4rem'
                        maxWidth='5%'
                        onChange={(_, n) => handleSeatsChange(n)}
                    >
                        <NumberInputField />
                        <NumberInputStepper>
                            <NumberIncrementStepper />
                            <NumberDecrementStepper />
                        </NumberInputStepper>
                    </NumberInput>
                </Flex>
                <Select
                    placeholder='Fuel'
                    onChange={handleSelectChange}
                    minWidth='6rem'
                    maxWidth='10%'
                    name='fuelId'
                >
                    {filters.fuels.map(f => (
                        <option key={f.id} value={f.id}>{f.type}</option>
                    ))}
                </Select>
                <Select
                    minWidth='6rem'
                    maxWidth='10%'
                    placeholder='Brand'
                    name='brandId'
                    onChange={handleBrandChange}
                >
                    {filters.brands.map(b => (
                        <option key={b.id} value={b.id}>{b.name}</option>
                    ))}
                </Select>
                <Select
                    placeholder='Model'
                    isDisabled={!selectedFilters.brandId}
                    onChange={handleSelectChange}
                    minWidth='6rem'
                    maxWidth='15%'
                    name='modelId'
                >
                    {selectedFilters.brandId && <>
                        {filters.brands
                            .find(x => x.id === selectedFilters.brandId)?.models.map(m => (
                                <option key={m.id} value={m.id}>{m.name}</option>
                            ))}
                    </>}
                </Select>
                <IconButton
                    colorScheme='teal'
                    aria-label='Search database'
                    icon={<SearchIcon />}
                    onClick={() => handleSearch(selectedFilters)}
                />
            </Flex>
            <Text>
                <b>Price:</b> {selectedFilters.minPrice} - {selectedFilters.maxPrice}
            </Text>
            <RangeSlider
                aria-label={['min', 'max']}
                colorScheme='pink'
                defaultValue={[0, 300]}
                min={0} max={300} step={10}
                onChangeEnd={handlePriceChange}
            >
                <RangeSliderTrack>
                    <RangeSliderFilledTrack />
                </RangeSliderTrack>
                <RangeSliderThumb index={0} />
                <RangeSliderThumb index={1} />
            </RangeSlider>
        </>
    )
}