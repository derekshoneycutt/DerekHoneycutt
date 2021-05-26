
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
	Exit 1
}
cd ..
echo "Frontend built"
