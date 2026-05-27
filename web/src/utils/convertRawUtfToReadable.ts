const decodeUnicodeEscapes = (input: string): string => {
    // Handles strings like: "\u041f\u0440\u0438\u0432\u0435\u0442"
    return input.replace(/\\u([0-9a-fA-F]{4})/g, (_, hex) => {
        const code = Number.parseInt(hex, 16);
        return Number.isFinite(code) ? String.fromCharCode(code) : _;
    });
};

const decodeLatin1ToUtf8 = (input: string): string => {
    // Treat each JS "character" as a raw byte (latin1) and decode as UTF-8.
    // This fixes common mojibake like "ÐŸÑ€Ð¸Ð²ÐµÑ‚" that appears when UTF-8
    // bytes were incorrectly interpreted as latin1/Windows-1252.
    const bytes = new Uint8Array(Array.from(input, (ch) => ch.charCodeAt(0) & 0xff));
    return new TextDecoder("utf-8").decode(bytes);
};

export const convertRawUtfToReadable = (value: string): string => {
    if (!value) return value;

    // 1) Convert explicit "\uXXXX" escapes if present.
    const escaped = decodeUnicodeEscapes(value);

    // 2) Try to fix typical mojibake by latin1 -> utf8 decode.
    try {
        const decoded = decodeLatin1ToUtf8(escaped);

        // If decode produced lots of replacement chars, keep the original.
        const replacementCount = (decoded.match(/�/g) ?? []).length;
        if (replacementCount < 2 && decoded.trim().length > 0 && decoded !== escaped) {
            return decoded;
        }
    } catch {
        // ignore decoding errors
    }

    return escaped;
};

