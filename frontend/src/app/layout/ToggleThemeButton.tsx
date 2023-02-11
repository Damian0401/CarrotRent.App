import { MoonIcon, SunIcon } from "@chakra-ui/icons";
import { IconButton, useColorMode } from "@chakra-ui/react";


export default function ToggleThemeButton() {
    const { colorMode, toggleColorMode } = useColorMode()

    return (
        <IconButton  
            borderRadius='0 1rem 0 0'
            variant='ghost'
            position='fixed'
            bottom='0px'
            left='0px'
            aria-label='toggle theme'
            icon={colorMode === 'light' ? <MoonIcon /> : <SunIcon />} 
            onClick={toggleColorMode}
            zIndex={1}
        />
    )
}