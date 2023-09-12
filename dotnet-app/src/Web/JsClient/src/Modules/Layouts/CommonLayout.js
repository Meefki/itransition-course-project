import { Outlet } from 'react-router-dom';
import ThemeLayout from './ThemeLayout';
import Header from '../Shared/Header'
import { FilterOptionsContext } from '../../Contexts/FilterOptionsContext';
import { SortOptionsContext } from '../../Contexts/SortOptionsContext';
import { useEffect, useState } from 'react';

const CommonLayout = () => {
    const [filters, setFilters] = useState([]);
    const [sort, setSort] = useState([]);
    const [valid, setValid] = useState(true);

    useEffect(() => {
        // setFilters([]);
        // setValid(true);
    }, []);

    return (
        <div className='h-100'>
            <FilterOptionsContext.Provider value={{ filterOptions: filters, setFilterOptions: setFilters, valid: valid, setValid: setValid }}>
            <SortOptionsContext.Provider value={{ sortOptions: sort, setSortOptions: setSort }}>
                <Header />
                <ThemeLayout>
                    <Outlet />
                </ThemeLayout>
            </SortOptionsContext.Provider>
            </FilterOptionsContext.Provider>
        </div>
    );
}

export default CommonLayout;