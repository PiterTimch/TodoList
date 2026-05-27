import React from "react";

type FormErrorMessageProps = {
    message?: string;
};

export const FormErrorMessage: React.FC<FormErrorMessageProps> = ({ message }) => {
    if (!message) return null;

    return <p className="text-sm text-red-600">{message}</p>;
};
