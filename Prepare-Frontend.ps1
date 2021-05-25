
echo "Building Frontend"
cd "./front_src/"
npm run wbp
if ($LastExitCode -ne 0) {
	echo "Fatal error occurred building with webpack"
	Exit 1
}
cd ..
echo "Frontend built"


echo "Cleaning prior builds"
Remove-Item -Path "./DerekHoneycutt/wwwroot/*" -Recurse
if ($LastExitCode -ne 0) {
	echo "Fatal error occurred removing existing files"
	Exit 1
}
echo "Prior builds cleaned"


echo "Copying directory to ASP.NET Core Project"
Copy-Item -Path "./front_src/wwwroot/*" -Destination "./DerekHoneycutt/wwwroot/" -Recurse
if ($LastExitCode -ne 0) {
	echo "Fatal error occurred copying files"
	Exit 1
}
echo "Newly built directory copied"
