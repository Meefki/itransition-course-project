import React, { useState } from "react";
import { Dropdown } from "antd";

function DropdownInput({ items = [], getItems = () => {}, addItem = () => {}, inputPlaceholder = "Enter an item name" }) {

    const [searchedText, setSearchedText] = useState('');

    function menuItemClick(key) {
        const keyText = key.target.textContent;
        setSearchedText('');
        getItems('');
        addItem(keyText);
    }

    const menuItems = () => {
        let nemuItems = [...items];
        if (searchedText && !items.find(item => item?.name === searchedText)) nemuItems.unshift({ name: searchedText})
        const menuItems = nemuItems.map((item, index) => {
            return { key: index, label: (
            <span className="d-flex" key={item?.name} onClick={menuItemClick} >{item?.name}</span>
        )}});

        return menuItems;
    }

    return (
        <Dropdown className="w-100" menu={{ items: menuItems() }} trigger={['click']}>
            <input
                size="large"
                value={searchedText}
                className="input-group-text"
                placeholder={inputPlaceholder}
                onChange={(e) => {
                    setSearchedText(e.target.value);
                    getItems(e.target.value);
                }}
            />
        </Dropdown>
    );
}

export default DropdownInput;