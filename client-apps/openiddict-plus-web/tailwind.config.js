const flowbite = require("flowbite-react/tailwind");

/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./index.html", "./src/**/*.{ts,tsx,js,jsx}",  flowbite.content()],
	theme: {
		extend: {},
	},
    plugins: [ flowbite.plugin(), require("tailwindcss-animate")],
}
