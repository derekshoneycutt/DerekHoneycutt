
Param(
	[String] $AppInsightsKey
)

$useKey = '$AppInsightsKey'
echo $useKey
printf "`r`nimport `{ Imogene as `$ `} from '../Imogene/Imogene'; import { ApplicationInsights } from '@microsoft/applicationinsights-web'; `$`(`(`) => `{ const i = new ApplicationInsights`(`{ config: `{ instrumentationKey: '$useKey' `} `}); i.loadAppInsights(), i.trackPageView() `});`r`n" | Out-File "main/appinsights.js"
cat "main\\appinsights.js"


