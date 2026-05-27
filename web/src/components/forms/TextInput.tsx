import React from "react";

type TextInputProps = {
    label: string;
    value: string;
    onChange: (value: string) => void;
    placeholder?: string;
};

export const TextInput: React.FC<TextInputProps> = ({
    label,
    value,
    onChange,
    placeholder,
}) => {
    return (
        <label className="block">
            <div className="mb-1.5 text-sm font-medium text-stone-700">{label}</div>
            <input
                className="w-full rounded-xl border border-stone-200 bg-amber-50 px-3 py-2 text-stone-800 shadow-sm outline-none transition placeholder:text-stone-400 focus:border-amber-300 focus:bg-white"
                type="text"
                value={value}
                placeholder={placeholder}
                onChange={(e) => onChange(e.target.value)}
            />
        </label>
    );
};

