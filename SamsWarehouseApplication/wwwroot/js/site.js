// On load, adds an event listen to the theme switch button.
window.addEventListener("load", () => {
    document.getElementById("btnSwitchTheme").addEventListener("click", (e) => {
        switchTheme();
    });
})

// Custom fetch method with included anti forgery verification.
function advFetch(url, options) {
    let verifyToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    if (options == undefined) {
        options = {};
    }

    if (options.headers == undefined) {
        options.headers = {};
    }

    if (verifyToken != undefined) {
        options.headers['RequestVerificationToken'] = verifyToken;
    }

    options.headers['x-fetch-request'] = "";

    var promise = fetch(url, options)
    return promise;
}

// Swaps to the theme that isn't currently stored in a cookie.'
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
