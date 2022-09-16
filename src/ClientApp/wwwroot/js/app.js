﻿window.isDarkMode = () => {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        return true;
    }
    return false;
};

window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', async event => {
    await DotNet.invokeMethodAsync("ClientApp", "OnDarkModeChanged", event.matches);
});

function splashscreen() {
    let preferredColorScheme = JSON.parse(window.localStorage["preferredColorScheme"] ?? "null");
    let colorScheme = preferredColorScheme ?? (window.isDarkMode() ? 1 : 0);

    if (colorScheme == 1) {
        const elem = document.getElementById("splashscreen");
        elem.style.backgroundColor = "#27272fff";
    }
}

splashscreen();