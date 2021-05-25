
echo "Cleaning Frontend Build"
if (Test-Path -Path "./front_src/wwwroot/") {
	Remove-Item -Path "./front_src/wwwroot/" -Recurse
}
echo "Prior Builds cleaned"


echo "Preparing Build Directories"
New-Item -Path "./front_src/wwwroot" -ItemType Directory
New-Item -Path "./front_src/wwwroot/cmpcss" -ItemType Directory
New-Item -Path "./front_src/wwwroot/cmpcss/sawtooth" -ItemType Directory
Copy-Item -Path "./front_src/statics/*" -Destination "./front_src/wwwroot/" -Recurse
echo "Build Directories Prepared"

echo "Building Frontend"
cd "./front_src/"
echo "Building frontend formally..."
npm run wbp
if ($LastExitCode -ne 0) {
	echo "Fatal error occurred building with webpack"
	
	echo "Cleaning Build"
	cd ..
	Remove-Item -Path "./wwwroot/" -Recurse
	echo "Build is cleaned and finished"

	Exit 1
}
cd ..
echo "Frontend built"

echo "Cleaning prior builds"
if (Test-Path -Path "./DerekHoneycutt/wwwroot/*") {
	Remove-Item -Path "./DerekHoneycutt/wwwroot/*" -Recurse
}
echo "Prior builds cleaned"


echo "Copying directory to ASP.NET Core Project"
Copy-Item -Path "./front_src/wwwroot/*" -Destination "./DerekHoneycutt/wwwroot/" -Recurse
echo "Newly built directory copied"


echo "Cleaning Build"
Remove-Item -Path "./front_src/wwwroot/" -Recurse
echo "Build is cleaned and finished"
