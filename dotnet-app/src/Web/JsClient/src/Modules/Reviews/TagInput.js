import React, { useState } from "react";
import { Dropdown } from "antd";

function TagInput({ tags = [], getTags = () => {}, addTag = () => {}, inputPlaceholder = "Enter a tagname" }) {

    const [searchedText, setSearchedText] = useState('');

    function menuItemClick(key) {
        const keyText = key.target.textContent;
        setSearchedText('');
        getTags('');
        addTag(keyText);
    }

    const menuItems = () => {
        let tagItems = [...tags];
        if (searchedText && !tags.find(tag => tag === searchedText)) tagItems.unshift(searchedText)
        const menuItems = tagItems.map((tag) => {
            return { key: tag, label: (
            <span className="d-flex" key={tag} onClick={menuItemClick} >{tag}</span>
        )}});

        return menuItems;
    }

    return (
        <Dropdown menu={{ items: menuItems() }} trigger={['click']}>
            <input
                size="large"
                value={searchedText}
                className="m-1 input-group-text"
                placeholder={inputPlaceholder}
                onChange={(e) => {
                    setSearchedText(e.target.value);
                    getTags(e.target.value);
                }}
            />
        </Dropdown>
    );
}

export default TagInput;