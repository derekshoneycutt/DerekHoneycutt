
$useDir = './DerekHoneycutt.Frontend'

echo "Cleaning Frontend Build"
if (Test-Path -Path "$useDir/wwwroot/") {
	Remove-Item -Path "$useDir/wwwroot/" -Recurse
}
echo "Prior Builds cleaned"


echo "Preparing Build Directories"
New-Item -Path "$useDir/wwwroot" -ItemType Directory
New-Item -Path "$useDir/wwwroot/cmpcss" -ItemType Directory
New-Item -Path "$useDir/wwwroot/cmpcss/sawtooth" -ItemType Directory
Copy-Item -Path "$useDir/statics/*" -Destination "$useDir/wwwroot/" -Recurse
echo "Build Directories Prepared"

echo "Building Frontend"
cd "$useDir/"
echo "Building frontend formally..."
npm run wbp
if ($LastExitCode -ne 0) {
	echo "Fatal error occurred building with webpack"
	Exit 1
}
cd ..
echo "Frontend built"
