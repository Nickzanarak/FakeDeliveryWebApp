/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#f4a500',
        dark: '#1a1a1a',
        card: '#3a3a3a',
      }
    },
  },
  plugins: [],
}