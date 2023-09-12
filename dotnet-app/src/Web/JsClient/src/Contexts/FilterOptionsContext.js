import { createContext } from "react";

export const FilterOptionsContext = createContext({
    filterOptions: {},
    setFilterOptions: () => {},
    valid: {},
    setValid: () => {}
});