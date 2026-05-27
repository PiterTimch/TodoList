import React from "react";

type TextAreaProps = {
    label: string;
    value: string;
    onChange: (value: string) => void;
    placeholder?: string;
};

export const TextArea: React.FC<TextAreaProps> = ({
    label,
    value,
    onChange,
    placeholder,
}) => {
    return (
        <label className="block">
            <div className="mb-1.5 text-sm font-medium text-stone-700">{label}</div>
            <textarea
                className="min-h-28 w-full resize-y rounded-xl border border-stone-200 bg-amber-50 px-3 py-2 text-stone-800 shadow-sm outline-none transition placeholder:text-stone-400 focus:border-amber-300 focus:bg-white"
                value={value}
                placeholder={placeholder}
                onChange={(e) => onChange(e.target.value)}
            />
        </label>
    );
};
