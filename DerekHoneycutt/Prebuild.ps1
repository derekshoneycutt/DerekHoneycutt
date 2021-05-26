
if (Test-Path -Path "./Prepare-Frontend.ps1") {
	echo "Current path"
	./Prepare-Frontend.ps1
}
elseif (Test-Path -Path "../Prepare-Frontend.ps1") {
	echo "Lower path"
	cd ".."
	./Prepare-Frontend.ps1
	cd "DerekHoneycutt"
}
elseif (Test-Path -Path "./DerekHoneycutt/Prepare-Frontend.ps1") {
	echo "Upper path"
	cd "DerekHoneycutt"
	./Prepare-Frontend.ps1
	cd ".."
}
