import { Imogene as $ } from '../Imogene/Imogene';
import { ApplicationInsights } from '@microsoft/applicationinsights-web'

$(() => {
    const appInsights = new ApplicationInsights({
        config: {
            instrumentationKey: '${{ secrets.APPINSIGHTS_INSTRUMENTKEY }}'
            /* ...Other Configuration Options... */
        }
    });
    appInsights.loadAppInsights();
    appInsights.trackPageView(); // Manually call trackPageView to establish the current user/session/pageview
});
