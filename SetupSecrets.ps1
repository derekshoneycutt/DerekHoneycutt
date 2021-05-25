
Param (
    [Parameter(Mandatory=$true)][String] $CONNECTIONSTRING_AZURE,
    [Parameter(Mandatory=$true)][String] $APPINSIGHTS_INSTRUMENTKEY,
    [Parameter(Mandatory=$true)][String] $USERCONFIG_TITLE,
    [Parameter(Mandatory=$true)][String] $USERCONFIG_FIRSTNAME,
    [Parameter(Mandatory=$true)][String] $USERCONFIG_LASTNAME,
    [Parameter(Mandatory=$true)][String] $USERCONFIG_DESCRIPTION,
    [Parameter(Mandatory=$true)][String] $USERCONFIG_URL
)

$appSettings = cat "DerekHoneycutt\\appsettings.json" | ConvertFrom-Json
$appSettings.ConnectionStrings.Azure = "$CONNECTIONSTRING_AZURE"
$appSettings | ConvertTo-Json -depth 100 | Out-File "DerekHoneycutt\\appsettings.json"
cat "DerekHoneycutt\\appsettings.json"
$appInsights = "`r`nimport { Imogene as x } from '../Imogene/Imogene'; import { ApplicationInsights } from '@microsoft/applicationinsights-web'; x(() => { const i = new ApplicationInsights({ config: { instrumentationKey: '$APPINSIGHTS_INSTRUMENTKEY' } }); i.loadAppInsights(), i.trackPageView() });`r`n"
echo $appInsights > "front_src\\main\\appinsights.js"
cat "front_src\\main\\appinsights.js"
$wpuserconfig = "`r`nconst UserConfig = { title: '$USERCONFIG_TITLE', first_name: '$USERCONFIG_FIRSTNAME', last_name: '$USERCONFIG_LASTNAME', description: '$USERCONFIG_DESCRIPTION', url: '$USERCONFIG_URL' }; module.exports['UserConfig'] = UserConfig;`r`n"
echo $wpuserconfig > "front_src\\webpack.user_config.js"
cat "front_src\\webpack.user_config.js"