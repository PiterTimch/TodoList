export const getApiErrorMessage = (error: unknown): string | undefined => {
    if (!error || typeof error !== "object" || !("status" in error)) {
        return undefined;
    }

    if (error.status !== 400) {
        return undefined;
    }

    const data = "data" in error ? error.data : undefined;
    if (data && typeof data === "object" && "message" in data) {
        const message = data.message;
        if (typeof message === "string" && message.trim()) {
            return message;
        }
    }

    return "Bad request.";
};
