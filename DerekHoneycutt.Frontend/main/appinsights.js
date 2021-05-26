import { Imogene as $ } from '../Imogene/Imogene';
import { ApplicationInsights } from '@microsoft/applicationinsights-web'

$(() => {
    const appInsights = new ApplicationInsights({
        config: {
            instrumentationKey: '81cab510-5b57-4b66-8a5a-cfb81456352c'
            /* ...Other Configuration Options... */
        }
    });
    appInsights.loadAppInsights();
    appInsights.trackPageView(); // Manually call trackPageView to establish the current user/session/pageview
});
