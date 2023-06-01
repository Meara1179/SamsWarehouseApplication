// On load, adds an event listen to the theme switch button.
window.addEventListener("load", () => {
    document.getElementById("btnSwitchTheme").addEventListener("click", (e) => {
        switchTheme();
    });
})

function switchTheme() {
    let currentTheme = localStorage.getItem("theme");

    if (currentTheme && currentTheme == "dark") {
        localStorage.setItem("theme", "light");
        document.cookie = "theme=lght;path=/";
        document.getElementById("themeStyle").setAttribute("href", "/css/LightTheme.css")
    }
    else {
        localStorage.setItem("theme", "dark");
        document.cookie = "theme=dark;path=/";
        document.getElementById("themeStyle").setAttribute("href", "/css/DarkTheme.css")
    }
}
