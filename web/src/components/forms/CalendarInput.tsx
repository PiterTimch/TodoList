import React from "react";

type CalendarInputProps = {
    label: string;
    value?: string;
    onChange: (value?: string) => void;
};

export const CalendarInput: React.FC<CalendarInputProps> = ({
    label,
    value,
    onChange,
}) => {
    return (
        <label className="block">
            <div className="mb-1.5 text-sm font-medium text-stone-700">{label}</div>
            <input
                className="w-full rounded-xl border border-stone-200 bg-amber-50 px-3 py-2 text-stone-800 shadow-sm outline-none transition focus:border-amber-300 focus:bg-white"
                type="date"
                value={value ?? ""}
                onChange={(e) => {
                    const next = e.target.value;
                    onChange(next ? next : undefined);
                }}
            />
        </label>
    );
};

