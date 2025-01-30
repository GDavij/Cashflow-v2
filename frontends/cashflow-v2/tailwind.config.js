/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    colors: {
        primary: "#3b82f6",
        "primary-hover": "#2563eb",
        "primary-active": "#1d4ed8",

        "required": "#ff5555",

        error: "#ef4444",
        "error-hover": "#dc2626",
        "error-active": "#b91c1c",


        success: '#00c951',
        "success-hover": '#00a63e',
        "success-active": '#008236',

        "white-0": "#fff",
        "white-50": "#eee",
        "white-100": "#ddd",
        "white-200": "#ccc",
        "white-300": "#bbc",
        "white-400": "#aab",
        "white-500": "#99a",
        "white-600": "#889",
        "white-700": "#778",
        "white-800": "#666",

        "black-0": "#000",
    },
  },
  plugins: [require("tailwindcss-motion")],
};

/*



*/
