import { Button } from "@chakra-ui/react";
import { Marker, Popup } from "react-leaflet";
import { Link } from "react-router-dom";
import { DepartmentMarker } from "../../app/models/Department";

interface Props {
    department: DepartmentMarker
}

export default function MapMaker({ department }: Props) {

    return (
        <>
            <Marker position={[department.xPosition, department.yPosition]} key={`${department.xPosition} ${department.yPosition}`}>
                <Popup>
                    <b>{department.name}</b> <br/>
                    {department.address} <br/>
                    <Button as={Link} className='marker-button' colorScheme='teal' to={`/departments/${department.id}`}>
                        View
                    </Button>
                </Popup>
            </Marker>
        </>
    )

}