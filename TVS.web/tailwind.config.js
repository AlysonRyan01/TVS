/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
      "./**/*.{html,js,razor}",
  ],
  theme: {
    colors: {
      "primary": "#FFFFFF",
    },
    fontFamily: {
      sans: ['Open Sans', 'sans-serif'],
      serif: ['Titillium Web', 'sans-serif']
    },
    extend: {},
  },
  plugins: [],
}

