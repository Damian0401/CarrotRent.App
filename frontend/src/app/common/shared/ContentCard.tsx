import { Card } from "@chakra-ui/card";
import { ArrowBackIcon } from "@chakra-ui/icons";
import { Center, IconButton, useColorMode, useMediaQuery } from "@chakra-ui/react";
import { PropsWithChildren } from "react";
import { useNavigate } from "react-router-dom";


const ContentCard = ({ children }: PropsWithChildren) => {
    const { colorMode } = useColorMode();
    const navigate = useNavigate();
    const [isSmaller] = useMediaQuery('(max-width: 768px)');

    return (
        <Center>
            <Card

                variant='main'
                width= {isSmaller ? "100vw" : "35vw"}
                maxHeight='89vh'
                position='relative'
                p='5' m='3'
                borderRadius='1rem'
                bgColor={colorMode === 'light' ? 'whiteAlpha.500' : 'blackAlpha.500'}
            >
                <IconButton
                    aria-label="back-button"
                    size='xs' position='absolute'
                    top='0' right='0'
                    icon={<ArrowBackIcon />}
                    onClick={() => navigate(-1)}
                    borderRadius='0'
                    borderTopRightRadius='1rem'
                    borderBottomLeftRadius='0.5rem'
                />
                {children}
            </Card>
        </Center>
    )
}

export default ContentCard;