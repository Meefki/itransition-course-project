import { createContext } from "react";

export const FilterOptionsContext = createContext({
    filterOptions: {},
    setFilterOptions: () => {}
});