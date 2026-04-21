const BazarrPlusConfig = {
    pluginUniqueId: '3e94f7ec-acca-42b4-ac3b-15a8cf446ba9'
};

export default function (view, params) {
    view.addEventListener('viewshow', function () {
        Dashboard.showLoadingMsg();
        const page = this;

        ApiClient.getPluginConfiguration(BazarrPlusConfig.pluginUniqueId).then(function (config) {
            page.querySelector('#BazarrUrl').value = config.BazarrUrl || '';
            page.querySelector('#BazarrToken').value = config.BazarrToken || '';
            Dashboard.hideLoadingMsg();
        }).catch(function () {
            Dashboard.hideLoadingMsg();
            Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
        });
    });

    view.querySelector('#BazarrPlusConfigForm').addEventListener('submit', function (e) {
        e.preventDefault();
        Dashboard.showLoadingMsg();
        const form = this;
        ApiClient.getPluginConfiguration(BazarrPlusConfig.pluginUniqueId).then(function (config) {
            const url = form.querySelector('#BazarrUrl').value.trim().replace(/\/+$/, '');
            const token = form.querySelector('#BazarrToken').value.trim();

            if (!url || !token) {
                Dashboard.hideLoadingMsg();
                Dashboard.processErrorResponse({ statusText: "Bazarr+ URL and API Token are required" });
                return;
            }

            config.BazarrUrl = url;
            config.BazarrToken = token;

            const el = form.querySelector('#bazarrplusresponse');
            ApiClient.updatePluginConfiguration(BazarrPlusConfig.pluginUniqueId, config).then(function (result) {
                if (el) {
                    el.innerText = 'Saved. Enable the provider in each Jellyfin library.';
                }
                Dashboard.processPluginConfigurationUpdateResult(result);
            }).catch(function () {
                Dashboard.hideLoadingMsg();
                Dashboard.processErrorResponse({ statusText: "Failed to save plugin configuration" });
            });
        }).catch(function () {
            Dashboard.hideLoadingMsg();
            Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
        });
        return false;
    });
}
